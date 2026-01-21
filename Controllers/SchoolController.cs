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
        GranteeDAL _granteeDAL = new GranteeDAL();


        // GET: School
        // single Index action with optional site id to avoid ambiguous matches
        public ActionResult Index(int id)
        {
            List<SchoolModel> schools = _schoolDAL.GetAllSchoolNames();
            List<SiteModel> sites = _granteeDAL.GetAllSitesByGranteeID(id);

            var linkedSchoolSiteVM = new LinkSchoolSiteViewModel
            {
                GranteeID = id,
                siteList = sites,
                schoolList = schools,
            };
            return View(linkedSchoolSiteVM);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SchoolGradeViewModel model, bool LinkSiteToSchool)
        {
            // Safety checks (VERY IMPORTANT)
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

            // ✅ Single SP call per grade
            //foreach (int gradeID in model.SelectedGrades)
            //{
            //    schoolId = _schoolDAL.AddSchool(
            //        model.School,
            //        gradeID,
            //        schoolId   // 0 first time, actual ID next times
            //    );
            //}

            string gradeList = string.Join(",", model.SelectedGrades);

            _schoolDAL.AddSchool(model.School, gradeList, LinkSiteToSchool);

            TempData["Success"] = "School added successfully.";

            return RedirectToAction(
                "Index",
                "Site",
                new { id = model.School.SiteID }
            );
        }

        public ActionResult Details(int id)
        {
            var schoolDetailsList = _schoolDAL.GetDetailsByID(id);

            List<string> gradeNames = new List<string>();

            foreach (var school in schoolDetailsList)
            {
                gradeNames.Add(school.GradeName);
            }

            // Assuming you want to show details for a single school, and aggregate grades
            var firstSchool = schoolDetailsList.FirstOrDefault();
            if (firstSchool != null)
            {
                firstSchool.GradeNameList = gradeNames;
                return View(firstSchool);
            }

            return HttpNotFound();
        }
    }
}