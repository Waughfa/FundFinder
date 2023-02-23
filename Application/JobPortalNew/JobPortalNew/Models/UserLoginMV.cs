using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortalNew.Models
{
    public class UserLoginMV
    {
        [Required(ErrorMessage = "Required*")]
        public String UserName { get; set; }
        [Required(ErrorMessage = "Required*")]
        public String Password { get; set; }
    }
}