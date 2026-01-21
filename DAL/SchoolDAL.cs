using Dapper;
using MiniReportsProject.Models;
using MiniReportsProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;

namespace MiniReportsProject.DAL
{
    public class SchoolDAL
    {
        public List<SchoolModel> GetAllSchoolNames()
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Query<SchoolModel>(
                        "sp_GetAllSchoolNames",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Fetching Grantee Types Failed in catch block: " + err.Message);
            }
        }

        public int LinkSchoolToSite(int schoolId, int siteId)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Execute(
                        "sp_LinkSchoolToSite",
                        new { SchoolID = schoolId, SiteID = siteId },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception err)
            {
                throw new Exception("Fetching Grantee Types Failed in catch block: " + err.Message);
            }
        }

        public void AddSchool(SchoolModel school, string gradeList)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    db.Execute(
                        "sp_InsertSchoolAndGrade",
                        new
                        {
                            GradeList = gradeList,
                            SchoolName = school.SchoolName,
                            SiteID = school.SiteID,
                            Address = school.Address
                        },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception err)
            {
                throw new Exception("Insert Failed in catch block: " + err.Message);
            }
        }
        
        public List<SchoolDetailsViewModel> GetDetailsByID(int schoolID)
        {
            try
            {
                using (var db = DapperContext.GetConnection())
                {
                    return db.Query<SchoolDetailsViewModel>(
                        "sp_GetSchoolDetailsByID",
                        new { SchoolID = schoolID },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            catch (Exception err)
            {
                throw new Exception("Fetching School Details Failed in catch block: " + err.Message);
            }
        }
    }
}