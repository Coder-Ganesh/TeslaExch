using System.Configuration;
using System.Data.SqlClient;

namespace Probet247.Models
{
    public class ConnectionHandler
    {
        private static SqlConnection con = null;

        static public SqlConnection Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);
            return con;
        }
    }
}