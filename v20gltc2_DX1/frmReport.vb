Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib

Public Class frmReport
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmReport_Closed)
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmReport_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAdd.Click
        frmReport.cAction = "New"
        Me.fReportInfor = New frmReportInfo
        Me.fReportInfor.ShowDialog()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
        Dim currentRowIndex As Object = Me.grdReport.CurrentRowIndex
        If (ObjectType.ObjTst(frmReport.sysDv.Item(IntegerType.FromObject(currentRowIndex)).Item("loai_maubc"), 1, False) = 0) Then
            Msg.Alert(StringType.FromObject(DirMain.oVar.Item("m_not_dele")), 1)
        ElseIf (ObjectType.ObjTst(Msg.Question(StringType.FromObject(DirMain.oVar.Item("m_sure_dele")), 1), 1, False) = 0) Then
            Dim str As String = "ma_maubc = 'v20GLTC2'"
            str = StringType.FromObject(ObjectType.AddObj(str, ObjectType.AddObj(ObjectType.AddObj(" AND form = '", frmReport.sysDv.Item(IntegerType.FromObject(currentRowIndex)).Item("form")), "'")))
            Sql.SQLDelete((DirMain.appConn), "v20dmmaubc", str)
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("DELETE v20GLTC2 WHERE form = '", frmReport.sysDv.Item(IntegerType.FromObject(currentRowIndex)).Item("form")), "'"))
            Sql.SQLExecute((DirMain.appConn), tcSQL)
            frmReport.sysDv.Item(IntegerType.FromObject(currentRowIndex)).Delete()
            DirMain.rpTypeTable.Rows.Item(IntegerType.FromObject(currentRowIndex)).Delete()
            If (ObjectType.ObjTst(DirMain.fPrint.cboReportType.SelectedIndex, currentRowIndex, False) = 0) Then
                DirMain.fPrint.cboReportType.SelectedIndex = IntegerType.FromObject(ObjectType.SubObj(currentRowIndex, 1))
            End If
            DirMain.fPrint.cboReportType.Items.RemoveAt(IntegerType.FromObject(currentRowIndex))
            DirMain.rpTypeTable.AcceptChanges()
            frmReport.sysDv.Table.AcceptChanges()
            Me.grdReport.Refresh()
        End If
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEdit.Click
        frmReport.cAction = "Edit"
        Me.fReportInfor = New frmReportInfo
        Me.fReportInfor.ShowDialog()
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmReport_Closed(ByVal sender As Object, ByVal e As EventArgs)
        frmReport.sysDv = Nothing
    End Sub

    Private Sub frmReport_Load(ByVal sender As Object, ByVal e As EventArgs)
        frmReport.sysLanguage = StringType.FromObject(Reg.GetRegistryKey("Language"))
        Obj.Init(Me)
        Me.Text = StringType.FromObject(DirMain.oLan.Item("400"))
        Dim control As Control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(DirMain.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
        Next
        frmReport.sysDv = New DataView
        frmReport.sysDv.Table = DirMain.rpTypeTable
        Dim tbs As New DataGridTableStyle
        Dim row As DataRow = DirectCast(Sql.GetRow((DirMain.sysConn), "grid", ("code = '" & DirMain.SysID & "'")), DataRow)
        Dim obj3 As Object = Strings.Trim(StringType.FromObject(row.Item("Fields")))
        Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Interaction.IIf((StringType.StrCmp(frmReport.sysLanguage, "V", False) = 0), Strings.Trim(StringType.FromObject(row.Item("Headers"))), Strings.Trim(StringType.FromObject(row.Item("Headers2")))))
        Dim obj4 As Object = Strings.Trim(StringType.FromObject(row.Item("Formats")))
        Dim obj6 As Object = Strings.Trim(StringType.FromObject(row.Item("Widths")))
        Dim count As Object = row.Table.Columns.Count
        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn((IntegerType.FromObject(count) + 1) - 1) {}
        Dim num2 As Integer = IntegerType.FromObject(ObjectType.SubObj(count, 1))
        Dim i As Integer = 0
        Do While (i <= num2)
            cols(i) = New DataGridTextBoxColumn
            i += 1
        Loop
        Fill2Grid.Fill((frmReport.sysDv), (grdReport), (tbs), (cols), StringType.FromObject(obj3), StringType.FromObject(objectValue), StringType.FromObject(obj4), StringType.FromObject(obj6))
        tbs.AllowSorting = False
        Me.grdReport.CurrentRowIndex = DirMain.fPrint.cboReportType.SelectedIndex
    End Sub

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.grdReport = New gridformtran
        Me.cmdAdd = New Button
        Me.cmdEdit = New Button
        Me.cmdDelete = New Button
        Me.cmdClose = New Button
        Me.grdReport.BeginInit()
        Me.SuspendLayout()
        Me.grdReport.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        Me.grdReport.BackgroundColor = Color.White
        Me.grdReport.CaptionVisible = False
        Me.grdReport.DataMember = ""
        Me.grdReport.HeaderForeColor = SystemColors.ControlText
        Me.grdReport.Location = New Point(8, 8)
        Me.grdReport.Name = "grdReport"
        Me.grdReport.ReadOnly = True
        Me.grdReport.Size = New Size(360, 182)
        Me.grdReport.TabIndex = 0
        Me.cmdAdd.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.cmdAdd.Location = New Point(384, 8)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.TabIndex = 1
        Me.cmdAdd.Tag = "L401"
        Me.cmdAdd.Text = "Them"
        Me.cmdEdit.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.cmdEdit.Location = New Point(384, 38)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.TabIndex = 2
        Me.cmdEdit.Tag = "L402"
        Me.cmdEdit.Text = "Sua"
        Me.cmdDelete.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.cmdDelete.Location = New Point(384, 68)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.TabIndex = 3
        Me.cmdDelete.Tag = "L403"
        Me.cmdDelete.Text = "Xoa"
        Me.cmdClose.Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
        Me.cmdClose.Location = New Point(384, 97)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.TabIndex = 4
        Me.cmdClose.Tag = "L404"
        Me.cmdClose.Text = "Thoat"
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(472, 205)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.grdReport)
        Me.Name = "frmUser"
        Me.Text = "Report"
        Me.grdReport.EndInit()
        Me.ResumeLayout(False)
    End Sub


    ' Properties
    Friend WithEvents cmdAdd As Button
    Friend WithEvents cmdClose As Button
    Friend WithEvents cmdDelete As Button
    Friend WithEvents cmdEdit As Button
    Friend WithEvents grdReport As gridformtran

    Public Shared cAction As String
    Private components As IContainer
    Public fReportInfor As frmReportInfo
    Public Shared nRPType As Byte
    Public Shared sysDv As DataView
    Public Shared sysLanguage As String
End Class

