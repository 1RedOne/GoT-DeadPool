using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOfThronePool.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameOfThronePool.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ShowCharacterRecordController : Controller
    {
        private readonly DeadPoolDBContext _context;
        // GET: /ShowCharacterRecordContoller/<controller>/
        public IActionResult Index()
        {
            return View(_context.ShowCharacterStatusRecords.ToList());
        }
    }
}
