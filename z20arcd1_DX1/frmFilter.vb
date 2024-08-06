Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol
Imports libscontrol.voucherseachlib

Namespace arcd1
    Public Class frmFilter
        Inherits Form
        ' Methods
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmDirInfor_Load)
            Me.flag = False
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
                DirMain.strKey = "1 = 1"
                Dim num5 As Integer = (Me.tabReports.TabPages.Count - 1)
                Dim i As Integer = 0
                Do While (i <= num5)
                    Dim num4 As Integer = (Me.tabReports.TabPages.Item(i).Controls.Count - 1)
                    Dim j As Integer = 0
                    Do While (j <= num4)
                        Dim str As String = ""
                        If ((Strings.InStr(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), "Master", CompareMethod.Binary) > 0) Or (Strings.InStr(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), "Detail", CompareMethod.Binary) > 0)) Then
                            Dim flag As Boolean = False
                            str = Fox.GetWordNum(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), 2, "#"c)
                            If (Strings.InStr(Me.tabReports.TabPages.Item(i).Controls.Item(j).GetType.ToString.ToLower, "libscontrol.txtnumeric", CompareMethod.Binary) > 0) Then
                                Dim numeric As txtNumeric = DirectCast(Me.tabReports.TabPages.Item(i).Controls.Item(j), txtNumeric)
                                If (numeric.Value <> 0) Then
                                    str = Strings.Replace(str, "%n", StringType.FromObject(Sql.ConvertVS2SQLType(numeric.Value, "")), 1, -1, CompareMethod.Binary)
                                Else
                                    str = ""
                                End If
                                flag = True
                            End If
                            If (Strings.InStr(Me.tabReports.TabPages.Item(i).Controls.Item(j).GetType.ToString.ToLower, "libscontrol.txtdate", CompareMethod.Binary) > 0) Then
                                Dim _txtdate As txtDate = DirectCast(Me.tabReports.TabPages.Item(i).Controls.Item(j), txtDate)
                                If (ObjectType.ObjTst(_txtdate.Text, Fox.GetEmptyDate, False) <> 0) Then
                                    str = Strings.Replace(str, "%d", StringType.FromObject(Sql.ConvertVS2SQLType(_txtdate.Value, "")), 1, -1, CompareMethod.Binary)
                                Else
                                    str = ""
                                End If
                                flag = True
                            End If
                            If Not flag Then
                                Dim box As TextBox = DirectCast(Me.tabReports.TabPages.Item(i).Controls.Item(j), TextBox)
                                If (StringType.StrCmp(Strings.Trim(box.Text), "", False) <> 0) Then
                                    If (Strings.InStr(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), "FC", CompareMethod.Binary) > 0) Then
                                        str = Strings.Replace(str, "%s", Strings.Trim(Strings.Replace(box.Text, "'", "", 1, -1, CompareMethod.Binary)), 1, -1, CompareMethod.Binary)
                                    End If
                                    If (Strings.InStr(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), "FN", CompareMethod.Binary) > 0) Then
                                        str = Strings.Replace(str, "%n", box.Text, 1, -1, CompareMethod.Binary)
                                    End If
                                Else
                                    str = ""
                                End If
                            End If
                        End If
                        If ((Strings.InStr(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), "Master", CompareMethod.Binary) > 0) And (StringType.StrCmp(Strings.Trim(str), "", False) <> 0)) Then
                            If (Strings.InStr(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), "EX", CompareMethod.Binary) > 0) Then
                                DirMain.strKey = (DirMain.strKey & " AND (" & str & ")")
                            Else
                                DirMain.strKey = (DirMain.strKey & " AND (a." & str & ")")
                            End If
                        End If
                        If ((Strings.InStr(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), "Detail", CompareMethod.Binary) > 0) And (StringType.StrCmp(Strings.Trim(str), "", False) <> 0)) Then
                            If (Strings.InStr(StringType.FromObject(Me.tabReports.TabPages.Item(i).Controls.Item(j).Tag), "EX", CompareMethod.Binary) > 0) Then
                                DirMain.strKey = (DirMain.strKey & " AND (" & str & ")")
                            Else
                                DirMain.strKey = (DirMain.strKey & " AND (a." & str & ")")
                            End If
                        End If
                        j += 1
                    Loop
                    i += 1
                Loop
                DirMain.strOrder = ""
                Dim text As String = DirMain.fPrint.txtGroup4.Text
                If (StringType.StrCmp([text], "0", False) = 0) Then
                    DirMain.strOrder = StringType.FromObject(Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("language"), "V", False) = 0), "Ten_kh", "Ten_kh2"))
                ElseIf (StringType.StrCmp([text], "1", False) = 0) Then
                    DirMain.strOrder = "Ma_kh"
                ElseIf (StringType.StrCmp([text], "2", False) = 0) Then
                    DirMain.strOrder = "Ps_no"
                ElseIf (StringType.StrCmp([text], "3", False) = 0) Then
                    DirMain.strOrder = "Ps_co"
                ElseIf (StringType.StrCmp([text], "4", False) = 0) Then
                    DirMain.strOrder = "No_ck"
                ElseIf (StringType.StrCmp([text], "5", False) = 0) Then
                    DirMain.strOrder = "Co_ck"
                End If
                DirMain.strGroups = ""
                If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
                    DirMain.strGroups = (DirMain.strGroups & ",nh_kh" & Strings.Trim(DirMain.fPrint.txtGroup1.Text))
                End If
                If (StringType.StrCmp(DirMain.fPrint.txtGroup2.Text, "0", False) <> 0) Then
                    DirMain.strGroups = (DirMain.strGroups & ",nh_kh" & Strings.Trim(DirMain.fPrint.txtGroup2.Text))
                End If
                If (StringType.StrCmp(DirMain.fPrint.txtGroup3.Text, "0", False) <> 0) Then
                    DirMain.strGroups = (DirMain.strGroups & ",nh_kh" & Strings.Trim(DirMain.fPrint.txtGroup3.Text))
                End If
                DirMain.strGroups = Strings.Mid(DirMain.strGroups, 2)
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
            reportformlib.AddFreeFields(DirMain.sysConn, Me.tabReports.TabPages.Item(3), 9)
            Me.txtTk.Text = StringType.FromObject(Reg.GetRegistryKey("DFAccount"))
            Dim oTk As New DirLib(Me.txtTk, Me.lblTen_tk, DirMain.sysConn, DirMain.appConn, "dmtk", "tk", "ten_tk", "Account", "tk_cn = 1", False, Me.cmdCancel)
            reportformlib.SetRPFormCaption(Me, Me.tabReports, DirMain.oLan, DirMain.oVar, DirMain.oLen)
            Me.txtTk.Text = StringType.FromObject(Reg.GetRegistryKey("DFAccount"))
            Dim vouchersearchlibobj As New DirLib(Me.txtMa_kh, Me.lblTen_kh, DirMain.sysConn, DirMain.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", DirMain.strKeyCust, True, Me.cmdCancel)
            Dim vouchersearchlibobj5 As New vouchersearchlibobj(Me.txtMa_nh1, Me.lblTen_nh1, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj6 As New vouchersearchlibobj(Me.txtMa_nh2, Me.lblTen_nh2, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=2", True, Me.cmdCancel)
            Dim vouchersearchlibobj7 As New vouchersearchlibobj(Me.txtMa_nh3, Me.lblTen_nh3, DirMain.sysConn, DirMain.appConn, "dmnhkh", "ma_nh", "ten_nh", "CustomerGroup", "loai_nh=3", True, Me.cmdCancel)
            Dim vouchersearchlibobj8 As New vouchersearchlibobj(Me.txtMa_dvcs, Me.lblTen_dvcs, DirMain.sysConn, DirMain.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj2 As New vouchersearchlibobj(Me.txtMa_td1, Me.lblTen_td1, DirMain.sysConn, DirMain.appConn, "dmtd1", "ma_td", "ten_td", "Free1", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj3 As New vouchersearchlibobj(Me.txtMa_td2, Me.lblTen_td2, DirMain.sysConn, DirMain.appConn, "dmtd2", "ma_td", "ten_td", "Free2", "1=1", True, Me.cmdCancel)
            Dim vouchersearchlibobj4 As New vouchersearchlibobj(Me.txtMa_td3, Me.lblTen_td3, DirMain.sysConn, DirMain.appConn, "dmtd3", "ma_td", "ten_td", "Free3", "1=1", True, Me.cmdCancel)
            Dim lib2 As New CharLib(Me.txtGroup1, "0,1,2,3")
            Dim lib3 As New CharLib(Me.txtGroup2, "0,1,2,3")
            Dim lib4 As New CharLib(Me.txtGroup3, "0,1,2,3")
            Dim lib5 As New CharLib(Me.txtGroup4, "0,1,2,3,4,5")
            Me.CancelButton = Me.cmdCancel
            Me.pnContent = clsvoucher.clsVoucher.AddStb(Me)
            Dim document As New PrintDocument
            Me.pnContent.Text = document.PrinterSettings.PrinterName
            Me.tabReports.TabPages.Remove(Me.tbgFree)
            Me.tabReports.TabPages.Remove(Me.tbgOther)
            Me.tabReports.SelectedTab = Me.tbgFilter
            Me.txtTitle.Text = Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTable.Rows.Item(0), Nothing, "Item", New Object() {ObjectType.AddObj("rep_title", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))}, Nothing, Nothing)))
            Me.txtDFrom.Value = DateType.FromObject(Reg.GetRegistryKey("DFDFrom"))
            Me.txtDTo.Value = DateType.FromObject(Reg.GetRegistryKey("DFDTo"))
            Me.txtGroup1.Text = StringType.FromInteger(0)
            Me.txtGroup2.Text = StringType.FromInteger(0)
            Me.txtGroup3.Text = StringType.FromInteger(0)
            Me.txtGroup4.Text = StringType.FromInteger(0)
            Me.lblPs_no.Text = Strings.Replace(Me.lblPs_no.Text, "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary)
            Me.lblPs_co.Text = Strings.Replace(Me.lblPs_co.Text, "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary)
            Me.lblDu_no.Text = Strings.Replace(Me.lblDu_no.Text, "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary)
            Me.lblDu_co.Text = Strings.Replace(Me.lblDu_co.Text, "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary)
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
            Me.Label15 = New Label
            Me.txtDu_co2 = New txtNumeric
            Me.txtDu_no2 = New txtNumeric
            Me.txtPs_co2 = New txtNumeric
            Me.txtPs_no2 = New txtNumeric
            Me.Label11 = New Label
            Me.Label12 = New Label
            Me.Label13 = New Label
            Me.Label14 = New Label
            Me.txtDu_co1 = New txtNumeric
            Me.txtDu_no1 = New txtNumeric
            Me.txtPs_co1 = New txtNumeric
            Me.txtPs_no1 = New txtNumeric
            Me.lblDu_co = New Label
            Me.lblPs_no = New Label
            Me.lblDu_no = New Label
            Me.lblPs_co = New Label
            Me.Label6 = New Label
            Me.txtGroup3 = New TextBox
            Me.txtGroup2 = New TextBox
            Me.txtGroup4 = New TextBox
            Me.txtGroup1 = New TextBox
            Me.lblTk = New Label
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
            Me.tbgOptions.SuspendLayout()
            Me.tbgFree.SuspendLayout()
            Me.SuspendLayout()
            Me.txtMa_dvcs.CharacterCasing = CharacterCasing.Upper
            Me.txtMa_dvcs.Location = New Point(160, 151)
            Me.txtMa_dvcs.Name = "txtMa_dvcs"
            Me.txtMa_dvcs.TabIndex = 7
            Me.txtMa_dvcs.Tag = "FCML"
            Me.txtMa_dvcs.Text = "TXTMA_DVCS"
            Me.lblMa_dvcs.AutoSize = True
            Me.lblMa_dvcs.Location = New Point(20, 153)
            Me.lblMa_dvcs.Name = "lblMa_dvcs"
            Me.lblMa_dvcs.Size = New Size(36, 16)
            Me.lblMa_dvcs.TabIndex = 1
            Me.lblMa_dvcs.Tag = "L102"
            Me.lblMa_dvcs.Text = "Don vi"
            Me.lblTen_dvcs.AutoSize = True
            Me.lblTen_dvcs.Location = New Point(264, 153)
            Me.lblTen_dvcs.Name = "lblTen_dvcs"
            Me.lblTen_dvcs.Size = New Size(50, 16)
            Me.lblTen_dvcs.TabIndex = 7
            Me.lblTen_dvcs.Tag = "L002"
            Me.lblTen_dvcs.Text = "Ten dvcs"
            Me.cmdOk.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdOk.Location = New Point(3, 268)
            Me.cmdOk.Name = "cmdOk"
            Me.cmdOk.TabIndex = 1
            Me.cmdOk.Tag = "L001"
            Me.cmdOk.Text = "Nhan"
            Me.cmdCancel.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
            Me.cmdCancel.Location = New Point(79, 268)
            Me.cmdCancel.Name = "cmdCancel"
            Me.cmdCancel.TabIndex = 2
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
            Me.tabReports.Size = New Size(609, 256)
            Me.tabReports.TabIndex = 0
            Me.tabReports.Tag = ""
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
            Me.tbgFilter.Size = New Size(601, 230)
            Me.tbgFilter.TabIndex = 0
            Me.tbgFilter.Tag = "L100"
            Me.tbgFilter.Text = "Dieu kien loc"
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
            Me.lblMau_bc.Location = New Point(20, 176)
            Me.lblMau_bc.Name = "lblMau_bc"
            Me.lblMau_bc.Size = New Size(69, 16)
            Me.lblMau_bc.TabIndex = 2
            Me.lblMau_bc.Tag = "L103"
            Me.lblMau_bc.Text = "Mau bao cao"
            Me.cboReports.Location = New Point(160, 174)
            Me.cboReports.Name = "cboReports"
            Me.cboReports.Size = New Size(300, 21)
            Me.cboReports.TabIndex = 8
            Me.cboReports.Text = "cboReports"
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Location = New Point(20, 199)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New Size(42, 16)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Tag = "L104"
            Me.lblTitle.Text = "Tieu de"
            Me.txtTitle.Location = New Point(160, 197)
            Me.txtTitle.Name = "txtTitle"
            Me.txtTitle.Size = New Size(300, 20)
            Me.txtTitle.TabIndex = 9
            Me.txtTitle.Tag = "NB"
            Me.txtTitle.Text = "txtTieu_de"
            Me.tbgOptions.Controls.Add(Me.Label15)
            Me.tbgOptions.Controls.Add(Me.txtDu_co2)
            Me.tbgOptions.Controls.Add(Me.txtDu_no2)
            Me.tbgOptions.Controls.Add(Me.txtPs_co2)
            Me.tbgOptions.Controls.Add(Me.txtPs_no2)
            Me.tbgOptions.Controls.Add(Me.Label11)
            Me.tbgOptions.Controls.Add(Me.Label12)
            Me.tbgOptions.Controls.Add(Me.Label13)
            Me.tbgOptions.Controls.Add(Me.Label14)
            Me.tbgOptions.Controls.Add(Me.txtDu_co1)
            Me.tbgOptions.Controls.Add(Me.txtDu_no1)
            Me.tbgOptions.Controls.Add(Me.txtPs_co1)
            Me.tbgOptions.Controls.Add(Me.txtPs_no1)
            Me.tbgOptions.Controls.Add(Me.lblDu_co)
            Me.tbgOptions.Controls.Add(Me.lblPs_no)
            Me.tbgOptions.Controls.Add(Me.lblDu_no)
            Me.tbgOptions.Controls.Add(Me.lblPs_co)
            Me.tbgOptions.Controls.Add(Me.Label6)
            Me.tbgOptions.Controls.Add(Me.txtGroup3)
            Me.tbgOptions.Controls.Add(Me.txtGroup2)
            Me.tbgOptions.Controls.Add(Me.txtGroup4)
            Me.tbgOptions.Controls.Add(Me.txtGroup1)
            Me.tbgOptions.Controls.Add(Me.lblTk)
            Me.tbgOptions.Location = New Point(4, 22)
            Me.tbgOptions.Name = "tbgOptions"
            Me.tbgOptions.Size = New Size(601, 230)
            Me.tbgOptions.TabIndex = 1
            Me.tbgOptions.Tag = "L200"
            Me.tbgOptions.Text = "Lua chon"
            Me.Label15.AutoSize = True
            Me.Label15.Location = New Point(188, 38)
            Me.Label15.Name = "Label15"
            Me.Label15.Size = New Size(350, 16)
            Me.Label15.TabIndex = 119
            Me.Label15.Tag = "L208"
            Me.Label15.Text = "0 - Ten khach, 1 - Ma khach, 2 - Ps no, 3 - Ps co, 4 - Du no, 5 - Du co"
            Me.txtDu_co2.Format = "m_ip_tien"
            Me.txtDu_co2.Location = New Point(300, 128)
            Me.txtDu_co2.MaxLength = 10
            Me.txtDu_co2.Name = "txtDu_co2"
            Me.txtDu_co2.TabIndex = 11
            Me.txtDu_co2.Tag = "MLEXDetail#Co_ck <= %n#"
            Me.txtDu_co2.Text = "m_ip_tien"
            Me.txtDu_co2.TextAlign = HorizontalAlignment.Right
            Me.txtDu_co2.Value = 0
            Me.txtDu_no2.Format = "m_ip_tien"
            Me.txtDu_no2.Location = New Point(300, 105)
            Me.txtDu_no2.MaxLength = 10
            Me.txtDu_no2.Name = "txtDu_no2"
            Me.txtDu_no2.TabIndex = 9
            Me.txtDu_no2.Tag = "MLEXDetail#No_ck <= %n#"
            Me.txtDu_no2.Text = "m_ip_tien"
            Me.txtDu_no2.TextAlign = HorizontalAlignment.Right
            Me.txtDu_no2.Value = 0
            Me.txtPs_co2.Format = "m_ip_tien"
            Me.txtPs_co2.Location = New Point(300, 82)
            Me.txtPs_co2.MaxLength = 10
            Me.txtPs_co2.Name = "txtPs_co2"
            Me.txtPs_co2.TabIndex = 7
            Me.txtPs_co2.Tag = "MLEXDetail#Ps_co <= %n#"
            Me.txtPs_co2.Text = "m_ip_tien"
            Me.txtPs_co2.TextAlign = HorizontalAlignment.Right
            Me.txtPs_co2.Value = 0
            Me.txtPs_no2.Format = "m_ip_tien"
            Me.txtPs_no2.Location = New Point(300, 59)
            Me.txtPs_no2.MaxLength = 10
            Me.txtPs_no2.Name = "txtPs_no2"
            Me.txtPs_no2.TabIndex = 5
            Me.txtPs_no2.Tag = "MLEXDetail#Ps_no <= %n#"
            Me.txtPs_no2.Text = "m_ip_tien"
            Me.txtPs_no2.TextAlign = HorizontalAlignment.Right
            Me.txtPs_no2.Value = 0
            Me.Label11.AutoSize = True
            Me.Label11.Location = New Point(264, 130)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New Size(25, 16)
            Me.Label11.TabIndex = 114
            Me.Label11.Tag = "L207"
            Me.Label11.Text = "Den"
            Me.Label11.TextAlign = ContentAlignment.TopCenter
            Me.Label12.AutoSize = True
            Me.Label12.Location = New Point(264, 61)
            Me.Label12.Name = "Label12"
            Me.Label12.Size = New Size(25, 16)
            Me.Label12.TabIndex = 113
            Me.Label12.Tag = "L207"
            Me.Label12.Text = "Den"
            Me.Label12.TextAlign = ContentAlignment.TopCenter
            Me.Label13.AutoSize = True
            Me.Label13.Location = New Point(264, 107)
            Me.Label13.Name = "Label13"
            Me.Label13.Size = New Size(25, 16)
            Me.Label13.TabIndex = 112
            Me.Label13.Tag = "L207"
            Me.Label13.Text = "Den"
            Me.Label13.TextAlign = ContentAlignment.TopCenter
            Me.Label14.AutoSize = True
            Me.Label14.Location = New Point(264, 84)
            Me.Label14.Name = "Label14"
            Me.Label14.Size = New Size(25, 16)
            Me.Label14.TabIndex = 111
            Me.Label14.Tag = "L207"
            Me.Label14.Text = "Den"
            Me.Label14.TextAlign = ContentAlignment.TopCenter
            Me.txtDu_co1.Format = "m_ip_tien"
            Me.txtDu_co1.Location = New Point(160, 128)
            Me.txtDu_co1.MaxLength = 10
            Me.txtDu_co1.Name = "txtDu_co1"
            Me.txtDu_co1.TabIndex = 10
            Me.txtDu_co1.Tag = "MLEXDetail#Co_ck >= %n#"
            Me.txtDu_co1.Text = "m_ip_tien"
            Me.txtDu_co1.TextAlign = HorizontalAlignment.Right
            Me.txtDu_co1.Value = 0
            Me.txtDu_no1.Format = "m_ip_tien"
            Me.txtDu_no1.Location = New Point(160, 105)
            Me.txtDu_no1.MaxLength = 10
            Me.txtDu_no1.Name = "txtDu_no1"
            Me.txtDu_no1.TabIndex = 8
            Me.txtDu_no1.Tag = "MLEXDetail#No_ck >= %n#"
            Me.txtDu_no1.Text = "m_ip_tien"
            Me.txtDu_no1.TextAlign = HorizontalAlignment.Right
            Me.txtDu_no1.Value = 0
            Me.txtPs_co1.Format = "m_ip_tien"
            Me.txtPs_co1.Location = New Point(160, 82)
            Me.txtPs_co1.MaxLength = 10
            Me.txtPs_co1.Name = "txtPs_co1"
            Me.txtPs_co1.TabIndex = 6
            Me.txtPs_co1.Tag = "MLEXDetail#Ps_co >= %n#"
            Me.txtPs_co1.Text = "m_ip_tien"
            Me.txtPs_co1.TextAlign = HorizontalAlignment.Right
            Me.txtPs_co1.Value = 0
            Me.txtPs_no1.Format = "m_ip_tien"
            Me.txtPs_no1.Location = New Point(160, 59)
            Me.txtPs_no1.MaxLength = 10
            Me.txtPs_no1.Name = "txtPs_no1"
            Me.txtPs_no1.TabIndex = 4
            Me.txtPs_no1.Tag = "MLEXDetail#Ps_no >= %n#"
            Me.txtPs_no1.Text = "m_ip_tien"
            Me.txtPs_no1.TextAlign = HorizontalAlignment.Right
            Me.txtPs_no1.Value = 0
            Me.lblDu_co.AutoSize = True
            Me.lblDu_co.Location = New Point(16, 130)
            Me.lblDu_co.Name = "lblDu_co"
            Me.lblDu_co.Size = New Size(89, 16)
            Me.lblDu_co.TabIndex = 106
            Me.lblDu_co.Tag = "L206"
            Me.lblDu_co.Text = "Du co cuoi %s tu"
            Me.lblPs_no.AutoSize = True
            Me.lblPs_no.Location = New Point(16, 61)
            Me.lblPs_no.Name = "lblPs_no"
            Me.lblPs_no.Size = New Size(64, 16)
            Me.lblPs_no.TabIndex = 105
            Me.lblPs_no.Tag = "L203"
            Me.lblPs_no.Text = "Ps no %s tu"
            Me.lblDu_no.AutoSize = True
            Me.lblDu_no.Location = New Point(16, 107)
            Me.lblDu_no.Name = "lblDu_no"
            Me.lblDu_no.Size = New Size(89, 16)
            Me.lblDu_no.TabIndex = 104
            Me.lblDu_no.Tag = "L205"
            Me.lblDu_no.Text = "Du no cuoi %s tu"
            Me.lblPs_co.AutoSize = True
            Me.lblPs_co.Location = New Point(16, 84)
            Me.lblPs_co.Name = "lblPs_co"
            Me.lblPs_co.Size = New Size(64, 16)
            Me.lblPs_co.TabIndex = 103
            Me.lblPs_co.Tag = "L204"
            Me.lblPs_co.Text = "Ps co %s tu"
            Me.Label6.AutoSize = True
            Me.Label6.Location = New Point(16, 38)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New Size(71, 16)
            Me.Label6.TabIndex = 102
            Me.Label6.Tag = "L202"
            Me.Label6.Text = "Sap xep theo"
            Me.txtGroup3.CharacterCasing = CharacterCasing.Upper
            Me.txtGroup3.Location = New Point(214, 13)
            Me.txtGroup3.MaxLength = 1
            Me.txtGroup3.Name = "txtGroup3"
            Me.txtGroup3.Size = New Size(24, 20)
            Me.txtGroup3.TabIndex = 2
            Me.txtGroup3.Tag = "FC"
            Me.txtGroup3.Text = "TXTNO_CO"
            Me.txtGroup2.CharacterCasing = CharacterCasing.Upper
            Me.txtGroup2.Location = New Point(187, 13)
            Me.txtGroup2.MaxLength = 1
            Me.txtGroup2.Name = "txtGroup2"
            Me.txtGroup2.Size = New Size(24, 20)
            Me.txtGroup2.TabIndex = 1
            Me.txtGroup2.Tag = "FC"
            Me.txtGroup2.Text = "TXTNO_CO"
            Me.txtGroup4.CharacterCasing = CharacterCasing.Upper
            Me.txtGroup4.Location = New Point(160, 36)
            Me.txtGroup4.MaxLength = 1
            Me.txtGroup4.Name = "txtGroup4"
            Me.txtGroup4.Size = New Size(24, 20)
            Me.txtGroup4.TabIndex = 3
            Me.txtGroup4.Tag = "FC"
            Me.txtGroup4.Text = "TXTNO_CO"
            Me.txtGroup1.CharacterCasing = CharacterCasing.Upper
            Me.txtGroup1.Location = New Point(160, 13)
            Me.txtGroup1.MaxLength = 1
            Me.txtGroup1.Name = "txtGroup1"
            Me.txtGroup1.Size = New Size(24, 20)
            Me.txtGroup1.TabIndex = 0
            Me.txtGroup1.Tag = "FC"
            Me.txtGroup1.Text = "TXTNO_CO"
            Me.lblTk.AutoSize = True
            Me.lblTk.Location = New Point(16, 15)
            Me.lblTk.Name = "lblTk"
            Me.lblTk.Size = New Size(135, 16)
            Me.lblTk.TabIndex = 98
            Me.lblTk.Tag = "L201"
            Me.lblTk.Text = "Thu tu sap xep theo nhom"
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
            Me.tbgFree.Size = New Size(601, 230)
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
            Me.tbgOther.Size = New Size(601, 230)
            Me.tbgOther.TabIndex = 3
            Me.tbgOther.Tag = "FreeReportOther"
            Me.tbgOther.Text = "Dieu kien khac"
            Me.AutoScaleBaseSize = New Size(5, 13)
            Me.ClientSize = New Size(608, 325)
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

        Private Sub txtDFrom_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub txtGroup1_Enter(ByVal sender As Object, ByVal e As EventArgs)
            Me.flag = False
        End Sub

        Private Sub txtGroup1_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
            Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
            Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
            If (((Me.intGroup2 + Me.intGroup3) <> 0) And (((Me.intGroup1 = Me.intGroup2) Or (Me.intGroup1 = Me.intGroup3)) Or (Me.intGroup1 = 0))) Then
                DirMain.fPrint.txtGroup1.Focus()
            End If
        End Sub

        Private Sub txtGroup2_Enter(ByVal sender As Object, ByVal e As EventArgs)
            If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) = 0) Then
                If Me.flag Then
                    DirMain.fPrint.txtGroup1.Focus()
                Else
                    DirMain.fPrint.txtGroup4.Focus()
                End If
            End If
            If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
                Me.flag = False
            End If
        End Sub

        Private Sub txtGroup2_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
            Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
            Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
            If (((Me.intGroup2 + Me.intGroup3) <> 0) And (((Me.intGroup2 = Me.intGroup1) Or (Me.intGroup2 = Me.intGroup3)) Or (Me.intGroup2 = 0))) Then
                DirMain.fPrint.txtGroup2.Focus()
            End If
        End Sub

        Private Sub txtGroup3_Enter(ByVal sender As Object, ByVal e As EventArgs)
            If (StringType.StrCmp(DirMain.fPrint.txtGroup2.Text, "0", False) = 0) Then
                If Me.flag Then
                    DirMain.fPrint.txtGroup2.Focus()
                Else
                    DirMain.fPrint.txtGroup4.Focus()
                End If
            End If
            If (StringType.StrCmp(DirMain.fPrint.txtGroup1.Text, "0", False) <> 0) Then
                Me.flag = False
            End If
        End Sub

        Private Sub txtGroup3_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.intGroup1 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup1.Text)))
            Me.intGroup2 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup2.Text)))
            Me.intGroup3 = CInt(Math.Round(Conversion.Val(DirMain.fPrint.txtGroup3.Text)))
            If ((Me.intGroup3 <> 0) And ((Me.intGroup3 = Me.intGroup2) Or (Me.intGroup3 = Me.intGroup1))) Then
                DirMain.fPrint.txtGroup3.Focus()
            End If
        End Sub

        Private Sub txtGroup4_Validated(ByVal sender As Object, ByVal e As EventArgs)
            Me.flag = True
        End Sub


        ' Properties
        Friend WithEvents cboReports As ComboBox
        Friend WithEvents cmdCancel As Button
        Friend WithEvents cmdOk As Button
        Friend WithEvents Label1 As Label
        Friend WithEvents Label11 As Label
        Friend WithEvents Label12 As Label
        Friend WithEvents Label13 As Label
        Friend WithEvents Label14 As Label
        Friend WithEvents Label15 As Label
        Friend WithEvents Label2 As Label
        Friend WithEvents Label3 As Label
        Friend WithEvents Label6 As Label
        Friend WithEvents lblDateFromTo As Label
        Friend WithEvents lblDu_co As Label
        Friend WithEvents lblDu_no As Label
        Friend WithEvents lblMa_dvcs As Label
        Friend WithEvents lblMa_td1 As Label
        Friend WithEvents lblMa_td2 As Label
        Friend WithEvents lblMa_td3 As Label
        Friend WithEvents lblMau_bc As Label
        Friend WithEvents lblPs_co As Label
        Friend WithEvents lblPs_no As Label
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
        Friend WithEvents lblTk As Label
        Friend WithEvents lblTk_co As Label
        Friend WithEvents lblTk_no As Label
        Friend WithEvents tabReports As TabControl
        Friend WithEvents tbgFilter As TabPage
        Friend WithEvents tbgFree As TabPage
        Friend WithEvents tbgOptions As TabPage
        Friend WithEvents tbgOther As TabPage
        Friend WithEvents txtDFrom As txtDate
        Friend WithEvents txtDTo As txtDate
        Friend WithEvents txtDu_co1 As txtNumeric
        Friend WithEvents txtDu_co2 As txtNumeric
        Friend WithEvents txtDu_no1 As txtNumeric
        Friend WithEvents txtDu_no2 As txtNumeric
        Friend WithEvents txtGroup1 As TextBox
        Friend WithEvents txtGroup2 As TextBox
        Friend WithEvents txtGroup3 As TextBox
        Friend WithEvents txtGroup4 As TextBox
        Friend WithEvents txtMa_dvcs As TextBox
        Friend WithEvents txtMa_kh As TextBox
        Friend WithEvents txtMa_nh1 As TextBox
        Friend WithEvents txtMa_nh2 As TextBox
        Friend WithEvents txtMa_nh3 As TextBox
        Friend WithEvents txtMa_td1 As TextBox
        Friend WithEvents txtMa_td2 As TextBox
        Friend WithEvents txtMa_td3 As TextBox
        Friend WithEvents txtPs_co1 As txtNumeric
        Friend WithEvents txtPs_co2 As txtNumeric
        Friend WithEvents txtPs_no1 As txtNumeric
        Friend WithEvents txtPs_no2 As txtNumeric
        Friend WithEvents txtTitle As TextBox
        Friend WithEvents txtTk As TextBox

        Private components As IContainer
        Private flag As Boolean
        Private intGroup1 As Integer
        Private intGroup2 As Integer
        Private intGroup3 As Integer
        Public pnContent As StatusBarPanel
    End Class
End Namespace

