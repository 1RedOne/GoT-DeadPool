﻿using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfThronePool.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<Response> EasySendEmailWait(string email, string subject, string message);
        string CheckKey();
    }
}
