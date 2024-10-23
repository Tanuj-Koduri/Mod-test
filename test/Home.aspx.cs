using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Security;
using Microsoft.Extensions.Configuration; // Added for modern configuration management

namespace PimsApp
{
    public partial class Home : Page
    {
        private readonly IConfiguration _configuration; // Added for dependency injection

        // Constructor injection for IConfiguration
        public Home(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Use null-conditional operator and pattern matching
            if (Session["Roles"] is List<string> roles && !IsPostBack)
            {
                // Use switch expression for role-based logic
                string userRole = roles switch
                {
                    var r when r.Contains("Admin") => "Admin",
                    var r when r.Contains("NormalUser") => "NormalUser",
                    var r when r.Contains("BothRoles") => "BothRoles",
                    _ => null
                };

                if (userRole != null)
                {
                    ConfigureUI(userRole);
                    BindComplaints();
                    DisplaySuccessMessage();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void ConfigureUI(string userRole)
        {
            bool isAdmin = userRole == "Admin" || userRole == "BothRoles";

            // Use null-conditional operator and null-coalescing operator
            if (gvComplaints.Columns.OfType<TemplateField>().FirstOrDefault(f => f.HeaderText == "Action Taken") is TemplateField actionTakenField)
            {
                actionTakenField.HeaderText = isAdmin ? "UpdateProgress" : "Current Status";
            }

            pageTitle.InnerText = isAdmin ? "Admin Dashboard - Complaints Management" : "My Complaints";
            gvComplaints.Columns[9].Visible = isAdmin;

            // Use string interpolation
            lblWelcome.Text = $"Welcome, {Session["Email"] as string ?? "User"}!";
        }

        private void DisplaySuccessMessage()
        {
            // Use null-conditional operator and null-coalescing operator
            string successMessage = Session["SuccessMessage"] as string;
            if (!string.IsNullOrEmpty(successMessage))
            {
                lblSucessMessage.Text = successMessage;
                lblSucessMessage.Visible = true;
                Session["SuccessMessage"] = null;
            }
        }

        private void BindComplaints()
        {
            // Use ConfigurationBuilder for modern configuration management
            string connectionString = _configuration.GetConnectionString("YourConnectionString");

            List<string> roles = Session["Roles"] as List<string>;
            string email = Session["Email"] as string;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Use string interpolation and ternary operator
                string query = roles.Contains("Admin") || roles.Contains("BothRoles")
                    ? $"SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, CurrentStatus, Status FROM Complaints ORDER BY Id DESC"
                    : $"SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, CurrentStatus, Status FROM Complaints WHERE Email = @Email ORDER BY Id DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (roles.Contains("NormalUser"))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                    }

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<ComplaintViewModel> complaints = new List<ComplaintViewModel>();

                        while (reader.Read())
                        {
                            complaints.Add(new ComplaintViewModel
                            {
                                Id = reader["Id"].ToString(),
                                ComplaintId = reader["ComplaintId"].ToString(),
                                Name = reader["Name"].ToString(),
                                EmpId = reader["EmpId"].ToString(),
                                Email = reader["Email"].ToString(),
                                ContactNumber = reader["ContactNumber"].ToString(),
                                DateTimeCapture = Convert.ToDateTime(reader["DateTimeCapture"]),
                                PictureCaptureLocation = reader["PictureCaptureLocation"].ToString(),
                                Comments = reader["Comments"].ToString(),
                                Status = reader["Status"].ToString(),
                                PictureUploads = reader["PictureUpload"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(System.IO.Path.GetFileName).ToArray(),
                                CurrentStatus = reader["CurrentStatus"].ToString(),
                            });
                        }

                        gvComplaints.DataSource = complaints;
                        gvComplaints.DataBind();
                    }
                }
            }
        }

        // Rest of the code remains largely unchanged, but can be further optimized using similar techniques
    }
}