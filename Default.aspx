<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphScriptInclude" runat="Server">
    <script type="text/javascript">
        $(document).ready(function(){
            GetLastMessage();
        });
        var lastMessage; //The Message ID of teh Last message recieved
        function GetLastMessage() { //Called when the chat starts up
            //Get the ID of the last message sent so it can start the chat
            $.ajax({
                async: true,
                type: "POST",
                contentType: "application/json; charset=uft-8",
                url: "Default.aspx/GetLastMessage",
                dataType: "json",
                success: function(data){
                    if (data.d != ""){
                        var json_obj = $.parseJSON(data.d);
                        lastMessage = json_obj;
                        RefreshChat();
                    }
                }
            });
            //Start refreshing the chat every 2 sec
            window.setInterval(function(){ RefreshChat(); }, 2000);
        }
        function SendChatMessage() {
            var message = <%=txtChatMessage.ClientID%>.value;
            $.ajax({
                type: "POST",
                url: "Default.aspx/SendChatMessage",
                data: "{message: '" + message + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            });
            <%=txtChatMessage.ClientID%>.value = "";
            return false;
        }
        function RefreshChat(){
            if(lastMessage != null){
                $.ajax({
                    async: true,
                    type: "POST",
                    data: "{lastCheck: '" + lastMessage + "'}",
                    contentType: "application/json; charset=uft-8",
                    url: "Default.aspx/RefreshChat",
                    dataType: "json",
                    success: function(data){
                        if (data.d != ""){
                            var json_obj = JSON.parse(data.d);
                            for(var i = 0; i < json_obj.length; i++){
                                lastMessage = json_obj[i].messageID;
                                var div = document.getElementById('ChatTextDiv');
                                var playerid = <%:Player.GetPlayerIDFromSession()%>;
                                if (playerid == json_obj[i].PlayerID){
                                    div.innerHTML = div.innerHTML + "<font color=\"red\">" + json_obj[i].sender + "[" + json_obj[i].MessageTime + "]: </font>" + json_obj[i].message + "<br/>";
                                }else{
                                    div.innerHTML = div.innerHTML + json_obj[i].sender + "[" + json_obj[i].MessageTime + "]: " + json_obj[i].message + "<br/>";
                                }
                            }
                            updateScroll();
                        }
                    }
                });
            }
        }
        function updateScroll(){
            var element = document.getElementById("ChatTextDiv");
            element.scrollTop = element.scrollHeight;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeading" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSideBar" runat="Server">
    <div id="ChatContainerDiv" runat="server" style="width: 290px; padding: 0; margin-left: auto; margin-right: auto; border: 2px solid black; border-radius: 3px; overflow: hidden;">
        <!-- Chat box -->
        <div id="ChatTextDiv" style="width: 100%; height: 150px; background-color: white; overflow: scroll; margin-left: auto; margin-right: auto; overflow-x: hidden;">
        </div>
        <div id="ChatControlsDiv" style="margin-left: auto; margin-right: auto; width: 100%;">
            <asp:TextBox ID="txtChatMessage" runat="server" Text="" Width="225" />
            <asp:Button ID="btnSendChatMessage" runat="server" Text="Send" Width="50" OnClientClick="SendChatMessage(); return false;" />
        </div>

    </div>
    <!-- End Chat -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="smMain" runat="server" />
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <asp:GridView ID="grdCity" runat="server" EmptyDataText="No Cities." AutoGenerateColumns="true" AutoGenerateSelectButton="true"
                OnSelectedIndexChanged="grdCity_SelectedIndexChanged" DataKeyNames="CityID" AutoGenerateEditButton="true">
            </asp:GridView>
            <asp:GridView ID="grdBuilding" runat="server" AutoGenerateColumns="true" AutoGenerateSelectButton="true"
                OnSelectedIndexChanged="grdBuilding_SelectedIndexChanged">
            </asp:GridView>
            <asp:DetailsView ID="dtlBuilding" runat="server" AutoGenerateRows="true">
                <Fields>
                    <asp:TemplateField></asp:TemplateField>
                </Fields>
            </asp:DetailsView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

