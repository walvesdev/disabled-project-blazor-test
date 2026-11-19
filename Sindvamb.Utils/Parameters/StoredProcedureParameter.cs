using System.Data;

namespace Sindvamb.Utils.Parameters
{
    /// <summary>
    /// Attribute For gettting the properties that must be added to the Parameter List of the used Stored Procedure
    /// </summary>
    public class StoredProcedureParameter : Attribute
    {
        public string PropName { get; set; }
        public SqlDbType SqlType { get; set; }

        public StoredProcedureParameter()
        {
        }
    }
}
