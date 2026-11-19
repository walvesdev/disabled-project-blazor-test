#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Service.Commands.Customers;
using Sindvamb.Service.Commands.FileControls;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Inputs;
using Sindvamb.Service.Net;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Service.Handlers.FileControls;

public class FileControlCommandHandler :
    IRequestHandler<GenericCommand<List<FileControlDTO>>, List<FileControlDTO>>,
    IRequestHandler<GenericCommand<Guid, FileControlDTO>, FileControlDTO>,
    IRequestHandler<GenericCommand<CreateInput<FileControlDTO>, bool>, bool>,
    IRequestHandler<GenericCommand<UpdateInput<FileControlDTO>, bool>, bool>,
    IRequestHandler<GenericCommand<DeleteInput<Guid, FileControl>, bool>, bool>,
    IRequestHandler<GetUserFilesComand, List<FileControlDTO>>,
    IRequestHandler<ApprovedChangeCommand, bool>
{
    private readonly IConfiguration _configuration;
    protected AppSettingsSection AppSettings { get; set; }
    private ISmtpClientHelper Email { get; set; }
    public IMediatorHandler? MediatorHandler { get; set; }

    public IOptions<AppSettingsSection> Options { get; }
    public ISindvambRepository<FileControl, Guid> Repository { get; set; }
    public IMapper Mapper { get; set; }
    public ISindvambUnitOfWork UnitOfWork { get; set; }


    public FileControlCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<FileControl, Guid> repository,
        ISmtpClientHelper email,
        ISindvambUnitOfWork unitOfWork)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        AppSettings = options.Value;
        Email = email;
        UnitOfWork = unitOfWork;
    }


    public async Task<List<FileControlDTO>> Handle(GenericCommand<List<FileControlDTO>> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var list = await Repository
                .GetAll()
                .Select(f => new FileControlDTO
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    TargetPath = f.TargetPath,
                    ContentType = f.ContentType,
                    Description = f.Description,
                    Approved = f.Approved,
                    DependentId = f.DependentId,
                    UserId = f.UserId,
                    LayoutId = f.LayoutId,

                })
                .ToListAsync(cancellationToken: cancellationToken);

            return list.MapTo<List<FileControlDTO>>();
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateFileControlRequest",
                "Erro ao obter arquivos. Se o erro persistir Contate o Suporte do sistema!"));

            return default;
        }
    }

    public async Task<FileControlDTO> Handle(GenericCommand<Guid, FileControlDTO> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = await Repository
                .GetAll()
                .FirstOrDefaultAsync(c => c.Id.Equals(request.Input), cancellationToken: cancellationToken)!;

            return model.MapTo<FileControlDTO>();
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateFileControlRequest",
                "Erro ao obter arquivo. Se o erro persistir Contate o Suporte do sistema!"));

            return default;
        }
    }

    public async Task<bool> Handle(GenericCommand<CreateInput<FileControlDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            await Repository.AddAsync(request.Input.Model.MapTo<FileControl>(), true,
                cancellationToken: cancellationToken);

            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("CreateFileControl",
                "Erro ao criar arquivo. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<FileControlDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = request.Input!.Model.MapTo<FileControl>();            

            if (model.Id != Guid.Empty)
            {
                var file = await Repository.FirstOrDefaultAsync(f => f.Id.Equals(model.Id));

                if (file.IsNotNull())
                {
                    file.FileName = model.FileName;
                    file.FileSize = model.FileSize;
                    file.FileArray = model.FileArray;
                    file.TargetPath = model.TargetPath;
                    file.ContentType = model.ContentType;
                    file.Description = model.Description;
                    file.Approved = model.Approved;
                    file.DependentId = model.DependentId;
                    file.UserId = model.UserId;
                    file.LayoutId = model.LayoutId;

                    await Repository.UpdateAsync(file, true, cancellationToken);

                    return true;
                }
            }

            var filename = model.FileName.Split(".")[0] ?? "";

            var fileRepo = await Repository.FirstOrDefaultAsync(f => f.FileName.Contains(filename));

            if (fileRepo.IsNotNull())
            {
                fileRepo.FileName = model.FileName;
                fileRepo.FileSize = model.FileSize;
                fileRepo.FileArray = model.FileArray;
                fileRepo.TargetPath = model.TargetPath;
                fileRepo.ContentType = model.ContentType;
                fileRepo.Description = model.Description;
                fileRepo.Approved = model.Approved;
                fileRepo.DependentId = model.DependentId;
                fileRepo.UserId = model.UserId;
                fileRepo.LayoutId = model.LayoutId;

                await Repository.UpdateAsync(fileRepo, true, cancellationToken);

                return true;
            }


            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }

            await Repository.AddAsync(model, true, cancellationToken);
            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateFileControl",
                "Erro ao atulizar arquivo. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, FileControl>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = Repository.Find(request.Input.Id);

            if (model.IsNull())
            {
                await MediatorHandler!.PublishNotification(new DomainNotification("DeleteFileControl",
                    $"arquivo não encontrado!"));
                return false;
            }

            await Repository.DeleteAsync(model, true, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("DeleteFileControl",
                "Erro ao atulizar arquivo. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<List<FileControlDTO>> Handle(GetUserFilesComand request, CancellationToken cancellationToken)
    {
        try
        {
            var list = new List<FileControlDTO>();

            if (request.IsDependent)
            {
                list = await Repository
                    .GetAll(file => file.DependentId == request.UserId)
                    .Select(f => new FileControlDTO
                    {
                        Id = f.Id,
                        FileName = f.FileName,
                        TargetPath = f.TargetPath,
                        ContentType = f.ContentType,
                        Description = f.Description,
                        Approved = f.Approved,
                        DependentId = f.DependentId,
                        UserId = f.UserId,
                        LayoutId = f.LayoutId,

                    })
                    .ToListAsync(cancellationToken: cancellationToken);
            }
            else
            {
                list = await Repository
                   .GetAll(file => file.UserId == request.UserId)
                        .Select(f => new FileControlDTO
                        {
                            Id = f.Id,
                            FileName = f.FileName,
                            TargetPath = f.TargetPath,
                            ContentType = f.ContentType,
                            Description = f.Description,
                            Approved = f.Approved,
                            DependentId = f.DependentId,
                            UserId = f.UserId,
                            LayoutId = f.LayoutId,

                        })
                   .ToListAsync(cancellationToken: cancellationToken);
            }

            return list;
        }
        catch (Exception e)
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateFileControlRequest",
                "Erro ao obter arquivos. Se o erro persistir Contate o Suporte do sistema!"));

            return default;
        }
    }

    public async Task<bool> Handle(ApprovedChangeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await UnitOfWork.Context.FileControl
                .FirstOrDefaultAsync(f => f.Id == request.FileControl.Id, cancellationToken: cancellationToken);

            if (entity is not null)
            {
                entity.Approved = request.FileControl.Approved;

                UnitOfWork.Context.FileControl.Update(entity);
                await UnitOfWork.Context.SaveChangesAsync(cancellationToken: cancellationToken);

                return true;
            }

            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateFileControl",
                "Arquivo não encontrado. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
        catch (Exception e)
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateFileControl",
                "Erro ao atulizar arquivo. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }

        return true;
    }

}