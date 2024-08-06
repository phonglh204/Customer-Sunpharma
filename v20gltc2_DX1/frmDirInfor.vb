Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol

Public Class frmDirInfor
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Closed, New EventHandler(AddressOf Me.frmDirInfor_Closed)
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
        Me.InitializeComponent()
    End Sub

    Private Function CheckFormular(ByVal formular As String) As Boolean
        If (StringType.StrCmp(formular.Trim, "", False) <> 0) Then
            Dim tcSQL As String = "SELECT 0 + "
            Dim flag2 As Boolean = True
            Dim start As Integer = 1
            tcSQL = "SELECT 0 + "
            Do While (start <= Strings.Len(formular.Trim))
Label_0028:
                If (StringType.StrCmp(Strings.Mid(formular.Trim, start, 1), "[", False) = 0) Then
                    tcSQL = (tcSQL & "NU")
                    flag2 = False
                End If
                If (StringType.StrCmp(Strings.Mid(formular.Trim, start, 1), "]", False) = 0) Then
                    tcSQL = (tcSQL & "LL")
                    flag2 = True
                    start += 1
                    GoTo Label_0028
                End If
                If flag2 Then
                    tcSQL = (tcSQL & Strings.Mid(formular.Trim, start, 1))
                End If
                start += 1
            Loop
            Try
                Sql.SQLExecute((DirMain.appConn), tcSQL)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError()
                Return False
            End Try
        End If
        Return True
    End Function

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If Not Me.CheckFormular(Me.txtCach_tinh.Text) Then
            Msg.Alert(StringType.FromObject(DirMain.oLan.Item("701")), 1)
            Me.txtCach_tinh.Focus()
        Else
            Dim strKeyField As String ' = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(("'" & Strings.Trim(StringType.FromObject(frmReport.sysDv.Item(DirMain.fReport.grdReport.CurrentRowIndex).Item("form"))) & "'"), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtStt.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtMa_so.Text, ""))))
            strKeyField = "'" & Strings.Trim(StringType.FromObject(frmReport.sysDv.Item(DirMain.fReport.grdReport.CurrentRowIndex).Item("form"))) & "'"
            strKeyField += ", " + Sql.ConvertVS2SQLType(Me.txtStt.Value, "")
            strKeyField += ", " + Sql.ConvertVS2SQLType(Me.txtMa_so.Text, "")
            DirMain.fReport.fReportInfor.oRPFormLib.SaveFormDir(Me, strKeyField)
        End If
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDirInfor_Closed(ByVal sender As Object, ByVal e As EventArgs)
        DirMain.fReport.fReportInfor.oRPFormLib.frmUpdate = New frmDirInfor
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lib4 As New CharLib(Me.txtIn_ck, "1, 0")
        Dim oBold As New CharLib(Me.txtBold, "1, 0")
        Dim lib3 As New CharLib(Me.txtKind, "9, 1, 0,2,3")
        Dim lib2 As New CharLib(Me.txtGiam_tru, "1, 0")
        Me.txtGiam_tru.MaxLength = 1
        If (StringType.StrCmp(DirMain.fReport.fReportInfor.oRPFormLib.cAction, "New", False) = 0) Then
            Me.txtForm.Text = StringType.FromObject(frmReport.sysDv.Item(DirMain.fReport.grdReport.CurrentRowIndex).Item("form"))
        End If
        'Me.txtKind.Text = ""
        'If ((((StringType.StrCmp(Me.txtKind.Text, "", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtTk_no.Text), "", False) = 0)) And (StringType.StrCmp(Strings.Trim(Me.txtTk_co.Text), "", False) = 0)) And (StringType.StrCmp(Strings.Trim(Me.txtCach_tinh.Text), "", False) = 0)) Then
        '    Me.txtKind.Text = "9"
        'End If
        'If ((StringType.StrCmp(Me.txtKind.Text, "", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtCach_tinh.Text), "", False) <> 0)) Then
        '    Me.txtKind.Text = "0"
        'End If
        'If ((StringType.StrCmp(Me.txtKind.Text, "", False) = 0) And ((StringType.StrCmp(Strings.Trim(Me.txtTk_no.Text), "", False) <> 0) Or (StringType.StrCmp(Strings.Trim(Me.txtTk_co.Text), "", False) <> 0))) Then
        '    Me.txtKind.Text = "1"
        'End If
        'Me.txtKind_TextChanged(Me.txtKind, New EventArgs)
        'AddHandler Me.txtTk_no.TextChanged, New EventHandler(AddressOf Me.txtTk_TextChanged)
        'AddHandler Me.txtTk_co.TextChanged, New EventHandler(AddressOf Me.txtTk_TextChanged)
        'Dim clscodeDept As New clsGetcodes.clsGetcodes(txtMa_bp, sysConn, appConn, "dmbp", "ma_bp", "SaleDept", "Status='1'", cmdCancel)
        'Dim clscodeFee As New clsGetcodes.clsGetcodes(txtMa_phi, sysConn, appConn, "dmphi", "ma_phi", "Fee", "Status='1'", cmdCancel)
        'Dim clscodeJob As New clsGetcodes.clsGetcodes(txtMa_vv, sysConn, appConn, "dmvv", "ma_vv", "Job", "Status='1'", cmdCancel)
    End Sub
    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblBold As Label
    Friend WithEvents lblBoldMess As Label
    Friend WithEvents lblCach_tinh As Label
    Friend WithEvents lblCach_tinhMess As Label
    Friend WithEvents lblChi_tieu As Label
    Friend WithEvents lblChi_tieu2 As Label
    Friend WithEvents lblCo_khong As Label
    Friend WithEvents lblCong_thuc As Label
    Friend WithEvents lblGiam_tru As Label
    Friend WithEvents lblIn_ck As Label
    Friend WithEvents lblInMess As Label
    Friend WithEvents lblMa_so As Label
    Friend WithEvents lblStt As Label
    Friend WithEvents lblTk_co As Label
    Friend WithEvents lblTk_no As Label
    Friend WithEvents txtBold As txtNumeric
    Friend WithEvents txtCach_tinh As TextBox
    Friend WithEvents txtChi_tieu As TextBox
    Friend WithEvents txtChi_tieu2 As TextBox
    Friend WithEvents txtForm As TextBox
    Friend WithEvents txtGiam_tru As txtNumeric
    Friend WithEvents txtIn_ck As txtNumeric
    Friend WithEvents txtKind As txtNumeric
    Friend WithEvents txtMa_so As TextBox
    Friend WithEvents txtStt As txtNumeric
    Friend WithEvents txtThuyet_minh As TextBox
    Friend WithEvents txtTk_co As TextBox
    Friend WithEvents txtTk_no As TextBox
    Private components As IContainer
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtMa_bp As System.Windows.Forms.TextBox
    Friend WithEvents txtMa_phi As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtMa_vv As System.Windows.Forms.TextBox

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblTk_no = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpInfor = New System.Windows.Forms.GroupBox()
        Me.lblStt = New System.Windows.Forms.Label()
        Me.txtStt = New txtNumeric()
        Me.txtTk_no = New System.Windows.Forms.TextBox()
        Me.txtMa_so = New System.Windows.Forms.TextBox()
        Me.lblMa_so = New System.Windows.Forms.Label()
        Me.txtChi_tieu = New System.Windows.Forms.TextBox()
        Me.lblChi_tieu = New System.Windows.Forms.Label()
        Me.txtChi_tieu2 = New System.Windows.Forms.TextBox()
        Me.lblChi_tieu2 = New System.Windows.Forms.Label()
        Me.txtIn_ck = New txtNumeric
        Me.lblIn_ck = New System.Windows.Forms.Label()
        Me.txtBold = New txtNumeric
        Me.lblBold = New System.Windows.Forms.Label()
        Me.lblInMess = New System.Windows.Forms.Label()
        Me.lblBoldMess = New System.Windows.Forms.Label()
        Me.lblCach_tinhMess = New System.Windows.Forms.Label()
        Me.lblCach_tinh = New System.Windows.Forms.Label()
        Me.txtKind = New txtNumeric
        Me.txtCach_tinh = New System.Windows.Forms.TextBox()
        Me.lblCong_thuc = New System.Windows.Forms.Label()
        Me.txtForm = New System.Windows.Forms.TextBox()
        Me.txtTk_co = New System.Windows.Forms.TextBox()
        Me.lblTk_co = New System.Windows.Forms.Label()
        Me.txtGiam_tru = New txtNumeric
        Me.lblGiam_tru = New System.Windows.Forms.Label()
        Me.lblCo_khong = New System.Windows.Forms.Label()
        Me.txtThuyet_minh = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtMa_bp = New System.Windows.Forms.TextBox()
        Me.txtMa_phi = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtMa_vv = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'lblTk_no
        '
        Me.lblTk_no.AutoSize = True
        Me.lblTk_no.Location = New System.Drawing.Point(48, 239)
        Me.lblTk_no.Name = "lblTk_no"
        Me.lblTk_no.Size = New System.Drawing.Size(94, 13)
        Me.lblTk_no.TabIndex = 5
        Me.lblTk_no.Tag = "L610"
        Me.lblTk_no.Text = "- Cac tai khoan no"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.Location = New System.Drawing.Point(8, 398)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 15
        Me.cmdOk.Tag = "L613"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(84, 398)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 16
        Me.cmdCancel.Tag = "L614"
        Me.cmdCancel.Text = "Huy"
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(8, 0)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(625, 136)
        Me.grpInfor.TabIndex = 0
        Me.grpInfor.TabStop = False
        '
        'lblStt
        '
        Me.lblStt.AutoSize = True
        Me.lblStt.Location = New System.Drawing.Point(23, 18)
        Me.lblStt.Name = "lblStt"
        Me.lblStt.Size = New System.Drawing.Size(28, 13)
        Me.lblStt.TabIndex = 22
        Me.lblStt.Tag = "L601"
        Me.lblStt.Text = "STT"
        '
        'txtStt
        '
        Me.txtStt.Format = "####"
        Me.txtStt.Location = New System.Drawing.Point(155, 16)
        Me.txtStt.MaxLength = 5
        Me.txtStt.Name = "txtStt"
        Me.txtStt.Size = New System.Drawing.Size(48, 20)
        Me.txtStt.TabIndex = 0
        Me.txtStt.Tag = "FNNBDF"
        Me.txtStt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtStt.Value = 0R
        '
        'txtTk_no
        '
        Me.txtTk_no.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_no.Location = New System.Drawing.Point(155, 237)
        Me.txtTk_no.Name = "txtTk_no"
        Me.txtTk_no.Size = New System.Drawing.Size(160, 20)
        Me.txtTk_no.TabIndex = 8
        Me.txtTk_no.Tag = "FCDF"
        Me.txtTk_no.Text = "TXTTK_NO"
        '
        'txtMa_so
        '
        Me.txtMa_so.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_so.Location = New System.Drawing.Point(155, 39)
        Me.txtMa_so.Name = "txtMa_so"
        Me.txtMa_so.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_so.TabIndex = 1
        Me.txtMa_so.Tag = "FCNBDF"
        Me.txtMa_so.Text = "TXTMA_SO"
        '
        'lblMa_so
        '
        Me.lblMa_so.AutoSize = True
        Me.lblMa_so.Location = New System.Drawing.Point(23, 41)
        Me.lblMa_so.Name = "lblMa_so"
        Me.lblMa_so.Size = New System.Drawing.Size(36, 13)
        Me.lblMa_so.TabIndex = 37
        Me.lblMa_so.Tag = "L602"
        Me.lblMa_so.Text = "Ma so"
        '
        'txtChi_tieu
        '
        Me.txtChi_tieu.Location = New System.Drawing.Point(155, 62)
        Me.txtChi_tieu.Name = "txtChi_tieu"
        Me.txtChi_tieu.Size = New System.Drawing.Size(300, 20)
        Me.txtChi_tieu.TabIndex = 2
        Me.txtChi_tieu.Tag = "FCNBDF"
        Me.txtChi_tieu.Text = "txtChi_tieu"
        '
        'lblChi_tieu
        '
        Me.lblChi_tieu.AutoSize = True
        Me.lblChi_tieu.Location = New System.Drawing.Point(23, 64)
        Me.lblChi_tieu.Name = "lblChi_tieu"
        Me.lblChi_tieu.Size = New System.Drawing.Size(42, 13)
        Me.lblChi_tieu.TabIndex = 39
        Me.lblChi_tieu.Tag = "L603"
        Me.lblChi_tieu.Text = "Chi tieu"
        '
        'txtChi_tieu2
        '
        Me.txtChi_tieu2.Location = New System.Drawing.Point(155, 85)
        Me.txtChi_tieu2.Name = "txtChi_tieu2"
        Me.txtChi_tieu2.Size = New System.Drawing.Size(300, 20)
        Me.txtChi_tieu2.TabIndex = 3
        Me.txtChi_tieu2.Tag = "FCDF"
        Me.txtChi_tieu2.Text = "txtChi_tieu2"
        '
        'lblChi_tieu2
        '
        Me.lblChi_tieu2.AutoSize = True
        Me.lblChi_tieu2.Location = New System.Drawing.Point(23, 87)
        Me.lblChi_tieu2.Name = "lblChi_tieu2"
        Me.lblChi_tieu2.Size = New System.Drawing.Size(51, 13)
        Me.lblChi_tieu2.TabIndex = 41
        Me.lblChi_tieu2.Tag = "L604"
        Me.lblChi_tieu2.Text = "Chi tieu 2"
        '
        'txtIn_ck
        '
        Me.txtIn_ck.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtIn_ck.Location = New System.Drawing.Point(155, 152)
        Me.txtIn_ck.MaxLength = 1
        Me.txtIn_ck.Name = "txtIn_ck"
        Me.txtIn_ck.Size = New System.Drawing.Size(32, 20)
        Me.txtIn_ck.TabIndex = 5
        Me.txtIn_ck.Tag = "FNNBDF"
        Me.txtIn_ck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblIn_ck
        '
        Me.lblIn_ck.AutoSize = True
        Me.lblIn_ck.Location = New System.Drawing.Point(24, 154)
        Me.lblIn_ck.Name = "lblIn_ck"
        Me.lblIn_ck.Size = New System.Drawing.Size(16, 13)
        Me.lblIn_ck.TabIndex = 43
        Me.lblIn_ck.Tag = "L605"
        Me.lblIn_ck.Text = "In"
        '
        'txtBold
        '
        Me.txtBold.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBold.Location = New System.Drawing.Point(155, 175)
        Me.txtBold.MaxLength = 1
        Me.txtBold.Name = "txtBold"
        Me.txtBold.Size = New System.Drawing.Size(32, 20)
        Me.txtBold.TabIndex = 6
        Me.txtBold.Tag = "FNNBDF"
        Me.txtBold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBold
        '
        Me.lblBold.AutoSize = True
        Me.lblBold.Location = New System.Drawing.Point(24, 177)
        Me.lblBold.Name = "lblBold"
        Me.lblBold.Size = New System.Drawing.Size(49, 13)
        Me.lblBold.TabIndex = 45
        Me.lblBold.Tag = "L606"
        Me.lblBold.Text = "Kieu chu"
        '
        'lblInMess
        '
        Me.lblInMess.AutoSize = True
        Me.lblInMess.Location = New System.Drawing.Point(192, 154)
        Me.lblInMess.Name = "lblInMess"
        Me.lblInMess.Size = New System.Drawing.Size(109, 13)
        Me.lblInMess.TabIndex = 46
        Me.lblInMess.Tag = "L615"
        Me.lblInMess.Text = "1 - Co in, 0 - Khong in"
        '
        'lblBoldMess
        '
        Me.lblBoldMess.AutoSize = True
        Me.lblBoldMess.Location = New System.Drawing.Point(192, 177)
        Me.lblBoldMess.Name = "lblBoldMess"
        Me.lblBoldMess.Size = New System.Drawing.Size(119, 13)
        Me.lblBoldMess.TabIndex = 47
        Me.lblBoldMess.Tag = "L616"
        Me.lblBoldMess.Text = "1 - Dam, 0 - Khong dam"
        '
        'lblCach_tinhMess
        '
        Me.lblCach_tinhMess.AutoSize = True
        Me.lblCach_tinhMess.Location = New System.Drawing.Point(192, 218)
        Me.lblCach_tinhMess.Name = "lblCach_tinhMess"
        Me.lblCach_tinhMess.Size = New System.Drawing.Size(420, 13)
        Me.lblCach_tinhMess.TabIndex = 56
        Me.lblCach_tinhMess.Tag = "L619"
        Me.lblCach_tinhMess.Text = "9 - Nhap, 1 - Phat sinh tai khoan, 0 - Tinh theo cac ma so, 2 - So du dau, 3 - So" &
    " du cuoi"
        '
        'txtKind
        '
        Me.txtKind.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtKind.Location = New System.Drawing.Point(155, 216)
        Me.txtKind.MaxLength = 1
        Me.txtKind.Name = "txtKind"
        Me.txtKind.Size = New System.Drawing.Size(32, 20)
        Me.txtKind.TabIndex = 7
        Me.txtKind.Tag = "FN"
        Me.txtKind.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCach_tinh
        '
        Me.lblCach_tinh.AutoSize = True
        Me.lblCach_tinh.Location = New System.Drawing.Point(24, 218)
        Me.lblCach_tinh.Name = "lblCach_tinh"
        Me.lblCach_tinh.Size = New System.Drawing.Size(52, 13)
        Me.lblCach_tinh.TabIndex = 55
        Me.lblCach_tinh.Tag = "L609"
        Me.lblCach_tinh.Text = "Cach tinh"
        '
        'txtCach_tinh
        '
        Me.txtCach_tinh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCach_tinh.Location = New System.Drawing.Point(155, 363)
        Me.txtCach_tinh.Name = "txtCach_tinh"
        Me.txtCach_tinh.Size = New System.Drawing.Size(300, 20)
        Me.txtCach_tinh.TabIndex = 14
        Me.txtCach_tinh.Tag = "FCDF"
        Me.txtCach_tinh.Text = "TXTCONG_THUC"
        '
        'lblCong_thuc
        '
        Me.lblCong_thuc.AutoSize = True
        Me.lblCong_thuc.Location = New System.Drawing.Point(48, 365)
        Me.lblCong_thuc.Name = "lblCong_thuc"
        Me.lblCong_thuc.Size = New System.Drawing.Size(62, 13)
        Me.lblCong_thuc.TabIndex = 61
        Me.lblCong_thuc.Tag = "L612"
        Me.lblCong_thuc.Text = "- Cong thuc"
        '
        'txtForm
        '
        Me.txtForm.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtForm.Location = New System.Drawing.Point(256, 448)
        Me.txtForm.Name = "txtForm"
        Me.txtForm.Size = New System.Drawing.Size(100, 20)
        Me.txtForm.TabIndex = 62
        Me.txtForm.Tag = "FCNBDF"
        Me.txtForm.Visible = False
        '
        'txtTk_co
        '
        Me.txtTk_co.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTk_co.Location = New System.Drawing.Point(155, 258)
        Me.txtTk_co.Name = "txtTk_co"
        Me.txtTk_co.Size = New System.Drawing.Size(160, 20)
        Me.txtTk_co.TabIndex = 9
        Me.txtTk_co.Tag = "FCDF"
        Me.txtTk_co.Text = "TXTTK_CO"
        '
        'lblTk_co
        '
        Me.lblTk_co.AutoSize = True
        Me.lblTk_co.Location = New System.Drawing.Point(48, 260)
        Me.lblTk_co.Name = "lblTk_co"
        Me.lblTk_co.Size = New System.Drawing.Size(94, 13)
        Me.lblTk_co.TabIndex = 64
        Me.lblTk_co.Tag = "L611"
        Me.lblTk_co.Text = "- Cac tai khoan co"
        '
        'txtGiam_tru
        '
        Me.txtGiam_tru.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGiam_tru.Location = New System.Drawing.Point(155, 342)
        Me.txtGiam_tru.Name = "txtGiam_tru"
        Me.txtGiam_tru.Size = New System.Drawing.Size(32, 20)
        Me.txtGiam_tru.TabIndex = 13
        Me.txtGiam_tru.Tag = "FNNBDF"
        Me.txtGiam_tru.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblGiam_tru
        '
        Me.lblGiam_tru.AutoSize = True
        Me.lblGiam_tru.Location = New System.Drawing.Point(48, 344)
        Me.lblGiam_tru.Name = "lblGiam_tru"
        Me.lblGiam_tru.Size = New System.Drawing.Size(68, 13)
        Me.lblGiam_tru.TabIndex = 66
        Me.lblGiam_tru.Tag = "L620"
        Me.lblGiam_tru.Text = "Tinh giam tru"
        '
        'lblCo_khong
        '
        Me.lblCo_khong.AutoSize = True
        Me.lblCo_khong.Location = New System.Drawing.Point(192, 344)
        Me.lblCo_khong.Name = "lblCo_khong"
        Me.lblCo_khong.Size = New System.Drawing.Size(195, 13)
        Me.lblCo_khong.TabIndex = 67
        Me.lblCo_khong.Tag = "L621"
        Me.lblCo_khong.Text = "1 - Tinh giam tru, 0 - Khong tinh giam tru"
        '
        'txtThuyet_minh
        '
        Me.txtThuyet_minh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtThuyet_minh.Location = New System.Drawing.Point(155, 108)
        Me.txtThuyet_minh.Name = "txtThuyet_minh"
        Me.txtThuyet_minh.Size = New System.Drawing.Size(100, 20)
        Me.txtThuyet_minh.TabIndex = 4
        Me.txtThuyet_minh.Tag = "FCDF"
        Me.txtThuyet_minh.Text = "TXTTHUYET_MINH"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 110)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 69
        Me.Label1.Tag = "L622"
        Me.Label1.Text = "Thuyet minh"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 136)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(625, 64)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 200)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(625, 15)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(48, 281)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(53, 13)
        Me.Label12.TabIndex = 89
        Me.Label12.Tag = ""
        Me.Label12.Text = "- Bo phan"
        '
        'txtMa_bp
        '
        Me.txtMa_bp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_bp.Location = New System.Drawing.Point(155, 279)
        Me.txtMa_bp.Name = "txtMa_bp"
        Me.txtMa_bp.Size = New System.Drawing.Size(160, 20)
        Me.txtMa_bp.TabIndex = 10
        Me.txtMa_bp.Tag = "FCDF"
        Me.txtMa_bp.Text = "TXTMA_BP"
        '
        'txtMa_phi
        '
        Me.txtMa_phi.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_phi.Location = New System.Drawing.Point(155, 300)
        Me.txtMa_phi.Name = "txtMa_phi"
        Me.txtMa_phi.Size = New System.Drawing.Size(160, 20)
        Me.txtMa_phi.TabIndex = 11
        Me.txtMa_phi.Tag = "FCDF"
        Me.txtMa_phi.Text = "TXTMA_PHI"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(48, 302)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(28, 13)
        Me.Label11.TabIndex = 92
        Me.Label11.Tag = ""
        Me.Label11.Text = "- Phi"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(48, 323)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(49, 13)
        Me.Label13.TabIndex = 93
        Me.Label13.Tag = ""
        Me.Label13.Text = "- Vu viec"
        '
        'txtMa_vv
        '
        Me.txtMa_vv.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_vv.Location = New System.Drawing.Point(155, 321)
        Me.txtMa_vv.Name = "txtMa_vv"
        Me.txtMa_vv.Size = New System.Drawing.Size(160, 20)
        Me.txtMa_vv.TabIndex = 12
        Me.txtMa_vv.Tag = "FCDF"
        Me.txtMa_vv.Text = "TXTMA_VV"
        '
        'frmDirInfor
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(641, 426)
        Me.Controls.Add(Me.txtMa_phi)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtStt)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtMa_vv)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtMa_bp)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtThuyet_minh)
        Me.Controls.Add(Me.lblCo_khong)
        Me.Controls.Add(Me.lblGiam_tru)
        Me.Controls.Add(Me.txtGiam_tru)
        Me.Controls.Add(Me.lblTk_co)
        Me.Controls.Add(Me.txtTk_co)
        Me.Controls.Add(Me.txtForm)
        Me.Controls.Add(Me.txtCach_tinh)
        Me.Controls.Add(Me.lblCong_thuc)
        Me.Controls.Add(Me.lblCach_tinhMess)
        Me.Controls.Add(Me.txtKind)
        Me.Controls.Add(Me.lblCach_tinh)
        Me.Controls.Add(Me.lblBoldMess)
        Me.Controls.Add(Me.lblInMess)
        Me.Controls.Add(Me.txtBold)
        Me.Controls.Add(Me.lblBold)
        Me.Controls.Add(Me.txtIn_ck)
        Me.Controls.Add(Me.lblIn_ck)
        Me.Controls.Add(Me.txtChi_tieu2)
        Me.Controls.Add(Me.lblChi_tieu2)
        Me.Controls.Add(Me.txtChi_tieu)
        Me.Controls.Add(Me.lblChi_tieu)
        Me.Controls.Add(Me.txtMa_so)
        Me.Controls.Add(Me.lblMa_so)
        Me.Controls.Add(Me.txtTk_no)
        Me.Controls.Add(Me.lblTk_no)
        Me.Controls.Add(Me.lblStt)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.grpInfor)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "frmDirInfor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmDirInfor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub txtKind_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtKind.TextChanged
        If Me.txtKind.Text = "0" Then
            Me.txtCach_tinh.Enabled = True
            Me.txtTk_no.Enabled = False
            Me.txtTk_co.Enabled = False
            Me.txtMa_bp.Enabled = False
            Me.txtMa_phi.Enabled = False
            Me.txtMa_vv.Enabled = False
            Me.txtGiam_tru.Enabled = False
        Else
            Me.txtTk_no.Enabled = True
            Me.txtTk_co.Enabled = True
            Me.txtCach_tinh.Enabled = False
            Me.txtGiam_tru.Enabled = True
            If Me.txtKind.Text = "1" Then
                Me.txtMa_bp.Enabled = True
                Me.txtMa_phi.Enabled = True
                Me.txtMa_vv.Enabled = True
            Else
                Me.txtMa_bp.Enabled = False
                Me.txtMa_phi.Enabled = False
                Me.txtMa_vv.Enabled = False
            End If
        End If
        If Not Me.txtTk_no.Enabled Then
            Me.txtTk_no.Text = ""
        End If
        If Not Me.txtTk_co.Enabled Then
            Me.txtTk_co.Text = ""
        End If
        If Not Me.txtMa_bp.Enabled Then
            Me.txtMa_bp.Text = ""
        End If
        If Not Me.txtMa_phi.Enabled Then
            Me.txtMa_phi.Text = ""
        End If
        If Not Me.txtMa_vv.Enabled Then
            Me.txtMa_vv.Text = ""
        End If
        If Not Me.txtCach_tinh.Enabled Then
            Me.txtCach_tinh.Text = ""
        End If
        If Not Me.txtGiam_tru.Enabled Then
            Me.txtGiam_tru.Text = "0"
        End If
        If Me.txtTk_no.Enabled Then
            Me.txtTk_TextChanged(Me.txtTk_no, New EventArgs)
        End If
    End Sub

    Private Sub txtTk_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        If BooleanType.FromObject(LateBinding.LateGet(sender, Nothing, "Enabled", New Object(0 - 1) {}, Nothing, Nothing)) Then
            If ((StringType.StrCmp(Strings.Trim(Me.txtTk_no.Text), "", False) <> 0) And (StringType.StrCmp(Strings.Trim(Me.txtTk_co.Text), "", False) <> 0)) Then
                Me.txtGiam_tru.Text = "0"
                Me.txtGiam_tru.Enabled = False
            Else
                Me.txtGiam_tru.Enabled = True
            End If
        Else
            Me.txtGiam_tru.Text = "0"
            Me.txtGiam_tru.Enabled = False
        End If
    End Sub
End Class

