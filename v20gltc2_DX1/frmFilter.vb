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

Public Class frmFilter
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
        Me.dsQuyNam = New DataSet
        Me.InitializeComponent()
    End Sub

    Public Sub AddReportType()
        Dim ds As New DataSet
        Dim tcSQL As String = "SELECT * FROM v20dmmaubc WHERE ma_maubc = 'v20GLTC2' ORDER BY form"
        Sql.SQLRetrieve((DirMain.appConn), tcSQL, "v20GLTC2", (ds))
        DirMain.rpTypeTable = ds.Tables.Item("v20GLTC2")
        Dim num2 As Integer = (DirMain.rpTypeTable.Rows.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            Me.cboReportType.Items.Add(RuntimeHelpers.GetObjectValue(LateBinding.LateGet(DirMain.rpTypeTable.Rows.Item(i), Nothing, "Item", New Object() {RuntimeHelpers.GetObjectValue(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "ten_maubc", "ten_maubc2"))}, Nothing, Nothing)))
            i += 1
        Loop
        Me.cboReportType.SelectedIndex = 0
        If (DirMain.rpTypeTable.Rows.Count = 1) Then
            Me.cboReportType.TabStop = False
        End If
    End Sub

    Private Sub cboReporttype_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboReportType.SelectedIndexChanged
        If Not Information.IsNothing(DirMain.rpTypeTable) Then
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTypeTable.Rows.Item(Me.cboReportType.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj("title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAdd.Click
        DirMain.fReport = New frmReport
        DirMain.fReport.Left = CInt(Math.Round(CDbl((((Me.Left + Me.cboReportType.Left) + (CDbl(Me.cboReportType.Width) / 2)) - (CDbl(DirMain.fReport.Width) / 2)))))
        DirMain.fReport.Top = (((Me.Top + Me.cboReportType.Top) + Me.cboReportType.Height) + 40)
        DirMain.fReport.StartPosition = FormStartPosition.Manual
        DirMain.fReport.ShowDialog()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOk.Click
        If reportformlib.CheckEmptyField(Me, Me.tabReports, DirMain.oVar) Then
            DirMain.strUnit = Strings.Trim(Me.txtMa_dvcs.Text)
            DirMain.dFrom = Me.txtNgay_ct11.Value
            DirMain.dTo = Me.txtNgay_ct12.Value
            Reg.SetRegistryKey("DFDFrom", Me.txtNgay_ct11.Value)
            Reg.SetRegistryKey("DFDTo", Me.txtNgay_ct12.Value)
            Me.pnContent.Text = StringType.FromObject(DirMain.oVar.Item("m_process"))
            DirMain.ShowReport()
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
        End If
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub frmDirInfor_Load(ByVal sender As Object, ByVal e As EventArgs)
        reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 9)
        reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
        Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj As New vouchersearchlibobj(Me.txtTk, Me.lblTen_tk, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, DirMain.sysConn, DirMain.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, DirMain.sysConn, DirMain.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
        Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, DirMain.sysConn, DirMain.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
        Dim obj2 As Object = New CharLib(Me.txtAll, "0, 1")
        Me.CancelButton = Me.cmdCancel
        Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
        Dim document As New PrintDocument
        Me.pnContent.Text = document.PrinterSettings.PrinterName
        Me.tabReports.TabPages.Remove(Me.tbgFree)
        Me.tabReports.TabPages.Remove(Me.tbgOther)
        Me.tabReports.TabPages.Remove(Me.tbgOptions)
        Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
        Me.txtNgay_ct11.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
        Me.txtNgay_ct12.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
        Me.txtNgay_ct01.Value = Me.txtNgay_ct11.Value.AddYears(-1)
        Me.txtNgay_ct02.Value = Me.txtNgay_ct12.Value.AddYears(-1)
        Me.GetQuarterYear()
        Me.AddReportType()
    End Sub

    Private Sub GetQuarterYear()
        If BooleanType.FromObject(ObjectType.BitOrObj((ObjectType.ObjTst(Me.txtNgay_ct12.Text, Fox.GetEmptyDate, False) = 0), Not Information.IsDate(Me.txtNgay_ct12.Text))) Then
            Me.txtQuy.Text = "0"
            Me.txtNam.Text = "0"
        Else
            Try
                Me.dsQuyNam.Clear()
                Sql.SQLRetrieve((DirMain.appConn), StringType.FromObject(ObjectType.AddObj("EXEC dbo.fs20_GetQuarterYear ", Sql.ConvertVS2SQLType(Me.txtNgay_ct12.Value, ""))), "tc", (Me.dsQuyNam))
                Me.txtQuy.Text = StringType.FromObject(Me.dsQuyNam.Tables.Item("tc").Rows.Item(0).Item("Quy"))
                Me.txtNam.Text = StringType.FromObject(Me.dsQuyNam.Tables.Item("tc").Rows.Item(0).Item("Nam"))
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Me.txtQuy.Text = "0"
                Me.txtNam.Text = "0"
                ProjectData.ClearProjectError()
            End Try
        End If
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
        Me.txtNam = New txtNumeric
        Me.txtQuy = New txtNumeric
        Me.Label3 = New Label
        Me.txtNgay_ct02 = New txtDate
        Me.lblDateBegin = New Label
        Me.txtNgay_ct01 = New txtDate
        Me.txtNgay_ct11 = New txtDate
        Me.Label1 = New Label
        Me.txtNgay_ct12 = New txtDate
        Me.lblDateFromTo = New Label
        Me.lblMau_bc = New Label
        Me.cboReports = New ComboBox
        Me.lblTitle = New Label
        Me.txtTitle = New TextBox
        Me.cboReportType = New ComboBox
        Me.tbgOptions = New TabPage
        Me.lblAll = New Label
        Me.optDetail = New RadioButton
        Me.optAll = New RadioButton
        Me.txtLevel = New txtNumeric
        Me.txtAll = New TextBox
        Me.optLevel = New RadioButton
        Me.optAcct = New RadioButton
        Me.optGL = New RadioButton
        Me.txtTk = New TextBox
        Me.lblTen_tk = New Label
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
        Me.cmdAdd = New Button
        Me.tabReports.SuspendLayout()
        Me.tbgFilter.SuspendLayout()
        Me.tbgOptions.SuspendLayout()
        Me.tbgFree.SuspendLayout()
        Me.SuspendLayout()
        Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New Point(160, 82)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.TabIndex = 6
        Me.txtMa_dvcs.Tag = "FCML"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New Point(20, 84)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New Size(36, 16)
        Me.lblMa_dvcs.TabIndex = 1
        Me.lblMa_dvcs.Tag = "L102"
        Me.lblMa_dvcs.Text = "Don vi"
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New Point(264, 84)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New Size(50, 16)
        Me.lblTen_dvcs.TabIndex = 7
        Me.lblTen_dvcs.Tag = "L002"
        Me.lblTen_dvcs.Text = "Ten dvcs"
        Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdOk.Location = New Point(3, 220)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Tag = "L001"
        Me.cmdOk.Text = "Nhan"
        Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
        Me.cmdCancel.Location = New Point(79, 220)
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
        Me.tabReports.Size = New Size(609, 212)
        Me.tabReports.TabIndex = 0
        Me.tbgFilter.Controls.Add(Me.txtNam)
        Me.tbgFilter.Controls.Add(Me.txtQuy)
        Me.tbgFilter.Controls.Add(Me.Label3)
        Me.tbgFilter.Controls.Add(Me.txtNgay_ct02)
        Me.tbgFilter.Controls.Add(Me.lblDateBegin)
        Me.tbgFilter.Controls.Add(Me.txtNgay_ct01)
        Me.tbgFilter.Controls.Add(Me.txtNgay_ct11)
        Me.tbgFilter.Controls.Add(Me.Label1)
        Me.tbgFilter.Controls.Add(Me.txtNgay_ct12)
        Me.tbgFilter.Controls.Add(Me.lblDateFromTo)
        Me.tbgFilter.Controls.Add(Me.lblMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.txtMa_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblTen_dvcs)
        Me.tbgFilter.Controls.Add(Me.lblMau_bc)
        Me.tbgFilter.Controls.Add(Me.cboReports)
        Me.tbgFilter.Controls.Add(Me.lblTitle)
        Me.tbgFilter.Controls.Add(Me.txtTitle)
        Me.tbgFilter.Controls.Add(Me.cboReportType)
        Me.tbgFilter.Location = New Point(4, 22)
        Me.tbgFilter.Name = "tbgFilter"
        Me.tbgFilter.Size = New Size(601, 186)
        Me.tbgFilter.TabIndex = 0
        Me.tbgFilter.Tag = "L100"
        Me.tbgFilter.Text = "Dieu kien loc"
        Me.txtNam.Format = ""
        Me.txtNam.Location = New Point(198, 59)
        Me.txtNam.MaxLength = 4
        Me.txtNam.Name = "txtNam"
        Me.txtNam.Size = New Size(62, 20)
        Me.txtNam.TabIndex = 5
        Me.txtNam.TabStop = False
        Me.txtNam.Tag = "FN"
        Me.txtNam.Text = "0"
        Me.txtNam.TextAlign = HorizontalAlignment.Right
        Me.txtNam.Value = 0
        Me.txtQuy.Format = ""
        Me.txtQuy.Location = New Point(160, 59)
        Me.txtQuy.MaxLength = 1
        Me.txtQuy.Name = "txtQuy"
        Me.txtQuy.Size = New Size(35, 20)
        Me.txtQuy.TabIndex = 4
        Me.txtQuy.TabStop = False
        Me.txtQuy.Tag = "FN"
        Me.txtQuy.Text = "0"
        Me.txtQuy.TextAlign = HorizontalAlignment.Right
        Me.txtQuy.Value = 0
        Me.Label3.AutoSize = True
        Me.Label3.Location = New Point(20, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New Size(50, 16)
        Me.Label3.TabIndex = 20
        Me.Label3.Tag = "L109"
        Me.Label3.Text = "Quy/nam"
        Me.txtNgay_ct02.Location = New Point(263, 36)
        Me.txtNgay_ct02.MaxLength = 10
        Me.txtNgay_ct02.Name = "txtNgay_ct02"
        Me.txtNgay_ct02.TabIndex = 3
        Me.txtNgay_ct02.Tag = "NB"
        Me.txtNgay_ct02.Text = "  /  /    "
        Me.txtNgay_ct02.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_ct02.Value = New DateTime(0)
        Me.lblDateBegin.AutoSize = True
        Me.lblDateBegin.Location = New Point(20, 38)
        Me.lblDateBegin.Name = "lblDateBegin"
        Me.lblDateBegin.Size = New Size(108, 16)
        Me.lblDateBegin.TabIndex = 15
        Me.lblDateBegin.Tag = "L107"
        Me.lblDateBegin.Text = "Ky truoc tu/den ngay"
        Me.txtNgay_ct01.Location = New Point(160, 36)
        Me.txtNgay_ct01.MaxLength = 10
        Me.txtNgay_ct01.Name = "txtNgay_ct01"
        Me.txtNgay_ct01.TabIndex = 2
        Me.txtNgay_ct01.Tag = "NB"
        Me.txtNgay_ct01.Text = "  /  /    "
        Me.txtNgay_ct01.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_ct01.Value = New DateTime(0)
        Me.txtNgay_ct11.Location = New Point(160, 13)
        Me.txtNgay_ct11.MaxLength = 10
        Me.txtNgay_ct11.Name = "txtNgay_ct11"
        Me.txtNgay_ct11.TabIndex = 0
        Me.txtNgay_ct11.Tag = "NB"
        Me.txtNgay_ct11.Text = "  /  /    "
        Me.txtNgay_ct11.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_ct11.Value = New DateTime(0)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New Point(20, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New Size(74, 16)
        Me.Label1.TabIndex = 10
        Me.Label1.Tag = "L105"
        Me.Label1.Text = "Chon bao cao"
        Me.txtNgay_ct12.Location = New Point(263, 13)
        Me.txtNgay_ct12.MaxLength = 10
        Me.txtNgay_ct12.Name = "txtNgay_ct12"
        Me.txtNgay_ct12.TabIndex = 1
        Me.txtNgay_ct12.Tag = "NB"
        Me.txtNgay_ct12.Text = "  /  /    "
        Me.txtNgay_ct12.TextAlign = HorizontalAlignment.Right
        Me.txtNgay_ct12.Value = New DateTime(0)
        Me.lblDateFromTo.AutoSize = True
        Me.lblDateFromTo.Location = New Point(20, 16)
        Me.lblDateFromTo.Name = "lblDateFromTo"
        Me.lblDateFromTo.Size = New Size(101, 16)
        Me.lblDateFromTo.TabIndex = 0
        Me.lblDateFromTo.Tag = "L101"
        Me.lblDateFromTo.Text = "Ky nay tu/den ngay"
        Me.lblMau_bc.AutoSize = True
        Me.lblMau_bc.Location = New Point(20, 131)
        Me.lblMau_bc.Name = "lblMau_bc"
        Me.lblMau_bc.Size = New Size(69, 16)
        Me.lblMau_bc.TabIndex = 2
        Me.lblMau_bc.Tag = "L103"
        Me.lblMau_bc.Text = "Mau bao cao"
        Me.cboReports.Location = New Point(160, 129)
        Me.cboReports.Name = "cboReports"
        Me.cboReports.Size = New Size(300, 21)
        Me.cboReports.TabIndex = 8
        Me.cboReports.Text = "cboReports"
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New Point(20, 155)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New Size(42, 16)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Tag = "L104"
        Me.lblTitle.Text = "Tieu de"
        Me.txtTitle.Location = New Point(160, 153)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New Size(300, 20)
        Me.txtTitle.TabIndex = 9
        Me.txtTitle.Tag = "NB"
        Me.txtTitle.Text = "txtTieu_de"
        Me.cboReportType.DropDownStyle = ComboBoxStyle.DropDownList
        Me.cboReportType.Location = New Point(160, 105)
        Me.cboReportType.Name = "cboReportType"
        Me.cboReportType.Size = New Size(300, 21)
        Me.cboReportType.TabIndex = 7
        Me.tbgOptions.Controls.Add(Me.lblAll)
        Me.tbgOptions.Controls.Add(Me.optDetail)
        Me.tbgOptions.Controls.Add(Me.optAll)
        Me.tbgOptions.Controls.Add(Me.txtLevel)
        Me.tbgOptions.Controls.Add(Me.txtAll)
        Me.tbgOptions.Controls.Add(Me.optLevel)
        Me.tbgOptions.Controls.Add(Me.optAcct)
        Me.tbgOptions.Controls.Add(Me.optGL)
        Me.tbgOptions.Controls.Add(Me.txtTk)
        Me.tbgOptions.Controls.Add(Me.lblTen_tk)
        Me.tbgOptions.Location = New Point(4, 22)
        Me.tbgOptions.Name = "tbgOptions"
        Me.tbgOptions.Size = New Size(601, 186)
        Me.tbgOptions.TabIndex = 1
        Me.tbgOptions.Tag = "L200"
        Me.tbgOptions.Text = "Lua chon"
        Me.lblAll.AutoSize = True
        Me.lblAll.Location = New Point(193, 21)
        Me.lblAll.Name = "lblAll"
        Me.lblAll.Size = New Size(191, 16)
        Me.lblAll.TabIndex = 8
        Me.lblAll.Tag = "L205"
        Me.lblAll.Text = "0 - Xem tat, 1 - Xem cho cac tk so cai"
        Me.optDetail.Location = New Point(160, 88)
        Me.optDetail.Name = "optDetail"
        Me.optDetail.Size = New Size(248, 24)
        Me.optDetail.TabIndex = 3
        Me.optDetail.Tag = "L206"
        Me.optDetail.Text = "Chi xem tai khoan chi tiet"
        Me.optAll.Location = New Point(64, 88)
        Me.optAll.Name = "optAll"
        Me.optAll.Size = New Size(88, 24)
        Me.optAll.TabIndex = 6
        Me.optAll.Tag = "L204"
        Me.optAll.Text = "Xem tat"
        Me.txtLevel.Format = "#0"
        Me.txtLevel.Location = New Point(160, 66)
        Me.txtLevel.MaxLength = 3
        Me.txtLevel.Name = "txtLevel"
        Me.txtLevel.TabIndex = 2
        Me.txtLevel.Tag = "ML"
        Me.txtLevel.Text = "0"
        Me.txtLevel.TextAlign = HorizontalAlignment.Right
        Me.txtLevel.Value = 0
        Me.txtAll.Location = New Point(160, 18)
        Me.txtAll.MaxLength = 1
        Me.txtAll.Name = "txtAll"
        Me.txtAll.Size = New Size(24, 20)
        Me.txtAll.TabIndex = 0
        Me.txtAll.Text = "txtAll"
        Me.txtAll.TextAlign = HorizontalAlignment.Right
        Me.optLevel.Location = New Point(20, 64)
        Me.optLevel.Name = "optLevel"
        Me.optLevel.Size = New Size(134, 24)
        Me.optLevel.TabIndex = 2
        Me.optLevel.Tag = "L203"
        Me.optLevel.Text = "Xem cho tk co bac <="
        Me.optAcct.Location = New Point(20, 40)
        Me.optAcct.Name = "optAcct"
        Me.optAcct.Size = New Size(134, 24)
        Me.optAcct.TabIndex = 1
        Me.optAcct.Tag = "L202"
        Me.optAcct.Text = "Xem cho tai khoan"
        Me.optGL.Location = New Point(20, 16)
        Me.optGL.Name = "optGL"
        Me.optGL.Size = New Size(134, 24)
        Me.optGL.TabIndex = 0
        Me.optGL.Tag = "L201"
        Me.optGL.Text = "Xem cho tk so cai"
        Me.txtTk.CharacterCasing = CharacterCasing.Upper
        Me.txtTk.Location = New Point(160, 42)
        Me.txtTk.Name = "txtTk"
        Me.txtTk.TabIndex = 2
        Me.txtTk.Tag = "ML"
        Me.txtTk.Text = "TXTTK"
        Me.lblTen_tk.AutoSize = True
        Me.lblTen_tk.Location = New Point(271, 44)
        Me.lblTen_tk.Name = "lblTen_tk"
        Me.lblTen_tk.Size = New Size(73, 16)
        Me.lblTen_tk.TabIndex = 9
        Me.lblTen_tk.Text = "Ten tai khoan"
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
        Me.tbgFree.Size = New Size(601, 186)
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
        Me.tbgOther.Size = New Size(601, 186)
        Me.tbgOther.TabIndex = 3
        Me.tbgOther.Tag = "FreeReportOther"
        Me.tbgOther.Text = "Dieu kien khac"
        Me.cmdAdd.Anchor = (AnchorStyles.Right Or AnchorStyles.Bottom)
        Me.cmdAdd.Location = New Point(455, 220)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New Size(150, 23)
        Me.cmdAdd.TabIndex = 2
        Me.cmdAdd.TabStop = False
        Me.cmdAdd.Tag = "L106"
        Me.cmdAdd.Text = "Tao mau bao cao"
        Me.AutoScaleBaseSize = New Size(5, 13)
        Me.ClientSize = New Size(608, 277)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.tabReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOk)
        Me.Name = "frmFilter"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "frmFilter"
        Me.tabReports.ResumeLayout(False)
        Me.tbgFilter.ResumeLayout(False)
        Me.tbgOptions.ResumeLayout(False)
        Me.tbgFree.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Private Sub txtNgay_ct12_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtNgay_ct12.Validated
        Me.GetQuarterYear()
    End Sub


    ' Properties
    Friend WithEvents cboReports As ComboBox
    Friend WithEvents cboReportType As ComboBox
    Friend WithEvents cmdAdd As Button
    Friend WithEvents cmdCancel As Button
    Friend WithEvents cmdOk As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblAll As Label
    Friend WithEvents lblDateBegin As Label
    Friend WithEvents lblDateFromTo As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_td1 As Label
    Friend WithEvents lblMa_td2 As Label
    Friend WithEvents lblMa_td3 As Label
    Friend WithEvents lblMau_bc As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_td1 As Label
    Friend WithEvents lblTen_td2 As Label
    Friend WithEvents lblTen_td3 As Label
    Friend WithEvents lblTen_tk As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents optAcct As RadioButton
    Friend WithEvents optAll As RadioButton
    Friend WithEvents optDetail As RadioButton
    Friend WithEvents optGL As RadioButton
    Friend WithEvents optLevel As RadioButton
    Friend WithEvents tabReports As TabControl
    Friend WithEvents tbgFilter As TabPage
    Friend WithEvents tbgFree As TabPage
    Friend WithEvents tbgOptions As TabPage
    Friend WithEvents tbgOther As TabPage
    Friend WithEvents txtAll As TextBox
    Friend WithEvents txtLevel As txtNumeric
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_td1 As TextBox
    Friend WithEvents txtMa_td2 As TextBox
    Friend WithEvents txtMa_td3 As TextBox
    Friend WithEvents txtNam As txtNumeric
    Friend WithEvents txtNgay_ct01 As txtDate
    Friend WithEvents txtNgay_ct02 As txtDate
    Friend WithEvents txtNgay_ct11 As txtDate
    Friend WithEvents txtNgay_ct12 As txtDate
    Friend WithEvents txtQuy As txtNumeric
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents txtTk As TextBox

    Private components As IContainer
    Private dsQuyNam As DataSet
    Public pnContent As StatusBarPanel
End Class

