using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameOfThronePool.Models;
using GameOfThronePool.Data;

namespace GameOfThronePool.Controllers
{
    public class HomeController : Controller
    {
        private readonly DeadPoolDBContext _context;
        public HomeController(DeadPoolDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["Kingdoms"] = _context.Users.ToList().Count;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
