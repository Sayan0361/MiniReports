using Dapper;
using MiniReportsProject.DAL;
using MiniReportsProject.Models;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace MiniReportsProject.Controllers
{
    public class StateController : Controller
    {
        public ActionResult Index()
        {
            using (var db = DapperContext.GetConnection())
            {
                IEnumerable<GranteeModel> grantees =
                    db.Query<GranteeModel>(
                        "sp_Grantee_GetAll",
                        commandType: CommandType.StoredProcedure
                    );

                return View(grantees);
            }
        }
    }
}
