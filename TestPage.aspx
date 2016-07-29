<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="TestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="grdCity" runat="server" EmptyDataText="No Cities." AutoGenerateColumns="true" AutoGenerateSelectButton="true"
            OnSelectedIndexChanged="grdCity_SelectedIndexChanged" DataKeyNames="CityID" AutoGenerateEditButton="true" >

        </asp:GridView>
        <asp:GridView ID="grdBuilding" runat="server" AutoGenerateColumns="true" AutoGenerateSelectButton="true"
            OnSelectedIndexChanged="grdBuilding_SelectedIndexChanged" >

        </asp:GridView>
        <asp:DetailsView ID="dtlBuilding" runat="server" AutoGenerateRows="true">
            <Fields>
                <asp:TemplateField>

                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
    </div>
    </form>
</body>
</html>
