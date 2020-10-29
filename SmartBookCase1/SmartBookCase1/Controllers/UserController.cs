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
    public class UserController : Controller
    {
        SmartBookcaseDtbsEntities2 db = new SmartBookcaseDtbsEntities2();
        int PasswordCode;

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewUser(UserInformation p1)
        {
            try
            {
                var varmi = db.UserInformation.Where(i => i.UserEmail == p1.UserEmail).SingleOrDefault();
                if (varmi != null)
                {
                    return View();
                }

                string pswrd = Encrypt.MD5Create(p1.UserPassword);
                p1.UserPassword = pswrd;
                db.UserInformation.Add(p1);
                db.SaveChanges();
                Session["UserName"] = p1.UserName;
                Session["UserID"] = p1.UserID;

                MailMessage eposta = new MailMessage();
                eposta.From = new MailAddress("smartbookcase@hotmail.com");
                eposta.To.Add(p1.UserEmail);
                eposta.Subject = "SMART-BOOKCASE";
                eposta.Body = "Sayın " + p1.UserName + " Smart-BookCase Otomasyon sistemi Kullanıcı kaydınız başarıyla oluşturulmuştur.";
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

        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session["UserID"] = null;
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(UserInformation p1)
        {
             try
              {
                  var u = db.UserInformation.Where(i => i.UserEmail == p1.UserEmail).SingleOrDefault();
                  if (u == null)
                  {
                      return View();
                  }
                  string pswrd = Encrypt.MD5Create(p1.UserPassword);
                  p1.UserPassword = pswrd;
                  if (u.UserPassword == p1.UserPassword)
                  {
                      Session["UserName"] = u.UserName;
                      Session["UserID"] = u.UserID;
                    return RedirectToAction("Index", "Home");
                  }
                  else
                  {
                      return View();
                  }
              }
              catch
              {
                  return View();
              }                     
        }

        public ActionResult Profile()
        {
            int kullaniciID = (int)Session["UserID"];
            var kisi = db.UserInformation.Where(i => i.UserID == kullaniciID).SingleOrDefault();
            return View(kisi);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var kisi = db.UserInformation.Where(i => i.UserID == id).SingleOrDefault();
            return View(kisi);
        }

        [HttpPost]
        public ActionResult Edit(int id, UserInformation p1)
        {
            try
            {
                var kisi = db.UserInformation.Where(i => i.UserID == id).SingleOrDefault();
                kisi.UserName = p1.UserName;
                kisi.UserEmail = p1.UserEmail;
                kisi.UserPhone = p1.UserPhone;
                if (p1.UserPassword != null)
                {
                    string pswrd = Encrypt.MD5Create(p1.UserPassword);
                    kisi.UserPassword = pswrd;
                }             
                db.SaveChanges();
                return RedirectToAction("Profile", "User");
            }
            catch
            {
                return View(p1);
            }

        }

        [HttpGet]
        public ActionResult PasswordReset()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult PasswordReset(UserInformation p1)
        {
            var u = db.UserInformation.Where(i => i.UserEmail == p1.UserEmail).SingleOrDefault();
            if (u == null)
            {
                return View();
            }

            Random rastgele = new Random();
            PasswordCode = rastgele.Next(100000, 999999);

            MailMessage eposta = new MailMessage();
            eposta.From = new MailAddress("smartbookcase@hotmail.com");
            eposta.To.Add(p1.UserEmail);
            eposta.Subject = "SMART-BOOKCASE";
            eposta.Body = "Sayın " + u.UserName + " Smart-BookCase Otomasyon sistemi tarafından Belirlenen Yeni Şifreniz : " + PasswordCode;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("smartbookcase@hotmail.com", "TeamForza5");
            smtp.Port = 587;
            smtp.Host = "smtp.live.com";
            smtp.EnableSsl = true;
            smtp.Send(eposta);

            string pswrd = Encrypt.MD5Create(PasswordCode.ToString());
            u.UserPassword = pswrd;
            db.SaveChanges();

            return RedirectToAction("Login", "User");
        }








    }
}