﻿Imports MySql.Data.MySqlClient
Public Class Form3

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TabControl1.Visible = True
        Cargar()
    End Sub

    Private Sub Consulta(ByVal Querry As String)
        Using conexion As New MySqlConnection(datasource)
            Dim Comando As New MySqlCommand(Querry, conexion)
            Dim Adaptador As New MySqlDataAdapter(Comando)
            Dim Tabla As New DataTable
            Try
                Adaptador.Fill(Tabla)
                Select Case Bandera_Rel
                    Case 1
                        DataGridView1.DataSource = Tabla
                        DataGridView1.ReadOnly = True
                        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                        DataGridView1.AutoResizeColumns()
                        DataGridView1.Columns(2).Visible = False
                    Case 2
                        DataGridView2.DataSource = Tabla
                        DataGridView2.ReadOnly = True
                        DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                        DataGridView2.AutoResizeColumns()
                        DataGridView2.Columns(2).Visible = False
                    Case 3
                        DataGridView3.DataSource = Tabla
                        DataGridView3.ReadOnly = True
                        DataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                        DataGridView3.AutoResizeColumns()
                    Case 4
                        DataGridView5.DataSource = Tabla
                        DataGridView5.ReadOnly = True
                        DataGridView5.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                        DataGridView5.AutoResizeColumns()
                End Select

            Catch ex As Exception
            Finally
                If conexion.State = ConnectionState.Open Then
                    conexion.Close()
                End If
            End Try
        End Using
    End Sub

    Dim Id1 As String
    Dim Id2 As String
    Private Sub Cargar()
        Id1 = ""
        Id2 = ""
        For i = 1 To 4
            If Me.Controls.Find("TabPage" & i, True).Count = 1 Then
                Dim b As TabPage = Me.Controls.Find("TabPage" & i, True)(0)
                b.Parent = Nothing
            End If
        Next
        Select Case Bandera_Rel
            Case 1
                TabPage1.Parent = TabControl1
                Consulta(Consulta_rel)
                Agregar_Equ_Prod.Text = "Agregar " & Tabla_Rel
                Eliminar_Equ_Prod.Text = "Eliminar " & Tabla_Rel
                With Combo_Equ_Prod
                    Try
                        Dim conexion As New MySqlConnection(datasource)
                        conexion.Open()
                        Dim consulta As String = "Select * from " & Tabla_Rel & " Where Activo='1'"
                        Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                        Dim MysqlDset As New DataSet
                        MysqlDadap.Fill(MysqlDset)
                        conexion.Close()
                        .DataSource = MysqlDset.Tables(0)
                        If Tabla_Rel = "Equipos" Then
                            Label1.Text = "Gestor para agregar Equipos al Producto: " & Elemento_rel
                            Id1 = Id_elem
                            .DisplayMember = "Nombre_Equipo"
                            .ValueMember = "Id_Equipo"
                        ElseIf Tabla_Rel = "Productos" Then
                            Label1.Text = "Gestor para agregar Productos al Equipo: " & Elemento_rel
                            Id2 = Id_elem
                            .DisplayMember = "Nombre_Producto"
                            .ValueMember = "Id_Producto"
                        End If
                        .SelectedValue = -1
                        '.Enabled = False
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try
                End With
            Case 2
                TabPage2.Parent = TabControl1
                Consulta(Consulta_rel)
                Agregar_Prov_Prod.Text = "Agregar " & Tabla_Rel
                Eliminar_Prov_Prod.Text = "Eliminar " & Tabla_Rel
                With Combo_Prov_Prod
                    Try
                        Dim conexion As New MySqlConnection(datasource)
                        conexion.Open()
                        Dim consulta As String = "Select * from " & Tabla_Rel & " Where Activo='1'"
                        Dim MysqlDadap As New MySqlDataAdapter(consulta, conexion)
                        Dim MysqlDset As New DataSet
                        MysqlDadap.Fill(MysqlDset)
                        conexion.Close()
                        .DataSource = MysqlDset.Tables(0)
                        If Tabla_Rel = "Proveedores" Then
                            Id1 = Id_elem
                            Label2.Text = "Gestor para agregar Proveedores al Producto: " & Elemento_rel
                            .DisplayMember = "Nombre_Proveedor"
                            .ValueMember = "Nit_Proveedor"
                        ElseIf Tabla_Rel = "Productos" Then
                            Id2 = Id_elem
                            Label2.Text = "Gestor para agregar Productos al Proveedor: " & Elemento_rel
                            .DisplayMember = "Nombre_Producto"
                            .ValueMember = "Id_Producto"
                        End If
                        .SelectedValue = -1
                        '.Enabled = False
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try
                End With
            Case 3
                Id1 = Id_elem
                TabPage3.Parent = TabControl1
                Consulta(Consulta_rel)
                Label3.Text = "Ubicaciones asignadas al producto: " & Elemento_rel
                Agregar_Ubicacion.Text = "Agregar " & Tabla_Rel
                Eliminar_Ubicacion.Text = "Eliminar " & Tabla_Rel
                Using conexion1 As New MySqlConnection(datasource)
                    Dim Comando1 As New MySqlCommand("select * from ubicaciones", conexion1)
                    Dim Adaptador1 As New MySqlDataAdapter(Comando1)
                    Dim Tabla1 As New DataTable
                    DataGridView3.Columns(6).Visible = False
                    Adaptador1.Fill(Tabla1)
                    DataGridView4.DataSource = Tabla1
                    DataGridView4.ReadOnly = True
                    DataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                    DataGridView4.AutoResizeColumns()
                    DataGridView4.Columns(0).Visible = False
                    conexion1.Close()
                End Using
            Case 4
                If Cantidad_a_mover = 0 Then
                    Label7.ForeColor = Color.Green
                    Cantidad_Mov_Reg.Enabled = False
                Else
                    Label7.ForeColor = Color.Red
                    Cantidad_Mov_Reg.Enabled = True
                End If
                TabPage4.Parent = TabControl1
                Consulta(Consulta_rel)
                Label7.Text = "Pendientes: " & Cantidad_a_mover
                DataGridView5.Columns("IdRel_Ubicaciones_Productos").Visible = False
        End Select
    End Sub
    Private Sub Agregar_Equ_Prod_Click(sender As Object, e As EventArgs) Handles Agregar_Equ_Prod.Click
        If Combo_Equ_Prod.SelectedValue = Nothing Then
            GoTo nada
        End If
        If DataGridView1.RowCount <= 1 Then
            GoTo sin_relaciones
        End If
        For Each row As DataGridViewRow In DataGridView1.Rows
            For Each cell As DataGridViewCell In row.Cells
                If cell.Value Is Nothing Then
                Else
                    If cell.Value.ToString() = Combo_Equ_Prod.Text Then
                        row.Selected = True
                        GoTo nada
                    End If
                End If
            Next
        Next
