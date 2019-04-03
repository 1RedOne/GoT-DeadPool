using GameOfThronePool.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfThronePool.Views.UserBonusQuestions
{
    public class MyBonusQuestionsViewComponent : ViewComponent
    {
        private readonly DeadPoolDBContext _context;

        public MyBonusQuestionsViewComponent(DeadPoolDBContext context)
        {
            _context = context;

        }

        public IViewComponentResult Invoke()
        {
            string username = HttpContext.User.Identity.Name;
            return View( _context.UserBonusQuestion.AsQueryable().
                    Where(m => m.UserName.Equals(username)).ToList());
        }
            
        }
    }

