using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuctionDataFactory;

namespace Auction.Controllers
{
    public class BidItemsController : Controller
    {
        private AuctionModel db = new AuctionModel();

        // GET: BidItems
        public ActionResult Index()
        {
            return View(db.Salvages.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: BidItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salvage salvage)
        {
            salvage.Status = 1;
            if (salvage.BidDate < DateTime.Now)
            {
                ModelState.AddModelError("Error", "You have selected a past date");
                return View(salvage);
            }
            if (ModelState.IsValid)
            {
                //check is another bid is available during that time
                var bidDuration = salvage.BidDate.AddHours(-1);
                var collision = db.Salvages.Where(m => m.BidDate <= salvage.BidDate && m.BidDate > bidDuration);
                if (collision.Any())
                {
                    ModelState.AddModelError("Error","There is currently a bid within the selected period");
                    return View(salvage);
                }
                db.Salvages.Add(salvage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salvage);
        }
        // GET: BidItems/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.Status = new SelectList(JUtility.DataArray.ConvertEnumToArrayList(typeof(SalvageStatus)), "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salvage salvage = db.Salvages.Find(id);
            if (salvage == null)
            {
                return HttpNotFound();
            }
            return View(salvage);
        }

        // POST: BidItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        // GET: BidItems/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Salvage salvage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salvage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = new SelectList(JUtility.DataArray.ConvertEnumToArrayList(typeof(SalvageStatus)), "Id", "Name", salvage.Status);
            return View(salvage);
        }

        // GET: BidItems/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salvage salvage = db.Salvages.Find(id);
            if (salvage == null)
            {
                return HttpNotFound();
            }
            return View(salvage);
        }

        // POST: BidItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Salvage salvage = db.Salvages.Find(id);
            db.Salvages.Remove(salvage);
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
