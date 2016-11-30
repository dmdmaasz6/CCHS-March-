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
using System.Data;
using CCHS_March_.Models;

namespace CCHS_March_.Controllers
{
    public class ComplainantController : Controller
    {
        private CCH_EntitiesContainer db = new CCH_EntitiesContainer();

        //This declares the variable that Dapper will use to acces the database.
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public ActionResult Responses(int id)
        {
            List<Customer_Response> thisList = new List<Customer_Response>();

            thisList = con.Query<Customer_Response>("SELECT * FROM Customer_Response WHERE Compliant_Id = @id", new { id = id }).ToList();

            Session["Complaint"] = id;

            return PartialView("_Responses", thisList);
        }

        public ActionResult CreateResponse()
        {
            return PartialView("_SubmitResponse");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitResponse(SubmitResponseViewModel model)
        {
            try
            {
                InsertFunctions inserts = new InsertFunctions();
                int complaint_id = (int) Session["Complaint"];

                int response_id = inserts.InsertResponse(model.response, complaint_id);

                if (response_id != 0)
                {
                    foreach (var item in model.attachments)
                    {
                        //ATTACHEMENTS DOCUMENTS INSERT
                        string path = Server.MapPath("~") + "attachments\\";
                        int attachment_id = inserts.InsertAttachment(item, response_id, path);
                    }
                }

                return View("Index");
            }
            catch (Exception)
            {
                return View("Index");
            }
        }

        public ActionResult ComplainantComplaints()
        {
            if (Session["Complainant"] != null)
            {
                try
                {
                    con.Open();

                    Complainant complainant = new Complainant();
                    complainant = (Complainant)Session["Complainant"];

                    List<Compliant> listComplaints = new List<Compliant>();

                    //thisViewModel.thisComlaint
                    listComplaints = con.Query<Compliant>("SELECT * FROM Compliants WHERE Complainant_Id = @com_id", new { com_id = complainant.Id }).ToList();

                    con.Close();

                    return PartialView("_Complaints", listComplaints);
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

        public ActionResult Index()
        {
            if (Session["Complainant"] != null)
            {
                try
                {
                    con.Open();

                    Complainant complainant = new Complainant();
                    complainant = (Complainant) Session["Complainant"];

                    List<Compliant> listComplaints = new List<Compliant>();

                    //thisViewModel.thisComlaint
                    listComplaints = con.Query<Compliant>("SELECT * FROM Compliants WHERE Complainant_Id = @com_id", new { com_id = complainant.Id }).ToList();

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

        public ActionResult Logout()
        {
            Session.Remove("Complainant");
            return RedirectToAction("Login");
        }

        //
        // GET: /Complainant/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ComplainantAccessViewModel model)
        {
            if (ModelState.IsValid)
            {

                Complainant checkModel = new Complainant();
                checkModel = con.Query<Complainant>("SELECT * FROM Complainants WHERE National_Id = @id", new { id = model.National_Id }).SingleOrDefault();

                if (checkModel != null)
                {
                    Session["Complainant"] = checkModel;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect Account Details");
                    return View(model);
                }

                
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(ComplainantViewModel model)
        {
            //Save Complainant to Databse
            //if (model.accFlag == null)
            //{
            if (ModelState.IsValid)
            {
                try
                {

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@firstname", model.FirstName);
                    param.Add("@lastname", model.LastName);
                    param.Add("@national_id", model.Id_Number);
                    param.Add("@nationality", model.Nationality);
                    param.Add("@po_box", model.PO_Box);
                    param.Add("@tel_number", model.Tel_Number);
                    param.Add("@fax_number", model.FaxNumber);
                    param.Add("@email", model.EmailAddress);

                    //int accStart = con.Query<int>("SELECT IDENT_CURRENT( 'Complainants' ) AS AccNum").FirstOrDefault();
                    int flag = 0;
                    string accNum = "";

                    while (flag == 0)
                    {
                        accNum = GetRandomString(8);
                        if (con.Query<Complainant>("SELECT * FROM COMPLAINANTS WHERE AccountNumber = @num", new { num = accNum }).SingleOrDefault() == null)
                        {
                            flag = 1;
                        }
                    }

                    param.Add("@accNum", accNum);

                    param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    con.Execute("complainant_insert", param, commandType: CommandType.StoredProcedure);
                    con.Close();

                    int complainant_id = param.Get<int>("@id");

                }
                catch (Exception ex)
                {
                    //Log error as per your need 
                    throw ex;
                }
                //}
                //else
                //{
                //    complainant_id = model.complainant.Id;
                //}

                return View();
            }
            else
            {
                return View(model);
            }
        }

        public string GetRandomString(int length)
        {
            string[] array = new string[54]
	        {
		        "0","2","3","4","5","6","8","9",
		        "a","b","c","d","e","f","g","h","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z",
		        "A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y","Z"
	        };
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(array[GetRandomNumber(53)]);
            return sb.ToString();
        }

        public int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new System.Exception("The maxNumber value should be greater than 1");
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            System.Random r = new System.Random(seed);
            return r.Next(1, maxNumber);
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

                if (compInfo != null)
                {
                    return Json("true");
                }
                else
                {
                    return Json("false");
                }
                
            }
            catch (Exception e)
            {
                return Json("false");
            }

        }


	}
}