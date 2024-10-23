using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PimsApp
{
    public partial class Home
    {
        // Modernized: Use of 'protected' instead of 'protected global' for better encapsulation
        // Modernized: Use of 'HtmlForm' instead of 'System.Web.UI.HtmlControls.HtmlForm' for cleaner code
        protected HtmlForm form1;

        // Modernized: Use of 'Label' instead of 'System.Web.UI.WebControls.Label'
        protected Label lblWelcome;

        // Modernized: Use of 'Button' instead of 'System.Web.UI.WebControls.Button'
        protected Button btnLogout;

        // Modernized: Use of 'HtmlGenericControl' instead of 'System.Web.UI.HtmlControls.HtmlGenericControl'
        protected HtmlGenericControl pageTitle;

        protected Label lblSucessMessage;

        protected Button btnRegisterComplaint;

        // Modernized: Use of 'GridView' instead of 'System.Web.UI.WebControls.GridView'
        protected GridView gvComplaints;

        // Modernized: Added a constructor to initialize fields
        public Home()
        {
            InitializeComponent();
        }

        // Modernized: Added a method to initialize components
        private void InitializeComponent()
        {
            // Initialize your controls here
        }

        // Modernized: Added a method for page load event
        protected void Page_Load(object sender, EventArgs e)
        {
            // Add your page load logic here
        }

        // Modernized: Added methods for button click events
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Add your logout logic here
        }

        protected void btnRegisterComplaint_Click(object sender, EventArgs e)
        {
            // Add your register complaint logic here
        }

        // Modernized: Added a method for GridView data binding
        private void BindComplaintsData()
        {
            // Add your data binding logic here
        }
    }
}