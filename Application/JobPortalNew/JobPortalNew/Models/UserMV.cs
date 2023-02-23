using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortalNew.Models
{
    public class UserMV
    {
        public UserMV()
        {
            Company = new CompanyMV();
        }

        public int UserID { get; set; }

        public int UserTypeID { get; set; }
        [Required(ErrorMessage ="Fill this up man *")]

        public string UserName { get; set; }
        [Required(ErrorMessage = "Fill this up man*")]

        public string Password { get; set; }
        [Required(ErrorMessage = "Fill this up man*")]

        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Fill this up man*")]

        public string ContactNo { get; set; }

        public bool AreYouProvider { get; set; }

        public CompanyMV Company { get; set; }
    }
}