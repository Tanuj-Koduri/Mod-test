using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.Security;
using Microsoft.Extensions.Configuration; // Added for modern configuration management

namespace PimsApp
{
    public partial class Home : Page
    {
        private readonly IConfiguration _configuration; // Added for dependency injection

        // Constructor for dependency injection
        public Home(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Use null-conditional operator for cleaner null checks
            var roles = Session["Roles"] as List<string>;

            if (!IsPostBack)
            {
                // Use pattern matching for cleaner role checks
                if (roles?.Any(role => role is "Admin" or "NormalUser" or "BothRoles") == true)
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

            // Use var for implicit typing
            var actionTakenField = gvComplaints.Columns
                .OfType<TemplateField>()
                .FirstOrDefault(f => f.HeaderText == "Action Taken");

            if (actionTakenField != null)
            {
                actionTakenField.HeaderText = (isAdmin || isBoth) ? "UpdateProgress" : "Current Status";
            }

            pageTitle.InnerText = (isAdmin || isBoth) ? "Admin Dashboard - Complaints Management" : "My Complaints";

            gvComplaints.Columns[9].Visible = isAdmin || isBoth;

            // Use string interpolation for cleaner string formatting
            lblWelcome.Text = $"Welcome, {Session["Email"]}!";
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
            // Use ConfigurationManager for connection string
            string connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

            var roles = Session["Roles"] as List<string>;
            string email = Session["Email"] as string;

            using (var conn = new SqlConnection(connectionString))
            {
                string query = GetComplaintsQuery(roles);
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
                            complaints.Add(CreateComplaintFromReader(reader));
                        }
                        gvComplaints.DataSource = complaints;
                        gvComplaints.DataBind();
                    }
                }
            }
        }

        private string GetComplaintsQuery(List<string> roles)
        {
            // Use ternary operator for conditional query
            return roles.Contains("Admin") || roles.Contains("BothRoles")
                ? "SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, CurrentStatus, Status FROM Complaints ORDER BY Id DESC"
                : "SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, CurrentStatus, Status FROM Complaints WHERE Email = @Email ORDER BY Id DESC";
        }

        private ComplaintViewModel CreateComplaintFromReader(SqlDataReader reader)
        {
            return new ComplaintViewModel
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
                                                                   .Select(Path.GetFileName)
                                                                   .ToArray(),
                CurrentStatus = reader["CurrentStatus"].ToString(),
            };
        }

        // ... (rest of the code remains largely unchanged)

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("Login.aspx");
        }

        protected string GetUserRoleClass()
        {
            // Use expression-bodied member for simple methods
            return User.IsInRole("Admin") || User.IsInRole("BothRoles") ? "admin" : "";
        }
    }
}