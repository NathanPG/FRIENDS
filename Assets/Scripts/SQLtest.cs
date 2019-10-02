using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using UnityEngine;

public class SQLtest : MonoBehaviour
{
    /// <summary>
    /// 建立数据库连接
    /// </summary>
    public MySqlConnection GetSqlConn()
    {
        // 数据库
        MySqlConnection sqlConn;
        string connStr = "Database=msgboard;Data Source=127.0.0.1;User Id=root;Password=root;port=3306";
        sqlConn = new MySqlConnection(connStr);
        return sqlConn;
    }

    /// <summary>
    /// Opens the sql.
    /// </summary>
    public void OpenSql()
    {
        // 数据库
        MySqlConnection sqlConn = GetSqlConn();
        try
        {
            sqlConn.Open();
            Debug.Log("NO ERROR!!!!!!!Connection success!");
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            Debug.Log("ERROR!!!!!!");
            return;
        }
    }
    private void Start()
    {
        OpenSql();
        //Debug.Log("Connection success!");
    }
}
