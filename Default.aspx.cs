using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PlayerID"] = 1;   
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false)]
    public static void SendChatMessage(string message)
    {
        ChatOperations.InsertChatMessage(message, (int)HttpContext.Current.Session["PlayerID"]);
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static string RefreshChat(int lastCheck)
    {
        List<ChatMessage> messages = ChatOperations.RefreshChat(lastCheck);
        return JsonConvert.SerializeObject(messages);
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static string GetLastMessage()
    {
        return JsonConvert.SerializeObject(ChatOperations.GetLastMessage());
    }
}