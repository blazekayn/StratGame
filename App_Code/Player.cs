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

    //-------Fields---------//
    public int PlayerID { get; set; }
    public string DisplayName { get; set; }
    public int Honor { get; set; }
    public int Experience { get; set; }
    public List<City> Cities { get; set; }
    //----End Fields--------//

    //------------Constructors-----------------//
    public Player()
    {
        //
        // TODO: Add constructor logic here
        //
        PlayerID = -1;
        DisplayName = "";
        Honor = -1;
        Experience = -1;
        Cities = new List<City>();
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
        //Load a list of the player's cities from the database into the Cities list.
        LoadCities();
    }

    /// <summary>
    /// Create a new player given all of its parameters. Used by NewPlayer method.
    /// </summary>
    /// <param name="PlayerID">The ID of the Player</param>
    /// <param name="DisplayName">The Display Name of the player</param>
    /// <param name="Honor">The Honor of the player</param>
    /// <param name="Experience">The Experience of the player</param>
    public Player(int PlayerID, string DisplayName, int Honor, int Experience)
    {
        this.PlayerID = PlayerID;
        this.DisplayName = DisplayName;
        this.Honor = Honor;
        this.Experience = Experience;
        //Load a list of the player's cities from the database into the Cities list.
        LoadCities();
    }
    //-------------------End Constructors----------------//

    //-------------------Class Methods-------------------//
    /// <summary>
    /// Add experience to the player. Return the amount of experience after the addition.
    /// </summary>
    /// <param name="amount">Amount of experience to add.</param>
    /// <returns>New amount of experience</returns>
    public int AddExperience(int amount)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DB"].ToString()))
        {
            conn.Open();
            string sql = "UPDATE PLAYER SET Experience=Experience+@ExpAdd  OUT INSERTED.Experience WHERE PlayerID=@PlayerID";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PlayerID", PlayerID);
            cmd.Parameters.AddWithValue("@ExpAdd", amount);
            return Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            conn.Dispose();
        }
        return -1;
    }

    private void LoadCities()
    {
        Cities = City.GetAllCitiesByPlayerID(PlayerID);
    }

    /// <summary>
    /// Add honor to the player. Return the amount of honor after the addition.
    /// </summary>
    /// <param name="amount">Amount of honor to add.</param>
    /// <returns>New amount of honor</returns>
    public int AddHonor(int amount)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DB"].ToString()))
        {
            conn.Open();
            string sql = "UPDATE PLAYER SET Honor=Honor+@ExpAdd  OUT INSERTED.Honor WHERE PlayerID=@PlayerID";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PlayerID", PlayerID);
            cmd.Parameters.AddWithValue("@ExpAdd", amount);
            return Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            conn.Dispose();
        }
        return -1;
    }


    //--------------End Class Methods-----------------//

    //------------------Static Methods-------------------//
    /// <summary>
    /// Create a new player in the database and return that player's object
    /// </summary>
    /// <param name="DisplayName">The display name the player wishes to use</param>
    /// <returns>The newly created player object</returns>
    public static Player NewPlayer(string DisplayName)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DB"].ToString()))
        {
            conn.Open();
            string sql = "INSERT INTO PLAYER(DisplayName, Honor, Experience) OUTPUT INSERTED.PlayerID Values(@DisplayName, @Honor, @Experience)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@DisplayName", DisplayName);
            cmd.Parameters.AddWithValue("@Honor", 0);
            cmd.Parameters.AddWithValue("@Experience", 0);
            return new Player(Convert.ToInt32(cmd.ExecuteScalar().ToString()), DisplayName, 0, 0);
            conn.Close();
            conn.Dispose();
        }
        return null;
    }
    //----------------End Static Methods-----------------//
}