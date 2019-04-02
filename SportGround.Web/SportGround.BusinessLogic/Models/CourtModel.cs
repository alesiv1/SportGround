using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
    public class CourtModel
    {
	    [Required]
		public int Id { get; set; }
	    [Required]
	    [StringLength(50, MinimumLength = 3)]
		public string Name { get; set; }
	}

    public class CourtWithWorkingHoursModel : CourtModel
    {
	    [Required]
		public List<CourtWorkingHoursModel> AllWorkingHours { get; set; }
	}
}
