using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PimsApp
{
    public partial class Home
    {
        // Use 'public' instead of 'protected' for better encapsulation
        // Use 'required' keyword to ensure non-null values
        public required HtmlForm Form { get; set; } // Renamed from 'form1' for clarity

        public required Label WelcomeLabel { get; set; } // Renamed from 'lblWelcome' for clarity

        public required Button LogoutButton { get; set; } // Renamed from 'btnLogout' for clarity

        public required HtmlGenericControl PageTitle { get; set; } // Renamed from 'pageTitle' for consistency

        public required Label SuccessMessage { get; set; } // Renamed from 'lblSucessMessage' and fixed typo

        public required Button RegisterComplaintButton { get; set; } // Renamed from 'btnRegisterComplaint' for clarity

        public required GridView ComplaintsGridView { get; set; } // Renamed from 'gvComplaints' for clarity

        // Remove unnecessary comments as the property names are now self-explanatory

        // Consider adding data annotations for validation and security
        // Example: [ValidateInput(false)] for user input fields

        // If using .NET Core or .NET 5+, consider replacing WebForms controls 
        // with more modern alternatives like Razor Pages or Blazor
    }
}