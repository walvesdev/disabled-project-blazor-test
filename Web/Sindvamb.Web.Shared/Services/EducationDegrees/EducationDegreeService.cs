namespace Sindvamb.Web.Shared.Services.EducationDegrees;

public class EducationDegreeService : ServiceBase<EducationDegreeDTO>, IEducationDegreeService
{
    public EducationDegreeService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
        EntitName = nameof(EducationDegree);
    }

}
