<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewEmployee.aspx.cs" Inherits="STORE_FINAL.Forms.AddNewEmployee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add New Employee</h2>

    <label>Employee ID:</label>
    <asp:TextBox ID="txtEmployeeID" runat="server"></asp:TextBox>
    <br /><br />

    <label>Name:</label>
    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
    <br /><br />
    
    <label>Select Department:</label>
    <asp:DropDownList ID="ddlDepartment" runat="server"></asp:DropDownList>
    <br /><br />


  <%--  <label>Department:</label>
    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="select2">
        <asp:ListItem Text="Select Department" Value="" />
        <asp:ListItem Text="Admin" Value="Admin" />
        <asp:ListItem Text="HR&D" Value="HR&D" />
        <asp:ListItem Text="IT" Value="IT" />
        <asp:ListItem Text="Store" Value="Store" />
        <asp:ListItem Text="Planning & Strategy (P&S)" Value="Planning & Strategy (P&S)" />
        <asp:ListItem Text="Quality Assurance" Value="Quality Assurance" />
        <asp:ListItem Text="Marketing" Value="Marketing" />

        <asp:ListItem Text="Accounts, Billing" Value="Accounts, Billing" />
        <asp:ListItem Text="Accounts, Commercial" Value="Accounts, Commercial" />
        <asp:ListItem Text="Accounts, Recovery" Value="Accounts, Recovery" />
        <asp:ListItem Text="Accounts, Accounts" Value="Accounts, Accounts" />
        <asp:ListItem Text="Accounts, Finance" Value="Accounts, Finance" />

        <asp:ListItem Text="Supply Chain, Procurement" Value="Supply Chain, Procurement" />
        <asp:ListItem Text="Supply Chain, Store Management" Value="Supply Chain, Store Management" />
        <asp:ListItem Text="Supply Chain, NI (Airport)" Value="Supply Chain, NI (Airport)" />

        <asp:ListItem Text="New Installation, NI(Escalator), Airport Project" Value="New Installation, NI(Escalator), Airport Project" />
        <asp:ListItem Text="New Installation, NI(Lift), Airport Project" Value="New Installation, NI(Lift), Airport Project" />
        <asp:ListItem Text="New Installation, NI (Escalator)" Value="New Installation, NI (Escalator)" />
        <asp:ListItem Text="New Installation, NI (Lift)" Value="New Installation, NI (Lift)" />

        <asp:ListItem Text="Existing Installation, EI (Sales)" Value="Existing Installation, EI (Sales)" />
        <asp:ListItem Text="Existing Installation, EI (FE)" Value="Existing Installation, EI (FE)" />
        <asp:ListItem Text="Existing Installation, EI (Escalator Servicing)" Value="Existing Installation, EI (Escalator Servicing)" />
        <asp:ListItem Text="Existing Installation, EI (Lift Servicing)" Value="Existing Installation, EI (Lift Servicing)" />
        <asp:ListItem Text="Existing Installation, EI (Escalator)" Value="Existing Installation, EI (Escalator)" />
        <asp:ListItem Text="Existing Installation, EI (Lift)" Value="Existing Installation, EI (Lift)" />


    </asp:DropDownList>
    <br /><br />--%>


    <label>Designation:</label>
    <asp:TextBox ID="txtDesignation" runat="server"></asp:TextBox>
    <br /><br />

    <asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" OnClick="btnAddEmployee_Click" />
    <br /><br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

</asp:Content>
