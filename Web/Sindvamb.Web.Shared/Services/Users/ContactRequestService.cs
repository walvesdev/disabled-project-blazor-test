namespace Sindvamb.Web.Shared.Services.Users;

public class ContactRequestService : ServiceBase<ContactRequestDTO>, IContactRequestService
{

    public ContactRequestService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
        EntitName = nameof(ContactRequest);
    }

    //
    // public async Task<List<ContactDTO>> GetAllContacts()
    // {
    //     var http = new HttpClientService<string, List<ContactDTO>, Guid>($"User/GetAllContacts");
    //
    //     var result = await http.Get();
    //
    //     if (!result.IsSuccessStatus)
    //     {
    //         var message = result.TryGetDomainNotification($"Erro ao buscar contatos. Por favor contate o suporte!");
    //         await ShowErrorModal(message);
    //     }
    //
    //     return result.Data;
    // }
}
