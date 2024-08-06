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
Namespace arso1t
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
                DirMain.strAccount = Strings.Trim(DirMain.fPrint.txtTk.Text)
                DirMain.strAccountName = Strings.Trim(DirMain.fPrint.lblTen_tk.Text)
                DirMain.strUnit = Strings.Trim(Me.txtMa_dvcs.Text)
                DirMain.dFrom = Me.txtDFrom.Value
                DirMain.dTo = Me.txtDTo.Value
                Reg.SetRegistryKey("DFDFrom", Me.txtDFrom.Value)
                Reg.SetRegistryKey("DFDTo", Me.txtDTo.Value)
                Reg.SetRegistryKey("DFAccount", Strings.Trim(Me.txtTk.Text))
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
            reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 10)
            Me.txtTk.Text = StringType.FromObject(Reg.GetRegistryKey("DFAccount"))
            Dim oAcc As New DirLib(Me.txtTk, Me.lblTen_tk, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "tk_cn = 1", False, Me.cmdCancel)
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Me.txtTk.Text = StringType.FromObject(Reg.GetRegistryKey("DFAccount"))
            Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", DirMain.strKeyCust, True, Me.cmdCancel)
            Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_nh1, Me.lblTen_nh1, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_nh2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=2", True, Me.cmdCancel)
            Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_nh3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=3", True, Me.cmdCancel)
            Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, DirMain.sysConn, DirMain.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, DirMain.sysConn, DirMain.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, DirMain.sysConn, DirMain.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
            Dim lib2 As New CharLib(Me.txtKieu, "1,2")
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
            Me.txtKieu.Text = StringType.FromInteger(2)
        End Sub

        <DebuggerStepThrough()> _
  Private Sub InitializeComponent()
            Me.txtMa_dvcs = New TextBox
            Me.lblMa_dvcs = New Label
            Me.lblTen_dvcs = New Label
            Me.cmdOk = New Button
            Me.cmdCancel = New Button
            Me.tabReports = New TabControl
            Me.tbgFilter = New TabPage
            Me.Label5 = New Label
            Me.txtKieu = New TextBox
            Me.Label4 = New Label
            Me.lblTen_nh3 = New Label
            Me.lblTen_nh2 = New Label
            Me.lblTen_nh1 = New Label
            Me.Label3 = New Label
            Me.Label2 = New Label
            Me.txtMa_nh3 = New TextBox
            Me.txtMa_nh2 = New TextBox
            Me.txtMa_nh1 = New TextBox
            Me.Label1 = New Label
            Me.lblTen_kh = New Label
            Me.lblTen_tk = New Label
            Me.txtMa_kh = New TextBox
            Me.txtTk = New TextBox
            Me.lblTk_co = New Label
            Me.lblTk_no = New Label
            Me.txtDTo = New txtDate
            Me.txtDFrom = New txtDate
            Me.lblDateFromTo = New Label
            Me.lblMau_bc = New Label
            Me.cboReports = New ComboBox
            Me.lblTitle = New Label
            Me.txtTitle = New TextBox
            Me.tbgOptions = New TabPage
            Me.tbgFree = New TabPage
            Me.lblMa_td1 = New Label
            Me.txtMa_td1 = New TextBox
            Me.txtMa_td2 = New TextBox
            Me.txtMa_td3 = New TextBox
            Me.lblTen_td2 = New Label
            Me.lblTen_td3 = New Label
            Me.lblMa_td3 = New Label
            Me.lblMa_td2 = New Label
            Me.lblTen_td1 = New Label
            Me.tbgOther = New TabPage
            Me.tabReports.SuspendLayout()
            Me.tbgFilter.SuspendLayout()
            Me.tbgFree.SuspendLayout()
            Me.SuspendLayout()
            Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New Point(160, 174)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.TabIndex = 8
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New Point(20, 176)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New Size(36, 16)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L102"
            Me.lblMa_dvcs.Text = "Don vi"
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New Point(264, 176)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New Size(50, 16)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(3, 284)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 0
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.Location = New Point(79, 284)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 1
            Me.cmdCancel.Tag = "L002"
            Me.cmdCancel.Text = "Huy"
            Me.tabReports.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
            Me.tabReports.Controls.Add(Me.tbgFilter)
            Me.tabReports.Controls.Add(Me.tbgOptions)
            Me.tabReports.Controls.Add(Me.tbgFree)
            Me.tabReports.Controls.Add(Me.tbgOther)
            Me.tabReports.Location = New Point(-2, 0)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New Size(609, 272)
            Me.tabReports.TabIndex = 0
            Me.tabReports.Tag = ""
            Me.tbgFilter.Controls.Add(Me.Label5)
            Me.tbgFilter.Controls.Add(Me.txtKieu)
            Me.tbgFilter.Controls.Add(Me.Label4)
            Me.tbgFilter.Controls.Add(Me.lblTen_nh3)
            Me.tbgFilter.Controls.Add(Me.lblTen_nh2)
            Me.tbgFilter.Controls.Add(Me.lblTen_nh1)
            Me.tbgFilter.Controls.Add(Me.Label3)
            Me.tbgFilter.Controls.Add(Me.Label2)
            Me.tbgFilter.Controls.Add(Me.txtMa_nh3)
            Me.tbgFilter.Controls.Add(Me.txtMa_nh2)
            Me.tbgFilter.Controls.Add(Me.txtMa_nh1)
            Me.tbgFilter.Controls.Add(Me.Label1)
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
            Me.tbgFilter.Location = New Point(4, 22)
            Me.tbgFilter.Name = "tbgFilter"
            Me.tbgFilter.Size = New Size(601, 246)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
            Me.Label5.AutoSize = True
            Me.Label5.Location = New Point(198, 153)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New Size(142, 16)
            Me.Label5.TabIndex = 25
            Me.Label5.Tag = "L111"
            Me.Label5.Text = "1-In tung trang, 2-In lien tuc"
            Me.txtKieu.CharacterCasing = CharacterCasing.Upper
            Me.txtKieu.Location = New Point(160, 151)
            Me.txtKieu.MaxLength = 1
            Me.txtKieu.Name = "txtKieu"
            Me.txtKieu.Size = New Size(32, 20)
            Me.txtKieu.TabIndex = 7
            Me.txtKieu.Tag = "FCNBML"
            Me.txtKieu.Text = "0"
            Me.Label4.AutoSize = True
            Me.Label4.Location = New Point(20, 153)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New Size(70, 16)
            Me.Label4.TabIndex = 23
            Me.Label4.Tag = "L110"
            Me.Label4.Text = "Kieu bao cao"
            Me.lblTen_nh3.AutoSize = True
            Me.lblTen_nh3.Location = New Point(264, 130)
            Me.lblTen_nh3.Name = "lblTen_nh3"
            Me.lblTen_nh3.Size = New Size(98, 16)
            Me.lblTen_nh3.TabIndex = 22
            Me.lblTen_nh3.Tag = "RF"
            Me.lblTen_nh3.Text = "Ten nhom khach 3"
            Me.lblTen_nh2.AutoSize = True
            Me.lblTen_nh2.Location = New Point(264, 107)
            Me.lblTen_nh2.Name = "lblTen_nh2"
            Me.lblTen_nh2.Size = New Size(98, 16)
            Me.lblTen_nh2.TabIndex = 21
            Me.lblTen_nh2.Tag = "RF"
            Me.lblTen_nh2.Text = "Ten nhom khach 2"
            Me.lblTen_nh1.AutoSize = True
            Me.lblTen_nh1.Location = New Point(264, 84)
            Me.lblTen_nh1.Name = "lblTen_nh1"
            Me.lblTen_nh1.Size = New Size(98, 16)
            Me.lblTen_nh1.TabIndex = 20
            Me.lblTen_nh1.Tag = "RF"
            Me.lblTen_nh1.Text = "Ten nhom khach 1"
            Me.Label3.AutoSize = True
            Me.Label3.Location = New Point(20, 130)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New Size(77, 16)
            Me.Label3.TabIndex = 19
            Me.Label3.Tag = "L109"
            Me.Label3.Text = "Nhom khach 3"
            Me.Label2.AutoSize = True
            Me.Label2.Location = New Point(20, 107)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New Size(77, 16)
            Me.Label2.TabIndex = 18
            Me.Label2.Tag = "L108"
            Me.Label2.Text = "Nhom khach 2"
            Me.txtMa_nh3.Location = New Point(160, 128)
            Me.txtMa_nh3.Name = "txtMa_nh3"
            Me.txtMa_nh3.TabIndex = 6
            Me.txtMa_nh3.Tag = "FCML"
            Me.txtMa_nh3.Text = "txtMa_nh3"
            Me.txtMa_nh2.Location = New Point(160, 105)
            Me.txtMa_nh2.Name = "txtMa_nh2"
            Me.txtMa_nh2.TabIndex = 5
            Me.txtMa_nh2.Tag = "FCML"
            Me.txtMa_nh2.Text = "txtMa_nh2"
            Me.txtMa_nh1.Location = New Point(160, 82)
            Me.txtMa_nh1.Name = "txtMa_nh1"
            Me.txtMa_nh1.TabIndex = 4
            Me.txtMa_nh1.Tag = "FCML"
            Me.txtMa_nh1.Text = "txtMa_nh1"
            Me.Label1.AutoSize = True
            Me.Label1.Location = New Point(20, 84)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New Size(77, 16)
            Me.Label1.TabIndex = 14
            Me.Label1.Tag = "L107"
            Me.Label1.Text = "Nhom khach 1"
            Me.lblTen_kh.AutoSize = True
            Me.lblTen_kh.Location = New Point(264, 61)
            Me.lblTen_kh.Name = "lblTen_kh"
            Me.lblTen_kh.Size = New Size(85, 16)
            Me.lblTen_kh.TabIndex = 13
            Me.lblTen_kh.Tag = "RF"
            Me.lblTen_kh.Text = "Ten khach hang"
            Me.lblTen_tk.AutoSize = True
            Me.lblTen_tk.Location = New Point(264, 15)
            Me.lblTen_tk.Name = "lblTen_tk"
            Me.lblTen_tk.Size = New Size(73, 16)
            Me.lblTen_tk.TabIndex = 12
            Me.lblTen_tk.Tag = "RF"
            Me.lblTen_tk.Text = "Ten tai khoan"
            Me.txtMa_kh.Location = New Point(160, 59)
            Me.txtMa_kh.Name = "txtMa_kh"
            Me.txtMa_kh.TabIndex = 3
            Me.txtMa_kh.Tag = "FCML"
            Me.txtMa_kh.Text = "txtMa_kh"
            Me.txtTk.Location = New Point(160, 13)
            Me.txtTk.Name = "txtTk"
            Me.txtTk.TabIndex = 0
            Me.txtTk.Tag = "FCNBDF"
            Me.txtTk.Text = "txtTk"
            Me.lblTk_co.AutoSize = True
            Me.lblTk_co.Location = New Point(20, 61)
            Me.lblTk_co.Name = "lblTk_co"
            Me.lblTk_co.Size = New Size(65, 16)
            Me.lblTk_co.TabIndex = 11
            Me.lblTk_co.Tag = "L106"
            Me.lblTk_co.Text = "Khach hang"
            Me.lblTk_no.AutoSize = True
            Me.lblTk_no.Location = New Point(20, 15)
            Me.lblTk_no.Name = "lblTk_no"
            Me.lblTk_no.Size = New Size(54, 16)
            Me.lblTk_no.TabIndex = 10
            Me.lblTk_no.Tag = "L105"
            Me.lblTk_no.Text = "Tai khoan"
            Me.txtDTo.Location = New Point(264, 36)
            Me.txtDTo.MaxLength = 10
            Me.txtDTo.Name = "txtDTo"
            Me.txtDTo.TabIndex = 2
            Me.txtDTo.Tag = "NB"
            Me.txtDTo.Text = "  /  /    "
            Me.txtDTo.TextAlign = HorizontalAlignment.Right
            Me.txtDTo.Value = New DateTime(0)
            Me.txtDFrom.Location = New Point(160, 36)
            Me.txtDFrom.MaxLength = 10
            Me.txtDFrom.Name = "txtDFrom"
            Me.txtDFrom.TabIndex = 1
            Me.txtDFrom.Tag = "NB"
            Me.txtDFrom.Text = "  /  /    "
            Me.txtDFrom.TextAlign = HorizontalAlignment.Right
            Me.txtDFrom.Value = New DateTime(0)
            Me.lblDateFromTo.AutoSize = True
            Me.lblDateFromTo.Location = New Point(20, 38)
            Me.lblDateFromTo.Name = "lblDateFromTo"
            Me.lblDateFromTo.Size = New Size(67, 16)
            Me.lblDateFromTo.TabIndex = 0
            Me.lblDateFromTo.Tag = "L101"
            Me.lblDateFromTo.Text = "Tu/den ngay"
            Me.lblMau_bc.AutoSize = True
            Me.lblMau_bc.Location = New Point(20, 199)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New Size(69, 16)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L103"
            Me.lblMau_bc.Text = "Mau bao cao"
            Me.cboReports.Location = New Point(160, 197)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New Size(300, 21)
            Me.cboReports.TabIndex = 9
            Me.cboReports.Text = "cboReports"
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New Point(20, 223)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(42, 16)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L104"
            Me.lblTitle.Text = "Tieu de"
            Me.txtTitle.Location = New Point(160, 221)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New Size(300, 20)
            Me.txtTitle.TabIndex = 10
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            Me.tbgOptions.Location = New Point(4, 22)
            Me.tbgOptions.Name = "tbgOptions"
            Me.tbgOptions.Size = New Size(601, 246)
            Me.tbgOptions.TabIndex = 1
            Me.tbgOptions.Tag = "L200"
            Me.tbgOptions.Text = "Lua chon"
            Me.tbgFree.Controls.Add(Me.lblMa_td1)
            Me.tbgFree.Controls.Add(Me.txtMa_td1)
            Me.tbgFree.Controls.Add(Me.txtMa_td2)
            Me.tbgFree.Controls.Add(Me.txtMa_td3)
            Me.tbgFree.Controls.Add(Me.lblTen_td2)
            Me.tbgFree.Controls.Add(Me.lblTen_td3)
            Me.tbgFree.Controls.Add(Me.lblMa_td3)
            Me.tbgFree.Controls.Add(Me.lblMa_td2)
            Me.tbgFree.Controls.Add(Me.lblTen_td1)
            Me.tbgFree.Location = New Point(4, 22)
            Me.tbgFree.Name = "tbgFree"
            Me.tbgFree.Size = New Size(601, 246)
            Me.tbgFree.TabIndex = 2
            Me.tbgFree.Tag = "FreeReportCaption"
            Me.tbgFree.Text = "Dieu kien ma tu do"
            Me.lblMa_td1.AutoSize = True
            Me.lblMa_td1.Location = New Point(20, 16)
            Me.lblMa_td1.Name = "lblMa_td1"
            Me.lblMa_td1.Size = New Size(57, 16)
            Me.lblMa_td1.TabIndex = 82
            Me.lblMa_td1.Tag = "FreeCaption1"
            Me.lblMa_td1.Text = "Ma tu do 1"
            Me.txtMa_td1.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_td1.Location = New Point(160, 12)
            Me.txtMa_td1.Name = "txtMa_td1"
            Me.txtMa_td1.TabIndex = 79
            Me.txtMa_td1.Tag = "FCDetail#ma_td1 like '%s%'#ML"
            Me.txtMa_td1.Text = "TXTMA_TD1"
            Me.txtMa_td2.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_td2.Location = New Point(160, 35)
            Me.txtMa_td2.Name = "txtMa_td2"
            Me.txtMa_td2.TabIndex = 80
            Me.txtMa_td2.Tag = "FCDetail#ma_td2 like '%s%'#ML"
            Me.txtMa_td2.Text = "TXTMA_TD2"
            Me.txtMa_td3.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_td3.Location = New Point(160, 58)
            Me.txtMa_td3.Name = "txtMa_td3"
            Me.txtMa_td3.TabIndex = 81
            Me.txtMa_td3.Tag = "FCDetail#ma_td3 like '%s%'#ML"
            Me.txtMa_td3.Text = "TXTMA_TD3"
            Me.lblTen_td2.AutoSize = True
            Me.lblTen_td2.Location = New Point(272, 39)
            Me.lblTen_td2.Name = "lblTen_td2"
            Me.lblTen_td2.Size = New Size(61, 16)
            Me.lblTen_td2.TabIndex = 86
            Me.lblTen_td2.Tag = ""
            Me.lblTen_td2.Text = "Ten tu do 2"
            Me.lblTen_td3.AutoSize = True
            Me.lblTen_td3.Location = New Point(272, 62)
            Me.lblTen_td3.Name = "lblTen_td3"
            Me.lblTen_td3.Size = New Size(61, 16)
            Me.lblTen_td3.TabIndex = 87
            Me.lblTen_td3.Tag = ""
            Me.lblTen_td3.Text = "Ten tu do 3"
            Me.lblMa_td3.AutoSize = True
            Me.lblMa_td3.Location = New Point(20, 62)
            Me.lblMa_td3.Name = "lblMa_td3"
            Me.lblMa_td3.Size = New Size(57, 16)
            Me.lblMa_td3.TabIndex = 84
            Me.lblMa_td3.Tag = "FreeCaption3"
            Me.lblMa_td3.Text = "Ma tu do 3"
            Me.lblMa_td2.AutoSize = True
            Me.lblMa_td2.Location = New Point(20, 39)
            Me.lblMa_td2.Name = "lblMa_td2"
            Me.lblMa_td2.Size = New Size(57, 16)
            Me.lblMa_td2.TabIndex = 83
            Me.lblMa_td2.Tag = "FreeCaption2"
            Me.lblMa_td2.Text = "Ma tu do 2"
            Me.lblTen_td1.AutoSize = True
            Me.lblTen_td1.Location = New Point(272, 16)
            Me.lblTen_td1.Name = "lblTen_td1"
            Me.lblTen_td1.Size = New Size(61, 16)
            Me.lblTen_td1.TabIndex = 85
            Me.lblTen_td1.Tag = ""
            Me.lblTen_td1.Text = "Ten tu do 1"
            Me.tbgOther.Location = New Point(4, 22)
            Me.tbgOther.Name = "tbgOther"
            Me.tbgOther.Size = New Size(601, 246)
            Me.tbgOther.TabIndex = 3
            Me.tbgOther.Tag = "FreeReportOther"
            Me.tbgOther.Text = "Dieu kien khac"
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(608, 341)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.cmdCancel)
            Me.Controls.Add(Me.cmdOk)
            Me.Name = "frmFilter"
            Me.StartPosition = FormStartPosition.CenterParent
            Me.Text = "frmFilter"
            Me.tabReports.ResumeLayout(False)
            Me.tbgFilter.ResumeLayout(False)
            Me.tbgFree.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents Label1 As Label
        Friend WithEvents Label2 As Label
        Friend WithEvents Label3 As Label
        Friend WithEvents Label4 As Label
        Friend WithEvents Label5 As Label
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_td1 As Label
        Friend WithEvents lblMa_td2 As Label
        Friend WithEvents lblMa_td3 As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblTen_dvcs As Label
        Friend WithEvents lblTen_kh As Label
        Friend WithEvents lblTen_nh1 As Label
        Friend WithEvents lblTen_nh2 As Label
        Friend WithEvents lblTen_nh3 As Label
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
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents txtDTo As txtDate
        Friend WithEvents txtKieu As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents txtMa_nh1 As TextBox
        Friend WithEvents txtMa_nh2 As TextBox
        Friend WithEvents txtMa_nh3 As TextBox
        Friend WithEvents txtMa_td1 As TextBox
        Friend WithEvents txtMa_td2 As TextBox
        Friend WithEvents txtMa_td3 As TextBox
        Friend WithEvents txtTitle As TextBox
        Friend WithEvents txtTk As TextBox

        Private components As IContainer
        Public pnContent As StatusBarPanel
    End Class
End Namespace

