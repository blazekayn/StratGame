using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for City
/// </summary>
public class City
{
    public int CityID { get; set; }
    public int Population { get; set; }
    public int Gold { get; set; }
    public int Lumber { get; set; }
    public int Stone { get; set; }
    public int Iron { get; set; }
    public string Name { get; set; }
    public Point Location { get; set; }
    public List<Building> Buildings { get; set; }

    public City()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Constructor of City. Create a city given all its attributes.
    /// </summary>
    /// <param name="CityID"></param>
    /// <param name="Population"></param>
    /// <param name="Name"></param>
    /// <param name="Gold"></param>
    /// <param name="Lumber"></param>
    /// <param name="Stone"></param>
    /// <param name="Iron"></param>
    /// <param name="Location"></param>
    public City(int CityID, int Population, string Name, int Gold, int Lumber, int Stone, int Iron, Point Location)
    {
        this.CityID = CityID;
        this.Population = Population;
        this.Name = Name;
        this.Gold = Gold;
        this.Lumber = Lumber;
        this.Stone = Stone;
        this.Iron = Iron;
        this.Location = Location;
        //Load all buildings for this city
        LoadBuildings();
    }

    /// <summary>
    /// Change the name of the city
    /// </summary>
    /// <param name="name">The name you wish to change the city to.</param>
    public void Rename(string name)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DB"].ToString()))
        {
            conn.Open();
            string sql = "UPDATE CITY SET Name=@Name WHERE CityID=@CityID";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }
    }

    /// <summary>
    /// Return a list of all cities owned by a player
    /// </summary>
    /// <param name="PlayerID">The ID of the player for which you want to get cities</param>
    /// <returns>A List(City) of cities</returns>
    public static List<City> GetAllCitiesByPlayerID(int PlayerID)
    {
        List<City> cities = new List<City>();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DB"].ToString()))
        {
            conn.Open();
            string sql = "SELECT CityID, Name, Gold, Lumber, Stone, Iron, Population, XCoord, YCoord FROM CITY WHERE PlayerID=@PlayerID";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PlayerID", PlayerID);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cities.Add(new City(dr.GetInt32(dr.GetOrdinal("CityID")), dr.GetInt32(dr.GetOrdinal("Population")), dr["Name"].ToString(), dr.GetInt32(dr.GetOrdinal("Gold")), dr.GetInt32(dr.GetOrdinal("Lumber")), dr.GetInt32(dr.GetOrdinal("Stone")), dr.GetInt32(dr.GetOrdinal("Iron")), new Point(dr.GetInt32(dr.GetOrdinal("XCoord")), dr.GetInt32(dr.GetOrdinal("YCoord")))));
            }
            dr.Close();
            conn.Close();
            conn.Dispose();
        }
        return cities;
    }

    private void LoadBuildings()
    {
        Buildings = Building.GetAllBuildingsByCityID(CityID);
    }
}