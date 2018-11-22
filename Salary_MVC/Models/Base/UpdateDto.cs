using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class UpdateDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}