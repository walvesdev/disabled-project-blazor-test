using Sindvamb.Web.Shared.Data;
using System.Linq;

namespace Sindvamb.Web.Shared.Components.Core;

public class ListComponentBase<TItem, TService> : AppComponentBase where TService : IServiceBase<TItem> where TItem : class, new()
{
    [Inject] protected TService Service { get; set; }

    [Inject] protected SwalService SwalService { get; set; }

    protected IEnumerable<TItem> Items { get; set; }

    protected IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 10, 20, 30, 50, 100 };

    protected int Count { get; set; }

    protected IEnumerable<int> PageItemsSource => new int[]
    {
        10,
        4,
        20,
        40,
        80,
        100
    };

    protected override async Task OnInitializedAsync()
    {
        Items = new List<TItem>();
        await GridLoadData();
    }

    public bool ShowTable { get; set; }

    protected async Task GridLoadData()
    {
        ToggleLoading();
        Items = await Service.GetAll<TItem>();
        Count = Items.Count();
        if (Items.Any())
        {
            ShowTable = true;
        }
        else
        {
            ShowTable = false;
        }
        ToggleLoading();
    }

    protected async Task AddButtonClick(string url)
    {
        await NavigationManager.NavigateTo(url);
    }

    protected async Task EditRow(dynamic args, string url)
    {
        await NavigationManager.NavigateTo($"{url}/{args.Id}");
    }

    protected async Task EditRowType(TItem dto, Func<TItem, Task<bool>> func)
    {

        try
        {
            var option = new EditDialogOption<TItem>()
            {
                Title = "Editar",
                Model = dto,
                CloseButtonText = "Fechar",
                SaveButtonText = "Salvar",
                IsBackdrop = true,
                IsScrolling = true,
                ItemsPerRow = 1,
                RowType = RowType.Normal,
                OnCloseAsync = () => Task.CompletedTask,
                OnEditAsync = async context =>
                {
                    if (context.Validate())
                    {
                        var result = await func.Invoke((TItem)context.Model);
                        dto = (TItem)context.Model;

                        if (result)
                        {
                            await GridLoadData();
                        }

                        await SwalService.Show(new SwalOption()
                        {
                            Category = SwalCategory.Success,
                            Content = "Registro atualizado com sucesso!",
                        });

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            };

            await DialogService.ShowEditDialog(option);
        }
        catch (Exception e)
        {

            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Content = "Erro ao atualizar registro!",
            });

        }
    }

    protected async Task AddRowType(Func<TItem, Task<TItem>> func)
    {
        try
        {
            TItem dto = new();

            var option = new EditDialogOption<TItem>()
            {
                Title = "Adicionar",
                Model = dto,
                CloseButtonText = "Fechar",
                SaveButtonText = "Salvar",
                IsBackdrop = true,
                IsScrolling = true,
                ItemsPerRow = 1,
                RowType = RowType.Normal,
                OnCloseAsync = () => Task.CompletedTask,
                OnEditAsync = async context =>
                {
                    if (context.Validate())
                    {
                        var result = await func.Invoke((TItem)context.Model);
                        dto = (TItem)context.Model;

                        if (result.IsNotNull())
                        {
                            await GridLoadData();
                        }

                        await SwalService.Show(new SwalOption()
                        {
                            Category = SwalCategory.Success,
                            Content = "Registro adicionado com sucesso!",
                        });

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            };

            await DialogService.ShowEditDialog(option);
        }
        catch (Exception e)
        {

            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Content = "Erro ao adicionar registro!",
            });
        }
    }

    protected async Task AddRowTemplate<TBodyTemplate>(string title) where TBodyTemplate : EditComponentBase<TItem, TService, Guid>
    {
        try
        {
            TItem dto = new();

            var option = new EditDialogOption<TItem>()
            {
                Title = title,
                Model = dto,
                IsScrolling = true,
                IsBackdrop = true,
                ItemsPerRow = 1,
                RowType = RowType.Normal,
                OnCloseAsync = async () =>
                {
                    await GridLoadData();
                }
            };

            option.BodyTemplate = BootstrapDynamicComponent.CreateComponent<TBodyTemplate>(
                new Dictionary<string, object?>
                {
                    ["OnCloseAsync"] = new Action(() => option.CloseDialogAsync())
                }).Render();

            await DialogService.ShowEditDialog(option);
        }
        catch (Exception e)
        {
            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Content = "Erro ao adicionar registro!",
            });
        }
    }

    protected async Task EditRowTemplate<TBodyTemplate>(string title, TItem model) where TBodyTemplate : EditComponentBase<TItem, TService, Guid>
    {
        try
        {
            var id = model.GetPropertyValue("Id");

            var option = new EditDialogOption<TItem>
            {
                Title = title,
                Model = model,
                IsBackdrop = true,
                IsScrolling = true,
                ItemsPerRow = 1,
                RowType = RowType.Normal,
                OnCloseAsync = async () =>
                {
                    await GridLoadData();
                }
            };

            option.BodyTemplate = BootstrapDynamicComponent.CreateComponent<TBodyTemplate>(
                new Dictionary<string, object?>
                {
                    ["Item"] = model,
                    ["Id"] = id,
                    ["OnCloseAsync"] = new Action(() => option.CloseDialogAsync())
                }).Render();

            await DialogService.ShowEditDialog(option);
        }
        catch (Exception e)
        {
            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Content = "Erro ao atualizar registro!",
            });
        }
    }

    protected async Task GridDeleteButtonClick(dynamic item)
    {
        try
        {
            ToggleLoading();

            var approveOperation = await SwalService.ShowModal(new SwalOption()
            {
                Category = SwalCategory.Warning,
                Title = "Atenção!",
                Content = $"Gostaria de excluir esse registro do banco de dados?",
            });

            if (approveOperation)
            {

                var sucesso = await Service.Delete(item.Id);

                if (sucesso)
                {
                    //await Grid.Reload();
                    await GridLoadData();

                    await SwalService.Show(new SwalOption()
                    {
                        Category = SwalCategory.Success,
                        Title = "Feito!",
                        Content = "Registro Deletado com sucesso!",
                    });
                }
                else
                {
                    await SwalService.Show(new SwalOption()
                    {
                        Category = SwalCategory.Error,
                        Title = "Atenção!",
                        Content = $"Erro ao excluir registro, por favor contate o suporte!",
                    });
                }

                ToggleLoading();
            }
        }
        catch (Exception e)
        {
            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Content = "Erro ao deletar registro!",
            });
        }
    }

    protected void ToggleShowTable()
    {
        ShowTable = !ShowTable;
        
    }

    //protected async Task<QueryData<TItem>?> OnQueryAsync(QueryPageOptions options)
    //{
    //    IEnumerable<TItem> items = Items;

    //    if (items is not null)
    //    {
    //        var total = items.Count();
    //        items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
    //        return new QueryData<TItem>()
    //        { Items = items, TotalCount = total, IsSorted = true, IsFiltered = true, IsSearch = true };
    //    }

    //    return default;
    //}

    public async Task<QueryData<TItem>?> OnQueryAsync(QueryPageOptions options)
    {
        var filters = options.ToFilter();

        var items = Items.Where(filters.GetFilterFunc<TItem>());

        var isSorted = false;

        if (options.AdvancedSortList.Count != 0)
        {
            items = items.Sort(options.AdvancedSortList);
            isSorted = true;
        }

        if (options.SortName == nameof(DateTime))
        {
            items = items.Sort(options.SortList);
            isSorted = true;
        }
        else if (!string.IsNullOrEmpty(options.SortName))
        {
            items = items.Sort(options.SortName, options.SortOrder);
            isSorted = true;
        }

        var enumerable = items.ToList();
        var total = enumerable.Count;

        items = enumerable.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

        return new QueryData<TItem>()
        {
            Items = items,
            TotalCount = total,
            IsSorted = isSorted,
            IsFiltered = filters.HasFilters()
        };
    }

    public static string OnSort(string sortName, SortOrder sortOrder)
    {
        string sortString = "";
        if (sortName == nameof(DateTime))
        {
            if (sortOrder == SortOrder.Asc)
            {
                sortString = "DateTime, Count";
            }
            else if (sortOrder == SortOrder.Desc)
            {
                sortString = "DateTime desc, Count desc";
            }
            else
            {
                sortString = "DateTime desc, Count";
            }
        }
        return sortString;
    }
}
