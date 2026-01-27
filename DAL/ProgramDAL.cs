using Dapper;
using MiniReportsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniReportsProject.DAL
{
    public class ProgramDAL
    {
        public List<ProgramModel> GetProgramData(int ID)
        {
            try
            {
                using(var db = DapperContext.GetConnection())
                {
                    return db.Query<ProgramModel>(
                        "sp_GetAllSitesByProgramID",
                        new { GranteeID = ID },
                        commandType: System.Data.CommandType.StoredProcedure
                    ).ToList();
                    
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error in GetProgramData: " + err.Message);
            }
        }

        public List<string> GetAllSitesDetailsByProgramID(int id, int GranteeID)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Query<string>(
                        "sp_GetAllSitesDetailsByProgramID",
                        new { ProgramID = id, GranteeID = GranteeID },
                        commandType: System.Data.CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error in GetAllSitesByProgramID: " + err.Message);
            }
        }

        public List<string> GetAllSchoolDetailsByProgramID(int id, int GranteeID)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Query<string>(
                        "sp_GetAllSchoolDetailsByProgramID",
                        new { ProgramID = id, GranteeID = GranteeID },
                        commandType: System.Data.CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error in GetAllSitesByProgramID: " + err.Message);
            }
        }

        public List<ProgramModel> GetAllProgramTypes()
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Query<ProgramModel>(
                        "sp_GetAllProgramTypes",
                        commandType: System.Data.CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error in GetAllProgramTypes: " + err.Message);
            }
        }

        public List<SiteModel> GetAllUnlinkedSitesWithProgram(int GranteeID)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Query<SiteModel>(
                        "sp_GetAllUnlinkedSitesWithProgram",
                        new { GranteeID = GranteeID },
                        commandType: System.Data.CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error in GetAllUnlinkedSitesWithProgram: " + err.Message);
            }
        }

        public int LinkProgramToSite(int SiteID, int ProgramTypeID)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Execute(
                        "sp_LinkProgramToSite",
                        new {SiteID, ProgramTypeID },
                        commandType: System.Data.CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception err)
            {
                throw new Exception("Error in LinkProgramToSite: " + err.Message);
            }
        }
    }
}