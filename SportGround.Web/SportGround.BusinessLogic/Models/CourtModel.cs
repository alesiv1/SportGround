using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
    public class CourtModel
    {
		public int Id { get; set; }
	    [Required]
	    [StringLength(50, MinimumLength = 3)]
		public string Name { get; set; }
		public bool CanBooking { get; set; }
	}

    public class CourtWithWorkingHoursModel : CourtModel
    {
	    [Required]
		public List<CourtWorkingHoursModel> AllWorkingHours { get; set; }
		public bool IsAvailableDays { get; set; }
	}
}
