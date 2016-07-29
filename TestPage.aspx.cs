using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestPage : System.Web.UI.Page
{
    Player player = new Player(1);

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(player.Honor);
        Response.Write(Player.NewPlayer("Dylan").PlayerID);
        foreach(City c in player.Cities)
        {
            Response.Write(c.Name + "<br/>");
            foreach(Building b in c.Buildings)
            {
                Response.Write("&nbsp;&nbsp;" + b.Type.ToString() + "<br/>");
            }
            Response.Write("-------<br/>");
        }

        grdCity.DataSource = player.Cities;
        grdCity.DataBind();
    }

    protected void grdCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdCity.SelectedIndex != -1)
        {
            grdBuilding.DataSource = player.Cities[grdCity.SelectedIndex].Buildings;
            grdBuilding.DataBind();
        }
    }

    protected void grdBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (grdCity.SelectedIndex != -1 && grdBuilding.SelectedIndex != -1)
        {
            List<Building> tempList = new List<Building>();
            tempList.Add(player.Cities[grdCity.SelectedIndex].Buildings[grdBuilding.SelectedIndex]);
            dtlBuilding.DataSource = tempList;
            dtlBuilding.DataBind();
        }
    }
}