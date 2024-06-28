using Microsoft.AspNetCore.Mvc;
using School_Portal.Iservices;
using School_Portal.Models;
using School_Portal.Services;
using School_Portal.ViewModels;
using System.Diagnostics;

namespace School_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;
        public HomeController(ILogger<HomeController> logger,IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _logger = logger;
        }

        public IActionResult Index()
        {
           var homeViewModel = _userHelper.GetHomeData();
            return View(homeViewModel);
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Courses()
        {
            return View();
        }
    }
}
