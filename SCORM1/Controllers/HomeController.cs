﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SCORM1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SCORM1.Models.Newspaper;
using SCORM1.Enum;
using SCORM1.Models.SCORM1;
using SCORM1.Models.PageCustomization;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Mail;
using SCORM1.Models.ViewModel;

namespace SCORM1.Controllers
{

    public class HomeController : Controller
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }
        private ApplicationSignInManager _signInManager;

        public HomeController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public HomeController(ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
            this.ApplicationDbContext = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public IEnumerable<SelectListItem> GetModule()
        {

            var Modules = ApplicationDbContext.Modules.ToList();
            IEnumerable<SelectListItem> Cursos = Modules.Select(x =>
           new SelectListItem
           {
               Value = x.Modu_Id.ToString(),
               Text = x.Modu_Name
           });
            return new SelectList(Cursos, "Value", "Text");
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = UserManager.FindById(userId);
                if (user.SesionUser == SESION.No)
                {
                    TempData["Info"] = "Ya has iniciado sesión con esta cuenta. Todas las sesiones actuales se cerrarán por seguridad.";
                    if (GetActualUserId().CompanyId != null)
                    {
                        string a = GetActualUserId().Company.CompanyName;
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    indexMain model = new indexMain();
                    model.Home = new Models.HomeViewModels { ActualRole = user.Role };
                    model.Login = new LoginViewModel();
                    model.TermsandConditions = new Models.HomeViewModels { terms = user.TermsandConditions, TermVideos = user.Videos };
                    model.Sesion = user.SesionUser;
                    StylesLogos CompanyLogo = ApplicationDbContext.StylesLogos.Where(x => x.companyId == user.CompanyId).FirstOrDefault();
                    if (CompanyLogo != null)
                    {
                        model.Logo = CompanyLogo.UrlLogo;
                    }
                    else
                    {
                        model.Logo = ApplicationDbContext.StylesLogos.Find(1).UrlLogo;
                       
                    }
                    return View(model);
                }
            }
            else
            {
                indexMain model = new indexMain();
                model.Login = new LoginViewModel();      
                model.Login = new LoginViewModel();
                model.Form = new FormViewModel();
                model.Sesion = SESION.Si;
                model.Form.ListModule = GetModule();

                return View(model);
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ApplicationUser GetActualUserId()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            return user;
        }

        //public ActionResult ProfileAdminInformation()
        //{
        //    Edition EditionToShow = ApplicationDbContext.Editions.Where(x => x.Edit_StateEdition == EDITIONSTATE.Activo).First();
        //    List<Article> ListArticlesToSend = ApplicationDbContext.Articles.Where(x => x.Section.Edition.Edit_Id == EditionToShow.Edit_Id).ToList();
        //    ROLES Role = GetActualUserId().Role;
        //    AdminInformationHomeViewModel model = new AdminInformationHomeViewModel { EditionCurrentToActive = EditionToShow, ActualRole = Role, ListArticles = ListArticlesToSend };
        //    int CompanyId = (int)GetActualUserId().CompanyId;
        //    string CompanyLogo = ApplicationDbContext.StylesLogos.Where(x => x.companyId == CompanyId).FirstOrDefault().UrlLogo;
        //    model.Logo = CompanyLogo;
        //    model.Sesion = GetActualUserId().SesionUser;
        //    return View(model);
        //}

        public ActionResult Company(string company)
        {
            List<Company> companiesActuality = ApplicationDbContext.Companies.ToList();
            Company CompanySelect = new Company();
            try
            {
                CompanySelect = companiesActuality.Where(x => x.CompanyName.Contains(company)).First();

            }
            catch (Exception)
            {

                throw;
            }
            indexMain model = new indexMain();

            StylesLogos CompanySearch = new StylesLogos();
            if (CompanySelect != null)
            {
                List<StylesLogos> compa = ApplicationDbContext.StylesLogos.Where(y => y.companyId == CompanySelect.CompanyId).ToList();
                if (compa != null && compa.Count != 0)
                {
                    CompanySearch = ApplicationDbContext.StylesLogos.Where(y => y.companyId == CompanySelect.CompanyId).First();
                }
                else
                {
                    CompanySearch = ApplicationDbContext.StylesLogos.Find(1);
                }
            }
            else
            {
                CompanySearch = ApplicationDbContext.StylesLogos.Find(1);
            }
            model.UrlImage1 = CompanySearch.UrlImage1;
            model.UrlImage2 = CompanySearch.UrlImage2;
            model.UrlImage3 = CompanySearch.UrlImage3;
            model.UrlImage4 = CompanySearch.UrlImage4;
            model.navBarColor = CompanySearch.navBarColor;
            model.UrlLogo = CompanySearch.UrlLogo;
            model.Login = new LoginViewModel();
            model.Sesion = SESION.Si;
            return View("Index", model);
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult loginExterno(string id)
        {

            string resultado = string.Empty;
            byte[] decryted = Convert.FromBase64String(id);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            resultado = System.Text.Encoding.Unicode.GetString(decryted);
            LoginViewModel model = new LoginViewModel();
            model.Email = resultado;
            model.Password = resultado;
            model.RememberMe = true;
            var result = SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            if (result.Result == SignInStatus.Success)
            {
                ApplicationUser UserCurrent = UserManager.FindByName(model.Email);
                if (UserCurrent.firstAccess == null)
                {
                    UserCurrent.firstAccess = DateTime.Now;
                }
                UserCurrent.lastAccess = DateTime.Now;
                Session["FirstName"] = UserCurrent.FirstName;
                Session["LastName"] = UserCurrent.LastName;
                UserManager.Update(UserCurrent);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return RedirectToAction("Index", "Home");

            }

        }
        public ActionResult ComunidadSocial(UserProfileViewModel model)
        {
            var ComunidadActiva = ApplicationDbContext.Users.Find(GetActualUserId().Id).ComunidadActiva;
            model.ComunidadActiva = ComunidadActiva;
            var hasCliente = ApplicationDbContext.Users.Find(GetActualUserId().Id).hasClientProfile;
            var GetModuleCompany = GetActualUserId().CompanyId;
            try
            {
                //En EditionToShow se realiza la consulta para verificar que la empresa a la que corresponde el usuario logeado,
                //tenga un registro en la tabla edition, section y article, que correspondan al CompanyID
                Edition EditionToShow = ApplicationDbContext.Editions.Where(x => x.Edit_StateEdition == EDITIONSTATE.Activo && x.CompanyId == GetModuleCompany).First();
                List<Article> ListArticlesToSend = ApplicationDbContext.Articles.Where(x => x.Section.Edition.Edit_Id == EditionToShow.Edit_Id).ToList();
                var GetUserProfile = GetActualUserId().Id;
                model = new UserProfileViewModel
                {
                    ActualRole = GetActualUserId().Role,
                    EditionCurrentToActive = EditionToShow,
                    ListArticles = ListArticlesToSend,
                    Logo = GetUrlLogo(),
                    ColorBarraSup = GetColorBarraSup(),
                    ColorIconos = GetColorIconos(),
                    ColorTextos = GetColorTextos(),
                    ColorBoton = GetColorBoton(),
                    ColorTextBtn = GetColorTextoBtn(),
                    ColorMenu = GetColorMenu(),
                    ColorTextMenu = GetColorTextMenu(),
                    Form = new FormViewModel(),
                    ComunidadActiva = ComunidadActiva,
                    hasClientProfile = GetActualUserId().Company.hasClientProfile,
                    ServVideoteca = GetActualUserId().Company.ServVideoteca,
                    ServJuegos = GetActualUserId().Company.ServJuegos,
                    ServRevista = GetActualUserId().Company.ServRevista,
                    ServBiblioteca = GetActualUserId().Company.ServBiblioteca,
                    ServABE = GetActualUserId().Company.ServABE,
                    ServVSDR = GetActualUserId().Company.ServVSDR,
                    ServCafeteria = GetActualUserId().Company.ServCafeteria,
                    ServEdutuber = GetActualUserId().Company.ServEdutuber
                };
                model.Form.ListModule = GetModule();
            }
            catch (Exception)
            {

            }
            return View(model);
        }
        public ActionResult TableroControl()
        {
            return View();
        }
        private string GetUrlLogo()
        {
            var companyId = (int)GetActualUserId().CompanyId;
            string Logo = "";
            StylesLogos CompanyLogo = ApplicationDbContext.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();
            if (CompanyLogo != null)
            {
                Logo = CompanyLogo.UrlLogo;
            }
            else
            {
                Logo = ApplicationDbContext.StylesLogos.Find(1).UrlLogo;
            }
            return Logo;
        }
        private string GetColorBarraSup()
        {

            string ColBarraSup = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDbContext.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColBarraSup = CompanyValidate.navBarColor;

            }
            else
            {
                ColBarraSup = ApplicationDbContext.StylesLogos.Find(1).navBarColor;
            }

            return ColBarraSup;
        }
        private string GetColorIconos()
        {

            string ColIconos = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDbContext.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColIconos = CompanyValidate.colorIconos;

            }
            else
            {
                ColIconos = ApplicationDbContext.StylesLogos.Find(1).colorIconos;
            }

            return ColIconos;
        }
        private string GetColorTextos()
        {

            string ColTextos = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDbContext.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextos = CompanyValidate.colorTexto;

            }
            else
            {
                ColTextos = ApplicationDbContext.StylesLogos.Find(1).colorTexto;
            }

            return ColTextos;
        }
        private string GetColorBoton()
        {

            string ColBoton = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDbContext.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColBoton = CompanyValidate.colorBoton;

            }
            else
            {
                ColBoton = ApplicationDbContext.StylesLogos.Find(1).colorBoton;
            }

            return ColBoton;
        }
        private string GetColorTextoBtn()
        {

            string ColTextoBtn = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDbContext.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextoBtn = CompanyValidate.colorTextoBtn;

            }
            else
            {
                ColTextoBtn = ApplicationDbContext.StylesLogos.Find(1).colorTextoBtn;
            }

            return ColTextoBtn;
        }
        private string GetColorMenu()
        {

            string ColMenu = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDbContext.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColMenu = CompanyValidate.colorMenu;

            }
            else
            {
                ColMenu = ApplicationDbContext.StylesLogos.Find(1).colorMenu;
            }

            return ColMenu;
        }
        private string GetColorTextMenu()
        {

            string ColTextMenu = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDbContext.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextMenu = CompanyValidate.colorTextMenu;

            }
            else
            {
                ColTextMenu = ApplicationDbContext.StylesLogos.Find(1).colorTextMenu;
            }

            return ColTextMenu;
        }
    }
}