using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public enum BuildingType
{
    Sawmill,
    Ironmine,
    Goldmine,
    Farm, 
    Stonequarry
}

/// <summary>
/// Summary description for Building
/// </summary>
public class Building
{
    public BuildingType Type { get; set; }
    public int Level { get; set; }
    public int BuildingID { get; set; }

    public Building()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Building(int BuildingID, int BuildingTypeID, int BuildingLevel)
    {
        Level = BuildingLevel;
        this.BuildingID = BuildingID;
        Type = (BuildingType)BuildingTypeID;
    }

    public static List<Building> GetAllBuildingsByCityID(int CityID)
    {
        List<Building> buildings = new List<Building>();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DB"].ToString()))
        {
            conn.Open();
            string sql = "SELECT BuildingID, BuildingTypeID, BuildingLevel FROM CITY_BUILDING WHERE CityID=@CityID";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                buildings.Add(new Building(dr.GetInt32(dr.GetOrdinal("BuildingID")), dr.GetInt32(dr.GetOrdinal("BuildingTypeID")), dr.GetInt32(dr.GetOrdinal("BuildingLevel"))));
            }
            dr.Close();
            conn.Close();
            conn.Dispose();
        }
        return buildings;
    }
}