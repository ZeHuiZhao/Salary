using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}