using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services;

namespace Sindvamb.Web.Shared.Services.FileControls;

public interface IFileControlService : IServiceBase<FileControlDTO>
{
    Task<List<FileControlDTO>> GetUserFiles(Guid userId, bool isDependent);
    Task<bool> ApprovedChange(FileControlDTO dto);
}
