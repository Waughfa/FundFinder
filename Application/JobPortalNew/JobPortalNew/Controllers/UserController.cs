using DataBaseLayer2;
using JobPortalNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobPortalNew.Controllers
{
    public class UserController : Controller
    {
        private jb3Entities Db = new jb3Entities();
        // GET: User
        public ActionResult NewUser()
        {
            return View(new UserMV());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserMV userMV)
        {
            if(ModelState.IsValid)
            {
                var checkuser = Db.UserTables.Where(u=> u.EmailAddress == userMV.EmailAddress).FirstOrDefault();
                if(checkuser!= null)
                {
                    ModelState.AddModelError("Email Address", "Email is Already Resgistered");
                    return View(userMV);    
                }
                checkuser = Db.UserTables.Where(u => u.UserName == userMV.UserName).FirstOrDefault();
                if (checkuser != null)
                {
                    ModelState.AddModelError("Email UserName", "UserName is Already Resgistered");
                    return View(userMV);
                }

                using (var trans = Db.Database.BeginTransaction())
                {

                    try
                    {
                        var user = new UserTable();
                        user.UserName = userMV.UserName;
                        user.Password = userMV.Password;
                        user.ContactNo = userMV.ContactNo;
                        user.EmailAddress = userMV.EmailAddress;
                        user.UserTypeID = userMV.AreYouProvider == true ? 2 : 3;

                        Db.UserTables.Add(user);
                        Db.SaveChanges();
                        if (userMV.AreYouProvider == true)
                        {
                            var company = new CompanyTable();
                            company.UserID = user.UserID;

                            if(string.IsNullOrEmpty(userMV.Company.EmailAddress))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.EmailAddress", "Required*");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.CompanyName))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.CompanyName", "Required*");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.ContactNo))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.ContactNo", "Required*");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.Description))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.Description", "Required*");
                                return View(userMV);
                            }

                            company.EmailAddress = userMV.Company.EmailAddress;
                            company.CompanyName = userMV.Company.CompanyName;
                            company.ContactNo = userMV.ContactNo;
                            company.Logo = "~/Content/assets/img/logo/logo.png";
                            company.Description = userMV.Company.Description;
                            Db.CompanyTables.Add(company);
                            Db.SaveChanges();

                        }

                        trans.Commit();
                        return RedirectToAction("Login");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(String.Empty, "Please Enter Correct Details!!");
                        trans.Rollback();
                    }

                }

            }
            return View(userMV);
        }

        public ActionResult Login()
        {
            return View(new UserLoginMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(UserLoginMV userLoginMV)
        {
            if (ModelState.IsValid) 
            {
                var user = Db.UserTables.Where(u => u.UserName == userLoginMV.UserName && u.Password == userLoginMV.Password).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "UserName or Password is Incorrect Details Maaan!");
                    return View(userLoginMV);
                }
                Session["UserID"] = user.UserID;
                Session["Username"] = user.UserName;
                Session["UserTypeID"] = user.UserTypeID;
                if(user.UserTypeID == 2)
                {
                    Session["CompanyID"] = user.CompanyTables.FirstOrDefault().CompanyID;
                }

                return RedirectToAction("Index" , "Home");

            }
            return View(userLoginMV);
        }

        public ActionResult Logout()
        {
            Session["UserID"] = string.Empty;
            Session["Username"] = string.Empty;
            Session["CompanyID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            return RedirectToAction("Index" , "Home");
        }

        public ActionResult AllUsers()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var users = Db.UserTables.ToList();
            return View(users);
        }
    }
}