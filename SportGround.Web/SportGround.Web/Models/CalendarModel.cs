using System;
using DHTMLX.Scheduler;
using SportGround.BusinessLogic.Models;

namespace SportGround.Web.Models
{
	public class CalendarModel
	{
		public DHXScheduler Scheduler { get; set; }
		public CourtWorkingDaysModel WorkingHours { get; set; }
	}
}