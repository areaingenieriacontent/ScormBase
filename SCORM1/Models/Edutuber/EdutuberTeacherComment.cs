using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SCORM1.Models.Edutuber
{
    public class EdutuberTeacherComment
    {
        [Key]
        public int comment_id { get; set; }
        [Column(Order = 0)]
        [ForeignKey("Enrollment")]
        public string user_id { get; set; }
        [Column(Order = 1)]
        [ForeignKey("Enrollment")]
        public int Edutuber_id { get; set; }
        public string teacher_id { get; set; }
        [AllowHtml]
        public string content { get; set; }
        public DateTime commentDate { get; set; }
        public virtual EdutuberEnrollment Enrollment { get; set; }
    }
}