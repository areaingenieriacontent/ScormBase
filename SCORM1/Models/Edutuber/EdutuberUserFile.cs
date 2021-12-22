using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCORM1.Models.Edutuber
{
    public class EdutuberUserFile
    {
        [Key]
        public int Edutuber_file_id { get; set; }
        [Column(Order = 0)]
        [ForeignKey("Enrollment")]
        public string user_id { get; set; }
        [Column(Order = 1)]
        [ForeignKey("Enrollment")]
        public int Edutuber_id { get; set; }
        [Display(Name = "Título del archivo")]
        public string register_name { get; set; }
        [Display(Name = "Descripción o contenido")]
        public string file_description { get; set; }
        [Display(Name = "Archivo")]
        public string file_name { get; set; }
        [Display(Name = "Extensión")]
        public string file_extention { get; set; }
        [Display(Name = "Fecha de registro")]
        public DateTime registered_date { get; set; }
        public virtual EdutuberEnrollment Enrollment { get; set; }
    }
}