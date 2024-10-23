// This file is auto-generated and should not be manually edited.
// Consider migrating to ASP.NET Core for new projects.

#nullable enable // Enable nullable reference types for improved null safety

namespace PimsApp
{
    public partial class Home
    {
        // Use modern C# property syntax
        protected global::System.Web.UI.HtmlControls.HtmlForm Form1 { get; private set; } = default!;

        protected global::System.Web.UI.WebControls.Label LblWelcome { get; private set; } = default!;

        protected global::System.Web.UI.WebControls.Button BtnLogout { get; private set; } = default!;

        protected global::System.Web.UI.HtmlControls.HtmlGenericControl PageTitle { get; private set; } = default!;

        protected global::System.Web.UI.WebControls.Label LblSuccessMessage { get; private set; } = default!;

        protected global::System.Web.UI.WebControls.Button BtnRegisterComplaint { get; private set; } = default!;

        protected global::System.Web.UI.WebControls.GridView GvComplaints { get; private set; } = default!;

        // Consider adding a method to initialize controls if needed
        private void InitializeControls()
        {
            // Example: Set default values or register event handlers
            BtnLogout.Click += (sender, e) => Logout();
            BtnRegisterComplaint.Click += (sender, e) => RegisterComplaint();
        }

        // Example methods (implement in code-behind file)
        private void Logout()
        {
            // Implement logout logic
        }

        private void RegisterComplaint()
        {
            // Implement complaint registration logic
        }
    }
}