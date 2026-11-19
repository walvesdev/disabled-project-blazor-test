
namespace Sindvamb.Web.Shared.Services.FileControls;

public class FileControlService : ServiceBase<FileControlDTO>, IFileControlService
{
    public FileControlService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
        EntitName = nameof(FileControl);
    }

    public async Task<List<FileControlDTO>> GetUserFiles(Guid userId, bool isDependent)
    {
        var http = new HttpClientService<Guid, List<FileControlDTO>, Guid>($"{EntitName}/GetUserFiles/{userId}/{isDependent}");

        var result = await http.Get();

        if (!result.IsSuccessStatus)
        {
            var message = base.GertErrorMessage(result?.ErrorMessage);
            await ShowErrorModal(message);
        }

        return result.Data;
    }
    
    public async Task<bool> ApprovedChange(FileControlDTO dto)
    {
        var http = new HttpClientService<FileControlDTO, bool, Guid>($"{EntitName}/ApprovedChange");

        var result = await http.Post(dto);

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification("Erro ao atualizar status do arquivo. Por favor contate o suporte!");

            await ShowErrorModal(message);
        }

        return result.Data;
    }
}
