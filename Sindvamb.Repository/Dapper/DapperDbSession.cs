#region

using Microsoft.Data.SqlClient;

// using Npgsql;

using System.Data;

#endregion

namespace Sindvamb.Repository.Dapper;

public sealed class DapperDbSession : IDisposable
{
    private Guid _id;
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; set; }

    public DapperDbSession()
    {
        _id = Guid.NewGuid();
        Connection = new SqlConnection("Server=localhost,1433;Initial Catalog=sindvamb-api;Persist Security Info=False;User ID=sa;Password=1q2w3E*@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        Connection.Open();
    }

    public void Dispose() => Connection?.Dispose();
}
