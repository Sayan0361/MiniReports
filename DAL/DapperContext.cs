using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MiniReportsProject.DAL
{
    public class DapperContext
    {
        public static IDbConnection GetConnection()
        {
            return new SqlConnection(
                ConfigurationManager
                .ConnectionStrings["MiniReportsDB"]
                .ConnectionString
            );
        }
    }
}