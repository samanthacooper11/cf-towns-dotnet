using System;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    private string envVariable;
    private string connectionString;
    private MySqlConnection mysql_connection;
    private string mysqlHostname;
    private string mysqlPort;
    private string mysqlUsername;
    private string mysqlPassword;
    private string mysqldbname;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (getEnvVar())
        {
            initiateDB();
            bindData();
            Label1.Text = "";
            closeConnection();
            
        } else
        {
            Label1.Text = "Please bind to a MySql service";
        } 
    }

    private bool getEnvVar()
    {
        //get environment variable
        envVariable = Environment.GetEnvironmentVariable("VCAP_SERVICES");
        dynamic jsonobj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(envVariable);

        //check if MySql credentials are in the env var
        try
        {
            var mysqlCredentails = jsonobj["p-mysql"][0];
            mysqlHostname = mysqlCredentails.credentials.hostname.ToString();
            mysqlPort = mysqlCredentails.credentials.port.ToString();
            mysqlUsername = mysqlCredentails.credentials.username.ToString();
            mysqlPassword = mysqlCredentails.credentials.password.ToString();
            mysqldbname = mysqlCredentails.credentials.name.ToString();
            //create connection string
            connectionString = "server=" + mysqlHostname + ";uid=" + mysqlUsername + ";pwd=" + mysqlPassword + ";";
            return true; 
        }
        catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
        {
            //no Mysql credentials within env var
            return false;
        }
    }

    private void initiateDB()
    {
        try
        {
            mysql_connection = new MySqlConnection(connectionString);
            mysql_connection.Open();
            //use the database created and then execute sql scripts to create table and populate with some sample data
            string sqlStatement = "USE " + mysqldbname + ";";
            MySqlCommand cmd = new MySqlCommand(sqlStatement, mysql_connection);
            cmd.ExecuteNonQuery();
            MySqlScript script1 = new MySqlScript(mysql_connection, File.ReadAllText("sql-scripts/create-schema.sql"));
            script1.Delimiter = "$$";
            script1.Execute();
            MySqlScript script2 = new MySqlScript(mysql_connection, File.ReadAllText("sql-scripts/insert-towns.sql"));
            script2.Delimiter = "$$";
            script2.Execute();
        }
        catch (MySqlException ex)
        {
        }
    }

    private void bindData()
    {
        using (mysql_connection)
        {
            //select all results and bind to GridView
            MySqlDataAdapter adaptor = new MySqlDataAdapter("SELECT * from " + mysqldbname + ".uktowns", mysql_connection);
            DataTable dt = new DataTable();
            adaptor.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }     
        }
    }

    private void closeConnection()
    {
        try
        {
            mysql_connection.Close();
        }
        catch (Exception)
        {

        }
    }

    protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        bindData();
    }
}