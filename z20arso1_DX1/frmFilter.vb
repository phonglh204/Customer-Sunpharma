Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib

Namespace arso1
    Public Class frmFilter
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.InitializeComponent
        End Sub

        Private Sub cboReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboReports.SelectedIndexChanged
            If Not Information.IsNothing(DirMain.rpTable) Then
                Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(Me.cboReports.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            End If
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Me.Close()
        End Sub

        Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
            If reportformlib.CheckEmptyField(Me, Me.tabReports, DirMain.oVar) Then
                DirMain.strAccount = Strings.Trim(Me.txtTk.Text)
                DirMain.strAccountName = Strings.Trim(Me.lblTen_tk.Text)
                DirMain.strCustID = Strings.Trim(Me.txtMa_kh.Text)
                DirMain.strCustName = Strings.Trim(Me.lblTen_kh.Text)
                DirMain.strUnit = Strings.Trim(Me.txtMa_dvcs.Text)
                DirMain.dFrom = Me.txtDFrom.Value
                DirMain.dTo = Me.txtDTo.Value
                Reg.SetRegistryKey("DFDFrom", Me.txtDFrom.Value)
                Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
                Reg.SetRegistryKey("DFAccount", Strings.Trim(Me.txtTk.Text))
                Reg.SetRegistryKey("DFCustomer", Strings.Trim(Me.txtMa_kh.Text))
                Me.pnContent.Text = StringType.FromObject(DirMain.oVar.Item("m_process"))
                DirMain.ShowReport()
                Dim document As New PrintDocument
                Me.pnContent.Text = document.PrinterSettings.PrinterName
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
            reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 7)
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Dim oTk As New DirLib(Me.txtTk, Me.lblTen_tk, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "tk_cn = 1", False, Me.cmdCancel)
            Dim lib2 As New DirLib(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", DirMain.strKeyCust, False, Me.cmdCancel)
            Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, DirMain.sysConn, DirMain.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, DirMain.sysConn, DirMain.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, DirMain.sysConn, DirMain.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
            Dim lib3 As New CharLib(Me.txtChi_tiet, "0,1")
            Dim Group_voucher As New CharLib(Me.txtGroup_voucher, "0,1")
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.tabReports.TabPages.Remove(Me.tbgFree)
            Me.tabReports.TabPages.Remove(Me.tbgOther)
            Me.tabReports.TabPages.Remove(Me.tbgOptions)
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
            Me.txtTk.Text = StringType.FromObject(Reg.GetRegistryKey("DFAccount"))
            Me.txtMa_kh.Text = StringType.FromObject(Reg.GetRegistryKey("DFCustomer"))
            Me.txtChi_tiet.Text = StringType.FromInteger(0)
            If (StringType.StrCmp(Me.txtTk.Text, "", False) <> 0) Then
                Me.lblTen_tk.Text = StringType.FromObject(Sql.GetValue((DirMain.appConn), "dmtk", StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("language"), "V", False) = 0), "ten_tk", "ten_tk2")), ("tk = '" & Me.txtTk.Text.Trim & "'")))
            End If
            If (StringType.StrCmp(Me.txtMa_kh.Text, "", False) <> 0) Then
                Me.lblTen_kh.Text = StringType.FromObject(Sql.GetValue((DirMain.appConn), "dmkh", StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("language"), "V", False) = 0), "ten_kh", "ten_kh2")), ("ma_kh = '" & Me.txtMa_kh.Text.Trim & "'")))
            End If
        End Sub

        <DebuggerStepThrough()> _
