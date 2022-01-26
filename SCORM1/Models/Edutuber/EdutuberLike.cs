using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SCORM1.Models.Edutuber
{
    public class EdutuberLike
    {
        [Key]
        [Display(Name = "EduLike Id")]
        public int EduLike_Id { get; set; }
        [Display(Name = "Fecha")]
        public DateTime EduLike_fecha { get; set; }
        [Display(Name = "Fecha")]
        public bool EduLike_Estate { get; set; }

        [ForeignKey("Video_Id")]
        [Display(Name = "Video Id")]
        public int EduVideo_id { get; set; }

        [ForeignKey("user")]
        [Display(Name = "Usuario")]
        public string user_id { get; set; }

        public EdutuberVideo Video_Id { get; set; }

        public ApplicationUser user { get; set; }
    }
}