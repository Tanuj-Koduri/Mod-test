<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PimsApp.Login" %>

<!DOCTYPE html>
<html lang="en"> <!-- Added lang attribute for accessibility -->
<head runat="server">
    <meta charset="utf-8"> <!-- Added charset meta tag -->
    <meta name="viewport" content="width=device-width, initial-scale=1"> <!-- Added viewport meta tag for responsiveness -->
    <title>Home Page - EcoSight</title> <!-- Updated title to match the app name -->
    <!-- Updated to Bootstrap 5.3.3 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
    <!-- Updated to Font Awesome 6.4.0 -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet">

    <style>
        /* CSS styles remain largely the same, but consider moving to an external CSS file for better maintainability */
        /* ... (existing styles) ... */
    </style>
</head>
<body>
    <form id="form1" runat="server" class="needs-validation" novalidate> <!-- Added Bootstrap form validation -->
        <div class="container">
            <div class="menu-bar">
                <label>Welcome to EcoSight: Ecological Incident Reporting & Monitoring</label>
            </div>

            <div class="content">
                <h1 class="display-4">Citizen Repair: Report Public Issues Here</h1> <!-- Changed h3 to h1 for better SEO -->
                <div class="card-container">
                    <div class="form-icon"><i class="fas fa-user"></i></div>
                    <h2 class="title">Login</h2> <!-- Changed h3 to h2 for better structure -->

                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="txtUsername" class="form-label">Username</label> <!-- Added form-label class -->
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Placeholder="Username" required></asp:TextBox> <!-- Added required attribute -->
                            <div class="invalid-feedback">Please enter your username.</div> <!-- Added feedback for validation -->
                        </div>
                        <div class="form-group">
                            <label for="txtPassword" class="form-label">Password</label> <!-- Added form-label class -->
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Password" required></asp:TextBox> <!-- Added required attribute -->
                            <div class="invalid-feedback">Please enter your password.</div> <!-- Added feedback for validation -->
                        </div>
                        <asp:Button ID="btnLoginUser" runat="server" CssClass="btn btn-primary" Text="Login" OnClick="btnLoginUser_Click" /> <!-- Added btn-primary class -->
                        <div class="forgot-password mt-3"> <!-- Added mt-3 for margin -->
                            <a href="ForgotPassword.aspx">Forgot Password?</a> <!-- Changed to a separate page for password reset -->
                        </div>
                    </div>
                </div>
                <asp:Label ID="lblMessage" runat="server" CssClass="message alert alert-danger" Visible="false" role="alert"></asp:Label> <!-- Added Bootstrap alert and ARIA role -->
            </div>
        </div>
    </form>

    <!-- Added Bootstrap and custom JavaScript for form validation -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>
    <script>
        (function () {
            'use strict'
            var forms = document.querySelectorAll('.needs-validation')
            Array.prototype.slice.call(forms)
                .forEach(function (form) {
                    form.addEventListener('submit', function (event) {
                        if (!form.checkValidity()) {
                            event.preventDefault()
                            event.stopPropagation()
                        }
                        form.classList.add('was-validated')
                    }, false)
                })
        })()
    </script>
</body>
</html>