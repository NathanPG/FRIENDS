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
        string connStr = "Database=test;Data Source=127.0.0.1;User Id=root;Password=212810;port=4323";
        sqlConn = new MySqlConnection(connStr);

        Console.Write(sqlConn.DataSource);
        Debug.Log(sqlConn.DataSource);


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

            //string myInsertQuery = "CREATE TABLE test02 (name CHAR(32), pwd CHAR(32) );";
            string myInsertQuery = "INSERT INTO test02 VALUES ('nihao','woshishabi')";
            MySqlCommand myCommand = new MySqlCommand(myInsertQuery);
            myCommand.Connection = sqlConn;
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();


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