sin_relaciones:
        If Id1 <> "" Then
            Id2 = Combo_Equ_Prod.SelectedValue
        Else
            Id1 = Combo_Equ_Prod.SelectedValue
        End If
        Try
            Using conn As New MySqlConnection(datasource)
                conn.Open()
                Dim cmd As New MySqlCommand("INSERT INTO rel_productos_equipos (Id_Producto, Id_equipo)" &
                            "VALUES ('" & Id1 & "', '" & Id2 & "')", conn)
                cmd.ExecuteNonQuery()
                'MessageBox.Show("Registro MODIFICADO")
                conn.Close()
            End Using
        Catch ex As Exception
            MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
        End Try
        Cargar()
nada:
    End Sub


    Private Sub Agregar_Prov_Prod_Click(sender As Object, e As EventArgs) Handles Agregar_Prov_Prod.Click
        If Combo_Prov_Prod.SelectedValue = Nothing Then
            GoTo nada
        End If
        If DataGridView2.RowCount <= 1 Then
            GoTo sin_relaciones
        End If
        For Each row As DataGridViewRow In DataGridView2.Rows
            For Each cell As DataGridViewCell In row.Cells
                If cell.Value Is Nothing Then
                Else
                    If cell.Value.ToString() = Combo_Prov_Prod.Text Then
                        'row.Selected = True
                        GoTo nada
                    End If
                End If
            Next
        Next
