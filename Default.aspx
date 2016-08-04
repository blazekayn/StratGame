<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphScriptInclude" runat="Server">
    <script type="text/javascript">
        //Start the chat and timer when the page is laoded.
        $(document).ready(function(){
            StartTickTimer();
            GetLastMessage();
        });

        var lastMessage; //The Message ID of teh Last message recieved
        var left; //Number of Seconds until server tick
        var min; //Number of minutes in countdown timer
        var sec; //Number of seconds in countdown timer
        var timerInterval;

        //Get the ID of the last message sent so it can start the chat
        function GetLastMessage() { //Called when the chat starts up
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
        //Sends a chat message to the server to be recorded in the database.
        //Need to do some parsing here to make sure it does not have script in it.
        //Actuall the checking probably needs on the server side.
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
        //Get all message from the server that have occured since the last message was recieved.
        //This is called every two seconds. And updates teh ChatTextDiv based on the results.
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
                                if(div.innerHTML.length > 10000){ //Code to trim the chat window down after it reaches 10000 characters
                                    div.innerHTML = div.innerHTML.substring(5000,div.innerHTML.length); //is experimental and might not work.
                                }
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
        //Keep the chat window scrolled to the bottom of the chat
        function updateScroll(){
            var element = document.getElementById("ChatTextDiv");
            element.scrollTop = element.scrollHeight;
        }

        //-------------------Tick Timer Functions--------------------//
        //Get the number of seconds until the next tick from the server and start the timer
        function StartTickTimer(){
                $.ajax({
                    async: true,
                    type: "POST",
                    contentType: "application/json; charset=uft-8",
                    url: "Default.aspx/GetSecTillNextTick",
                    dataType: "json",
                    success: function(data){
                        if (data.d != ""){
                            var json_obj = $.parseJSON(data.d);
                            left = json_obj;
                            min = Math.floor(left/60);
                            sec = left%60;
                            var div = document.getElementById('TickTimerDiv');
                            div.innerHTML = min + ":" + sec;
                            timerInterval = setInterval(function(){ TickTimer(); }, 1000);
                        }
                    }
                });
        }
        //Method to update the timer by one second. Every tick (2 min) calls the server to
        //get a more accurate system time using the StartTickTimer method.
        //This timer is purely to help the user know when a tick is and has nothing to do with
        //the actual server ticks
        function TickTimer(){
            if(left < 1){
                <%=ClientScript.GetPostBackEventReference(upMain, "")%>
                clearInterval(timerInterval);
                StartTickTimer();
            }else{
                left--;
                min = Math.floor(left/60);
                sec = left%60;
                var div = document.getElementById('TickTimerDiv');
                div.innerHTML = "Time until next tick: " + min + ":" + sec;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHeading" runat="Server">
    <div id="OutofDateWarning">

    </div>
    <div id="TickTimerDiv" style="font-size:large; color:green;">

    </div>
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
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" UseSubmitBehavior="true" />
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

