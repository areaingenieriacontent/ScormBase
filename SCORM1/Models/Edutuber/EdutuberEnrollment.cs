using SCORM1.Models.SCORM1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCORM1.Models.Edutuber
{
    public class EdutuberEnrollment
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("User")]
        public string user_id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Edutuber")]
        public int Edutuber_id { get; set; }
        [Display(Name = "Fecha de matrícula")]
        public DateTime Edutuber_enro_init_date { get; set; }
        [Display(Name = "Fecha de finalización")]
        public DateTime Edutuber_enro_finish_date { get; set; }
        [Display(Name = "Calificación")]
        public float qualification { get; set; }
        public virtual EdutuberSession Edutuber { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}