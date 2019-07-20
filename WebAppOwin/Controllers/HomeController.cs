using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppOwin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page. ApplicationInsight";

            var v = HttpRuntime.TargetFramework;
            System.Diagnostics.Trace.TraceInformation("Information > Framework" + v.ToString());
            System.Diagnostics.Trace.TraceWarning("Warning > About");
            System.Diagnostics.Trace.TraceError("Error > About");

           // throw new FormatException();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";


            System.Diagnostics.Trace.TraceInformation("Information > Contact");
            System.Diagnostics.Trace.TraceWarning("Warning > Contact");
            System.Diagnostics.Trace.TraceError("Error > Contact");


            return View();
        }
    }
}