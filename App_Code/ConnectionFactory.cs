using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ConnectionFactory
/// </summary>
public class ConnectionFactory
{
    public ConnectionFactory()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static SqlConnection GetDBConnection()
    {
        return new SqlConnection(ConfigurationManager.AppSettings["DB"].ToString());
    }
}