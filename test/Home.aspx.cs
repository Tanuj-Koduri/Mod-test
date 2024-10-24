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
        // Use IConfiguration for modern configuration management
        private readonly IConfiguration _configuration;

        public Home(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Use pattern matching for type checking
            if (Session["Roles"] is List<string> roles && !IsPostBack)
            {
                // Use switch expression for role-based logic
                string userRole = roles.FirstOrDefault(r => r is "Admin" or "NormalUser" or "BothRoles");
                switch (userRole)
                {
                    case "Admin" or "BothRoles":
                        SetupAdminView(roles);
                        break;
                    case "NormalUser":
                        SetupNormalUserView();
                        break;
                    default:
                        Response.Redirect("Login.aspx");
                        return;
                }

                SetWelcomeMessage();
                BindComplaints();
                DisplaySuccessMessage();
            }
        }

        private void SetupAdminView(List<string> roles)
        {
            // Use null-conditional operator and null-coalescing operator
            var actionTakenField = gvComplaints.Columns
                .OfType<TemplateField>()
                .FirstOrDefault(f => f.HeaderText == "Action Taken");

            actionTakenField?.HeaderText = roles.Contains("Admin") || roles.Contains("BothRoles") 
                ? "UpdateProgress" 
                : "Current Status";

            pageTitle.InnerText = "Admin Dashboard - Complaints Management";
            gvComplaints.Columns[9].Visible = true;
        }

        private void SetupNormalUserView()
        {
            pageTitle.InnerText = "My Complaints";
            gvComplaints.Columns[9].Visible = false;
        }

        private void SetWelcomeMessage()
        {
            // Use string interpolation
            lblWelcome.Text = $"Welcome, {Session["Email"] as string}!";
        }

        private void DisplaySuccessMessage()
        {
            // Use null-conditional operator
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
            // Use modern configuration management
            string connectionString = _configuration.GetConnectionString("YourConnectionString");

            List<string> roles = Session["Roles"] as List<string>;
            string email = Session["Email"] as string;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Use string interpolation and ternary operator
                string query = roles.Contains("Admin") || roles.Contains("BothRoles")
                    ? "SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, CurrentStatus, Status FROM Complaints ORDER BY Id DESC"
                    : "SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, CurrentStatus, Status FROM Complaints WHERE Email = @Email ORDER BY Id DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (roles.Contains("NormalUser"))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                    }

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Use LINQ to read data
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
                                PictureUploads = reader["PictureUpload"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Path.GetFileName).ToArray(),
                                CurrentStatus = reader["CurrentStatus"].ToString(),
                            });
                        }

                        gvComplaints.DataSource = complaints;
                        gvComplaints.DataBind();
                    }
                }
            }
        }

        // ... (rest of the code remains largely unchanged)

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Use modern authentication methods if available
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("Login.aspx");
        }

        protected string GetUserRoleClass()
        {
            // Use pattern matching
            return User.IsInRole("Admin") || User.IsInRole("BothRoles") ? "admin" : "";
        }
    }
}