using Console = System.Console;

namespace Sindvamb.Web.Shared.Services;

public class ServiceBase<TEntity> : IServiceBase<TEntity>
{
    public bool ShowErrorMessage { get; set; } = true;
    public ToastService? ToastService { get; set; }
    public SwalService? SwalService { get; set; }
    public string EntitName { get; set; }


    public ServiceBase(ToastService? toastService, SwalService? swalService)
    {
        ToastService = toastService;
        SwalService = swalService;
    }

    public async Task<List<TModel>> GetAll<TModel>()
    {
        var http = new HttpClientService<dynamic, List<TModel>, dynamic>($"{EntitName}/GetAll");

        var result = await http.Get();

        if (!result.IsSuccessStatus)
        {
            var message = GertErrorMessage(result?.ErrorMessage);
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<TModel> Get<TModel>(dynamic id)
    {
        var http = new HttpClientService<dynamic, TModel, dynamic>($"{EntitName}/Get/{id}");

        var result = await http.Get();

        if (!result.IsSuccessStatus)
        {
            var message = GertErrorMessage(result?.ErrorMessage);
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<TModel> Create<TModel>(TModel dto)
    {
        var http = new HttpClientService<TModel, TModel, dynamic>($"{EntitName}/Create");

        var result = await http.Post(dto);

        if (!result.IsSuccessStatus)
        {
            var message = GertErrorMessage(result?.ErrorMessage);
            await ShowErrorModal(message);
        }

        return result.Data;
    }
    
    public async Task<bool> Update<TModel>(TModel model)
    {
        var http = new HttpClientService<TModel, bool, Guid>($"{EntitName}/Update");

        var result = await http.Put(model);

        if (!result.IsSuccessStatus)
        {
            var message = GertErrorMessage(result?.ErrorMessage);
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<bool> Delete(dynamic id)
    {
        var http = new HttpClientService<dynamic, bool, dynamic>($"{EntitName}/Delete");

        var result = await http.Delete(id);

        if (!result.IsSuccessStatus)
        {
            var message = GertErrorMessage(result?.ErrorMessage);
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<List<TModel>> GetByFilter<TModel>(object filter)
    {
        var http = new HttpClientService<object, List<TModel>, dynamic>($"{EntitName}/GetByFilter");
        var result = await http.Get(filter);

        if (!result.IsSuccessStatus)
        {
            var message = GertErrorMessage(result?.ErrorMessage);
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public string GertErrorMessage(string? errorMessage)
    {
        if (!errorMessage.IsNullOrWhiteSpace())
        {
            try
            {
                var message = errorMessage?.Deserialize<ErrorMessage>();

                if (message.IsNotNull() && message.Value.IsNotNull())
                {
                    return message.Value?.ValidationErrors?.FirstOrDefault().Validator!; 
                }

                return $"{errorMessage}";
            }
            catch 
            {
                return $"{errorMessage}";
            }
        }

        return $"Erro ao executar essa operação. Por favor contate o suporte!";
    }

    public async Task ShowErrorModal(string message)
    {
        if (ShowErrorMessage)
        {
            await SwalService?.Show(new SwalOption
            {
                Category = SwalCategory.Error,
                Title = "Atenção!",
                Content = message,
                IsAutoHide = false,
                Class = ""
            });
        }

        Console.WriteLine(message);
    }
}