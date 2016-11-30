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
                    List<Compliant> listComplaints = new List<Compliant>();
                    thisViewModel.thisComplainant = item;

                    //thisViewModel.thisComlaint
                    listComplaints = con.Query<Compliant>("SELECT * FROM Compliants WHERE Complainant_Id = @com_id", new { com_id = item.Id }).ToList();

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

        public ActionResult CreateCorporate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCorporate(CreateCorporate model)
        {
            return View();
        }

        public ActionResult CreateIndividual()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateIndividual(CreateIndividual model)
        {
            Complainant complainant = new Complainant();
            complainant = (Complainant) Session["Complainant"];

            InsertFunctions inserts = new InsertFunctions();

            if (complainant != null){
                int rep_id, advisor_id, complaint_id;
               
                //Save Complaint to Database
                try
                {
                    //COMPLAINT INSERT
                    model.complaint.Type = "Individual";
                    complaint_id = inserts.InsertComplaint(model.complaint, complainant.Id);
                    
                    //REPRESENTATION DOCUMENTS INSERT
                    string path = Server.MapPath("~") + "Rep_Data\\";
                    rep_id = inserts.InsertRepresentation(model.rep_doc, complaint_id, path);

                    //ADVISOR INSERT
                    advisor_id = inserts.InsertAdvisor(model.advisor, rep_id);
                }
                catch (Exception ex)
                {
                    //Log error as per your need 
                    throw ex;
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Complainant", null);
            }
            
        }

        [HttpPost]
        public ActionResult CreateComplaint(ComplaintViewModel model)
        {
            return View();
        }
    }
}
