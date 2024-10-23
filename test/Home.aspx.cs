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
            var roles = Session["Roles"] as List<string> ?? new List<string>(); // Null coalescing operator

            if (!IsPostBack)
            {
                if (roles.Any(r => new[] { "Admin", "NormalUser", "BothRoles" }.Contains(r))) // LINQ Any and Contains
                {
                    bool isAdmin = roles.Contains("Admin");
                    bool isBoth = roles.Contains("BothRoles");

                    var actionTakenField = gvComplaints.Columns
                        .OfType<TemplateField>()
                        .FirstOrDefault(f => f.HeaderText == "Action Taken");

                    if (actionTakenField != null)
                    {
                        actionTakenField.HeaderText = (isAdmin || isBoth) ? "UpdateProgress" : "Current Status";
                    }

                    pageTitle.InnerText = (isAdmin || isBoth) ? "Admin Dashboard - Complaints Management" : "My Complaints";

                    gvComplaints.Columns[9].Visible = roles.Contains("Admin") || roles.Contains("BothRoles");

                    string email = Session["Email"] as string;
                    lblWelcome.Text = $"Welcome, {email}!"; // String interpolation

                    BindComplaints();
                    DisplaySuccessMessage();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void DisplaySuccessMessage()
        {
            if (Session["SuccessMessage"] is string successMessage && !string.IsNullOrEmpty(successMessage))
            {
                lblSucessMessage.Text = successMessage;
                lblSucessMessage.Visible = true;
                Session["SuccessMessage"] = null;
            }
        }

        private void BindComplaints()
        {
            string connectionString = _configuration.GetConnectionString("YourConnectionString"); // Using IConfiguration

            var roles = Session["Roles"] as List<string> ?? new List<string>();
            string email = Session["Email"] as string;

            using (var conn = new SqlConnection(connectionString))
            {
                string query = roles.Contains("Admin") || roles.Contains("BothRoles")
                    ? "SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, CurrentStatus, Status FROM Complaints ORDER BY Id DESC"
                    : "SELECT Id, FirstName + ' ' + LastName AS Name, EmpId, Email, ContactNumber, DateTimeCapture, PictureCaptureLocation + ' ' + StreetAddress1 + ' ' + City + ', ' + Zip + ' ' + State AS PictureCaptureLocation, Comments, PictureUpload, ComplaintId, CurrentStatus, Status FROM Complaints WHERE Email = @Email ORDER BY Id DESC";

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

        // ... (rest of the code remains largely unchanged)

        private void UpdateComplaintCurrentStatus(string complaintId, string status)
        {
            string connectionString = _configuration.GetConnectionString("YourConnectionString");
            string query = "UPDATE Complaints SET CurrentStatus = @CurrentStatus WHERE ComplaintId = @ComplaintId";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CurrentStatus", status);
                cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ... (remaining methods)
    }
}