using MiniReportsProject.DAL;
using MiniReportsProject.Models;
using MiniReportsProject.ViewModel;
using System.Linq;
using System.Web.Mvc;

namespace MiniReportsProject.Controllers
{
    public class SiteController : Controller
    {
        SiteDAL _siteDAL = new SiteDAL();
        GranteeDAL _granteeDAL = new GranteeDAL();

        // GET: Site
        // id = siteId, optional granteeId supplied by caller
        public ActionResult Index(int id, int? granteeId)
        {
            var schoolList = _siteDAL.GetAllSchoolsBySiteID(id);

            var site = _siteDAL.GetSiteByID(id);
            var resolvedGranteeId = site.GrantID;

            string granteeName = null;
            if (resolvedGranteeId > 0)
            {
                granteeName = _granteeDAL.GetGranteeNameByID(resolvedGranteeId);
            }

            var vm = new SiteSchoolsViewModel
            {
                SiteID = id,
                GranteeID = resolvedGranteeId,
                GranteeName = granteeName,
                Schools = schoolList
            };

            vm.Modal = new ModalViewModel
            {
                Title = "Login",
                Message = "Login Success.",
                Status = true, // set to true to sho
                UserName = site.SiteName
            };

            return View(vm);
        }

        // GET: Create new site
        public ActionResult Create(int id)
        {
            var model = new SiteCreateViewModel
            {
                GrantID = id,
                EntityTypes = _granteeDAL.GetAllGranteeTypes("Site")
            };

            return View(model);
        }

        // POST: Create new site
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SiteCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.EntityTypes = _granteeDAL.GetAllGranteeTypes("Site");
                return View(model);
            }

            int siteTypeId = _granteeDAL.GetTypeIDByTypeName("Site", model.SelectedTypeName);
            if (siteTypeId == 0)
            {
                ModelState.AddModelError(nameof(model.SelectedTypeName), "Invalid site type.");
                model.EntityTypes = _granteeDAL.GetAllGranteeTypes("Site");
                return View(model);
            }

            var site = new SiteModel
            {
                SiteID = 0,  // Always 0 for new sites
                SiteName = model.SiteName,
                SiteTypeID = siteTypeId,
                Address = model.Address,
                GrantID = model.GrantID
            };

            _siteDAL.AddOrEditSite(site);

            TempData["Success"] = "Site created successfully.";
            return RedirectToAction("Index", "Grantee", new { id = model.GrantID });
        }

        // GET: Edit site
        public ActionResult Edit(int SiteID)
        {
            var site = _siteDAL.GetSiteByID(SiteID);
            if (site == null)
            {
                return HttpNotFound();
            }

            var model = new SiteCreateViewModel
            {
                SiteID = site.SiteID,
                SiteName = site.SiteName,
                Address = site.Address,
                GrantID = site.GrantID,
                SelectedTypeName = _granteeDAL.GetAllGranteeTypes("Site")
                    .FirstOrDefault(t => t.EntityTypeID == site.SiteTypeID)?.TypeName,
                EntityTypes = _granteeDAL.GetAllGranteeTypes("Site")
            };

            return View(model);
        }

        // POST: Edit site
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(SiteCreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        model.EntityTypes = _granteeDAL.GetAllGranteeTypes("Site");
        //        return View(model);
        //    }

        //    if (model.SiteID <= 0)
        //    {
        //        ModelState.AddModelError("", "Invalid site ID.");
        //        model.EntityTypes = _granteeDAL.GetAllGranteeTypes("Site");
        //        return View(model);
        //    }

        //    int siteTypeId = _granteeDAL.GetTypeIDByTypeName("Site", model.SelectedTypeName);
        //    if (siteTypeId == 0)
        //    {
        //        ModelState.AddModelError(nameof(model.SelectedTypeName), "Invalid site type.");
        //        model.EntityTypes = _granteeDAL.GetAllGranteeTypes("Site");
        //        return View(model);
        //    }

        //    var site = new SiteModel
        //    {
        //        SiteID = model.SiteID,
        //        SiteName = model.SiteName,
        //        SiteTypeID = siteTypeId,
        //        Address = model.Address,
        //        GrantID = model.GrantID
        //    };

        //    _siteDAL.AddOrEditSite(site);

        //    TempData["Success"] = "Site updated successfully.";
        //    return RedirectToAction("Index", "Grantee", new { id = model.GrantID });
        //}

        public ActionResult Delete(int id)
        {
            var site = _siteDAL.GetSiteByID(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var site = _siteDAL.GetSiteByID(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            _siteDAL.DeleteSite(id);
            TempData["Success"] = "Site deleted successfully.";
            return RedirectToAction("Index", "Grantee", new { id = site.GrantID });
        }

        public JsonResult GetSiteByID(int id)
        {
            var site = _siteDAL.GetSiteByID(id);
            var siteTypes = _granteeDAL.GetAllGranteeTypes("Site");
            var selectedType = siteTypes.FirstOrDefault(t => t.EntityTypeID == site.SiteTypeID);
            if (site == null)
            {
                return Json(new { success = false, message = "Site not found." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = true,
                data = new
                {
                    site.SiteID,
                    site.SiteName,
                    site.SiteTypeID,
                    site.Address,
                    site.GrantID,
                    siteTypes,
                    selectedType,
                }
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Edit site
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(SiteCreateViewModel model)
        {
            if(model.SiteID <= 0)
            {
                return Json(new { success = false, message = "Invalid ID" });
            }

            int siteTypeId = _granteeDAL.GetTypeIDByTypeName("Site", model.SelectedTypeName);

            if (siteTypeId == 0)
                return Json(new { success = false, message = "Invalid type" });

            var site = new SiteModel
            {
                SiteID = model.SiteID,
                SiteName = model.SiteName,
                SiteTypeID = siteTypeId,
                Address = model.Address,
                GrantID = model.GrantID
            };

            _siteDAL.AddOrEditSite(site);

            return Json(new { success = true });
        }
    }
}