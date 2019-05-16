using FluentValidation.Attributes;
using SportGround.BusinessLogic.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
	[Validator(typeof(CourtValidation))]
	public class CourtModel
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public bool CanBooking { get; set; }
	}

    public class CourtWithWorkingDaysModel : CourtModel
    {
	    [Required]
		public List<CourtWorkingDaysModel> AllWorkingDays { get; set; }

		public bool IsWorkingDays { get; set; }
	}
}
