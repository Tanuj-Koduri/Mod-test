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
    public partial class Home : Page // Changed from System.Web.UI.Page to Page
    {
        private readonly IConfiguration _configuration; // Added for dependency injection

        // Constructor for dependency injection
        public Home(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Use pattern matching for type checking
            if (Session["Roles"] is List<string> roles && !IsPostBack)
            {
                if (roles.Any(role => new[] { "Admin", "NormalUser", "BothRoles" }.Contains(role)))
                {
                    SetupPageBasedOnRole(roles);
                    BindComplaints();
                    DisplaySuccessMessage();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void SetupPageBasedOnRole(List<string> roles)
        {
            bool isAdmin = roles.Contains("Admin");
            bool isBoth = roles.Contains("BothRoles");

            // Use null-conditional operator and null-coalescing operator
            var actionTakenField = gvComplaints.Columns
                .OfType<TemplateField>()
                .FirstOrDefault(f => f.HeaderText == "Action Taken");

            actionTakenField?.HeaderText = (isAdmin || isBoth) ? "UpdateProgress" : "Current Status";

            pageTitle.InnerText = (isAdmin || isBoth) ? "Admin Dashboard - Complaints Management" : "My Complaints";

            gvComplaints.Columns[9].Visible = isAdmin || isBoth;

            // Use string interpolation
            lblWelcome.Text = $"Welcome, {Session["Email"] as string}!";
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
            // Use configuration injection instead of ConfigurationManager
            string connectionString = _configuration.GetConnectionString("YourConnectionString");

            List<string> roles = Session["Roles"] as List<string>;
            string email = Session["Email"] as string;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = GetComplaintsQuery(roles);
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (roles.Contains("NormalUser"))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                    }
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<ComplaintViewModel> complaints = ReadComplaints(reader);
                        gvComplaints.DataSource = complaints;
                        gvComplaints.DataBind();
                    }
                }
            }
        }

        private string GetComplaintsQuery(List<string> roles)
        {
            // Use string interpolation and ternary operator
            return roles.Contains("Admin") || roles.Contains("BothRoles")
                ? @"SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, 
                    PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, 
                    Comments, PictureUpload, ComplaintId, CurrentStatus, Status 
                    FROM Complaints ORDER BY Id DESC"
                : @"SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, 
                    PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, 
                    Comments, PictureUpload, ComplaintId, CurrentStatus, Status 
                    FROM Complaints WHERE Email = @Email ORDER BY Id DESC";
        }

        private List<ComplaintViewModel> ReadComplaints(SqlDataReader reader)
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
                    PictureUploads = reader["PictureUpload"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(Path.GetFileName).ToArray(),
                    CurrentStatus = reader["CurrentStatus"].ToString(),
                });
            }
            return complaints;
        }

        // Rest of the code remains largely unchanged, but consider:
        // 1. Using async/await for database operations
        // 2. Implementing proper error handling and logging
        // 3. Using parameterized queries for all database operations to prevent SQL injection
        // 4. Implementing proper input validation and sanitization
        // 5. Using a repository pattern or service layer to separate data access from business logic
        // 6. Considering the use of an ORM like Entity Framework for data access
        // 7. Implementing proper authorization checks throughout the application
        // 8. Using dependency injection for services and configuration
    }
}