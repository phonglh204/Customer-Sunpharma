Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Imports libscontrol

Public Class frmDate2
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDate_Load)
        Me.InitializeComponent()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        Me.Close()
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDate_Load(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Me.Text = StringType.FromObject(modVoucher.oLan.Item("300"))
        Dim control As Control
        For Each control In Me.Controls
            If (StringType.StrCmp(Strings.Left(StringType.FromObject(control.Tag), 1), "L", False) = 0) Then
                control.Text = StringType.FromObject(modVoucher.oLan.Item(Strings.Mid(StringType.FromObject(control.Tag), 2, 3)))
            End If
        Next
        Obj.Init(Me)
        Me.txtNgay_ct.AddCalenderControl()
        Me.txtNgay_ct.Value = DateAndTime.Now.Date
        Dim oCustTax As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "kh_yn=1", True, Me.cmdCancel)
    End Sub

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblNgay_ct = New System.Windows.Forms.Label()
        Me.cmdOk = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.grpInfor = New System.Windows.Forms.GroupBox()
        Me.txtNgay_ct = New txtDate()
        Me.txtMa_kh = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTen_kh = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblNgay_ct
        '
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New System.Drawing.Point(23, 23)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New System.Drawing.Size(96, 13)
        Me.lblNgay_ct.TabIndex = 7
        Me.lblNgay_ct.Tag = "L301"
        Me.lblNgay_ct.Text = "Ngay chung tu moi"
        '
        'cmdOk
        '
        Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdOk.Location = New System.Drawing.Point(8, 80)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 2
        Me.cmdOk.Tag = "L302"
        Me.cmdOk.Text = "Nhan"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(84, 80)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Tag = "L303"
        Me.cmdCancel.Text = "Huy"
        '
        'grpInfor
        '
        Me.grpInfor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpInfor.Location = New System.Drawing.Point(8, 8)
        Me.grpInfor.Name = "grpInfor"
        Me.grpInfor.Size = New System.Drawing.Size(592, 66)
        Me.grpInfor.TabIndex = 17
        Me.grpInfor.TabStop = False
        '
        'txtNgay_ct
        '
        Me.txtNgay_ct.Location = New System.Drawing.Point(155, 21)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.Size = New System.Drawing.Size(100, 20)
        Me.txtNgay_ct.TabIndex = 0
        Me.txtNgay_ct.Text = "01/01/1900"
        Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct.Value = New Date(1900, 1, 1, 0, 0, 0, 0)
        '
        'txtMa_kh
        '
        Me.txtMa_kh.BackColor = System.Drawing.Color.White
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(155, 44)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(100, 20)
        Me.txtMa_kh.TabIndex = 1
        Me.txtMa_kh.Tag = "FCCF"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 46)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 136
        Me.Label4.Tag = "L002"
        Me.Label4.Text = "Ma khach"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_kh.AutoSize = True
        Me.lblTen_kh.Location = New System.Drawing.Point(259, 46)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(60, 13)
        Me.lblTen_kh.TabIndex = 137
        Me.lblTen_kh.Tag = "FCRF"
        Me.lblTen_kh.Text = "Ten Khach"
        '
        'frmDate2
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(608, 108)
        Me.Controls.Add(Me.txtMa_kh)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblTen_kh)
        Me.Controls.Add(Me.lblNgay_ct)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Controls.Add(Me.txtNgay_ct)
        Me.Controls.Add(Me.grpInfor)
        Me.Name = "frmDate2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmDate"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub


    ' Properties
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents grpInfor As GroupBox
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents txtNgay_ct As txtDate
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents lblTen_kh As Label

    Private components As IContainer
End Class

