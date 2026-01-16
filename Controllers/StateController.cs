using Dapper;
using Microsoft.Ajax.Utilities;
using MiniReportsProject.DAL;
using MiniReportsProject.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace MiniReportsProject.Controllers
{
    public class StateController : Controller
    {
        GranteeDAL _granteeDAL = new GranteeDAL();

        public ActionResult Index()
        {
            List<GranteeModel> grantees = _granteeDAL.GetAllGrantees();
            return View(grantees);
        }

        public ActionResult Create()
        {
            // populate types into a strongly-typed view model (Level = "Grantee")
            var createViewModel = new GranteeCreateViewModel
            {
                EntityTypes = _granteeDAL.GetAllGranteeTypes("Grantee")
            };
            return View(createViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GranteeCreateViewModel createViewModel)
        {
            if (!ModelState.IsValid)
            {
                // re-populate types for redisplay
                createViewModel.EntityTypes = _granteeDAL.GetAllGranteeTypes("Grantee");
                return View(createViewModel);
            }

            // lookup EntityTypeID by Level and TypeName via stored procedure
            int typeId = _granteeDAL.GetTypeIDByTypeName("Grantee", createViewModel.SelectedTypeName);
            if (typeId == 0)
            {
                ModelState.AddModelError(nameof(createViewModel.SelectedTypeName), "Selected type not found.");
                createViewModel.EntityTypes = _granteeDAL.GetAllGranteeTypes("Grantee");
                return View(createViewModel);
            }

            // create GranteeModel and persist
            var grantee = new GranteeModel
            {
                GranteeName = createViewModel.GranteeName,
                GranteeTypeID = typeId,
                Address = createViewModel.Address
            };

            _granteeDAL.AddGrantee(grantee);
            return RedirectToAction("Index");
        }

        public ActionResult GetGranteeTypesByLevel(string level)
        {
            List<EntityTypeModel> granteeTypes = _granteeDAL.GetAllGranteeTypes(level);
            return View(granteeTypes);
        }
    }
}
