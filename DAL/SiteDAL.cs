using Dapper;
using MiniReportsProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MiniReportsProject.DAL
{
    public class SiteDAL
    {
        public void AddSite(SiteModel site)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    int rowsEffected = db.Execute(
                        "sp_InsertSite",
                        new
                        {
                            SiteName = site.SiteName,
                            SiteTypeID = site.SiteTypeID,
                            Address = site.Address,
                            GrantID = site.GrantID

                        },
                        commandType: CommandType.StoredProcedure
                    );
                    if (rowsEffected == 0)
                    {
                        throw new Exception("Insert operation failed.");
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("Insert Failed in catch block: " + err.Message);
            }
        }
    }
}