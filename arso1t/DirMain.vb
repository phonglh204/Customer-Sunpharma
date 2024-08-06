Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscommon
Imports libscontrol

Namespace arso1t
    Module DirMain
        ' Methods
        Private Sub grdKeyup(ByVal sender As Object, ByVal e As KeyEventArgs)
            If (e.Control And (e.KeyCode = Keys.A)) Then
                DirMain.SelectRows(True)
            End If
            If (e.Control And (e.KeyCode = Keys.U)) Then
                DirMain.SelectRows(False)
            End If
        End Sub

        <STAThread()> _
        Public Sub main(ByVal CmdArgs As String())
            If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                DirMain.sysConn = Sys.GetSysConn
                If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                    DirMain.sysConn.Close()
                    DirMain.sysConn = Nothing
                Else
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "AcctCustomers"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    Try
                        DirMain.strKeyCust = Strings.Replace(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c), "%", " ", 1, -1, CompareMethod.Binary)
                    Catch exception1 As exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As exception = exception1
                        DirMain.strKeyCust = "1=1"
                        ProjectData.ClearProjectError()
                    End Try
                    DirMain.PrintReport()
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Sub Print(ByVal nType As Integer)
            Dim num2 As Integer
            DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.Select(0)
            Dim flag As Boolean = False
            Dim num9 As Integer = (DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count - 1)
            num2 = 2
            Do While (num2 <= num9)
                If BooleanType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(num2).Item("Tag")) Then
                    flag = True
                    Exit Do
                End If
                num2 += 1
            Loop
            If Not flag Then
                Msg.Alert(StringType.FromObject(DirMain.oLan.Item("401")), 2)
            Else
                Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
                Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
                Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
                If (StringType.StrCmp(Strings.Trim(DirMain.fPrint.txtKieu.Text), "1", False) = 0) Then
                    Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
                    Dim num8 As Integer = (getGrid.GetDataView.Count - 1)
                    num2 = 2
                    Do While (num2 <= num8)
                        If BooleanType.FromObject(getGrid.GetDataView.Item(num2).Item("Tag")) Then
                            getGrid.GetDataView.Item(num2).Item("Tag") = False
                            DirMain.strCustID = Strings.Trim(StringType.FromObject(getGrid.GetDataView.Item(num2).Item("ma_kh")))
                            DirMain.strCustName = Strings.Trim(StringType.FromObject(Sql.GetValue((DirMain.appConn), "dmkh", StringType.FromObject(ObjectType.AddObj("ten_kh", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))), ("ma_kh = '" & DirMain.strCustID & "'"))))
                            Dim ds As New DataSet
                            Dim str2 As String = "EXEC sp18DCCustomer "
                            str2 = (StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str2, Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strAccount, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strCustID, "")))), ObjectType.AddObj(ObjectType.AddObj(", '", Reg.GetRegistryKey("Language")), "'"))) & ", 0")
                            str2 += ",0"
                            Sql.SQLRetrieve((DirMain.appConn), str2, "arso1", (ds))
                            Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
                            clsprint.oRpt.SetDataSource(ds.Tables.Item("arso1"))
                            clsprint.oVar = DirMain.oVar
                            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, "DCCustomer", DirMain.oOption, clsprint.oRpt)
                            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
                            clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
                            clsprint.oRpt.SetParameterValue("strAccount", DirMain.strAccount)
                            clsprint.oRpt.SetParameterValue("strAccountName", DirMain.strAccountName)
                            clsprint.oRpt.SetParameterValue("strCustID", DirMain.strCustID)
                            clsprint.oRpt.SetParameterValue("strCustName", DirMain.strCustName)
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(getGrid.GetDataView.Item(num2).Item("No_dk"))) Then
                                getGrid.GetDataView.Item(num2).Item("No_dk") = 0
                            End If
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(getGrid.GetDataView.Item(num2).Item("Co_dk"))) Then
                                getGrid.GetDataView.Item(num2).Item("Co_dk") = 0
                            End If
                            clsprint.oRpt.SetParameterValue("n_du_no", RuntimeHelpers.GetObjectValue(getGrid.GetDataView.Item(num2).Item("No_dk")))
                            clsprint.oRpt.SetParameterValue("n_du_co", RuntimeHelpers.GetObjectValue(getGrid.GetDataView.Item(num2).Item("Co_dk")))
                            Try
                                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(getGrid.GetDataView.Item(num2).Item("No_dk_nt"))) Then
                                    getGrid.GetDataView.Item(num2).Item("No_dk_nt") = 0
                                End If
                                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(getGrid.GetDataView.Item(num2).Item("Co_dk_nt"))) Then
                                    getGrid.GetDataView.Item(num2).Item("Co_dk_nt") = 0
                                End If
                                clsprint.oRpt.SetParameterValue("n_du_no_nt", RuntimeHelpers.GetObjectValue(getGrid.GetDataView.Item(num2).Item("No_dk_nt")))
                                clsprint.oRpt.SetParameterValue("n_du_co_nt", RuntimeHelpers.GetObjectValue(getGrid.GetDataView.Item(num2).Item("Co_dk_nt")))
                                clsprint.oRpt.SetParameterValue("h_so_ps_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("302")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
                            Catch exception1 As exception
                                ProjectData.SetProjectError(exception1)
                                Dim exception As exception = exception1
                                ProjectData.ClearProjectError()
                            End Try
                            If (nType = 0) Then
                                clsprint.PrintReport(1)
                            Else
                                clsprint.ShowReports()
                            End If
                            clsprint.oRpt.Close()
                            ds = Nothing
                        End If
                        num2 += 1
                    Loop
                    getGrid = Nothing
                Else
                    strFile = StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Interaction.IIf(((selectedIndex = 0) Or (selectedIndex = 2)), "arso1t.rpt", "arso1ta.rpt")))
                    Dim zero As Decimal = Decimal.Zero
                    Dim num3 As Decimal = Decimal.Zero
                    Dim num6 As Decimal = Decimal.Zero
                    Dim num5 As Decimal = Decimal.Zero
                    Dim set2 As New DataSet
                    Dim browse As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
                    Dim num7 As Integer = (browse.GetDataView.Count - 1)
                    num2 = 2
                    Do While (num2 <= num7)
                        If BooleanType.FromObject(browse.GetDataView.Item(num2).Item("Tag")) Then
                            browse.GetDataView.Item(num2).Item("Tag") = False
                            DirMain.strCustID = Strings.Trim(StringType.FromObject(browse.GetDataView.Item(num2).Item("ma_kh")))
                            DirMain.strCustName = Strings.Trim(StringType.FromObject(Sql.GetValue((DirMain.appConn), "dmkh", StringType.FromObject(ObjectType.AddObj("ten_kh", Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "V", False) = 0), "", "2"))), ("ma_kh = '" & DirMain.strCustID & "'"))))
                            Dim str3 As String = "EXEC fs_DCCustomerE "
                            str3 = (StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str3, Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strAccount, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strCustID, "")))), ObjectType.AddObj(ObjectType.AddObj(", '", Reg.GetRegistryKey("Language")), "'"))) & ", 0")
                            Sql.SQLRetrieve((DirMain.appConn), str3, "arso1", (set2))
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(browse.GetDataView.Item(num2).Item("No_dk"))) Then
                                browse.GetDataView.Item(num2).Item("No_dk") = 0
                            End If
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(browse.GetDataView.Item(num2).Item("Co_dk"))) Then
                                browse.GetDataView.Item(num2).Item("Co_dk") = 0
                            End If
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(browse.GetDataView.Item(num2).Item("No_dk_nt"))) Then
                                browse.GetDataView.Item(num2).Item("No_dk_nt") = 0
                            End If
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(browse.GetDataView.Item(num2).Item("Co_dk_nt"))) Then
                                browse.GetDataView.Item(num2).Item("Co_dk_nt") = 0
                            End If
                            zero = DecimalType.FromObject(ObjectType.AddObj(zero, browse.GetDataView.Item(num2).Item("No_dk")))
                            num3 = DecimalType.FromObject(ObjectType.AddObj(num3, browse.GetDataView.Item(num2).Item("Co_dk")))
                            num6 = DecimalType.FromObject(ObjectType.AddObj(num6, browse.GetDataView.Item(num2).Item("No_dk_nt")))
                            num5 = DecimalType.FromObject(ObjectType.AddObj(num5, browse.GetDataView.Item(num2).Item("Co_dk_nt")))
                        End If
                        num2 += 1
                    Loop
                    Dim clsprint2 As New clsprint(browse.GetForm, strFile, Nothing)
                    clsprint2.oRpt.SetDataSource(set2.Tables.Item("arso1"))
                    clsprint2.oVar = DirMain.oVar
                    clsprint2.SetReportVar(DirMain.sysConn, DirMain.appConn, "AcctCustomers", DirMain.oOption, clsprint2.oRpt)
                    clsprint2.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
                    clsprint2.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
                    clsprint2.oRpt.SetParameterValue("strAccount", DirMain.strAccount)
                    clsprint2.oRpt.SetParameterValue("strAccountName", DirMain.strAccountName)
                    Try
                        clsprint2.oRpt.SetParameterValue("n_no_dk", zero)
                        clsprint2.oRpt.SetParameterValue("n_co_dk", num3)
                    Catch exception4 As exception
                        ProjectData.SetProjectError(exception4)
                        Dim exception2 As exception = exception4
                        ProjectData.ClearProjectError()
                    End Try
                    Try
                        clsprint2.oRpt.SetParameterValue("n_no_dk_nt", num6)
                        clsprint2.oRpt.SetParameterValue("n_co_dk_nt", num5)
                        clsprint2.oRpt.SetParameterValue("h_so_ps_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("302")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
                    Catch exception5 As exception
                        ProjectData.SetProjectError(exception5)
                        Dim exception3 As exception = exception5
                        ProjectData.ClearProjectError()
                    End Try
                    If (nType = 0) Then
                        clsprint2.PrintReport(1)
                    Else
                        clsprint2.ShowReports()
                    End If
                    clsprint2.oRpt.Close()
                    browse = Nothing
                    set2 = Nothing
                End If
            End If
        End Sub

        Public Sub PrintReport()
            DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
            DirMain.fPrint.ShowDialog()
            DirMain.fPrint.Dispose()
            DirMain.sysConn.Close()
            DirMain.appConn.Close()
        End Sub

        Private Sub ReportDetailProc(ByVal nIndex As Integer)
            If (nIndex = 0) Then
                Dim str As String
                If (ObjectType.ObjTst(Reg.GetRegistryKey("language"), "V", False) = 0) Then
                    str = StringType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow.Item("ten_kh"))
                Else
                    str = StringType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow.Item("ten_kh2"))
                End If
                str = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(str), "", False) <> 0), (Strings.Trim(DirMain.strCustID) & " - " & Strings.Trim(str)), Strings.Trim(DirMain.strCustID)))
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s1", DirMain.strAccount, 1, -1, CompareMethod.Binary)
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s2", Strings.Trim(str), 1, -1, CompareMethod.Binary)
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text)
                clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "so_luong").HeaderText = ""
                clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "so_luong").Width = 0
                Try
                    clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "gia").HeaderText = ""
                    clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "gia").Width = 0
                    clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "tien").HeaderText = ""
                    clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "tien").Width = 0
                Catch exception1 As exception
                    ProjectData.SetProjectError(exception1)
                    Dim exception As exception = exception1
                    ProjectData.ClearProjectError()
                End Try
                Try
                    clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "gia_nt").HeaderText = ""
                    clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "gia_nt").Width = 0
                    clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "tien_nt").HeaderText = ""
                    clsvoucher.clsVoucher.GetColumn(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetGrid, "tien_nt").Width = 0
                Catch exception3 As exception
                    ProjectData.SetProjectError(exception3)
                    Dim exception2 As exception = exception3
                    ProjectData.ClearProjectError()
                End Try
            End If
        End Sub

        Private Sub ReportProc(ByVal nIndex As Integer)
            Dim getGrid As ReportBrowse
            Select Case nIndex
                Case 0
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text, "%s", DirMain.strAccount, 1, -1, CompareMethod.Binary)
                    getGrid = DirMain.oDirFormLib.GetClsreports.GetGrid
                    getGrid.GetGrid.ReadOnly = False
                    getGrid.GetDataView.AllowDelete = False
                    getGrid.GetDataView.AllowNew = False
                    Dim num As Integer = 0
                    Do While (1 <> 0)
                        Try
                            num += 1
                            getGrid.GetGrid.TableStyles.Item(0).GridColumnStyles.Item(num).ReadOnly = True
                        Catch exception1 As exception
                            ProjectData.SetProjectError(exception1)
                            Dim exception As exception = exception1
                            ProjectData.ClearProjectError()
                            Exit Do
                        End Try
                    Loop
                    Exit Select
                Case 1
                    If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                        DirMain.strCustID = Strings.Trim(StringType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow.Item("ma_kh")))
                        If (StringType.StrCmp(Strings.Trim(DirMain.strCustID), "", False) <> 0) Then
                            Dim str As String = ((StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strAccount, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strCustID, "")))) & ", '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("Language"))) & "'") & ", 0")
                            DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                            oDirFormDetailLib.sysConn = DirMain.sysConn
                            oDirFormDetailLib.appConn = DirMain.appConn
                            oDirFormDetailLib.oLan = DirMain.oLan
                            oDirFormDetailLib.oLen = DirMain.oLen
                            oDirFormDetailLib.oVar = DirMain.oVar
                            oDirFormDetailLib.SysID = "DCCustomer"
                            oDirFormDetailLib.cForm = "DCCustomer"
                            oDirFormDetailLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                            oDirFormDetailLib.strAliasReports = "arso1td"
                            oDirFormDetailLib.Init()
                            oDirFormDetailLib.strSQLRunReports = ("sp18DCCustomer " & Strings.Trim(str) & ",0")
                            RemoveHandler DirMain.oDirFormLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportProc)
                            AddHandler DirMain.oDirFormDetailLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                            oDirFormDetailLib.Show()
                            RemoveHandler DirMain.oDirFormDetailLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                            AddHandler DirMain.oDirFormLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportProc)
                            DirMain.oDirFormDetailLib = Nothing
                        End If
                        Return
                    End If
                    Return
                Case 2
                    DirMain.Print(0)
                    Return
                Case 3
                    DirMain.Print(1)
                    Return
                Case Else
                    Return
            End Select
            AddHandler getGrid.GetGrid.KeyUp, New KeyEventHandler(AddressOf DirMain.grdKeyup)
            getGrid = Nothing
        End Sub

        Private Sub SelectRows(ByVal lType As Boolean)
            Dim num2 As Integer = (DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                DirMain.oDirFormLib.GetClsreports.GetGrid.GetDataView.Item(i).Item("Tag") = lType
                i += 1
            Loop
        End Sub

        Public Sub ShowReport()
            Dim str As String = "EXEC fs_AcctCustomers "
            str = StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strAccount, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh1.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh2.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh3.Text, ""))))
            DirMain.oDirFormLib = New reportformlib("1011111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "arso1t"
            oDirFormLib.Init()
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show()
            RemoveHandler oDirFormLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportProc)
            DirMain.oDirFormLib = Nothing
        End Sub


        ' Fields
        Public appConn As SqlConnection
        Public dFrom As DateTime
        Public dTo As DateTime
        Public fPrint As frmFilter = New frmFilter
        Private oDirFormDetailLib As reportformlib
        Private oDirFormLib As reportformlib
        Public oLan As Collection = New Collection
        Public oLen As Collection = New Collection
        Public oOption As Collection = New Collection
        Public oVar As Collection = New Collection
        Public rpTable As DataTable
        Public strAccount As String
        Public strAccountName As String
        Private strCustID As String
        Private strCustName As String
        Public strKeyCust As String
        Public strUnit As String
        Public sysConn As SqlConnection
        Public SysID As String
    End Module
End Namespace

