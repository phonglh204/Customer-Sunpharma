Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol

Public Class frmReportInfo
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmReportInfo_Load)
        Me.iCurrRow = DirMain.fReport.grdReport.CurrentRowIndex
        Me.InitializeComponent()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDetail.Click
        On Error Resume Next
        Dim visible As Boolean
        Dim control2 As Control = Nothing
        Dim control As Control = Nothing
        For Each control In Me.Controls
            If (Strings.InStr(StringType.FromObject(control.Tag), "H", CompareMethod.Binary) > 0) Then
                control.Visible = Not control.Visible
                visible = control.Visible
            End If
            If (Strings.InStr(StringType.FromObject(control.Tag), "HL", CompareMethod.Binary) > 0) Then
                control2 = control
            End If
        Next
        If visible Then
            Me.cmdDetail.Text = StringType.FromObject(DirMain.oLan.Item("513"))
            If (Not control2 Is Nothing) Then
                Me.Height = (control2.Top + &H53)
            End If
        Else
            Me.Height = Me.oldheight
            Me.cmdDetail.Text = StringType.FromObject(DirMain.oLan.Item("512"))
        End If
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        Dim flag As Boolean = True
        Dim control As Control
        For Each control In Me.Controls
            If ((Strings.InStr(StringType.FromObject(control.Tag), "NB", CompareMethod.Binary) > 0) And (StringType.StrCmp(Strings.Trim(control.Text), "", False) = 0)) Then
                control.Focus()
                Msg.Alert(StringType.FromObject(DirMain.oVar.Item("m_not_blank")), 2)
                flag = False
                Exit For
            End If
        Next
        If flag Then
            If (StringType.StrCmp(frmReport.cAction, "New", False) = 0) Then
                Sql.AppendBlank(frmReport.sysDv)
                Me.iCurrRow = (frmReport.sysDv.Count - 1)
            End If
            Dim o As DataRowView = frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow))
            For Each control In Me.Controls
                If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "F", False) = 0) Then
                    Dim obj2 As Object = Strings.Right(control.Name, (control.Name.Length - 3))
                    LateBinding.LateSet(o, Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(obj2), control.Text}, Nothing)
                End If
            Next
            Dim toRow As DataRow = frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)).Row
            If (StringType.StrCmp(frmReport.cAction, "New", False) = 0) Then
                Dim obj3 As Object = &H41
                frmReport.sysDv.Sort = "form"
                Dim num3 As Integer = (frmReport.sysDv.Count - 1)
                Dim i As Integer = 1
                Do While (i <= num3)
                    If ((ObjectType.ObjTst(i, Me.iCurrRow, False) <> 0) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(frmReport.sysDv.Item(i).Item("form"))), ("v20GLTC2" & StringType.FromChar(Strings.Chr(IntegerType.FromObject(obj3)))), False) <= 0)) Then
                        obj3 = ObjectType.AddObj(obj3, 1)
                    End If
                    i += 1
                Loop
                frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)).Item("ma_maubc") = "v20GLTC2"
                frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)).Item("loai_maubc") = 0
                frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)).Item("form") = ("v20GLTC2" & StringType.FromChar(Strings.Chr(IntegerType.FromObject(obj3))))
                Sql.SQLInsert((DirMain.appConn), "v20dmmaubc", toRow)
                Dim tcSQL As String = "fs_CopyFReports 'v20GLTC2'"
                tcSQL = ((tcSQL & ", '" & Strings.Trim(StringType.FromObject(DirMain.rpTypeTable.Rows.Item(DirMain.fReport.grdReport.CurrentRowIndex).Item("form"))) & "'") & ", 'v20GLTC2" & StringType.FromChar(Strings.Chr(IntegerType.FromObject(obj3))) & "'")
                Sql.SQLExecute((DirMain.appConn), tcSQL)
            Else
                Dim str2 As String = "ma_maubc = 'v20GLTC2'"
                str2 = StringType.FromObject(ObjectType.AddObj(str2, ObjectType.AddObj(ObjectType.AddObj(" AND form = '", toRow.Item("form")), "'")))
                Sql.SQLUpdate((DirMain.appConn), "v20dmmaubc", toRow, str2)
            End If
            Dim selectedIndex As Integer = DirMain.fPrint.cboReportType.SelectedIndex
            DirMain.fPrint.cboReportType.Items.Clear()
            DirMain.fPrint.AddReportType()
            DirMain.fPrint.cboReportType.SelectedIndex = selectedIndex
            o = Nothing
            DirMain.fReport.grdReport.CurrentRowIndex = IntegerType.FromObject(Me.iCurrRow)
            Me.EditReport()
            Me.Close()
        End If
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub EditReport()
        oRPFormLib = New reportviewlib("111001001")
        oRPFormLib.SysID = DirMain.SysID
        oRPFormLib.appConn = DirMain.appConn
        oRPFormLib.sysConn = DirMain.sysConn
        oRPFormLib.oLan = DirMain.oLan
        oRPFormLib.oVar = DirMain.oVar
        oRPFormLib.oOptions = DirMain.oOption
        oRPFormLib.GetClsreports.strSQLRunReport = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("SELECT * FROM v20gltc2 WHERE form = '", frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)).Item("form")), "' ORDER BY stt, ma_so"))
        oRPFormLib.GetClsreports.strAliasReport = "v20GLTC2"
        oRPFormLib.Init()
        oRPFormLib.frmUpdate = New frmDirInfor
        oRPFormLib.Show()
        Dim tcSQL As String = ("fs20_UpdateFieldIds 'v20GLTC2', '" & Strings.Trim(StringType.FromObject(frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)).Item("form"))) & "'")
        Sql.SQLExecute((DirMain.appConn), tcSQL)
        tcSQL = (" ff20_CheckFinancialReport 'v20GLTC2', '" & Strings.Trim(StringType.FromObject(frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)).Item("form"))) & "'")
        Dim ds As New DataSet
        Sql.SQLRetrieve((DirMain.appConn), tcSQL, "v20GLTC2", (ds))
        Dim sLeft As String = Strings.Trim(StringType.FromObject(ds.Tables.Item("v20GLTC2").Rows.Item(0).Item("Code")))
        If (StringType.StrCmp(sLeft, "", False) <> 0) Then
            Msg.Alert(Strings.Replace(StringType.FromObject(DirMain.oLan.Item("702")), "%s", sLeft, 1, -1, CompareMethod.Binary), 1)
        End If
        ds = Nothing
    End Sub

    Private Sub frmReportInfo_Load(ByVal sender As Object, ByVal e As EventArgs)
        Obj.Init(Me)
        On Error Resume Next
        Me.Text = StringType.FromObject(DirMain.oLan.Item("400"))
        Dim control As Control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(DirMain.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "F", False) = 0) Then
                Dim box As TextBox = Nothing
                Dim obj2 As Object = Strings.Right(control.Name, (control.Name.Length - 3))
                If (StringType.StrCmp(Strings.Mid(StringType.FromObject(control.Tag), 2, 1), "C", False) = 0) Then
                    box = DirectCast(control, TextBox)
                End If
                If (StringType.StrCmp(frmReport.cAction, "New", False) = 0) Then
                    box.Text = ""
                    GoTo break
                End If
                If (StringType.StrCmp(Strings.Mid(StringType.FromObject(control.Tag), 2, 1), "C", False) = 0) Then
                    box.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(obj2)}, Nothing, Nothing)))
                Else
                    box.Text = StringType.FromObject(LateBinding.LateGet(frmReport.sysDv.Item(IntegerType.FromObject(Me.iCurrRow)), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(obj2)}, Nothing, Nothing))
                End If
            End If
