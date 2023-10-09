using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrudOperationsPartialView.Models;

namespace CrudOperationsPartialView.Controllers
{
    public class MainController : Controller
    {
        RamyaEntities db = new RamyaEntities();
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetProducts()
        {
            var list_data = db.Products.Select(x=>new { x.name,x.category,x.brand,x.price,x.rating,x.cid}).ToList();
            return Json(list_data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetGroupedProducts()
        {
            var product_list = (from a in db.Products group a by a.category into b
                                select new  {
                                category=b.Key,
                                Total=b.Sum(a=>a.price)}).ToList();
                return Json(product_list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(int id) {
            Product product = db.Products.Where(x => x.cid == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Partial() {
            var ProductsList = db.Products.ToList();
            return PartialView("_ProductsPieChart",ProductsList);
        }
        [HttpPost]
        public ActionResult Save(Product abc)
        {
            if (abc.cid==0)
            {
                db.Products.Add(abc);
                db.SaveChanges();
                return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.Entry(abc).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpGet]
        public ActionResult Edit(int id) {
             
            db.Configuration.ProxyCreationEnabled = false;
            var data = db.Products.Where(x => x.cid == id).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
    }
