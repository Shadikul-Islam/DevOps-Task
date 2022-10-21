using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using K8STestApp.Models;

namespace K8STestApp.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogInController
       
        public ActionResult IndexLogIn()
        {
            //ViewData["Msg"] = "Test Data";
            LoginModels md = new LoginModels();
            md.LogInID = "";
            md.Password = "";
            return View("~/Pages/LogIn.cshtml", md);
        }

        public ActionResult Index([Bind] LoginModels ad)
        {
            MyDBContext db = new MyDBContext();
            //bool isAutenticated = true;// db.LoginCheck(ad);
	    bool isAutenticated = db.LoginCheck(ad);
            if(isAutenticated)
            {
                //TempData["msg"] = "Welcome, You have successfully logged in the System!";
                //return View("~/Pages/LogIn.cshtml");
                return View("~/Pages/Index.cshtml");
            }
            else
            {
                TempData["msg"] = "Admin id or Password is wrong.!";
                return View("~/Pages/LogIn.cshtml");
            }
        }

        // GET: LogInController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LogInController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LogInController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LogInController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LogInController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LogInController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LogInController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
