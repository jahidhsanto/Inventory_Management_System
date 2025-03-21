<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="STORE_FINAL.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container body-content">

<%--CONTENT START--%>
    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h3 id="aspnetTitle">Developer Information</h3>
            <div class="col-md-8">
                <div style="display: flex; align-items: center;">
                    <!-- Image on the left -->
                    <img src="\images\jahidhsanto.jpg" alt="Jahid Hassan Santo" style="width: 150px; margin-right: 20px; object-fit: cover;" />
                    
                    <!-- Contact Info on the right -->
                    <div>
                        <div><strong>Name:</strong> Jahid Hassan Santo</div>
                        <div><strong>Contact number:</strong> +880 1878 959101</div>
                        <div><strong>Support:</strong> <a href="mailto:jahidhsanto@gmail.com.com">jahidhsanto@gmail.com</a><br /></div>
                    </div>
                </div>
            </div>
        </section>

        <section class="row">
            <div class="row justify-content-center">
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="card card-primary card-outline mb-4">
                        <div class="card-header">
                            <div class="card-title">User Login</div>
                        </div>
                        <div class="card-body">
                            <div class="mb-3">
                                <label for="txtUserName" class="form-label">Email address</label>
                                <asp:TextBox ID="txtUserName" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtPassword" class="form-label">Password</label>
                                <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="card-footer">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                        </div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
        </section>
    </main>

<%--CONTENT START--%>

            <hr />
            <footer>
                <p>&copy; <%: DateTime.Now.Year %> - My ASP.NET Application</p>
            </footer>
        </div>
    </form>
    <asp:PlaceHolder runat="server">
        <%--<%: Scripts.Render("~/Scripts/bootstrap.js") %>--%>

        <!-- Bootstrap CSS (Add this in the <head> section of your HTML) -->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
        
        <!-- Bootstrap JavaScript (Add this just before the closing </body> tag) -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>


        <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>

    </asp:PlaceHolder>
</body>
</html>
