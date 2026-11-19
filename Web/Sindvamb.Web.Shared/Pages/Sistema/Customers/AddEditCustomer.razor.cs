using Sindvamb.Web.Shared.Models;

namespace Sindvamb.Web.Shared.Pages.Sistema.Customers
{
    public partial class AddEditCustomer : EditComponentBase<CustomerModel, ICustomerService, Guid>
    {
        public int UserStatusCount { get; set; }

        [Inject] public IUserService UserService { get; set; }
        [Inject] public IEducationDegreeService EducationDegreeService { get; set; }
        [Inject] public ISpecialNeedsService SpecialNeedsService { get; set; }

        public List<CustomerTypeDTO> CustomerTypes { get; set; } = new();

        public List<PartnerUnitDTO> PartnerUnitList { get; set; } = new();

        public List<RoleDTO> RolesList { get; set; } = new();

        public List<UserStatusDTO> UserStatusList { get; set; } = new();

        public List<EducationDegreeDTO> EducationDegreeList { get; set; } = new();

        public List<SpecialNeedsDTO> SpecialNeedsList { get; set; } = new();


        public async Task GetAllUserStatus()
        {
            UserStatusList = await UserService.GetAllUserStatus();
            UserStatusCount = UserStatusList.Count;
        }

        public async Task GetAllRoles()
        {
            RolesList = await UserService.GetAllRoles();
        }

        public async Task GetAllPartnerUnit()
        {
            PartnerUnitList = await UserService.GetAllPartnerUnit();
        }

        public async Task GetAllCustomerTypes()
        {
            CustomerTypes = await Service.GetAllCustomerTypes();
        }

        public async Task GetAllEducationDegree()
        {
            EducationDegreeList = await EducationDegreeService.GetAll<EducationDegreeDTO>();
        }

        public async Task GetAllSpecialNeeds()
        {
            SpecialNeedsList = await SpecialNeedsService.GetAll<SpecialNeedsDTO>();
        }

        protected override async Task OnParametersSetAsync()
        {
            OperationEntityText = "Cliente";

            await base.OnParametersSetAsync();

            await GetAllUserStatus();
            await GetAllRoles();
            await GetAllPartnerUnit();
            await GetAllEducationDegree();
            await GetAllCustomerTypes();
            await GetAllSpecialNeeds();

        }

        private async Task OnCustomerTypeChanged(SelectedItem item)
        {
            Item.CustomerType = CustomerTypes.FirstOrDefault(c => c.Id.Equals(Guid.Parse(item.Value)))!;
        }

        private async Task OnPartnerUnitChanged(SelectedItem item)
        {
            Item.User.PartnerUnit = PartnerUnitList.FirstOrDefault(c => c.Id.Equals(Guid.Parse(item.Value)))!;
        }

        private async Task ResetSubmit(EditContext ev)
        {
            await FormSubmit(ev, "customer");
        }
    }
}
