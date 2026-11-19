using Microsoft.AspNetCore.Components;
using Sindvamb.Web.Shared.Models;

namespace Sindvamb.Web.Shared.Pages.Sistema.Orders
{
    public partial class AddEditOrder : EditComponentBase<OrderDTO, IOrderService, Guid>
    {
        [Parameter] public Guid ProductCategoryId { get; set; }

        [Inject] public IUserService UserService { get; set; }

        [Inject] public ICustomerService CustomerService { get; set; }

        [Inject] public IOrderTypeService OrderTypeService { get; set; }

        [Inject] public IProductAreaService ProductAreaService { get; set; }

        [Inject] public IProductCategoryService ProductCategoryService { get; set; }

        [Inject] public IOrderStatusService OrderStatusService { get; set; }

        public string ProductCategoryDescription { get; set; }

        public List<PartnerUnitDTO> PartnerUnitList { get; set; } = new();

        public List<CustomerModel> CustomerList { get; set; } = new();

        public List<ProductAreaDTO> ProductAreaList { get; set; } = new();

        public List<OrderTypeDTO> OrderTypeList { get; set; } = new();

        public List<OrderStatusDTO> OrderStatusList { get; set; } = new();

        public ProductCategoryDTO ProductCategory { get; set; } = new();

        public string? EmailClient { get; set; }


        public string EmailWithCopy { get; set; } = string.Empty;

        public bool DisabledFileds { get; set; }

        protected override async Task OnInitializedAsync()
        {
            OperationEntityText = "Demanda";

            await base.OnInitializedAsync();
            try
            {

                await GetAllCustomers();
                await GetAllPartnerUnit();
                await GetAllOrderTypes();
                //await GetAllProductArea();
                await GetAllOrderStatus();
            }
            catch
            {
                return ;
            }

        }

        protected override void OnParametersSet()
        {
            try
            {
                base.OnParametersSet();

                if (ProductCategoryId > Guid.Empty)
                {
                    Item.ProductCategoryId = ProductCategoryId;
                }

                if (!IsEdit)
                {
                    DisabledFileds = true;
                    //ProductCategory = await ProductCategoryService.Get(ProductCategoryId);
                }
                else
                {
                    DisabledFileds = false;
                    //ProductCategory = await ProductCategoryService.Get(Item?.ProductCategory?.Id);
                    if (Item.ClientId != Guid.Empty)
                    {
                        ClientChanged(Item.ClientId);
                    }
                }

                //ProductCategoryDescription = ProductCategory?.Description;
            }
            catch
            {
                return;
            }
        }

        public async Task GetAllPartnerUnit()
        {
            PartnerUnitList = await UserService.GetAllPartnerUnit();
        }

        public async Task GetAllCustomers()
        {
            CustomerList = await CustomerService.GetAll<CustomerModel>();
        }

        public async Task GetAllOrderTypes()
        {
            OrderTypeList = await OrderTypeService.GetAll<OrderTypeDTO>();
        }


        public async Task GetAllProductArea()
        {
            ProductAreaList = await ProductAreaService.GetAll<ProductAreaDTO>();
        }

        public async Task GetAllOrderStatus()
        {
            OrderStatusList = await OrderStatusService.GetAll<OrderStatusDTO>();
        }

        private async Task PartnerUnitChanged(object unitId)
        {
            CustomerList = (await CustomerService.GetAll<CustomerModel>()).Where(p => p.User.PartnerUnitId == (Guid?)unitId).ToList();
            DisabledFileds = false;
        }

        private void ClientChanged(object clientId)
        {
            EmailClient = CustomerList?.FirstOrDefault(c => c.Id == (Guid)clientId)?.User?.Email;
        }
    }
}
