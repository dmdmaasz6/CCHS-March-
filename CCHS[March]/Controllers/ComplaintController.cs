using CCHS_March_.Models.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCHS_March_.Models.Data_Models;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using CCHS_March_.Models;
using System.IO;

namespace CCHS_March_.Models
{
    public class ComplaintController : Controller
    {
        private CCH_EntitiesContainer db = new CCH_EntitiesContainer();

        //This declares the variable that Dapper will use to acces the database.
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        //
        // GET: /Complaint/
        public ActionResult Index()
        {
            List<ComplaintIndexViewModel> viewList = new List<ComplaintIndexViewModel>();

            try
            {
                con.Open();

                List<Complainant> complainants = new List<Complainant>();
                complainants = con.Query<Complainant>("SELECT * FROM Complainants").ToList();

                foreach (var item in complainants)
                {
                    ComplaintIndexViewModel thisViewModel = new ComplaintIndexViewModel();
                    List<Compliant> listComplaints  = new List<Compliant>();
                    thisViewModel.thisComplainant = item;

                    //thisViewModel.thisComlaint
                    listComplaints = con.Query<Compliant>("SELECT * FROM Compliants WHERE Complainant_Id = @com_id", new { com_id = item.Id}).ToList();

                    foreach (var item1 in listComplaints)
	                {
                        thisViewModel.thisComlaint = item1;
                        viewList.Add(thisViewModel);   
	                }
                }

                con.Close();

                return View(viewList);
            }
            catch (Exception)
            {

                throw;
            }
        }



        //
        // GET: /Complaint/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                con.Open();

                ComplaintViewModel viewModel = new ComplaintViewModel();
                viewModel.complaint = con.Query<Compliant>("SELECT * FROM Compliants WHERE ID = @ComID", new { ComID = id }).FirstOrDefault();
                int comID = con.Query<int>("SELECT Complainant_Id FROM Compliants WHERE ID = @ComID", new { ComID = id }).FirstOrDefault();
                viewModel.complainant = con.Query<Complainant>("SELECT * FROM Complainants WHERE ID = @ComID", new { ComID = comID }).FirstOrDefault();
                viewModel.body_corporate = con.Query<BodyCorporate>("SELECT * FROM BodyCorporates WHERE Compliant_Id = @compl_Id", new { compl_Id = id }).FirstOrDefault();
                
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

        public FilePathResult GetFile(int id)
        {
            Representation rep = new Representation();
            using (con)
            {
                con.Open();
                rep = con.Query<Representation>("SELECT * FROM Representations WHERE ID = @ComID", new { ComID = id }).FirstOrDefault();
                con.Close();
            }
            return File(rep.Proof_of_Rep, System.Net.Mime.MediaTypeNames.Application.Pdf); ;
        }

