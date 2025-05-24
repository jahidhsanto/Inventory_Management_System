<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery_Test.aspx.cs" Inherits="STORE_FINAL.Test_02.MaterialDelivery_Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .badge {
            background-color: #007bff;
            color: white;
            padding: 3px 8px;
            border-radius: 5px;
        }

        .form-control[disabled] {
            background-color: #e9ecef;
        }
    </style>
    <style>
        .chips-container {
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
        }

        .chip-professional {
            background: #f8f9fa;
            border: 1px solid #ced4da;
            border-radius: 8px;
            padding: 8px 16px;
            min-height: 38px;
            font-size: 1rem;
            color: #343a40;
            box-shadow: 0 1px 2px rgba(0,0,0,0.03);
            transition: box-shadow 0.2s;
            position: relative;
        }

            .chip-professional:hover {
                box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            }

        .chip-label {
            flex: 1;
            font-weight: 500;
        }

        .chip-close {
            color: #dc3545;
            font-size: 1.25rem;
            line-height: 1;
            background: none;
            border: none;
            cursor: pointer;
            outline: none;
        }

            .chip-close:hover {
                color: #a71d2a;
                text-decoration: none;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h3 class="mb-4">Requisition Items</h3>

        <div class="mb-3">
            <label for="txtRequisitionID" class="form-label">Requisition ID</label>
            <asp:TextBox ID="txtRequisitionID" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-info mb-3" Text="Load by Requisition ID" OnClick="btnLoad_Click" />

        <asp:Panel ID="pnlStateView" runat="server">
            <asp:Repeater ID="rptMaterials" runat="server" OnItemDataBound="rptMaterials_ItemDataBound">
                <HeaderTemplate>
                    <table class="table table-bordered table-striped">
                        <thead class="table-dark">
                            <tr>
                                <th>Material Name</th>
                                <th>Quantity</th>
                                <th>Serial Number</th>
                                <th>Requires Serial</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfMaterialID" runat="server" Value='<%# Eval("Material_ID") %>' />
                            <%# Eval("Materials_Name") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="form-control" />
                        </td>
                        <td>
                            <!-- Dropdown for selecting serials -->
                            <asp:DropDownList ID="ddlSerialOptions" runat="server" CssClass="form-control select2">
                            </asp:DropDownList>

                            <!-- Container for chips -->
                            <div id='<%# "chipContainer_" + Eval("Material_ID") %>' class="chips-container mt-2"></div>

                            <!-- Hidden field to store selected serials -->
                            <asp:HiddenField ID="hfSelectedSerials" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblRequiresSerial" runat="server" Text='<%# Eval("Requires_Serial_Number") %>' CssClass="badge" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                        </table>
                </FooterTemplate>
            </asp:Repeater>
        </asp:Panel>

        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success mt-3" Text="Save" OnClick="btnSave_Click" />

        <asp:Label ID="lblStatus" runat="server" CssClass="mt-3 text-danger d-block" />
    </div>
    <script>
        function addSerial(materialId, dropdown) {
            const selectedValue = dropdown.value;
            const selectedText = dropdown.options[dropdown.selectedIndex].text;
            const chipContainer = document.getElementById("chipContainer_" + materialId);

            if (!selectedValue || document.getElementById(`chip_${materialId}_${selectedValue}`)) return;

            // Create chip
            const chip = document.createElement("div");
            chip.id = `chip_${materialId}_${selectedValue}`;
            chip.className = "chip-professional mb-2 d-flex align-items-center justify-content-between";
            chip.innerHTML = `
                <span class="chip-label">${selectedText}</span>
                <button type="button" class="chip-close btn btn-link p-0 ms-2" onclick="removeSerial('${materialId}', '${selectedValue}', '${selectedText}')" title="Remove">
                    <span aria-hidden="true">&times;</span>
                </button>
                <input type="hidden" name="serial_${materialId}" value="${selectedValue}">
            `;
            chipContainer.appendChild(chip);

            // Remove selected option from dropdown
            dropdown.remove(dropdown.selectedIndex);
            dropdown.selectedIndex = 0;
            updateHiddenField(materialId);
        }

        function removeSerial(materialId, serialNumber, serialText) {
            const chip = document.getElementById(`chip_${materialId}_${serialNumber}`);
            if (chip) chip.remove();

            // Find the correct dropdown for this materialId
            const dropdown = document.querySelector(`#chipContainer_${materialId}`).previousElementSibling;
            if (dropdown && dropdown.tagName === "SELECT") {
                // Add the option back to the dropdown
                const option = document.createElement("option");
                option.value = serialNumber;
                option.text = serialText;
                dropdown.appendChild(option);
            }

            updateHiddenField(materialId);
        }

        function updateHiddenField(materialId) {
            const hiddenField = document.querySelector(`#chipContainer_${materialId}`).nextElementSibling;
            const inputs = document.querySelectorAll(`#chipContainer_${materialId} input[type="hidden"]`);
            const serials = Array.from(inputs).map(input => input.value);
            hiddenField.value = serials.join(",");
        }
    </script>


</asp:Content>
