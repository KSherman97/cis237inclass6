using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237inclass6.Models;

namespace cis237inclass6.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private CarsKShermanEntities db = new CarsKShermanEntities();

        // GET: Cars
        public ActionResult Index()
        {
            // return View(db.Cars.ToList());

            // set up a variable to hold the cars data
            DbSet<Car> CarsToFilter = db.Cars;

            // setup some strings to hold the data that might be in the session.
            // If there is nothing in the session we can still use these variables
            // as a default value
            string filterMake = "";
            string filterMin = "";
            string filterMax = "";

            // define a min and max integer for the cylinders
            int min = 1;
            int max = 16;

            if(Session["make"] != null && !String.IsNullOrWhiteSpace((string)Session["make"]))
            {
                filterMake = (string)Session["make"];
                
            }
            if (Session["min"] != null && !String.IsNullOrWhiteSpace((string)Session["min"]))
            {
                filterMin = (string)Session["min"];
                min = Int32.Parse(filterMin);
            }
            if (Session["max"] != null && !String.IsNullOrWhiteSpace((string)Session["max"]))
            {
                filterMax = (string)Session["max"];
                max = Int32.Parse(filterMax);
            }

            // do the filter on the carsToFilter dataset. Use the where that we used before
            // when doing the last inclass, only this time send in more lamda expressions 
            // to narrow it down further. Since we setup the default values for each of the filter
            // paramaters, min, max, and filterMake, we can count on this always running with no 
            // errors
            IEnumerable<Car> filtered = CarsToFilter.Where(car => car.cylinders >= min &&
                                                                  car.cylinders <= max &&
                                                                  car.make.Contains(filterMake));

            // convert the dataset to a list now. that the query work is done on it.
            // the view is expecting a list, so we conver the database set to a list.
            IEnumerable<Car> finalFiltered = filtered.ToList();

            // place the string representation of the values that are in the session into 
            // the viewbag so that they can be retrieved and displayed on the view
            ViewBag.filterMake = filterMake;
            ViewBag.filterMin = filterMin;
            ViewBag.filterMax = filterMax;

            // return the view with the filtered selection of cars
            return View(finalFiltered);

            // this is the original
            // return View(db.Cars.ToList());

        }

        // GET: Cars/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,year,make,model,type,horsepower,cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        [HttpGet]
        // GET: Cars/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,year,make,model,type,horsepower,cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
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

        // Mark the method as post since it is reached from a form submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        // This is the Filter method
        public ActionResult Filter()
        {
            // get the form data that we sent out of
            // the request object
            // the string that is used as a key to get
            string make = Request.Form.Get("make");
            string min = Request.Form.Get("min");
            string max = Request.Form.Get("max");

            // now that we have the data pulled out from the request object
            // let's put it into the session so that 
            // other methods can have access to it
            Session["make"] = make;
            Session["min"] = min;
            Session["max"] = max;

            // Redirect to the index page
            return RedirectToAction("Index");
        }

        // dump data to json
        // handles the conversion of data to json
        // JsonRequestBehavior.AllowGet gives clients permission
        // to obtain the json data
        public ActionResult Json()
        {
            return Json(db.Cars.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
