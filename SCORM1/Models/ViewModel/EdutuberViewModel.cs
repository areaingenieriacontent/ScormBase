using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using SCORM1.Models.Edutuber;

namespace SCORM1.Models.ViewModel
{
    public class EdutuberViewModel : BaseViewModel
    {
        public int CountLike { get; set; }
        public List<EdutuberVideo> ListEdutuberVideo { get; set; }
        public List<EdutuberLike> ListEdutuberLike { get; set; }
    }
}