using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using School_Portal.Data;
using School_Portal.Models;
using School_Portal.ViewModels;

namespace School_Portal.Controllers
{
    public class CourseCategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CourseCategoryController(AppDbContext db)
        {
            _db = db;
        }
        //parameter is needed when you are asked to find by id,username or whatever
        public IActionResult Index()
        {
            var courseCategories = _db.CourseCategories.Where(x => x.Name != null && x.IsActive).ToList();
            return View(courseCategories);
        }

        [HttpGet]
        public IActionResult AddCourseCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCourseCategory(CourseCategory data)
        {
			var courseCategories = _db.CourseCategories.Where(x => x.Name == data.Name && x.IsActive).ToList();
			if (data.Name != null && courseCategories.Count > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
				var courseCategoru = new CourseCategory
				{
					Name = data.Name,
					CreatedDate = DateTime.Now,
					IsActive = true,
				};
				_db.CourseCategories.Add(courseCategoru);
				_db.SaveChanges();
				return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult EditCourseCategory(int? id)
        {
            if (id == null && id <= 0) 
            {
                return NotFound();
            }
            CourseCategory courseCategory = _db.CourseCategories.Find(id);

            return Json(new { isError = false, data = courseCategory });
        }

        [HttpPost]
        public JsonResult EditedCourseCategory(string courseCategory)
        {
            if (courseCategory == string.Empty)
            {
                return Json(new { isError = true, msg = "Not found" });
            }
            var courseCategories = JsonConvert.DeserializeObject<CourseViewModelEdit>(courseCategory);
            if (courseCategories != null)
            {
                var courseCategoru = _db.CourseCategories.FirstOrDefault(r => r.Id == courseCategories.Id);
                if (courseCategoru != null)
                {
                    courseCategoru.Name = courseCategories.Name;
                    _db.CourseCategories.Update(courseCategoru);
                    _db.SaveChanges();
                    return Json(new { isError = false, msg = "Course Category edited successfully" });
                }
            }
            return Json(new { isError = true, msg = "Unable to Edit" });
        }
        [HttpPost]
        public JsonResult DeleteCourseCategoryById(int id)
        {
            if (id == null && id <= 0)
            {
                return Json(new { isError = true, msg = "Not found" });
            }
            var courseCategory = _db.CourseCategories.Where(y => y.Id == id && y.IsActive).FirstOrDefault();
            if (courseCategory != null)
            {
                courseCategory.IsActive = false;
                _db.CourseCategories.Update(courseCategory);
                _db.SaveChanges();
                return Json(new { isError = false, msg = "CourseCategory Deleted Successfully." });
            }
            return Json(new { isError = true, msg = "Not found" });
        }
    }
}
