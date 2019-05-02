using DHTMLX.Scheduler;
using System;
using System.Web.Mvc;

namespace SportGround.Web.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult Index()
        {
			var sched = new DHXScheduler(this);
			sched.Skin = DHXScheduler.Skins.Terrace;
			sched.LoadData = true;
			sched.EnableDataprocessor = true;
			sched.InitialDate = new DateTime(2016, 5, 5);
			return View(sched);
		}
    }
}
