﻿using SimpleCloudFoundry4.ViewModels;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Web.Mvc;

namespace SimpleCloudFoundry4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ConfigServerSettings()
        {
            var config = ServerConfig.Configuration;
  
            if (config != null)
            {
                var section = config.GetSection("spring:cloud:config");
                ViewBag.Enabled = section["enabled"] ?? "";
                ViewBag.Environment = section["env"] ?? "";
                ViewBag.FailFast = section["failFast"] ?? "";
                ViewBag.Label = section["label"] ?? "";
                ViewBag.Name = section["name"] ?? "";
                ViewBag.Password = section["password"] ?? "";
                ViewBag.Uri = section["uri"] ?? "";
                ViewBag.Username = section["username"] ?? "";
                ViewBag.ValidateCertificates = section["validate_certificates"] ?? "";
                ViewBag.AccessTokenUri = section["access_token_uri"] ?? "";
                ViewBag.ClientId = section["client_id"] ?? "";
                ViewBag.ClientSecret = section["client_secret"] ?? "";
            }
            else
            {

                ViewBag.Enabled = "Not Available";
                ViewBag.Environment = "Not Available";
                ViewBag.FailFast = "Not Available";
                ViewBag.Label = "Not Available";
                ViewBag.Name = "Not Available";
                ViewBag.Password = "Not Available";
                ViewBag.Uri = "Not Available";
                ViewBag.Username = "Not Available";
                ViewBag.ValidateCertificates = "Not Available";
                ViewBag.AccessTokenUri ="Not Available";
                ViewBag.ClientId = "Not Available";
                ViewBag.ClientSecret ="Not Available";
            }
            return View();
        }
        public ActionResult Reload()
        {
            if (ServerConfig.Configuration != null)
            {
                ServerConfig.Configuration.Reload();
            }

            return View();
        }

        public ActionResult ConfigServerData()
        {

            var config = ServerConfig.Configuration;
            if (config != null)
            {
                ViewBag.Bar = config["bar"] ?? "Not returned";
                ViewBag.Foo = config["foo"] ?? "Not returned";

                ViewBag.Info_Url = config["info:url"] ?? "Not returned";
                ViewBag.Info_Description = config["info:description"] ?? "Not returned";

            }

            return View();
        }
        public ActionResult CloudFoundry()
        {

            var cloudFoundryApplication = ServerConfig.CloudFoundryApplication;
            var cloudFoundryServices = ServerConfig.CloudFoundryServices;
            return View(new CloudFoundryViewModel(
                cloudFoundryApplication == null ? new CloudFoundryApplicationOptions() : cloudFoundryApplication,
                cloudFoundryServices == null ? new CloudFoundryServicesOptions() : cloudFoundryServices));
        }
    }
}