        //
        // GET: /Complaint/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Complaint/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Complaint/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Complaint/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateComplaint()
        {
            ComplaintViewModel model = new ComplaintViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateComplaint(ComplaintViewModel model)
        {
            int body_id, complainant_id, rep_id, advisor_id, complaint_id;
            try
            {
                // TODO: This is the data saving logic
                
                //Save Complainant to Databse
                if (model.accFlag == null)
                {
                    try
                    {

                        DynamicParameters param = new DynamicParameters();
                        param.Add("@firstname", model.complainant.FirstName);
                        param.Add("@lastname", model.complainant.LastName);
                        param.Add("@nationality", model.complainant.Nationality);
                        param.Add("@po_box", model.complainant.PO_Box);
                        param.Add("@tel_number", model.complainant.Tel_Number);
                        param.Add("@fax_number", model.complainant.FaxNumber);
                        param.Add("@email", model.complainant.EmailAddress);

                        int accStart = con.Query<int>("SELECT IDENT_CURRENT( 'Complainants' ) AS AccNum").FirstOrDefault();
                        param.Add("@accNum", "ECB-ACC" + (accStart + 1));

                        param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                        con.Execute("complainant_insert", param, commandType: CommandType.StoredProcedure);
                        con.Close();

                        complainant_id = param.Get<int>("@id");

                    }
                    catch (Exception ex)
                    {
                        //Log error as per your need 
                        throw ex;
                    }
                }
                else
                {
                    complainant_id = model.complainant.Id;
                }

                //Save Complaint to Database
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@subjectMAtter", model.complaint.SubjectMatter);
                    param.Add("@details", model.complaint.Details);
                    param.Add("@matterEscalation", model.complaint.MatterEscalation);
                    param.Add("@purpose", model.complaint.Purpose);
                    param.Add("@purposedOutcome", model.complaint.PurposedOutcome);
                    param.Add("@yearExpla", model.complaint.YearExplanation);
                    param.Add("@consent", model.complaint.ConsentToInvestigation);
                    param.Add("@complainant_id", complainant_id);
                    param.Add("@heading", model.complaint.SubjectMatter.Substring(0,10));
                    param.Add("@status", "Submitted for Review");
                    param.Add("@date", System.DateTime.Now);
                    param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    con.Open();
                    con.Execute("complaint_insert", param, commandType: CommandType.StoredProcedure);
                    con.Close();

                    complaint_id = param.Get<int>("@id");

                }
                catch (Exception ex)
                {
                    //Log error as per your need 
                    throw ex;
                }

                //Save Body Corporate to Database
                try
                {

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@Country", model.body_corporate.CountryofOrigin);
                    param.Add("@Registration_Number", model.body_corporate.Registration_Number);
                    param.Add("@Body_Name", model.body_corporate.Body_Name);
                    param.Add("@complaint_id", complaint_id);
                    param.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    con.Open();
                    con.Execute("body_corporate_Insert", param, commandType: CommandType.StoredProcedure);
                    con.Close();

                    body_id = param.Get<int>("@Id");

                }
                catch (Exception ex)
                {
                    //Log error as per your need 
                    throw ex;
                }

                //Save Representation to Database
                try
                {
                    DataConversion conversion = new DataConversion();

                    DynamicParameters param = new DynamicParameters();

                    string path = Server.MapPath("~") + "Rep_Data\\" + model.rep_doc.FileName;
                    model.rep_doc.SaveAs(path);

                    param.Add("@proof_of_rep", path);
                    param.Add("@comp_id", complaint_id);
                    param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    con.Open();
                    con.Execute("representation_insert", param, commandType: CommandType.StoredProcedure);
                    con.Close();

                    rep_id = param.Get<int>("@id");

                }
                catch (Exception ex)
                {
                    //Log error as per your need 
                    throw ex;
                }

                //Save Advisor to Database
                try
                {

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@firstname", model.advisor.FirstName);
                    param.Add("@lastname", model.advisor.LastName);
                    param.Add("@address", model.advisor.Address);
                    param.Add("@work_tel", model.advisor.WorkTel);
                    param.Add("@cell", model.advisor.Cell);
                    param.Add("@email", model.advisor.EmailAddress);
                    param.Add("@rep_id", rep_id);
                    param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    con.Open();
                    con.Execute("advisor_insert", param, commandType: CommandType.StoredProcedure);
                    con.Close();

                    advisor_id = param.Get<int>("@id");

                }
                catch (Exception ex)
                {
                    //Log error as per your need 
                    throw ex;
                }


                return RedirectToAction("CreateComplaint");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetComplainantInfo(string accountNumber)
        {
            try
            {
                con.Open();
                
                Complainant compInfo = new Complainant();
                compInfo = con.Query<Complainant>("SELECT * FROM Complainants WHERE AccountNumber = @accNum", new { accNum = accountNumber }).FirstOrDefault();
                //{
                //    FirstName = "Bob",
                //    LastName = "Cravens",
                //    Nationality = "Namibian",
                //    Tel_Number = "061258741",
                //    FaxNumber = "0612583001",
                //    EmailAddress = "maaszdonovan@gmail.com",
                //    PO_Box = "P.O. Box 277 Maltahohe Namibia"
                //};
                return Json(compInfo);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
