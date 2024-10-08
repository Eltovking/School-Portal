﻿using iText.Forms.Form.Element;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using School_Portal.Data;
using School_Portal.Iservices;
using School_Portal.Models;
using School_Portal.ViewModels;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace School_Portal.Controllers
{
	public class CourseController : Controller
	{
        private readonly AppDbContext _db;
        private readonly IUserHelper _userHelper;
        public CourseController(AppDbContext db,IUserHelper userHelper)
        {
            _db  =  db;
            _userHelper = userHelper;
        }
        public IActionResult Index()
		{
            var courses = _db.Courses
                .Where(x =>  x.Name != null && x.IsActive)
                .Include(v=>v.CourseCategory)
                .Include(y => y.Teacher)
                .ToList();
            return View(courses);            
		}

        [HttpGet]
        public IActionResult AddCourse()
        {
            ViewBag.Categories = _userHelper.GetCourseCategories();
            ViewBag.Teacher = _userHelper.GetTeacher();
            //ViewBag.Categories = _userHelper.GetCourseTeacher();
            return View();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddCourse(CourseViewModel data)       
        {
            var imageDataUrl = "";
            var pdfString = await ExtractTextFromPdf(data.CourseMaterials!);
            var imageString = await ExtractTextFromPdf(data.Image!);
            if (data.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await  data.Image.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();

                    // Convert byte array to Base64 string
                   string ConvertedImage = Convert.ToBase64String(imageBytes);
                   imageDataUrl = string.Format("data:image/jpeg;base64,{0}", ConvertedImage);
                }
                //here i instantiate a new class called course and then start mapping data from courseViewModel into the new course
                Course dimma = new Course();
                dimma.Name = data.Name;
                dimma.Image = $"data:{data.Image.ContentType};base64,{imageString}";
                dimma.CourseMaterials = $"data:{data.CourseMaterials.ContentType};base64,{pdfString}";
                dimma.Price = data.Price;
                dimma.TeacherId = data.TeacherId;
                dimma.Description = data.Description;
                dimma.CategoryId = data.CategoryId;
                dimma.IsActive = true;
                dimma.CreatedDate = DateTime.Now;
                _db.Courses.Add(dimma);
                _db.SaveChanges();
                return RedirectToAction("Index");
             
            }
            else
            {
                // handle response
            }
                 return View();
		}

        public static async Task<string> ExtractTextFromPdf(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }

        //      public IActionResult GetCourses()
        //      {
        //	List<Course> courseList = _db.Courses.ToList();

        //	if (courseList.Count() > 0)
        //	{
        //		return View();
        //	}
        //          return View();

        //}

        //public IActionResult GetCoursesFrom()
        //{

        //	List<Course> courseList = _db.Courses.ToList();

        //          if (courseList.Count() > 0)
        //          {
        //              return RedirectToAction("GetCourses");
        //          }
        //          else 
        //          { 
        //              //handle what happens here

        //          }    
        //	return View();
        //}
        // Here we do a check for possible error 
        public IActionResult ApproveCourse(int courseId)
        {
            if (courseId != 0)
            {
                var isApprove = _userHelper.ApproveCourse(courseId);
                if (isApprove)
                {
                    return RedirectToAction("Index");
                }
				return RedirectToAction("Index");
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

        public IActionResult RemoveCourse(int courseId)
        {
            if (courseId != 0)
            {
                var isRemove = _userHelper.RemoveCourse(courseId);
                if (!isRemove)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            else
            { 
                return RedirectToAction("Index");
            }
        }
		[HttpGet]
	
        public IActionResult EditCourse(int? courseId)
        {
            var courseViewModel = new CourseViewModelEdit();
            ViewBag.Teacher = _userHelper.GetTeacher();
            ViewBag.Categories = _userHelper.GetCourseCategories();
            if (courseId == 0)
            {
                return RedirectToAction("Index");
            }
            var course = _db.Courses.Where(y => y.Id == courseId && y.IsActive)
                .Include(c => c.CourseCategory)
                .Include(p => p.Teacher)
                .FirstOrDefault();
            if (course != null)
            {
                courseViewModel.Id = course.Id;
                courseViewModel.CategoryName = course.CourseCategory?.Name;
                courseViewModel.Description = course.Description;
                courseViewModel.Price = course.Price;
                courseViewModel.Image = course.Image;
                courseViewModel.CategoryId = course.Id;
                courseViewModel.CourseMaterials = course.CourseMaterials;
                courseViewModel.Name = course.Name;
                courseViewModel.TeacherId = course.TeacherId;

                // Initialize Teacher property
                courseViewModel.Teacher = new ApplicationUser(); 
                courseViewModel.Teacher.FirstName = course.Teacher?.FirstName;
                courseViewModel.Teacher.LastName = course.Teacher?.LastName;
            }
            return View(courseViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditedCourse(CourseViewModel data)
        {
            var imageDataUrl = "";
            if (data != null)
            {

                if (data.Image != null)
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        await data.Image.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        // Convert byte array to Base64 string
                        string ConvertedImage = Convert.ToBase64String(imageBytes);
                        imageDataUrl = string.Format("data:image/jpeg;base64,{0}", ConvertedImage);
                    }
                }

                var course = _db.Courses.Where(y => y.Id == data.Id && y.IsActive).FirstOrDefault();
                course.Name = data.Name;
                course.Description = data.Description;
                course.Price = data.Price;
                course.Image = imageDataUrl;
                course.CategoryId = data.CategoryId;
                course.TeacherId = data.TeacherId;
                _db.Courses.Update(course);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(data);
        }



        //[HttpPost]
        public IActionResult DeleteCourse(int? courseId)
        {
            if (courseId != null)
            {
                var course = _db.Courses.Where(y => y.Id == courseId && y.IsActive).FirstOrDefault();
                if (course != null)
                {
                    course.IsActive = false;
                    _db.Courses.Update(course);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
               
            }
            return RedirectToAction("Index");

        }


        public IActionResult GetCourseById(int CourseId)
        {
            try
            {
                if (CourseId > 0)
                {
                    var coursedetail = _userHelper.GetCourse(CourseId);
                    return PartialView(coursedetail);
                }
                return Json(new { isError = true, msg = "could not find courses" });
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        public IActionResult IntiateCoursePayment(string userData, string base64)
        {
            try
            {
                if (userData != null)
                {
					var paymentModel = JsonConvert.DeserializeObject<PaymentViewModel>(userData);
                    if (paymentModel != null)
                    {
						var coursedetail = _userHelper.IntiateCoursePayment(paymentModel, User.Identity.Name, base64);
                        if (coursedetail != null)
                        {
							return Json(new 

                            {  isError = false, 

                                msg = "Payment successfully"
                            });
						}
					}
					return Json(new { isError = true, msg = "payment failed" });
                }
                return Json(new { isError = true, msg = "it's null" });
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
      
    }
}
