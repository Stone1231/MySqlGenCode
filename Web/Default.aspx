<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
    <script>
        function Clear() {
            document.getElementById("<%=txtCode.ClientID%>").value = "";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="connTextBox" runat="server"
             Text="database='neweggchat_db';server='10.16.179.81';uid='root';pwd='root@neweggsso_admin';charset='utf8';pooling=true" Width="649px"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="瀏覽" OnClick="Button2_Click" />
        <br />
        AllTableNames<br />
        <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand">
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="read" Text="讀取" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        ObjectName
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        <input id="Hidden1" type="hidden" runat="server" />
                    </ItemTemplate>    
                </asp:TemplateField>  
                </Columns>
        </asp:GridView>
        <br />
        Columns<br />
        <asp:GridView ID="GridView2" runat="server" OnRowDataBound="GridView2_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        PropertyName
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        <input id="Hidden1" type="hidden" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        DataType.Name
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        ObjectName
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        PrimaryKey<br />
        <asp:GridView ID="GridView3" runat="server"></asp:GridView>
        <br />

        ClassName
        <asp:TextBox ID="txtCmd" runat="server" Text="cmd"></asp:TextBox>

        ObjectName
        <asp:TextBox ID="txtObjectName" runat="server"></asp:TextBox>

        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" OnClientClick="Clear();" Text="產生Insert" />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" OnClientClick="Clear();" Text="產生Update" />
        <br />
        <asp:TextBox ID="txtCode" runat="server" TextMode="MultiLine" Width="700" Rows="30"></asp:TextBox>
    </div>
    </form>
</body>
</html>
