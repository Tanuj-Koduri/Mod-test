using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PimsApp
{
    public partial class Home
    {
        // Use readonly modifier for better performance and thread safety
        protected readonly HtmlForm form1;

        // Use string interpolation for better readability
        protected Label lblWelcome => FindControl("lblWelcome") as Label;

        // Use Button instead of generic WebControl
        protected Button btnLogout => FindControl("btnLogout") as Button;

        // Use HtmlGenericControl instead of HtmlControls.HtmlGenericControl
        protected HtmlGenericControl pageTitle => FindControl("pageTitle") as HtmlGenericControl;

        // Corrected spelling of "Success"
        protected Label lblSuccessMessage => FindControl("lblSuccessMessage") as Label;

        protected Button btnRegisterComplaint => FindControl("btnRegisterComplaint") as Button;

        // Use GridView instead of generic WebControl
        protected GridView gvComplaints => FindControl("gvComplaints") as GridView;

        // Constructor to initialize readonly field
        public Home()
        {
            form1 = new HtmlForm();
        }
    }
}