sin_relaciones:
        If Id1 <> "" Then
            Id2 = Combo_Prov_Prod.SelectedValue
        Else
            Id1 = Combo_Prov_Prod.SelectedValue
        End If
        Try
            Using conn As New MySqlConnection(datasource)
                conn.Open()
                Dim cmd As New MySqlCommand("INSERT INTO rel_productos_proveedores (Id_Producto, Nit_Proveedor)" &
                            "VALUES ('" & Id1 & "', '" & Id2 & "')", conn)
                cmd.ExecuteNonQuery()
                'MessageBox.Show("Registro MODIFICADO")
                conn.Close()
            End Using
        Catch ex As Exception
            MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
        End Try
        Cargar()
nada:
    End Sub

    Private Sub Agregar_Ubicacion_Click(sender As Object, e As EventArgs) Handles Agregar_Ubicacion.Click

        Dim id_ubicacion As Integer = DataGridView4.Item(0, DataGridView4.CurrentRow.Index).Value
        Dim a As Integer = DataGridView4.CurrentRow.Index
        For Each row As DataGridViewRow In DataGridView3.Rows
            If row.Cells(0) Is Nothing Then
            Else
                If row.Cells(0).Value = DataGridView4.Item(1, a).Value And row.Cells(1).Value = DataGridView4.Item(2, a).Value And row.Cells(2).Value = DataGridView4.Item(3, a).Value And row.Cells(3).Value = DataGridView4.Item(4, a).Value Then
                    MsgBox("La ubicación que desea agregar ya existe")
                    GoTo nada
                End If
            End If
        Next
        If Aforo_Ubicacion.Text = "" Then
            Aforo_Ubicacion.Text = "N/A"
        End If
        Try
            Using conn As New MySqlConnection(datasource)
                conn.Open()
                Dim cmd As New MySqlCommand("INSERT INTO rel_ubicaciones_productos (Id_Producto, Id_Ubicacion, Cantidad, Aforo)" &
                            "VALUES ('" & Id1 & "', '" & id_ubicacion & "', '0', '" & Aforo_Ubicacion.Text & "')", conn)
                cmd.ExecuteNonQuery()
                'MessageBox.Show("Registro MODIFICADO")
                conn.Close()
            End Using
        Catch ex As Exception
            MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
        End Try
        Cargar()
