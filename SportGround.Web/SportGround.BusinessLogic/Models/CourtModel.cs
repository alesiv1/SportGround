using System;
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
}
