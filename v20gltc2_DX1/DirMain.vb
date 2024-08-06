Imports System.Windows.Forms
Imports System.Drawing
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Collections
Imports libscommon
Imports libscontrol
Imports libscontrol.reportformlib
Imports libscontrol.voucherseachlib

Module DirMain
    ' Methods
    Private Sub Grid_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim enumerator As IEnumerator
        Try
            enumerator = DirectCast(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "TableStyles", New Object() {0}, Nothing, Nothing), Nothing, "GridColumnStyles", New Object(0 - 1) {}, Nothing, Nothing), IEnumerable).GetEnumerator
            Do While enumerator.MoveNext
                Dim current As DataGridColumnStyle = DirectCast(enumerator.Current, DataGridColumnStyle)
                Dim objArray6 As Object() = New Object(1 - 1) {}
                Dim o As Object = sender
                objArray6(0) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(o, Nothing, "CurrentRowIndex", New Object(0 - 1) {}, Nothing, Nothing))
                Dim args As Object() = objArray6
                Dim copyBack As Boolean() = New Boolean() {True}
                If copyBack(0) Then
                    LateBinding.LateSetComplex(o, Nothing, "CurrentRowIndex", New Object() {RuntimeHelpers.GetObjectValue(args(0))}, Nothing, True, False)
                End If
                Dim obj2 As Object = LateBinding.LateGet(sender, Nothing, "DataSource", args, Nothing, copyBack)
                current.ReadOnly = (StringType.StrCmp((Strings.Trim(StringType.FromObject(LateBinding.LateGet(obj2, Nothing, "item", New Object() {"tk_no"}, Nothing, Nothing))) & Strings.Trim(StringType.FromObject(LateBinding.LateGet(obj2, Nothing, "item", New Object() {"tk_co"}, Nothing, Nothing))) & Strings.Trim(StringType.FromObject(LateBinding.LateGet(obj2, Nothing, "item", New Object() {"cach_tinh"}, Nothing, Nothing)))), "", False) <> 0)
                obj2 = Nothing
            Loop
        Catch
        End Try
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
                DirMain.SysID = "v20ISPart1"
                Sys.InitMessage(DirMain.sysConn, DirMain.oLan, DirMain.SysID)
                clsHTKK.oOption = DirMain.oOption
                Sys.InitMessage(DirMain.sysConn, clsHTKK.oLan, "clsHTKK")
                DirMain.PrintReport()
                DirMain.rpTable = Nothing
                DirMain.rpTypeTable = Nothing
            End If
        End If
    End Sub

    Private Sub mnuClick(ByVal sender As Object, ByVal e As EventArgs)
        If (ObjectType.ObjTst(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing), 5, False) = 0) Then
            clsHTKK.ExportToHTKK(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("TAX")), New DataView(DirMain.oDirFormLib.GetClsreports.ob.GetDataView.Table.Copy), DirMain.fPrint.txtNgay_ct11.Value, DirMain.fPrint.txtNgay_ct12.Value)
        End If
    End Sub

    Private Sub Print(ByVal nType As Integer)
        Dim selectedIndex As Integer = DirMain.fPrint.cboReports.SelectedIndex
        Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
        Dim obj2 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("302"))), "%d1", StringType.FromDate(DirMain.dFrom), 1, -1, CompareMethod.Binary)), "%d2", StringType.FromDate(DirMain.dTo), 1, -1, CompareMethod.Binary)
        Dim obj4 As Object = Strings.Replace(StringType.FromObject(Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("303"))), "%n1", DirMain.fPrint.txtQuy.Text, 1, -1, CompareMethod.Binary)), "%n2", DirMain.fPrint.txtNam.Text, 1, -1, CompareMethod.Binary)
        Dim obj5 As Object = Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("304"))), "%n2", DirMain.fPrint.txtNam.Text, 1, -1, CompareMethod.Binary)
        Dim obj3 As Object = Strings.Replace(StringType.FromObject(RuntimeHelpers.GetObjectValue(DirMain.oLan.Item("305"))), "%n1", DirMain.fPrint.txtQuy.Text, 1, -1, CompareMethod.Binary)
        Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
        Dim clsprint As New clsprint(getGrid.GetForm, strFile, Nothing)
        clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        clsprint.dr = clsprint.GetdtRowFromCtrl(DirMain.fPrint.tbgFilter)
        clsprint.dr.Table.Columns.Add("title1", GetType(String))
        clsprint.dr.Table.Columns.Add("title2", GetType(String))
        clsprint.dr.Item("title1") = Strings.Trim(StringType.FromObject(DirMain.rpTypeTable.Rows.Item(DirMain.fPrint.cboReportType.SelectedIndex).Item("title")))
        clsprint.dr.Item("title2") = Strings.Trim(StringType.FromObject(DirMain.rpTypeTable.Rows.Item(DirMain.fPrint.cboReportType.SelectedIndex).Item("title2")))
        clsprint.oVar = DirMain.oVar
        clsprint.SetReportVar(DirMain.sysConn, DirMain.appConn, DirMain.SysID, DirMain.oOption, clsprint.oRpt)
        clsprint.oRpt.SetParameterValue("Title", Strings.Trim(DirMain.fPrint.txtTitle.Text))
        clsprint.oRpt.SetParameterValue("t_date", RuntimeHelpers.GetObjectValue(obj2))
        Try
            Dim num As Integer
            Dim str2 As String
            If (StringType.StrCmp(Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("bilingual_fr"))), "", False) = 0) Then
                num = 1
                Do
                    str2 = Strings.Replace("h_line%i", "%i", Strings.Format(num, "0"), 1, -1, CompareMethod.Binary)
                    clsprint.oRpt.SetParameterValue(str2, Strings.Trim(StringType.FromObject(LateBinding.LateGet(DirMain.rpTypeTable.Rows.Item(DirMain.fPrint.cboReportType.SelectedIndex), Nothing, "Item", New Object() {ObjectType.AddObj(str2, Interaction.IIf((ObjectType.ObjTst(Reg.GetRegistryKey("Language"), "E", False) = 0), "2", ""))}, Nothing, Nothing))))
                    num += 1
                Loop While (num <= 5)
            ElseIf (StringType.StrCmp(Fox.GetWordNum(StringType.FromObject(DirMain.rpTable.Rows.Item(selectedIndex).Item("bilingual_fr")), 1, ","c), "1", False) = 0) Then
                num = 1
                Do
                    str2 = Strings.Replace("h_line%i", "%i", Strings.Format(num, "0"), 1, -1, CompareMethod.Binary)
                    clsprint.oRpt.SetParameterValue(("1" & str2), Strings.Trim(StringType.FromObject(DirMain.rpTypeTable.Rows.Item(DirMain.fPrint.cboReportType.SelectedIndex).Item(str2))))
                    clsprint.oRpt.SetParameterValue(("2" & str2), Strings.Trim(StringType.FromObject(DirMain.rpTypeTable.Rows.Item(DirMain.fPrint.cboReportType.SelectedIndex).Item((str2 & "2")))))
                    num += 1
                Loop While (num <= 5)
            Else
                num = 1
                Do
                    str2 = Strings.Replace("h_line%i", "%i", Strings.Format(num, "0"), 1, -1, CompareMethod.Binary)
                    clsprint.oRpt.SetParameterValue(("2" & str2), Strings.Trim(StringType.FromObject(DirMain.rpTypeTable.Rows.Item(DirMain.fPrint.cboReportType.SelectedIndex).Item(str2))))
                    clsprint.oRpt.SetParameterValue(("1" & str2), Strings.Trim(StringType.FromObject(DirMain.rpTypeTable.Rows.Item(DirMain.fPrint.cboReportType.SelectedIndex).Item((str2 & "2")))))
                    num += 1
                Loop While (num <= 5)
            End If
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
        Try
            clsprint.oRpt.SetParameterValue("t_QuaterYear", RuntimeHelpers.GetObjectValue(obj4))
            clsprint.oRpt.SetParameterValue("t_Year", RuntimeHelpers.GetObjectValue(obj5))
            clsprint.oRpt.SetParameterValue("t_Quater", RuntimeHelpers.GetObjectValue(obj3))
        Catch exception3 As Exception
            ProjectData.SetProjectError(exception3)
            Dim exception2 As Exception = exception3
            ProjectData.ClearProjectError()
        End Try
        If (nType = 0) Then
            clsprint.PrintReport(1)
            clsprint.oRpt.SetDataSource(getGrid.GetDataView.Table)
        Else
            clsprint.ShowReports()
        End If
        clsprint.oRpt.Close()
        getGrid = Nothing
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
            Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
            Dim str As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Strings.Trim(StringType.FromObject(curDataRow.Item("Tk_no"))), Interaction.IIf(((StringType.StrCmp(Strings.Trim(StringType.FromObject(curDataRow.Item("Tk_no"))), "", False) <> 0) And (StringType.StrCmp(Strings.Trim(StringType.FromObject(curDataRow.Item("Tk_co"))), "", False) <> 0)), " - ", "")), Strings.Trim(StringType.FromObject(curDataRow.Item("Tk_co")))))
            curDataRow = Nothing
            DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Replace(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text, "%s", Strings.Trim(str), 1, -1, CompareMethod.Binary)
            DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text = Strings.Trim(DirMain.oDirFormDetailLib.GetClsreports.GetGrid.GetForm.Text)
        End If
    End Sub

    Private Sub ReportProc(ByVal nIndex As Integer)
        Select Case nIndex
            Case 0
                Dim getGrid As ReportBrowse = DirMain.oDirFormLib.GetClsreports.GetGrid
                getGrid.GetGrid.ReadOnly = False
                getGrid.GetDataView.AllowNew = False
                getGrid.GetDataView.AllowDelete = False
                AddHandler getGrid.GetGrid.CurrentCellChanged, New EventHandler(AddressOf DirMain.Grid_CurrentCellChanged)
                getGrid = Nothing
                DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(5).Style = ToolBarButtonStyle.PushButton
                DirMain.oDirFormLib.GetClsreports.tbr.Buttons.Item(5).ToolTipText = StringType.FromObject(DirMain.oLan.Item("800"))
                DirMain.oDirFormLib.GetClsreports.tbr.ImageList.Images.Item(5) = Image.FromFile(StringType.FromObject(ObjectType.AddObj(Reg.GetRegistryKey("ImageDir"), "export.bmp")))
                DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(5).Text = StringType.FromObject(DirMain.oLan.Item("800"))
                AddHandler DirMain.oDirFormLib.GetClsreports.mnFile.MenuItems.Item(5).Click, New EventHandler(AddressOf DirMain.mnuClick)
                AddHandler DirMain.oDirFormLib.GetClsreports.tbr.ButtonClick, New ToolBarButtonClickEventHandler(AddressOf DirMain.tbrClick)
                Exit Select
            Case 1
                Dim currentCell As DataGridCell = DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid.CurrentCell
                If (currentCell.RowNumber >= 0) Then
                    If Information.IsNothing(DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow) Then
                        Return
                    End If
                    Dim curDataRow As DataRowView = DirMain.oDirFormLib.GetClsreports.GetGrid.CurDataRow
                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(curDataRow.Item("cach_tinh"))), "", False) <> 0) Then
                        Return
                    End If
                    DirMain.strDrAccount = Strings.Trim(StringType.FromObject(curDataRow.Item("tk_no")))
                    DirMain.strCrAccount = Strings.Trim(StringType.FromObject(curDataRow.Item("tk_co")))
                    DirMain.intGiam_tru = IntegerType.FromObject(curDataRow.Item("Giam_tru"))
                    If ((StringType.StrCmp(DirMain.strDrAccount, "", False) = 0) And (StringType.StrCmp(DirMain.strCrAccount, "", False) = 0)) Then
                        Return
                    End If
                    Dim str2 As String = ""
                    curDataRow = Nothing
                    str2 = (str2 & "'" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("Language"))) & "', ")
                    Dim str As String = "1=1"
                    str = (str & " AND dbo.ff_InUnits(a.ma_dvcs, '" & Strings.Trim(DirMain.strUnit) & "') = 1")
                    Dim grid As DataGrid = DirMain.oDirFormLib.GetClsreports.GetGrid.GetGrid
                    If Fox.InList(grid.TableStyles.Item((grid.TableStyles.Count - 1)).GridColumnStyles.Item(currentCell.ColumnNumber).MappingName, New Object() {"ky_truoc", "ky_truoc_nt"}) Then
                        str = StringType.FromObject(ObjectType.AddObj(str, ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct BETWEEN ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNgay_ct01.Value, "")), " AND "), Sql.ConvertVS2SQLType(DirMain.fPrint.txtNgay_ct02.Value, "")), ")")))
                    Else
                        str = StringType.FromObject(ObjectType.AddObj(str, ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct BETWEEN ", Sql.ConvertVS2SQLType(DirMain.dFrom, "")), " AND "), Sql.ConvertVS2SQLType(DirMain.dTo, "")), ")")))
                    End If
                    grid = Nothing
                    If (StringType.StrCmp(DirMain.strDrAccount, "", False) <> 0) Then
                        str = (str & " AND dbo.ff_Inlist(a.Tk, '" & DirMain.strDrAccount & "') = 1")
                        If (DirMain.intGiam_tru <> 1) Then
                            str = (str & " AND Ps_no + Ps_no_nt <> 0")
                        Else
                            str = StringType.FromObject(ObjectType.AddObj(str, ObjectType.AddObj(ObjectType.AddObj(" AND Ps_no + Ps_no_nt <> 0 AND dbo.ff_Inlist(a.Tk_du, '", DirMain.oOption.Item("m_tk_kqkd")), "')<>1")))
                        End If
                        If (StringType.StrCmp(DirMain.strCrAccount, "", False) <> 0) Then
                            str = (str & " AND dbo.ff_Inlist(a.Tk_du, '" & DirMain.strCrAccount & "') = 1")
                        End If
                    Else
                        If (StringType.StrCmp(DirMain.strCrAccount, "", False) <> 0) Then
                            str = (str & " AND dbo.ff_Inlist(a.Tk, '" & DirMain.strCrAccount & "') = 1")
                        End If
                        If (DirMain.intGiam_tru <> 1) Then
                            str = (str & " AND Ps_co + Ps_co_nt <> 0")
                        Else
                            str = StringType.FromObject(ObjectType.AddObj(str, ObjectType.AddObj(ObjectType.AddObj(" AND Ps_co + Ps_co_nt <> 0 AND dbo.ff_Inlist(a.Tk_du, '", DirMain.oOption.Item("m_tk_kqkd")), "')<>1")))
                        End If
                    End If
                    DirMain.oDirFormDetailLib = New reportformlib("0111110001")
                    oDirFormDetailLib.sysConn = DirMain.sysConn
                    oDirFormDetailLib.appConn = DirMain.appConn
                    oDirFormDetailLib.oLan = DirMain.oLan
                    oDirFormDetailLib.oLen = DirMain.oLen
                    oDirFormDetailLib.oVar = DirMain.oVar
                    oDirFormDetailLib.SysID = DirMain.SysID
                    oDirFormDetailLib.cForm = "Detail"
                    oDirFormDetailLib.cCode = StringType.FromObject(Interaction.IIf((DirMain.fPrint.cboReports.SelectedIndex = 0), "151", "152"))
                    oDirFormDetailLib.strAliasReports = "v20GLTC2d"
                    oDirFormDetailLib.Init()
                    oDirFormDetailLib.strSQLRunReports = ("fs_ReportDetailAutoSum " & str2 & vouchersearchlibobj.ConvertLong2ShortStrings(str, 10))
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

    Public Sub ShowReport()
        Dim str As String = "EXEC sp20IncomeStatementPart1_DX1 "
        str = (StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(StringType.FromObject(ObjectType.AddObj(str, Sql.ConvertVS2SQLType(DirMain.fPrint.txtNgay_ct11.Value, ""))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNgay_ct12.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNgay_ct01.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtNgay_ct02.Value, "")))), ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(DirMain.fPrint.txtMa_dvcs.Text, "")))) & ", '" & Strings.Trim(StringType.FromObject(DirMain.rpTypeTable.Rows.Item(DirMain.fPrint.cboReportType.SelectedIndex).Item("form"))) & "'")
        DirMain.oDirFormLib = New reportformlib("1011111111")
        Dim oDirFormLib As reportformlib = DirMain.oDirFormLib
        oDirFormLib.sysConn = DirMain.sysConn
        oDirFormLib.appConn = DirMain.appConn
        oDirFormLib.oLan = DirMain.oLan
        oDirFormLib.oLen = DirMain.oLen
        oDirFormLib.oVar = DirMain.oVar
        oDirFormLib.SysID = DirMain.SysID
        oDirFormLib.cForm = DirMain.SysID
        oDirFormLib.cCode = Strings.Trim(StringType.FromObject(DirMain.rpTable.Rows.Item(DirMain.fPrint.cboReports.SelectedIndex).Item("rep_id")))
        oDirFormLib.strAliasReports = "v20GLTC2"
        oDirFormLib.Init()
        oDirFormLib.strSQLRunReports = str
        AddHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        oDirFormLib.Show()
        RemoveHandler oDirFormLib.ReportProc, New ReportProcEventHandler(AddressOf DirMain.ReportProc)
        oDirFormLib = Nothing
        DirMain.oDirFormLib = Nothing
    End Sub

    Private Sub tbrClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs)
        Dim objArray2 As Object() = New Object(1 - 1) {}
        Dim args As ToolBarButtonClickEventArgs = e
        objArray2(0) = args.Button
        Dim objArray As Object() = objArray2
        Dim copyBack As Boolean() = New Boolean() {True}
        If copyBack(0) Then
            args.Button = DirectCast(objArray(0), ToolBarButton)
        End If
        If (ObjectType.ObjTst(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "Buttons", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "IndexOf", objArray, Nothing, copyBack), 5, False) = 0) Then
            clsHTKK.ExportToHTKK(StringType.FromObject(DirMain.oDirFormLib.oLan.Item("TAX")), New DataView(DirMain.oDirFormLib.GetClsreports.ob.GetDataView.Table.Copy), DirMain.fPrint.txtNgay_ct11.Value, DirMain.fPrint.txtNgay_ct12.Value)
        End If
    End Sub


    ' Fields
    Public Const _RP_CODE As String = "v20GLTC2"
    Public appConn As SqlConnection
    Public dFrom As DateTime
    Public dTo As DateTime
    Public fPrint As frmFilter = New frmFilter
    Public fReport As frmReport = New frmReport
    Private intGiam_tru As Integer
    Private oDirFormDetailLib As reportformlib
    Private oDirFormLib As reportformlib
    Private oHTKK As clsHTKK = New clsHTKK
    Public oLan As Collection = New Collection
    Public oLen As Collection = New Collection
    Public oOption As Collection = New Collection
    Public oVar As Collection = New Collection
    Public rpTable As DataTable
    Public rpTypeTable As DataTable
    Public strCrAccount As String
    Public strDrAccount As String
    Public strUnit As String
    Public sysConn As SqlConnection
    Public SysID As String
End Module

