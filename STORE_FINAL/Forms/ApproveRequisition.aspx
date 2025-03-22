<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApproveRequisition.aspx.cs" Inherits="STORE_FINAL.Forms.ApproveRequisition" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center">ApproveRequisition</h2>

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
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="Created_Date" HeaderText="Requested Date" DataFormatString="{0:yyyy-MM-dd}" />
                
                <%-- Approve Button --%>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-success"
                            CommandName="Approve" CommandArgument='<%# Eval("Requisition_ID") %>' />

                        <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger"
                            CommandName="Reject" CommandArgument='<%# Eval("Requisition_ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
