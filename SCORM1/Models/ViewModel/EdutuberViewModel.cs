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
        public List<EdutuberVideo> ListEdutuberVideo { get; set; }
    }
}