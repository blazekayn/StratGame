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