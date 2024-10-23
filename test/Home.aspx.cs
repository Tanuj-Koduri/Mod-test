using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Security.Claims;

namespace PimsApp.Pages
{
    public class HomeModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeModel> _logger;

        public HomeModel(IConfiguration configuration, ILogger<HomeModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Use claims instead of session for roles
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            if (!roles.Any(r => r == "Admin" || r == "NormalUser" || r == "BothRoles"))
            {
                return RedirectToPage("Login");
            }

            ViewData["IsAdmin"] = roles.Contains("Admin");
            ViewData["IsBoth"] = roles.Contains("BothRoles");
            ViewData["Email"] = User.FindFirstValue(ClaimTypes.Email);

            BindComplaints();
            DisplaySuccessMessage();

            return Page();
        }

        private void DisplaySuccessMessage()
        {
            // Use TempData instead of Session for success message
            if (TempData.TryGetValue("SuccessMessage", out object successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
                TempData.Remove("SuccessMessage");
            }
        }

        private void BindComplaints()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var email = User.FindFirstValue(ClaimTypes.Email);

            using var conn = new SqlConnection(connectionString);
            var query = roles.Contains("Admin") || roles.Contains("BothRoles")
                ? "SELECT * FROM Complaints ORDER BY Id DESC"
                : "SELECT * FROM Complaints WHERE Email = @Email ORDER BY Id DESC";

            using var cmd = new SqlCommand(query, conn);
            if (!roles.Contains("Admin") && !roles.Contains("BothRoles"))
            {
                cmd.Parameters.AddWithValue("@Email", email);
            }

            conn.Open();
            using var reader = cmd.ExecuteReader();
            var complaints = new List<ComplaintViewModel>();

            while (reader.Read())
            {
                complaints.Add(new ComplaintViewModel
                {
                    Id = reader["Id"].ToString(),
                    ComplaintId = reader["ComplaintId"].ToString(),
                    Name = $"{reader["FirstName"]} {reader["LastName"]}",
                    EmpId = reader["EmpId"].ToString(),
                    Email = reader["Email"].ToString(),
                    ContactNumber = reader["ContactNumber"].ToString(),
                    DateTimeCapture = Convert.ToDateTime(reader["DateTimeCapture"]),
                    PictureCaptureLocation = $"{reader["PictureCaptureLocation"]} {reader["StreetAddress1"]} {reader["City"]}, {reader["Zip"]} {reader["State"]}",
                    Comments = reader["Comments"].ToString(),
                    Status = reader["Status"].ToString(),
                    PictureUploads = reader["PictureUpload"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Path.GetFileName).ToArray(),
                    CurrentStatus = reader["CurrentStatus"].ToString(),
                });
            }

            ViewData["Complaints"] = complaints;
        }

        public IActionResult OnPostUpdateStatus(string complaintId, string status)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connectionString);
            var query = "UPDATE Complaints SET [Status] = @Status WHERE [ComplaintId] = @ComplaintId";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

            conn.Open();
            cmd.ExecuteNonQuery();

            if (status == "Not Started")
            {
                UpdateComplaintCurrentStatus(complaintId, "In Progress");
            }

            return RedirectToPage();
        }

        private void UpdateComplaintCurrentStatus(string complaintId, string status)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connectionString);
            var query = "UPDATE Complaints SET CurrentStatus = @CurrentStatus WHERE ComplaintId = @ComplaintId";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CurrentStatus", status);
            cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IActionResult OnPostLogout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}