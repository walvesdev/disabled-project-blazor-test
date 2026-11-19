using BootstrapBlazor.Components;

using System.Diagnostics.CodeAnalysis;

namespace Sindvamb.Web.Shared.Models
{
    public class OnChangeValidateFormModel
    {
        [NotNull]
        public ValidateForm Form { get; set; }

        public bool HasValidation { get; set; }

        public bool ModelHasChanged { get; set; }

        public CustomerModel Model { get; set; } = null;
    }
}
