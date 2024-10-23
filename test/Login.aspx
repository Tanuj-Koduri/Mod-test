<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PimsApp.Login" %>

<!DOCTYPE html>
<html lang="en"> <!-- Added lang attribute for accessibility -->
<head runat="server">
    <meta charset="utf-8"> <!-- Added charset meta tag -->
    <meta name="viewport" content="width=device-width, initial-scale=1"> <!-- Added viewport meta tag for responsive design -->
    <title>Login - EcoSight</title> <!-- Updated title for better SEO -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet"> <!-- Updated Font Awesome version -->

    <style>
        /* CSS styles remain largely unchanged, but consider moving to an external CSS file for better maintainability */
    </style>
</head>
<body>
    <form id="form1" runat="server" class="needs-validation" novalidate> <!-- Added Bootstrap form validation -->
        <div class="container">
            <div class="menu-bar">
                <h1 class="h4">Welcome to EcoSight: Ecological Incident Reporting & Monitoring</h1> <!-- Changed label to h1 for better semantics -->
            </div>

            <div class="content">
                <h2 class="display-4">Citizen Repair: Report Public Issues Here</h2> <!-- Changed h3 to h2 for proper heading hierarchy -->
                <div class="card-container">
                    <div class="form-icon"><i class="fas fa-user"></i></div>
                    <h3 class="title">Login</h3>

                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="txtUsername">Username</label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Placeholder="Username" required></asp:TextBox> <!-- Added required attribute -->
                            <div class="invalid-feedback">Please enter your username.</div> <!-- Added feedback for validation -->
                        </div>
                        <div class="form-group">
                            <label for="txtPassword">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Password" required></asp:TextBox> <!-- Added required attribute -->
                            <div class="invalid-feedback">Please enter your password.</div> <!-- Added feedback for validation -->
                        </div>
                        <asp:Button ID="btnLoginUser" runat="server" CssClass="btn" Text="Login" OnClick="btnLoginUser_Click" />
                        <div class="forgot-password">
                            <a href="ForgotPassword.aspx">Forgot Password?</a> <!-- Updated link to a separate page -->
                        </div>
                    </div>
                </div>
                <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>
            </div>
        </div>
    </form>

    <!-- Added JavaScript for form validation -->
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