break:
        Next
        Me.oldheight = Me.Height
        Me.cmdDetail_Click(Me.cmdDetail, New EventArgs)
    End Sub

    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtTen_maubc = New System.Windows.Forms.TextBox()
        Me.txtTen_maubc2 = New System.Windows.Forms.TextBox()
        Me.lblComment = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.txtTitle2 = New System.Windows.Forms.TextBox()
        Me.txtH_line1 = New System.Windows.Forms.TextBox()
        Me.txtH_line2 = New System.Windows.Forms.TextBox()
        Me.txtH_line3 = New System.Windows.Forms.TextBox()
        Me.txtH_line4 = New System.Windows.Forms.TextBox()
        Me.txtH_line5 = New System.Windows.Forms.TextBox()
        Me.cmdDetail = New System.Windows.Forms.Button()
        Me.txtH_line52 = New System.Windows.Forms.TextBox()
        Me.txtH_line42 = New System.Windows.Forms.TextBox()
        Me.txtH_line32 = New System.Windows.Forms.TextBox()
        Me.txtH_line22 = New System.Windows.Forms.TextBox()
        Me.txtH_line12 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(15, 12)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(68, 13)
        Me.lblName.TabIndex = 0
        Me.lblName.Tag = "L501"
        Me.lblName.Text = "Ten bao cao"
        '
        'txtTen_maubc
        '
        Me.txtTen_maubc.Location = New System.Drawing.Point(96, 10)
        Me.txtTen_maubc.Name = "txtTen_maubc"
        Me.txtTen_maubc.Size = New System.Drawing.Size(250, 20)
        Me.txtTen_maubc.TabIndex = 0
        Me.txtTen_maubc.Tag = "FCNB"
        Me.txtTen_maubc.Text = "txtTen_maubc"
        '
        'txtTen_maubc2
        '
        Me.txtTen_maubc2.Location = New System.Drawing.Point(96, 33)
        Me.txtTen_maubc2.Name = "txtTen_maubc2"
        Me.txtTen_maubc2.Size = New System.Drawing.Size(250, 20)
        Me.txtTen_maubc2.TabIndex = 1
        Me.txtTen_maubc2.Tag = "FC"
        Me.txtTen_maubc2.Text = "txtTen_maubc2"
        '
        'lblComment
        '
        Me.lblComment.AutoSize = True
        Me.lblComment.Location = New System.Drawing.Point(15, 35)
        Me.lblComment.Name = "lblComment"
        Me.lblComment.Size = New System.Drawing.Size(35, 13)
        Me.lblComment.TabIndex = 2
        Me.lblComment.Tag = "L502"
        Me.lblComment.Text = "Ten 2"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(9, 110)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 14
        Me.cmdOk.Tag = "L503"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(85, 110)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 15
        Me.cmdCancel.Tag = "L504"
        Me.cmdCancel.Text = "Huy"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(432, 106)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Tag = "L505"
        Me.Label1.Text = "Tieu de"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Tag = "L506"
        Me.Label2.Text = "Tieu de 2"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(193, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Tag = "L507H"
        Me.Label3.Text = "Thong tin them"
        '
        'txtTitle
        '
        Me.txtTitle.Location = New System.Drawing.Point(96, 56)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(250, 20)
        Me.txtTitle.TabIndex = 2
        Me.txtTitle.Tag = "FC"
        Me.txtTitle.Text = "txtTitle"
        '
        'txtTitle2
        '
        Me.txtTitle2.Location = New System.Drawing.Point(96, 79)
        Me.txtTitle2.Name = "txtTitle2"
        Me.txtTitle2.Size = New System.Drawing.Size(250, 20)
        Me.txtTitle2.TabIndex = 3
        Me.txtTitle2.Tag = "FC"
        Me.txtTitle2.Text = "txtTitle2"
        '
        'txtH_line1
        '
        Me.txtH_line1.Location = New System.Drawing.Point(37, 154)
        Me.txtH_line1.Name = "txtH_line1"
        Me.txtH_line1.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line1.TabIndex = 4
        Me.txtH_line1.Tag = "FCH"
        Me.txtH_line1.Text = "txtH_line1"
        Me.txtH_line1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtH_line2
        '
        Me.txtH_line2.Location = New System.Drawing.Point(37, 177)
        Me.txtH_line2.Name = "txtH_line2"
        Me.txtH_line2.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line2.TabIndex = 6
        Me.txtH_line2.Tag = "FCH"
        Me.txtH_line2.Text = "txtH_line2"
        Me.txtH_line2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtH_line3
        '
        Me.txtH_line3.Location = New System.Drawing.Point(37, 200)
        Me.txtH_line3.Name = "txtH_line3"
        Me.txtH_line3.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line3.TabIndex = 8
        Me.txtH_line3.Tag = "FCH"
        Me.txtH_line3.Text = "txtH_line3"
        Me.txtH_line3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtH_line4
        '
        Me.txtH_line4.Location = New System.Drawing.Point(37, 223)
        Me.txtH_line4.Name = "txtH_line4"
        Me.txtH_line4.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line4.TabIndex = 10
        Me.txtH_line4.Tag = "FCH"
        Me.txtH_line4.Text = "txtH_line4"
        Me.txtH_line4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtH_line5
        '
        Me.txtH_line5.Location = New System.Drawing.Point(37, 246)
        Me.txtH_line5.Name = "txtH_line5"
        Me.txtH_line5.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line5.TabIndex = 12
        Me.txtH_line5.Tag = "FCHL"
        Me.txtH_line5.Text = "txtH_line5"
        Me.txtH_line5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdDetail
        '
        Me.cmdDetail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDetail.Location = New System.Drawing.Point(376, 110)
        Me.cmdDetail.Name = "cmdDetail"
        Me.cmdDetail.Size = New System.Drawing.Size(64, 23)
        Me.cmdDetail.TabIndex = 16
        Me.cmdDetail.TabStop = False
        Me.cmdDetail.Tag = "L513"
        Me.cmdDetail.Text = "Mo rong"
        '
        'txtH_line52
        '
        Me.txtH_line52.Location = New System.Drawing.Point(232, 246)
        Me.txtH_line52.Name = "txtH_line52"
        Me.txtH_line52.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line52.TabIndex = 13
        Me.txtH_line52.Tag = "FCH"
        Me.txtH_line52.Text = "txtH_line52"
        Me.txtH_line52.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtH_line42
        '
        Me.txtH_line42.Location = New System.Drawing.Point(232, 223)
        Me.txtH_line42.Name = "txtH_line42"
        Me.txtH_line42.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line42.TabIndex = 11
        Me.txtH_line42.Tag = "FCH"
        Me.txtH_line42.Text = "txtH_line42"
        Me.txtH_line42.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtH_line32
        '
        Me.txtH_line32.Location = New System.Drawing.Point(232, 200)
        Me.txtH_line32.Name = "txtH_line32"
        Me.txtH_line32.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line32.TabIndex = 9
        Me.txtH_line32.Tag = "FCH"
        Me.txtH_line32.Text = "txtH_line32"
        Me.txtH_line32.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtH_line22
        '
        Me.txtH_line22.Location = New System.Drawing.Point(232, 177)
        Me.txtH_line22.Name = "txtH_line22"
        Me.txtH_line22.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line22.TabIndex = 7
        Me.txtH_line22.Tag = "FCH"
        Me.txtH_line22.Text = "txtH_line22"
        Me.txtH_line22.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtH_line12
        '
        Me.txtH_line12.Location = New System.Drawing.Point(232, 154)
        Me.txtH_line12.Name = "txtH_line12"
        Me.txtH_line12.Size = New System.Drawing.Size(192, 20)
        Me.txtH_line12.TabIndex = 5
        Me.txtH_line12.Tag = "FCH"
        Me.txtH_line12.Text = "txtH_line12"
        Me.txtH_line12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(152, 136)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 16)
        Me.Label4.TabIndex = 17
        Me.Label4.Tag = "L508H"
        Me.Label4.Text = "Thong tin"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(344, 136)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(80, 16)
        Me.Label5.TabIndex = 18
        Me.Label5.Tag = "L509H"
        Me.Label5.Text = "Thong tin 2"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmReportInfo
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(448, 137)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtH_line52)
        Me.Controls.Add(Me.txtH_line42)
        Me.Controls.Add(Me.txtH_line32)
        Me.Controls.Add(Me.txtH_line22)
        Me.Controls.Add(Me.txtH_line12)
        Me.Controls.Add(Me.cmdDetail)
        Me.Controls.Add(Me.txtH_line5)
        Me.Controls.Add(Me.txtH_line4)
        Me.Controls.Add(Me.txtH_line3)
        Me.Controls.Add(Me.txtH_line2)
        Me.Controls.Add(Me.txtH_line1)
        Me.Controls.Add(Me.txtTitle2)
        Me.Controls.Add(Me.txtTitle)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.txtTen_maubc2)
        Me.Controls.Add(Me.lblComment)
        Me.Controls.Add(Me.txtTen_maubc)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "frmReportInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmReportInfo"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdDetail As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblComment As Label
    Friend WithEvents lblName As Label
    Friend WithEvents txtH_line1 As TextBox
    Friend WithEvents txtH_line12 As TextBox
    Friend WithEvents txtH_line2 As TextBox
    Friend WithEvents txtH_line22 As TextBox
    Friend WithEvents txtH_line3 As TextBox
    Friend WithEvents txtH_line32 As TextBox
    Friend WithEvents txtH_line4 As TextBox
    Friend WithEvents txtH_line42 As TextBox
    Friend WithEvents txtH_line5 As TextBox
    Friend WithEvents txtH_line52 As TextBox
    Friend WithEvents txtTen_maubc As TextBox
    Friend WithEvents txtTen_maubc2 As TextBox
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents txtTitle2 As TextBox

    Private components As IContainer
    Private iCurrRow As Object
    Private oldheight As Integer
    Public oRPFormLib As reportviewlib
    Private strOldReport As String
End Class

