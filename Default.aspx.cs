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
        if (!IsPostBack) { Session["Player"] = new Player(1); } //For testing remove once login page is live
        //Bind the City Gridview
        grdCity.DataSource = ((Player)Session["Player"]).Cities;
        grdCity.DataBind();
        
    }

    //-------------------CHAT METHODS----------------------------//
    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false)]
    public static void SendChatMessage(string message)
    {
        ChatOperations.InsertChatMessage(message, ((Player)HttpContext.Current.Session["Player"]).PlayerID);
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
    //------------------END CHAT METHODS--------------------------//
    //-----------------TICK COUNTDOWN TIMER METHODS---------------//
    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static int GetSecTillNextTick()
    {
        //Calculate the number of seconds until the next even minute in server time
        int minutes = DateTime.Now.Minute;
        int seconds = DateTime.Now.Second;
        int adjust = 1 - (minutes % 2);
        int nextMinutes = adjust;
        adjust = adjust * 60;
        int nextSeconds = 60 - (seconds % 60);
        adjust += nextSeconds;;
        return adjust;
    }
    //--------------END TICK COUNTDOWN TIMER METHODS--------------//

    protected void grdCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdCity.SelectedIndex != -1)
        {
            grdBuilding.DataSource = ((Player)HttpContext.Current.Session["Player"]).Cities[grdCity.SelectedIndex].Buildings;
            grdBuilding.DataBind();
        }
    }

    protected void grdBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdCity.SelectedIndex != -1 && grdBuilding.SelectedIndex != -1)
        {
            List<Building> tempList = new List<Building>();
            tempList.Add(((Player)HttpContext.Current.Session["Player"]).Cities[grdCity.SelectedIndex].Buildings[grdBuilding.SelectedIndex]);
            dtlBuilding.DataSource = tempList;
            dtlBuilding.DataBind();
        }
    }
}