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

            // resolve grantee id: prefer explicit parameter, otherwise fetch site to get GrantID
            int resolvedGranteeId = granteeId ?? 0;
            if (!granteeId.HasValue)
            {
                var site = _siteDAL.GetSiteByID(id);
                if (site != null)
                {
                    resolvedGranteeId = site.GrantID;
                }
            }

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

            return View(vm);
        }

        public ActionResult Create(int id)
        {
            var model = new SiteCreateViewModel
            {
                GrantID = id,
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
            return RedirectToAction("Index", "Grantee", new { id = site.GrantID });
        }
    }
}