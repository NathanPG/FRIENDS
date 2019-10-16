using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SQLHandler : MonoBehaviour
{
    /// <summary>
    /// 建立数据库连接
    /// </summary>
    public MySqlConnection GetSqlConn()
    {
        // 数据库
        MySqlConnection sqlConn;

        string connStr = "Database=test;Data Source=127.0.0.1;User Id=root;Password=0129;port=3306";

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

        
        try
        {
            Dictionary<string, string> result = searchUsr("shabi", "233");
            Debug.Log(result["coin"]);
            Debug.Log(result["exp"]);
        }
        catch (Exception ex)
        {
            Debug.Log("Error: Unable to add SHabi");
            Debug.Log(ex.Message);
        }


    }

/// <summary>
///  Initialize:
///  (X) Two databse: (do the check first and then set up the database structure)
///  user
///  task
///  (X) addUsr(String name, String pwd)
///  () searchUsr(String name, String pwd)
///  () addCoin(String name, int coin)
///  () addExp(Strig name, int exp)
///  () losCoin(String name, int coin)
/// 
///  () getTask()
///  () addTask(String title, String content, int coin)
///  () deleteTask(String title)
///  
/// </summary>
/// 

    public void setTable()
    {
        MySqlConnection sqlConn = GetSqlConn();
        try
        {
            sqlConn.Open();
            Debug.Log("suppose to set up the table");
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return;
        }

        try
        {

            Debug.Log("Start run");
            String SetUsr = " CREATE TABLE usr ( name CHAR(32) PRIMARY KEY , pwd CHAR(32) NOT NULL , coin int DEFAULT 100, exp int DEFAULT 0 );";
            String SetTsk = " CREATE TABLE tsk ( id int, content CHAR(32), coin int, exp int);";
            Debug.Log("unable to run");
            MySqlCommand setBaseUsr = new MySqlCommand(SetUsr);
            setBaseUsr.Connection = sqlConn;
            MySqlCommand setBaseTsk = new MySqlCommand(SetTsk);
            setBaseTsk.Connection = sqlConn;
            Debug.Log("hahah");
            setBaseUsr.ExecuteNonQuery();
            setBaseTsk.ExecuteNonQuery();
            Debug.Log("have run");
            sqlConn.Close();
            
        }
        catch (Exception ex)
        {
            sqlConn.Close();
            Console.Write("Create TABLE (maybe it has been created)");
            Console.Write(ex.ToString()); 
        }
        
    }

    public Boolean addUsr(String name, String pwd)
    {
        MySqlConnection sqlConn = GetSqlConn();
        try
        {
            sqlConn.Open();

        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return false;
        }

        try
        {
            String strUsr = "INSERT usr(name , pwd) VALUES (@name, @pwd);";
            MySqlCommand instUsr = new MySqlCommand(strUsr , sqlConn);
            instUsr.Parameters.AddWithValue("@name",name);
            instUsr.Parameters.AddWithValue("@pwd", pwd);
            instUsr.ExecuteNonQuery();
            sqlConn.Close();
        }
        catch (Exception ex)
        {
            sqlConn.Close();
            Console.Write("INSERT INTO Usr may can not work");
            Console.Write(ex.Message);
            return false; 
        }
       
        return true;
    }


    public Dictionary<string,string> searchUsr(String name, String pwd)
    {
        Dictionary<string, string> result =new Dictionary<string, string>();


        MySqlConnection sqlConn = GetSqlConn();
        try
        {
            sqlConn.Open();

        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            result["Error"] = "Unable to Connect to DB";
            return result; 
        }

        try
        {
            String strUsr = "SELECT * FROM usr where name=@name;";
            MySqlCommand findUsr = new MySqlCommand(strUsr, sqlConn);
            findUsr.Parameters.AddWithValue("@name", name);
            MySqlDataReader resUsr = findUsr.ExecuteReader();
            resUsr.Read(); 
            if( resUsr.HasRows )
            {
                if( pwd.Equals(resUsr["pwd"]) )
                {
                    result["coin"] = resUsr["coin"].ToString() ;
                    result["exp"] = resUsr["exp"].ToString() ;
                }
                else
                {
                    result["Error"] = "Wrong pwd";
                }

                return result; 
            }
            else
            {
                sqlConn.Close();
                result["Error"] = "Cannot find the shit";
                return result; 
            }
        }
        catch (Exception ex)
        {
            sqlConn.Close();

            result["Error"] = "have problem in finding Usr";
        }
        return result;
    }
    

    private void Start()
    {
        OpenSql();
        //Debug.Log("Connection success!");
    }
}
