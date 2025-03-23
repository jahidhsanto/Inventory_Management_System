<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockReview.aspx.cs" Inherits="STORE_FINAL.Pages.StockReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Stock Review</h2>

    <asp:Panel ID="PanelFilters" runat="server" CssClass="mb-3">
        <label for="ddlStatus">Filter by Status:</label>
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="false">
            <asp:ListItem Text="All" Value="All"></asp:ListItem>
            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
            <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary mt-2" OnClick="btnFilter_Click" />
    </asp:Panel>

    <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive" style="overflow-x: auto; white-space: nowrap;">
        <asp:GridView 
            ID="ApproveRequisitionGridView" 
            runat="server" 
            AutoGenerateColumns="False" 
            CssClass="table table-bordered table-striped"
            HorizontalAlign="Left"
            DataKeyNames="Requisition_ID"
            OnRowCommand="ApproveRequisitionGridView_RowCommand">
            <Columns>
                <asp:BoundField DataField="Requisition_ID" HeaderText="Requisition ID" ReadOnly="True" />
                <asp:BoundField DataField="Requested_By" HeaderText="Requested By" />
                <asp:BoundField DataField="Material_Name" HeaderText="Material Name" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="Stock_Quantity" HeaderText="Available Stock" />
                <asp:BoundField DataField="Created_Date" HeaderText="Requested Date" DataFormatString="{0:yyyy-MM-dd}" />
                
                <asp:TemplateField HeaderText="Actions">
<%--                <ItemTemplate>
                        <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-success"
                            CommandName="Approve" CommandArgument='<%# Eval("Requisition_ID") %>'
                            Visible='<%# Eval("Status").ToString() == "Pending" %>' />
                                                
                        <asp:Button ID="btnPending" runat="server" Text="Pending" CssClass="btn btn-warning"
                            CommandName="Pending" CommandArgument='<%# Eval("Requisition_ID") %>'
                            Visible='<%# Eval("Status").ToString() == "Approved" || Eval("Status").ToString() == "Rejected" %>' />

                        <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger"
                            CommandName="Reject" CommandArgument='<%# Eval("Requisition_ID") %>'
                            Visible='<%# Eval("Status").ToString() == "Pending" %>' />
                    </ItemTemplate>--%>



                    <ItemTemplate>
                        <!-- Delivered Button: Visible only if status is 'Ordered' -->
                        <asp:Button ID="btnDelivered" runat="server" Text="Delivered" CssClass="btn btn-success"
                            CommandName="Delivered" CommandArgument='<%# Eval("Requisition_ID") %>' 
                            Visible='<%# Eval("Store_Status").ToString() == "Ordered" %>' />

                        <!-- Ordered Button: Visible if status is 'Out of Stock' -->
                        <asp:Button ID="btnOrdered" runat="server" Text="Ordered" CssClass="btn btn-primary"
                            CommandName="Ordered" CommandArgument='<%# Eval("Requisition_ID") %>' 
                            Visible='<%# Eval("Store_Status").ToString() == "Out of Stock" %>' />

                        <!-- Out of Stock Button: Visible if status is 'Delivered' -->
                        <asp:Button ID="btnOutOfStock" runat="server" Text="Out of Stock" CssClass="btn btn-danger"
                            CommandName="Out_Of_Stock" CommandArgument='<%# Eval("Requisition_ID") %>' 
                            Visible='<%# Eval("Store_Status").ToString() == "Delivered" %>' />
                    </ItemTemplate>

                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
