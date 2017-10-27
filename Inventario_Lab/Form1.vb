Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Drawing.Imaging
Imports System.Data

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim TL(6) As ToolTip 'Ayudas visuales para el usuario
        Dim Menu = {"Administrar", "Movimientos", "Equipos", "Productos", "Proveedores", "Reportes"}
        For i = 1 To 6
            If Me.Controls.Find("PictureBox" & i, True).Count = 1 Then
                Dim b As PictureBox = Me.Controls.Find("PictureBox" & i, True)(0)
                TL(i) = New ToolTip
                TL(i).SetToolTip(b, Menu(i - 1))
            End If
        Next
        Label1.Text = Usuario_Perfil
        TabControl1.Visible = False
        TabControl1.BackColor = System.Drawing.Color.Transparent
        TabControl2.Visible = False
        TabControl2.BackColor = System.Drawing.Color.Transparent
        Foto_Usuario.AllowDrop = True
        Foto_Equipo.AllowDrop = True
        Foto_Producto.AllowDrop = True
    End Sub

    Private Sub Esconder_tabpages()
        TabControl1.Visible = True
        TabControl2.Visible = False
        For i = 1 To 6
            If Me.Controls.Find("TabPage" & i, True).Count = 1 Then
                Dim b As TabPage = Me.Controls.Find("TabPage" & i, True)(0)
                b.Parent = Nothing
            End If
        Next
    End Sub
    Private Sub Esconder_tabpages_submenu()
        TabControl2.Visible = True
        For i = 6 To 16
            If Me.Controls.Find("TabPage" & i, True).Count = 1 Then
                Dim b As TabPage = Me.Controls.Find("TabPage" & i, True)(0)
                b.Parent = Nothing
            End If
        Next
    End Sub
    Private Sub Menu_Seleccionado(ByVal Bandera_Menu As Integer)
        TabControl2.Visible = False
        Select Case Bandera_Menu
            Case 1
                Esconder_tabpages()
                TabPage1.Parent = TabControl1 'Administrar
            Case 2
                Esconder_tabpages()
                TabPage2.Parent = TabControl1 'Movimientos
            Case 3
                Esconder_tabpages()
                TabPage3.Parent = TabControl1 'Equipos
            Case 4
                Esconder_tabpages()
                TabPage4.Parent = TabControl1 'Productos
            Case 5
                Esconder_tabpages()
                TabPage5.Parent = TabControl1 'Proveedores
            Case 6
                Esconder_tabpages()
                TabPage6.Parent = TabControl1 'Reportes
        End Select
    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Menu_Seleccionado(1)
        cant_reg_encon = 0
        z = "USUARIOS"
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Menu_Seleccionado(2)
        Esconder_tabpages_submenu()
        TabPage12.Parent = TabControl2
    End Sub

    Dim reg_bus_equ As String
    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Menu_Seleccionado(3)
        Esconder_tabpages_submenu()
        TabPage13.Parent = TabControl2
        cant_reg_encon = 0
        z = "EQUIPOS"
        Recorrer_Equipos()
        'MsgBox(Convert.ToInt32(Convert.ToBoolean(Tabla1.Rows(0).ItemArray(6).ToString)))
        'MsgBox(Convert.ToInt32(Activo_Equipo.Checked))
    End Sub

    Dim reg_bus_produ As String
    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Menu_Seleccionado(4)
        Esconder_tabpages_submenu()
        TabPage14.Parent = TabControl2
        cant_reg_encon = 0
        z = "PRODUCTOS"
        Recorrer_Productos()
    End Sub
    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Menu_Seleccionado(5)
        Esconder_tabpages_submenu()
        TabPage15.Parent = TabControl2
        cant_reg_encon = 0
        z = "PROVEEDORES"
        Recorrer_Proveedores()
    End Sub
    Private Sub PictureBox6_Click(sender As Object, e As EventArgs) Handles PictureBox6.Click
        Menu_Seleccionado(6)
        Esconder_tabpages_submenu()
        TabPage16.Parent = TabControl2
    End Sub

    'Dim Tab As String = ""
    Dim Tabla1 As New DataTable
    Private Sub Cargar_tabla(ByVal Nombre_Tabla As String)
        Using conexion As New MySqlConnection(datasource)
            Dim Comando As New MySqlCommand("select * from " & Nombre_Tabla & " ", conexion)
            Dim Adaptador As New MySqlDataAdapter(Comando)
            Dim Tabla As New DataTable
            Try
                Adaptador.Fill(Tabla)
            Catch ex As Exception
            Finally
                If conexion.State = ConnectionState.Open Then
                    conexion.Close()
                End If
            End Try
            Tabla1 = Tabla
        End Using
    End Sub
    Dim reg_bus As String = ""
    Dim Encontrado As Boolean = False
    Private Sub GUARDAR(ByVal Nombre_Tabla As String, ByVal Querry As String)
        Cargar_tabla(Nombre_Tabla)
        If Encontrado = True Then
            Dim result As Integer = MessageBox.Show("Esta seguro que desea MODIFICAR el registro", "Alerta", MessageBoxButtons.YesNo)
            If result = DialogResult.No Then
                'MessageBox.Show("No pressed")

            ElseIf result = DialogResult.Yes Then
                Try
                    Using conn As New MySqlConnection(datasource)
                        conn.Open()
                        Dim cmd As New MySqlCommand(Querry, conn)
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Registro MODIFICADO")
                        conn.Close()
                    End Using
                Catch ex As Exception
                    MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
                End Try
            End If
        Else
            Dim result As Integer = MessageBox.Show("Esta seguro que desea AGREGAR el registro", "Alerta", MessageBoxButtons.YesNo)
            If result = DialogResult.No Then
                'MessageBox.Show("No pressed")

            ElseIf result = DialogResult.Yes Then
                Try
                    Using conn As New MySqlConnection(datasource)
                        conn.Open()
                        Dim cmd As New MySqlCommand(Querry, conn)
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Registro AGREGADO")
                        conn.Close()
                    End Using
                Catch ex As Exception
                    MsgBox("El registro no pudo Agregarse por: " & vbCrLf & ex.Message)

                End Try
            End If
        End If
    End Sub
    Public Sub ELIMINAR(ByVal Nombre_Tabla As String, ByVal Id_Registro As String)
        Cargar_tabla(Nombre_Tabla)
        Dim result As Integer = MessageBox.Show("Esta seguro que desea ELIMINAR el registro", "Alerta", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")

        ElseIf result = DialogResult.Yes Then
            'MessageBox.Show("Yes pressed")
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim query As String = "DELETE FROM " & Nombre_Tabla & " WHERE " & Tabla1.Columns(0).ColumnName & " = '" & Id_Registro & "'"
                    Dim cmd As New MySqlCommand(query, conn)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Registro ELIMINADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Eliminarse por: " & vbCrLf & ex.Message)
            End Try
        End If

    End Sub
    Private Sub Usuario(ByVal fila_Usuario As Integer)
        With Perfiles_Usuario
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from perfiles"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .DataSource = MysqlDset.Tables(0)
                .DisplayMember = "Nombre_Perfil" 'elnombre de tu columna de tu base de datos q deseas mostrar
                .ValueMember = "Id_Perfil" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                .SelectedValue = Tabla1.Rows(fila_Usuario).ItemArray(4)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
        With Doag_Usuarios
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from doag"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .DataSource = MysqlDset.Tables(0)
                .DisplayMember = "Nombre_Doag" 'elnombre de tu columna de tu base de datos q deseas mostrar
                .ValueMember = "Id_Doag" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                .SelectedValue = Tabla1.Rows(fila_Usuario).ItemArray(6)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
    End Sub
    Private Sub Gestion_Usuario_Click(sender As Object, e As EventArgs) Handles Gestion_Usuario.Click
        Esconder_tabpages_submenu()
        TabPage7.Parent = TabControl2 'Usuarios
        Recorrer_Usuarios()
        TabPage8.Parent = TabControl2 'Doag
        Cargar_tabla("Doag")
        Nombre_Doag.Text = Tabla1.Rows(0).ItemArray(1).ToString
        Monto_Doag.Text = Tabla1.Rows(0).ItemArray(2).ToString
        Comentario_Doag.Text = Tabla1.Rows(0).ItemArray(3).ToString
        DataGridView1.DataSource = Nothing
        DataGridView1.DataSource = Tabla1
        DataGridView1.ColumnHeadersVisible = False
        DataGridView1.ReadOnly = True
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Columns(1).Width = 160
        DataGridView1.Columns(2).Width = 160
        DataGridView1.Columns(3).Width = 310
        DataGridView1.Columns(0).Visible = False
        TabPage9.Parent = TabControl2 'Perfiles
        Cargar_tabla("Perfiles")
        Nombre_Perfil.Text = Tabla1.Rows(0).ItemArray(1).ToString
        Nivel_Permisos.Text = Tabla1.Rows(0).ItemArray(2).ToString
    End Sub

    Dim Categ As String
    Private Sub Gestion_Almacen_Click(sender As Object, e As EventArgs) Handles Gestion_Almacen.Click
        Esconder_tabpages_submenu()
        TabPage10.Parent = TabControl2
        'Cargar_tabla("Categorias")
        With Nombre_Categoria
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from categorias"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .DataSource = MysqlDset.Tables(0)
                .DisplayMember = "Nombre_Categoria" 'elnombre de tu columna de tu base de datos q deseas mostrar
                .ValueMember = "Id_Categoria" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                .SelectedValue = MysqlDset.Tables(0).Rows(0).Item(0)
                Categ = MysqlDset.Tables(0).Rows(0).Item(0)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
        Using conexion As New MySqlConnection(datasource)
            Dim Comando As New MySqlCommand(“select * from categorias_sub where " & "Id_Categoria" & "='” & Categ & “'”, conexion)
            Dim Adaptador As New MySqlDataAdapter(Comando)
            Dim Tabla As New DataTable
            Try
                Adaptador.Fill(Tabla)
                DataGridView2.DataSource = Tabla
                DataGridView2.ReadOnly = False
                DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DataGridView2.Columns(1).Width = 190
                DataGridView2.Columns(0).Visible = False
                DataGridView2.Columns(2).Visible = False
                DataGridView2.Rows(0).Selected = True
                DataGridView2.CurrentCell = DataGridView2.Rows(0).Cells(1)
            Catch ex As Exception
            Finally
                If conexion.State = ConnectionState.Open Then
                    conexion.Close()
                End If
            End Try
        End Using
        DataGridView2.ColumnHeadersVisible = False
        TabPage11.Parent = TabControl2
        Llenar_Combos_Ubicaciones()
        Cargar_tabla("Ubicaciones")
        reg_bus_ubic = Tabla1.Rows(0).ItemArray(0)
        Estantes.Text = Tabla1.Rows(0).ItemArray(1).ToString
        Entrepanos.Text = Tabla1.Rows(0).ItemArray(2).ToString
        Cajas_Colores.Text = Tabla1.Rows(0).ItemArray(3).ToString
        Zonas.Text = Tabla1.Rows(0).ItemArray(4).ToString
        DataGridView3.DataSource = Nothing
        DataGridView3.DataSource = Tabla1
        DataGridView3.ReadOnly = True
        DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView3.Columns(1).Width = 100
        DataGridView3.Columns(2).Width = 120
        DataGridView3.Columns(3).Width = 180
        DataGridView3.Columns(4).Width = 250
        DataGridView3.Columns(0).Visible = False
    End Sub

    Private Sub Categorias()
        With Nombre_Categoria
            Try
                .SelectedValue = Nombre_Categoria.SelectedValue
                Categ = Nombre_Categoria.SelectedValue
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
        Using conexion As New MySqlConnection(datasource)
            Dim Comando As New MySqlCommand(“select * from categorias_sub where " & "Id_Categoria" & "='” & Categ & “'”, conexion)
            Dim Adaptador As New MySqlDataAdapter(Comando)
            Dim Tabla As New DataTable
            Try
                Adaptador.Fill(Tabla)
                DataGridView2.DataSource = Tabla
                DataGridView2.ReadOnly = False
                DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DataGridView2.Columns(1).Width = 190
                DataGridView2.Columns(0).Visible = False
                DataGridView2.Columns(2).Visible = False
            Catch ex As Exception
            Finally
                If conexion.State = ConnectionState.Open Then
                    conexion.Close()
                End If
            End Try
        End Using
        DataGridView2.ColumnHeadersVisible = False
    End Sub
    Private Sub Llenar_Combos_Ubicaciones()
        With Estantes
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from datos_app"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .Items.Clear()
                Dim a As String = MysqlDset.Tables(0).Rows(0).Item(2)
                Dim i1 As Integer = 0
                For i1 = 1 To a
                    .Items.Add(i1)
                Next
                .SelectedValue = Estantes.Items(0)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
        With Entrepanos
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from datos_app"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .Items.Clear()
                Dim a As String = MysqlDset.Tables(0).Rows(1).Item(2)
                Dim i1 As Integer = 0
                For i1 = 1 To a
                    .Items.Add(i1)
                Next
                .SelectedValue = Entrepanos.Items(0)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
        With Cajas_Colores
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from datos_app"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .Items.Clear()
                Dim a As String = MysqlDset.Tables(0).Rows(2).Item(2)
                Dim i1 As Integer = 0
                For i1 = 1 To a
                    .Items.Add(i1)
                Next
                .Items.Add(MysqlDset.Tables(0).Rows(3).Item(1))
                .Items.Add(MysqlDset.Tables(0).Rows(4).Item(1))
                .Items.Add(MysqlDset.Tables(0).Rows(5).Item(1))
                .Items.Add(MysqlDset.Tables(0).Rows(6).Item(1))
                .SelectedValue = Cajas_Colores.Items(0)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
        With Zonas
            Try
                .Items.Clear()
                .Items.Add("Bodega")
                .Items.Add("Reactivos")
                .Items.Add("Gases")
                .Items.Add("Acceso Especial")
                .SelectedValue = Zonas.Items(0)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
    End Sub

    Private Sub DataGridView3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView3.SelectionChanged
        Try
            Dim fila_actual As Integer = (DataGridView3.CurrentRow.Index)
            If fila_actual = (DataGridView1.Rows.Count - 1) Then
                reg_bus_doag = "nuevo"
                Nombre_Doag.Text = ""
                Monto_Doag.Text = ""
                Comentario_Doag.Text = ""
            Else
                Cargar_tabla("Ubicaciones")
                reg_bus_ubic = Tabla1.Rows(fila_actual).ItemArray(0)
                Estantes.Text = Tabla1.Rows(fila_actual).ItemArray(1).ToString
                Entrepanos.Text = Tabla1.Rows(fila_actual).ItemArray(2).ToString
                Cajas_Colores.Text = Tabla1.Rows(fila_actual).ItemArray(3).ToString
                Zonas.Text = Tabla1.Rows(fila_actual).ItemArray(4).ToString
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
    End Sub

    Dim usuario_num = 0

    Private Sub Recorrer_Usuarios()
        Cargar_tabla("Usuarios")
        Label10.Text = "Usuario " & (usuario_num + 1) & " de " & (Tabla1.Rows.Count)
        reg_bus = Tabla1.Rows(usuario_num).ItemArray(0).ToString
        Nombre_Usuario.Text = Tabla1.Rows(usuario_num).ItemArray(1).ToString
        Usuario_Nickname.Text = Tabla1.Rows(usuario_num).ItemArray(2).ToString
        Contrasena_Usuario.Text = Tabla1.Rows(usuario_num).ItemArray(3).ToString
        If Tabla1.Rows(usuario_num).ItemArray(5).ToString <> "" Or IsNothing(Tabla1.Rows(usuario_num).ItemArray(5)) Then
            'Foto_Usuario.Load(Tabla1.Rows(usuario_num).ItemArray(5).ToString)
            Foto_Usuario.Image = Image.FromFile(Tabla1.Rows(usuario_num).ItemArray(5).ToString)
        Else
            Foto_Usuario.Load("C:\Users\Msi\documents\visual studio 2017\Projects\Inventario_Lab\Inventario_Lab\Resources\Foto!.png")
        End If
        'Inventario_Lab.My.Resources.Resources.Foto
        Usuario(usuario_num)
    End Sub
    Private Sub Guardar_Usuario_Click(sender As Object, e As EventArgs) Handles Guardar_Usuario.Click
        Dim conexion As New MySqlConnection(datasource)
        Dim consulta As New MySqlCommand(“select * from usuarios where Id_Usuario='” & reg_bus & “'”, conexion)
        conexion.Open()
        Dim leerbd As MySqlDataReader = consulta.ExecuteReader()
        If leerbd.Read <> False Then
            Encontrado = True
            GUARDAR("usuarios", "UPDATE usuarios SET Nombre_Usuario = '" & Nombre_Usuario.Text & "', Usuario = '" & Usuario_Nickname.Text & "', " &
                                "Contrasena = '" & Contrasena_Usuario.Text & "', Id_Perfil = '" & Perfiles_Usuario.SelectedValue & "', " &
                                "Id_Doag = '" & Doag_Usuarios.SelectedValue & "' " &
                                "WHERE Id_Usuario = '" & reg_bus & "'")
        Else
            Encontrado = False
            GUARDAR("usuarios", "INSERT INTO usuarios (Nombre_Usuario, Usuario, Contrasena, Id_Perfil, Id_Doag) " &
                            "VALUES ('" & Nombre_Usuario.Text & "','" & Usuario_Nickname.Text & "', '" & Contrasena_Usuario.Text & "'," &
                            " '" & Perfiles_Usuario.SelectedValue & "', '" & Doag_Usuarios.SelectedValue & "')")

        End If
        conexion.Close()
        Recorrer_Usuarios()
        Buscar_Usuario.Enabled = True
    End Sub

    Private Sub Nuevo_Usuario_Click(sender As Object, e As EventArgs) Handles Nuevo_Usuario.Click
        reg_bus = "nuevo"
        Cargar_tabla("Usuarios")
        usuario_num = Tabla1.Rows.Count
        Buscar_Usuario.Enabled = False
        usuario_num = Tabla1.Rows.Count
        Nombre_Usuario.Text = ""
        Usuario_Nickname.Text = ""
        Contrasena_Usuario.Text = ""
    End Sub

    Private Sub Eliminar_Usuario_Click(sender As Object, e As EventArgs) Handles Eliminar_Usuario.Click
        ELIMINAR("Usuarios", reg_bus)
    End Sub

    Dim cant_reg_encon As Integer = 0
    Dim z As String 'memorioa del usuario a buscar
    Private Sub Buscar_Usuario_Click(sender As Object, e As EventArgs) Handles Buscar_Usuario.Click
        Cargar_tabla("Usuarios")
        If z <> Buscar_Us.Text Then
            cant_reg_encon = 0
        End If
        Dim conexion As New MySqlConnection(datasource)
        conexion.Open()
        Dim consulta As String = "Select * from usuarios"
        Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
        Dim MysqlDset As New DataSet
        MysqlDadap.Fill(MysqlDset)
        conexion.Close()
        Dim i As Integer = 0
        Dim foundRows() As Data.DataRow
        foundRows = MysqlDset.Tables(0).Select("Nombre_Usuario Like '" & Buscar_Us.Text & "%'")
        z = Buscar_Us.Text
        If cant_reg_encon = 0 And foundRows.Length > 1 Then
            cant_reg_encon = foundRows.Length
            For Each row In Tabla1.Rows
                If foundRows(cant_reg_encon - 1).Item(1) = row(1) Then
                    'MsgBox(foundRows(cant_reg_encon - 1).Item(1))
                    usuario_num = i
                    Recorrer_Usuarios()
                    cant_reg_encon = cant_reg_encon - 1
                    GoTo otro
                End If
                i = i + 1
            Next
        Else
            If foundRows.Length = 0 Then
                MsgBox("No se encontro ninguna coincidencia")
            ElseIf cant_reg_encon = 0 Then
                For Each row In Tabla1.Rows
                    If foundRows(cant_reg_encon).Item(1) = row(1) Then
                        usuario_num = i
                        Recorrer_Usuarios()
                        GoTo otro
                    End If
                    i = i + 1
                Next
            Else
                For Each row In Tabla1.Rows
                    If foundRows(cant_reg_encon - 1).Item(1) = row(1) Then
                        usuario_num = i
                        Recorrer_Usuarios()
                        cant_reg_encon = cant_reg_encon - 1
                        GoTo otro
                    End If
                    i = i + 1
                Next
            End If
        End If
otro:
    End Sub

    Private Sub Siguiente_Usuario_Click(sender As Object, e As EventArgs) Handles Siguiente_Usuario.Click
        If usuario_num >= (Tabla1.Rows.Count - 1) Then
            usuario_num = 0
            Recorrer_Usuarios()
        Else
            usuario_num = usuario_num + 1
            Recorrer_Usuarios()
        End If
    End Sub
    Private Sub Anterior_Usuario_Click(sender As Object, e As EventArgs) Handles Anterior_Usuario.Click
        If usuario_num = 0 Then
            usuario_num = Tabla1.Rows.Count - 1
            Recorrer_Usuarios()
        Else
            usuario_num = usuario_num - 1
            Recorrer_Usuarios()
        End If
    End Sub

    Dim reg_bus_doag As String
    Private Sub DataGridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged
        Try
            Dim fila_actual As Integer = (DataGridView1.CurrentRow.Index)
            If fila_actual = (DataGridView1.Rows.Count - 1) Then
                reg_bus_doag = "nuevo"
                Nombre_Doag.Text = ""
                Monto_Doag.Text = ""
                Comentario_Doag.Text = ""
            Else
                Cargar_tabla("Doag")
                reg_bus_doag = Tabla1.Rows(fila_actual).ItemArray(0).ToString
                Nombre_Doag.Text = Tabla1.Rows(fila_actual).ItemArray(1).ToString
                Monto_Doag.Text = Format(Tabla1.Rows(fila_actual).ItemArray(2).ToString, "Currency") 'Text1.Text = Format(Numero, "Currency")
                Comentario_Doag.Text = Tabla1.Rows(fila_actual).ItemArray(3).ToString
            End If
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Guardar_Doag_Click(sender As Object, e As EventArgs) Handles Guardar_Doag.Click
        Dim conexion As New MySqlConnection(datasource)
        Dim consulta As New MySqlCommand(“select * from doag where " & Tabla1.Columns(0).ColumnName & "='” & reg_bus_doag & “'”, conexion)
        conexion.Open()
        Dim leerbd As MySqlDataReader = consulta.ExecuteReader()
        If leerbd.Read <> False Then
            Encontrado = True
            GUARDAR("doag", "UPDATE doag SET Nombre_Doag = '" & Nombre_Doag.Text & "', Monto = '" & CType(Monto_Doag.Text, Integer) & "', " &
                                "Comentario = '" & Comentario_Doag.Text & "' " &
                                "WHERE Id_Doag = '" & reg_bus_doag & "'")
        Else
            Encontrado = False
            GUARDAR("doag", "INSERT INTO doag (Nombre_Doag, Monto, Comentario) " &
                            "VALUES ('" & Nombre_Doag.Text & "','" & Monto_Doag.Text & "', '" & Comentario_Doag.Text & "')")
        End If
        conexion.Close()
        Cargar_tabla("Doag")
        DataGridView1.DataSource = Nothing
        DataGridView1.DataSource = Tabla1
        DataGridView1.ColumnHeadersVisible = False
        DataGridView1.ReadOnly = True
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.Columns(0).Visible = False
    End Sub

    Private Sub Nuevo_Doag_Click(sender As Object, e As EventArgs) Handles Nuevo_Doag.Click
        reg_bus_doag = "nuevo"
        Nombre_Doag.Text = ""
        Monto_Doag.Text = ""
        Comentario_Doag.Text = ""
    End Sub

    Private Sub Eliminar_Doag_Click(sender As Object, e As EventArgs) Handles Eliminar_Doag.Click
        ELIMINAR("doag", reg_bus_doag)
    End Sub
    Private Sub Nombre_Categoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Nombre_Categoria.SelectedIndexChanged
        DataGridView2.DataSource = Nothing
        Try
            Using conexion As New MySqlConnection(datasource)
                Dim Comando As New MySqlCommand(“select * from categorias_sub where " & "Id_Categoria" & "='” & Nombre_Categoria.SelectedValue & “'”, conexion)
                Dim Adaptador As New MySqlDataAdapter(Comando)
                Categ = Nombre_Categoria.SelectedValue
                Dim Tabla As New DataTable
                Try
                    Adaptador.Fill(Tabla)
                    DataGridView2.DataSource = Tabla
                    DataGridView2.ReadOnly = False
                    DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                    DataGridView2.Columns(1).Width = 190
                    DataGridView2.Columns(0).Visible = False
                    DataGridView2.Columns(2).Visible = False
                    DataGridView2.Rows(0).Selected = True
                    DataGridView2.CurrentCell = DataGridView2.Rows(0).Cells(1)
                Catch ex As Exception
                Finally
                    If conexion.State = ConnectionState.Open Then
                        conexion.Close()
                    End If
                End Try
            End Using
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Guardar_Categoria_Click(sender As Object, e As EventArgs) Handles Guardar_Categoria.Click
        Try
            Dim conexion As New MySqlConnection(datasource)
            Dim consulta As New MySqlCommand(“select * from categorias where Id_Categoria='” & Categ & “'”, conexion)
            conexion.Open()
            Dim leerbd As MySqlDataReader = consulta.ExecuteReader()
            If leerbd.Read <> False Then
                Encontrado = True
                GUARDAR("categorias", "UPDATE categorias SET Nombre_Categoria = '" & Nombre_Categoria.Text & "' " &
                                "WHERE Id_Categoria = '" & Categ & "'")
            Else
                Encontrado = False
                GUARDAR("categorias", "INSERT INTO categorias (Nombre_Categoria) " &
                            "VALUES ('" & Nombre_Categoria.Text & "')")
            End If
            conexion.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Dim nom_cat As String = Nombre_Categoria.Text
        With Nombre_Categoria
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from categorias"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .DataSource = MysqlDset.Tables(0)
                .DisplayMember = "Nombre_Categoria" 'elnombre de tu columna de tu base de datos q deseas mostrar
                .ValueMember = "Id_Categoria" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                For Each item In Nombre_Categoria.Items
                    If item.Row(1).ToString = nom_cat Then
                        .SelectedValue = item.Row(0).ToString
                        Categ = item.Row(0).ToString
                    End If
                Next

                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
    End Sub

    Private Sub Nueva_Categoria_Click(sender As Object, e As EventArgs) Handles Nueva_Categoria.Click
        Categ = "nuevo"
        Nombre_Categoria.Text = ""
    End Sub
    Private Sub Eliminar_Categoria_Click(sender As Object, e As EventArgs) Handles Eliminar_Categoria.Click
        Dim a As Integer = DataGridView2.Rows.Count
        If (a - 1) < 1 Then
            ELIMINAR("categorias", Categ)
        Else
            MsgBox("Antes de eliminar esta categoria, asegúrese de que no tenga subcategorias")
        End If
    End Sub
    Private Sub Guardar_SubCategoria_Click(sender As Object, e As EventArgs) Handles Guardar_SubCategoria.Click
        Dim fila As Integer = (DataGridView2.Rows.Count - 2)
        For i = 0 To fila
            If DataGridView2.Item(0, i).FormattedValue = "" Then
                MsgBox(DataGridView2.Item(1, i).FormattedValue)
                Try
                    Using conn As New MySqlConnection(datasource)
                        conn.Open()
                        Dim cmd As New MySqlCommand("INSERT INTO categorias_sub (Nombre_SubCategoria, Id_Categoria) " &
                                "VALUES ('" & DataGridView2.Item(1, i).FormattedValue & "', '" & Categ & "')", conn)
                        cmd.ExecuteNonQuery()
                        'MessageBox.Show("Registro AGREGADO")
                        conn.Close()
                    End Using
                Catch ex As Exception
                    MsgBox("El registro no pudo Agregarse por: " & vbCrLf & ex.Message)
                End Try
            Else
                Try
                    Using conn As New MySqlConnection(datasource)
                        conn.Open()
                        Dim cmd As New MySqlCommand("UPDATE categorias_sub SET Nombre_SubCategoria = '" & DataGridView2.Item(1, i).FormattedValue & "' " &
                                    "WHERE Id_SubCategoria = '" & DataGridView2.Item(0, i).FormattedValue & "'", conn)
                        cmd.ExecuteNonQuery()
                        'MessageBox.Show("Registro MODIFICADO")
                        conn.Close()
                    End Using
                Catch ex As Exception
                    MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
                End Try
            End If

        Next
        MessageBox.Show("SubCategorias Actualizadas")
        Categorias()
    End Sub

    Private Sub Eliminar_SubCategoria_Click(sender As Object, e As EventArgs) Handles Eliminar_SubCategoria.Click
        Dim a As Integer
        If DataGridView2.CurrentRow.Index.ToString <> Nothing Then
            a = DataGridView2.CurrentRow.Index
        Else
            a = 0
        End If
        ELIMINAR("categorias_sub", DataGridView2.Item(0, a).FormattedValue)
        Categorias()
    End Sub

    Private Sub Agregar_Estante_Click(sender As Object, e As EventArgs) Handles Agregar_Estante.Click
        Dim a As Integer = 0
        Dim b As String = ""
        Try
            Dim conexion As New MySqlConnection(datasource)
            conexion.Open()
            Dim consulta As String = "Select * from datos_app"
            Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
            Dim MysqlDset As New DataSet
            MysqlDadap.Fill(MysqlDset)
            conexion.Close()
            a = Convert.ToInt16(MysqlDset.Tables(0).Rows(0).Item(2))
            b = MysqlDset.Tables(0).Rows(0).Item(0)
            '.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Try
            Using conn As New MySqlConnection(datasource)
                conn.Open()
                Dim cmd As New MySqlCommand("UPDATE datos_app SET Detalles = '" & (a + 1) & "' " &
                            "WHERE IdDatos_App = '" & b & "'", conn)
                cmd.ExecuteNonQuery()
                'MessageBox.Show("Registro MODIFICADO")
                conn.Close()
            End Using
        Catch ex As Exception
            MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
        End Try
        Llenar_Combos_Ubicaciones()
    End Sub

    Private Sub Eliminar_Estante_Click(sender As Object, e As EventArgs) Handles Eliminar_Estante.Click
        Dim a As Integer = 0
        Dim b As String = ""
        Try
            Dim conexion As New MySqlConnection(datasource)
            conexion.Open()
            Dim consulta As String = "Select * from datos_app"
            Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
            Dim MysqlDset As New DataSet
            MysqlDadap.Fill(MysqlDset)
            conexion.Close()
            a = Convert.ToInt16(MysqlDset.Tables(0).Rows(0).Item(2))
            b = MysqlDset.Tables(0).Rows(0).Item(0)
            '.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Dim Num_men As Integer = 0
        For Each row As DataGridViewRow In Me.DataGridView3.Rows
            'obtenemos el valor de la columna en la variable declarada
            If Convert.ToInt16(row.Cells(1).Value) > Num_men Then
                Num_men = row.Cells(1).Value 'donde (0) es la columna a recorrer
            End If
        Next
        If Num_men < a Then
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim cmd As New MySqlCommand("UPDATE datos_app SET Detalles = '" & (a - 1) & "' " &
                                "WHERE IdDatos_App = '" & b & "'", conn)
                    cmd.ExecuteNonQuery()
                    'MessageBox.Show("Registro MODIFICADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
            End Try
        Else
            MsgBox("Su numero minimo de estantes: " & a)
        End If
        Llenar_Combos_Ubicaciones()
    End Sub

    Private Sub Agregar_Entrepano_Click(sender As Object, e As EventArgs) Handles Agregar_Entrepano.Click
        Dim a As Integer = 0
        Dim b As String = ""
        Try
            Dim conexion As New MySqlConnection(datasource)
            conexion.Open()
            Dim consulta As String = "Select * from datos_app"
            Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
            Dim MysqlDset As New DataSet
            MysqlDadap.Fill(MysqlDset)
            conexion.Close()
            a = Convert.ToInt16(MysqlDset.Tables(0).Rows(1).Item(2))
            b = MysqlDset.Tables(0).Rows(1).Item(0)
            '.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Try
            Using conn As New MySqlConnection(datasource)
                conn.Open()
                Dim cmd As New MySqlCommand("UPDATE datos_app SET Detalles = '" & (a + 1) & "' " &
                            "WHERE IdDatos_App = '" & b & "'", conn)
                cmd.ExecuteNonQuery()
                'MessageBox.Show("Registro MODIFICADO")
                conn.Close()
            End Using
        Catch ex As Exception
            MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
        End Try
        Llenar_Combos_Ubicaciones()
    End Sub

    Private Sub Eliminar_Entrepano_Click(sender As Object, e As EventArgs) Handles Eliminar_Entrepano.Click
        Dim a As Integer = 0
        Dim b As String = ""
        Try
            Dim conexion As New MySqlConnection(datasource)
            conexion.Open()
            Dim consulta As String = "Select * from datos_app"
            Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
            Dim MysqlDset As New DataSet
            MysqlDadap.Fill(MysqlDset)
            conexion.Close()
            a = Convert.ToInt16(MysqlDset.Tables(0).Rows(1).Item(2))
            b = MysqlDset.Tables(0).Rows(1).Item(0)
            '.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Dim Num_men As Integer = 0
        For Each row As DataGridViewRow In Me.DataGridView3.Rows
            'obtenemos el valor de la columna en la variable declarada
            If Convert.ToInt16(row.Cells(2).Value) > Num_men Then
                Num_men = row.Cells(2).Value 'donde (0) es la columna a recorrer
            End If
        Next
        If Num_men < a Then
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim cmd As New MySqlCommand("UPDATE datos_app SET Detalles = '" & (a - 1) & "' " &
                                "WHERE IdDatos_App = '" & b & "'", conn)
                    cmd.ExecuteNonQuery()
                    'MessageBox.Show("Registro MODIFICADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
            End Try
        Else
            MsgBox("Su numero minimo de entrepaños es: " & a)
        End If
        Llenar_Combos_Ubicaciones()
    End Sub

    Private Sub Agregar_Caja_Click(sender As Object, e As EventArgs) Handles Agregar_Caja.Click
        Dim a As Integer = 0
        Dim b As String = ""
        Try
            Dim conexion As New MySqlConnection(datasource)
            conexion.Open()
            Dim consulta As String = "Select * from datos_app"
            Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
            Dim MysqlDset As New DataSet
            MysqlDadap.Fill(MysqlDset)
            conexion.Close()
            a = Convert.ToInt16(MysqlDset.Tables(0).Rows(2).Item(2))
            b = MysqlDset.Tables(0).Rows(2).Item(0)
            '.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Try
            Using conn As New MySqlConnection(datasource)
                conn.Open()
                Dim cmd As New MySqlCommand("UPDATE datos_app SET Detalles = '" & (a + 1) & "' " &
                            "WHERE IdDatos_App = '" & b & "'", conn)
                cmd.ExecuteNonQuery()
                'MessageBox.Show("Registro MODIFICADO")
                conn.Close()
            End Using
        Catch ex As Exception
            MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
        End Try
        Llenar_Combos_Ubicaciones()
    End Sub

    Private Sub Eliminar_Caja_Click(sender As Object, e As EventArgs) Handles Eliminar_Caja.Click
        Dim a As Integer = 0
        Dim b As String = ""
        Try
            Dim conexion As New MySqlConnection(datasource)
            conexion.Open()
            Dim consulta As String = "Select * from datos_app"
            Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
            Dim MysqlDset As New DataSet
            MysqlDadap.Fill(MysqlDset)
            conexion.Close()
            a = Convert.ToInt16(MysqlDset.Tables(0).Rows(2).Item(2))
            b = MysqlDset.Tables(0).Rows(2).Item(0)
            '.Enabled = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Dim Num_men As Integer = 0
        For Each row As DataGridViewRow In Me.DataGridView3.Rows
            'obtenemos el valor de la columna en la variable declarada
            If row.Cells(3).Value = "Azul" Or row.Cells(3).Value = "Rojo" Or row.Cells(3).Value = "Amarillo" Or row.Cells(3).Value = "Blanco" Then
            Else
                If Convert.ToInt16(row.Cells(3).Value) > Num_men Then
                    Num_men = row.Cells(3).Value 'donde (0) es la columna a recorrer
                End If
            End If

        Next
        If Num_men < a Then
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim cmd As New MySqlCommand("UPDATE datos_app SET Detalles = '" & (a - 1) & "' " &
                                "WHERE IdDatos_App = '" & b & "'", conn)
                    cmd.ExecuteNonQuery()
                    'MessageBox.Show("Registro MODIFICADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
            End Try
        Else
            MsgBox("Su numero minimo de cajas es: " & a)
        End If
        Llenar_Combos_Ubicaciones()
    End Sub
    Dim reg_bus_ubic As String
    Private Sub Guardar_Ubicacion_Click(sender As Object, e As EventArgs) Handles Guardar_Ubicacion.Click
        Cargar_tabla("ubicaciones")
        If Estantes.SelectedIndex = (-1) Or Estantes.SelectedIndex = (-1) Or Estantes.SelectedIndex = (-1) Or Estantes.SelectedIndex = (-1) Then
            MsgBox("Ningun campo puede ser vacio")
            GoTo Err
        End If
        Dim a As Integer
        Dim conexion As New MySqlConnection(datasource)
        Dim consulta As New MySqlCommand(“select * from ubicaciones where " & Tabla1.Columns(0).ColumnName & "='” & reg_bus_ubic & “'”, conexion)
        conexion.Open()
        Dim leerbd As MySqlDataReader = consulta.ExecuteReader()
        If leerbd.Read <> False Then
            Encontrado = True
            GUARDAR("ubicaciones", "UPDATE ubicaciones SET Estante = '" & Estantes.Text & "', Entrepano = '" & Entrepanos.Text & "', " &
                                "Caja_Color = '" & Cajas_Colores.Text & "', " & "Zona = '" & Zonas.Text & "' " &
                                "WHERE Id_Ubicacion = '" & reg_bus_ubic & "'")
            a = reg_bus_ubic
        Else
            Encontrado = False
            GUARDAR("ubicaciones", "INSERT INTO ubicaciones (Estante, Entrepano, Caja_Color, Zona) " &
                            "VALUES ('" & Estantes.Text & "','" & Entrepanos.Text & "', '" & Cajas_Colores.Text & "', '" & Zonas.Text & "')")
            Cargar_tabla("ubicaciones")
            a = Tabla1.Rows((Tabla1.Rows.Count - 1)).ItemArray(0)
        End If

        DataGridView3.DataSource = Nothing
        DataGridView3.DataSource = Tabla1
        DataGridView3.ReadOnly = True
        DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView3.Columns(1).Width = 100
        DataGridView3.Columns(2).Width = 120
        DataGridView3.Columns(3).Width = 180
        DataGridView3.Columns(4).Width = 250
        DataGridView3.Columns(0).Visible = False
        For Each row As DataGridViewRow In Me.DataGridView3.Rows
            'obtenemos el valor de la columna en la variable declarada
            If Convert.ToInt16(row.Cells(0).Value) = a Then
                DataGridView3.CurrentCell = DataGridView3(1, row.Index)
            End If
        Next
Err:
    End Sub

    Private Sub Nueva_Ubicacion_Click(sender As Object, e As EventArgs) Handles Nueva_Ubicacion.Click
        reg_bus_ubic = "nuevo"
        Llenar_Combos_Ubicaciones()
        Estantes.SelectedIndex = -1
        Entrepanos.SelectedItem = -1
        Cajas_Colores.SelectedItem = -1
        Zonas.SelectedItem = -1
    End Sub

    Private Sub Eliminar_Ubicacion_Click(sender As Object, e As EventArgs) Handles Eliminar_Ubicacion.Click
        ELIMINAR("Ubicaciones", reg_bus_ubic)
        Dim a = Tabla1.Rows((Tabla1.Rows.Count - 1)).ItemArray(0)
        Cargar_tabla("ubicaciones")
        DataGridView3.DataSource = Nothing
        DataGridView3.DataSource = Tabla1
        DataGridView3.ReadOnly = True
        DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView3.Columns(1).Width = 100
        DataGridView3.Columns(2).Width = 120
        DataGridView3.Columns(3).Width = 180
        DataGridView3.Columns(4).Width = 250
        DataGridView3.Columns(0).Visible = False
        For Each row As DataGridViewRow In Me.DataGridView3.Rows
            'obtenemos el valor de la columna en la variable declarada
            If Convert.ToInt16(row.Cells(0).Value) = a Then
                DataGridView3.CurrentCell = DataGridView3(1, row.Index)
            End If
        Next
    End Sub
    Private Sub Equipos_Consultar_Click(sender As Object, e As EventArgs) Handles Equipos_Consultar.Click
        Cargar_tabla("Equipos")
        If z <> Buscar_Eq.Text Then
            cant_reg_encon = 0
        End If
        Dim conexion As New MySqlConnection(datasource)
        conexion.Open()
        Dim consulta As String = "Select * from Equipos"
        Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
        Dim MysqlDset As New DataSet
        MysqlDadap.Fill(MysqlDset)
        conexion.Close()
        Dim i As Integer = 0
        Dim foundRows() As Data.DataRow
        foundRows = MysqlDset.Tables(0).Select("Nombre_Equipo Like '" & Buscar_Eq.Text & "%'")
        z = Buscar_Eq.Text
        If cant_reg_encon = 0 And foundRows.Length > 1 Then
            cant_reg_encon = foundRows.Length
            For Each row In Tabla1.Rows
                If foundRows(cant_reg_encon - 1).Item(1) = row(1) Then
                    'MsgBox(foundRows(cant_reg_encon - 1).Item(1))
                    Equipo_num = i
                    Recorrer_Equipos()
                    cant_reg_encon = cant_reg_encon - 1
                    GoTo otro
                End If
                i = i + 1
            Next
        Else
            If foundRows.Length = 0 Then
                MsgBox("No se encontro ninguna coincidencia")
            ElseIf cant_reg_encon = 0 Then
                For Each row In Tabla1.Rows
                    If foundRows(cant_reg_encon).Item(1) = row(1) Then
                        Equipo_num = i
                        Recorrer_Equipos()
                        GoTo otro
                    End If
                    i = i + 1
                Next
            Else
                For Each row In Tabla1.Rows
                    If foundRows(cant_reg_encon - 1).Item(1) = row(1) Then
                        Equipo_num = i
                        Recorrer_Equipos()
                        cant_reg_encon = cant_reg_encon - 1
                        GoTo otro
                    End If
                    i = i + 1
                Next
            End If
        End If
otro:
    End Sub
    Private Sub Anterior_Equipo_Click(sender As Object, e As EventArgs) Handles Anterior_Equipo.Click
        If Equipo_num = 0 Then
            Equipo_num = Tabla1.Rows.Count - 1
            Recorrer_Equipos()
        Else
            Equipo_num = Equipo_num - 1
            Recorrer_Equipos()
        End If
    End Sub
    Private Sub Siguiente_Equipo_Click(sender As Object, e As EventArgs) Handles Siguiente_Equipo.Click
        If Equipo_num >= (Tabla1.Rows.Count - 1) Then
            Equipo_num = 0
            Recorrer_Equipos()
        Else
            Equipo_num = Equipo_num + 1
            Recorrer_Equipos()
        End If
    End Sub

    Dim Equipo_num = 0

    Private Sub Recorrer_Equipos()
        Cargar_tabla("Equipos")
        Label60.Text = "Equipo " & (Equipo_num + 1) & " de " & (Tabla1.Rows.Count)
        reg_bus_equ = Tabla1.Rows(Equipo_num).ItemArray(0).ToString
        Numero_Equipo.Text = Tabla1.Rows(Equipo_num).ItemArray(1).ToString
        Nombre_Equipo.Text = Tabla1.Rows(Equipo_num).ItemArray(2).ToString
        Marca_Equipo.Text = Tabla1.Rows(Equipo_num).ItemArray(3).ToString
        Serie_Equipo.Text = Tabla1.Rows(Equipo_num).ItemArray(4).ToString
        Activo_Equipo.Checked = Tabla1.Rows(Equipo_num).ItemArray(6).ToString
        If Tabla1.Rows(Equipo_num).ItemArray(5).ToString <> "" Or IsNothing(Tabla1.Rows(Equipo_num).ItemArray(5)) Then
            Foto_Equipo.Image = Image.FromFile(Tabla1.Rows(Equipo_num).ItemArray(5).ToString)
        Else
            Foto_Equipo.Load("C:\Users\Msi\documents\visual studio 2017\Projects\Inventario_Lab\Inventario_Lab\Resources\Foto!.png")
        End If
    End Sub

    Private Sub Equipos_Modificar_Click(sender As Object, e As EventArgs) Handles Equipos_Modificar.Click
        Cargar_tabla("Equipos")
        Dim conexion As New MySqlConnection(datasource)
        Dim consulta As New MySqlCommand(“select * from equipos where Id_Equipo='” & reg_bus_equ & “'”, conexion)
        conexion.Open()
        Dim leerbd As MySqlDataReader = consulta.ExecuteReader()
        If leerbd.Read <> False Then
            Encontrado = True
            GUARDAR("equipos", "UPDATE equipos SET Cod_Equipo = '" & UCase(Numero_Equipo.Text) & "', Nombre_Equipo = '" & UCase(Nombre_Equipo.Text) & "', " &
                                "Marca = '" & UCase(Marca_Equipo.Text) & "', Serie = '" & UCase(Serie_Equipo.Text) & "', " &
                                "Activo = '" & Convert.ToInt32(Activo_Equipo.Checked) & "' " &
                                "WHERE Id_Equipo = '" & reg_bus_equ & "'")
        Else
            Encontrado = False
            GUARDAR("equipos", "INSERT INTO equipos (Cod_Equipo, Nombre_Equipo, Marca, Serie, Activo) " &
                            "VALUES ('" & UCase(Numero_Equipo.Text) & "','" & UCase(Nombre_Equipo.Text) & "', '" & UCase(Marca_Equipo.Text) & "'," &
                            " '" & UCase(Serie_Equipo.Text) & "', '" & Convert.ToInt32(Activo_Equipo.Checked) & "')")
        End If
        conexion.Close()
        Recorrer_Equipos()
    End Sub

    Private Sub Equipos_Crear_Click(sender As Object, e As EventArgs) Handles Equipos_Crear.Click
        reg_bus_equ = "nuevo"
        Cargar_tabla("Equipos")
        Equipo_num = Tabla1.Rows.Count
        Numero_Equipo.Text = ""
        Nombre_Equipo.Text = ""
        Marca_Equipo.Text = ""
        Serie_Equipo.Text = ""
        Activo_Equipo.Checked = False
    End Sub

    Private Sub Modificar_Prod_Equ_Click(sender As Object, e As EventArgs) Handles Modificar_Prod_Equ.Click
        Bandera_Rel = 1
        Consulta_rel = "SELECT Cod_Producto, Nombre_Producto,IdRel_Producto_Equipos FROM rel_productos_equipos " &
                        "INNER JOIN productos " &
                        "ON (rel_productos_equipos.Id_Producto = productos.Id_Producto) " &
                        "INNER JOIN equipos " &
                        "ON (rel_productos_equipos.Id_Equipo = equipos.Id_Equipo) " &
                        "WHERE rel_productos_equipos.Id_Equipo='" & reg_bus_equ & "'"
        Id_elem = reg_bus_equ
        Tabla_Rel = "Productos"
        Elemento_rel = Nombre_Equipo.Text
        Form3.ShowDialog()
    End Sub

    Private Sub Proveedores_Consultar_Click(sender As Object, e As EventArgs) Handles Proveedores_Consultar.Click
        Cargar_tabla("Proveedores")
        If z <> Buscar_Prov.Text Then
            cant_reg_encon = 0
        End If
        Dim conexion As New MySqlConnection(datasource)
        conexion.Open()
        Dim consulta As String = "Select * from Proveedores"
        Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
        Dim MysqlDset As New DataSet
        MysqlDadap.Fill(MysqlDset)
        conexion.Close()
        Dim i As Integer = 0
        Dim foundRows() As Data.DataRow
        foundRows = MysqlDset.Tables(0).Select("Nombre_Proveedor Like '" & Buscar_Prov.Text & "%'")
        z = Buscar_Prov.Text
        If cant_reg_encon = 0 And foundRows.Length > 1 Then
            cant_reg_encon = foundRows.Length
            For Each row In Tabla1.Rows
                If foundRows(cant_reg_encon - 1).Item(1) = row(1) Then
                    'MsgBox(foundRows(cant_reg_encon - 1).Item(1))
                    Proveedor_num = i
                    Recorrer_Proveedores()
                    cant_reg_encon = cant_reg_encon - 1
                    GoTo otro
                End If
                i = i + 1
            Next
        Else
            If foundRows.Length = 0 Then
                MsgBox("No se encontro ninguna coincidencia")
            ElseIf cant_reg_encon = 0 Then
                For Each row In Tabla1.Rows
                    If foundRows(cant_reg_encon).Item(1) = row(1) Then
                        Proveedor_num = i
                        Recorrer_Proveedores()
                        GoTo otro
                    End If
                    i = i + 1
                Next
            Else
                For Each row In Tabla1.Rows
                    If foundRows(cant_reg_encon - 1).Item(1) = row(1) Then
                        Proveedor_num = i
                        Recorrer_Proveedores()
                        cant_reg_encon = cant_reg_encon - 1
                        GoTo otro
                    End If
                    i = i + 1
                Next
            End If
        End If
otro:
    End Sub

    Private Sub Siguiente_Proveedor_Click(sender As Object, e As EventArgs) Handles Siguiente_Proveedor.Click
        If Proveedor_num >= (Tabla1.Rows.Count - 1) Then
            Proveedor_num = 0
            Recorrer_Proveedores()
        Else
            Proveedor_num = Proveedor_num + 1
            Recorrer_Proveedores()
        End If
    End Sub

    Private Sub Anterior_Proveedor_Click(sender As Object, e As EventArgs) Handles Anterior_Proveedor.Click
        If Proveedor_num = 0 Then
            Proveedor_num = Tabla1.Rows.Count - 1
            Recorrer_Proveedores()
        Else
            Proveedor_num = Proveedor_num - 1
            Recorrer_Proveedores()
        End If
    End Sub

    Dim Proveedor_num = 0
    Dim reg_bus_prov As String
    Private Sub Recorrer_Proveedores()
        Cargar_tabla("Proveedores")
        Label59.Text = "Proveedor " & (Proveedor_num + 1) & " de " & (Tabla1.Rows.Count)
        'reg_bus_equ = Tabla1.Rows(0).ItemArray(0).ToString
        reg_bus_prov = Tabla1.Rows(Proveedor_num).ItemArray(0).ToString
        Nit_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(0).ToString
        Nombre_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(1).ToString
        Contacto_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(2).ToString
        Direccion_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(3).ToString
        Ciudad_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(4).ToString
        Telefono_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(5).ToString
        Email_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(6).ToString
        Fax_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(7).ToString
        Web_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(8).ToString
        Detalle_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(9).ToString
        Clasificacion_Proveedor.Text = Tabla1.Rows(Proveedor_num).ItemArray(10).ToString
        Aprovado_Proveedor.Checked = Tabla1.Rows(Proveedor_num).ItemArray(11).ToString
        Activo_Proveedor.Checked = Tabla1.Rows(Proveedor_num).ItemArray(13).ToString
    End Sub

    Private Sub Proveedores_Modificar_Click(sender As Object, e As EventArgs) Handles Proveedores_Modificar.Click
        Cargar_tabla("Proveedores")
        Dim conexion As New MySqlConnection(datasource)
        Dim consulta As New MySqlCommand(“select * from proveedores where Nit_Proveedor='” & reg_bus_prov & “'”, conexion)
        conexion.Open()
        Dim leerbd As MySqlDataReader = consulta.ExecuteReader()
        If leerbd.Read <> False Then
            Encontrado = True
            GUARDAR("proveedores", "UPDATE proveedores SET Nit_Proveedor = '" & UCase(Nit_Proveedor.Text) & "', Nombre_Proveedor = '" & UCase(Nombre_Proveedor.Text) & "', " &
                                "Nombre_Contacto = '" & UCase(Contacto_Proveedor.Text) & "', Direccion = '" & UCase(Direccion_Proveedor.Text) & "', " &
                                "Ciudad = '" & UCase(Ciudad_Proveedor.Text) & "', Numero_Telefono = '" & UCase(Telefono_Proveedor.Text) & "', Email_Contacto = '" & UCase(Email_Proveedor.Text) & "', " &
                                "Numero_Fax = '" & UCase(Fax_Proveedor.Text) & "', Pagina_Web = '" & UCase(Web_Proveedor.Text) & "', Detalle = '" & UCase(Detalle_Proveedor.Text) & "', " &
                                "Clasificacion_OIMS = '" & UCase(Clasificacion_Proveedor.Text) & "', Aprovado = '" & Convert.ToInt32(Aprovado_Proveedor.Checked) & "', Activo = '" & Convert.ToInt32(Activo_Proveedor.Checked) & "' " &
                                "WHERE Nit_Proveedor = '" & reg_bus_prov & "'")
        Else
            Encontrado = False
            GUARDAR("proveedores", "INSERT INTO proveedores (Nit_Proveedor, Nombre_Proveedor, Nombre_Contacto, Direccion, Ciudad, Numero_Telefono, Email_Contacto, Numero_Fax, Pagina_Web, Detalle, Clasificacion_OIMS, Aprovado,  Activo) " &
                            "VALUES ('" & UCase(Nit_Proveedor.Text) & "','" & UCase(Nombre_Proveedor.Text) & "', '" & UCase(Contacto_Proveedor.Text) & "', '" & UCase(Direccion_Proveedor.Text) & "'," &
                            " '" & UCase(Ciudad_Proveedor.Text) & "','" & UCase(Telefono_Proveedor.Text) & "','" & UCase(Email_Proveedor.Text) & "', '" & UCase(Fax_Proveedor.Text) & "'," &
                            " '" & UCase(Web_Proveedor.Text) & "','" & UCase(Detalle_Proveedor.Text) & "','" & UCase(Clasificacion_Proveedor.Text) &
                            "', '" & Convert.ToInt32(Aprovado_Proveedor.Checked) & "'," & " '" & Convert.ToInt32(Activo_Proveedor.Checked) & "')")
        End If
        conexion.Close()
        Recorrer_Proveedores()
    End Sub

    Private Sub Proveedores_Crear_Click(sender As Object, e As EventArgs) Handles Proveedores_Crear.Click
        'reg_bus_prov = Tabla1.Rows(Proveedor_num).ItemArray(0).ToString
        Cargar_tabla("Proveedores")
        Proveedor_num = Tabla1.Rows.Count
        reg_bus_prov = "nuevo"
        Nit_Proveedor.Text = ""
        Nombre_Proveedor.Text = ""
        Contacto_Proveedor.Text = ""
        Direccion_Proveedor.Text = ""
        Ciudad_Proveedor.Text = ""
        Telefono_Proveedor.Text = ""
        Email_Proveedor.Text = ""
        Fax_Proveedor.Text = ""
        Web_Proveedor.Text = ""
        Detalle_Proveedor.Text = ""
        Clasificacion_Proveedor.Text = ""
        Aprovado_Proveedor.Checked = False
        Activo_Proveedor.Checked = False
    End Sub

    Private Sub Modificar_Prod_Prov_Click(sender As Object, e As EventArgs) Handles Modificar_Prod_Prov.Click
        Bandera_Rel = 2
        Consulta_rel = "SELECT Cod_Producto, Nombre_Producto,IdRel_Productos_Proveedores FROM rel_productos_proveedores " &
                        "INNER JOIN productos " &
                        "ON (rel_productos_proveedores.Id_Producto = productos.Id_Producto) " &
                        "INNER JOIN proveedores " &
                        "ON (rel_productos_proveedores.Nit_Proveedor= proveedores.Nit_Proveedor) " &
                        "WHERE rel_productos_proveedores.Nit_Proveedor='" & reg_bus_prov & "'"
        Id_elem = reg_bus_prov
        Tabla_Rel = "Productos"
        Elemento_rel = Nombre_Proveedor.Text
        Form3.ShowDialog()
    End Sub

    Dim Producto_num = 0
    Dim stock_producto As Integer
    Private Sub Recorrer_Productos()
        Cargar_tabla("Productos")
        Label72.Text = "Producto " & (Producto_num + 1) & " de " & (Tabla1.Rows.Count)
        reg_bus_produ = Tabla1.Rows(Producto_num).ItemArray(0).ToString
        Codigo_Producto.Text = Tabla1.Rows(Producto_num).ItemArray(1).ToString
        Nombre_Producto.Text = Tabla1.Rows(Producto_num).ItemArray(2).ToString
        Marca_Producto.Text = Tabla1.Rows(Producto_num).ItemArray(5).ToString
        Serie_Producto.Text = Tabla1.Rows(Producto_num).ItemArray(6).ToString
        Activo_Producto.Checked = Tabla1.Rows(Producto_num).ItemArray(9).ToString
        stock_producto = Tabla1.Rows(Producto_num).ItemArray(8).ToString
        Unidades_Producto.Items.Clear()
        With Unidades_Producto
            .Items.Add("Uni")
            .Items.Add("Kg")
            .Items.Add("lb")
            .Items.Add("L")
            .Items.Add("Gal")
        End With
        If Tabla1.Rows(Producto_num).ItemArray(7).ToString <> "" Or IsNothing(Tabla1.Rows(Producto_num).ItemArray(7)) Then
            Foto_Producto.Image = Image.FromFile(Tabla1.Rows(Producto_num).ItemArray(7).ToString)
        Else
            Foto_Producto.Load("C:\Users\Msi\documents\visual studio 2017\Projects\Inventario_Lab\Inventario_Lab\Resources\Foto!.png")
        End If
        Producto(Producto_num)
        Try
            Dim conexion As New MySqlConnection(datasource)
            conexion.Open()
            Dim consulta As String = "Select * from stock where Id_Stock='" & stock_producto & "'"
            Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
            Dim MysqlDset As New DataSet
            MysqlDadap.Fill(MysqlDset)
            conexion.Close()
            '.DataSource = MysqlDset.Tables(0)
            Stock_Minimo.Text = MysqlDset.Tables(0).Rows(0).Item(1).ToString
            Stock_Maximo.Text = MysqlDset.Tables(0).Rows(0).Item(2).ToString
            Stock_Existente.Text = MysqlDset.Tables(0).Rows(0).Item(3).ToString
            Unidades_Producto.SelectedItem = MysqlDset.Tables(0).Rows(0).Item(4).ToString
            Compra_Maxima.Text = MysqlDset.Tables(0).Rows(0).Item(5).ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub Producto(ByVal numero_Producto As Integer)
        With Categoria_Producto
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from categorias"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .DataSource = MysqlDset.Tables(0)
                .DisplayMember = "Nombre_Categoria" 'elnombre de tu columna de tu base de datos q deseas mostrar
                .ValueMember = "Id_Categoria" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                .SelectedValue = Tabla1.Rows(numero_Producto).ItemArray(3)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
        With SubCategoria_Producto
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from categorias_sub where Id_Categoria='" & Tabla1.Rows(numero_Producto).ItemArray(3) & "'"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .DataSource = MysqlDset.Tables(0)
                .DisplayMember = "Nombre_SubCategoria" 'elnombre de tu columna de tu base de datos q deseas mostrar
                .ValueMember = "Id_SubCategoria" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                .SelectedValue = Tabla1.Rows(numero_Producto).ItemArray(4)
                '.Enabled = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
    End Sub

    Private Sub Categoria_Producto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Categoria_Producto.SelectedIndexChanged
        With SubCategoria_Producto
            Try
                Dim conexion As New MySqlConnection(datasource)
                conexion.Open()
                Dim consulta As String = "Select * from categorias_sub where Id_Categoria='" & Categoria_Producto.SelectedValue & "'"
                Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                Dim MysqlDset As New DataSet
                MysqlDadap.Fill(MysqlDset)
                conexion.Close()
                .DataSource = MysqlDset.Tables(0)
                .DisplayMember = "Nombre_SubCategoria" 'elnombre de tu columna de tu base de datos q deseas mostrar
                .ValueMember = "Id_SubCategoria" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                '.SelectedValue = Tabla1.Rows(numero_Producto).ItemArray(4)
                '.Enabled = False
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
            End Try
        End With
    End Sub

    Private Sub Productos_Modificar_Click(sender As Object, e As EventArgs) Handles Productos_Modificar.Click
        Cargar_tabla("Productos")
        Dim conexion As New MySqlConnection(datasource)
        Dim consulta As New MySqlCommand(“select * from productos where Id_Producto='” & reg_bus_produ & “'”, conexion)
        conexion.Open()
        Dim leerbd As MySqlDataReader = consulta.ExecuteReader()
        If Codigo_Producto.Text = "" Or Nombre_Producto.Text = "" Or Marca_Producto.Text = "" Then
            MsgBox("Los campos que tengan (*) son obligatorios")
            GoTo Faltan_Campos
        End If
        If Stock_Existente.Text = "" Then
            Stock_Existente.Text = 0
        End If
        If Stock_Minimo.Text = "" Then
            Stock_Minimo.Text = 0
        End If
        If Stock_Maximo.Text = "" Then
            Stock_Maximo.Text = 0
        End If
        If Compra_Maxima.Text = "" Then
            Compra_Maxima.Text = 0
        End If
        If leerbd.Read <> False Then
            Encontrado = True
            'stock_producto = Tabla1.Rows(Producto_num).ItemArray(8).ToString
            GUARDAR("productos", "UPDATE productos SET Cod_Producto = '" & UCase(Codigo_Producto.Text) & "', Nombre_Producto = '" & UCase(Nombre_Producto.Text) & "', " &
                                "Marca = '" & UCase(Marca_Producto.Text) & "', Serie = '" & UCase(Serie_Producto.Text) & "', " &
                                "Id_Categoria = '" & Categoria_Producto.SelectedValue & "', Id_SubCategoria = '" & SubCategoria_Producto.SelectedValue &
                                "', Activo = '" & Convert.ToInt32(Activo_Producto.Checked) & "' " &
                                "WHERE Id_Producto = '" & reg_bus_produ & "'")
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim cmd As New MySqlCommand("UPDATE stock SET Stock_Minimo = '" & Stock_Minimo.Text & "', Stock_Maximo = '" & Stock_Maximo.Text & "', " &
                                "Stock_Existente = '" & Stock_Existente.Text & "', Unidades = '" & Unidades_Producto.SelectedItem & "', " &
                                "Compra_Maxima = '" & Compra_Maxima.Text & "' " &
                                "WHERE Id_Stock = '" & stock_producto & "'", conn)
                    cmd.ExecuteNonQuery()
                    'MessageBox.Show("Stock MODIFICADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
            End Try
        Else
            Encontrado = False
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim cmd As New MySqlCommand("INSERT INTO stock (Stock_Minimo, Stock_Maximo, Stock_Existente, Unidades, Compra_Maxima) " &
                            "VALUES ('" & Stock_Minimo.Text & "','" & Stock_Maximo.Text & "', '" & Stock_Existente.Text &
                            "', '" & Unidades_Producto.SelectedItem & "', '" & Compra_Maxima.Text & "')", conn)
                    cmd.ExecuteNonQuery()
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Agregarse por: " & vbCrLf & ex.Message)
            End Try
            Cargar_tabla("Stock")
            GUARDAR("productos", "INSERT INTO productos (Cod_Producto, Nombre_Producto, Marca, Serie, Id_Categoria, Id_SubCategoria, Id_Stock, Activo) " &
                            "VALUES ('" & UCase(Codigo_Producto.Text) & "', '" & UCase(Nombre_Producto.Text) & "', '" & UCase(Marca_Producto.Text) &
                            "', '" & UCase(Serie_Producto.Text) & "', '" & Categoria_Producto.SelectedValue & "', '" & SubCategoria_Producto.SelectedValue &
                            "', '" & Tabla1.Rows(Tabla1.Rows.Count - 1).ItemArray(0).ToString & "'," & " '" & Convert.ToInt32(Activo_Producto.Checked) & "')")
        End If
        Proveedor_Producto.PerformClick()
        conexion.Close()
        Recorrer_Productos()
Faltan_Campos:
    End Sub

    Private Sub Productos_Crear_Click(sender As Object, e As EventArgs) Handles Productos_Crear.Click
        reg_bus_produ = "nuevo"
        Cargar_tabla("Productos")
        Producto_num = Tabla1.Rows.Count
        Codigo_Producto.Text = ""
        Nombre_Producto.Text = ""
        Marca_Producto.Text = ""
        Serie_Producto.Text = ""
        Activo_Producto.Checked = False
        Stock_Minimo.Text = ""
        Stock_Maximo.Text = ""
        Stock_Existente.Text = ""
        Compra_Maxima.Text = ""
    End Sub

    Private Sub Anterior_Producto_Click(sender As Object, e As EventArgs) Handles Anterior_Producto.Click
        Cargar_tabla("Productos")
        If Producto_num = 0 Then
            Producto_num = Tabla1.Rows.Count - 1
            Recorrer_Productos()
        Else
            Producto_num = Producto_num - 1
            Recorrer_Productos()
        End If
    End Sub

    Private Sub Siguiente_Productos_Click(sender As Object, e As EventArgs) Handles Siguiente_Productos.Click
        Cargar_tabla("Productos")
        If Producto_num >= (Tabla1.Rows.Count - 1) Then
            Producto_num = 0
            Recorrer_Productos()
        Else
            Producto_num = Producto_num + 1
            Recorrer_Productos()
        End If
    End Sub

    Private Sub Productos_Consultar_Click(sender As Object, e As EventArgs) Handles Productos_Consultar.Click
        Cargar_tabla("Productos")
        If z <> Buscar_Eq.Text Then
            cant_reg_encon = 0
        End If
        Dim conexion As New MySqlConnection(datasource)
        conexion.Open()
        Dim consulta As String = "Select * from Productos"
        Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
        Dim MysqlDset As New DataSet
        MysqlDadap.Fill(MysqlDset)
        conexion.Close()
        Dim i As Integer = 0
        Dim foundRows() As Data.DataRow
        foundRows = MysqlDset.Tables(0).Select("Nombre_Producto Like '" & Buscar_Produ.Text & "%'")
        z = Buscar_Produ.Text
        If cant_reg_encon = 0 And foundRows.Length > 1 Then
            cant_reg_encon = foundRows.Length
            For Each row In Tabla1.Rows
                If foundRows(cant_reg_encon - 1).Item(1) = row(1) Then
                    'MsgBox(foundRows(cant_reg_encon - 1).Item(1))
                    Producto_num = i
                    Recorrer_Productos()
                    cant_reg_encon = cant_reg_encon - 1
                    GoTo otro
                End If
                i = i + 1
            Next
        Else
            If foundRows.Length = 0 Then
                MsgBox("No se encontro ninguna coincidencia")
            ElseIf cant_reg_encon = 0 Then
                For Each row In Tabla1.Rows
                    If foundRows(cant_reg_encon).Item(1) = row(1) Then
                        Producto_num = i
                        Recorrer_Productos()
                        GoTo otro
                    End If
                    i = i + 1
                Next
            Else
                For Each row In Tabla1.Rows
                    If foundRows(cant_reg_encon - 1).Item(1) = row(1) Then
                        Producto_num = i
                        Recorrer_Productos()
                        cant_reg_encon = cant_reg_encon - 1
                        GoTo otro
                    End If
                    i = i + 1
                Next
            End If
        End If
otro:
    End Sub

    Private Sub Equipo_Producto_Click(sender As Object, e As EventArgs) Handles Equipo_Producto.Click
        Bandera_Rel = 1
        Consulta_rel = "SELECT Cod_Equipo, Nombre_Equipo,IdRel_Producto_Equipos FROM rel_productos_equipos " &
                        "INNER JOIN productos " &
                        "ON (rel_productos_equipos.Id_Producto = productos.Id_Producto) " &
                        "INNER JOIN equipos " &
                        "ON (rel_productos_equipos.Id_Equipo = equipos.Id_Equipo) " &
                        "WHERE rel_productos_equipos.Id_Producto='" & reg_bus_produ & "'"
        Id_elem = reg_bus_produ
        Tabla_Rel = "Equipos"
        Elemento_rel = Nombre_Producto.Text
        Form3.ShowDialog()
    End Sub

    Private Sub Proveedor_Producto_Click(sender As Object, e As EventArgs) Handles Proveedor_Producto.Click
        Bandera_Rel = 2
        Consulta_rel = "SELECT Nombre_Proveedor, Ciudad, IdRel_Productos_Proveedores FROM rel_productos_proveedores " &
                        "INNER JOIN productos " &
                        "ON (rel_productos_proveedores.Id_Producto = productos.Id_Producto) " &
                        "INNER JOIN proveedores " &
                        "ON (rel_productos_proveedores.Nit_Proveedor= proveedores.Nit_Proveedor) " &
                        "WHERE rel_productos_proveedores.Id_Producto='" & reg_bus_produ & "'"
        Id_elem = reg_bus_produ
        Tabla_Rel = "Proveedores"
        Form3.ShowDialog()
    End Sub
    Private Sub Ubicacion_Producto_Click(sender As Object, e As EventArgs) Handles Ubicacion_Producto.Click
        Bandera_Rel = 3
        Consulta_rel = "SELECT Estante,Entrepano,Caja_Color,Zona,Cantidad,Aforo,IdRel_Ubicaciones_Productos FROM rel_ubicaciones_productos " &
                        "INNER JOIN productos " &
                        "ON (rel_ubicaciones_productos.Id_Producto = productos.Id_Producto) " &
                        "INNER JOIN ubicaciones " &
                        "ON (rel_ubicaciones_productos.Id_Ubicacion = ubicaciones.Id_Ubicacion) " &
                        "WHERE rel_ubicaciones_productos.Id_Producto='" & reg_bus_produ & "'"
        Id_elem = reg_bus_produ
        Tabla_Rel = "Ubicaciones"
        Elemento_rel = Nombre_Producto.Text
        Form3.ShowDialog()
    End Sub

    Private Sub Movimiento_Ingreso_Click(sender As Object, e As EventArgs) Handles Movimiento_Ingreso.Click
        Label86.Text = "Proveedor *"
        Tipo_Movimiento.Text = "INGRESO"
        Fecha_Movimiento.Text = Today.ToString("yyyy-MM-dd")
        Label84.Visible = True
        Monto_Movimiento.Visible = True
        N_Referencia_Movimiento.Visible = True
        N_Referencia_Movimiento.Enabled = True
        Proveedor_Movimiento.Visible = True
        N_Orden_Movimiento.Enabled = True
        N_Orden_Movimiento.Text = ""
        N_Referencia_Movimiento.Text = ""
        Cargar_tabla("Proveedores")
        With Proveedor_Movimiento
            '.Items.Clear()
            .Text = ""
            .DataSource = Tabla1
            .DisplayMember = "Nombre_Proveedor" 'elnombre de tu columna de tu base de datos q deseas mostrar
            .ValueMember = "Nit_Proveedor"
        End With
        Datos_Movimientos.Visible = True
        Confirmar_Transaccion.Visible = True
    End Sub

    Private Sub Movimiento_Salida_Click(sender As Object, e As EventArgs) Handles Movimiento_Salida.Click
        Dim numAleatorio As New Random()
        Label86.Text = ""
        Tipo_Movimiento.Text = "SALIDA"
        Fecha_Movimiento.Text = Today.ToString("yyyy-MM-dd")
        Monto_Movimiento.Text = ""
        Proveedor_Movimiento.SelectedValue = -1
        N_Referencia_Movimiento.Enabled = False
        Label83.Visible = True
        Label84.Visible = False
        Monto_Movimiento.Visible = False
        N_Orden_Movimiento.Enabled = False
        N_Orden_Movimiento.Text = "S" & numAleatorio.Next
        N_Referencia_Movimiento.Text = N_Orden_Movimiento.Text
        Cargar_tabla("Usuarios")
        Datos_Movimientos.Visible = True
        Confirmar_Transaccion.Visible = True
        Proveedor_Movimiento.Visible = False
    End Sub
    Private Sub dataGridView4_DefaultValuesNeeded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles DataGridView4.DefaultValuesNeeded
        Dim a1 As Integer = DataGridView4.Rows.Count
        Dim a As Integer = a1
        If a1 >= 1 And Tipo_Movimiento.Text = "SALIDA" Then
            For i = 0 To (a - 1)
                'DataGridView4.Item("Responsable", (i)).Value = Usuario_Perfil
            Next
        End If

    End Sub
    Dim Ord_Movimiento_num = 0
    Dim Id_Ord_Movimiento As Integer
    Private Sub Confirmar_Transaccion_Click(sender As Object, e As EventArgs) Handles Confirmar_Transaccion.Click
        Gru_Movimiento.Visible = True
        Producto_Movimiento.SelectedItem = -1
        Usuario_Movimiento.SelectedItem = -1
        If Tipo_Movimiento.Text = "SALIDA" Then
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim cmd As New MySqlCommand("INSERT INTO orden_movimientos (N_Orden_Compra, N_Referencia, Tipo, Fecha, Observaciones)" &
                            "VALUES ('" & UCase(N_Orden_Movimiento.Text) & "', '" & UCase(N_Referencia_Movimiento.Text) & "', '" & UCase(Tipo_Movimiento.Text) &
                            "', '" & UCase(Fecha_Movimiento.Text) & "', '" & UCase(Observaciones_Movimiento.Text) & "')", conn)
                    cmd.ExecuteNonQuery()
                    'MessageBox.Show("Registro MODIFICADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
            End Try
            Precio_Movimiento.Visible = False
            Usuario_Movimiento.Visible = True
            Label95.Text = "Usuario"
            With Producto_Movimiento
                Try
                    Dim conexion As New MySqlConnection(datasource)
                    conexion.Open()
                    Dim consulta As String = "Select * from productos"
                    Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                    Dim MysqlDset As New DataSet
                    MysqlDadap.Fill(MysqlDset)
                    conexion.Close()
                    .DataSource = MysqlDset.Tables(0)
                    .DisplayMember = "Nombre_Producto" 'elnombre de tu columna de tu base de datos q deseas mostrar
                    .ValueMember = "Id_Producto" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                    '.Enabled = False
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            End With
            With Usuario_Movimiento
                Try
                    Dim conexion As New MySqlConnection(datasource)
                    conexion.Open()
                    Dim consulta As String = "Select * from usuarios"
                    Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                    Dim MysqlDset As New DataSet
                    MysqlDadap.Fill(MysqlDset)
                    conexion.Close()
                    .DataSource = MysqlDset.Tables(0)
                    .DisplayMember = "Nombre_Usuario" 'elnombre de tu columna de tu base de datos q deseas mostrar
                    .ValueMember = "Id_Usuario" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                    '.Enabled = False
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            End With
        Else
            If N_Orden_Movimiento.Text = "" Or N_Referencia_Movimiento.Text = "" Or Monto_Movimiento.Text = "" Then
                MsgBox("Algunos campos son obligatorios")
                GoTo nada
            End If
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim cmd As New MySqlCommand("INSERT INTO orden_movimientos (N_Orden_Compra, N_Referencia, Tipo, Fecha, Observaciones, Monto, Nit_Proveedor)" &
                            "VALUES ('" & UCase(N_Orden_Movimiento.Text) & "', '" & UCase(N_Referencia_Movimiento.Text) & "', '" & UCase(Tipo_Movimiento.Text) &
                            "', '" & UCase(Fecha_Movimiento.Text) & "', '" & UCase(Observaciones_Movimiento.Text) &
                            "', '" & UCase(Monto_Movimiento.Text) & "', '" & UCase(Proveedor_Movimiento.SelectedValue) & "')", conn)
                    cmd.ExecuteNonQuery()
                    'MessageBox.Show("Registro MODIFICADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
            End Try
            Precio_Movimiento.Visible = True
            Usuario_Movimiento.Visible = False
            Label95.Text = "Precio"
            Using conexion As New MySqlConnection(datasource)
                Dim Comando As New MySqlCommand("SELECT * FROM rel_productos_proveedores " &
                        "INNER JOIN productos " &
                        "ON (rel_productos_proveedores.Id_Producto = productos.Id_Producto) " &
                        "INNER JOIN proveedores " &
                        "ON (rel_productos_proveedores.Nit_Proveedor= proveedores.Nit_Proveedor) " &
                        "WHERE rel_productos_proveedores.Nit_Proveedor='" & Proveedor_Movimiento.SelectedValue & "'", conexion)
                Dim Adaptador As New MySqlDataAdapter(Comando)
                Dim Tabla As New DataTable
                Try
                    Adaptador.Fill(Tabla)
                Catch ex As Exception
                Finally
                    If conexion.State = ConnectionState.Open Then
                        conexion.Close()
                    End If
                End Try
                Tabla1 = Tabla
            End Using
            With Producto_Movimiento
                .DataSource = Tabla1
                .DisplayMember = "Nombre_Producto" 'elnombre de tu columna de tu base de datos q deseas mostrar
                .ValueMember = "Id_Producto" 'el ide de tu tabla relacionada con el nombre que muestras muy importante para saber el ide de quien seleccionas en tu combobox
                '.Enabled = False
            End With
        End If
        Movimiento_Ingreso.Enabled = False
        Movimiento_Salida.Enabled = False
        Generar_Movimientos.Visible = True
        Confirmar_Transaccion.Visible = False
        N_Orden_Movimiento.Enabled = False
        N_Referencia_Movimiento.Enabled = False
        Monto_Movimiento.Enabled = False
        Observaciones_Movimiento.Enabled = False
        Proveedor_Movimiento.Enabled = False
        Cargar_tabla("orden_movimientos")
        Ord_Movimiento_num = Tabla1.Rows.Count - 1
        Id_Ord_Movimiento = Tabla1.Rows(Ord_Movimiento_num).ItemArray(0).ToString
        N_Orden_Movimiento.Text = Tabla1.Rows(Ord_Movimiento_num).ItemArray(2).ToString
        N_Referencia_Movimiento.Text = Tabla1.Rows(Ord_Movimiento_num).ItemArray(3).ToString
        Observaciones_Movimiento.Text = Tabla1.Rows(Ord_Movimiento_num).ItemArray(5).ToString
        Monto_Movimiento.Text = Tabla1.Rows(Ord_Movimiento_num).ItemArray(6).ToString
        Proveedor_Movimiento.SelectedValue = Tabla1.Rows(Ord_Movimiento_num).ItemArray(7).ToString
nada:
    End Sub
    Dim ruta_fotos As String = "C:\Users\Msi\Desktop\Imagenes_Prueba\"
    Private Sub Foto_Usuario_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Foto_Usuario.DragEnter
        'DataFormats.FileDrop nos devuelve el array de rutas de archivos
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'Los archivos son externos a nuestra aplicación por lo que de indicaremos All ya que dará lo mismo.
            e.Effect = DragDropEffects.All
        End If
    End Sub
    Private Sub Foto_Usuario_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Foto_Usuario.DragDrop
        Dim result As Integer = MessageBox.Show("Esta seguro que desea CAMBIAR la foto", "Alerta", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")

        ElseIf result = DialogResult.Yes Then
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim strRutaArchivoImagen As String
                'Asignamos la primera posición del array de ruta de archivos a la variable de tipo string
                'declarada anteriormente ya que en este caso sólo mostraremos una imagen en el control.
                strRutaArchivoImagen = e.Data.GetData(DataFormats.FileDrop)(0)
                Try
                    Foto_Usuario.Image.Dispose()
                    Foto_Usuario.Image = Nothing
                    Application.DoEvents()
                    If System.IO.File.Exists(ruta_fotos & "U_" & Usuario_Nickname.Text & "_" & reg_bus & ".png") = True Then
                        System.IO.File.Delete(ruta_fotos & "U_" & Usuario_Nickname.Text & "_" & reg_bus & ".png")
                    End If

                Catch oe As Exception
                    MsgBox(oe.Message, MsgBoxStyle.Critical)
                End Try
                Try
                    Dim imagen As New Bitmap(New Bitmap(strRutaArchivoImagen), 239, 203)
                    imagen.Save(ruta_fotos & "U_" & Usuario_Nickname.Text & "_" & reg_bus & ".png", System.Drawing.Imaging.ImageFormat.Png)
                    Dim foto As String = ruta_fotos & "U_" & Usuario_Nickname.Text & "_" & reg_bus & ".png"
                    foto = Replace(foto, "\", "/")
                    Try
                        Using conn As New MySqlConnection(datasource)
                            conn.Open()
                            Dim cmd As New MySqlCommand("UPDATE usuarios SET Foto = '" & foto & "' " &
                                        "WHERE Id_Usuario = '" & reg_bus & "'", conn)
                            cmd.ExecuteNonQuery()
                            'MessageBox.Show("Registro MODIFICADO")
                            conn.Close()
                        End Using
                    Catch ex As Exception
                        MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
                    End Try
                Catch exc As Exception
                    MessageBox.Show(exc, " Atención !", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                'File.Copy(strRutaArchivoImagen, ruta_foto_usuario & Usuario_Nickname.Text & "_" & reg_bus & ".png")
                'La cargamos al control
                Foto_Usuario.Image = Image.FromFile(ruta_fotos & "U_" & Usuario_Nickname.Text & "_" & reg_bus & ".png")
            End If
        End If
    End Sub
    Private Sub Foto_Equipo_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Foto_Equipo.DragEnter
        'DataFormats.FileDrop nos devuelve el array de rutas de archivos
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'Los archivos son externos a nuestra aplicación por lo que de indicaremos All ya que dará lo mismo.
            e.Effect = DragDropEffects.All
        End If
    End Sub
    Private Sub Foto_Equipo_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Foto_Equipo.DragDrop
        Dim result As Integer = MessageBox.Show("Esta seguro que desea CAMBIAR la foto", "Alerta", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")

        ElseIf result = DialogResult.Yes Then
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim strRutaArchivoImagen As String
                'Asignamos la primera posición del array de ruta de archivos a la variable de tipo string
                'declarada anteriormente ya que en este caso sólo mostraremos una imagen en el control.
                strRutaArchivoImagen = e.Data.GetData(DataFormats.FileDrop)(0)
                Try
                    Foto_Equipo.Image.Dispose()
                    Foto_Equipo.Image = Nothing
                    Application.DoEvents()
                    If System.IO.File.Exists(ruta_fotos & "E_" & Numero_Equipo.Text & "_" & reg_bus_equ & ".png") = True Then
                        System.IO.File.Delete(ruta_fotos & "E_" & Numero_Equipo.Text & "_" & reg_bus_equ & ".png")
                    End If

                Catch oe As Exception
                    MsgBox(oe.Message, MsgBoxStyle.Critical)
                End Try
                Try
                    Dim imagen As New Bitmap(New Bitmap(strRutaArchivoImagen), 239, 203)
                    imagen.Save(ruta_fotos & "E_" & Numero_Equipo.Text & "_" & reg_bus_equ & ".png", System.Drawing.Imaging.ImageFormat.Png)
                    Dim foto As String = ruta_fotos & "E_" & Numero_Equipo.Text & "_" & reg_bus_equ & ".png"
                    foto = Replace(foto, "\", "/")
                    Try
                        Using conn As New MySqlConnection(datasource)
                            conn.Open()
                            Dim cmd As New MySqlCommand("UPDATE equipos SET Foto = '" & foto & "' " &
                                        "WHERE Id_Equipo = '" & reg_bus_equ & "'", conn)
                            cmd.ExecuteNonQuery()
                            'MessageBox.Show("Registro MODIFICADO")
                            conn.Close()
                        End Using
                    Catch ex As Exception
                        MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
                    End Try
                Catch exc As Exception
                    MessageBox.Show(exc, " Atención !", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                'File.Copy(strRutaArchivoImagen, ruta_foto_usuario & Usuario_Nickname.Text & "_" & reg_bus & ".png")
                'La cargamos al control
                Foto_Equipo.Image = Image.FromFile(ruta_fotos & "E_" & Numero_Equipo.Text & "_" & reg_bus_equ & ".png")
            End If
        End If
    End Sub
    Private Sub Foto_Producto_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Foto_Producto.DragEnter
        'DataFormats.FileDrop nos devuelve el array de rutas de archivos
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'Los archivos son externos a nuestra aplicación por lo que de indicaremos All ya que dará lo mismo.
            e.Effect = DragDropEffects.All
        End If
    End Sub
    Private Sub Foto_Producto_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Foto_Producto.DragDrop
        Dim result As Integer = MessageBox.Show("Esta seguro que desea CAMBIAR la foto", "Alerta", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")

        ElseIf result = DialogResult.Yes Then
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim strRutaArchivoImagen As String
                'Asignamos la primera posición del array de ruta de archivos a la variable de tipo string
                'declarada anteriormente ya que en este caso sólo mostraremos una imagen en el control.
                strRutaArchivoImagen = e.Data.GetData(DataFormats.FileDrop)(0)
                Try
                    Foto_Producto.Image.Dispose()
                    Foto_Producto.Image = Nothing
                    Application.DoEvents()
                    If System.IO.File.Exists(ruta_fotos & "P_" & Codigo_Producto.Text & "_" & reg_bus_produ & ".png") = True Then
                        System.IO.File.Delete(ruta_fotos & "P_" & Codigo_Producto.Text & "_" & reg_bus_produ & ".png")
                    End If

                Catch oe As Exception
                    MsgBox(oe.Message, MsgBoxStyle.Critical)
                End Try
                Try
                    Dim imagen As New Bitmap(New Bitmap(strRutaArchivoImagen), 239, 203)
                    imagen.Save(ruta_fotos & "P_" & Codigo_Producto.Text & "_" & reg_bus_produ & ".png", System.Drawing.Imaging.ImageFormat.Png)
                    Dim foto As String = ruta_fotos & "P_" & Codigo_Producto.Text & "_" & reg_bus_produ & ".png"
                    foto = Replace(foto, "\", "/")
                    Try
                        Using conn As New MySqlConnection(datasource)
                            conn.Open()
                            Dim cmd As New MySqlCommand("UPDATE productos SET Foto = '" & foto & "' " &
                                        "WHERE Id_Producto = '" & reg_bus_produ & "'", conn)
                            cmd.ExecuteNonQuery()
                            'MessageBox.Show("Registro MODIFICADO")
                            conn.Close()
                        End Using
                    Catch ex As Exception
                        MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
                    End Try
                Catch exc As Exception
                    MessageBox.Show(exc, " Atención !", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
                'File.Copy(strRutaArchivoImagen, ruta_foto_usuario & Usuario_Nickname.Text & "_" & reg_bus & ".png")
                'La cargamos al control
                Foto_Producto.Image = Image.FromFile(ruta_fotos & "P_" & Codigo_Producto.Text & "_" & reg_bus_produ & ".png")
            End If
        End If
    End Sub

    Private Sub Generar_Movimientos_Click(sender As Object, e As EventArgs) Handles Generar_Movimientos.Click
        DataGridView4.Visible = True
        Dim consulta_movimientos As String = ""
        Dim stock As Integer
        If IsNumeric(Cantidad_Movimiento.Text) Then
            Cantidad_a_mover = Cantidad_Movimiento.Text
            Tipo_Movi = Tipo_Movimiento.Text
            Using conexion As New MySqlConnection(datasource)
                Dim Comando As New MySqlCommand("SELECT * FROM stock " &
                        "INNER JOIN productos " &
                        "ON (stock.Id_Stock = productos.Id_Stock) " &
                        "WHERE Id_Producto='" & Producto_Movimiento.SelectedValue & "'", conexion)
                Dim Adaptador As New MySqlDataAdapter(Comando)
                Dim Tabla As New DataTable
                Try
                    Adaptador.Fill(Tabla)
                Catch ex As Exception
                Finally
                    If conexion.State = ConnectionState.Open Then
                        conexion.Close()
                    End If
                End Try
                Tabla1 = Tabla
            End Using
            'DataGridView4.DataSource = Tabla1
            id_stock = Tabla1.Rows(0).ItemArray(0).ToString
            If Tipo_Movimiento.Text = "SALIDA" Then
                If Descripcion_Movimiento.Text = "" Or Producto_Movimiento.Text = "" Or Usuario_Movimiento.Text = "" Then
                    MsgBox("Faltan algunos datos para generar el movimiento")
                    GoTo err
                End If
                If Convert.ToInt16(Tabla1.Rows(0).ItemArray(3).ToString) < Convert.ToInt16(Cantidad_Movimiento.Text) Then
                    MsgBox("Atencion NO puede realizar el movimiento, porque no posee esa cantidad en su inventario")
                    GoTo err
                End If
                stock = Convert.ToInt16(Tabla1.Rows(0).ItemArray(3).ToString) - Convert.ToInt16(Cantidad_Movimiento.Text)
            Else
                If Descripcion_Movimiento.Text = "" Or Producto_Movimiento.Text = "" Or Precio_Movimiento.Text = "" Then
                    MsgBox("Faltan algunos datos para generar el movimiento")
                    GoTo err
                End If
                If Tabla1.Rows(0).ItemArray(1).ToString <> 0 Then
                    If Convert.ToInt16(Tabla1.Rows(0).ItemArray(2).ToString) < Convert.ToInt16(Cantidad_Movimiento.Text) Then
                        MsgBox("Atencion NO puede realizar el movimiento, porque su movimiento supera el máximo permitido")
                        GoTo err
                    End If
                    stock = Convert.ToInt16(Tabla1.Rows(0).ItemArray(3).ToString) + Convert.ToInt16(Cantidad_Movimiento.Text)
                ElseIf Tabla1.Rows(0).ItemArray(1).ToString = 0 Then
                    stock = Convert.ToInt16(Tabla1.Rows(0).ItemArray(3).ToString) + Convert.ToInt16(Cantidad_Movimiento.Text)
                End If
            End If
            Using conexion As New MySqlConnection(datasource)
                Dim Comando As New MySqlCommand("SELECT Estante,Entrepano,Caja_Color,Zona,Cantidad,Aforo FROM rel_ubicaciones_productos " &
                            "INNER JOIN productos " &
                            "ON (rel_ubicaciones_productos.Id_Producto = productos.Id_Producto) " &
                            "INNER JOIN ubicaciones " &
                            "ON (rel_ubicaciones_productos.Id_Ubicacion = ubicaciones.Id_Ubicacion) " &
                            "WHERE rel_ubicaciones_productos.Id_Producto='" & Producto_Movimiento.SelectedValue & "'", conexion)
                Dim Adaptador As New MySqlDataAdapter(Comando)
                Dim Tabla As New DataTable
                Try
                    Adaptador.Fill(Tabla)
                Catch ex As Exception
                Finally
                    If conexion.State = ConnectionState.Open Then
                        conexion.Close()
                    End If
                End Try
                Tabla1 = Tabla
            End Using
            If Tabla1.Rows.Count >= 1 Then
                Bandera_Rel = 4
                Consulta_rel = "SELECT Estante,Entrepano,Caja_Color,Zona,Cantidad,Aforo,IdRel_Ubicaciones_Productos FROM rel_ubicaciones_productos " &
                "INNER JOIN productos " &
                "ON (rel_ubicaciones_productos.Id_Producto = productos.Id_Producto) " &
                "INNER JOIN ubicaciones " &
                "ON (rel_ubicaciones_productos.Id_Ubicacion = ubicaciones.Id_Ubicacion) " &
                "WHERE rel_ubicaciones_productos.Id_Producto='" & Producto_Movimiento.SelectedValue & "'"
                Id_elem = Producto_Movimiento.SelectedValue
                'Tabla_Rel = "Ubicaciones"
                'Elemento_rel = Nombre_Producto.Text
                Try
                    Using conn As New MySqlConnection(datasource)
                        conn.Open()
                        Dim cmd As New MySqlCommand("UPDATE stock SET Stock_Existente = '" & stock &
                        "' WHERE Id_Stock = '" & id_stock & "'", conn)
                        cmd.ExecuteNonQuery()
                        'MessageBox.Show("Registro MODIFICADO")
                        conn.Close()
                    End Using
                Catch ex As Exception
                    MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
                End Try
                Nueva_Transaccion.Visible = True
                If Tipo_Movimiento.Text = "SALIDA" Then
                    Try
                        Using conn As New MySqlConnection(datasource)
                            conn.Open()
                            Dim cmd As New MySqlCommand("INSERT INTO movimientos (IdOrden_Movimiento, Id_Producto, Cantidad, Id_Usuario, Fecha, Descripcion, Id_Usuario_Final)" &
                                "VALUES ('" & UCase(Id_Ord_Movimiento) & "', '" & Producto_Movimiento.SelectedValue & "', '" & Cantidad_Movimiento.Text &
                                "', '" & id_Usuar_Per & "', '" & Fecha_Movimiento.Text & "', '" & UCase(Descripcion_Movimiento.Text) & "', '" & Usuario_Movimiento.SelectedValue & "')", conn)
                            cmd.ExecuteNonQuery()
                            'MessageBox.Show("Registro MODIFICADO")
                            conn.Close()
                        End Using
                    Catch ex As Exception
                        MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
                    End Try
                Else
                    Try
                        Using conn As New MySqlConnection(datasource)
                            conn.Open()
                            Dim cmd As New MySqlCommand("INSERT INTO movimientos (IdOrden_Movimiento, Id_Producto, Cantidad, Precio_Compra, Id_Usuario, Fecha, Descripcion)" &
                                "VALUES ('" & UCase(Id_Ord_Movimiento) & "', '" & Producto_Movimiento.SelectedValue & "', '" & Cantidad_Movimiento.Text &
                                "', '" & Precio_Movimiento.Text & "', '" & id_Usuar_Per & "', '" & Fecha_Movimiento.Text & "', '" & UCase(Descripcion_Movimiento.Text) & "')", conn)
                            cmd.ExecuteNonQuery()
                            'MessageBox.Show("Registro MODIFICADO")
                            conn.Close()
                        End Using
                    Catch ex As Exception
                        MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
                    End Try
                End If

                If Tipo_Movimiento.Text = "SALIDA" Then
                    consulta_movimientos = "SELECT Nombre_Producto,Cantidad,Descripcion,Nombre_Usuario FROM movimientos " & 'Nombre_Producto,Cantidad,Precio_Compra,Descripcion,Nombre_Usuario
                        "INNER JOIN productos " &
                        "ON (movimientos.Id_Producto = productos.Id_Producto) " &
                        "INNER JOIN usuarios " &
                        "ON (movimientos.Id_Usuario_Final = usuarios.Id_Usuario) " &
                        "WHERE IdOrden_Movimiento='" & Id_Ord_Movimiento & "'"
                Else
                    consulta_movimientos = "SELECT Nombre_Producto,Cantidad,Precio_Compra,Descripcion FROM movimientos " & 'Nombre_Producto,Cantidad,Precio_Compra,Descripcion,Nombre_Usuario
                                "INNER JOIN productos " &
                                "ON (movimientos.Id_Producto = productos.Id_Producto) " &
                                "WHERE IdOrden_Movimiento='" & Id_Ord_Movimiento & "'"
                End If
                Using conexion As New MySqlConnection(datasource)
                    Dim Comando As New MySqlCommand(consulta_movimientos, conexion)
                    Dim Adaptador As New MySqlDataAdapter(Comando)
                    Dim Tabla As New DataTable
                    Try
                        Adaptador.Fill(Tabla)
                    Catch ex As Exception
                    Finally
                        If conexion.State = ConnectionState.Open Then
                            conexion.Close()
                        End If
                    End Try
                    DataGridView4.DataSource = Tabla
                End Using
                Descripcion_Movimiento.Text = ""
                Precio_Movimiento.Text = ""
                Cantidad_Movimiento.Text = ""
                Form3.ShowDialog()
            Else
                MsgBox("Atencion NO puede realizar el movimiento, porque el producto no cuenta con ubicaciones")
                GoTo err
            End If
        Else
            MsgBox("El campo de cantidad solo acepta valores numéricos")
        End If
err:
    End Sub

    Private Sub Nueva_Transaccion_Click(sender As Object, e As EventArgs) Handles Nueva_Transaccion.Click
        DataGridView4.Visible = False
        DataGridView4.DataSource = Nothing
        Gru_Movimiento.Visible = False
        Movimiento_Ingreso.Enabled = True
        Movimiento_Salida.Enabled = True
        Generar_Movimientos.Visible = False
        Confirmar_Transaccion.Visible = True
        N_Orden_Movimiento.Enabled = True
        N_Referencia_Movimiento.Enabled = True
        Monto_Movimiento.Enabled = True
        Observaciones_Movimiento.Enabled = True
        Proveedor_Movimiento.Enabled = True
        N_Orden_Movimiento.Text = ""
        N_Referencia_Movimiento.Text = ""
        Monto_Movimiento.Text = ""
        Observaciones_Movimiento.Text = ""
        Proveedor_Movimiento.SelectedItem = -1
        Nueva_Transaccion.Visible = False
    End Sub

End Class
