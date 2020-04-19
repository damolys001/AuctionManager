using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AuctionDataFactory;

namespace Auction.Controllers
{
    public class HomeController : Controller
    {
        private AuctionModel db = new AuctionModel();
        public ActionResult StaffBid()
        {
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View(db.Salvages.Where(m=>m.Status==1));
        }
        
        public ActionResult Details(long? id)
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

        // GET: BidItems/Create
        
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