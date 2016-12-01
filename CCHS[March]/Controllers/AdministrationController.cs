using CCHS_March_.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using CCHS_March_.Models.View_Models;

namespace CCHS_March_.Controllers
{
    public class AdministrationController : Controller
    {

        //This declares the variable that Dapper will use to acces the database.
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        //
        // GET: /Administration/
        public ActionResult Index()
        {
            if (User.Identity.Name != null)
            {
                try
                {
                    con.Open();
                    
                    List<Compliant> listComplaints = new List<Compliant>();

                    //thisViewModel.thisComlaint
                    listComplaints = con.Query<Compliant>("SELECT * FROM Compliants").ToList();

                    con.Close();

                    return View(listComplaints);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "PLease login before taking action");
                return RedirectToAction("Login");
            }
        }

        public ActionResult ManageComplaint(int id)
        {
            try
            {
                con.Open();

                ManageViewModel viewModel = new ManageViewModel();
                viewModel.complaint = con.Query<Compliant>("SELECT * FROM Compliants WHERE ID = @ComID", new { ComID = id }).FirstOrDefault();
                int comID = con.Query<int>("SELECT Complainant_Id FROM Compliants WHERE ID = @ComID", new { ComID = id }).FirstOrDefault();
                viewModel.complainant = con.Query<Complainant>("SELECT * FROM Complainants WHERE ID = @ComID", new { ComID = comID }).FirstOrDefault();
                viewModel.body_corporate = con.Query<BodyCorporate>("SELECT * FROM BodyCorporates WHERE Compliant_Id = @compl_Id", new { compl_Id = id }).FirstOrDefault();
                viewModel.assesors = new List<UserViewModel>();
                viewModel.assesors = con.Query<UserViewModel>("SELECT Firstname, Lastname, Email, Department, Position FROM AspNetUsers WHERE UserName IN (SELECT Username FROM  Assistants WHERE Compliant_Id = @id)", new { id = id }).ToList();
                
                Representation rep = new Representation();
                rep = con.Query<Representation>("SELECT * FROM Representations WHERE ID = @ComID", new { ComID = comID }).FirstOrDefault();

                //viewModel.rep_doc = File(rep.Proof_of_Rep + "RepDoc_" + id, System.Net.Mime.MediaTypeNames.Application.Octet);

                ViewBag.repId = rep.Id;

                viewModel.advisor = con.Query<Advisor>("SELECT * FROM Advisors WHERE Representation_Id = @rep_id", new { rep_id = rep.Id }).FirstOrDefault();

                con.Close();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetUsers(int id)
        {
            try
            {
                con.Open();

                List<string> list = con.Query<string>("SELECT UserName FROM AspNetUsers WHERE (UserName NOT IN (SELECT Username FROM Assistants WHERE Compliant_Id = @id))", new { id = id}).ToList();
                //{
                //    FirstName = "Bob",
                //    LastName = "Cravens",
                //    Nationality = "Namibian",
                //    Tel_Number = "061258741",
                //    FaxNumber = "0612583001",
                //    EmailAddress = "maaszdonovan@gmail.com",
                //    PO_Box = "P.O. Box 277 Maltahohe Namibia"
                //};

                return Json(new { list = list }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("false");
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SaveAssesors(List<String> values, int id)
        {
            try
            {
                foreach (var item in values)
                {
                    con.Execute("INSERT INTO Assistants (Username, Compliant_Id) VALUES ( @username, @id) ", new { username = item, id = id });
                }

                return Json(new { Result = "Successfull Updated" });
            }
            catch(Exception)
            {
                return Json(new { Result = "Update Failed" });
            }
        }
    }
}