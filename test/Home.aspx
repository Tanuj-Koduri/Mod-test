<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="PimsApp.Home" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <title>Admin Page - Dashboard</title>
    <!-- Updated to Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3" crossorigin="anonymous">
    <style>
        /* CSS styles remain mostly the same, with minor adjustments */
        .image-container img {
            max-width: 250px;
            height: auto;
            margin-right: 10px;
            margin-bottom: 10px;
            border: 1px solid #ccc;
            padding: 5px;
            border-radius: 5px;
        }

        /* Other styles remain the same */
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
            <div class="container-fluid">
                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <asp:Label ID="lblWelcome" runat="server" Text="Welcome!" CssClass="navbar-text fw-bold" />
                    </li>
                    <li class="nav-item ms-3">
                        <asp:Button ID="btnLogout" runat="server" CssClass="btn btn-danger" Text="Logout" OnClick="btnLogout_Click" />
                    </li>
                </ul>
            </div>
        </nav>

        <div class="container mt-5">
            <div class="banner">
                EcoSight: Ecological Incident Reporting & Monitoring
            </div>
            <h5 id="pageTitle" runat="server" class="mb-3 text-center"></h5>
            <asp:Label ID="lblSucessMessage" runat="server" CssClass="alert alert-success" Visible="false"></asp:Label>
            
            <div class="row mb-4">
                <div class="col-12 text-end mb-4">
                    <asp:Button ID="btnRegisterComplaint" runat="server" CssClass="btn btn-primary" Text="Register Complaint" OnClick="btnRegisterComplaint_Click" />
                </div>
                
                <asp:GridView ID="gvComplaints" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" 
                              OnRowDataBound="gvComplaints_RowDataBound" OnRowCommand="gvComplaints_RowCommand">
                    <Columns>
                        <!-- GridView columns remain mostly the same -->
                        <asp:BoundField DataField="ComplaintId" HeaderText="Complaint Id" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="EmpId" HeaderText="Emp Id" ItemStyle-CssClass="text-nowrap" />
                        <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-CssClass="email-column" />
                        <asp:BoundField DataField="ContactNumber" HeaderText="Number" />
                        <asp:BoundField DataField="DateTimeCapture" HeaderText="Date/Time of Capture" DataFormatString="{0:dd-MM-yyyy HH:mm}" />
                        <asp:BoundField DataField="PictureCaptureLocation" HeaderText="Location" />
                        <asp:BoundField DataField="Comments" HeaderText="Description" />
                        
                        <asp:TemplateField HeaderText="Images/Pictures">
                            <ItemTemplate>
                                <asp:Literal ID="litImages" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Current Status">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlCurrentStatus" runat="server" CssClass="form-select" AutoPostBack="True" OnSelectedIndexChanged="ddlCurrentStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="Not Started" Value="Not Started" />
                                    <asp:ListItem Text="In Progress" Value="In Progress" />
                                    <asp:ListItem Text="Resolved" Value="Resolved" />
                                    <asp:ListItem Text="Re-opened" Value="Re-opened" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action Taken">
                            <ItemTemplate>
                                <asp:HiddenField ID="hfComplaintId" runat="server" Value='<%# Eval("ComplaintId") %>' />
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>' CssClass="d-block mb-2" />
                                <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control mb-2" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                <asp:Button ID="btnUpdateStatus" runat="server" Text="Update" CssClass="btn btn-primary mb-2" CommandName="UpdateStatus" CommandArgument="<%# Container.DataItemIndex %>" />
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-secondary mb-2" CommandName="Edit" OnClick="btnEditComplaint_Click" CommandArgument="<%# Container.DataItemIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>

    <!-- Updated to Bootstrap 5 and added defer attribute for performance -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p" crossorigin="anonymous" defer></script>
</body>
</html>