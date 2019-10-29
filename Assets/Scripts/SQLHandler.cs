using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public class outputMessage
{
    public Boolean success;
    public string ErrorMessage;
    public Dictionary<string, Dictionary<string, string>> lst;

    public outputMessage()
    {
        this.lst = new Dictionary<string, Dictionary<string, string>>();
    }
}

public class inputMessage
{
    public string way;
    public Dictionary<string, string> argument;

    public inputMessage()
    {
        this.argument = new Dictionary<string, string>();
    }
}
public class SQLHandler : MonoBehaviour
{
    public PlayerIndicator playerIndicator;
    /// <summary>
    /// 建立数据库连接
    /// </summary>
    public MySqlConnection GetSqlConn()
    {
        // 数据库
        MySqlConnection sqlConn;
        string connStr = "Database=test;Data Source=127.0.0.1;User Id=root;Password=0129;port=3306";
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

        try
        {
            Dictionary<string, string> result = getTsk() ;

            List<string> test = new List<string>(result.Keys);
            Debug.Log(test.Count);
            for (int i = 0; i < test.Count; i++)
            {
                Debug.Log( result[test[i] ] );

            }
        }
        catch (Exception ex)
        {
            Debug.Log("Error: Unable to get Dict");
            Debug.Log(ex.Message);
        }

    }

    /// <summary>
    ///  Initialize:
    ///  (X) Two databse: (do the check first and then set up the database structure)
    ///  user
    ///  task
    ///  (X) addUsr(String name, String pwd)
    ///  (X) searchUsr(String name, String pwd)
    ///  () addCoin(String name, int coin)
    ///  () addExp(Strig name, int exp)
    ///  () losCoin(String name, int coin)
    /// 
    ///  (X) getTsk()
    ///  () addTsk(String title, String content, int coin)
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
            String SetUsr = "CREATE TABLE usr ( name CHAR(32) PRIMARY KEY , pwd CHAR(32) NOT NULL , coin int DEFAULT 100, exp int DEFAULT 0 );";
            String SetTsk = "CREATE TABLE tsk ( id int AUTO_INCREMENT PRIMARY KEY, title CHAR(32) , content CHAR(255), coin int, exp int, owner CHAR(32) NOT NULL, taker CHAR(32) );";
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
            Debug.Log(ex.Message);
            return false;
        }

        try
        {
            String strUsr = "INSERT usr(name , pwd) VALUES (@name, @pwd);";
            MySqlCommand instUsr = new MySqlCommand(strUsr, sqlConn);
            instUsr.Parameters.AddWithValue("@name", name);
            instUsr.Parameters.AddWithValue("@pwd", pwd);
            instUsr.ExecuteNonQuery();
            sqlConn.Close();
        }
        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.Log("INSERT INTO Usr may can not work");
            Debug.Log(ex.Message);
            return false;
        }
        return true;
    }


    public Dictionary<string, string> searchUsr(String name, String pwd)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();


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
            if (resUsr.HasRows)
            {
                if (pwd.Equals(resUsr["pwd"]))
                {
                    result["coin"] = resUsr["coin"].ToString();
                    result["exp"] = resUsr["exp"].ToString();
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

            Console.Write("searchUsr Usr may can not work");
            Console.Write(ex.Message);

            result["Error"] = "have problem in finding Usr";
        }
        return result;
    }


    public Dictionary<string, string> getTsk()
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
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
            String strsql = "SELECT * FROM tsk;";
            MySqlCommand sqlComm = new MySqlCommand(strsql, sqlConn);
            MySqlDataReader sqlRes = sqlComm.ExecuteReader();
            
            while(sqlRes.Read())
            {
                int id = (int)sqlRes["id"]; 
                string content = (string)sqlRes["content"];
                string owner = (string)sqlRes["owner"];
                string row = content+"|"+owner;
                result.Add(id.ToString(), row);
            }            
        }
        catch (Exception ex)
        {
            sqlConn.Close();

            Console.Write("getTsk  may can not work");
            Console.Write(ex.Message);

            result["Error"] = "have problem in finding Tsk" + ex.ToString() ;
        }
        return result;
    }


    public Boolean addTsk(String title, String content, int coin, String owner)
    {
        MySqlConnection sqlConn = GetSqlConn();
        try
        {
            sqlConn.Open();

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return true; 
        }

        try
        {
            String strUsr = "INSERT tsk(title , content, coin, owner) VALUES (@title, @content, @coin, @owner);";
            MySqlCommand instUsr = new MySqlCommand(strUsr, sqlConn);
            instUsr.Parameters.AddWithValue("@title", title);
            instUsr.Parameters.AddWithValue("@content",content);
            instUsr.Parameters.AddWithValue("@coin", coin);
            instUsr.Parameters.AddWithValue("@owner",owner);
            instUsr.ExecuteNonQuery();
            sqlConn.Close();
        }
        catch (Exception ex)
        {
            sqlConn.Close();
            Debug.Log("INSERT INTO Tsk may can not work");
            Debug.Log(ex.ToString() );
            return false;
        }
        return true;
    }

    private void Start()
    {
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
        if (!playerIndicator.isPlayer)
        {
            //OpenSql();
        }
        //Debug.Log("Connection success!");
    }
}
