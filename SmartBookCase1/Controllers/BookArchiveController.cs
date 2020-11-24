using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartBookCase1.Models.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace SmartBookCase1.Controllers
{
    public class BookArchiveController : Controller
    {
        // GET: BookArchive
        SmartBookcaseDtbsEntitie db = new SmartBookcaseDtbsEntitie();


        [HttpGet]
        public ActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(BookArchive p1)
        {

            try
            {
                var varmi = db.BookArchive.Where(i => i.BookName == p1.BookName).SingleOrDefault();
                if (varmi != null)
                {
                    ViewBag.Message = "Girilen İsimde Kayitli bir Kitap Zaten var!! ";
                    return View();
                }

                db.BookArchive.Add(p1);
                db.SaveChanges();


                return RedirectToAction("ViewBook", "BookArchive");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult ViewBook(string p)
        {
            var degerler = from d in db.BookArchive select d;
            if (!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(m => m.BookName.Contains(p));
            }
            return View(degerler.ToList());          
        }

        public ActionResult DeleteBook(int id)
        {
            var a = db.BookArchive.Find(id);
            db.BookArchive.Remove(a);
            db.SaveChanges();

            return RedirectToAction("ViewBook", "BookArchive");
        }

        [HttpGet]
        public ActionResult EditBook(int id)
        {
            var kitap = db.BookArchive.Where(i => i.BookID == id).SingleOrDefault();
            return View(kitap);
        }

        [HttpPost]
        public ActionResult EditBook(int id, BookArchive p1)
        {
            try
            {
                var kitap = db.BookArchive.Where(i => i.BookID == id).SingleOrDefault();
                kitap.BookName = p1.BookName;
                kitap.BookCategory = p1.BookCategory;
                kitap.BookStock = p1.BookStock;
                kitap.BookAuthor = p1.BookAuthor;
                kitap.BookBarcode = p1.BookBarcode;

                db.SaveChanges();
                return RedirectToAction("ViewBook", "BookArchive");
            }
            catch
            {
                return View(p1);
            }


        }








    }
}