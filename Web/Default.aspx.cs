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
using System.IO;
using System.Text;
using System.Collections;

public partial class _Default : basePage 
{
    protected string TabName
    {
        get
        {
            if (this.ViewState["TabName"] != null)
            {
                return ViewState["TabName"].ToString();
            }
            else
            {
                return "";//"";
            }
        }
        set
        {
            ViewState["TabName"] = value;
        }
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{


    //}
    //Database='neweggchat_db';Data Source='10.16.179.81';User Id='root';Password='root@neweggsso_admin';charset='utf8';pooling=true
    //database='neweggchat_db';server='10.16.179.81';uid='root';pwd='root@neweggsso_admin';charset='utf8';pooling=true
    string objName = "";
    string cmdName = "";
    protected void Button1_Click(object sender, EventArgs e)
    {
        objName = txtObjectName.Text;
        cmdName = txtCmd.Text;
        DataTable dbTab = this.getTableSchema(TabName);
        SetColObj();
        StringBuilder dbM = new StringBuilder();
        StringBuilder dbM2 = new StringBuilder();
        StringBuilder dbM3 = new StringBuilder();
        for (int j = 0; j < dbTab.Columns.Count; j++)
        {
            if (!dbTab.Columns[j].AutoIncrement)
            {
                if (dbM2.ToString() != "")
                {
                    dbM2.Append(", ");
                }
                dbM2.Append(dbTab.Columns[j].ColumnName);

                if (dbM3.ToString() != "")
                {
                    dbM3.Append(", ");
                }
                dbM3.Append("@"); dbM3.Append(dbTab.Columns[j].ColumnName);
            }
        }

        dbM.AppendFormat("    StringBuilder sqlStatement = new StringBuilder();"); dbM.Append(NewLine);
        dbM.AppendFormat("    sqlStatement.Append(\"INSERT INTO {0} \");", TabName); dbM.Append(NewLine);
        dbM.AppendFormat("    sqlStatement.Append(\"({0}) \");", dbM2.ToString()); dbM.Append(NewLine);
        dbM.AppendFormat("    sqlStatement.Append(\"VALUES({0})\");", dbM3.ToString()); dbM.Append(NewLine);

        dbM.AppendFormat("    MySqlCommand {0} = new MySqlCommand();", cmdName); dbM.Append(NewLine);
        dbM.AppendFormat("    {0}.CommandText = sqlStatement.ToString();", cmdName); dbM.Append(NewLine);

        //dbM.Append("    MySqlParameter[] parameters = "); dbM.Append(NewLine);
        //dbM.Append("            {"); dbM.Append(NewLine);

        for (int j = 0; j < dbTab.Columns.Count; j++)
        {
            if (!dbTab.Columns[j].AutoIncrement)
            {
                //if (j == dbTab.Columns.Count - 1)
                //{

                //    dbM.AppendFormat("        {0}", getBindDbCol(dbTab.Columns[j])); dbM.Append(NewLine);
                //}
                //else
                //{
                //    dbM.AppendFormat("        {0},", getBindDbCol(dbTab.Columns[j])); dbM.Append(NewLine);
                //}
                dbM.AppendFormat("        {0}", getBindDbCol(dbTab.Columns[j])); dbM.Append(NewLine);
            }
        }
        //dbM.Append("            };"); dbM.Append(NewLine);
        //dbM.Append("    cmd.Parameters.AddRange(parameters);"); dbM.Append(NewLine);
        txtCode.Text = dbM.ToString();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        objName = txtObjectName.Text;
        cmdName = txtCmd.Text;
        DataTable dbTab = this.getTableSchema(TabName);
        SetColObj();
        StringBuilder dbM = new StringBuilder();

        StringBuilder dbM2 = new StringBuilder();
        for (int j = 0; j < dbTab.Columns.Count; j++)
        {
            bool ispk = false;
            foreach (DataColumn dc in dbTab.PrimaryKey)
            {
                if (dc.ColumnName == dbTab.Columns[j].ColumnName)
                {
                    ispk = true;
                }
            }
            if (!ispk)
            {
                if (dbM2.ToString() != "")
                {
                    dbM2.Append(", ");
                }
                dbM2.AppendFormat("{0} = @{0}", dbTab.Columns[j].ColumnName);
            }
        }

        dbM.AppendFormat("        StringBuilder sqlStatement = new StringBuilder();"); dbM.Append(NewLine);
        dbM.AppendFormat("        sqlStatement.Append(\"UPDATE {0} SET {1} WHERE {2}\");", TabName, dbM2.ToString(), getPkSqlWhere(dbTab.PrimaryKey)); dbM.Append(NewLine);

        dbM.AppendFormat("    MySqlCommand {0} = new MySqlCommand();", cmdName); dbM.Append(NewLine);
        dbM.AppendFormat("    {0}.CommandText = sqlStatement.ToString();", cmdName); dbM.Append(NewLine);

        //dbM.Append("    MySqlParameter[] parameters = "); dbM.Append(NewLine);
        //dbM.Append("            {"); dbM.Append(NewLine);

        for (int j = 0; j < dbTab.Columns.Count; j++)
        {
            bool ispk = false;
            foreach (DataColumn dc in dbTab.PrimaryKey)
            {
                if (dc.ColumnName == dbTab.Columns[j].ColumnName)
                {
                    ispk = true;
                }
            }
            if (!ispk)
            {
                //dbM.AppendFormat("        {0},", getBindDbCol(dbTab.Columns[j])); dbM.Append(NewLine);
                dbM.AppendFormat("        {0}", getBindDbCol(dbTab.Columns[j])); dbM.Append(NewLine);
            }
        }

        for (int j = 0; j < dbTab.PrimaryKey.Length; j++)
        {
            //if (j == dbTab.PrimaryKey.Length - 1) {
            //    dbM.AppendFormat("        {0}", getBindDbCol(dbTab.PrimaryKey[j])); dbM.Append(NewLine);
            //}
            //else{
            //    dbM.AppendFormat("        {0}, ", getBindDbCol(dbTab.PrimaryKey[j])); dbM.Append(NewLine);
            //}
            dbM.AppendFormat("        {0}", getBindDbCol(dbTab.PrimaryKey[j])); dbM.Append(NewLine);
        }

        //dbM.Append("            };"); dbM.Append(NewLine);
        //dbM.Append("    cmd.Parameters.AddRange(parameters);"); dbM.Append(NewLine);
        txtCode.Text = dbM.ToString();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        ViewInfo();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.DataRow){
            Literal Literal1 = e.Row.FindControl("Literal1") as Literal;
            string tname = e.Row.DataItem as string;
            Literal1.Text = this.getObjectName(tname);
            HtmlInputHidden Hidden1 = e.Row.FindControl("Hidden1") as HtmlInputHidden;
            Hidden1.Value = tname;
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "read") {

            HtmlInputHidden Hidden1 =GridView1.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("Hidden1") as HtmlInputHidden;

            TabName = Hidden1.Value;

            SetTableCode();
        }
    }
    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow) {
            Literal Literal1 = e.Row.FindControl("Literal1") as Literal;
            DataColumn dc = e.Row.DataItem as DataColumn;
            Literal1.Text = dc.DataType.Name;

            Literal Literal2 = e.Row.FindControl("Literal2") as Literal;
            Literal2.Text = this.getObjectName(dc.ColumnName);

            TextBox TextBox1 = e.Row.FindControl("TextBox1") as TextBox;
            TextBox1.Text = this.getObjectName(dc.ColumnName);

            HtmlInputHidden Hidden1 = e.Row.FindControl("Hidden1") as HtmlInputHidden;
            Hidden1.Value = dc.ColumnName;
        }
    }

    private void ViewInfo() {
        Conn = connTextBox.Text;

        IList<string> tabNames = this.getAllTableName();

        GridView1.DataSource = tabNames;
        GridView1.DataBind();
    }
    
    private void SetTableCode() {
        DataTable dt = this.getTableSchema(TabName);

        GridView2.DataSource = dt.Columns;
        GridView2.DataBind();

        GridView3.DataSource = dt.PrimaryKey;
        GridView3.DataBind();
    }

    Hashtable hstab = new Hashtable();
    private void SetColObj() {
        TextBox txtObj;
        HtmlInputHidden hidCol;
        foreach (GridViewRow row in GridView2.Rows) {
            txtObj = row.FindControl("TextBox1") as TextBox;
            hidCol = row.FindControl("Hidden1") as HtmlInputHidden;
            hstab.Add(hidCol.Value, txtObj.Text);
        }
        //GridView2.Rows
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
            strs.AppendFormat("{0} = @{0} ", dcs[i].ColumnName);
        }

        return strs.ToString();
    }

    protected string getBindDbCol(DataColumn dc) {
        StringBuilder strs = new StringBuilder();
        //strs.AppendFormat("        new MySqlParameter(\"@{0}\", {1}.{2})", dc.ColumnName, objName, hstab[dc.ColumnName]); 
        strs.AppendFormat("{4}.Parameters.Add(\"@{0}\", MySqlDbType.{1});{4}.Parameters[\"@{0}\"].Value = {2}.{3};", dc.ColumnName, dc.DataType.Name, this.objName, hstab[dc.ColumnName], cmdName);
        return strs.ToString();
    }
}
