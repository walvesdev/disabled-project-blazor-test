#region

using FluentValidation;

#endregion

namespace Sindvamb.Events.BuildingBlocks.Common;

public class GetAllValidator<T> : AbstractValidator<T>
{
    public static string SortingErroMsg => "Sorting inválido";

    public static string SkipCountErroMsg => "SkipCount inválido";

    public static string MaxResultCountErroMsg => "MaxResultCount informado";
}
