<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="PrintChallan.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.PrintChallan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        @media print {
            body {
                font-family: Arial, sans-serif;
            }
            .challan-container {
                border: 1px solid #000;
                padding: 20px;
                max-width: 800px;
                margin: auto;
                font-size: 14px;
            }
            .challan-header {
                text-align: center;
                font-size: 20px;
                margin-bottom: 20px;
            }
            .challan-footer {
                margin-top: 20px;
                text-align: center;
                font-size: 12px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="challan-container">
        <div class="challan-header">
            <h3>Material Delivery Challan</h3>
            <p><strong>Challan Number:</strong> <span id="challanNumber"></span></p>
            <p><strong>Delivery Date:</strong> <span id="deliveryDate"></span></p>
        </div>

        <h4>Delivery Details:</h4>
        <table border="1" cellpadding="5" cellspacing="0" style="width: 100%; margin-bottom: 20px;">
            <thead>
                <tr>
                    <th>S.No</th>
                    <th>Material Name</th>
                    <th>Serial Number</th>
                    <th>Quantity</th>
                </tr>
            </thead>
            <tbody id="challanItems">
                <!-- Delivery items will be inserted here dynamically -->
            </tbody>
        </table>

        <div class="challan-footer">
            <p><strong>Store Incharge: </strong> ____________________</p>
        </div>

        <div class="text-center">
            <button onclick="window.print();" class="btn btn-success">Print Challan</button>
        </div>
    </div>
</asp:Content>
