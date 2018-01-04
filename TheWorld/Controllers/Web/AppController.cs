﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IEmailService _emailService;
        private IConfigurationRoot _config;

        public AppController(IEmailService emailService, IConfigurationRoot configuration)
        {
            _emailService = emailService;
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                _emailService.SendEmail(_config["MailSettings:ToAddress"], contactViewModel.Email, "From TheWorld",
                    contactViewModel.Message);

                ModelState.Clear();
                ViewBag.Message = "Email sent. Thanks!";
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
