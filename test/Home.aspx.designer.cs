using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PimsApp
{
    public partial class Home
    {
        // Use 'public' instead of 'protected' for better encapsulation
        public HtmlForm Form { get; set; } // Renamed from 'form1' for clarity

        public Label WelcomeLabel { get; set; } // Renamed from 'lblWelcome' for clarity

        public Button LogoutButton { get; set; } // Renamed from 'btnLogout' for clarity

        public HtmlGenericControl PageTitle { get; set; } // Renamed from 'pageTitle' for consistency

        public Label SuccessMessage { get; set; } // Renamed from 'lblSucessMessage' and fixed typo

        public Button RegisterComplaintButton { get; set; } // Renamed from 'btnRegisterComplaint' for clarity

        public GridView ComplaintsGridView { get; set; } // Renamed from 'gvComplaints' for clarity
    }
}