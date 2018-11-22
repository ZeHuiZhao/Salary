<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Salary_MVC.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form method="get">
        <div>
            <p>set_password_ASC</p>
            明文<input type="text" name="InputGuid" value="<%=this.InputWrapper.InputGuid %>" />
            <p>加密:<span><%=this.InputWrapper.JiaMiResult %></span></p>
        </div>
        <div>
            <p>get_password_ASC</p>
            密文<input type="text" name="InputJiaMi" value="<%=this.InputWrapper.InputJiaMi %>" />
            <p>解密:<span><%=this.InputWrapper.JieMiResult %></span></p>
        </div>
        <input type="submit" value="确定" />
    </form>
</body>
</html>
