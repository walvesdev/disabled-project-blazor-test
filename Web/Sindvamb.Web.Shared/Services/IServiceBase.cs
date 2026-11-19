namespace Sindvamb.Web.Shared.Services;

public interface IServiceBase<TModel>
{
    public bool ShowErrorMessage { get; set; } 
    public ToastService? ToastService { get; set; }
    public SwalService? SwalService { get; set; }
    public string EntitName { get; set; }
    Task<List<TModel>> GetAll<TModel>();
    Task<TModel> Get<TModel>(dynamic id);
    Task<TModel> Create<TModel>(TModel dto);
    Task<bool> Update<TModel>(TModel model);
    Task<bool> Delete(dynamic id);
    Task<List<TModel>> GetByFilter<TModel>(object filter);
    string GertErrorMessage(string? errorMessage);
    Task ShowErrorModal(string message);
}