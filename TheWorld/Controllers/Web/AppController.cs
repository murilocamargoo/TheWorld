using Microsoft.AspNetCore.Mvc;
using System;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IEmailService _emailService;

        public AppController(IEmailService emailService)
        {
            _emailService = emailService;
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
            _emailService.SendEmail("murilo.ccamargo@gmail.com", contactViewModel.Email, "From TheWorld", contactViewModel.Message);
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
