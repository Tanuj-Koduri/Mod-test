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

        // Constructor injection for configuration
        public Home(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Use pattern matching for type checking and null check
            if (Session["Roles"] is List<string> roles && roles.Any())
            {
                if (!IsPostBack)
                {
                    // Use switch expression for role-based logic
                    string userRole = roles.Contains("Admin") ? "Admin" :
                                      roles.Contains("BothRoles") ? "BothRoles" :
                                      roles.Contains("NormalUser") ? "NormalUser" : "Unknown";

                    ConfigureUIBasedOnRole(userRole);
                    BindComplaints();
                    DisplaySuccessMessage();
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        private void ConfigureUIBasedOnRole(string userRole)
        {
            bool isAdmin = userRole == "Admin" || userRole == "BothRoles";

            // Use null-conditional operator and null-coalescing operator
            var actionTakenField = gvComplaints.Columns
                .OfType<TemplateField>()
                .FirstOrDefault(f => f.HeaderText == "Action Taken");

            actionTakenField?.HeaderText = isAdmin ? "UpdateProgress" : "Current Status";

            pageTitle.InnerText = isAdmin ? "Admin Dashboard - Complaints Management" : "My Complaints";

            gvComplaints.Columns[9].Visible = isAdmin;

            // Use string interpolation
            lblWelcome.Text = $"Welcome, {Session["Email"] as string ?? "User"}!";
        }

        private void DisplaySuccessMessage()
        {
            if (Session["SuccessMessage"] is string successMessage)
            {
                lblSucessMessage.Text = successMessage;
                lblSucessMessage.Visible = true;
                Session["SuccessMessage"] = null;
            }
        }

        private void BindComplaints()
        {
            // Use modern configuration management
            string connectionString = _configuration.GetConnectionString("YourConnectionString");

            var roles = Session["Roles"] as List<string>;
            string email = Session["Email"] as string;

            using (var conn = new SqlConnection(connectionString))
            {
                // Use string interpolation and ternary operator for query
                string query = $@"SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, 
                                  DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + 
                                  Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, 
                                  CurrentStatus, Status 
                                  FROM Complaints 
                                  {(roles.Contains("Admin") || roles.Contains("BothRoles") ? "" : "WHERE Email = @Email")} 
                                  ORDER BY Id DESC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (roles.Contains("NormalUser"))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                    }

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var complaints = new List<ComplaintViewModel>();

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
                                PictureUploads = reader["PictureUpload"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(System.IO.Path.GetFileName).ToArray(),
                                CurrentStatus = reader["CurrentStatus"].ToString(),
                            });
                        }

                        gvComplaints.DataSource = complaints;
                        gvComplaints.DataBind();
                    }
                }
            }
        }

        // Rest of the code remains largely the same, with minor improvements in syntax and readability
        // ...

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // Clear all session data
            FormsAuthentication.SignOut();
            Response.Redirect("Login.aspx");
        }

        protected string GetUserRoleClass() => User.IsInRole("Admin") || User.IsInRole("BothRoles") ? "admin" : "";
    }
}