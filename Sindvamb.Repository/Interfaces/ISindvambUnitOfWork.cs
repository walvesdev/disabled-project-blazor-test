using Sindvamb.Repository.Contexts;
using Sindvamb.Repository.ORM;

namespace Sindvamb.Repository.Interfaces;

public interface ISindvambUnitOfWork : IUnitOfWork<SindvambDbContext>
{

}