Private Sub InitializeComponent()
            Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
            Me.lblMa_dvcs = New System.Windows.Forms.Label()
            Me.lblTen_dvcs = New System.Windows.Forms.Label()
            Me.cmdOk = New System.Windows.Forms.Button()
            Me.cmdCancel = New System.Windows.Forms.Button()
            Me.tabReports = New System.Windows.Forms.TabControl()
            Me.tbgFilter = New System.Windows.Forms.TabPage()
            Me.lblCo_khong = New System.Windows.Forms.Label()
            Me.lblGiam_tru = New System.Windows.Forms.Label()
            Me.txtChi_tiet = New System.Windows.Forms.TextBox()
            Me.lblTen_kh = New System.Windows.Forms.Label()
            Me.lblTen_tk = New System.Windows.Forms.Label()
            Me.txtMa_kh = New System.Windows.Forms.TextBox()
            Me.txtTk = New System.Windows.Forms.TextBox()
            Me.lblTk_co = New System.Windows.Forms.Label()
            Me.lblTk_no = New System.Windows.Forms.Label()
            Me.txtDTo = New libscontrol.txtDate()
            Me.txtDFrom = New libscontrol.txtDate()
            Me.lblDateFromTo = New System.Windows.Forms.Label()
            Me.lblMau_bc = New System.Windows.Forms.Label()
            Me.cboReports = New System.Windows.Forms.ComboBox()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.txtTitle = New System.Windows.Forms.TextBox()
            Me.tbgOptions = New System.Windows.Forms.TabPage()
            Me.tbgFree = New System.Windows.Forms.TabPage()
            Me.lblMa_td1 = New System.Windows.Forms.Label()
            Me.txtMa_td1 = New System.Windows.Forms.TextBox()
            Me.txtMa_td2 = New System.Windows.Forms.TextBox()
            Me.txtMa_td3 = New System.Windows.Forms.TextBox()
            Me.lblTen_td2 = New System.Windows.Forms.Label()
            Me.lblTen_td3 = New System.Windows.Forms.Label()
            Me.lblMa_td3 = New System.Windows.Forms.Label()
            Me.lblMa_td2 = New System.Windows.Forms.Label()
            Me.lblTen_td1 = New System.Windows.Forms.Label()
            Me.tbgOther = New System.Windows.Forms.TabPage()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.txtGroup_voucher = New System.Windows.Forms.TextBox()
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.tbgFree.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtMa_dvcs
            '
            Me.txtMa_dvcs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New System.Drawing.Point(160, 128)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_dvcs.TabIndex = 6
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            '
            'lblMa_dvcs
            '
            Me.lblMa_dvcs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New System.Drawing.Point(20, 130)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New System.Drawing.Size(38, 13)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L102"
            Me.lblMa_dvcs.Text = "Don vi"
            '
            'lblTen_dvcs
            '
            Me.lblTen_dvcs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New System.Drawing.Point(264, 130)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New System.Drawing.Size(52, 13)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            '
            'cmdOk
            '
            Me.cmdOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdOk.Location = New System.Drawing.Point(3, 241)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.Size = New System.Drawing.Size(75, 23)
            Me.cmdOk.TabIndex = 1
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            '
            'cmdCancel
            '
            Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cmdCancel.Location = New System.Drawing.Point(79, 241)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
            Me.cmdCancel.TabIndex = 2
            Me.cmdCancel.Tag = "L002"
            Me.cmdCancel.Text = "Huy"
            '
            'tabReports
            '
            Me.tabReports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tabReports.Controls.Add(Me.tbgFilter)
            Me.tabReports.Controls.Add(Me.tbgOptions)
            Me.tabReports.Controls.Add(Me.tbgFree)
            Me.tabReports.Controls.Add(Me.tbgOther)
            Me.tabReports.Location = New System.Drawing.Point(-2, 0)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New System.Drawing.Size(609, 229)
            Me.tabReports.TabIndex = 0
            Me.tabReports.Tag = ""
            '
            'tbgFilter
            '
            Me.tbgFilter.Controls.Add(Me.Label1)
            Me.tbgFilter.Controls.Add(Me.Label2)
            Me.tbgFilter.Controls.Add(Me.txtGroup_voucher)
            Me.tbgFilter.Controls.Add(Me.lblCo_khong)
            Me.tbgFilter.Controls.Add(Me.lblGiam_tru)
            Me.tbgFilter.Controls.Add(Me.txtChi_tiet)
            Me.tbgFilter.Controls.Add(Me.lblTen_kh)
            Me.tbgFilter.Controls.Add(Me.lblTen_tk)
            Me.tbgFilter.Controls.Add(Me.txtMa_kh)
            Me.tbgFilter.Controls.Add(Me.txtTk)
            Me.tbgFilter.Controls.Add(Me.lblTk_co)
            Me.tbgFilter.Controls.Add(Me.lblTk_no)
            Me.tbgFilter.Controls.Add(Me.txtDTo)
            Me.tbgFilter.Controls.Add(Me.txtDFrom)
            Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
            Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
            Me.tbgFilter.Controls.Add(Me.lblMau_bc)
            Me.tbgFilter.Controls.Add(Me.cboReports)
            Me.tbgFilter.Controls.Add(Me.lblTitle)
            Me.tbgFilter.Controls.Add(Me.txtTitle)
            Me.tbgFilter.Location = New System.Drawing.Point(4, 22)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New System.Drawing.Size(601, 203)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            '
            'lblCo_khong
            '
            Me.lblCo_khong.AutoSize = True
            Me.lblCo_khong.Location = New System.Drawing.Point(198, 84)
            Me.lblCo_khong.Name = "lblCo_khong"
            Me.lblCo_khong.Size = New System.Drawing.Size(121, 13)
            Me.lblCo_khong.TabIndex = 70
            Me.lblCo_khong.Tag = "L108"
            Me.lblCo_khong.Text = "0 - Khong, 1 - Co chi tiet"
            '
            'lblGiam_tru
            '
            Me.lblGiam_tru.AutoSize = True
            Me.lblGiam_tru.Location = New System.Drawing.Point(20, 84)
            Me.lblGiam_tru.Name = "lblGiam_tru"
            Me.lblGiam_tru.Size = New System.Drawing.Size(78, 13)
            Me.lblGiam_tru.TabIndex = 69
            Me.lblGiam_tru.Tag = "L107"
            Me.lblGiam_tru.Text = "Chi tiet theo hh"
            '
            'txtChi_tiet
            '
            Me.txtChi_tiet.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtChi_tiet.Location = New System.Drawing.Point(160, 82)
            Me.txtChi_tiet.MaxLength = 1
            Me.txtChi_tiet.Name = "txtChi_tiet"
            Me.txtChi_tiet.Size = New System.Drawing.Size(32, 20)
            Me.txtChi_tiet.TabIndex = 4
            Me.txtChi_tiet.Tag = "FCNBML"
            Me.txtChi_tiet.Text = "0"
            '
            'lblTen_kh
            '
            Me.lblTen_kh.AutoSize = True
            Me.lblTen_kh.Location = New System.Drawing.Point(264, 38)
            Me.lblTen_kh.Name = "lblTen_kh"
            Me.lblTen_kh.Size = New System.Drawing.Size(86, 13)
            Me.lblTen_kh.TabIndex = 13
            Me.lblTen_kh.Tag = "RF"
            Me.lblTen_kh.Text = "Ten khach hang"
            '
            'lblTen_tk
            '
            Me.lblTen_tk.AutoSize = True
            Me.lblTen_tk.Location = New System.Drawing.Point(264, 15)
            Me.lblTen_tk.Name = "lblTen_tk"
            Me.lblTen_tk.Size = New System.Drawing.Size(73, 13)
            Me.lblTen_tk.TabIndex = 12
            Me.lblTen_tk.Tag = "RF"
            Me.lblTen_tk.Text = "Ten tai khoan"
            '
            'txtMa_kh
            '
            Me.txtMa_kh.Location = New System.Drawing.Point(160, 36)
            Me.txtMa_kh.Name = "txtMa_kh"
            Me.txtMa_kh.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_kh.TabIndex = 1
            Me.txtMa_kh.Tag = "FCNBDF"
            Me.txtMa_kh.Text = "txtMa_kh"
            '
            'txtTk
            '
            Me.txtTk.Location = New System.Drawing.Point(160, 13)
            Me.txtTk.Name = "txtTk"
            Me.txtTk.Size = New System.Drawing.Size(100, 20)
            Me.txtTk.TabIndex = 0
            Me.txtTk.Tag = "FCNBDF"
            Me.txtTk.Text = "txtTk"
            '
            'lblTk_co
            '
            Me.lblTk_co.AutoSize = True
            Me.lblTk_co.Location = New System.Drawing.Point(20, 38)
            Me.lblTk_co.Name = "lblTk_co"
            Me.lblTk_co.Size = New System.Drawing.Size(65, 13)
            Me.lblTk_co.TabIndex = 11
            Me.lblTk_co.Tag = "L106"
            Me.lblTk_co.Text = "Khach hang"
            '
            'lblTk_no
            '
            Me.lblTk_no.AutoSize = True
            Me.lblTk_no.Location = New System.Drawing.Point(20, 15)
            Me.lblTk_no.Name = "lblTk_no"
            Me.lblTk_no.Size = New System.Drawing.Size(55, 13)
            Me.lblTk_no.TabIndex = 10
            Me.lblTk_no.Tag = "L105"
            Me.lblTk_no.Text = "Tai khoan"
            '
            'txtDTo
            '
            Me.txtDTo.Location = New System.Drawing.Point(264, 59)
            Me.txtDTo.MaxLength = 10
            Me.txtDTo.Name = "txtDTo"
            Me.txtDTo.Size = New System.Drawing.Size(100, 20)
            Me.txtDTo.TabIndex = 3
            Me.txtDTo.Tag = "NB"
            Me.txtDTo.Text = "  /  /    "
            Me.txtDTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDTo.Value = New Date(CType(0, Long))
            '
            'txtDFrom
            '
            Me.txtDFrom.Location = New System.Drawing.Point(160, 59)
            Me.txtDFrom.MaxLength = 10
            Me.txtDFrom.Name = "txtDFrom"
            Me.txtDFrom.Size = New System.Drawing.Size(100, 20)
            Me.txtDFrom.TabIndex = 2
            Me.txtDFrom.Tag = "NB"
            Me.txtDFrom.Text = "  /  /    "
            Me.txtDFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.txtDFrom.Value = New Date(CType(0, Long))
            '
            'lblDateFromTo
            '
            Me.lblDateFromTo.AutoSize = True
            Me.lblDateFromTo.Location = New System.Drawing.Point(20, 61)
            Me.lblDateFromTo.Name = "lblDateFromTo"
            Me.lblDateFromTo.Size = New System.Drawing.Size(69, 13)
            Me.lblDateFromTo.TabIndex = 0
            Me.lblDateFromTo.Tag = "L101"
            Me.lblDateFromTo.Text = "Tu/den ngay"
            '
            'lblMau_bc
            '
            Me.lblMau_bc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New System.Drawing.Point(20, 153)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New System.Drawing.Size(70, 13)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L103"
            Me.lblMau_bc.Text = "Mau bao cao"
            '
            'cboReports
            '
            Me.cboReports.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cboReports.Location = New System.Drawing.Point(160, 151)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New System.Drawing.Size(300, 21)
            Me.cboReports.TabIndex = 7
            Me.cboReports.Text = "cboReports"
            '
            'lblTitle
            '
            Me.lblTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New System.Drawing.Point(20, 176)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(43, 13)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L104"
            Me.lblTitle.Text = "Tieu de"
            '
            'txtTitle
            '
            Me.txtTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.txtTitle.Location = New System.Drawing.Point(160, 174)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New System.Drawing.Size(300, 20)
            Me.txtTitle.TabIndex = 8
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            '
            'tbgOptions
            '
            Me.tbgOptions.Location = New System.Drawing.Point(4, 22)
            Me.tbgOptions.Name = "tbgOptions"
            Me.tbgOptions.Size = New System.Drawing.Size(601, 187)
            Me.tbgOptions.TabIndex = 1
            Me.tbgOptions.Tag = "L200"
            Me.tbgOptions.Text = "Lua chon"
            '
            'tbgFree
            '
            Me.tbgFree.Controls.Add(Me.lblMa_td1)
            Me.tbgFree.Controls.Add(Me.txtMa_td1)
            Me.tbgFree.Controls.Add(Me.txtMa_td2)
            Me.tbgFree.Controls.Add(Me.txtMa_td3)
            Me.tbgFree.Controls.Add(Me.lblTen_td2)
            Me.tbgFree.Controls.Add(Me.lblTen_td3)
            Me.tbgFree.Controls.Add(Me.lblMa_td3)
            Me.tbgFree.Controls.Add(Me.lblMa_td2)
            Me.tbgFree.Controls.Add(Me.lblTen_td1)
            Me.tbgFree.Location = New System.Drawing.Point(4, 22)
            Me.tbgFree.Name = "tbgFree"
            Me.tbgFree.Size = New System.Drawing.Size(601, 187)
            Me.tbgFree.TabIndex = 2
            Me.tbgFree.Tag = "FreeReportCaption"
            Me.tbgFree.Text = "Dieu kien ma tu do"
            '
            'lblMa_td1
            '
            Me.lblMa_td1.AutoSize = True
            Me.lblMa_td1.Location = New System.Drawing.Point(20, 16)
            Me.lblMa_td1.Name = "lblMa_td1"
            Me.lblMa_td1.Size = New System.Drawing.Size(58, 13)
            Me.lblMa_td1.TabIndex = 82
            Me.lblMa_td1.Tag = "FreeCaption1"
            Me.lblMa_td1.Text = "Ma tu do 1"
            '
            'txtMa_td1
            '
            Me.txtMa_td1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_td1.Location = New System.Drawing.Point(160, 12)
            Me.txtMa_td1.Name = "txtMa_td1"
            Me.txtMa_td1.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_td1.TabIndex = 79
            Me.txtMa_td1.Tag = "FCDetail#ma_td1 like '%s%'#ML"
            Me.txtMa_td1.Text = "TXTMA_TD1"
            '
            'txtMa_td2
            '
            Me.txtMa_td2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_td2.Location = New System.Drawing.Point(160, 35)
            Me.txtMa_td2.Name = "txtMa_td2"
            Me.txtMa_td2.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_td2.TabIndex = 80
            Me.txtMa_td2.Tag = "FCDetail#ma_td2 like '%s%'#ML"
            Me.txtMa_td2.Text = "TXTMA_TD2"
            '
            'txtMa_td3
            '
            Me.txtMa_td3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtMa_td3.Location = New System.Drawing.Point(160, 58)
            Me.txtMa_td3.Name = "txtMa_td3"
            Me.txtMa_td3.Size = New System.Drawing.Size(100, 20)
            Me.txtMa_td3.TabIndex = 81
            Me.txtMa_td3.Tag = "FCDetail#ma_td3 like '%s%'#ML"
            Me.txtMa_td3.Text = "TXTMA_TD3"
            '
            'lblTen_td2
            '
            Me.lblTen_td2.AutoSize = True
            Me.lblTen_td2.Location = New System.Drawing.Point(272, 39)
            Me.lblTen_td2.Name = "lblTen_td2"
            Me.lblTen_td2.Size = New System.Drawing.Size(62, 13)
            Me.lblTen_td2.TabIndex = 86
            Me.lblTen_td2.Tag = ""
            Me.lblTen_td2.Text = "Ten tu do 2"
            '
            'lblTen_td3
            '
            Me.lblTen_td3.AutoSize = True
            Me.lblTen_td3.Location = New System.Drawing.Point(272, 62)
            Me.lblTen_td3.Name = "lblTen_td3"
            Me.lblTen_td3.Size = New System.Drawing.Size(62, 13)
            Me.lblTen_td3.TabIndex = 87
            Me.lblTen_td3.Tag = ""
            Me.lblTen_td3.Text = "Ten tu do 3"
            '
            'lblMa_td3
            '
            Me.lblMa_td3.AutoSize = True
            Me.lblMa_td3.Location = New System.Drawing.Point(20, 62)
            Me.lblMa_td3.Name = "lblMa_td3"
            Me.lblMa_td3.Size = New System.Drawing.Size(58, 13)
            Me.lblMa_td3.TabIndex = 84
            Me.lblMa_td3.Tag = "FreeCaption3"
            Me.lblMa_td3.Text = "Ma tu do 3"
            '
            'lblMa_td2
            '
            Me.lblMa_td2.AutoSize = True
            Me.lblMa_td2.Location = New System.Drawing.Point(20, 39)
            Me.lblMa_td2.Name = "lblMa_td2"
            Me.lblMa_td2.Size = New System.Drawing.Size(58, 13)
            Me.lblMa_td2.TabIndex = 83
            Me.lblMa_td2.Tag = "FreeCaption2"
            Me.lblMa_td2.Text = "Ma tu do 2"
            '
            'lblTen_td1
            '
            Me.lblTen_td1.AutoSize = True
            Me.lblTen_td1.Location = New System.Drawing.Point(272, 16)
            Me.lblTen_td1.Name = "lblTen_td1"
            Me.lblTen_td1.Size = New System.Drawing.Size(62, 13)
            Me.lblTen_td1.TabIndex = 85
            Me.lblTen_td1.Tag = ""
            Me.lblTen_td1.Text = "Ten tu do 1"
            '
            'tbgOther
            '
            Me.tbgOther.Location = New System.Drawing.Point(4, 22)
            Me.tbgOther.Name = "tbgOther"
            Me.tbgOther.Size = New System.Drawing.Size(601, 187)
            Me.tbgOther.TabIndex = 3
            Me.tbgOther.Tag = "FreeReportOther"
            Me.tbgOther.Text = "Dieu kien khac"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(198, 107)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(121, 13)
            Me.Label1.TabIndex = 73
            Me.Label1.Tag = "L108"
            Me.Label1.Text = "0 - Khong, 1 - Co chi tiet"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(20, 107)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(104, 13)
            Me.Label2.TabIndex = 72
            Me.Label2.Tag = ""
            Me.Label2.Text = "Nhóm theo chứng từ"
            '
            'txtGroup_voucher
            '
            Me.txtGroup_voucher.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
            Me.txtGroup_voucher.Location = New System.Drawing.Point(160, 105)
            Me.txtGroup_voucher.MaxLength = 1
            Me.txtGroup_voucher.Name = "txtGroup_voucher"
            Me.txtGroup_voucher.Size = New System.Drawing.Size(32, 20)
            Me.txtGroup_voucher.TabIndex = 5
            Me.txtGroup_voucher.Tag = "FCNBML"
            Me.txtGroup_voucher.Text = "0"
            '
            'frmFilter
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(608, 298)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Name = "frmFilter"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.tabReports.ResumeLayout(False)
            Me.tbgFilter.ResumeLayout(False)
            Me.tbgFilter.PerformLayout()
            Me.tbgFree.ResumeLayout(False)
            Me.tbgFree.PerformLayout()
            Me.ResumeLayout(False)

        End Sub


        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents lblCo_khong As Label
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lblGiam_tru As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_td1 As Label
        Friend WithEvents lblMa_td2 As Label
        Friend WithEvents lblMa_td3 As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_kh As Label
        Friend WithEvents lblTen_td1 As Label
        Friend WithEvents lblTen_td2 As Label
        Friend WithEvents lblTen_td3 As Label
        Friend WithEvents lblTen_tk As Label
        Friend WithEvents lblTitle As Label
        Friend WithEvents lblTk_co As Label
        Friend WithEvents lblTk_no As Label
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents tbgFree As TabPage
        Friend WithEvents tbgOptions As TabPage
        Friend WithEvents tbgOther As TabPage
        Friend WithEvents txtChi_tiet As TextBox
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents txtDTo As txtDate
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents txtMa_td1 As TextBox
        Friend WithEvents txtMa_td2 As TextBox
        Friend WithEvents txtMa_td3 As TextBox
        Friend WithEvents txtTitle As TextBox
        Friend WithEvents txtTk As TextBox


        Private components As IContainer
        Friend WithEvents Label1 As Label
        Friend WithEvents Label2 As Label
        Friend WithEvents txtGroup_voucher As TextBox
        Public pnContent As StatusBarPanel
    End Class
End Namespace

