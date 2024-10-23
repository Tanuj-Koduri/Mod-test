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

        // Remove individual XML comments for each control
        // Instead, add a general comment explaining the purpose of this class
        /// <summary>
        /// Represents the Home page of the PimsApp.
        /// This class contains properties for UI controls used in the Home.aspx page.
        /// </summary>
        /// <remarks>
        /// This code is auto-generated. Changes to this file may be overwritten
        /// when the code is regenerated. To modify, move field declarations
        /// from the designer file to the code-behind file.
        /// </remarks>
    }
}