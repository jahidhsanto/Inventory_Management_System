<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RequisitionForm.aspx.cs" Inherits="STORE_FINAL.Forms.RequisitionForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Material Requisition Form</h2>

    <label>Employee Name:</label>
    <asp:DropDownList ID="ddlEmployeeName" runat="server"></asp:DropDownList>
    <br /><br />

    <label>Select Material:</label>
    <asp:DropDownList ID="ddlMaterials" runat="server"></asp:DropDownList>
    <br /><br />


<%--    <select id="ddlMaterials" class="form-control" style="width: 100%"></select>
    <br /><br />--%>

    <label>Quantity:</label>
    <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
    <br /><br />

    <asp:Button ID="btnSubmitRequisition" runat="server" Text="Submit Request" OnClick="btnSubmitRequisition_Click" />
    <br /><br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

<%--    <script>
        $(document).ready(function () {
            $('#ddlMaterials').select2({
                placeholder: "Search Material...",
                allowClear: true,  // Adds a clear button
                minimumInputLength: 1, // Requires at least 1 character to start searching
                ajax: {
                    url: 'GetMaterials.ashx',
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return {
                            searchTerm: params.term
                        };
                    },
                    processResults: function (data) {
                        return {
                            results: $.map(data, function (item) {
                                return {
                                    id: item.MaterialID,
                                    text: item.Name
                                };
                            })
                        };
                    },
                    cache: true
                }
            });
        });
    </script>--%>

</asp:Content>
