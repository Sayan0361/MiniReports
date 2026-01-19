using MiniReportsProject.DAL;
using MiniReportsProject.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MiniReportsProject.Controllers
{
    public class SchoolController : Controller
    {
        SchoolDAL _schoolDAL = new SchoolDAL();

        // GET: School
        // single Index action with optional site id to avoid ambiguous matches
        public ActionResult Index(int? id)
        {
            // if caller provided a site id, expose it to the view (used for linking)
            if (id.HasValue)
            {
                ViewBag.SiteId = id.Value;
            }

            // currently GetAllSchoolNames returns the same list in both cases;
            // change DAL if you need different behaviour when id is present (e.g. only unlinked schools)
            List<SchoolModel> schools = _schoolDAL.GetAllSchoolNames();
            return View(schools);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkSelected(int siteId, int[] selectedIds)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["Error"] = "Please select at least one school.";
                return RedirectToAction("Index", new { id = siteId });
            }

            int linkedCount = 0;

            foreach (var schoolId in selectedIds)
            {
                linkedCount += _schoolDAL.LinkSchoolToSite(schoolId, siteId);
            }

            TempData["Success"] = $"{linkedCount} school(s) linked successfully.";
            return RedirectToAction("Index", "Site", new { id = siteId });
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(SchoolModel school, int id)
        {
            school.SiteID = id;
            if (ModelState.IsValid)
            {
                _schoolDAL.AddSchool(school);
                TempData["Success"] = "School added successfully.";
                return RedirectToAction("Index" ,"Site", new {id = id});
            }
            return View(school);
        }
    }
}