<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication6.Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Styles/DefaultStyle.css" rel="stylesheet" />
    <link href="Styles/ListStyle.css" rel="stylesheet" />
    <link href="Styles/LabelStyle.css" rel="stylesheet" />
    <title>Recursiva Challenge 🚀</title>
</head>
<body id="PageBody" runat="server">
    <header>
        <nav>
            <div class="menu-icon">
                <i class="fa fa-bars fa-2x"></i>
            </div>
            <div class="logo">
                <img src="../Assets/img/logo.png" width="120" alt="Sample Photo" />
            </div>
        </nav>
    </header>
    <form id="form1" runat="server">
        <div id="contenedor" style='display: inline-block'>
            <div id="addFile" onclick="return Upload_openBrowser()">
                <a href="#" class="myButton">+</a>
                <p id="textoButton">Añade tus archivos</p>
            </div>
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="AddFile" onchange="handler()" hidden /><br />
            <asp:Label ID="LblFileName" runat="server" Text="Seleccione un archivo CSV"></asp:Label><br />
            <asp:Button ID="btnUpload" Text="Enviar" runat="server" OnClick="btnUpload_Click" />
        </div>
        <div>
            <asp:Label ID="LblCantidadPersonasRegistradas" CssClass="LabelCssClass" Visible="false" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="LblPromedioSociosRacing" CssClass="LabelCssClass" Visible="false" runat="server" Text=""></asp:Label>
            <asp:BulletedList ID="BulletedList1" runat="server" CssClass="BulletedListCssClass"></asp:BulletedList>
            <asp:BulletedList ID="BulletedList2" runat="server"></asp:BulletedList>
            <asp:BulletedList ID="BulletedList3" runat="server"></asp:BulletedList>
            <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
        </div>
        <script>    
            function Upload_openBrowser() {

                document.getElementById('fileUpload').click();
            }
            function handler() {

                var file = document.getElementById("<%=fileUpload.ClientID%>");

                document.getElementById('LblFileName').innerHTML = file.files[0].name;

            }
            var navbar = document.querySelector('nav')
            window.onscroll = function () {
                if (window.pageYOffset > 0) {
                    navbar.classList.add('scrolled')
                } else {
                    navbar.classList.remove('scrolled')
                }
            }
        </script>
    </form>
</body>
</html>
