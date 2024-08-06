Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib
Imports libscontrol.voucherseachlib

Namespace arcd1
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
                    Try 
                        DirMain.strKeyCust = Strings.Replace(Fox.GetWordNum(Strings.Trim(CmdArgs(0)), 1, "#"c), "%", " ", 1, -1, CompareMethod.Binary)
                    Catch exception1 As Exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As Exception = exception1
                        DirMain.strKeyCust = "1=1"
                        ProjectData.ClearProjectError
                    End Try
                    DirMain.appConn = Sys.GetConn
                    Sys.InitVar(DirMain.sysConn, DirMain.oVar)
                    Sys.InitOptions(DirMain.appConn, DirMain.oOption)
                    Sys.InitColumns(DirMain.sysConn, DirMain.oLen)
                    DirMain.SysID = "CustomersSummary"
                    Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                    DirMain.PrintReport
                    DirMain.rpTable = Nothing
                End If
            End If
        End Sub

        Private Shared Sub Print(ByVal nType As Integer)
            DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.Select(0)
            Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("301"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
            Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
            Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            clsprint.oVar = DirMain.oVar
            clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
            clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
            clsprint.oRpt.SetParameterValue("strAccount", DirMain.strAccount)
            clsprint.oRpt.SetParameterValue("strAccountName", DirMain.strAccountName)
            If (nType = 0) Then
                clsprint.PrintReport(1)
                clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
            Else
                clsprint.ShowReports
            End If
            clsprint.oRpt.Close
            getGrid = Nothing
        End Sub

        Public Shared Sub PrintReport()
            DirMain.rpTable = clsprint.InitComboReport(DirMain.sysConn, DirMain.fPrint.cboReports, DirMain.SysID)
            DirMain.fPrint.ShowDialog
            DirMain.fPrint.Dispose
            DirMain.sysConn.Close
            DirMain.appConn.Close
        End Sub

        Private Shared Sub ReportDetailProc(ByVal nIndex As Integer)
            If (nIndex = 0) Then
                Dim str As String
                If (ObjectType.ObjTst(Reg.GetRegistryKey("language"), "V", False) = 0) Then
                    str = StringType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow.Item("ten_kh"))
                Else
                    str = StringType.FromObject(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow.Item("ten_kh2"))
                End If
                str = StringType.FromObject(Interaction.IIf((StringType.StrCmp(Strings.Trim(str), "", False) <> 0), (Strings.Trim(DirMain.strCustID) & " - " & Strings.Trim(str)), Strings.Trim(DirMain.strCustID)))
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", Strings.Trim(str), 1, -1, CompareMethod.Binary)
                DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text)
            End If
        End Sub

        Private Shared Sub ReportProc(ByVal nIndex As Integer)
            Select Case nIndex
                Case 0
                    DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormLib.GetClsreports.GetGrid.GetForm.Text, "%s", DirMain.strAccount, 1, -1, CompareMethod.Binary)
                    Exit Select
                Case 1
                    If Not Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                        Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                        If (Information.IsDBNull(RuntimeHelpers.GetObjectValue(curDataRow.Item("ma_kh"))) Or (StringType.StrCmp(Strings.Trim(StringType.FromObject(curDataRow.Item("ma_kh"))), "", False) = 0)) Then
                            Return
                        End If
                        DirMain.strCustID = Strings.Trim(StringType.FromObject(curDataRow.Item("ma_kh")))
                        If (StringType.StrCmp(Strings.Trim(DirMain.strCustID), "", False) = 0) Then
                            Return
                        End If
                        Dim str2 As String = ""
                        Dim cString As String = "ps_no, ps_co, ps_no_nt, ps_co_nt"
                        Dim num2 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                        Dim i As Integer = 1
                        Do While (i <= num2)
                            Dim str3 As String = Strings.Trim(Fox.GetWordNum(cString, i, ","c))
                            str2 = (str2 & Strings.Trim(StringType.FromObject(curDataRow.Item(str3))) & ", ")
                            i += 1
                        Loop
                        str2 = (str2 & "'" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("Language"))) & "', ")
                        curDataRow = Nothing
                        Dim strSQLLong As String = "(a.ngay_ct BETWEEN "
                        strSQLLong += Sql.ConvertVS2SQLType(DirMain.dFrom, "") + " AND " + Sql.ConvertVS2SQLType(DirMain.dTo, "") + ")"
                        strSQLLong += " AND a.tk LIKE '" & Strings.Trim(DirMain.strAccount) & "%'"
                        strSQLLong += " And dbo.ff_InUnits(a.ma_dvcs, '" + Strings.Trim(DirMain.strUnit) + "') = 1"
                        If strCustID.Trim <> "" Then
                            strSQLLong += " AND a.ma_kh = '" + DirMain.strCustID.Trim.Replace("'", "''") + "'"
                        End If
                        DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                            oDirFormDetailLib.sysConn = DirMain.sysConn
                            oDirFormDetailLib.appConn = DirMain.appConn
                            oDirFormDetailLib.oLan = DirMain.oLan
                            oDirFormDetailLib.oLen = DirMain.oLen
                            oDirFormDetailLib.oVar = DirMain.oVar
                            oDirFormDetailLib.SysID = DirMain.SysID
                            oDirFormDetailLib.cForm = "Detail"
                            oDirFormDetailLib.cCode = StringType.FromObject(Interaction.IIf((DirMain.fPrint.cboReports.SelectedIndex = 0), "171", "172"))
                            oDirFormDetailLib.strAliasReports = "arcd1d"
                            oDirFormDetailLib.Init()
                        oDirFormDetailLib.strSQLRunReports = ("sp20ReportDetailAccount " & str2 & vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10))
                        RemoveHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                            AddHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                            oDirFormDetailLib.Show()
                            RemoveHandler oDirFormDetailLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportDetailProc)
                            AddHandler DirMain.oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
                            DirMain.oDirFormDetailLib = Nothing
                            Exit Select
                        End If
                        Return
                Case 2
                    DirMain.Print(0)
                    Exit Select
                Case 3
                    DirMain.Print(1)
                    Exit Select
            End Select
        End Sub

        Public Shared Sub ShowReport()
            Dim str As String = "EXEC sp20CustomersSummary_DX1 "
            str = (((StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType(DirMain.fPrint.txtDFrom.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtDTo.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.strAccount, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_kh.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh1.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh2.Text, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_nh3.Text, "")))) & ", '" & DirMain.strGroups & "'") & ", '" & DirMain.strOrder & "'") & ", " & vouchersearchlibobj.ConvertLong2ShortStrings(DirMain.strKey, 10))
            DirMain.oDirFormLib = New reportformlib("1011111111")
            oDirFormLib.sysConn = DirMain.sysConn
            oDirFormLib.appConn = DirMain.appConn
            oDirFormLib.oLan = DirMain.oLan
            oDirFormLib.oLen = DirMain.oLen
            oDirFormLib.oVar = DirMain.oVar
            oDirFormLib.SysID = DirMain.SysID
            oDirFormLib.cForm = DirMain.SysID
            oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
            oDirFormLib.strAliasReports = "arcd1"
            oDirFormLib.Init
            oDirFormLib.strSQLRunReports = str
            AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            oDirFormLib.Show
            RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
            DirMain.oDirFormLib = Nothing
        End Sub


        ' Fields
        Public Shared appConn As SqlConnection
        Public Shared dFrom As DateTime
        Public Shared dTo As DateTime
        Public Shared fPrint As frmFilter = New frmFilter
        Private Shared oDirFormDetailLib As reportformlib
        Private Shared oDirFormLib As reportformlib
        Public Shared oLan As Collection = New Collection
        Public Shared oLen As Collection = New Collection
        Public Shared oOption As Collection = New Collection
        Public Shared oVar As Collection = New Collection
        Public Shared rpTable As DataTable
        Public Shared strAccount As String
        Public Shared strAccountName As String
        Public Shared strAccountRef As String
        Private Shared strCustID As String
        Private Shared strCustName As String
        Public Shared strGroups As String
        Public Shared strKey As String
        Public Shared strKeyCust As String
        Public Shared strOrder As String
        Public Shared strUnit As String
        Public Shared sysConn As SqlConnection
        Public Shared SysID As String
    End Class
End Namespace

