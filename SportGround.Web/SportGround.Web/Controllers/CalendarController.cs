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
	    private ICourtService _courtServices;
	    private ICourtWorkingDaysService _courtWorkingDaysServices;
		private static int activeCourtId = -1;

		public CalendarController(IBookingService bookingServices, ICourtService services, ICourtWorkingDaysService courtWorkingDaysServices)
	    {
		    _bookingServices = bookingServices;
		    _courtServices = services;
		    _courtWorkingDaysServices = courtWorkingDaysServices;
	    }

		public ActionResult Index(int? courtId)
		{
			CourtWorkingDaysModel workingHours = null;
			if (courtId != null)
			{
				SetCourtId(courtId);
				workingHours = _courtWorkingDaysServices.GetWorkingDaysForCourt(courtId ?? -1).FirstOrDefault();
			}
		    var dateTimeNow = DateTime.Now;
		    var sched = new DHXScheduler(this)
		    {
			    Skin = DHXScheduler.Skins.Flat,
			    LoadData = true,
			    InitialDate = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day),
			};
			if (Request.IsAuthenticated)
				sched.EnableDataprocessor = true;
			else
				sched.Config.isReadonly = true;
			sched.LoadData = true;
			sched.Extensions.Add(SchedulerExtensions.Extension.Collision);
		    sched.Extensions.Add(SchedulerExtensions.Extension.Limit);
		    sched.Config.first_hour = workingHours != null ? workingHours.StartTime.Hour : 8;
		    sched.Config.last_hour = workingHours != null ? workingHours.EndTime.Hour : 21;
		    sched.PreventCache();
			return View(sched);
		}

		public ContentResult Data()
		{
			var userId = Int32.Parse(((ClaimsIdentity) this.User.Identity).FindFirstValue("Id"));
			var allBookings = this.User.IsInRole("Admin") 
				? _bookingServices.GetBookingList().ToList()
				: _bookingServices.GetBookingList().Where(book => book.User.Id == userId).ToList();
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
			var courtId = String.IsNullOrEmpty(actionValues["court"])
				? activeCourtId
				: Convert.ToInt32(actionValues["court"]);
			var action = new DataAction(actionValues);
			var booking = new CourtBookingModel()
			{
				Id = Convert.ToInt64(actionValues["id"]),
				User = new UserModel()
				{
					Id = Int32.Parse(((ClaimsIdentity)this.User.Identity).FindFirstValue("Id"))
				},
				Court = new CourtModel()
				{
					Id = courtId
				},
				StartDate = Convert.ToDateTime(actionValues["start_date"]),
				EndDate = Convert.ToDateTime(actionValues["end_date"])
			};
			try
			{
				switch (action.Type)
				{
					case DataActionTypes.Insert:
						if (!IsCourt(activeCourtId) || booking.StartDate.Day < DateTime.UtcNow.Day)
						{
							throw new Exception("You cant book unknow court");
						}
						_bookingServices.Create(booking);
						break;
					case DataActionTypes.Delete:
						_bookingServices.Delete(booking.Id);
						break;
					default:
						if (booking.StartDate.Day < DateTime.UtcNow.Day)
						{
							throw new Exception("This day is unvalid!");
						}
						_bookingServices.Update(booking.Id, booking);
						break;
				}
				action.TargetId = Convert.ToInt64(actionValues["id"]);
			}
			catch
			{
				action.Type = DataActionTypes.Error;
			}
			return (new AjaxSaveResponse(action));
		}

		private void SetCourtId(int? courtId)
		{
			var isCourt = IsCourt(courtId);
				activeCourtId = isCourt ? (courtId ?? -1 ) : -1;
		}

		private bool IsCourt(int? courtId)
		{
			return courtId != -1 ? _courtServices.GetCourtList().Any(c => c.Id == courtId) : false;
		}
	}
}
