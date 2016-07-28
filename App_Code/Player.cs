using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Player
/// </summary>
public class Player
{
    public int PlayerID;
    public string DisplayName;
    public int Honor;
    public int Experience;

    public Player()
    {
        //
        // TODO: Add constructor logic here
        //

    }

    /// <summary>
    /// Will load a player from the database using its PlayerID. hello World
    /// </summary>
    /// <param name="PlayerID">The ID of the player to load from the database</param>
    public Player(int PlayerID)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DB"].ToString()))
        {
            conn.Open();
            string sql = "SELECT DisplayName, Honor, Experience FROM PLAYER WHERE PlayerID=@PlayerID";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PlayerID", PlayerID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                DisplayName = dr["DisplayName"].ToString();
                Honor = dr.GetInt32(dr.GetOrdinal("Honor"));
                Experience = dr.GetInt32(dr.GetOrdinal("Experience"));
                this.PlayerID = PlayerID;
            }
            dr.Close();
            conn.Close();
            conn.Dispose();
        }
    }
    
}