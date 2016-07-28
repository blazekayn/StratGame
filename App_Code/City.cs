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
    public int CityID;
    public int Population;
    public int Gold;
    public int Lumber;
    public int Stone;
    public int Iron;
    public string Name;
    public Point Location;

    public City()
    {
        //
        // TODO: Add constructor logic here
        //
    }

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
    }

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
}