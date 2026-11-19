using MediatR;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.FileControls;
using Sindvamb.Utils.Attributes;

using System;
using System.Collections.Generic;

namespace Sindvamb.Service.Commands.FileControls
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<GetUserFilesComand, List<FileControlDTO>>),
        ImplementType = typeof(FileControlCommandHandler))
    ]
    public class GetUserFilesComand : Command<GetUserFilesComand, Guid, List<FileControlDTO>>
    {
        public bool IsDependent { get; set; }
        public Guid UserId { get; set; }

        public GetUserFilesComand(Guid userId, bool isDependent)
        {
            UserId = userId;
            IsDependent = isDependent;
        }
    }
}
