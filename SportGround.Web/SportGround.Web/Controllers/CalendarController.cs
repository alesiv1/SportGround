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
	    private static int activeCourtId = -1;

		public CalendarController(IBookingService bookingServices, ICourtService services)
	    {
		    _bookingServices = bookingServices;
		    _courtServices = services;
		}

		public ActionResult Index(int? courtId)
	    {
			if(courtId != null) SetCourtId(courtId);
		    var dateTimeNow = DateTime.Now;
		    var sched = new DHXScheduler(this);
		    sched.Skin = DHXScheduler.Skins.Flat;
		    sched.LoadData = true;
		    sched.EnableDataprocessor = true;
		    sched.InitialDate = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day);
		    var data = sched.InitialValues.Values;
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
						if (!IsCourt(activeCourtId))
						{
							throw new Exception("You cant book unknow court");
						}
						changedBooking.Court.Id = activeCourtId;
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
