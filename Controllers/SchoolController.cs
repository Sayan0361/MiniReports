using MiniReportsProject.DAL;
using MiniReportsProject.Models;
using MiniReportsProject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

        public ActionResult Add(int id)
        {
            var model = new SchoolGradeViewModel
            {
                School = new SchoolModel
                {
                    SiteID = id
                },
                SelectedGrades = new List<int>()
            };

            return View(model);
        }


        //[HttpPost]
        //public ActionResult Add(SchoolModel school, int id)
        //{
        //    school.SiteID = id;
        //    if (ModelState.IsValid)
        //    {
        //        _schoolDAL.AddSchool(school);
        //        TempData["Success"] = "School added successfully.";
        //        return RedirectToAction("Index" ,"Site", new {id = id});
        //    }
        //    return View(school);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SchoolGradeViewModel model)
        {
            // 🔒 Safety checks (VERY IMPORTANT)
            if (model == null)
            {
                return HttpNotFound();
            }

            if (model.School == null)
            {
                ModelState.AddModelError("", "School details are missing.");
            }

            if (model.SelectedGrades == null || !model.SelectedGrades.Any())
            {
                ModelState.AddModelError("", "Please select at least one grade.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int schoolId = 0;

            // ✅ Single SP call per grade
            foreach (int gradeID in model.SelectedGrades)
            {
                schoolId = _schoolDAL.AddSchool(
                    model.School,
                    gradeID,
                    schoolId   // 0 first time, actual ID next times
                );
            }

            TempData["Success"] = "School added successfully.";

            return RedirectToAction(
                "Index",
                "Site",
                new { id = model.School.SiteID }
            );
        }

    }
}