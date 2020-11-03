﻿using System;
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
    public class MemberController : Controller
    {
        SmartBookcaseDtbsEntities6 db = new SmartBookcaseDtbsEntities6();

        [HttpGet]
        public ActionResult AddMember()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMember(MemberInformation p1)
        {

            try
            {
                var varmi = db.MemberInformation.Where(i => i.MemberEmail == p1.MemberEmail).SingleOrDefault();
                if (varmi != null)
                {
                    return View();
                }

                db.MemberInformation.Add(p1);
                db.SaveChanges();

                MailMessage eposta = new MailMessage();
                eposta.From = new MailAddress("smartbookcase@hotmail.com");
                eposta.To.Add(p1.MemberEmail);
                eposta.Subject = "SMART-BOOKCASE";
                eposta.Body = "Sayın " + p1.MemberName + "; Kütüphanemizde üye kaydınız başarıyla oluşturulmuştur.";
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential("smartbookcase@hotmail.com", "TeamForza5");
                smtp.Port = 587;
                smtp.Host = "smtp.live.com";
                smtp.EnableSsl = true;
                smtp.Send(eposta);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult ViewMember(string p)
        {
            var degerler = from d in db.MemberInformation select d;
            if (!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(m => m.MemberName.Contains(p));
            }
            return View(degerler.ToList());
            //var uyeler = db.MemberInformation.ToList();
            //return View(uyeler);
        }

        public ActionResult DeleteMember(int id)
        {
            var a = db.MemberInformation.Find(id);
            db.MemberInformation.Remove(a);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditMember(int id)
        {
            var kisi = db.MemberInformation.Where(i => i.MemberID == id).SingleOrDefault();
            return View(kisi);
        }

        [HttpPost]
        public ActionResult EditMember(int id, MemberInformation p1)
        {
            try
            {
                var kisi = db.MemberInformation.Where(i => i.MemberID == id).SingleOrDefault();
                kisi.MemberName = p1.MemberName;
                kisi.MemberEmail = p1.MemberEmail;
                kisi.MemberPhone = p1.MemberPhone;
                kisi.MemberAdress = p1.MemberAdress;

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(p1);
            }


        }
    }
}