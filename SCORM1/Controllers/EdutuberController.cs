using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCORM1.Models;
using SCORM1.Models.Edutuber;
using PagedList;
using SCORM1.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SCORM1.Models.PageCustomization;

namespace SCORM1.Controllers
{
    public class EdutuberController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public EdutuberController()
        {
            db = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.db));
        }

        // GET: Edutuber
        public ActionResult EdutuberIndex(/*string search, int? i*/)
        {
            var userId = User.Identity.GetUserId();
            EdutuberViewModel listvid = new EdutuberViewModel();

            List<EdutuberVideo> listvideo = db.EdutuberVideos.ToList();

            listvid.ColorBarraSup = GetColorBarraSup();
            listvid.ColorIconos = GetColorIconos();
            listvid.ColorTextos = GetColorTextos();
            listvid.ColorBoton = GetColorBoton();
            listvid.ColorTextBtn = GetColorTextoBtn();
            listvid.ColorMenu = GetColorMenu();
            listvid.ColorTextMenu = GetColorTextMenu();
            listvid.ListEdutuberVideo = listvideo;

            listvid.Sesion = db.Users.Find(userId).SesionUser;

            return View(listvid);

            //return View(db.EdutuberVideos.Where(z => z.EduVid_Titulo.StartsWith(search) || search == null).ToList().ToPagedList(i ?? 1, 5));
            //return View(ApplicationDbContext.EdutuberVideos.ToList());
        }

        // GET: Edutuber/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Edutuber/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Edutuber/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Edutuber");
            }
            catch
            {
                return View();
            }
        }

        // GET: Edutuber/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Edutuber/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Edutuber");
            }
            catch
            {
                return View();
            }
        }

        // GET: Edutuber/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Edutuber/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ApplicationUser GetActualUserId()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            return user;
        }
        private string GetColorBarraSup()
        {

            string ColBarraSup = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = db.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColBarraSup = CompanyValidate.navBarColor;

            }
            else
            {
                ColBarraSup = db.StylesLogos.Find(1).navBarColor;
            }

            return ColBarraSup;
        }
        private string GetColorIconos()
        {

            string ColIconos = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = db.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColIconos = CompanyValidate.colorIconos;

            }
            else
            {
                ColIconos = db.StylesLogos.Find(1).colorIconos;
            }

            return ColIconos;
        }
        private string GetColorTextos()
        {

            string ColTextos = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = db.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextos = CompanyValidate.colorTexto;

            }
            else
            {
                ColTextos = db.StylesLogos.Find(1).colorTexto;
            }

            return ColTextos;
        }
        private string GetColorBoton()
        {

            string ColBoton = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = db.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColBoton = CompanyValidate.colorBoton;

            }
            else
            {
                ColBoton = db.StylesLogos.Find(1).colorBoton;
            }

            return ColBoton;
        }
        private string GetColorTextoBtn()
        {

            string ColTextoBtn = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = db.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextoBtn = CompanyValidate.colorTextoBtn;

            }
            else
            {
                ColTextoBtn = db.StylesLogos.Find(1).colorTextoBtn;
            }

            return ColTextoBtn;
        }
        private string GetColorMenu()
        {

            string ColMenu = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = db.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColMenu = CompanyValidate.colorMenu;

            }
            else
            {
                ColMenu = db.StylesLogos.Find(1).colorMenu;
            }

            return ColMenu;
        }
        private string GetColorTextMenu()
        {

            string ColTextMenu = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = db.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextMenu = CompanyValidate.colorTextMenu;

            }
            else
            {
                ColTextMenu = db.StylesLogos.Find(1).colorTextMenu;
            }

            return ColTextMenu;
        }

    }
}
