using DataBaseLayer2;
using JobPortalNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortalNew.Controllers
{
    public class JobController : Controller
    {
        private jb3Entities Db = new jb3Entities();
        // GET: Job
        public ActionResult PostJob()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var job = new PostJobMV();
            ViewBag.JobCategoryID = new SelectList(
                                     Db.JobCategoryTables.ToList(),
                                     "JobCategoryID",
                                     "JobCategory",
                                     "0");

            ViewBag.JobNatureID = new SelectList(
                                     Db.JobNatureTables.ToList(),
                                     "JobCategoryID",
                                     "JobNature",
                                     "0");

            return View(job);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostJob(PostJobMV postJobMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0 ;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"] ), out userid); 
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            ///to string is added new 
            postJobMV.UserID = userid;
            postJobMV.CompanyID = companyid; 
         

            if (ModelState.IsValid)
            {
                var post = new PostJobTable();
               
                ///DB           /// input
                post.UserID = postJobMV.UserID;
                post.CompanyID = postJobMV.CompanyID;
                post.JobCategoryID = postJobMV.JobCategoryID;
                post.Job_Title = postJobMV.JobTitle;
                post.Job_Description = postJobMV.JobDescription;
                post.MinSalary = postJobMV.MinSalary;
                post.MixSalary = postJobMV.MaxSalary; //// there may be spelling error no tel 
                post.Location = postJobMV.Location;
                post.Vacancey = postJobMV.Vacancy;
                post.JobNatureID = 1;
                post.PostDate = postJobMV.PostDate;
                post.ApplicationLastDate = postJobMV.ApplicationLastDate;
                post.LastDate = 2020 ;
                
                post.JobStatusID = 1;
                post.WebUrl = postJobMV.WebUrl;
                Db.PostJobTables.Add(post); 
                Db.SaveChanges();
                return RedirectToAction("CompanyJobsList");
            }

            ViewBag.JobCategoryID = new SelectList(
                                     Db.JobCategoryTables.ToList(),
                                     "JobCategoryID",
                                     "JobCategory",
                                     "0");

            ViewBag.JobNatureID = new SelectList(
                                     Db.JobNatureTables.ToList(),
                                     "JobCategoryID",
                                     "JobNature",
                                     "0");
            return View(postJobMV);
        }

        public ActionResult CompanyJobsList()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            var allpost = Db.PostJobTables.Where(c => c.CompanyID == companyid && c.UserID == userid ).ToList();

            return View(allpost);  
        }


        public ActionResult AllCompanyPendingJobs()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            var allpost = Db.PostJobTables.ToList();

            if(allpost.Count()>0)
            {
                allpost = allpost.OrderByDescending(o=>o.PostJobID).ToList();
            }

            return View(allpost);
        }

        public ActionResult ContactCompanyPendingJobs() //////////////////////////////////////////////////////////////////////
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            var allpost = Db.PostJobTables.Where(u => u.JobStatusID == 4).ToList();

            if (allpost.Count() > 0)
            {
                allpost = allpost.OrderByDescending(o => o.PostJobID).ToList();
            }

            return View(allpost);
        }




        public ActionResult AddRequirements(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var details = Db.JobRequirementDetailTables.Where(j=>j.PostJobID == id  ).ToList();
            if(details.Count() > 0)
            {
                details = details.OrderBy(r=>r.JobRequirementID).ToList();
            }
            var requirements = new JobRequirementsMV();
            requirements.Details = details;
            requirements.PostJobID = (int)id; 

            ViewBag.JobRequirementID = new SelectList(Db.JobRequirementTables.ToList(), "JobRequirementID", "JobRequirementTitle" , "0");

            return View(requirements);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddRequirements(JobRequirementsMV jobRequirementsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            try
            {
                var requirements = new JobRequirementDetailTable();
                requirements.JobRequirementID = jobRequirementsMV.JobRequirementID;
                requirements.JobRequirementDetails = jobRequirementsMV.JobRequirementDetails;
                requirements.PostJobID = jobRequirementsMV.PostJobID;
                Db.JobRequirementDetailTables.Add(requirements);
                Db.SaveChanges();
                return RedirectToAction("AddRequirements", new {id = requirements.PostJobID}); 
            }
            catch (Exception ex)
            {
                var details = Db.JobRequirementDetailTables.Where(j => j.PostJobID == jobRequirementsMV.PostJobID).ToList();
                if (details.Count() > 0)
                {
                    details = details.OrderBy(r => r.JobRequirementID).ToList();
                }
                jobRequirementsMV.Details =details;
                ModelState.AddModelError("JobRequirementID", "Required");
            }
             
            

            ViewBag.JobRequirementID = new SelectList(Db.JobRequirementTables.ToList(), "JobRequirementID", "JobRequirementTitle", jobRequirementsMV.JobRequirementID);

            return View(jobRequirementsMV);
        }

        public ActionResult DeleteRequirment(int? id)
        {


            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpostid = Db.JobRequirementDetailTables.Find(id).PostJobID;

            var requirements = Db.JobRequirementTables.Find(id);
            Db.Entry(requirements).State = System.Data.Entity.EntityState.Deleted;
            Db.SaveChanges(); 

            return RedirectToAction("AddRequirements", new {id = jobpostid});
        }

        public ActionResult ApprovedPost(int? id)
        {


            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = Db.PostJobTables.Find(id);
            jobpost.JobStatusID = 2; 
            Db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");
        }

        public ActionResult CanceledPost(int? id)
        {


            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = Db.PostJobTables.Find(id);
            jobpost.JobStatusID = 3;
            Db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");
        }


        public ActionResult DeleteJobPost(int? id)
        {


            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = Db.PostJobTables.Find(id);
            Db.Entry(jobpost).State = System.Data.Entity.EntityState.Deleted; 
            Db.SaveChanges();
            return RedirectToAction("CompanyJobsList");
        }

        
         public ActionResult DeleteJobPostFromContact(int? id)
        {


            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = Db.PostJobTables.Find(id);
            Db.Entry(jobpost).State = System.Data.Entity.EntityState.Deleted;
            Db.SaveChanges();
            return RedirectToAction("ContactCompanyPendingJobs");
        }



        public ActionResult JobDetails(int? id)
        {
            var getpostjob = Db.PostJobTables.Find(id);
            var postjob = new PostJobDetailMV();
            postjob.PostJobID = getpostjob.PostJobID;
            postjob.Company = getpostjob.CompanyTable.CompanyName;
            postjob.JobCategory = getpostjob.CompanyTable.CompanyName;
            postjob.Job_Title = getpostjob.Job_Title;
            postjob.Job_Description = getpostjob.Job_Description;
            postjob.MinSalary = getpostjob.MinSalary;
            postjob.MixSalary = getpostjob.MixSalary;
            postjob.Location = getpostjob.Location;
            postjob.Vacancey = getpostjob.Vacancey;
            postjob.JobNature = getpostjob.JobNatureTable.JobNature;
            postjob.PostDate = getpostjob.PostDate;
            postjob.ApplicationLastDate = getpostjob.ApplicationLastDate;
            postjob.WebUrl = getpostjob.WebUrl;

            getpostjob.JobRequirementDetailTables= getpostjob.JobRequirementDetailTables.OrderBy(d=>d.JobRequirementID).ToList();
            int jobrequiremntid = 0;
            var jobrequirements = new JobRequirementMV();
           
            foreach (var detail in getpostjob.JobRequirementDetailTables)
            {
                var jobrequirementsdetails = new JobRequirementDetailMV();
                if (jobrequiremntid == 0)
                {
                    jobrequirements.JobRequirementID = detail.JobRequirementID;
                    jobrequirements.JobRequirementTitle = detail.JobRequirementTable.JobRequirementTitle;
                    jobrequirementsdetails.JobRequirementID= detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequiremntid = detail.JobRequirementID;
                }
                else if(jobrequiremntid == detail.JobRequirementID)
                {
                    jobrequirementsdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequiremntid = detail.JobRequirementID;
                }
                else if(jobrequiremntid != detail.JobRequirementID)
                {
                    postjob.Requirements.Add(jobrequirements);
                    jobrequirements = new JobRequirementMV();   
                    jobrequirements.JobRequirementID = detail.JobRequirementID;
                    jobrequirements.JobRequirementTitle = detail.JobRequirementTable.JobRequirementTitle;
                    jobrequirementsdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequiremntid = detail.JobRequirementID;
                }
            }

            postjob.Requirements.Add(jobrequirements);


            return View(postjob);
        }

        public ActionResult FilterJob()
        {
            var obj = new FilterJobMV();

            var date = DateTime.Now.Date;
            var result = Db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID == 2).ToList();

            obj.Result = result;

            ViewBag.JobCategoryID = new SelectList(
                                     Db.JobCategoryTables.ToList(),
                                     "JobCategoryID",
                                     "JobCategory",
                                     "0");

            ViewBag.JobNatureID = new SelectList(
                                     Db.JobNatureTables.ToList(),
                                     "JobCategoryID",
                                     "JobNature",
                                     "0");

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterJob(FilterJobMV filterJobMV)
        {
            var date = DateTime.Now.Date;
            var result = Db.PostJobTables.Where(r=>r.ApplicationLastDate >= date && r.JobStatusID == 2 &&( r.JobCategoryID == filterJobMV.JobCategoryID || r.JobNatureID == filterJobMV.JobNatureID)).ToList();

            filterJobMV.Result = result;

            ViewBag.JobCategoryID = new SelectList(
                                     Db.JobCategoryTables.ToList(),
                                     "JobCategoryID",
                                     "JobCategory",
                                     filterJobMV.JobCategoryID);

            ViewBag.JobNatureID = new SelectList(
                                     Db.JobNatureTables.ToList(),
                                     "JobCategoryID",
                                     "JobNature",
                                     filterJobMV.JobNatureID);

            return View(filterJobMV);
        }



        public ActionResult FilterJobForHomePage()
        {
            var obj = new FilterJobMV();

            var date = DateTime.Now.Date;
            var result = Db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID == 2).ToList();

            obj.Result = result;

            ViewBag.JobCategoryID = new SelectList(
                                     Db.JobCategoryTables.ToList(),
                                     "JobCategoryID",
                                     "JobCategory",
                                     "0");

            ViewBag.JobNatureID = new SelectList(
                                     Db.JobNatureTables.ToList(),
                                     "JobCategoryID",
                                     "JobNature",
                                     "0");

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterJobForHomePage(FilterJobMV filterJobMV)
        {
            var date = DateTime.Now.Date;
            var result = Db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID == 2 && (r.JobCategoryID == filterJobMV.JobCategoryID || r.JobNatureID == filterJobMV.JobNatureID)).ToList();

            filterJobMV.Result = result;

            ViewBag.JobCategoryID = new SelectList(
                                     Db.JobCategoryTables.ToList(),
                                     "JobCategoryID",
                                     "JobCategory",
                                     filterJobMV.JobCategoryID);

            ViewBag.JobNatureID = new SelectList(
                                     Db.JobNatureTables.ToList(),
                                     "JobCategoryID",
                                     "JobNature",
                                     filterJobMV.JobNatureID);

            return View(filterJobMV);
        }



        [HttpPost]
        public ActionResult applyNow(String _id)
        {
            

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
                //action  controller
            }

            if (Convert.ToString(Session["UserTypeID"]) == "2")
            {
                return RedirectToAction("Login", "User");
                ///making sure no user ccc can access accept post
            }


            if (_id == null)
            {
                return null; 
            }
            var id = Convert.ToInt32(_id);
            var result = Db.PostJobTables.Where(r=> r.PostJobID== id).First();
            result.JobStatusID = 4;

            Db.SaveChanges();

            return RedirectToAction("FilterJob");
        }




    }
}
