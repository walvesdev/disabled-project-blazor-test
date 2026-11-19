using System.Data.Common;

using Sindvamb.Repository.Contexts;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Repository.ORM;

namespace Sindvamb.Repository.UnitsOfWork;

public class SindvambUnitOfWork : UnitOfWork<SindvambDbContext>, ISindvambUnitOfWork
{
    public SindvambUnitOfWork(SindvambDbContext context, DbTransaction transaction = null) : base(context, transaction)
    {

    }
}
