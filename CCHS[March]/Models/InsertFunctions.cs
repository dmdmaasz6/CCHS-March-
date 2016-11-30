using CCHS_March_.Models.Data_Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace CCHS_March_.Models
{
    public class InsertFunctions
    {

        //This declares the variable that Dapper will use to acces the database.
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public int InsertComplaint(Compliant model, int id)
        {
            //Save Complaint to Database
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@type", model.Type);
                param.Add("@subjectMAtter", model.SubjectMatter);
                param.Add("@details", model.Details);
                param.Add("@matterEscalation", model.MatterEscalation);
                param.Add("@purpose", model.Purpose);
                param.Add("@purposedOutcome", model.PurposedOutcome);
                param.Add("@yearExpla", model.YearExplanation);
                param.Add("@consent", model.ConsentToInvestigation);
                param.Add("@complainant_id", id);
                param.Add("@heading", model.SubjectMatter.Substring(0, 10));
                param.Add("@status", "Submitted for Review");
                param.Add("@date", System.DateTime.Now);
                param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                con.Open();
                con.Execute("complaint_insert", param, commandType: CommandType.StoredProcedure);
                con.Close();

                int complaint_id = param.Get<int>("@id");

                return complaint_id;
            }
            catch (Exception ex)
            {
                //Log error as per your need 
                return 0;
            }

        }

        public int InsertRepresentation(HttpPostedFileBase rep_doc, int id, string path)
        {
            try
            {
                DataConversion conversion = new DataConversion();

                DynamicParameters param = new DynamicParameters();

                path = path + rep_doc.FileName;
                rep_doc.SaveAs(path);

                param.Add("@proof_of_rep", path);
                param.Add("@comp_id", id);
                param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                con.Open();
                con.Execute("representation_insert", param, commandType: CommandType.StoredProcedure);
                con.Close();

                int rep_id = param.Get<int>("@id");

                return rep_id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int InsertAdvisor(Advisor model, int id)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@firstname", model.FirstName);
                param.Add("@lastname", model.LastName);
                param.Add("@address", model.Address);
                param.Add("@work_tel", model.WorkTel);
                param.Add("@cell", model.Cell);
                param.Add("@email", model.EmailAddress);
                param.Add("@rep_id", id);
                param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                con.Open();
                con.Execute("advisor_insert", param, commandType: CommandType.StoredProcedure);
                con.Close();

                int advisor_id = param.Get<int>("@id");

                return advisor_id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void InsertBodyCorporate()
        {

        }

        public int InsertResponse(Customer_Response model, int complaint_id)
        {
            try
            {
                model.Date_Created = System.DateTime.Now;

                DynamicParameters param = new DynamicParameters();
                param.Add("@subject", model.Subject);
                param.Add("@body", model.Message_Body);
                param.Add("@date", model.Date_Created);
                param.Add("@parent", model.Parent_Message_Id);
                param.Add("@complaint_id", complaint_id);
                param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                con.Open();
                con.Execute("response_insert", param, commandType: CommandType.StoredProcedure);
                con.Close();

                int response_id = param.Get<int>("@id");

                return response_id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int InsertAttachment(HttpPostedFileBase res_doc, int response_id, string path)
        {
            try
            {
                path = path + res_doc.FileName;
                res_doc.SaveAs(path);

                DynamicParameters param = new DynamicParameters();
                param.Add("@loc", path);
                param.Add("@date", System.DateTime.Now);
                param.Add("@response", response_id);
                param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                con.Open();
                con.Execute("attachment_insert", param, commandType: CommandType.StoredProcedure);
                con.Close();

                int attachment_id = param.Get<int>("@id");

                return attachment_id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}