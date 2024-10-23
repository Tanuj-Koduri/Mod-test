// This file is auto-generated. Manual changes may be overwritten.

namespace PimsApp
{
    public partial class Home
    {
        // Use 'private' instead of 'protected' for better encapsulation
        // Use 'readonly' for fields that are set only in the constructor
        private readonly System.Web.UI.HtmlControls.HtmlForm form1;

        // Use nullable reference types for better null handling
        private System.Web.UI.WebControls.Label? lblWelcome { get; set; }

        // Use properties instead of fields for better encapsulation
        public System.Web.UI.WebControls.Button? BtnLogout { get; private set; }

        // Use camelCase for private fields
        private System.Web.UI.HtmlControls.HtmlGenericControl? pageTitle;

        // Use string interpolation for clearer string formatting
        public System.Web.UI.WebControls.Label? LblSuccessMessage { get; private set; }

        // Use modern event handling syntax
        public event EventHandler? RegisterComplaintClicked;
        private System.Web.UI.WebControls.Button? btnRegisterComplaint;

        // Use a more modern grid control if possible, e.g., Telerik RadGrid
        private System.Web.UI.WebControls.GridView? gvComplaints;

        // Add a constructor to initialize readonly fields
        public Home()
        {
            form1 = new System.Web.UI.HtmlControls.HtmlForm();
        }

        // Add methods to raise events
        protected virtual void OnRegisterComplaintClicked(EventArgs e)
        {
            RegisterComplaintClicked?.Invoke(this, e);
        }

        // Add a method to populate the grid
        public void PopulateComplaints(IEnumerable<Complaint> complaints)
        {
            if (gvComplaints != null)
            {
                gvComplaints.DataSource = complaints;
                gvComplaints.DataBind();
            }
        }
    }
}