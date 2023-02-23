using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortalNew.Models
{
    public class PostJobDetailMV
    {
        public PostJobDetailMV()
        {
            Requirements = new List<JobRequirementMV>();
        }
        public int PostJobID { get; set; }
        public string Company { get; set; }
        public string JobCategory { get; set; }
        public string Job_Title { get; set; }
        public string Job_Description { get; set; }
        public int MinSalary { get; set; }
        public int MixSalary { get; set; }
        public string Location { get; set; }
        public int Vacancey { get; set; }
        public string JobNature { get; set; }
        public System.DateTime PostDate { get; set; }
        public System.DateTime ApplicationLastDate { get; set; }
        public int JobRequirementID { get; set; }
        public string WebUrl { get; set; }

        public List<JobRequirementMV> Requirements { get; set; }



    }
}