using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using SCORM1.Models.PageCustomization;
using PagedList;
using SCORM1.Models.Logs;
using SCORM1.Models.ratings;
using System.Security.Cryptography.X509Certificates;
using SCORM1.Models.RigidCourse;
using iTextSharp.text.pdf.events;
using SCORM1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SCORM1.Models.ViewModel;
using SCORM1.Models.Edutuber;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using SCORM1.Enum;
using SCORM1.Models.Lms;

namespace SCORM1.Controllers
{
    public class EdutuberController : Controller
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public EdutuberController()
        {
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        // GET: Edutuber
        public ActionResult Index()
        {
            return View();
        }
        public ApplicationUser GetActualUserId()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            return user;
        }
        #region Edutuber Users
        #endregion
        #region Edutuber Teachers
        //Returns all available Edutuber sessions available
        [Authorize]
        public ActionResult EdutuberUserList()
        {

            EdutuberUserVM edutuberModel = new EdutuberUserVM();
            //Get list from data base;
            var actualUser = ApplicationDbContext.Users.Find(GetActualUserId().Id);

            List<EdutuberSession> EdutuberList = new List<EdutuberSession>();
            List<EdutuberSession> tempList = new List<EdutuberSession>();
            List<EdutuberEnrollment> matriculaEdutuber = new List<EdutuberEnrollment>();
            List<EdutuberSession> EdutuberReady = new List<EdutuberSession>();
            if (actualUser.Role == ROLES.AdministradoGeneral)
            {
                EdutuberList = ApplicationDbContext.EdutuberSessions.ToList();
            }
            if (actualUser.Role == ROLES.Usuario)
            {
                matriculaEdutuber = ApplicationDbContext.EdutuberEnrollments.Where(x => x.user_id == actualUser.Id)
                    .ToList();
                EdutuberList = ApplicationDbContext.EdutuberSessions.ToList();


                for (int i = 0; i < EdutuberList.Count(); i++)
                {
                    if (matriculaEdutuber.Count() > 0)
                    {
                        for (int x = 0; x < matriculaEdutuber.Count(); x++)
                        {
                            if (EdutuberList[i].id == matriculaEdutuber[x].Edutuber_id && EdutuberList[i].available == true && EdutuberList[i].end_date > DateTime.Now)
                            {
                                EdutuberReady.Add(EdutuberList[i]);
                            }
                        }

                    }

                }

            }

            //Removes not available and date expired Edutuber sessions To-Do
            if (EdutuberList.Count > 0)
            {
                foreach (EdutuberSession debateRoom in EdutuberList)
                {
                    if (!debateRoom.available)
                    {
                        tempList.Add(debateRoom);
                    }
                    else if (debateRoom.end_date < DateTime.Now)
                    {
                        tempList.Add(debateRoom);
                    }
                }
                foreach (EdutuberSession debateRoom in tempList)
                {
                    EdutuberList.Remove(debateRoom);
                }
            }

            //To-Do, filter list by enrollment and open
            edutuberModel.listOfEdutuber = EdutuberReady;
            edutuberModel.Sesion = GetActualUserId().SesionUser;
            return View(edutuberModel);
        }
        [Authorize]
        public ActionResult EdutuberUserListTeacher()
        {
            EdutuberUserVM edutuberModel = new EdutuberUserVM();
            //Get list from data base;
            List<EdutuberSession> edutuberList = new List<EdutuberSession>();
            List<EdutuberSession> tempList = new List<EdutuberSession>();
            edutuberList = ApplicationDbContext.EdutuberSessions.ToList();

            //Removes not available and date expired Edutuber sessions To-Do
            if (edutuberList.Count > 0)
            {
                foreach (EdutuberSession debateRoom in edutuberList)
                {
                    if (!debateRoom.available)
                    {
                        tempList.Add(debateRoom);
                    }
                    else if (debateRoom.end_date < DateTime.Now)
                    {
                        tempList.Add(debateRoom);
                    }
                }
                foreach (EdutuberSession debateRoom in tempList)
                {
                    edutuberList.Remove(debateRoom);
                }
            }
            //To-Do, filter list by enrollment and open
            edutuberModel.listOfEdutuber = edutuberList;
            edutuberModel.Sesion = GetActualUserId().SesionUser;
            return View(edutuberModel);
        }

        [Authorize]
        public ActionResult EdutuberContent(int id)
        {
            var actualUser = ApplicationDbContext.Users.Find(GetActualUserId().Id);
            EdutuberUserVM edutuberModel = new EdutuberUserVM();
            EdutuberSession edutuberToReturn = ApplicationDbContext.EdutuberSessions.Find(id);
            var edutuberEnrollments = ApplicationDbContext.EdutuberEnrollments.Where(x => x.Edutuber_id == id && x.user_id == actualUser.Id).FirstOrDefault();
            List<EdutuberUserFile> edutuberUserIssuedFiles = ApplicationDbContext.EdutuberUserFiles.Where(x => x.user_id == actualUser.Id && x.Edutuber_id == id).ToList();
            List<EdutuberTeacherComment> edutuberTeacherComments = ApplicationDbContext.EdutuberTeacherComments.Where(x => x.user_id == actualUser.Id && x.Edutuber_id == id).ToList();
            EdutuberUserFile file = new EdutuberUserFile();
            if (edutuberToReturn.end_date.Subtract(DateTime.Now).TotalMinutes < 15)
            {
                edutuberModel.meetingAvailable = true;
            }
            //add loaded data to view model
            edutuberModel.listOfComments = edutuberTeacherComments;
            edutuberModel.EdutuberEnrollment = edutuberEnrollments;
            edutuberModel.listOfIssuedFiles = edutuberUserIssuedFiles;
            edutuberModel.EdutuberFileToAdd = file;
            edutuberModel.actualEdutuber = edutuberToReturn;
            edutuberModel.Sesion = GetActualUserId().SesionUser;
            return View(edutuberModel);
        }


