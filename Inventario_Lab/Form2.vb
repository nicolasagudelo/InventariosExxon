Imports MySql.Data.MySqlClient
Module Module1


    Public datasource = "Server=localhost; Database=bd_inventario; Userid=root; Password=dm900494665; Allow Zero Datetime=True; CHARSET=latin1"
    Public Usuario_Perfil = ""
    Public Bandera_Rel As Integer = 0
    Public Consulta_rel = ""
    Public Tabla_Rel = "" ' Tabla del origen de la relacion
    Public Elemento_rel = "" ' Nombre elemento que deseo relacionar
    Public Id_elem = "" ' Codigo/Id elemento que deseo relacionar
    Public Cantidad_a_mover
    Public Tipo_Movi
    Public id_stock
    Public id_Usuar_Per

End Module
Public Class Form2

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim aceptado As Boolean = False
        Dim conexion As New MySqlConnection(datasource)
        Dim consulta As New MySqlCommand(“select * from usuarios where Usuario='” & TextBox1.Text & “' and Contrasena='” & TextBox2.Text & “'”, conexion)
        conexion.Open()
        Dim leerbd As MySqlDataReader = consulta.ExecuteReader()
        If leerbd.Read <> False Then
            aceptado = True
        Else
            aceptado = False
        End If
        If aceptado = True Then
            'MsgBox(“Acceso Concedido”, MsgBoxStyle.Information)
            Dim conexion1 As New MySqlConnection(datasource)
            conexion1.Open()
            Dim consulta1 As String = "Select Nombre_Perfil from perfiles WHERE Id_Perfil='" & leerbd.Item(4) & "'"
            Dim MysqlDadap1 As New MySqlDataAdapter(consulta1, conexion1)
            Dim MysqlDset1 As New DataSet
            MysqlDadap1.Fill(MysqlDset1)
            conexion1.Close()
            Usuario_Perfil = UCase(TextBox1.Text) & " / " & MysqlDset1.Tables(0).Rows(0).ItemArray(0)
            id_Usuar_Per = leerbd.Item(0)
            Form1.Show()
            Me.Close()
        Else
            MsgBox(“Combinación Incorrecta”, MsgBoxStyle.Information)
        End If
        conexion.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Button1.PerformClick()
    End Sub

End Class
