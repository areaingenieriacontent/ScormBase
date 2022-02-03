using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using SCORM1.Models.Lms;
using SCORM1.Models;
using SCORM1.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SCORM1.Models.PageCustomization;

namespace SCORM1.Controllers
{
    public class ContactoController : Controller
    {
        protected ApplicationDbContext ApplicationDb { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        // GET: Contacto/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ContactoModel contact = new ContactoModel
            {                
                ColorBarraSup = GetColorBarraSup(),
                ColorIconos = GetColorIconos(),
                ColorTextos = GetColorTextos(),
                ColorBoton = GetColorBoton(),
                ColorTextBtn = GetColorTextoBtn(),
                ColorMenu = GetColorMenu(),
                ColorTextMenu = GetColorTextMenu(),
                TituloFooter = GetTituloFooter(),
                ColortituloIndex = GetTituloFooter(),
                UrlImgMesaServicio = GetUrlImgMesaServicio(),
                UrlLogoHeader = GetUrlLogoHeader(),
                LinkSitioWeb = GetLinkSitioWeb()
            };
            contact.Sesion = ApplicationDb.Users.Find(userId).SesionUser;
            return View(contact);
            
        }

        // POST: Contacto/Create
        [HttpPost]
        public ActionResult Create(string correos,string identificacion, string empresa, string nombres, string categoria, string descripcion)
        {
            ApplicationDb = new ApplicationDbContext();
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("info@bvtrainingcommunity.com");
            correo.To.Add("bureauveritassoporte@gmail.com");
            correo.Subject = categoria + identificacion;
            string caso = "El usuario " + nombres + " identicado con el numero " + identificacion + " genero un nuevo caso de soporte: ";
            correo.Body = caso + descripcion;
            correo.IsBodyHtml = false;
            correo.Priority = MailPriority.Normal;
            //
            var smtp = new SmtpClient();
        
            smtp.Send(correo);
            int count = ApplicationDb.Correos.ToList().Count();
            CorreoModel correoDB;
            if (count == 0)
            {

                correoDB = new CorreoModel
                {
                    Nombre = nombres,
                    Categoria = categoria,
                    Documento = identificacion,
                    Mensaje = descripcion,
                    Empresa = empresa,
                    IdMensaje = "1",
                    Correos=correos
                };
            }
            else
            {
                count = count + 1;
                correoDB = new CorreoModel
                {
                    Nombre = nombres,
                    Categoria = categoria,
                    Documento = identificacion,
                    Mensaje = descripcion,
                    Empresa = empresa,
                    IdMensaje = count.ToString(),
                    Correos=correos
                  
                };
            }

            ApplicationDb.Correos.Add(correoDB);
            ApplicationDb.SaveChanges();

            CreateUser(correos);
            return RedirectToAction("Index", "Home");

        }
        public ActionResult CreateUser(string correos)
        {
            ApplicationDb = new ApplicationDbContext();
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("info@bvtrainingcommunity.com");
            correo.To.Add(correos);
            correo.Subject = "Inquietud a la mesa de servicio Bureau Veritas";
            string caso = "Acabamos de recibir tu mensaje. Nuestro equipo de trabajo analizará tu solicitud para determinar el nivel de complejidad."+'\n'+
                "Te estaremos informando al correo registrado el tiempo de repuesta asignado.";
            correo.Body = caso;
            correo.IsBodyHtml = false;
            correo.Priority = MailPriority.Normal;
            //
            var smtp = new SmtpClient();

            smtp.Send(correo);

            return RedirectToAction("Index", "Home");

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
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColBarraSup = CompanyValidate.navBarColor;

            }
            else
            {
                ColBarraSup = ApplicationDb.StylesLogos.Find(1).navBarColor;
            }

            return ColBarraSup;
        }
        private string GetColorIconos()
        {

            string ColIconos = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColIconos = CompanyValidate.colorIconos;

            }
            else
            {
                ColIconos = ApplicationDb.StylesLogos.Find(1).colorIconos;
            }

            return ColIconos;
        }
        private string GetColorTextos()
        {

            string ColTextos = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextos = CompanyValidate.colorTexto;

            }
            else
            {
                ColTextos = ApplicationDb.StylesLogos.Find(1).colorTexto;
            }

            return ColTextos;
        }
        private string GetColorBoton()
        {

            string ColBoton = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColBoton = CompanyValidate.colorBoton;

            }
            else
            {
                ColBoton = ApplicationDb.StylesLogos.Find(1).colorBoton;
            }

            return ColBoton;
        }
        private string GetColorTextoBtn()
        {

            string ColTextoBtn = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextoBtn = CompanyValidate.colorTextoBtn;

            }
            else
            {
                ColTextoBtn = ApplicationDb.StylesLogos.Find(1).colorTextoBtn;
            }

            return ColTextoBtn;
        }
        private string GetColorMenu()
        {

            string ColMenu = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColMenu = CompanyValidate.colorMenu;

            }
            else
            {
                ColMenu = ApplicationDb.StylesLogos.Find(1).colorMenu;
            }

            return ColMenu;
        }
        private string GetColorTextMenu()
        {

            string ColTextMenu = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTextMenu = CompanyValidate.colorTextMenu;

            }
            else
            {
                ColTextMenu = ApplicationDb.StylesLogos.Find(1).colorTextMenu;
            }

            return ColTextMenu;
        }
        private string GetColorTituloIndex()
        {

            string ColTitIndex = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                ColTitIndex = CompanyValidate.colorTituloIndex;

            }
            else
            {
                ColTitIndex = ApplicationDb.StylesLogos.Find(1).colorTituloIndex;
            }

            return ColTitIndex;
        }
        private string GetTituloFooter()
        {

            string TituloFooter = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                TituloFooter = CompanyValidate.titulofooter;

            }
            else
            {
                TituloFooter = ApplicationDb.StylesLogos.Find(1).titulofooter;
            }

            return TituloFooter;
        }
        private string GetUrlImgMesaServicio()
        {

            string imgmesaservicio = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                imgmesaservicio = CompanyValidate.UrlImgMesaServicio;

            }
            else
            {
                imgmesaservicio = ApplicationDb.StylesLogos.Find(1).UrlImgMesaServicio;
            }

            return imgmesaservicio;
        }
        private string GetUrlLogoHeader()
        {

            string logohead = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                logohead = CompanyValidate.UrlLogoHeader;

            }
            else
            {
                logohead = ApplicationDb.StylesLogos.Find(1).UrlLogoHeader;
            }

            return logohead;
        }
        private string GetLinkSitioWeb()
        {

            string linksitioweb = "";

            var companyId = (int)GetActualUserId().CompanyId;
            StylesLogos CompanyValidate = ApplicationDb.StylesLogos.Where(x => x.companyId == companyId).FirstOrDefault();

            if (CompanyValidate != null)
            {
                linksitioweb = CompanyValidate.LinkSitioWeb;

            }
            else
            {
                linksitioweb = ApplicationDb.StylesLogos.Find(1).LinkSitioWeb;
            }

            return linksitioweb;
        }
    }
}
