using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheWorld.Context;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IEmailService _emailService;
        private IConfigurationRoot _config;
        private IWorldRepository _repository;
        private ILogger<AppController> _logger;

        public AppController(IEmailService emailService, IConfigurationRoot configuration, IWorldRepository repository, ILogger<AppController> logger)
        {
            _emailService = emailService;
            _config = configuration;
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var data = _repository.GetAllTrips();

                return View(data);
            }
            catch (Exception e)
            {
                _logger.LogError($"Falha ao recuperar as viagens na página inicial:{e.Message}");
                return Redirect("/error");
            }
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
