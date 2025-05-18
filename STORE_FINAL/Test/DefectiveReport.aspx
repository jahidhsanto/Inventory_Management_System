<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="DefectiveReport.aspx.cs" Inherits="STORE_FINAL.Test.DefectiveReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Defective Materials</h2>
        <asp:GridView ID="gvDefective" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Materials_Name" HeaderText="Material" />
                <asp:BoundField DataField="Defective_Count" HeaderText="Count" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
