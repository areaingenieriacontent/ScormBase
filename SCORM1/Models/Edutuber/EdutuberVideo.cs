using SCORM1.Models.SCORM1;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SCORM1.Models.Edutuber
{
    public class EdutuberVideo
    {
        [Key]
        public int EduVideo_id { get; set; }
        [Display (Name = "Titulo Video")]
        public string EduVid_Titulo { get; set; }
        [Display (Name = "Descripcion")]
        public string EduVid_Descri { get; set; }
        [Display(Name = "Url Video")]
        public string EduVid_UrlVideo { get; set; }
        [Display(Name = "Url Imagen portada")]
        public string EduVid_UrlImag1 { get; set; }
        [Display(Name = "Url Imagen hover")]
        public string EduVid_UrlImag2 { get; set; }
        [Display(Name = "Url Foro Experiencias")]
        public string EduVid_UrlExpe { get; set; }
        [Display(Name = "Likes")]
        public string EduVid_CountLike { get; set; }

        [ForeignKey("company")]
        [Display(Name = "Compañia/Empresa")]
        public int company_Id { get; set; }

        public Company company { get; set; }


    }
}