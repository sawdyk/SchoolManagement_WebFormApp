using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for myDB
/// </summary>
public class clsMyDB
{
    public clsMyDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string connString = ConfigurationManager.ConnectionStrings["PASSISConnectionString"].ConnectionString;
    private SqlConnection conn;

    public SqlConnection Conn
    {
        get { return conn; }
        set { conn = value; }
    }

    public void connct()
    {
        conn = new SqlConnection(connString);
        conn.Open();
    }

    public void closeConnct()
    {
        conn.Close();
    }

    public void excQuery(string query)
    {
        this.connct();
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.ExecuteNonQuery();
        this.closeConnct();
    }

    public void excQuery(SqlCommand cmd)
    {
        this.connct();
        cmd.ExecuteNonQuery();
        this.closeConnct();
    }

    public SqlDataReader fetch(string query)
    {
        this.connct();
        SqlCommand cmd = new SqlCommand(query, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public SqlDataReader fetch(SqlCommand cmd)
    {
        this.connct();
        //SqlCommand cmd = new SqlCommand(query, conn);
        SqlDataReader reader = cmd.ExecuteReader();
        return reader;
    }

    public bool exist(string table, string condition)
    {
        bool exist = false;

        //get user
        this.connct();
        string que = "SELECT * FROM " + table + " WHERE " + condition;
        SqlCommand cmd = new SqlCommand(que, conn);
       
        SqlDataReader dat = this.fetch(cmd);
      
        if (dat.HasRows) exist = true;
        this.closeConnct();
        return exist;
    }


}