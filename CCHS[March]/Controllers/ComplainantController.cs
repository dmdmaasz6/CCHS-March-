using CCHS_March_.Models.Data_Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCHS_March_.Controllers
{
    public class ComplainantController : Controller
    {
        private CCH_EntitiesContainer db = new CCH_EntitiesContainer();

        //This declares the variable that Dapper will use to acces the database.
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);


        //
        // GET: /Complainant/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string accountNum)
        {
            return View();
        }
	}
}