Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.clsvoucher.clsVoucher

Namespace arso1
    <StandardModule> _
    Friend NotInheritable Class DirMain
        ' Methods
        <STAThread> _
        Public Shared Sub main(ByVal CmdArgs As String())
            If Not BooleanType.FromObject(ObjectType.BitAndObj(Not Sys.isLogin, (ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0))) Then
                DirMain.sysConn = Sys.GetSysConn
                If ((ObjectType.ObjTst(Reg.GetRegistryKey("Customize"), "0", False) = 0) AndAlso Not Sys.CheckRights(DirMain.sysConn, "Access")) Then
                    DirMain.sysConn.Close
                    DirMain.sysConn = Nothing
                Else
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "DCCustomer"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    Try 
                        DirMain.strKeyCust = Strings.Replace(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c), "%", " ", 1, -1, CompareMethod.Binary)
                    Catch exception1 As Exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As Exception = exception1
                        DirMain.strKeyCust = "1=1"
                        ProjectData.ClearProjectError
                    End Try
                    DirMain.PrintReport
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Shared Sub Print(ByVal nType As Integer)
            Dim obj2 As Object
            Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim sort As String = getGrid.GetDataView.Sort
            getGrid.GetDataView.Sort = ""
            DirMain.nBgDrAmt = DecimalType.FromObject(getGrid.GetDataView.Item(0).Item("ps_no"))
            DirMain.nBgCrAmt = DecimalType.FromObject(getGrid.GetDataView.Item(0).Item("ps_co"))
            DirMain.nBgFCDrAmt = DecimalType.FromObject(getGrid.GetDataView.Item(0).Item("ps_no_nt"))
            DirMain.nBgFCCrAmt = DecimalType.FromObject(getGrid.GetDataView.Item(0).Item("ps_co_nt"))
            getGrid.GetDataView.Sort = sort
            getGrid = Nothing
            Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(Reg.GetRegistryKey("ReportDir"))
            If (StringType.StrCmp(DirMain.fPrint.txtChi_tiet.Text, "0", False) = 0) Then
                strFile = (strFile & Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file"))) & ".rpt")
            Else
                Select Case selectedIndex
                    Case 0, 2
                        strFile = (strFile & "arso1b.rpt")
                        goto Label_016B
                    Case 1, 3
                        strFile = (strFile & "arso1c.rpt")
                        goto Label_016B
                End Select
            End If
        Label_016B:
            obj2 = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
            Dim browse As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim clsprint As New clsprint(browse.GetForm, strFile, Nothing)
            clsprint.oRpt.SetDataSource(browse.GetDataView.Table)
            clsprint.oVar = DirMain.oVar
            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
            clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
            clsprint.oRpt.SetParameterValue("strAccount", DirMain.strAccount)
            clsprint.oRpt.SetParameterValue("strAccountName", DirMain.strAccountName)
            clsprint.oRpt.SetParameterValue("strCustID", DirMain.strCustID)
            clsprint.oRpt.SetParameterValue("strCustName", DirMain.strCustName)
            clsprint.oRpt.SetParameterValue("n_du_no", DirMain.nBgDrAmt)
            clsprint.oRpt.SetParameterValue("n_du_co", DirMain.nBgCrAmt)
            Try 
                clsprint.oRpt.SetParameterValue("h_so_ps_vnd", Strings.Replace(StringType.FromObject(DirMain.oLan.Item("302")), "%s", StringType.FromObject(DirMain.oOption.Item("m_ma_nt0")), 1, -1, CompareMethod.Binary))
                clsprint.oRpt.SetParameterValue("n_du_no_nt", DirMain.nBgFCDrAmt)
                clsprint.oRpt.SetParameterValue("n_du_co_nt", DirMain.nBgFCCrAmt)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                ProjectData.ClearProjectError
            End Try
            If (nType = 0) Then
                clsprint.PrintReport(1)
                clsprint.oRpt.SetDataSource(browse.GetDataView.Table)
            Else
                clsprint.ShowReports
            End If
            clsprint.oRpt.Close
            browse = Nothing
        End Sub

        Public Shared Sub PrintReport()
            DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
            DirMain.fPrint.ShowDialog
            DirMain.fPrint.Dispose
            DirMain.sysConn.Close
            DirMain.appConn.Close
        End Sub

        Private Shared Sub ReportProc(ByVal nIndex As Integer)
            Select Case nIndex
                Case 0
                    Dim str As String = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(DirMain.fPrint.lblTen_kh.Text), "", False) <> 0), (Strings.Trim(DirMain.fPrint.txtMa_kh.Text) & " - " & Strings.Trim(DirMain.fPrint.lblTen_kh.Text)), Strings.Trim(DirMain.fPrint.txtMa_kh.Text)))
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text, "%s1", Strings.Trim(DirMain.fPrint.txtTk.Text), 1, -1, CompareMethod.Binary)
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text, "%s2", Strings.Trim(str), 1, -1, CompareMethod.Binary)
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text)
                    If (DoubleType.FromString(DirMain.fPrint.txtChi_tiet.Text) = 0) Then
                        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "so_luong").HeaderText = ""
                        GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "so_luong").Width = 0
                        Try
                            GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "gia").HeaderText = ""
                            GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "gia").Width = 0
                            GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "tien").HeaderText = ""
                            GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "tien").Width = 0
                        Catch exception1 As exception
                            ProjectData.SetProjectError(exception1)
                            Dim exception As exception = exception1
                            ProjectData.ClearProjectError()
                        End Try
                        Try
                            GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "gia_nt").HeaderText = ""
                            GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "gia_nt").Width = 0
                            GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "tien_nt").HeaderText = ""
                            GetColumn(DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid, "tien_nt").Width = 0
                        Catch exception3 As exception
                            ProjectData.SetProjectError(exception3)
                            Dim exception2 As exception = exception3
                            ProjectData.ClearProjectError()
                        End Try
                    End If
                    Exit Select
                Case 2
                    DirMain.Print(0)
                    Exit Select
                Case 3
                    DirMain.Print(1)
                    Exit Select
            End Select
        End Sub

        Public Shared Sub ShowReport()
            Try
                Dim str As String = "EXEC sp20DCCustomer_DX1 "
                str += Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtTk.Text, "")
                str += ", " + Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")
                str += ", '" + Reg.GetRegistryKey("Language") + "'"
                str += ", " + DirMain.fPrint.txtChi_tiet.Text
                str += ", " + DirMain.fPrint.txtGroup_voucher.Text
                DirMain.oDirFormLib = New reportformlib("0111111111")
                oDirFormLib.sysConn = DirMain.sysConn
                oDirFormLib.appConn = DirMain.appConn
                oDirFormLib.oLan = DirMain.oLan
                oDirFormLib.oLen = DirMain.oLen
                oDirFormLib.oVar = DirMain.oVar
                oDirFormLib.SysID = DirMain.SysID
                oDirFormLib.cForm = DirMain.SysID
                oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
                oDirFormLib.strAliasReports = "arso1"
                oDirFormLib.Init()
                oDirFormLib.strSQLRunReports = str
                AddHandler oDirFormLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportProc)
                oDirFormLib.Show()
                RemoveHandler oDirFormLib.ReportProc, New reportformlib.ReportProcEventHandler(AddressOf DirMain.ReportProc)
                DirMain.oDirFormLib = Nothing
            Catch ex As Exception
                Msg.Alert(ex.ToString())
            End Try
        End Sub


        ' Fields
        Public Shared appConn As SqlConnection
        Public Shared dFrom As DateTime
        Public Shared dTo As DateTime
        Public Shared fPrint As frmFilter = New frmFilter
        Private Shared nBgCrAmt As Decimal
        Private Shared nBgDrAmt As Decimal
        Private Shared nBgFCCrAmt As Decimal
        Private Shared nBgFCDrAmt As Decimal
        Private Shared oDirFormLib As reportformlib
        Public Shared oLan As Collection = New Collection
        Public Shared oLen As Collection = New Collection
        Public Shared oOption As Collection = New Collection
        Public Shared oVar As Collection = New Collection
        Public Shared rpTable As DataTable
        Public Shared strAccount As String
        Public Shared strAccountName As String
        Public Shared strCustID As String
        Public Shared strCustName As String
        Public Shared strKeyCust As String
        Public Shared strUnit As String
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