nada:
    End Sub

    Private Sub Eliminar_Ubicacion_Click(sender As Object, e As EventArgs) Handles Eliminar_Ubicacion.Click
        Dim result As Integer = MessageBox.Show("Esta seguro que desea ELIMINAR el registro", "Alerta", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")

        ElseIf result = DialogResult.Yes Then
            'MessageBox.Show("Yes pressed")
            Dim a As Integer = DataGridView3.CurrentRow.Index
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim query As String = "DELETE FROM rel_ubicaciones_productos WHERE " & DataGridView3.Columns(6).Name & " = '" & DataGridView3.Item(6, a).Value & "'"
                    Dim cmd As New MySqlCommand(query, conn)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Registro ELIMINADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Eliminarse por: " & vbCrLf & ex.Message)
            End Try
        End If
        Cargar()
    End Sub

    Private Sub Eliminar_Equ_Prod_Click(sender As Object, e As EventArgs) Handles Eliminar_Equ_Prod.Click
        Dim result As Integer = MessageBox.Show("Esta seguro que desea ELIMINAR el registro", "Alerta", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")

        ElseIf result = DialogResult.Yes Then
            'MessageBox.Show("Yes pressed")
            Dim a As Integer = DataGridView1.CurrentRow.Index
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim query As String = "DELETE FROM rel_productos_equipos WHERE " & DataGridView1.Columns(2).Name & " = '" & DataGridView1.Item(2, a).Value & "'"
                    Dim cmd As New MySqlCommand(query, conn)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Registro ELIMINADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Eliminarse por: " & vbCrLf & ex.Message)
            End Try
        End If
        Cargar()
    End Sub

    Private Sub Eliminar_Prov_Prod_Click(sender As Object, e As EventArgs) Handles Eliminar_Prov_Prod.Click
        Dim result As Integer = MessageBox.Show("Esta seguro que desea ELIMINAR el registro", "Alerta", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")

        ElseIf result = DialogResult.Yes Then
            'MessageBox.Show("Yes pressed")
            Dim a As Integer = DataGridView2.CurrentRow.Index
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim query As String = "DELETE FROM rel_productos_proveedores WHERE " & DataGridView2.Columns(2).Name & " = '" & DataGridView2.Item(2, a).Value & "'"
                    Dim cmd As New MySqlCommand(query, conn)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Registro ELIMINADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Eliminarse por: " & vbCrLf & ex.Message)
            End Try
        End If
        Cargar()
    End Sub
    Dim Valor As String
    Private Sub Cantidad_Mov_Reg_Click(sender As Object, e As EventArgs) Handles Cantidad_Mov_Reg.Click
        If IsNumeric(Mov_ind.Text) Then
            If Mov_ind.Text < 0 Then
                GoTo err
            End If

            If Cantidad_a_mover < Convert.ToInt16(Mov_ind.Text) Then
                MsgBox("La cantidad supera la el valor del movimiento")
                GoTo err
            End If
            Dim fila As Integer = DataGridView5.CurrentCell.RowIndex
            If Tipo_Movi = "SALIDA" Then
                If Convert.ToInt16(Mov_ind.Text) <= DataGridView5.Item(4, fila).Value Then
                    Cantidad_a_mover = Cantidad_a_mover - Convert.ToInt16(Mov_ind.Text)
                    Valor = DataGridView5.Item(4, fila).Value - Convert.ToInt16(Mov_ind.Text)
                Else
                    MsgBox("La cantidad es mayor a la existente en la ubicación")
                    GoTo err
                End If
            Else
                If DataGridView5.Item(5, fila).Value = "N/A" Then
                    Cantidad_a_mover = Cantidad_a_mover - Convert.ToInt16(Mov_ind.Text)
                    Valor = DataGridView5.Item(4, fila).Value + Convert.ToInt16(Mov_ind.Text)
                Else

                    If Convert.ToInt16(Mov_ind.Text) < DataGridView5.Item(5, fila).Value Then
                        Cantidad_a_mover = Cantidad_a_mover - Convert.ToInt16(Mov_ind.Text)
                        Valor = DataGridView5.Item(4, fila).Value + Convert.ToInt16(Mov_ind.Text)

                    Else
                        MsgBox("La cantidad es mayor a el aforo en la ubicación")
                        GoTo err
                    End If

                End If
            End If
            Try
                Using conn As New MySqlConnection(datasource)
                    conn.Open()
                    Dim cmd As New MySqlCommand("UPDATE rel_ubicaciones_productos SET Cantidad = '" & Valor &
                        "' WHERE IdRel_Ubicaciones_Productos = '" & DataGridView5.Item(6, fila).Value & "'", conn)
                    cmd.ExecuteNonQuery()
                    'MessageBox.Show("Registro MODIFICADO")
                    conn.Close()
                End Using
            Catch ex As Exception
                MsgBox("El registro no pudo Modificarse por: " & vbCrLf & ex.Message)
            End Try
            Cargar()
            'DataGridView5.Item(4, fila).Value ' Cantidad
            'DataGridView5.Item(5, fila).Value ' Capacidad
        Else
err:
            MsgBox("Valor invalido")
            Mov_ind.Text = ""
        End If
    End Sub

    Private Sub Form3_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Select Case Bandera_Rel
            Case 4
                If Cantidad_a_mover <> 0 Then
                    e.Cancel = True
                    MsgBox("Aun tiene movimientos pendientes ")
                Else
                    Me.Dispose()
                End If
        End Select
    End Sub
End Class