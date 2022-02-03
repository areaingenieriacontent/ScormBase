using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCORM1.Models.PageCustomization
{
    public class StylesLogos
    {
        internal string colorsBacgraundTittle;

        [Key]
        public int Styl_Id { get; set; }

        public int? companyId { get; set; }

        [AllowHtml]
        [Display(Name = "Logo")]
        public string UrlLogo { get; set; }

        [Display(Name = "Color de la Barra de navegación")]
        public string navBarColor { get; set; }

        [Display(Name = "Imagen  1")]
        [AllowHtml]
        public string UrlImage1 { get; set; }

        [Display(Name = "Imagen  2")]
        [AllowHtml]
        public string UrlImage2 { get; set; }

        [Display(Name = "Imagen  3")]
        [AllowHtml]
        public string UrlImage3 { get; set; }

        [Display(Name = "Imagen  4")]
        [AllowHtml]
        public string UrlImage4 { get; set; }

        [Display(Name = "Titulo 1")]
        public string Title1 { get; set; }
        [Display(Name = "Titulo 2")]
        public string Title2 { get; set; }
        [Display(Name = "Titulo 3")]
        public string Title3 { get; set; }
        [Display(Name = "Color de los Títulos")]
        public string colorsTittle { get; set; }
        [Display(Name = "Color de fondo del titulo ")]
        public string colorsBacgraundTitles { get; set; }
        [Display(Name = "Color Iconos")]
        public string colorIconos { get; set; }
        [Display(Name = "Color Textos")]
        public string colorTexto { get; set; }
        [Display(Name = "Color Boton")]
        public string colorBoton { get; set; }
        [Display(Name = "Color Texto Boton")]
        public string colorTextoBtn { get; set; }
        [Display(Name = "Color Menu")]
        public string colorMenu { get; set; }
        [Display(Name = "Color Texto Menu")]
        public string colorTextMenu { get; set; }
        [Display(Name = "Titulo Footer")]
        public string titulofooter { get; set; }
        [Display(Name = "Color Titulo Index")]
        public string colorTituloIndex { get; set; }
        [Display(Name = "Imagen Mesa De Servicio")]
        public string UrlImgMesaServicio { get; set; }
        [Display(Name = "Logo Header Index")]
        public string UrlLogoHeader { get; set; }
        [Display(Name = "Link Sitio Web de la Empresa")]
        public string LinkSitioWeb { get; set; }


    }


}