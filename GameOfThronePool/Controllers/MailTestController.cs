using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameOfThronePool.Services;
using Microsoft.AspNetCore.Http;

namespace GameOfThronePool.Controllers
{
    public class MailTestController : Controller

    {
        private string[] admins = new string[] { "stephen@foxdeploy.com", "sred13@gmail.com" };
        private readonly IEmailSender _emailSender;

        public MailTestController(
            IEmailSender emailSender
            )
        {
            _emailSender = emailSender;

        }
        [Authorize]
        public IActionResult Index()
        {
            string username = HttpContext.User.Identity.Name;
            
            if (!(admins.Contains(username)))
            {
                return Unauthorized();
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<JsonResult> Send(string toEmail, string Subject, string Body){
            string username = HttpContext.User.Identity.Name;

            if (!(admins.Contains(username)))
            {
                return Json(new {
                status = "Forbidden"
                });
            }

            await _emailSender.SendEmailConfirmationAsync("sred13@gmail.com", "https://www.foxdeploy.com");
            var response = await _emailSender.EasySendEmailWait(toEmail, Subject, Body);
            string key = _emailSender.CheckKey();
            //await _emailSender.SendEmailAsync("sred13@gmail.com", "GoT-NewUser: " + Subject, Body);

            return Json(new
            {
                fromEmail = "sred13@gmail.com",
                toEmail = toEmail,
                Subject = Subject,
                Body = Body,
                Status = response.StatusCode.ToString(),
                Key = key

            }
                );
        }
    }
}