<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="PrintChallan.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.PrintChallan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .challan-header {
            display: flex;
            justify-content: space-between;
            margin-bottom: 20px;
            border-bottom: 2px solid #000;
            padding-bottom: 10px;
        }

        .org-info, .challan-info {
            width: 45%;
        }

        .signature-section {
            margin-top: 50px;
            display: flex;
            justify-content: space-between;
        }

        .signature-box {
            width: 30%;
            text-align: center;
        }

        .print-btn {
            margin-bottom: 20px;
        }

        .challan-table th, .challan-table td {
            border: 1px solid #000 !important;
        }

        .challan-table {
            margin-top: 20px;
        }
    </style>
    <style id="table_style">
        table {
            width: 100%;
            border-collapse: collapse;
        }

        table, th, td {
            border: 1px solid black;
            padding: 8px;
        }
    </style>

    <script>
        function PrintTable() {
            var printWindow = window.open('', '', 'height=600,width=800');
            printWindow.document.write('<html><head><title>Print Preview</title>');

            // Grab the embedded style blocks
            const allStyles = document.querySelectorAll('style');
            allStyles.forEach(style => {
                printWindow.document.write('<style>' + style.innerHTML + '</style>');
            });

            printWindow.document.write('</head><body>');

            var challanContent = document.getElementById("challanArea");
            if (challanContent) {
                printWindow.document.write(challanContent.innerHTML);
            } else {
                printWindow.document.write('<p>Nothing to print.</p>');
            }

            printWindow.document.write('</body></html>');
            printWindow.document.close();

            printWindow.onload = function () {
                printWindow.focus();
                printWindow.print();
                //  printWindow.close();
            };
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="container mt-4">
       <h2 class="text-center">🖨️ Print Challan</h2>

       <asp:Label ID="lblMessage" runat="server" CssClass="text-danger font-weight-bold" />

       <div class="form-inline my-3">
           <asp:Label ID="lblChallanInput" runat="server" Text="Enter Challan Number:" CssClass="mr-2" />
           <asp:TextBox ID="txtChallanNumber" runat="server" CssClass="form-control mr-2" />
           <asp:Button ID="btnFetchChallan" runat="server" CssClass="btn btn-primary" Text="Fetch & Preview" OnClick="btnFetchChallan_Click" />
       </div>

       <asp:Panel ID="pnlChallan" runat="server" Visible="false">
           <div id="challanArea">
               <!-- Header -->
               <div class="challan-header">
                   <div class="org-info">
                       <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/LoGo.jpg" Width="100px" /><br />
                       <strong>Organization Name</strong><br />
                       Org Code: ORG123<br />
                       123 Street, City, Country
                   </div>

                   <div class="challan-info text-right">
                       <strong>Challan No:</strong> <asp:Label ID="lblChallanNumber" runat="server" Text="" /><br />
                       <strong>Delivery Date:</strong> <asp:Label ID="lblDate" runat="server" Text="" /><br />
                       <strong>Created By:</strong> <asp:Label ID="lblDeliveredTo" runat="server" Text="" />
                   </div>
               </div>

               <!-- Table -->
               <asp:Repeater ID="rptChallanDetails" runat="server">
                   <HeaderTemplate>
                       <table class="table table-bordered challan-table">
                           <thead class="thead-dark">
                               <tr>
                                   <th style="width: 5%;">SL</th>
                                   <th>Description<br />(Material Name & Serial)</th>
                                   <th style="width: 15%;">Quantity</th>
                               </tr>
                           </thead>
                           <tbody>
                   </HeaderTemplate>
                   <ItemTemplate>
                       <tr>
                           <td><%# Container.ItemIndex + 1 %></td>
                           <td><strong><%# Eval("Materials_Name") %></strong><br />SN: <%# Eval("Serial_Number") %></td>
                           <td><%# Eval("Quantity") %></td>
                       </tr>
                   </ItemTemplate>
                   <FooterTemplate>
                           </tbody>
                       </table>
                   </FooterTemplate>
               </asp:Repeater>

               <!-- Signature -->
               <div class="signature-section">
                   <div class="signature-box">
                       ___________________________<br />
                       Received By
                   </div>
                   <div class="signature-box">
                       ___________________________<br />
                       Authorized Signatory
                   </div>
                   <div class="signature-box">
                       ___________________________<br />
                       Store Incharge
                   </div>
               </div>
           </div>

           <!-- Print Button -->
           <div class="text-center mt-4 print-btn">
               <asp:Button ID="btnPrint" runat="server"
                   CssClass="btn btn-success"
                   Text="🖨️ Print"
                   OnClientClick="PrintTable(); return false;" />
           </div>

       </asp:Panel>
   </div>
</asp:Content>