        [Authorize]
        public ActionResult EdutuberContentTeacher(int id)
        {

            var actualUser = ApplicationDbContext.Users
                .Find(GetActualUserId().Id);
            EdutuberUserVM edutuberModel = new EdutuberUserVM();
            EdutuberSession edutuberToReturn = ApplicationDbContext.EdutuberSessions.Find(id);
            List<EdutuberUserFile> edutuberUserIssuedFiles = ApplicationDbContext.EdutuberUserFiles
                .Where(x => x.Edutuber_id == id)
                .OrderByDescending(x => x.registered_date)
                .ToList();
            var idedutuber = edutuberUserIssuedFiles.Where(m => m.Edutuber_id == id)
                .OrderByDescending(m => m.registered_date).First();
            ;
            List<ApplicationUser> applicationUsers = ApplicationDbContext.Users
                .ToList();
            var DbUsers = ApplicationDbContext.EdutuberEnrollments.Where(m => m.Edutuber_id == id)
                .ToList();

            List<ApplicationUser> ListUser = new List<ApplicationUser>();

            if (applicationUsers.Count != 0)
            {
                for (int i = 0; i < applicationUsers.Count; i++)
                {
                    for (int x = 0; x < DbUsers.Count; x++)
                    {
                        if (applicationUsers[i].Id == DbUsers[x].user_id)
                        {
                            ListUser.Add(applicationUsers[i]);
                        }

                    }
                }
            }

            List<EdutuberTeacherComment> edutuberTeacherComments = ApplicationDbContext.EdutuberTeacherComments
                .Where(x => x.teacher_id == actualUser.Id && x.Edutuber_id == id)
                .ToList();

            EdutuberUserFile file = new EdutuberUserFile();
            if (edutuberToReturn.end_date.Subtract(DateTime.Now).TotalMinutes < 15)
            {
                edutuberModel.meetingAvailable = true;
            }
            //add loaded data to view model
            edutuberModel.ListEnrollment = DbUsers;
            edutuberModel.listOfUsers = ListUser;
            edutuberModel.listOfIssuedFiles = edutuberUserIssuedFiles;
            edutuberModel.listOfComments = edutuberTeacherComments;
            edutuberModel.teacherName = actualUser.FirstName;
            edutuberModel.teacherLastName = actualUser.LastName;
            edutuberModel.EdutuberFileToAdd = file;
            edutuberModel.actualEdutuber = edutuberToReturn;
            edutuberModel.Sesion = GetActualUserId().SesionUser;
            return View(edutuberModel);
        }
        public string numero { get; set; }
        [Authorize]
        public JsonResult ComentTeacher1(string id)
        {
            bool result = false;
            if (id != null)
            {
                numero = id;
                EdutuberUserVM edutuberModel = new EdutuberUserVM();
                edutuberModel.userId = numero;
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [Authorize]
        public JsonResult ComentTeacher(string id, int? id2, string dato)
        {

            bool result = false;
            var actualUser = ApplicationDbContext.Users.Find(GetActualUserId().Id);
            EdutuberUserVM edutuberModel = new EdutuberUserVM();

            if (id != null)
            {
                EdutuberTeacherComment edutuberTeacherComment = new EdutuberTeacherComment
                {
                    user_id = id,
                    Edutuber_id = (int)id2,
                    teacher_id = actualUser.Id,
                    commentDate = DateTime.Now,
                    content = dato,

                };
                result = true;
                ApplicationDbContext.EdutuberTeacherComments.Add(edutuberTeacherComment);
                ApplicationDbContext.SaveChanges();

            }
            else
            {
                throw new Exception("Error al crear el comentario");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("EvaluarTeacher/{id:string}/{id2:string}/{dato:float}")]
        [Authorize]
        public JsonResult EvaluarTeacher(string id, int? id2, float? dato)
        {
            EdutuberUserVM model = new EdutuberUserVM();
            model.EdutuberEnrollment = new EdutuberEnrollment();
            var entidad = ApplicationDbContext.EdutuberEnrollments.Where(x => x.user_id == id && x.Edutuber_id == id2).FirstOrDefault();

            bool result = false;
            if (entidad != null)
            {
                entidad.qualification = (float)dato;
                entidad.Edutuber_enro_init_date = entidad.Edutuber_enro_init_date;
                entidad.Edutuber_enro_finish_date = entidad.Edutuber_enro_finish_date;
                model.Sesion = GetActualUserId().SesionUser;
                ApplicationDbContext.SaveChanges();
                result = true;
            }
            else
            {
                throw new Exception("Error al encontrar Edutuber");
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public ActionResult UpdateSession(int id)
        {
            CreateEdutuberSession model = new CreateEdutuberSession();
            model.actualEdutuber1 = new EdutuberSession();
            var entidad = ApplicationDbContext.EdutuberSessions.Find(id);

            if (entidad != null)
            {
                model.actualEdutuber1.id = entidad.id;
                model.actualEdutuber1.name = entidad.name;
                model.actualEdutuber1.open = entidad.open;
                model.actualEdutuber1.start_date = entidad.start_date;
                model.actualEdutuber1.end_date = entidad.end_date;
                model.actualEdutuber1.available = entidad.available;
                model.actualEdutuber1.case_content = entidad.case_content;
                model.actualEdutuber1.session_url = entidad.session_url;
                model.actualEdutuber1.resource_url = entidad.resource_url;
                model.Sesion = GetActualUserId().SesionUser;
            }
            else
            {
                throw new Exception("Error al encontrar Edutuber");
            }
            return View(model);
        }

        //Function thats receive the view model information and an uploaded file
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UploadEdutuberFile(EdutuberUserVM edutuberModel, HttpPostedFileBase upload)
        {
            var actualUser = ApplicationDbContext.Users.Find(GetActualUserId().Id);

            var edutubere = ApplicationDbContext.EdutuberSessions.Find(edutuberModel.actualEdutuber.id);
            edutuberModel.Sesion = GetActualUserId().SesionUser;
            if (upload != null && upload.ContentLength > 0 && upload.ContentLength <= (2 * 1000000))
            {
                string[] allowedExtensions = new[] { ".pdf", ".doc", ".pptx", ".xls", ".xlsx", ".docx" };
                var ext = Path.GetExtension(DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + upload.FileName).ToLower();
                var file = "";
                foreach (var extention in allowedExtensions)
                {
                    if (extention.Contains(ext))
                    {
                        file = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + upload.FileName).ToLower();
                        upload.SaveAs(Server.MapPath("~/EdutuberUploads/" + file));
                        EdutuberUserFile fileToAdd = new EdutuberUserFile
                        {
                            user_id = actualUser.Id,
                            Edutuber_id = edutuberModel.actualEdutuber.id,
                            register_name = edutuberModel.EdutuberFileToAdd.register_name,
                            file_description = edutuberModel.EdutuberFileToAdd.file_description,
                            file_extention = ext,
                            file_name = file,
                            registered_date = DateTime.Now
                        };
                        ApplicationDbContext.EdutuberUserFiles.Add(fileToAdd);
                        ApplicationDbContext.SaveChanges();
                        TempData["Info"] = "Archivo cargado exitosamente";
                        List<EdutuberUserFile> fileList = ApplicationDbContext.EdutuberUserFiles.Where(x => x.user_id == actualUser.Id && x.Edutuber_id == edutuberModel.actualEdutuber.id).ToList();
                        List<EdutuberTeacherComment> teacherComments = ApplicationDbContext.EdutuberTeacherComments.Where(x => x.user_id == actualUser.Id && x.Edutuber_id == edutuberModel.actualEdutuber.id).ToList();
                        var edutuberEnrollments = ApplicationDbContext.EdutuberEnrollments.Where(x => x.Edutuber_id == edutuberModel.actualEdutuber.id && x.user_id == actualUser.Id).FirstOrDefault();
                        edutuberModel.EdutuberEnrollment = edutuberEnrollments;
                        edutuberModel.listOfIssuedFiles = fileList;
                        edutuberModel.listOfComments = teacherComments;
                        return View("EdutuberContent", edutuberModel);
                    }
                }
                TempData["Info"] = "El formato del archivo no es valido";
                return View("EdutuberContent", edutuberModel);
            }
            else
            {
                TempData["Info"] = "Los campos no pueden estar vacios";
                return View("EdutuberContent", edutuberModel);
            }
        }
        public ActionResult RedirectToUrl(int id)
        {
            EdutuberSession edutuberToReturn = ApplicationDbContext.EdutuberSessions.Find(id);
            return Redirect(edutuberToReturn.session_url);
        }

        #endregion
        #region Edutuber Admin        
        #endregion

        public ActionResult CreateEdutuber()
        {
            CreateEdutuberSession edutuberModel = new CreateEdutuberSession();
            //Get list from data base;
            List<EdutuberSession> edutuberList = new List<EdutuberSession>();
            edutuberList = ApplicationDbContext.EdutuberSessions.ToList();

            //To-Do, filter list by enrollment and open
            edutuberModel.listOfEdutuber = edutuberList;
            edutuberModel.Sesion = GetActualUserId().SesionUser;
            return View(edutuberModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateEdutuber2(CreateEdutuberSession entrada)
        {
            if (ModelState.IsValid)
            {
                var anterior = ApplicationDbContext.EdutuberSessions.Where(x => x.name == entrada.actualEdutuber1.name).Count();
                if (anterior != 0)
                {
                    throw new Exception("El nombre de Edutuber ya esta siendo utilizado ");
                }

                EdutuberSession edutuberSession = new EdutuberSession
                {
                    name = entrada.actualEdutuber1.name,
                    start_date = entrada.actualEdutuber1.start_date,
                    end_date = entrada.actualEdutuber1.end_date,
                    case_content = entrada.actualEdutuber1.case_content,
                    available = entrada.actualEdutuber1.available,
                    resource_url = entrada.actualEdutuber1.resource_url,
                    session_url = entrada.actualEdutuber1.session_url,
                    open = entrada.actualEdutuber1.open,
                };
                ApplicationDbContext.EdutuberSessions.Add(edutuberSession);
                ApplicationDbContext.SaveChanges();

            }
            entrada.Sesion = GetActualUserId().SesionUser;
            return RedirectToAction("CreateEdutuber", "Edutuber");
        }
        public ActionResult MatriculaEdutuber(int? page, int id_Edutuber)
        {
            int companyId = (int)GetActualUserId().CompanyId;
            IPagedList<ApplicationUser> ListOfUser = ApplicationDbContext.Users.OrderBy(x => x.UserName).Where(x => x.CompanyId == companyId && x.ComunidadActiva != null).ToList().ToPagedList(page ?? 1, 15);
            var actualEdutuberEnrrollment = ApplicationDbContext.EdutuberEnrollments.Where(x => x.Edutuber_id == id_Edutuber).ToList();


            CreateEdutuberSession model = new CreateEdutuberSession
            {
                listEnrollment = actualEdutuberEnrrollment,
                UserOfCompany = ListOfUser,
                Id_Edutuber = id_Edutuber,

            };
            model.Sesion = GetActualUserId().SesionUser;
            return View(model);
        }
        [Authorize]
        public ActionResult Edutuber2(string id, int id_Edutuber, int? page)
        {
            var user = id;
            var anterior = ApplicationDbContext.EdutuberEnrollments.Where(x => x.user_id == user && x.Edutuber_id == id_Edutuber).FirstOrDefault();
            if (anterior != null)
            {
                throw new Exception("Ya esta Matriculado");
            }
            var actualEdutuberEnrrollment = ApplicationDbContext.EdutuberSessions.Where(x => x.id == id_Edutuber).FirstOrDefault();


            EdutuberEnrollment edutuberEnrollment = new EdutuberEnrollment
            {
                user_id = user,
                Edutuber_id = id_Edutuber,
                Edutuber_enro_init_date = actualEdutuberEnrrollment.start_date,
                Edutuber_enro_finish_date = actualEdutuberEnrrollment.end_date,
                qualification = 0,
            };


            ApplicationDbContext.EdutuberEnrollments.Add(edutuberEnrollment);
            ApplicationDbContext.SaveChanges();
            int a = 1;
            return RedirectToAction("MatriculaEdutuber", new { page, id_Edutuber });
        }
        public ActionResult DeleteEdutuber(string id, int id_Edutuber)
        {
            var user = id;
            var entidad = ApplicationDbContext.EdutuberEnrollments.Where(x => x.user_id == user && x.Edutuber_id == id_Edutuber).FirstOrDefault();
            try
            {
                if (entidad == null)
                {
                    throw new Exception("No esta matriculado");
                }
                ApplicationDbContext.EdutuberEnrollments.Remove(entidad);
                ApplicationDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("El elemento no se puede ser eliminado", ex.InnerException);
            }
            int a = 1;
            return RedirectToAction("MatriculaEdutuber", new { a, id_Edutuber });
        }
        [Authorize]
        public JsonResult DeleteSession(int id)
        {
            bool result = false;
            var entidad = ApplicationDbContext.EdutuberSessions
                .Where(x => x.id == id)
                .FirstOrDefault();
            var entidadMatriculas = ApplicationDbContext.EdutuberEnrollments
                .Where(x => x.Edutuber_id == id)
                .ToList();
            if (entidad != null)
            {
                try
                {
                    ApplicationDbContext.EdutuberEnrollments.RemoveRange(entidadMatriculas);
                    ApplicationDbContext.EdutuberSessions.Remove(entidad);
                    ApplicationDbContext.SaveChanges();
                    result = true;

                }
                catch (Exception ex)
                {
                    throw new Exception("El elemento no se puede ser eliminado", ex.InnerException);
                }
            }
            else
            {
                throw new Exception("No se encuentra Edutuber");
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult UpdateSession2(CreateEdutuberSession entrada)
        {
            var anterior = ApplicationDbContext.EdutuberSessions
                  .Where(x => x.id != entrada.actualEdutuber1.id && x.name == entrada.actualEdutuber1.name)
                  .FirstOrDefault();

            if (anterior != null)
            {
                throw new Exception("La configuracion ya existe");
            }
            var entidad = ApplicationDbContext.EdutuberSessions
                .Find(entrada.actualEdutuber1.id);

            if (entidad != null)
            {

                entidad.name = entrada.actualEdutuber1.name;
                entidad.case_content = entrada.actualEdutuber1.case_content;
                entidad.start_date = entrada.actualEdutuber1.start_date;
                entidad.end_date = entrada.actualEdutuber1.end_date;
                entidad.available = entrada.actualEdutuber1.available;
                entidad.open = entrada.actualEdutuber1.available;
                entidad.session_url = entrada.actualEdutuber1.session_url;
                entidad.resource_url = entrada.actualEdutuber1.resource_url;
                try
                {
                    entrada.Sesion = GetActualUserId().SesionUser;
                    ApplicationDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("El elemento no se puede modificar", ex.InnerException);
                }

            }
            else
            {
                throw new Exception("Elemento no encontrado");
            }

            return RedirectToAction("CreateEdutuber");
        }
        public string IpUser()
        {
            string ip = Request.UserHostAddress;
            string szXForwardedFor = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string Ipserver;
            if (szXForwardedFor != null)
            {
                Ipserver = szXForwardedFor;
            }
            else
            {
                Ipserver = ip;
            }
            return Ipserver;
        }

        public ActionResult SearchUserManager(CreateEdutuberSession model, int? page)
        {
            var companyId = GetActualUserId().CompanyId;
            if (string.IsNullOrEmpty(model.User) || string.IsNullOrWhiteSpace(model.User))
            {
                var table = ApplicationDbContext.TableChanges.Find(72);
                var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                var code = ApplicationDbContext.CodeLogs.Find(252);
                var idcompany = UserCurrent.CompanyId;
                IPagedList<ApplicationUser> ListOfUser = ApplicationDbContext.Users.OrderBy(x => x.UserName).Where(x => x.CompanyId == companyId && x.ComunidadActiva != null).ToList().ToPagedList(page ?? 1, 15);
                var actualEdutuberEnrrollment = ApplicationDbContext.EdutuberEnrollments.Where(x => x.Edutuber_id == model.Id_Edutuber).ToList();
                model.listEnrollment = actualEdutuberEnrrollment;
                model.UserOfCompany = ListOfUser;
                model.Id_Edutuber = model.Id_Edutuber;
                if (idcompany != null)
                {
                    var company = ApplicationDbContext.Companies.Find(idcompany);
                    string ip = IpUser();
                    var idchange = new IdChange
                    {
                        IdCh_IdChange = null
                    };
                    ApplicationDbContext.IdChanges.Add(idchange);
                    ApplicationDbContext.SaveChanges();

                }

                int a = 1;
                return RedirectToAction("MatriculaEdutuber", new { a, model.Id_Edutuber });
            }
            else
            {
                IPagedList<ApplicationUser> ListOfUser;
                var conteo = ApplicationDbContext.Users.OrderBy(x => x.UserName).Where(x => x.CompanyId == companyId && (x.FirstName.Contains(model.User) || x.LastName.Contains(model.User) || x.UserName.Contains(model.User))).ToList();
                if (conteo.Count > 0)
                {
                    ListOfUser = ApplicationDbContext.Users.OrderBy(x => x.UserName).Where(x => x.CompanyId == companyId && (x.FirstName.Contains(model.User) || x.LastName.Contains(model.User) || x.UserName.Contains(model.User))).ToList().ToPagedList(page ?? 1, conteo.Count);
                }
                else
                {
                    ListOfUser = ApplicationDbContext.Users.OrderBy(x => x.UserName).Where(x => x.CompanyId == companyId && (x.FirstName.Contains(model.User) || x.LastName.Contains(model.User) || x.UserName.Contains(model.User))).ToList().ToPagedList(page ?? 1, 6);
                }
                model = new CreateEdutuberSession
                {
                    UserOfCompany = ListOfUser,
                    Id_Edutuber = model.Id_Edutuber,
                };

                model.Sesion = GetActualUserId().SesionUser;
                var table = ApplicationDbContext.TableChanges.Find(72);
                var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                var code = ApplicationDbContext.CodeLogs.Find(252);
                var idcompany = UserCurrent.CompanyId;
                var actualEdutuberEnrrollment = ApplicationDbContext.EdutuberEnrollments.Where(x => x.Edutuber_id == model.Id_Edutuber).ToList();
                model.listEnrollment = actualEdutuberEnrrollment;
                model.UserOfCompany = ListOfUser;
                model.Id_Edutuber = model.Id_Edutuber;
                if (idcompany != null)
                {
                    var company = ApplicationDbContext.Companies.Find(idcompany);
                    string ip = IpUser();
                    var idchange = new IdChange
                    {
                        IdCh_IdChange = null
                    };
                    ApplicationDbContext.IdChanges.Add(idchange);
                    ApplicationDbContext.SaveChanges();

                }
                return View("MatriculaEdutuber", model);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult SeachUserEdutuber(CreateEdutuberSession model, int? page)
        {
            var GetModuleCompany = GetActualUserId().CompanyId;
            if (string.IsNullOrWhiteSpace(model.SearchEdutuber) || string.IsNullOrEmpty(model.SearchEdutuber))
            {
                //Inicio código logs
                var table = ApplicationDbContext.TableChanges.Find(72);
                var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                var code = ApplicationDbContext.CodeLogs.Find(206);
                var idcompany = UserCurrent.CompanyId;
                if (idcompany != null)
                {
                    var company = ApplicationDbContext.Companies.Find(idcompany);
                    string ip = IpUser();
                    var idchange = new IdChange
                    {
                        IdCh_IdChange = null
                    };
                    ApplicationDbContext.IdChanges.Add(idchange);
                    ApplicationDbContext.SaveChanges();
                    Log logsesiontrue = new Log
                    {
                        ApplicationUser = UserCurrent,
                        CoLo_Id = code.CoLo_Id,
                        CodeLogs = code,
                        Log_Date = DateTime.Now,
                        Log_StateLogs = LOGSTATE.Realizado,
                        TableChange = table,
                        TaCh_Id = table.TaCh_Id,
                        IdChange = idchange,
                        IdCh_Id = idchange.IdCh_Id,
                        User_Id = UserCurrent.Id,
                        Log_Description = "El Edutuber con id: " + UserCurrent.Id + " acaba de realizar una búsqueda de un usuario, sin ingresar ningún nombre para buscar, en la compañía con id " + company.CompanyId,
                        Company = company,
                        Company_Id = company.CompanyId,
                        Log_Ip = ip
                    };
                    ApplicationDbContext.Logs.Add(logsesiontrue);
                    ApplicationDbContext.SaveChanges();
                }
                //Fin código logs
                return View();
            }
            else
            {
                IPagedList<EdutuberSession> ListEdutuber = ApplicationDbContext.EdutuberSessions.OrderBy(x => x.name).Where(x => x.open == true && x.available == true && x.end_date < DateTime.Now && x.name.Contains(model.SearchEdutuber)).ToList().ToPagedList(page ?? 1, 10);
                //model = new EdutuberSession {  UserEnrolllment = ListOfUser, cLogo = GetUrlLogo() };
                model.Sesion = GetActualUserId().SesionUser;
                //Inicio código logs
                var table = ApplicationDbContext.TableChanges.Find(72);
                var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                var code = ApplicationDbContext.CodeLogs.Find(206);
                var idcompany = UserCurrent.CompanyId;
                if (idcompany != null)
                {
                    var company = ApplicationDbContext.Companies.Find(idcompany);
                    string ip = IpUser();
                    var idchange = new IdChange
                    {
                        IdCh_IdChange = null
                    };
                    ApplicationDbContext.IdChanges.Add(idchange);
                    ApplicationDbContext.SaveChanges();
                    Log logsesiontrue = new Log
                    {
                        ApplicationUser = UserCurrent,
                        CoLo_Id = code.CoLo_Id,
                        CodeLogs = code,
                        Log_Date = DateTime.Now,
                        Log_StateLogs = LOGSTATE.Realizado,
                        TableChange = table,
                        TaCh_Id = table.TaCh_Id,
                        IdChange = idchange,
                        IdCh_Id = idchange.IdCh_Id,
                        User_Id = UserCurrent.Id,
                        Log_Description = "El Edutuber con id: " + UserCurrent.Id + " acaba de realizar una búsqueda de un usuario ingresando un nombre para buscar, en la compañía con id" + company.CompanyId,
                        Company = company,
                        Company_Id = company.CompanyId,
                        Log_Ip = ip
                    };
                    ApplicationDbContext.Logs.Add(logsesiontrue);
                    ApplicationDbContext.SaveChanges();
                }
                //Fin código logs
                return View("CreateEdutuber", model);
            }
        }

        [Authorize]
        public ActionResult EnrollmentMasiveEdutuber(int id_Edutuber)
        {
            var GetModuleCompany = GetActualUserId().CompanyId;
            EdutuberSession getSession = ApplicationDbContext.EdutuberSessions.Find(id_Edutuber);
            var Users = ApplicationDbContext.Users.Where(x => x.CompanyId == GetModuleCompany && x.Role == ROLES.Usuario).ToList();
            List<AllUserEdutuber> Listalluser = new List<AllUserEdutuber>();
            foreach (var item in Users)
            {
                Listalluser.Add(new AllUserEdutuber
                {
                    User_Id = item.Id,
                    Edutuber_Id = id_Edutuber
                });
            }
            List<AreasEdutuber> ListAreas = new List<AreasEdutuber>();
            var areas = ApplicationDbContext.Areas.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListAreas.Add(new AreasEdutuber
            {
                Edutuber_Id = id_Edutuber,
                Listareas = areas
            });
            List<PositionsEdutuber> ListPositions = new List<PositionsEdutuber>();
            var positions = ApplicationDbContext.Position.ToList();

            ListPositions.Add(new PositionsEdutuber
            {
                Edutuber_Id = id_Edutuber,
                Listpositions = positions
            });
            List<CitiesEdutuber> ListCities = new List<CitiesEdutuber>();
            var city = ApplicationDbContext.City.ToList();
            ListCities.Add(new CitiesEdutuber
            {
                Edutuber_Id = id_Edutuber,
                Listcities = city
            });
            List<LocationsEdutuber> ListLocations = new List<LocationsEdutuber>();
            var location = ApplicationDbContext.Location.Where(x => x.CompanyId == GetModuleCompany).ToList();
            ListLocations.Add(new LocationsEdutuber
            {
                Edutuber_Id = id_Edutuber,
                Listlocations = location
            });
            CreateEdutuberSession model = new CreateEdutuberSession
            {
                ActualRole = GetActualUserId().Role,
                actualEdutuber1 = getSession,
                ListAllUser = Listalluser,
                ListAreas = ListAreas,
                ListPositions = ListPositions,
                ListCitices = ListCities,
                ListLocations = ListLocations
            };
            model.Sesion = GetActualUserId().SesionUser;
            var table = ApplicationDbContext.TableChanges.Find(34);
            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
            var code = ApplicationDbContext.CodeLogs.Find(219);
            var idcompany = UserCurrent.CompanyId;

            return View("EnrollmentMasiveEdutuber", model);
        }



        [Authorize]
        public ActionResult AllUsers()
        {
            var GetModuleCompany = GetActualUserId().CompanyId;
            var Users = ApplicationDbContext.Users.Where(x => x.CompanyId == GetModuleCompany && x.Role == ROLES.Usuario).ToList();
            CreateEdutuberSession model = new CreateEdutuberSession
            {
                ActualRole = GetActualUserId().Role,
                listUser123 = Users
            };
            model.Sesion = GetActualUserId().SesionUser;
            return PartialView("_AllUser", model);
        }
        [Authorize]
        public ActionResult EnrollmentAreas(CreateEdutuberSession entrada)
        {

            var edutubersession = ApplicationDbContext.EdutuberSessions.Find(entrada.Id_Edutuber);
            var areas = entrada.ListAreas.Take(1);
            var idarea = areas.Select(x => x.Area_Id).Single();
            var user = ApplicationDbContext.Users.Where(x => x.AreaId == idarea && x.Role == ROLES.Usuario).ToList();
            if (user.Count() != 0)
            {
                foreach (var item in user)
                {
                    var useractual = ApplicationDbContext.Users.Find(item.Id);

                    EdutuberEnrollment edutuberEnrollment = new EdutuberEnrollment
                    {
                        user_id = useractual.Id,
                        Edutuber_id = edutubersession.id,
                        Edutuber_enro_init_date = edutubersession.start_date,
                        Edutuber_enro_finish_date = edutubersession.start_date,
                        qualification = 0,
                    };
                    ApplicationDbContext.EdutuberEnrollments.Add(edutuberEnrollment);
                    ApplicationDbContext.SaveChanges();
                }
            }
            else
            {
                TempData["Info"] = "No hay usuarios asociados con esta area";
            };

            return View();
        }
        [Authorize]
        public ActionResult CancelEnrollmentAreas(AdminTrainingMassiveEnrollmentViewMode Enrollments)
        {
            var GetModuleCompany = GetActualUserId().CompanyId;
            var A = Enrollments.ListAreas.Take(1);
            int c = A.Select(x => x.Modu_Id).Single();
            int d = A.Select(x => x.Area_Id).Single();
            List<user> ListUserCancel = new List<user>();
            List<user1> ListUserCancel1 = new List<user1>();
            List<user2> ListUserCancel2 = new List<user2>();
            var user = ApplicationDbContext.Users.Where(x => x.AreaId == d && x.Role == ROLES.Usuario).ToList();
            Module getModule = ApplicationDbContext.Modules.Find(c);
            List<AllUser> Listalluser = new List<AllUser>();
            foreach (var item in user)
            {
                Listalluser.Add(new AllUser
                {
                    User_Id = item.Id,
                    Modu_Id = c
                });
            }
            if (user.Count != 0)
            {
                foreach (var item in user)
                {
                    int Modu_Id = c;
                    string User_Id = item.Id;
                    var useractual = ApplicationDbContext.Users.Find(User_Id);
                    var module = ApplicationDbContext.Modules.Find(Modu_Id);
                    var getEnrollment = useractual.Enrollment.Where(x => x.ApplicationUser.Id == useractual.Id && x.Module.Modu_Id == module.Modu_Id).FirstOrDefault();
                    if (getEnrollment != null)
                    {
                        var advanceuser = ApplicationDbContext.AdvanceUsers.Where(x => x.User_Id == useractual.Id && x.TopicsCourse.Module.Modu_Id == getEnrollment.Module.Modu_Id).ToList();
                        var attempts = ApplicationDbContext.Attempts.Where(x => x.UserId == User_Id && x.BankQuestion.TopicsCourse.Modu_Id == getEnrollment.Modu_Id).ToList();
                        if (advanceuser.Count != 0 || attempts.Count != 0)
                        {
                            ListUserCancel.Add(new user
                            {
                                UserName = useractual.UserName,
                                Name = useractual.FirstName + useractual.LastName,
                                Email = useractual.Email
                            });
                            TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                            var table = ApplicationDbContext.TableChanges.Find(22);
                            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                            var code = ApplicationDbContext.CodeLogs.Find(227);
                            var idcompany = UserCurrent.CompanyId;
                            if (idcompany != null)
                            {
                                var company = ApplicationDbContext.Companies.Find(idcompany);
                                string ip = IpUser();
                                var idchange = new IdChange
                                {
                                    IdCh_IdChange = getEnrollment.Enro_Id.ToString()
                                };
                                ApplicationDbContext.IdChanges.Add(idchange);
                                ApplicationDbContext.SaveChanges();
                                Log logsesiontrue = new Log
                                {
                                    ApplicationUser = UserCurrent,
                                    CoLo_Id = code.CoLo_Id,
                                    CodeLogs = code,
                                    Log_Date = DateTime.Now,
                                    Log_StateLogs = LOGSTATE.Realizado,
                                    TableChange = table,
                                    TaCh_Id = table.TaCh_Id,
                                    IdChange = idchange,
                                    IdCh_Id = idchange.IdCh_Id,
                                    User_Id = UserCurrent.Id,
                                    Log_Description = "El usuario con id: " + UserCurrent.Id + " Intento des matricular por área el curso con id " + Modu_Id + " al usuario con id " + item.Id + " pero el usuario ya tiene avances en este curso, en la compañía con id" + company.CompanyId,
                                    Company = company,
                                    Company_Id = company.CompanyId,
                                    Log_Ip = ip
                                };
                                ApplicationDbContext.Logs.Add(logsesiontrue);
                                ApplicationDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            ListUserCancel1.Add(new user1
                            {
                                UserName = useractual.UserName,
                                Name = useractual.FirstName + useractual.LastName,
                                Email = useractual.Email
                            });
                            TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                            TempData["Info1"] = "Usuarios desmatriculados.";
                            var table = ApplicationDbContext.TableChanges.Find(22);
                            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                            var code = ApplicationDbContext.CodeLogs.Find(227);
                            var idcompany = UserCurrent.CompanyId;
                            if (idcompany != null)
                            {
                                var company = ApplicationDbContext.Companies.Find(idcompany);
                                string ip = IpUser();
                                var idchange = new IdChange
                                {
                                    IdCh_IdChange = getEnrollment.Enro_Id.ToString()
                                };
                                ApplicationDbContext.IdChanges.Add(idchange);
                                ApplicationDbContext.SaveChanges();
                                Log logsesiontrue = new Log
                                {
                                    ApplicationUser = UserCurrent,
                                    CoLo_Id = code.CoLo_Id,
                                    CodeLogs = code,
                                    Log_Date = DateTime.Now,
                                    Log_StateLogs = LOGSTATE.Realizado,
                                    TableChange = table,
                                    TaCh_Id = table.TaCh_Id,
                                    IdChange = idchange,
                                    IdCh_Id = idchange.IdCh_Id,
                                    User_Id = UserCurrent.Id,
                                    Log_Description = "El usuario con id: " + UserCurrent.Id + " Acaba de des matricular por área el curso con id " + getEnrollment.Enro_Id + " al usuario con id " + getEnrollment.User_Id + ", en la compañía con id" + company.CompanyId,
                                    Company = company,
                                    Company_Id = company.CompanyId,
                                    Log_Ip = ip
                                };
                                ApplicationDbContext.Logs.Add(logsesiontrue);
                                ApplicationDbContext.SaveChanges();
                            }
                            ApplicationDbContext.Enrollments.Remove(getEnrollment);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        ListUserCancel2.Add(new user2
                        {
                            UserName = useractual.UserName,
                            Name = useractual.FirstName + useractual.LastName,
                            Email = useractual.Email
                        });
                        TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                        TempData["Info2"] = "Los siguientes usuarios no tienen matriculado este curso.";
                    }
                }
            }
            else
            {
                TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                TempData["Info3"] = "No hay usuarios asociados con esta area";
            }
            List<Areas> ListAreas = new List<Areas>();
            var areas = ApplicationDbContext.Areas.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListAreas.Add(new Areas
            {
                Modu_Id = c,
                Listareas = areas
            });
            List<Positions> ListPositions = new List<Positions>();
            var positions = ApplicationDbContext.Position.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListPositions.Add(new Positions
            {
                Modu_Id = c,
                Listpositions = positions
            });
            List<Cities> ListCities = new List<Cities>();
            var city = ApplicationDbContext.City.ToList();
            ListCities.Add(new Cities
            {
                Modu_Id = c,
                Listcities = city
            });
            List<Locations> ListLocations = new List<Locations>();
            var location = ApplicationDbContext.Location.Where(x => x.CompanyId == GetModuleCompany).ToList();
            ListLocations.Add(new Locations
            {
                Modu_Id = c,
                Listlocations = location
            });
            AdminTrainingMassiveEnrollmentViewMode model = new AdminTrainingMassiveEnrollmentViewMode
            {
                ActualRole = GetActualUserId().Role,
                Module = getModule,
                ListAllUser = Listalluser,
                ListAreas = ListAreas,
                ListPositions = ListPositions,
                ListCitices = ListCities,
                ListLocations = ListLocations,
                ListUserCancel = ListUserCancel,
                ListUserCancel1 = ListUserCancel1,
                ListUserCancel2 = ListUserCancel2
            };
            model.Sesion = GetActualUserId().SesionUser;
            return View("CancelEnrollments", model);
        }
        [Authorize]
        public ActionResult EnrollmentPositions(AdminTrainingMassiveEnrollmentViewMode Enrollments)
        {
            var module = ApplicationDbContext.Modules.Find(Enrollments.Module.Modu_Id);
            var A = Enrollments.ListPositions.Take(1);
            int d = A.Select(x => x.Posi_Id).Single();
            var user = ApplicationDbContext.Users.Where(x => x.PositionId == d && x.Role == ROLES.Usuario).ToList();
            if (user.Count != 0)
            {
                foreach (var item in user)
                {
                    var useractual = ApplicationDbContext.Users.Find(item.Id);
                    var getEnrollment = useractual.Enrollment.Where(x => x.ApplicationUser.Id == useractual.Id && x.Module.Modu_Id == module.Modu_Id).ToList();
                    if (getEnrollment.Count == 0)
                    {
                        DateTime finish = new DateTime();
                        switch (module.Modu_Period)
                        {
                            case VIGENCIA.Dias:
                                finish = DateTime.Now.AddDays(module.Modu_Validity);
                                break;
                            case VIGENCIA.Meses:
                                finish = DateTime.Now.AddMonths(module.Modu_Validity);
                                break;
                            case VIGENCIA.Años:
                                finish = DateTime.Now.AddYears(module.Modu_Validity);
                                break;
                            default:
                                break;
                        }
                        DateTime b = finish;
                        TempData["Info"] = "Matricula Registrada";
                        Enrollment enrollment = new Enrollment { Module = module, ApplicationUser = useractual, Company = GetActualUserId().Company, Enro_InitDateModule = DateTime.Now, Enro_FinishDateModule = b, Enro_StateEnrollment = ENROLLMENTSTATE.Activo };
                        ApplicationDbContext.Enrollments.Add(enrollment);
                        ApplicationDbContext.SaveChanges();
                        var table = ApplicationDbContext.TableChanges.Find(22);
                        var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                        var code = ApplicationDbContext.CodeLogs.Find(223);
                        var idcompany = UserCurrent.CompanyId;
                        if (idcompany != null)
                        {
                            var company = ApplicationDbContext.Companies.Find(idcompany);
                            string ip = IpUser();
                            var idchange = new IdChange
                            {
                                IdCh_IdChange = enrollment.Enro_Id.ToString()
                            };
                            ApplicationDbContext.IdChanges.Add(idchange);
                            ApplicationDbContext.SaveChanges();
                            Log logsesiontrue = new Log
                            {
                                ApplicationUser = UserCurrent,
                                CoLo_Id = code.CoLo_Id,
                                CodeLogs = code,
                                Log_Date = DateTime.Now,
                                Log_StateLogs = LOGSTATE.Realizado,
                                TableChange = table,
                                TaCh_Id = table.TaCh_Id,
                                IdChange = idchange,
                                IdCh_Id = idchange.IdCh_Id,
                                User_Id = UserCurrent.Id,
                                Log_Description = "El usuario con id: " + UserCurrent.Id + " Acaba de matricular por cargo el curso con id " + enrollment.Enro_Id + " al usuario con id " + enrollment.User_Id + ", en la compañía con id" + company.CompanyId,
                                Company = company,
                                Company_Id = company.CompanyId,
                                Log_Ip = ip
                            };
                            ApplicationDbContext.Logs.Add(logsesiontrue);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        TempData["Info"] = "Algunos usuarios ya tiene matriculada esta materia";
                        var table = ApplicationDbContext.TableChanges.Find(22);
                        var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                        var code = ApplicationDbContext.CodeLogs.Find(223);
                        var idcompany = UserCurrent.CompanyId;
                        if (idcompany != null)
                        {
                            var company = ApplicationDbContext.Companies.Find(idcompany);
                            string ip = IpUser();
                            var idchange = new IdChange
                            {
                                IdCh_IdChange = null
                            };
                            ApplicationDbContext.IdChanges.Add(idchange);
                            ApplicationDbContext.SaveChanges();
                            Log logsesiontrue = new Log
                            {
                                ApplicationUser = UserCurrent,
                                CoLo_Id = code.CoLo_Id,
                                CodeLogs = code,
                                Log_Date = DateTime.Now,
                                Log_StateLogs = LOGSTATE.Realizado,
                                TableChange = table,
                                TaCh_Id = table.TaCh_Id,
                                IdChange = idchange,
                                IdCh_Id = idchange.IdCh_Id,
                                User_Id = UserCurrent.Id,
                                Log_Description = "El usuario con id: " + UserCurrent.Id + " Intento matricular por cargo el curso con id " + module.Modu_Id + " al usuario con id " + item.Id + "pero este usuario ya tienen matriculado este curso, en la compañía con id" + company.CompanyId,
                                Company = company,
                                Company_Id = company.CompanyId,
                                Log_Ip = ip
                            };
                            ApplicationDbContext.Logs.Add(logsesiontrue);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                TempData["Info"] = "No hay usuarios asociados con este cargo";
            }
            return RedirectToAction("Enrollments", new { Modu_Id = module.Modu_Id });
        }
        [Authorize]
        public ActionResult CancelEnrollmentPositions(AdminTrainingMassiveEnrollmentViewMode Enrollments)
        {
            var GetModuleCompany = GetActualUserId().CompanyId;
            var A = Enrollments.ListPositions.Take(1);
            int c = A.Select(x => x.Modu_Id).Single();
            int d = A.Select(x => x.Posi_Id).Single();
            List<user> ListUserCancel = new List<user>();
            List<user1> ListUserCancel1 = new List<user1>();
            List<user2> ListUserCancel2 = new List<user2>();
            var user = ApplicationDbContext.Users.Where(x => x.PositionId == d && x.Role == ROLES.Usuario).ToList();
            Module getModule = ApplicationDbContext.Modules.Find(c);
            List<AllUser> Listalluser = new List<AllUser>();
            foreach (var item in user)
            {
                Listalluser.Add(new AllUser
                {
                    User_Id = item.Id,
                    Modu_Id = c
                });
            }
            if (user.Count != 0)
            {
                foreach (var item in user)
                {
                    int Modu_Id = c;
                    string User_Id = item.Id;
                    var useractual = ApplicationDbContext.Users.Find(User_Id);
                    var module = ApplicationDbContext.Modules.Find(Modu_Id);
                    var getEnrollment = useractual.Enrollment.Where(x => x.ApplicationUser.Id == useractual.Id && x.Module.Modu_Id == module.Modu_Id).FirstOrDefault();
                    if (getEnrollment != null)
                    {
                        var advanceuser = ApplicationDbContext.AdvanceUsers.Where(x => x.User_Id == useractual.Id && x.TopicsCourse.Module.Modu_Id == getEnrollment.Module.Modu_Id).ToList();
                        var attempts = ApplicationDbContext.Attempts.Where(x => x.UserId == User_Id && x.BankQuestion.TopicsCourse.Modu_Id == getEnrollment.Modu_Id).ToList();
                        if (advanceuser.Count != 0 || attempts.Count != 0)
                        {
                            ListUserCancel.Add(new user
                            {
                                UserName = useractual.UserName,
                                Name = useractual.FirstName + useractual.LastName,
                                Email = useractual.Email
                            });
                            TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                            var table = ApplicationDbContext.TableChanges.Find(22);
                            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                            var code = ApplicationDbContext.CodeLogs.Find(228);
                            var idcompany = UserCurrent.CompanyId;
                            if (idcompany != null)
                            {
                                var company = ApplicationDbContext.Companies.Find(idcompany);
                                string ip = IpUser();
                                var idchange = new IdChange
                                {
                                    IdCh_IdChange = getEnrollment.Enro_Id.ToString()
                                };
                                ApplicationDbContext.IdChanges.Add(idchange);
                                ApplicationDbContext.SaveChanges();
                                Log logsesiontrue = new Log
                                {
                                    ApplicationUser = UserCurrent,
                                    CoLo_Id = code.CoLo_Id,
                                    CodeLogs = code,
                                    Log_Date = DateTime.Now,
                                    Log_StateLogs = LOGSTATE.Realizado,
                                    TableChange = table,
                                    TaCh_Id = table.TaCh_Id,
                                    IdChange = idchange,
                                    IdCh_Id = idchange.IdCh_Id,
                                    User_Id = UserCurrent.Id,
                                    Log_Description = "El usuario con id: " + UserCurrent.Id + " Intento des matricular por cargo el curso con id " + Modu_Id + " al usuario con id " + item.Id + " pero el usuario ya tiene avances en este curso, en la compañía con id" + company.CompanyId,
                                    Company = company,
                                    Company_Id = company.CompanyId,
                                    Log_Ip = ip
                                };
                                ApplicationDbContext.Logs.Add(logsesiontrue);
                                ApplicationDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            ListUserCancel1.Add(new user1
                            {
                                UserName = useractual.UserName,
                                Name = useractual.FirstName + useractual.LastName,
                                Email = useractual.Email
                            });
                            TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                            TempData["Info1"] = "Usuarios desmatriculados.";
                            var table = ApplicationDbContext.TableChanges.Find(22);
                            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                            var code = ApplicationDbContext.CodeLogs.Find(228);
                            var idcompany = UserCurrent.CompanyId;
                            if (idcompany != null)
                            {
                                var company = ApplicationDbContext.Companies.Find(idcompany);
                                string ip = IpUser();
                                var idchange = new IdChange
                                {
                                    IdCh_IdChange = getEnrollment.Enro_Id.ToString()
                                };
                                ApplicationDbContext.IdChanges.Add(idchange);
                                ApplicationDbContext.SaveChanges();
                                Log logsesiontrue = new Log
                                {
                                    ApplicationUser = UserCurrent,
                                    CoLo_Id = code.CoLo_Id,
                                    CodeLogs = code,
                                    Log_Date = DateTime.Now,
                                    Log_StateLogs = LOGSTATE.Realizado,
                                    TableChange = table,
                                    TaCh_Id = table.TaCh_Id,
                                    IdChange = idchange,
                                    IdCh_Id = idchange.IdCh_Id,
                                    User_Id = UserCurrent.Id,
                                    Log_Description = "El usuario con id: " + UserCurrent.Id + " Acaba de des matricular por cargo el curso con id " + getEnrollment.Enro_Id + " al usuario con id " + getEnrollment.User_Id + ", en la compañía con id" + company.CompanyId,
                                    Company = company,
                                    Company_Id = company.CompanyId,
                                    Log_Ip = ip
                                };
                                ApplicationDbContext.Logs.Add(logsesiontrue);
                                ApplicationDbContext.SaveChanges();
                            }
                            ApplicationDbContext.Enrollments.Remove(getEnrollment);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        ListUserCancel2.Add(new user2
                        {
                            UserName = useractual.UserName,
                            Name = useractual.FirstName + useractual.LastName,
                            Email = useractual.Email
                        });
                        TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                        TempData["Info2"] = "Los siguientes usuarios no tienen matriculado este curso.";
                    }
                }
            }
            else
            {
                TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                TempData["Info3"] = "No hay usuarios asociados con este cargo";
            }
            List<Areas> ListAreas = new List<Areas>();
            var areas = ApplicationDbContext.Areas.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListAreas.Add(new Areas
            {
                Modu_Id = c,
                Listareas = areas
            });
            List<Positions> ListPositions = new List<Positions>();
            var positions = ApplicationDbContext.Position.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListPositions.Add(new Positions
            {
                Modu_Id = c,
                Listpositions = positions
            });
            List<Cities> ListCities = new List<Cities>();
            var city = ApplicationDbContext.City.ToList();
            ListCities.Add(new Cities
            {
                Modu_Id = c,
                Listcities = city
            });
            List<Locations> ListLocations = new List<Locations>();
            var location = ApplicationDbContext.Location.Where(x => x.CompanyId == GetModuleCompany).ToList();
            ListLocations.Add(new Locations
            {
                Modu_Id = c,
                Listlocations = location
            });
            AdminTrainingMassiveEnrollmentViewMode model = new AdminTrainingMassiveEnrollmentViewMode
            {
                ActualRole = GetActualUserId().Role,
                Module = getModule,
                ListAllUser = Listalluser,
                ListAreas = ListAreas,
                ListPositions = ListPositions,
                ListCitices = ListCities,
                ListLocations = ListLocations,
                ListUserCancel = ListUserCancel,
                ListUserCancel1 = ListUserCancel1,
                ListUserCancel2 = ListUserCancel2
            };
            model.Sesion = GetActualUserId().SesionUser;
            return View("CancelEnrollments", model);
        }
        [Authorize]
        public ActionResult EnrollmentCities(AdminTrainingMassiveEnrollmentViewMode Enrollments)
        {
            var module = ApplicationDbContext.Modules.Find(Enrollments.Module.Modu_Id);
            var GetModuleCompany = GetActualUserId().CompanyId;
            var A = Enrollments.ListCitices.Take(1);
            int d = A.Select(x => x.City_Id).Single();
            var user = ApplicationDbContext.Users.Where(x => x.CityId == d && x.Role == ROLES.Usuario && x.CompanyId == GetModuleCompany).ToList();
            if (user.Count != 0)
            {
                foreach (var item in user)
                {
                    var useractual = ApplicationDbContext.Users.Find(item.Id);
                    var getEnrollment = useractual.Enrollment.Where(x => x.ApplicationUser.Id == useractual.Id && x.Module.Modu_Id == module.Modu_Id).ToList();
                    if (getEnrollment.Count == 0)
                    {
                        DateTime finish = new DateTime();
                        switch (module.Modu_Period)
                        {
                            case VIGENCIA.Dias:
                                finish = DateTime.Now.AddDays(module.Modu_Validity);
                                break;
                            case VIGENCIA.Meses:
                                finish = DateTime.Now.AddMonths(module.Modu_Validity);
                                break;
                            case VIGENCIA.Años:
                                finish = DateTime.Now.AddYears(module.Modu_Validity);
                                break;
                            default:
                                break;
                        }
                        DateTime b = finish;
                        TempData["Info"] = "Matricula Registrada";
                        Enrollment enrollment = new Enrollment { Module = module, ApplicationUser = useractual, Company = GetActualUserId().Company, Enro_InitDateModule = DateTime.Now, Enro_FinishDateModule = b, Enro_StateEnrollment = ENROLLMENTSTATE.Activo };
                        ApplicationDbContext.Enrollments.Add(enrollment);
                        ApplicationDbContext.SaveChanges();
                        var table = ApplicationDbContext.TableChanges.Find(22);
                        var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                        var code = ApplicationDbContext.CodeLogs.Find(224);
                        var idcompany = UserCurrent.CompanyId;
                        if (idcompany != null)
                        {
                            var company = ApplicationDbContext.Companies.Find(idcompany);
                            string ip = IpUser();
                            var idchange = new IdChange
                            {
                                IdCh_IdChange = enrollment.Enro_Id.ToString()
                            };
                            ApplicationDbContext.IdChanges.Add(idchange);
                            ApplicationDbContext.SaveChanges();
                            Log logsesiontrue = new Log
                            {
                                ApplicationUser = UserCurrent,
                                CoLo_Id = code.CoLo_Id,
                                CodeLogs = code,
                                Log_Date = DateTime.Now,
                                Log_StateLogs = LOGSTATE.Realizado,
                                TableChange = table,
                                TaCh_Id = table.TaCh_Id,
                                IdChange = idchange,
                                IdCh_Id = idchange.IdCh_Id,
                                User_Id = UserCurrent.Id,
                                Log_Description = "El usuario con id: " + UserCurrent.Id + " Acaba de matricular por ciudad el curso con id " + enrollment.Enro_Id + " al usuario con id " + enrollment.User_Id + ", en la compañía con id" + company.CompanyId,
                                Company = company,
                                Company_Id = company.CompanyId,
                                Log_Ip = ip
                            };
                            ApplicationDbContext.Logs.Add(logsesiontrue);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        TempData["Info"] = "Algunos usuarios ya tiene matriculada esta materia";
                        var table = ApplicationDbContext.TableChanges.Find(22);
                        var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                        var code = ApplicationDbContext.CodeLogs.Find(224);
                        var idcompany = UserCurrent.CompanyId;
                        if (idcompany != null)
                        {
                            var company = ApplicationDbContext.Companies.Find(idcompany);
                            string ip = IpUser();
                            var idchange = new IdChange
                            {
                                IdCh_IdChange = null
                            };
                            ApplicationDbContext.IdChanges.Add(idchange);
                            ApplicationDbContext.SaveChanges();
                            Log logsesiontrue = new Log
                            {
                                ApplicationUser = UserCurrent,
                                CoLo_Id = code.CoLo_Id,
                                CodeLogs = code,
                                Log_Date = DateTime.Now,
                                Log_StateLogs = LOGSTATE.Realizado,
                                TableChange = table,
                                TaCh_Id = table.TaCh_Id,
                                IdChange = idchange,
                                IdCh_Id = idchange.IdCh_Id,
                                User_Id = UserCurrent.Id,
                                Log_Description = "El usuario con id: " + UserCurrent.Id + " Intento matricular por ciudad el curso con id " + module.Modu_Id + " al usuario con id " + item.Id + "pero este usuario ya tienen matriculado este curso, en la compañía con id" + company.CompanyId,
                                Company = company,
                                Company_Id = company.CompanyId,
                                Log_Ip = ip
                            };
                            ApplicationDbContext.Logs.Add(logsesiontrue);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                TempData["Info"] = "No hay usuarios asociados con esta ciudad";
            }
            return RedirectToAction("Enrollments", new { Modu_Id = module.Modu_Id });
        }
        [Authorize]
        public ActionResult CancelEnrollmentCities(AdminTrainingMassiveEnrollmentViewMode Enrollments)
        {
            var GetModuleCompany = GetActualUserId().CompanyId;
            var A = Enrollments.ListCitices.Take(1);
            int c = A.Select(x => x.Modu_Id).Single();
            int d = A.Select(x => x.City_Id).Single();
            List<user> ListUserCancel = new List<user>();
            List<user1> ListUserCancel1 = new List<user1>();
            List<user2> ListUserCancel2 = new List<user2>();
            var user = ApplicationDbContext.Users.Where(x => x.CityId == d && x.Role == ROLES.Usuario && x.Company.CompanyId == GetModuleCompany).ToList();
            Module getModule = ApplicationDbContext.Modules.Find(c);
            List<AllUser> Listalluser = new List<AllUser>();
            foreach (var item in user)
            {
                Listalluser.Add(new AllUser
                {
                    User_Id = item.Id,
                    Modu_Id = c
                });
            }
            if (user.Count != 0)
            {
                foreach (var item in user)
                {
                    int Modu_Id = c;
                    string User_Id = item.Id;
                    var useractual = ApplicationDbContext.Users.Find(User_Id);
                    var module = ApplicationDbContext.Modules.Find(Modu_Id);
                    var getEnrollment = useractual.Enrollment.Where(x => x.ApplicationUser.Id == useractual.Id && x.Module.Modu_Id == module.Modu_Id).FirstOrDefault();
                    if (getEnrollment != null)
                    {
                        var advanceuser = ApplicationDbContext.AdvanceUsers.Where(x => x.User_Id == useractual.Id && x.TopicsCourse.Module.Modu_Id == getEnrollment.Module.Modu_Id).ToList();
                        var attempts = ApplicationDbContext.Attempts.Where(x => x.UserId == User_Id && x.BankQuestion.TopicsCourse.Modu_Id == getEnrollment.Modu_Id).ToList();
                        if (advanceuser.Count != 0 || attempts.Count != 0)
                        {
                            ListUserCancel.Add(new user
                            {
                                UserName = useractual.UserName,
                                Name = useractual.FirstName + useractual.LastName,
                                Email = useractual.Email
                            });
                            TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                            var table = ApplicationDbContext.TableChanges.Find(22);
                            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                            var code = ApplicationDbContext.CodeLogs.Find(229);
                            var idcompany = UserCurrent.CompanyId;
                            if (idcompany != null)
                            {
                                var company = ApplicationDbContext.Companies.Find(idcompany);
                                string ip = IpUser();
                                var idchange = new IdChange
                                {
                                    IdCh_IdChange = getEnrollment.Enro_Id.ToString()
                                };
                                ApplicationDbContext.IdChanges.Add(idchange);
                                ApplicationDbContext.SaveChanges();
                                Log logsesiontrue = new Log
                                {
                                    ApplicationUser = UserCurrent,
                                    CoLo_Id = code.CoLo_Id,
                                    CodeLogs = code,
                                    Log_Date = DateTime.Now,
                                    Log_StateLogs = LOGSTATE.Realizado,
                                    TableChange = table,
                                    TaCh_Id = table.TaCh_Id,
                                    IdChange = idchange,
                                    IdCh_Id = idchange.IdCh_Id,
                                    User_Id = UserCurrent.Id,
                                    Log_Description = "El usuario con id: " + UserCurrent.Id + " Intento des matricular por ciudad el curso con id " + Modu_Id + " al usuario con id " + item.Id + " pero el usuario ya tiene avances en este curso, en la compañía con id" + company.CompanyId,
                                    Company = company,
                                    Company_Id = company.CompanyId,
                                    Log_Ip = ip
                                };
                                ApplicationDbContext.Logs.Add(logsesiontrue);
                                ApplicationDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            ListUserCancel1.Add(new user1
                            {
                                UserName = useractual.UserName,
                                Name = useractual.FirstName + useractual.LastName,
                                Email = useractual.Email
                            });
                            TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                            TempData["Info1"] = "Usuarios desmatriculados.";
                            var table = ApplicationDbContext.TableChanges.Find(22);
                            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                            var code = ApplicationDbContext.CodeLogs.Find(229);
                            var idcompany = UserCurrent.CompanyId;
                            if (idcompany != null)
                            {
                                var company = ApplicationDbContext.Companies.Find(idcompany);
                                string ip = IpUser();
                                var idchange = new IdChange
                                {
                                    IdCh_IdChange = getEnrollment.Enro_Id.ToString()
                                };
                                ApplicationDbContext.IdChanges.Add(idchange);
                                ApplicationDbContext.SaveChanges();
                                Log logsesiontrue = new Log
                                {
                                    ApplicationUser = UserCurrent,
                                    CoLo_Id = code.CoLo_Id,
                                    CodeLogs = code,
                                    Log_Date = DateTime.Now,
                                    Log_StateLogs = LOGSTATE.Realizado,
                                    TableChange = table,
                                    TaCh_Id = table.TaCh_Id,
                                    IdChange = idchange,
                                    IdCh_Id = idchange.IdCh_Id,
                                    User_Id = UserCurrent.Id,
                                    Log_Description = "El usuario con id: " + UserCurrent.Id + " Acaba de des matricular por ciudad el curso con id " + getEnrollment.Enro_Id + " al usuario con id " + getEnrollment.User_Id + ", en la compañía con id" + company.CompanyId,
                                    Company = company,
                                    Company_Id = company.CompanyId,
                                    Log_Ip = ip
                                };
                                ApplicationDbContext.Logs.Add(logsesiontrue);
                                ApplicationDbContext.SaveChanges();
                            }
                            ApplicationDbContext.Enrollments.Remove(getEnrollment);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        ListUserCancel2.Add(new user2
                        {
                            UserName = useractual.UserName,
                            Name = useractual.FirstName + useractual.LastName,
                            Email = useractual.Email
                        });
                        TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                        TempData["Info2"] = "Los siguientes usuarios no tienen matriculado este curso.";
                    }
                }
            }
            else
            {
                TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                TempData["Info3"] = "No hay usuarios asociados con este cargo";
            }
            List<Areas> ListAreas = new List<Areas>();
            var areas = ApplicationDbContext.Areas.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListAreas.Add(new Areas
            {
                Modu_Id = c,
                Listareas = areas
            });
            List<Positions> ListPositions = new List<Positions>();
            var positions = ApplicationDbContext.Position.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListPositions.Add(new Positions
            {
                Modu_Id = c,
                Listpositions = positions
            });
            List<Cities> ListCities = new List<Cities>();
            var city = ApplicationDbContext.City.ToList();
            ListCities.Add(new Cities
            {
                Modu_Id = c,
                Listcities = city
            });
            List<Locations> ListLocations = new List<Locations>();
            var location = ApplicationDbContext.Location.Where(x => x.CompanyId == GetModuleCompany).ToList();
            ListLocations.Add(new Locations
            {
                Modu_Id = c,
                Listlocations = location
            });
            AdminTrainingMassiveEnrollmentViewMode model = new AdminTrainingMassiveEnrollmentViewMode
            {
                ActualRole = GetActualUserId().Role,
                Module = getModule,
                ListAllUser = Listalluser,
                ListAreas = ListAreas,
                ListPositions = ListPositions,
                ListCitices = ListCities,
                ListLocations = ListLocations,
                ListUserCancel = ListUserCancel,
                ListUserCancel1 = ListUserCancel1,
                ListUserCancel2 = ListUserCancel2
            };
            model.Sesion = GetActualUserId().SesionUser;
            return View("CancelEnrollments", model);
        }

        [Authorize]
        public ActionResult EnrollmentLocations(AdminTrainingMassiveEnrollmentViewMode Enrollments)
        {
            var module = ApplicationDbContext.Modules.Find(Enrollments.Module.Modu_Id);
            var A = Enrollments.ListLocations.Take(1);
            int d = A.Select(x => x.Loca_Id).Single();
            var user = ApplicationDbContext.Users.Where(x => x.LocationId == d && x.Role == ROLES.Usuario).ToList();
            if (user.Count != 0)
            {
                foreach (var item in user)
                {
                    var useractual = ApplicationDbContext.Users.Find(item.Id);
                    var getEnrollment = useractual.Enrollment.Where(x => x.ApplicationUser.Id == useractual.Id && x.Module.Modu_Id == module.Modu_Id).ToList();
                    if (getEnrollment.Count == 0)
                    {
                        DateTime finish = new DateTime();
                        switch (module.Modu_Period)
                        {
                            case VIGENCIA.Dias:
                                finish = DateTime.Now.AddDays(module.Modu_Validity);
                                break;
                            case VIGENCIA.Meses:
                                finish = DateTime.Now.AddMonths(module.Modu_Validity);
                                break;
                            case VIGENCIA.Años:
                                finish = DateTime.Now.AddYears(module.Modu_Validity);
                                break;
                            default:
                                break;
                        }
                        DateTime b = finish;
                        TempData["Info"] = "Matricula Registrada";
                        Enrollment enrollment = new Enrollment { Module = module, ApplicationUser = useractual, Company = GetActualUserId().Company, Enro_InitDateModule = DateTime.Now, Enro_FinishDateModule = b, Enro_StateEnrollment = ENROLLMENTSTATE.Activo };
                        ApplicationDbContext.Enrollments.Add(enrollment);
                        ApplicationDbContext.SaveChanges();
                        var table = ApplicationDbContext.TableChanges.Find(22);
                        var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                        var code = ApplicationDbContext.CodeLogs.Find(225);
                        var idcompany = UserCurrent.CompanyId;
                        if (idcompany != null)
                        {
                            var company = ApplicationDbContext.Companies.Find(idcompany);
                            string ip = IpUser();
                            var idchange = new IdChange
                            {
                                IdCh_IdChange = enrollment.Enro_Id.ToString()
                            };
                            ApplicationDbContext.IdChanges.Add(idchange);
                            ApplicationDbContext.SaveChanges();
                            Log logsesiontrue = new Log
                            {
                                ApplicationUser = UserCurrent,
                                CoLo_Id = code.CoLo_Id,
                                CodeLogs = code,
                                Log_Date = DateTime.Now,
                                Log_StateLogs = LOGSTATE.Realizado,
                                TableChange = table,
                                TaCh_Id = table.TaCh_Id,
                                IdChange = idchange,
                                IdCh_Id = idchange.IdCh_Id,
                                User_Id = UserCurrent.Id,
                                Log_Description = "El usuario con id: " + UserCurrent.Id + " Acaba de matricular por ubicación el curso con id " + enrollment.Enro_Id + " al usuario con id " + enrollment.User_Id + ", en la compañía con id" + company.CompanyId,
                                Company = company,
                                Company_Id = company.CompanyId,
                                Log_Ip = ip
                            };
                            ApplicationDbContext.Logs.Add(logsesiontrue);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        TempData["Info"] = "Algunos usuarios ya tiene matriculada esta materia";
                        var table = ApplicationDbContext.TableChanges.Find(22);
                        var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                        var code = ApplicationDbContext.CodeLogs.Find(225);
                        var idcompany = UserCurrent.CompanyId;
                        if (idcompany != null)
                        {
                            var company = ApplicationDbContext.Companies.Find(idcompany);
                            string ip = IpUser();
                            var idchange = new IdChange
                            {
                                IdCh_IdChange = null
                            };
                            ApplicationDbContext.IdChanges.Add(idchange);
                            ApplicationDbContext.SaveChanges();
                            Log logsesiontrue = new Log
                            {
                                ApplicationUser = UserCurrent,
                                CoLo_Id = code.CoLo_Id,
                                CodeLogs = code,
                                Log_Date = DateTime.Now,
                                Log_StateLogs = LOGSTATE.Realizado,
                                TableChange = table,
                                TaCh_Id = table.TaCh_Id,
                                IdChange = idchange,
                                IdCh_Id = idchange.IdCh_Id,
                                User_Id = UserCurrent.Id,
                                Log_Description = "El usuario con id: " + UserCurrent.Id + " Intento matricular por ubicación el curso con id " + module.Modu_Id + " al usuario con id " + item.Id + "pero este usuario ya tienen matriculado este curso, en la compañía con id" + company.CompanyId,
                                Company = company,
                                Company_Id = company.CompanyId,
                                Log_Ip = ip
                            };
                            ApplicationDbContext.Logs.Add(logsesiontrue);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                TempData["Info"] = "No hay usuarios asociados con esta ubicación";
            }
            return RedirectToAction("Enrollments", new { Modu_Id = module.Modu_Id });
        }


        [Authorize]
        public ActionResult CancelEnrollmentLocations(AdminTrainingMassiveEnrollmentViewMode Enrollments)
        {
            var GetModuleCompany = GetActualUserId().CompanyId;
            var A = Enrollments.ListLocations.Take(1);
            int c = A.Select(x => x.Modu_Id).Single();
            int d = A.Select(x => x.Loca_Id).Single();
            List<user> ListUserCancel = new List<user>();
            List<user1> ListUserCancel1 = new List<user1>();
            List<user2> ListUserCancel2 = new List<user2>();
            var user = ApplicationDbContext.Users.Where(x => x.LocationId == d && x.Role == ROLES.Usuario).ToList();
            Module getModule = ApplicationDbContext.Modules.Find(c);
            List<AllUser> Listalluser = new List<AllUser>();
            foreach (var item in user)
            {
                Listalluser.Add(new AllUser
                {
                    User_Id = item.Id,
                    Modu_Id = c
                });
            }
            if (user.Count != 0)
            {
                foreach (var item in user)
                {
                    int Modu_Id = c;
                    string User_Id = item.Id;
                    var useractual = ApplicationDbContext.Users.Find(User_Id);
                    var module = ApplicationDbContext.Modules.Find(Modu_Id);
                    var getEnrollment = useractual.Enrollment.Where(x => x.ApplicationUser.Id == useractual.Id && x.Module.Modu_Id == module.Modu_Id).FirstOrDefault();
                    if (getEnrollment != null)
                    {
                        var advanceuser = ApplicationDbContext.AdvanceUsers.Where(x => x.User_Id == useractual.Id && x.TopicsCourse.Module.Modu_Id == getEnrollment.Module.Modu_Id).ToList();
                        var attempts = ApplicationDbContext.Attempts.Where(x => x.UserId == User_Id && x.BankQuestion.TopicsCourse.Modu_Id == getEnrollment.Modu_Id).ToList();
                        if (advanceuser.Count != 0 || attempts.Count != 0)
                        {
                            ListUserCancel.Add(new user
                            {
                                UserName = useractual.UserName,
                                Name = useractual.FirstName + useractual.LastName,
                                Email = useractual.Email
                            });
                            TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                            var table = ApplicationDbContext.TableChanges.Find(22);
                            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                            var code = ApplicationDbContext.CodeLogs.Find(230);
                            var idcompany = UserCurrent.CompanyId;
                            if (idcompany != null)
                            {
                                var company = ApplicationDbContext.Companies.Find(idcompany);
                                string ip = IpUser();
                                var idchange = new IdChange
                                {
                                    IdCh_IdChange = getEnrollment.Enro_Id.ToString()
                                };
                                ApplicationDbContext.IdChanges.Add(idchange);
                                ApplicationDbContext.SaveChanges();
                                Log logsesiontrue = new Log
                                {
                                    ApplicationUser = UserCurrent,
                                    CoLo_Id = code.CoLo_Id,
                                    CodeLogs = code,
                                    Log_Date = DateTime.Now,
                                    Log_StateLogs = LOGSTATE.Realizado,
                                    TableChange = table,
                                    TaCh_Id = table.TaCh_Id,
                                    IdChange = idchange,
                                    IdCh_Id = idchange.IdCh_Id,
                                    User_Id = UserCurrent.Id,
                                    Log_Description = "El usuario con id: " + UserCurrent.Id + " Intento des matricular por ubicación el curso con id " + Modu_Id + " al usuario con id " + item.Id + " pero el usuario ya tiene avances en este curso, en la compañía con id" + company.CompanyId,
                                    Company = company,
                                    Company_Id = company.CompanyId,
                                    Log_Ip = ip
                                };
                                ApplicationDbContext.Logs.Add(logsesiontrue);
                                ApplicationDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            ListUserCancel1.Add(new user1
                            {
                                UserName = useractual.UserName,
                                Name = useractual.FirstName + useractual.LastName,
                                Email = useractual.Email
                            });
                            TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                            TempData["Info1"] = "Usuarios desmatriculados.";
                            var table = ApplicationDbContext.TableChanges.Find(22);
                            var UserCurrent = ApplicationDbContext.Users.Find(GetActualUserId().Id);
                            var code = ApplicationDbContext.CodeLogs.Find(230);
                            var idcompany = UserCurrent.CompanyId;
                            if (idcompany != null)
                            {
                                var company = ApplicationDbContext.Companies.Find(idcompany);
                                string ip = IpUser();
                                var idchange = new IdChange
                                {
                                    IdCh_IdChange = getEnrollment.Enro_Id.ToString()
                                };
                                ApplicationDbContext.IdChanges.Add(idchange);
                                ApplicationDbContext.SaveChanges();
                                Log logsesiontrue = new Log
                                {
                                    ApplicationUser = UserCurrent,
                                    CoLo_Id = code.CoLo_Id,
                                    CodeLogs = code,
                                    Log_Date = DateTime.Now,
                                    Log_StateLogs = LOGSTATE.Realizado,
                                    TableChange = table,
                                    TaCh_Id = table.TaCh_Id,
                                    IdChange = idchange,
                                    IdCh_Id = idchange.IdCh_Id,
                                    User_Id = UserCurrent.Id,
                                    Log_Description = "El usuario con id: " + UserCurrent.Id + " Acaba de des matricular por ubicación el curso con id " + getEnrollment.Enro_Id + " al usuario con id " + getEnrollment.User_Id + ", en la compañía con id" + company.CompanyId,
                                    Company = company,
                                    Company_Id = company.CompanyId,
                                    Log_Ip = ip
                                };
                                ApplicationDbContext.Logs.Add(logsesiontrue);
                                ApplicationDbContext.SaveChanges();
                            }
                            ApplicationDbContext.Enrollments.Remove(getEnrollment);
                            ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        ListUserCancel2.Add(new user2
                        {
                            UserName = useractual.UserName,
                            Name = useractual.FirstName + useractual.LastName,
                            Email = useractual.Email
                        });
                        TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                        TempData["Info2"] = "Los siguientes usuarios no tienen matriculado este curso.";
                    }
                }
            }
            else
            {
                TempData["Info"] = "No se puede cancelar la matricula debido a que,los siguientes usuarios ya tienen avances en este curso.";
                TempData["Info3"] = "No hay usuarios asociados con este cargo";
            }
            List<Areas> ListAreas = new List<Areas>();
            var areas = ApplicationDbContext.Areas.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListAreas.Add(new Areas
            {
                Modu_Id = c,
                Listareas = areas
            });
            List<Positions> ListPositions = new List<Positions>();
            var positions = ApplicationDbContext.Position.Where(x => x.CompanyId == GetModuleCompany).ToList();

            ListPositions.Add(new Positions
            {
                Modu_Id = c,
                Listpositions = positions
            });
            List<Cities> ListCities = new List<Cities>();
            var city = ApplicationDbContext.City.ToList();
            ListCities.Add(new Cities
            {
                Modu_Id = c,
                Listcities = city
            });
            List<Locations> ListLocations = new List<Locations>();
            var location = ApplicationDbContext.Location.Where(x => x.CompanyId == GetModuleCompany).ToList();
            ListLocations.Add(new Locations
            {
                Modu_Id = c,
                Listlocations = location
            });
            AdminTrainingMassiveEnrollmentViewMode model = new AdminTrainingMassiveEnrollmentViewMode
            {
                ActualRole = GetActualUserId().Role,
                Module = getModule,
                ListAllUser = Listalluser,
                ListAreas = ListAreas,
                ListPositions = ListPositions,
                ListCitices = ListCities,
                ListLocations = ListLocations,
                ListUserCancel = ListUserCancel,
                ListUserCancel1 = ListUserCancel1,
                ListUserCancel2 = ListUserCancel2
            };
            model.Sesion = GetActualUserId().SesionUser;
            return View("CancelEnrollments", model);
        }
    }
}