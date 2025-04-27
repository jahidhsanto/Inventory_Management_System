<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="StockAvailability.aspx.cs" Inherits="STORE_FINAL.Role_Employee.StockAvailability" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-3 text-center">Stock Availability</h2>

        <!-- Search Bar -->
        <div class="mt-5">
            <label>🔍 Search Stock:</label>
            <input type="text" id="StockAvailability" class="form-control" placeholder="Type to search stock..." onkeyup="filterStockAvailability()">
        </div>

        <asp:GridView ID="StockAvailabilityGridView" runat="server" AutoGenerateColumns="false" 
            CssClass="table table-striped table-bordered mt-3" 
            DataKeyNames="Material_ID"
            OnRowCommand="gvMaterials_RowCommand"

            Style="table-layout: fixed; width: 100%;">
            <Columns>
                <asp:BoundField DataField="Material_ID" HeaderText="Material ID"
                    ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="Materials_Name" HeaderText="Material Name"
                    ItemStyle-Width="200px" HeaderStyle-Width="200px" />
                <asp:BoundField DataField="Part_Id" HeaderText="Part ID"
                    ItemStyle-CssClass="wrap-text" HeaderStyle-CssClass="wrap-text" />
                <asp:BoundField DataField="Stock_Quantity" HeaderText="Stock QTY"
                    ItemStyle-CssClass="wrap-text" HeaderStyle-CssClass="wrap-text" />
                <asp:BoundField DataField="UoM" HeaderText="UoM"
                    ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                <asp:BoundField DataField="Category" HeaderText="Category"
                    ItemStyle-CssClass="wrap-text" HeaderStyle-CssClass="wrap-text" />
                <asp:BoundField DataField="Unit_Price" HeaderText="Unit Price"
                    ItemStyle-CssClass="wrap-text" HeaderStyle-CssClass="wrap-text" DataFormatString="{0:C}" />
                <asp:BoundField DataField="model" HeaderText="Model"
                    ItemStyle-CssClass="wrap-text" HeaderStyle-CssClass="wrap-text" />
                <asp:BoundField DataField="Sub_Category" HeaderText="Sub Category"
                    ItemStyle-CssClass="wrap-text" HeaderStyle-CssClass="wrap-text" />
                <asp:BoundField DataField="Control" HeaderText="Control"
                    ItemStyle-CssClass="wrap-text" HeaderStyle-CssClass="wrap-text" />

                <asp:TemplateField HeaderText="Stock Status">
                    <ItemTemplate>
                        <span class='<%# Eval("Stock_Status").ToString().Trim() == "In Stock" ? "badge rounded-pill bg-success" : 
                                Eval("Stock_Status").ToString().Trim()  == "Low Stock" ? "badge rounded-pill bg-warning text-dark" : 
                                "badge rounded-pill bg-danger" %>'>
                            <%# Eval("Stock_Status") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnView" runat="server" CommandName="PlaceRequisition"
                            CommandArgument='<%# Eval("Material_ID") %>' Text="📝 Order"
                            CssClass="btn btn-info btn-sm" />
                    </ItemTemplate>
                    <ItemStyle Width="90px" HorizontalAlign="Center" />
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>

    <!-- JavaScript for Live Search -->
    <script>
        function filterStockAvailability() {
            var input, filter, table, tr, td, i, j, txtValue, found;
            input = document.getElementById("StockAvailability");
            filter = input.value.toUpperCase();
            table = document.getElementById("<%= StockAvailabilityGridView.ClientID %>");
            tr = table.getElementsByTagName("tr");

            for (i = 1; i < tr.length; i++) { // Start from 1 to skip the header row
                td = tr[i].getElementsByTagName("td");
                found = false; // Reset found flag for each row

                for (j = 0; j < td.length; j++) { // Loop through all columns
                    if (td[j]) {
                        txtValue = td[j].textContent || td[j].innerText;
                        if (txtValue.toUpperCase().indexOf(filter) > -1) {
                            found = true; // Match found in any column
                            break; // Stop checking further columns in this row
                        }
                    }
                }

                // Show or hide row based on search result
                tr[i].style.display = found ? "" : "none";
            }
        }
    </script>

</asp:Content>
