using Dapper;
using Microsoft.Ajax.Utilities;
using MiniReportsProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MiniReportsProject.DAL
{
    public class GranteeDAL
    {
        public List<GranteeModel> GetAllGrantees()
        {
            using (var db = DapperContext.GetConnection())
            {
                return db.Query<GranteeModel>(
                    "sp_Grantee_GetAll",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }

        public void AddGrantee(GranteeModel grantee)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    int rowsEffected = db.Execute(
                        "sp_Grantee_Insert",
                        new
                        {
                            GranteeName = grantee.GranteeName,
                            GrantTypeID = grantee.GranteeTypeID,
                            Address = grantee.Address
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
                throw new Exception("Insert Failed in catch block: "+ err.Message);
            }
        }

        public List<EntityTypeModel> GetAllGranteeTypes(string Level)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Query<EntityTypeModel>(
                    "sp_GetEntityTypesByLevel",
                    new
                    {
                        Level = Level
                    },
                    commandType: CommandType.StoredProcedure
                ).ToList();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Fetching Grantee Types Failed in catch block: " + err.Message);
            }
        }

        public int GetTypeIDByTypeName(string level, string typeName)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    var result = db.Query<int>(
                        "sp_GetTypeIDByTypeName",
                        new
                        {
                            Level = level,
                            TypeName = typeName
                        },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception err)
            {
                throw new Exception("Lookup Failed in catch block: " + err.Message);
            }
        }

        public string GetGranteeNameByID(int GranteeID)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    var result = db.Query<string>(
                        "sp_GetGranteeNameByID",
                        new
                        {
                            GranteeID = @GranteeID
                        },
                        commandType: CommandType.StoredProcedure
                    ).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception err)
            {
                throw new Exception("Failed to fetch name from ID: " + err.Message);
            }

        }
        public List<SiteModel> GetAllSitesByGranteeID(int GranteeID)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    var result = db.Query<SiteModel>(
                        "sp_GetAllSitesByGranteeID",
                        new
                        {
                            GranteeID
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList<SiteModel>();

                    return result;
                }
            }
            catch (Exception err)
            {
                throw new Exception("Failed to fetch names: " + err.Message);
            }
        }
    }
}