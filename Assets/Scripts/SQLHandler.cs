using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;



public class outputMessage
{
    public Boolean success;
    public string ErrorMessage;
    public  Dictionary<string, Dictionary<string, string> > lst;
}

public class inputMessage
{
    public string way;
    public Dictionary<string, string> argument; 
}



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
        return sqlConn;
    }

    /// <summary>
    /// Opens the sql.
    /// </summary>
    public void OpenSql()
    {
        jsontest(); 
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

    public void jsontest()
    {
        
    }

    /// <summary>
    ///  Initialize:
    ///  (X) Two databse: (do the check first and then set up the database structure)
    ///  user
    ///  task
    ///  (X) addUsr()
    ///  (X) searchUsr()
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



    /*
     * string addUsr(string msg)
     * 
     * input msg is actually a inputMessage(I will do the serialize part): 
     * way: "addUsr"
     * argument [it is a dictionary]:
     *  key(string) : value(string)
     *  "name"       : "shabi"
     *  "pwd"       : "wo si le"
     * 
     * 
     * output msg(string)
     * 
     * success: True (addSuccess) / False (add doesn't success)
     * ErrorMessage (if False) : will have the reason why it is false; 
     * lst (if success)  : lst["result"] = a dictionary: {"name":"shabi", "pwd":"zhe shi mi ma", "coin":"100", "exp": "0"}
     *     (False : null):
     */ 
    public string addUsr(string msg)
    {
        inputMessage input = JsonConvert.DeserializeObject<inputMessage>(msg);
        outputMessage output = new outputMessage();

        MySqlConnection sqlConn = GetSqlConn();
        try
        {
            sqlConn.Open();

        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            output.ErrorMessage = "addUsr: Connection between mysql doesn't work correctly";
            output.success = false;
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


    /*
     * string search(string msg)
     * 
     * input msg is actually a inputMessage(I will do the serialize part): 
     * way: "searchUsr"
     * argument [it is a dictionary]:
     *  key(string) : value(string)
     *  "usr"       : "shabi"
     *  "pwd"       : "wo si le"
     * 
     * 
     * output msg(string)
     * Defining success: there is a user 
     *          false  : (1)there is no such a usr (2) the pwd is not correct  
     * success: True (search Success) / False (search doesn't success)
     * ErrorMessage (if False) : will have the reason why it is false; 
     * lst (if success)  : lst["result"] = a dictionary: {"name":"shabi", "pwd":"zhe shi mi ma", "coin":"#", "exp": "#"}
     *     (False : null):
     */
    public string searchUsr(string msg)
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

    /*
     * string getallTsk()
     * 
     * input msg is actually a inputMessage(I will do deserialize part): 
     * way: "getallTsk"
     * 
     * 
     * 
     * output msg(string)
     *   
     * success: True (connect Success) / False (connect doesn't success)
     * ErrorMessage (if False) : will have the reason why it is false; 
     * lst (if success)  : lst["id"] = a dictionary: {"id":"#", "title":"X", "content":"XXX", "coin": "#" , "exp":"XX", "owner":"XX", "taker":"XX"}
     *     (False : null):
     */
    public string getallTsk()
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



    /*
     * string addTsk(string msg)
     * 
     * input msg is actually a inputMessage(I will do deserialize part): 
     * way: "addTsk"
     * argument [it is a dictionary]:
     *  key(string) : value(string)
     *  "title"       : "shabi"
     *  "content"     : "wo si le"
     *  "coin"        : "23"
     *  "owner"       : "woshiibaba"
     * 
     * output msg(string)
     * 
     * success: True (add Success) / False (add doesn't success)
     * ErrorMessage (if False) : will have the reason why it is false; 
     * lst (if success)  : lst["result"] = a dictionary: {"id":"#", "content":"XX", "content":"XX", "coin": "#" , "owner":"XXX"}
     *     (False : null):
     */
    public string addTsk(string msg)
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

    /*
     * string takeTsk(string msg)
     * 
     * input msg is actually a inputMessage(I will do deserialize part): 
     * way: "taekTsk"
     * argument [it is a dictionary]:
     *  key(string) : value(string)
     *  "id"        : "XXXX"
     *  "taker"     : "nibaba"
     * 
     * output msg(string)
     * 
     * success: True (take Success) / False (take doesn't success)
     * ErrorMessage (if False) : will have the reason why it is false; 
     * lst (if success)  : lst["result"] = a dictionary: {"id":"#", "content":"XX", "title":"XX", "coin": "#" , "owner":"XXX", "taker": "XXXXX"}
     *     (False : null):
     */
    public string takeTsk(string msg)
    {
        return new outputMessage();
    }

    private void Start()
    {
        OpenSql();
        //Debug.Log("Connection success!");
    }
}
