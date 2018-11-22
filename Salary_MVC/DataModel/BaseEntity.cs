using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Salary_MVC.DataModel
{
    public class BaseEntity
    {
        protected static readonly Common.CryptographyHandler pass = new Common.CryptographyHandler();

        [Key]
        public Guid Id { get; set; }
    }
}