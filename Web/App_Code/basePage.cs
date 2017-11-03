using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using MySql.Data.MySqlClient;
using System.Text;  
public partial class basePage : System.Web.UI.Page 
{
    protected string Language 
    {
        get
        {
            if (this.ViewState["Language "] != null)
            {
                return ViewState["Language "].ToString();
            }
            else
            {
                return "objc";//"";
            }
        }
        set
        {
            ViewState["Language"] = value;
        }

    }    
    protected string Conn
    {
        get
        {
            if (this.ViewState["conn"] != null)
            {
                return ViewState["conn"].ToString();
            }
            else
            {
                return "";//"";
            }
        }
        set
        {
            ViewState["conn"] = value;
        }

    }

    protected string NewLine = "\r\n";
 
    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected DataTable getTableSchema(string tableName)
    {
        MySqlConnection conn = new MySqlConnection(Conn);

        conn.Open();
        MySqlCommand cmd = conn.CreateCommand();

        cmd.CommandText = "SELECT * FROM " + tableName;

        MySqlDataAdapter da = new MySqlDataAdapter();
        da.SelectCommand = cmd;

        DataTable dt = new DataTable();
        da.FillSchema(dt, SchemaType.Source);

        conn.Close();

        return dt; 
    }

    protected IList<string> getAllTableName() {
        IList<string> tableNames = new List<string>();

        MySqlConnection conn = new MySqlConnection(Conn);
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand();
        
        cmd.CommandText = "show TABLE STATUS from " + conn.Database + " where Engine='InnoDB';";
        //conn.Database
        MySqlDataReader dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            tableNames.Add(dr["Name"].ToString());
        }

        return tableNames;
    }

    protected string ConvertType(string strType)
    {
        string str = "";
        switch (Language)
        {
            case "objc":
                switch (strType)
                {
                    case "Int32":
                    case "Int64":
                    case "Integer":
                        str = "int";
                        break;
                    case "String":
                        str = "NSString";
                        break;
                    case "Boolean":
                        str = "BOOL";
                        break;
                    case "Decimal":
                        str = "double";//暫時
                        break;
                    case "Single":
                        str = "double";//暫時
                        break;
                    case "Double":
                        str = "double";
                        break;
                    case "Byte[]":
                        str = "NSData";
                        break;
                    case "DateTime":
                        str = "NSDate";
                        break;
                    default:
                        str = strType;
                        break;
                }
                break;
        }
        return str;
    }

    protected string getObjectName(string strTableClass)
    {
        string str = "";
        Array ary1 = strTableClass.Split('_');
        
        if (ary1.Length > 1)
        {
            int i = 0;
            int j = 0;
            for (i = 0; i <= ary1.Length - 1; i++)
            {
                string ary2 = ary1.GetValue(i).ToString();
                if (ary2.Length > 0)
                {
                    for (j = 0; j <= ary2.Length - 1; j++)
                    {
                        if (j == 0)
                        {
                            str += ary2[j].ToString().ToUpper();
                        }
                        else
                        {
                            str += ary2[j].ToString().ToLower();
                        }
                    }
                }
            }
        }
        else
        {
            str = string.Format("{0}{1}", strTableClass[0].ToString().ToUpper(), strTableClass.Substring(1));
        }

        return str;
    }

    protected string getInfoClass(string tabName) {
        return string.Format("{0}Info", getObjectName(tabName));
    }

    protected string getDBClass(string tabName)
    {
        return string.Format("{0}DB", getObjectName(tabName));
    }

    protected string getDeclare(DataColumn dc){
        string typeName = ConvertType(dc.DataType.Name);

        if (typeName.StartsWith("NS"))
        {
            return string.Format("    {0} *{1};", typeName, getObjectName(dc.ColumnName));
        }
        else 
        {
            return string.Format("    {0} {1};", typeName, getObjectName(dc.ColumnName));
        }
    }

    protected string getPkParams(DataColumn[] dcs)
    {
        StringBuilder strs = new StringBuilder();
        for (int i = 0; i < dcs.Length; i++) {
            if (i > 0)
            {
                strs.AppendFormat(" {0}", getObjectName(dcs[i].ColumnName));
            }
            strs.Append(":");

            string typeName = ConvertType(dcs[i].DataType.Name);
            if (typeName.StartsWith("NS"))
            {
                strs.AppendFormat("({0} *){1}", typeName, getObjectName(dcs[i].ColumnName));
            }
            else
            {
                strs.AppendFormat("({0}){1}", typeName, getObjectName(dcs[i].ColumnName));
            }
        }

        return strs.ToString();
    }

    protected string getPkSqlWhere(DataColumn[] dcs)
    {
        StringBuilder strs = new StringBuilder();
        for (int i = 0; i < dcs.Length; i++)
        {
            if (i > 0)
            {
                strs.Append("and ");
            }
            strs.AppendFormat("{0} = ? ", dcs[i].ColumnName);
        }

        return strs.ToString();
    }

    //protected string getInsertColsString(DataTable dt) { 
    

    //}

}
