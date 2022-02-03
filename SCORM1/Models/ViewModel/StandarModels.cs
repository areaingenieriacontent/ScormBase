using SCORM1.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCORM1.Models.ViewModel;
using SCORM1.Models.PageCustomization;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SCORM1.Models.Lms;

namespace SCORM1.Models
{
    public class BaseViewModel
    {
       
        public DateTime BlockUser { get; set; }
        public SESION Sesion { get; set; }
        public Terms_and_Conditions terms { get; set; }
        public VIDEOS TermVideos { get; set; }
        public ROLES ActualRole { get; set; }
        public List<Banner> Banners { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public string UserLog { get; set; }

        public string FileTerms { get; set; }
        public String Logo { get; set; }
        public string ColorBarraSup { get; set; }
        public string ColorIconos { get; set; }
        public string ColorTextos { get; set; }
        public string ColorBoton { get; set; }
        public string ColorTextBtn { get; set; }
        public string ColorMenu { get; set; }
        public string ColorTextMenu { get; set; }
        public string TituloFooter { get; set; }
        public string ColortituloIndex { get; set; }
        public string UrlImgMesaServicio { get; set; }
        public string UrlLogoHeader { get; set; }
        public string LinkSitioWeb { get; set; }
        public string userId { get; set; }
        public List<Enrollment> listenrrolment { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


    }

}