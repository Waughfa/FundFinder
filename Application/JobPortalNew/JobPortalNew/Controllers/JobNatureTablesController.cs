using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataBaseLayer2;

namespace JobPortalNew.Controllers
{
    public class JobNatureTablesController : Controller
    {
        private jb3Entities db = new jb3Entities();

        // GET: JobNatureTables
        public ActionResult Index()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            return View(db.JobNatureTables.ToList());
        }

        // GET: JobNatureTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobNatureTable jobNatureTable = db.JobNatureTables.Find(id);
            if (jobNatureTable == null)
            {
                return HttpNotFound();
            }
            return View(jobNatureTable);
        }

        // GET: JobNatureTables/Create
        public ActionResult Create()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            return View(new JobNatureTable());
        }

        // POST: JobNatureTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobNatureID,JobNature")] JobNatureTable jobNatureTable)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                db.JobNatureTables.Add(jobNatureTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobNatureTable);
        }

        // GET: JobNatureTables/Edit/5
        public ActionResult Edit(int? id)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobNatureTable jobNatureTable = db.JobNatureTables.Find(id);
            if (jobNatureTable == null)
            {
                return HttpNotFound();
            }
            return View(jobNatureTable);
        }

        // POST: JobNatureTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobNatureID,JobNature")] JobNatureTable jobNatureTable)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                db.Entry(jobNatureTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobNatureTable);
        }

        // GET: JobNatureTables/Delete/5////////////////////////////////delete not suppose to be here
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobNatureTable jobNatureTable = db.JobNatureTables.Find(id);
            if (jobNatureTable == null)
            {
                return HttpNotFound();
            }
            return View(jobNatureTable);
        }

        // POST: JobNatureTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobNatureTable jobNatureTable = db.JobNatureTables.Find(id);
            db.JobNatureTables.Remove(jobNatureTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
