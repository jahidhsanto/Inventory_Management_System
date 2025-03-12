using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Serialization;

namespace STORE_FINAL
{
    public class GetMaterials : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string searchTerm = context.Request["searchTerm"] ?? "";

            List<object> materials = new List<object>();

            // Debugging: log the searchTerm to see what is being passed
            System.Diagnostics.Debug.WriteLine("Search Term: " + searchTerm);

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Material_ID, Name FROM Material WHERE Name LIKE @SearchTerm + '%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        materials.Add(new
                        {
                            MaterialID = reader["Material_ID"],
                            Name = reader["Name"]
                        });
                    }
                }
                catch (Exception ex)
                {
                    // If there's an exception, log it and return an error response
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                    context.Response.StatusCode = 500; // Internal Server Error
                    context.Response.Write("{\"error\":\"" + ex.Message + "\"}");
                    return;
                }
            }

            // Log the results being returned for debugging
            System.Diagnostics.Debug.WriteLine("Materials: " + materials.Count);

            JavaScriptSerializer js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(materials));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
