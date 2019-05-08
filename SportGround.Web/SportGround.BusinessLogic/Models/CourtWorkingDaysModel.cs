﻿using SportGround.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
	public class CourtWorkingDaysModel
	{
		public int Id { get; set; }
		public CourtModel Court { get; set; }
		public DaysOfTheWeek Day { get; set; }
		public List<DaysOfTheWeek> AvailableDays { get; set; }
		[DataType(DataType.DateTime)]
		public DateTimeOffset StartTime { get; set; }
		[DataType(DataType.DateTime)]
		public DateTimeOffset EndTime { get; set; }
	}
}
