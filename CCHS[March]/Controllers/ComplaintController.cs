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
                    thisViewModel.thisComplainant = item;
                    thisViewModel.thisComlaint = con.Query<Compliant>("SELECT * FROM Compliants WHERE Id = @com_id", new { com_id = item.Id}).SingleOrDefault();

                    viewList.Add(thisViewModel);

                }

                con.Close();

                return View(viewList);
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }



        //
        // GET: /Complaint/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                    param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    con.Open();
                    con.Execute("complainant_insert", param, commandType: CommandType.StoredProcedure);
                    con.Close();

                    complainant_id = param.Get<int>("@id");

                }
                catch (Exception ex)
                {
                    //Log error as per your need 
                    throw ex;
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

                    string path = Server.MapPath("~") + "\\Rep_Data\\" + model.rep_doc.FileName;
                    model.rep_doc.SaveAs(path);

                    param.Add("@proof_of_rep", path);
                    param.Add("@comp_id", complainant_id);
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
                    param.Add("@firstname", model.complainant.FirstName);
                    param.Add("@lastname", model.complainant.LastName);
                    param.Add("@address", model.complainant.Nationality);
                    param.Add("@work_tel", model.complainant.PO_Box);
                    param.Add("@cell", model.complainant.Tel_Number);
                    param.Add("@email", model.complainant.FaxNumber);
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
    }
}
