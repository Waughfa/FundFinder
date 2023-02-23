using DataBaseLayer2;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace JobPortalNew.Models
{
    public class JobRequirementsMV
    {
        ///JobRequirmentsMV
        public JobRequirementsMV()
        {
            Details = new List<JobRequirementDetailTable>();
        }


        [Required(ErrorMessage = "Required*")]
        public string JobRequirementDetails { get; set; }
        [Required(ErrorMessage = "Required*")]
        public int JobRequirementID { get; set; }
        public int PostJobID { get; set; }

        /// <summary>
        /// my codes 1 line
        /// </summary>
        //public string JobRequirementTitle { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public List<JobRequirementDetailTable> Details { get; set; } 
    }
}