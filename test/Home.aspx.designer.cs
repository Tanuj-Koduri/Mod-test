// Use latest C# features and nullable reference types
#nullable enable

using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PimsApp
{
    // Use 'file' keyword to make the class internal to this file
    public partial class Home
    {
        // Use readonly for fields that are set only in the constructor or at declaration
        // Use XML documentation comments for better IntelliSense support
        /// <summary>
        /// The main form of the page.
        /// </summary>
        protected readonly HtmlForm form1 = null!;

        /// <summary>
        /// Label for displaying welcome message.
        /// </summary>
        protected readonly Label lblWelcome = null!;

        /// <summary>
        /// Button for logging out.
        /// </summary>
        protected readonly Button btnLogout = null!;

        /// <summary>
        /// Generic control for page title.
        /// </summary>
        protected readonly HtmlGenericControl pageTitle = null!;

        /// <summary>
        /// Label for displaying success messages.
        /// </summary>
        protected readonly Label lblSucessMessage = null!;

        /// <summary>
        /// Button for registering a complaint.
        /// </summary>
        protected readonly Button btnRegisterComplaint = null!;

        /// <summary>
        /// GridView for displaying complaints.
        /// </summary>
        protected readonly GridView gvComplaints = null!;

        // Constructor to initialize fields (if needed)
        public Home()
        {
            // Initialize fields here if necessary
        }
    }
}