using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;
using SCORM1.Models.SCORM1;
using SCORM1.Models.Edutuber;

namespace SCORM1.Models.ViewModel
{
    public class EdutuberUserVM : BaseViewModel
    {
        public EdutuberSession actualEdutuber { get; set; }
        public List<EdutuberSession> listOfEdutuber { get; set; }
        public List<EdutuberUserFile> listOfIssuedFiles { get; set; }
        public List<EdutuberTeacherComment> listOfComments { get; set; }
        public List<ApplicationUser> listOfUsers { get; set; }
        public List<EdutuberEnrollment> ListEnrollment { get; set; }
        public List<ApplicationUser> ListTrainer { get; set; }
        public string teacherName { get; set; }
        public string teacherLastName { get; set; }
        public string idUserdos { get; set; }
        public bool meetingAvailable { get; set; }
        public int Id_Edutuber { get; set; }
        public string User { get; set; }


        /*file upload variables*/
        public EdutuberUserFile EdutuberFileToAdd { get; set; }
        public EdutuberTeacherComment EdutuberTeacherComment { get; set; }

        public EdutuberEnrollment EdutuberEnrollment { get; set; }
        public EdutuberSession session { get; set; }
    }
    public class CreateEdutuberSession : BaseViewModel
    {
        public EdutuberSession actualEdutuber1 { get; set; }
        public EdutuberEnrollment edutuberEnrollment { get; set; }
        public List<EdutuberEnrollment> listEnrollment { get; set; }
        public List<EdutuberSession> listOfEdutuber { get; set; }
        public List<UserAndMassiveManagementViewModel> listUser { get; set; }
        public IPagedList<ApplicationUser> UserOfCompany { get; set; }
        public List<ApplicationUser> listUser123 { get; set; }
        public IPagedList<EdutuberEnrollment> ListEdutuber { get; set; }
        public string SearchEdutuber { get; set; }
        public int Id_Edutuber { get; set; }
        public string User { get; set; }
        public EdutuberSession session { get; set; }
        [Display(Name = "Areas")]
        public int Area_Id { get; set; }
        [Display(Name = "Cargos")]
        public int Posi_Id { get; set; }
        [Display(Name = "Ciudades")]
        public int Cyty_Id { get; set; }
        [Display(Name = "Ubicaciones")]
        public int Loca_Id { get; set; }
        public List<AllUserEdutuber> ListAllUser { get; set; }
        public List<AreasEdutuber> ListAreas { get; set; }
        public List<PositionsEdutuber> ListPositions { get; set; }
        public List<CitiesEdutuber> ListCitices { get; set; }
        public List<LocationsEdutuber> ListLocations { get; set; }
        public List<user> ListUserCancel { get; set; }
        public List<user1> ListUserCancel1 { get; set; }
        public List<user2> ListUserCancel2 { get; set; }
    }
    public class AllUserEdutuber : BaseViewModel
    {
        public int Edutuber_Id { get; set; }
        public string User_Id { get; set; }
    }
    public class AreasEdutuber : BaseViewModel
    {
        public int Area_Id { get; set; }
        public int Edutuber_Id { get; set; }
        public List<Area> Listareas { get; set; }
    }
    public class PositionsEdutuber : BaseViewModel
    {
        public int Posi_Id { get; set; }
        public int Edutuber_Id { get; set; }
        public List<Position> Listpositions { get; set; }
    }
    public class CitiesEdutuber : BaseViewModel
    {
        public int City_Id { get; set; }
        public int Edutuber_Id { get; set; }
        public List<City> Listcities { get; set; }
    }
    public class LocationsEdutuber : BaseViewModel
    {
        public int Loca_Id { get; set; }
        public int Edutuber_Id { get; set; }
        public List<Location> Listlocations { get; set; }
    }
}