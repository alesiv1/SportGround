using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using SportGround.BusinessLogic.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using SportGround.BusinessLogic.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace SportGround.Web.Controllers
{
    public class CalendarController : Controller
    {
	    private IBookingService _bookingServices;

	    public CalendarController(IBookingService bookingServices)
	    {
		    _bookingServices = bookingServices;
	    }

		public ActionResult Index()
	    {
		    var dateTimeNow = DateTime.Now;
		    var sched = new DHXScheduler(this);
		    sched.Skin = DHXScheduler.Skins.Flat;
		    sched.LoadData = true;
		    sched.EnableDataprocessor = true;
		    sched.InitialDate = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day);
		    return View(sched);
	    }

		public ContentResult Data()
		{
			var allBookings = _bookingServices.GetBookingList().ToList();
			var date = new SchedulerAjaxData(allBookings
				.Select(e => new
				{
					id = e.Id,
					text = e.Court.Name,
					start_date = e.StartDate,
					end_date = e.EndDate,
					court = e.Court.Id
				}));
			return date;
		}

		public ContentResult Save(int? id, FormCollection actionValues)
		{
			var action = new DataAction(actionValues);
			var changedBooking = new CourtBookingModel()
			{
				Id = Convert.ToInt64(actionValues["id"]),
				User = new UserModel()
				{
					Id = Int32.Parse(((ClaimsIdentity)this.User.Identity).FindFirstValue("Id"))
				},
				Court = new CourtModel()
				{
					Id = Convert.ToInt32(actionValues["court"])
				},
				StartDate = Convert.ToDateTime(actionValues["start_date"]),
				EndDate = Convert.ToDateTime(actionValues["end_date"])
			};
			try
			{
				switch (action.Type)
				{
					case DataActionTypes.Insert:
						changedBooking.Court.Id = 1;  /// bed place if i must fix
						_bookingServices.Create(changedBooking);
						break;
					case DataActionTypes.Delete:
						_bookingServices.Delete(changedBooking.Id);
						break;
					default:
						_bookingServices.Update(changedBooking.Id, changedBooking);
						break;
				}
				action.TargetId = Convert.ToInt64(actionValues["id"]);
			}
			catch (Exception a)
			{
				action.Type = DataActionTypes.Error;
			}
			return (new AjaxSaveResponse(action));
		}
	}
}
