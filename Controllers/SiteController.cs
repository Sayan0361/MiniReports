using MiniReportsProject.DAL;
using MiniReportsProject.Models;
using MiniReportsProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiniReportsProject.Controllers
{
    public class SiteController : Controller
    {
        SiteDAL _siteDAL = new SiteDAL();
        GranteeDAL _granteeDAL = new GranteeDAL();
        // GET: Site
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int grantId)
        {
            var model = new SiteCreateViewModel
            {
                GrantID = grantId,
                EntityTypes = _granteeDAL.GetAllGranteeTypes("Site")
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SiteCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EntityTypes = _granteeDAL.GetAllGranteeTypes("Site");
                return View(model);
            }

            // lookup SiteTypeID using SAME SP
            int siteTypeId = _granteeDAL.GetTypeIDByTypeName("Site", model.SelectedTypeName);

            if (siteTypeId == 0)
            {
                ModelState.AddModelError(nameof(model.SelectedTypeName), "Selected type not found.");
                model.EntityTypes = _granteeDAL.GetAllGranteeTypes("Site");
                return View(model);
            }

            var site = new SiteModel
            {
                SiteName = model.SiteName,
                SiteTypeID = siteTypeId,
                Address = model.Address,
                GrantID = model.GrantID
            };

            _siteDAL.AddSite(site);

            return RedirectToAction("Index", "Grantee", new { id = model.GrantID });
        }

    }
}