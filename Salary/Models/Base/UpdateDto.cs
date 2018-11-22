using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.Models.Base
{
    public class UpdateDto
    {
        [Required]
        public int Id { get; set; }
    }
}