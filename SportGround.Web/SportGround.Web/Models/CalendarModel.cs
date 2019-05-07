using DHTMLX.Scheduler;
using System;
using System.Collections.Generic;

namespace SportGround.Web.Models
{
	public class CalendarModel
	{
		public string Courts { get; set; }
		public DHXScheduler Scheduler { get; set; }
	}
}