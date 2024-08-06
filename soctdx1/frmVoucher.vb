Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports libscontrol
Imports libscommon
Imports libscontrol.voucherseachlib
Imports libscontrol.clsvoucher.clsVoucher
Imports System.Math
Public Class frmVoucher
    Inherits Form
    ' Methods
    Public Sub New()
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.frmVoucher_Load)
        AddHandler MyBase.Activated, New EventHandler(AddressOf Me.frmVoucher_Activated)
        Me.arrControlButtons = New Button(13 - 1) {}
        'Me.oTitleButton = New TitleButton(Me)
        Me.lAllowCurrentCellChanged = True
        Me.xInventory = New clsInventory
        Me.InitializeComponent()
    End Sub

    Public Sub AddNew()
        Dim obj2 As Object = "stt_rec is null or stt_rec = ''"
        Me.grdHeader.ScatterBlank()
        modVoucher.tblDetail.AddNew()
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        Me.pnContent.Text = ""
        ScatterMemvarBlankWithDefault(Me)
        If (ObjectType.ObjTst(Me.txtNgay_ct.Text, Fox.GetEmptyDate, False) = 0) Then
            Me.txtNgay_ct.Value = DateAndTime.Now.Date
            Me.txtNgay_lct.Value = Me.txtNgay_ct.Value
        End If
        If (StringType.StrCmp(Strings.Trim(Me.cmdMa_nt.Text), "", False) = 0) Then
            Me.cmdMa_nt.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ma_nt"))
        End If
        Me.txtTy_gia.Value = DoubleType.FromObject(oVoucher.GetFCRate(Me.cmdMa_nt.Text, Me.txtNgay_ct.Value))
        Me.txtSo_ct.Text = oVoucher.GetVoucherNo
        Me.txtStatus.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_status"))
        Me.txtMa_gd.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("m_ma_gd"))
        Unit.SetUnit(Me.txtMa_dvcs)
        Me.EDFC()
        Me.cOldIDNumber = Me.cIDNumber
        Me.iOldMasterRow = Me.iMasterRow
        Me.EDTranType()
        Me.UpdateList()
        Me.ShowTabDetail()
        If Me.txtMa_dvcs.Enabled Then
            Me.txtMa_dvcs.Focus()
        Else
            Me.txtMa_gd.Focus()
        End If
        Me.EDTBColumns()
        Me.InitFlowHandling(Me.cboAction)
        Me.EDStatus()
        Me.oSecurity.SetReadOnly()
        xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
        xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
        Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
        Me.txtS1.Text = "0"
    End Sub

    Private Sub AfterUpdateSI(ByVal lcIDNumber As String, ByVal lcAction As String)
        Dim tcSQL As String = String.Concat(New String() {"fs_AfterUpdateSI '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        Sql.SQLExecute((modVoucher.appConn), tcSQL)
    End Sub

    Private Sub BeforUpdateSI(ByVal lcIDNumber As String, ByVal lcAction As String)
        Dim tcSQL As String = String.Concat(New String() {"fs_BeforUpdateSI '", lcIDNumber, "', '", lcAction, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        Sql.SQLExecute((modVoucher.appConn), tcSQL)
    End Sub

    Public Sub Cancel()
        On Error Resume Next
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        Dim num2 As Integer
        If (currentRowIndex >= 0) Then
            Me.grdDetail.Select(currentRowIndex)
        End If
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            num2 = (modVoucher.tblDetail.Count - 1)
            currentRowIndex = num2
            While (currentRowIndex >= 0)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) Then
                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), "", False) = 0) Then
                        modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    End If
                Else
                    modVoucher.tblDetail.Item(currentRowIndex).Delete()
                End If
                currentRowIndex = (currentRowIndex + -1)
            End While
            If (Me.iOldMasterRow = -1) Then
                ScatterMemvarBlank(Me)
                Dim obj2 As Object = "stt_rec = ''"
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
                Me.cmdNew.Focus()
                oVoucher.cAction = "Start"
                Me.grdDetail.ReadOnly = True
            Else
                ScatterMemvar(modVoucher.tblMaster.Item(Me.iOldMasterRow), Me)
                Dim obj3 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iOldMasterRow).Item("stt_rec")), "'")
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj3)
                Me.cmdEdit.Focus()
                oVoucher.cAction = "View"
                Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iOldMasterRow).Row
                Me.grdHeader.Scatter()
                xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iOldMasterRow), Me.tbDetail)
            End If
            Me.EDTranType()
        Else
            num2 = (modVoucher.tblDetail.Count - 1)
            currentRowIndex = num2
            While (currentRowIndex >= 0)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) Then
                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), "", False) = 0) Then
                        modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    End If
                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                        modVoucher.tblDetail.Item(currentRowIndex).Delete()
                    End If
                Else
                    modVoucher.tblDetail.Item(currentRowIndex).Delete()
                End If
                currentRowIndex = (currentRowIndex + -1)
            End While
            AppendFrom(modVoucher.tblDetail, Me.oldtblDetail)
            Me.RefrehForm()
            Me.cmdEdit.Focus()
            oVoucher.cAction = "View"
        End If
        Me.UpdateList()
        Me.vCaptionRefresh()
        Me.EDTBColumns()
        xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
    End Sub

    Public Sub Delete()
        If Not Me.isEdit Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("018")), 2)
        ElseIf Me.oSecurity.GetStatusDelelete Then
            Dim num As Integer
            Dim str4 As String
            Dim str5 As String
            Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
            Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            Dim lcIDNumber As String = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            num = num2
            Do While (num >= 0)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("stt_rec"))) Then
                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                        modVoucher.tblDetail.Item(num).Delete()
                    End If
                Else
                    modVoucher.tblDetail.Item(num).Delete()
                End If
                num = (num + -1)
            Loop
            If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                str5 = "ct90, ct84, ph84"
                str4 = ""
            Else
                str5 = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & ", ct90, ct84, ph84")
                str4 = GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)
            End If
            Dim num3 As Integer = IntegerType.FromObject(Fox.GetWordCount(str5, ","c))
            num = 1
            Do While (num <= num3)
                Dim cTable As String = Strings.Trim(Fox.GetWordNum(str5, num, ","c))
                str4 = (str4 & ChrW(13) & GenSQLDelete(cTable, cKey))
                num += 1
            Loop
            modVoucher.tblMaster.Item(Me.iMasterRow).Delete()
            If (Me.iMasterRow > 0) Then
                Me.iMasterRow -= 1
            ElseIf (modVoucher.tblMaster.Count = 0) Then
                Me.iMasterRow = -1
            End If
            If (Me.iMasterRow = -1) Then
                ScatterMemvarBlank(Me)
                oVoucher.cAction = "Start"
                Dim obj2 As Object = "stt_rec = ''"
                modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            Else
                oVoucher.cAction = "View"
                Me.RefrehForm()
            End If
            If (ObjectType.ObjTst(modVoucher.oVar.Item("m_pack_yn"), 0, False) = 0) Then
                str4 = ((String.Concat(New String() {str4, ChrW(13), "UPDATE ", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), " SET Status = '*'"}) & ", datetime2 = GETDATE(), user_id2 = " & StringType.FromObject(Reg.GetRegistryKey("CurrUserId"))) & "  WHERE " & cKey)
            End If
            Me.BeforUpdateSI(lcIDNumber, "Del")
            Sql.SQLExecute((modVoucher.appConn), str4)
            Me.pnContent.Text = ""
        End If
    End Sub

    Private Sub DeleteItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If ((((currentRowIndex >= 0) And (currentRowIndex < modVoucher.tblDetail.Count)) AndAlso Not Me.grdDetail.EndEdit(Me.grdDetail.TableStyles.Item(0).GridColumnStyles.Item(Me.grdDetail.CurrentCell.ColumnNumber), currentRowIndex, False)) AndAlso (ObjectType.ObjTst(Msg.Question(StringType.FromObject(modVoucher.oVar.Item("m_sure_dele")), 1), 1, False) = 0)) Then
                Me.grdDetail.Select(currentRowIndex)
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), False)
                modVoucher.tblDetail.Item(currentRowIndex).Delete()
                Me.UpdateList()
                AllowCurrentCellChanged((Me.lAllowCurrentCellChanged), True)
            End If
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Public Sub EDFC()
        If (ObjectType.ObjTst(Me.cmdMa_nt.Text, modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            Me.txtTy_gia.Enabled = False
            ChangeFormatColumn(Me.colGia_ban_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colGia_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colTien_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colCk_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colGia_ck_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            ChangeFormatColumn(Me.colGia_ban_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia")))
            ChangeFormatColumn(Me.colTien_ban_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))
            Me.colGia_ban_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("066"))
            Me.colGia_nt2.HeaderText = StringType.FromObject(modVoucher.oLan.Item("067"))
            Me.colTien_nt2.HeaderText = StringType.FromObject(modVoucher.oLan.Item("068"))
            Me.colCk_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("069"))
            Me.colGia_ck_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("076")).Replace("%s", "")
            Me.colThue_nt.HeaderText = StringType.FromObject(modVoucher.oLan.Item("070"))
            Me.colGia_ban_nt0.HeaderText = StringType.FromObject(modVoucher.oLan.Item("023")).Replace("%s", "").Trim
            Me.colTien_ban_nt0.HeaderText = StringType.FromObject(modVoucher.oLan.Item("021"))
            Try
                Me.colGia_ban.MappingName = "H7"
                Me.colGia2.MappingName = "H4"
                Me.colTien2.MappingName = "H1"
                Me.colCk.MappingName = "H6"
                Me.colThue.MappingName = "H8"
                Me.colGia_ban0.MappingName = "H9"
                Me.colTien_ban0.MappingName = "H10"
                Me.colGia_ck.MappingName = "H11"
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien"))
            Me.txtT_tien_nt2.Value = Me.txtT_tien_nt2.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
            Me.txtT_tien2.Visible = False
            Me.txtT_thue.Visible = False
            Me.txtT_tt.Visible = False
            Me.txtT_ck.Visible = False
        Else
            Me.txtTy_gia.Enabled = True
            ChangeFormatColumn(Me.colGia_ban_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colGia_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colTien_nt2, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colCk_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colGia_ck_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colThue_nt, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            ChangeFormatColumn(Me.colGia_ban_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_gia_nt")))
            ChangeFormatColumn(Me.colTien_ban_nt0, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))
            Me.colGia_ban_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("071")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_nt2.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("072")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colTien_nt2.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("073")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colCk_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("074")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_ck_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("076")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colThue_nt.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("075")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colGia_ban_nt0.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("023")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Me.colTien_ban_nt0.HeaderText = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("024")), "%s", Me.cmdMa_nt.Text, 1, -1, CompareMethod.Binary)
            Try
                Me.colGia_ban.MappingName = "gia_ban"
                Me.colGia2.MappingName = "gia2"
                Me.colTien2.MappingName = "tien2"
                Me.colCk.MappingName = "ck"
                Me.colGia_ck.MappingName = "gia_ck"
                Me.colThue.MappingName = "thue"
                Me.colGia_ban0.MappingName = "gia_ban0"
                Me.colTien_ban0.MappingName = "tien_ban0"
            Catch exception4 As Exception
                ProjectData.SetProjectError(exception4)
                ProjectData.ClearProjectError()
            End Try
            Me.txtT_tien_nt2.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_thue_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tt_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_ck_nt.Format = StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt"))
            Me.txtT_tien_nt2.Value = Me.txtT_tien_nt2.Value
            Me.txtT_thue_nt.Value = Me.txtT_thue_nt.Value
            Me.txtT_tt_nt.Value = Me.txtT_tt_nt.Value
            Me.txtT_ck_nt.Value = Me.txtT_ck_nt.Value
            Me.txtT_tien2.Visible = True
            Me.txtT_thue.Visible = True
            Me.txtT_tt.Visible = True
            Me.txtT_ck.Visible = True
        End If
        If CInt(oOption.Item("m_gia_sale")) = 0 Then
            Try
                Me.colGia_ban0.MappingName = "H1"
                Me.colTien_ban0.MappingName = "H2"
                Me.colGia_ban_nt0.MappingName = "H3"
                Me.colTien_ban_nt0.MappingName = "H4"
            Catch ex As Exception
            End Try
        End If
        Me.EDStatus()
        Me.oSecurity.Invisible()
    End Sub

    Public Sub Edit()
        Me.oldtblDetail = Copy2Table(modVoucher.tblDetail)
        Me.iOldMasterRow = Me.iMasterRow
        oVoucher.rOldMaster = modVoucher.tblMaster.Item(Me.iMasterRow)
        If Not Me.isEdit_Edit Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("017")), 2)
            Me.cmdSave.Enabled = False
        Else
            Me.ShowTabDetail()
            If Me.txtMa_dvcs.Enabled Then
                Me.txtMa_dvcs.Focus()
            Else
                Me.txtMa_gd.Focus()
            End If
            Me.EDTBColumns()
            Me.InitFlowHandling(Me.cboAction)
            Me.EDStatus()
            Me.oSecurity.SetReadOnly()
            If Not Me.oSecurity.GetStatusEdit Then
                Me.cmdSave.Enabled = False
            End If
            xtabControl.ReadOnlyTabControls(False, Me.tbDetail)
            Me.EDTrans()
            Me.oSite.Key = ("ma_dvcs = '" & Strings.Trim(Me.txtMa_dvcs.Text) & "'")
        End If
    End Sub

    Private Sub EDStatus()
        Try
            oVoucher.RefreshHandling(Me.cboAction)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim exception As Exception = exception1
            ProjectData.ClearProjectError()
        End Try
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Me.cboStatus.SelectedIndex = 0
        Else
            oVoucher.RefreshStatus(Me.cboStatus)
        End If
        Me.RefreshControlField()
        Me.lblAction.Visible = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Me.cboAction.Visible = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
        Me.grdHeader.Edit = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
    End Sub

    Private Sub EDStatus(ByVal lED As Boolean)
        oVoucher.RefreshHandling(Me.cboAction)
        oVoucher.RefreshStatus(Me.cboStatus)
        Me.lblAction.Visible = lED
        Me.cboAction.Visible = lED
        Me.grdHeader.Edit = lED
    End Sub

    Private Sub EDTBColumns()
        Dim index As Integer = 0
        Do
            modVoucher.tbcDetail(index).TextBox.Enabled = Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"})
            Try
                grdDetail.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = Fox.InList(grdDetail.TableStyles.Item(0).GridColumnStyles.Item(index).MappingName.ToLower, {"ten_vt", "gia_ban_nt", "ck_nt", "gia_ck_nt", "thue_suat", "thue_nt", "tien_ban_nt0", "so_dh", "so_line", "so_hd", "sv_line", "sl_hd", "sl_xuat", "sl_giao"})
            Catch ex As Exception
            End Try
            index += 1
        Loop While (index <= MaxColumns - 1)
        Try
            Me.colTen_vt.TextBox.Enabled = False
            'Me.colGia_ban_nt.TextBox.Enabled = False
            'Me.colCk_nt.TextBox.Enabled = False
            'Me.colThue_suat.TextBox.Enabled = False
            'Me.colThue_nt.TextBox.Enabled = False
            'Me.colGia_ban_nt0.TextBox.Enabled = False
            'Me.colTien_ban_nt0.TextBox.Enabled = False
            Me.colSo_dh.TextBox.Enabled = False
            Me.colSo_line.TextBox.Enabled = False
            Me.colSo_hd.TextBox.Enabled = False
            Me.colSv_line.TextBox.Enabled = False
            Me.colSl_xuat.TextBox.Enabled = False
            Me.colSl_hd.TextBox.Enabled = False
            Me.colSl_giao.TextBox.Enabled = False
            GetColumn(Me.grdDetail, "ten_kh_thue").TextBox.Enabled = False
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
    End Sub

    Private Sub EDTBColumns(ByVal lED As Boolean)
        Dim index As Integer = 0
        While index <= MaxColumns - 1
            modVoucher.tbcDetail(index).TextBox.Enabled = lED
            index += 1
        End While
        Me.EDStatus(lED)
    End Sub

    Private Sub EDTrans()
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
    End Sub

    Private Sub EDTranType()
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        If (StringType.StrCmp(Strings.Trim(Me.txtLoai_ct.Text), "1", False) = 0) Then
            Me.colSl_xuat.MappingName = "T1"
            Me.colSl_hd.MappingName = "sl_hd"
            Me.colSl_giao.MappingName = "sl_giao"
            Me.ContextMenu.MenuItems.Item(0).Enabled = True
        Else
            Me.colSl_xuat.MappingName = "sl_xuat"
            Me.colSl_hd.MappingName = "T2"
            Me.colSl_giao.MappingName = "T3"
            Me.ContextMenu.MenuItems.Item(0).Enabled = False
        End If
    End Sub

    Private Sub frmRetrieveLoad(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub frmVoucher_Activated(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.isActive Then
            Me.isActive = True
            Me.InitRecords()
        End If
    End Sub

    Private Sub frmVoucher_Load(ByVal sender As Object, ByVal e As EventArgs)
        'Me.oTitleButton.Code = modVoucher.VoucherCode
        'Me.oTitleButton.Connection = modVoucher.sysConn
        clsdrawlines.Init(Me, Me.tbDetail)
        Me.oVoucher = New clsvoucher.clsVoucher(Me.arrControlButtons, Me, Me.pnContent)
        oVoucher.isRead = Sys.CheckRights(modVoucher.sysConn, "Read")
        oVoucher.sysConn = modVoucher.sysConn
        oVoucher.appConn = modVoucher.appConn
        oVoucher.txtVDate = Me.txtNgay_ct
        oVoucher.lblStatus = Me.lblStatus
        oVoucher.lblStatusMess = Me.lblStatusMess
        oVoucher.cmdFC = Me.cmdMa_nt
        oVoucher.txtFCRate = Me.txtTy_gia
        oVoucher.oTab = Me.tbDetail
        oVoucher.oLan = modVoucher.oLan
        oVoucher.oOption = modVoucher.oOption
        oVoucher.oVar = modVoucher.oVar
        oVoucher.oVoucherRow = modVoucher.oVoucherRow
        oVoucher.VoucherCode = modVoucher.VoucherCode
        oVoucher.tblMaster = modVoucher.tblMaster
        oVoucher.tblDetail = modVoucher.tblDetail
        oVoucher.txtStatus = Me.txtStatus
        Me.tblHandling = oVoucher.InitHandling(Me.cboAction)
        Me.tblStatus = oVoucher.InitStatus(Me.cboStatus)
        If (StringType.StrCmp(modVoucher.cLan, "V", False) = 0) Then
            Me.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ten_ct"))
        Else
            Me.Text = StringType.FromObject(modVoucher.oVoucherRow.Item("ten_ct2"))
        End If
        Sys.InitMessage(modVoucher.sysConn, oVoucher.oClassMsg, "SysClass")
        Me.lblStatus.Text = StringType.FromObject(oVoucher.oClassMsg.Item("011"))
        Me.lblAction.Text = StringType.FromObject(oVoucher.oClassMsg.Item("033"))
        oVoucher.Init()
        Me.txtNgay_lct.AddCalenderControl()
        Dim lib3 As New DirLib(Me.txtMa_gd, Me.lblTen_gd, modVoucher.sysConn, modVoucher.appConn, "dmmagd", "ma_gd", "ten_gd", "VCTransCode", ("ma_ct = '" & modVoucher.VoucherCode & "'"), False, Me.cmdEdit)
        AddHandler Me.txtMa_gd.Validated, New EventHandler(AddressOf Me.txtMa_gd_Valid)
        Dim lib4 As New DirLib(Me.txtMa_dvcs, Me.lblTen_dvcs, modVoucher.sysConn, modVoucher.appConn, "dmdvcs", "ma_dvcs", "ten_dvcs", "Unit", "1=1", False, Me.cmdEdit)
        Dim lib2 As New CharLib(Me.txtStatus, "0, 1")
        Dim ldate As New clsGLdate(Me.txtNgay_lct, Me.txtNgay_ct)
        Unit.SetUnit(modVoucher.appConn, Me.txtMa_dvcs)
        Me.txtNgay_ct.TabStop = (ObjectType.ObjTst(modVoucher.oVoucherRow.Item("m_ngay_ct"), 1, False) = 0)
        Me.iMasterRow = -1
        Me.iOldMasterRow = -1
        Me.iDetailRow = -1
        Me.cIDNumber = ""
        Me.cOldIDNumber = ""
        Me.nColumnControl = -1
        modVoucher.alMaster = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "tmp")
        modVoucher.alDetail = (Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "tmp")
        Dim cFile As String = ("Structure\Voucher\" & modVoucher.VoucherCode)
        If Not Sys.XML2DataSet((modVoucher.dsMain), cFile) Then
            Dim tcSQL As String = ("SELECT * FROM " & modVoucher.alMaster)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alMaster, (modVoucher.dsMain))
            tcSQL = ("SELECT * FROM " & modVoucher.alDetail)
            Sql.SQLRetrieve((modVoucher.sysConn), tcSQL, modVoucher.alDetail, (modVoucher.dsMain))
            Sys.DataSet2XML(modVoucher.dsMain, cFile)
        End If
        modVoucher.tblMaster.Table = modVoucher.dsMain.Tables.Item(modVoucher.alMaster)
        modVoucher.tblDetail.Table = modVoucher.dsMain.Tables.Item(modVoucher.alDetail)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), (grdDetail), (modVoucher.tbsDetail), (modVoucher.tbcDetail), "SIDetail")
        oVoucher.SetMaxlengthItem(Me.grdDetail, modVoucher.alDetail, modVoucher.sysConn)
        Me.grdDetail.dvGrid = modVoucher.tblDetail
        Me.grdDetail.cFieldKey = "ma_vt"
        Me.grdDetail.AllowSorting = False
        Me.grdDetail.TableStyles.Item(0).AllowSorting = False
        Me.colMa_vt = GetColumn(Me.grdDetail, "ma_vt")
        Me.colDvt = GetColumn(Me.grdDetail, "Dvt")
        Me.colMa_kho = GetColumn(Me.grdDetail, "ma_kho")
        Me.colMa_vi_tri = GetColumn(Me.grdDetail, "ma_vi_tri")
        Me.colMa_lo = GetColumn(Me.grdDetail, "ma_lo")
        Me.colSo_luong = GetColumn(Me.grdDetail, "so_luong")
        Me.colGia2 = GetColumn(Me.grdDetail, "gia2")
        Me.colGia_nt2 = GetColumn(Me.grdDetail, "gia_nt2")
        Me.colTien2 = GetColumn(Me.grdDetail, "tien2")
        Me.colTien_nt2 = GetColumn(Me.grdDetail, "tien_nt2")
        Me.colTl_ck = GetColumn(Me.grdDetail, "tl_ck")
        Me.colCk = GetColumn(Me.grdDetail, "ck")
        Me.colCk_nt = GetColumn(Me.grdDetail, "ck_nt")
        Me.colGia_ck_nt = GetColumn(Me.grdDetail, "gia_ck_nt")
        Me.colGia_ck = GetColumn(Me.grdDetail, "gia_ck")
        Me.colMa_thue = GetColumn(Me.grdDetail, "ma_thue")
        Me.colThue_suat = GetColumn(Me.grdDetail, "thue_suat")
        Me.colGia_ban_nt = GetColumn(Me.grdDetail, "gia_ban_nt")
        Me.colGia_ban = GetColumn(Me.grdDetail, "gia_ban")
        Me.colThue_nt = GetColumn(Me.grdDetail, "thue_nt")
        Me.colThue = GetColumn(Me.grdDetail, "thue")
        Me.colGia_ban_nt0 = GetColumn(Me.grdDetail, "gia_ban_nt0")
        Me.colGia_ban0 = GetColumn(Me.grdDetail, "gia_ban0")
        Me.colTien_ban_nt0 = GetColumn(Me.grdDetail, "tien_ban_nt0")
        Me.colTien_ban0 = GetColumn(Me.grdDetail, "tien_ban0")
        Me.colTen_vt = GetColumn(Me.grdDetail, "ten_vt")
        Me.colSo_dh = GetColumn(Me.grdDetail, "so_dh")
        Me.colSo_line = GetColumn(Me.grdDetail, "so_line")
        Me.colSo_hd = GetColumn(Me.grdDetail, "so_hd")
        Me.colSv_line = GetColumn(Me.grdDetail, "sv_line")
        Me.colSl_xuat = GetColumn(Me.grdDetail, "sl_xuat")
        Me.colSl_hd = GetColumn(Me.grdDetail, "sl_hd")
        Me.colSl_giao = GetColumn(Me.grdDetail, "sl_giao")
        Me.colS1 = GetColumn(Me.grdDetail, "s1") 'Dung cho khach hang nhan hoa don
        Dim str As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keyaccount", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim sKey As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "keycust", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Me.oSite = New VoucherKeyLibObj(Me.colMa_kho, "ten_kho", modVoucher.sysConn, modVoucher.appConn, "dmkho", "ma_kho", "ten_kho", "Site", ("ma_dvcs = '" & Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("DFUnit"))) & "'"), modVoucher.tblDetail, Me.pnContent, False, Me.cmdEdit)
        Me.oUOM = New VoucherKeyCheckLibObj(Me.colDvt, "ten_dvt", modVoucher.sysConn, modVoucher.appConn, "vdmvtqddvt", "dvt", "ten_dvt", "UOMItem", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oUOM.Cancel = True
        Me.colDvt.TextBox.CharacterCasing = CharacterCasing.Normal

        Dim monumber As New monumber(GetColumn(Me.grdDetail, "so_lsx"))
        Dim oCust As New DirLib(Me.txtMa_kh, Me.lblTen_kh, modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", sKey, False, Me.cmdEdit)
        AddHandler Me.txtMa_kh.Validated, New EventHandler(AddressOf Me.txtMa_kh_valid)
        Dim clscustomerref As New clscustomerref(modVoucher.appConn, Me.txtMa_kh, Me.txtOng_ba, modVoucher.VoucherCode, Me.oVoucher)
        Dim oNvbh As New DirLib(Me.txtMa_nvbh, lblTen_nvbh, sysConn, modVoucher.appConn, "dmnvbh", "ma_nvbh", "ten_nvbh", "SaleEmployee", "1=1", False, Me.cmdEdit)
        Me.oInvItemDetail = New VoucherLibObj(Me.colMa_vt, "ten_vt", modVoucher.sysConn, modVoucher.appConn, "dmvt", "ma_vt", "ten_vt", "Item", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        VoucherLibObj.oClassMsg = oVoucher.oClassMsg
        Me.oInvItemDetail.Colkey = True
        VoucherLibObj.dvDetail = modVoucher.tblDetail
        Me.oLocation = New VoucherKeyLibObj(Me.colMa_vi_tri, "ten_vi_tri", modVoucher.sysConn, modVoucher.appConn, "dmvitri", "ma_vi_tri", "ten_vi_tri", "Location", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Me.oLot = New VoucherKeyLibObj(Me.colMa_lo, "ten_lo", modVoucher.sysConn, modVoucher.appConn, "vcdlo13_tt", "ma_lo", "ten_lo", "Lot13_tt", "1=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)
        Dim ocus_vat As New VoucherKeyLibObj(Me.colS1, "ten_kh_thue", modVoucher.sysConn, modVoucher.appConn, "dmkh", "ma_kh", "ten_kh", "Customer", "1=1 and kh_yn=1", modVoucher.tblDetail, Me.pnContent, True, Me.cmdEdit)

        Try
            oVoucher.AddValidFields(Me.grdDetail, modVoucher.tblDetail, Me.pnContent, Me.cmdEdit)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        oVoucher.HideFields(Me.grdDetail)
        ChangeFormatColumn(Me.colSo_luong, StringType.FromObject(modVoucher.oVar.Item("m_ip_sl")))
        AddHandler Me.colMa_kho.TextBox.Enter, New EventHandler(AddressOf Me.WhenSiteEnter)
        AddHandler Me.colMa_kho.TextBox.Validated, New EventHandler(AddressOf Me.WhenSiteLeave)
        AddHandler Me.colDvt.TextBox.Move, New EventHandler(AddressOf Me.WhenUOMEnter)
        AddHandler Me.colDvt.TextBox.Validated, New EventHandler(AddressOf Me.WhenUOMLeave)
        AddHandler Me.colMa_vi_tri.TextBox.Move, New EventHandler(AddressOf Me.WhenLocationEnter)
        AddHandler Me.colMa_lo.TextBox.Move, New EventHandler(AddressOf Me.WhenLotEnter)
        AddHandler Me.colMa_vt.TextBox.Enter, New EventHandler(AddressOf Me.SetEmptyColKey)
        AddHandler Me.colMa_vt.TextBox.Validated, New EventHandler(AddressOf Me.WhenItemLeave)
        AddHandler Me.colSo_luong.TextBox.Leave, New EventHandler(AddressOf Me.txtSo_luong_valid)
        AddHandler Me.colGia_nt2.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_nt2_valid)
        AddHandler Me.colTl_ck.TextBox.Leave, New EventHandler(AddressOf Me.txtTl_ck_valid)
        AddHandler Me.colMa_thue.TextBox.Leave, New EventHandler(AddressOf Me.txtMa_thue_valid)
        AddHandler Me.colGia_ban_nt0.TextBox.Leave, New EventHandler(AddressOf Me.txtGia_ban_nt0_valid)
        Dim objectValue As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj4 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim obj3 As Object = RuntimeHelpers.GetObjectValue(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fielddate", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim index As Integer = 0
        Do
            Dim objArray As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj4)}
            Dim flagArray As Boolean() = New Boolean() {True}
            If flagArray(0) Then
                obj4 = RuntimeHelpers.GetObjectValue(objArray(0))
            End If
            If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray, Nothing, flagArray)), modVoucher.tbcDetail(index).MappingName.ToLower, 0) > 0) Then
                modVoucher.tbcDetail(index).NullText = "0"
            Else
                Dim objArray2 As Object() = New Object() {RuntimeHelpers.GetObjectValue(obj3)}
                flagArray = New Boolean() {True}
                If flagArray(0) Then
                    obj3 = RuntimeHelpers.GetObjectValue(objArray2(0))
                End If
                If (Strings.InStr(StringType.FromObject(LateBinding.LateGet(Nothing, GetType(Strings), "LCase", objArray2, Nothing, flagArray)), modVoucher.tbcDetail(index).MappingName.ToLower, 0) > 0) Then
                    modVoucher.tbcDetail(index).NullText = StringType.FromObject(Fox.GetEmptyDate)
                Else
                    modVoucher.tbcDetail(index).NullText = ""
                End If
            End If
            If (index <> 0) Then
                AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter)
            Else
                AddHandler modVoucher.tbcDetail(index).TextBox.Enter, New EventHandler(AddressOf Me.txt_Enter_Ma_vt)
            End If
            index += 1
        Loop While (index <= MaxColumns - 1)
        Dim menu As New ContextMenu
        Dim item As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("201")), New EventHandler(AddressOf Me.NewItem), Shortcut.F4)
        Dim item2 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("202")), New EventHandler(AddressOf Me.DeleteItem), Shortcut.F8)
        menu.MenuItems.Add(item)
        menu.MenuItems.Add(New MenuItem("-"))
        menu.MenuItems.Add(item2)
        Dim menu2 As New ContextMenu
        Dim item3 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("008")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F5)
        Dim item4 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("057")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F6)
        Dim item5 As New MenuItem(StringType.FromObject(modVoucher.oLan.Item("058")), New EventHandler(AddressOf Me.RetrieveItems), Shortcut.F7)
        menu2.MenuItems.Add(item3)
        menu2.MenuItems.Add(New MenuItem("-"))
        menu2.MenuItems.Add(item4)
        menu2.MenuItems.Add(item5)
        Me.ContextMenu = menu2
        Me.txtKeyPress.Left = (-100 - Me.txtKeyPress.Width)
        Me.grdDetail.ContextMenu = menu
        ScatterMemvarBlank(Me)
        oVoucher.cAction = "Start"
        Me.isActive = False
        Me.grdHeader = New grdHeader(Me.tbDetail, (Me.txtKeyPress.TabIndex - 1), Me, modVoucher.appConn, modVoucher.sysConn, modVoucher.VoucherCode, Me.pnContent, Me.cmdEdit)
        Me.EDTBColumns()
        Me.oSecurity = New clssecurity(modVoucher.VoucherCode, IntegerType.FromObject(Reg.GetRegistryKey("CurrUserid")))
        Me.oSecurity.oVoucher = Me.oVoucher
        Me.oSecurity.cboAction = Me.cboAction
        Me.oSecurity.cboStatus = Me.cboStatus
        Me.oSecurity.cTotalField = "t_tt, t_tt_nt"
        Dim aGrid As New Collection
        aGrid.Add(Me, "Form", Nothing, Nothing)
        aGrid.Add(Me.grdHeader, "grdHeader", Nothing, Nothing)
        aGrid.Add(Me.grdDetail, "grdDetail", Nothing, Nothing)
        Me.oSecurity.aGrid = aGrid
        Me.oSecurity.Init()
        Me.oSecurity.Invisible()
        Me.oSecurity.SetReadOnly()
        Me.InitSOPrice()
        xtabControl.ScatterMemvarBlankTabControl(Me.tbDetail)
        xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
        xtabControl.SendTabKeys(Me.tbDetail)
        xtabControl.SetMaxlength(Me.tbDetail, modVoucher.alMaster, modVoucher.sysConn)
        Me.InitInventory()
        Dim clS1 As New CharLib(Me.txtS1, "0, 1")
        AddHandler Me.txtS1.TextChanged, New EventHandler(AddressOf Me.txtS1_TextChanged)
    End Sub

    Private Function GetIDItem(ByVal tblItem As DataView, ByVal sStart As String) As String
        Dim str2 As String = (sStart & "00")
        Dim num2 As Integer = (tblItem.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblItem.Item(i).Item("stt_rec0"))) AndAlso (ObjectType.ObjTst(tblItem.Item(i).Item("stt_rec0"), str2, False) > 0)) Then
                str2 = StringType.FromObject(tblItem.Item(i).Item("stt_rec0"))
            End If
            i += 1
        Loop
        Return Strings.Format(CInt(Math.Round(CDbl((DoubleType.FromString(str2) + 1)))), "000")
    End Function

    Public Sub GoRecno(ByVal cRecno As Object)
        If (StringType.StrCmp(oVoucher.cAction, "View", False) = 0) Then
            Dim obj2 As Object = cRecno
            If (ObjectType.ObjTst(obj2, "Top", False) = 0) Then
                If (Me.iMasterRow > 0) Then
                    Me.iMasterRow = 0
                    Me.RefrehForm()
                End If
            ElseIf (ObjectType.ObjTst(obj2, "Prev", False) = 0) Then
                If (Me.iMasterRow > 0) Then
                    Me.iMasterRow -= 1
                    Me.RefrehForm()
                End If
            ElseIf (ObjectType.ObjTst(obj2, "Next", False) = 0) Then
                If ((Me.iMasterRow < (modVoucher.tblMaster.Count - 1)) And (modVoucher.tblMaster.Count > 0)) Then
                    Me.iMasterRow += 1
                    Me.RefrehForm()
                End If
            ElseIf ((ObjectType.ObjTst(obj2, "Bottom", False) = 0) AndAlso ((Me.iMasterRow < (modVoucher.tblMaster.Count - 1)) And (modVoucher.tblMaster.Count > 0))) Then
                Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
                Me.RefrehForm()
            End If
        End If
    End Sub

    Private Sub grdDetail_CurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdDetail.CurrentCellChanged
        If Not Me.lAllowCurrentCellChanged Then
            Return
        End If
        On Error Resume Next
        Dim currentRowIndex As Integer = grdDetail.CurrentRowIndex
        Dim columnNumber As Integer = grdDetail.CurrentCell.ColumnNumber
        Dim oValue As String = Strings.Trim(StringType.FromObject(grdDetail.Item(currentRowIndex, columnNumber)))
        Dim str2 As String = grdDetail.TableStyles.Item(0).GridColumnStyles.Item(columnNumber).MappingName.ToUpper.ToString
        Select Case str2
            Case "MA_VT"
                Me.coldMa_vt = StringType.FromObject(oValue)
            Case "MA_KHO"
                Me.cOldSite = StringType.FromObject(oValue)
            Case "DVT"
                Me.coldDvt = StringType.FromObject(oValue)
            Case "SO_LUONG"
                Me.noldSo_luong = DecimalType.FromObject(oValue)
                Me.coldSo_luong = StringType.FromObject(oValue)
            Case "GIA_NT2"
                Me.noldGia_nt2 = DecimalType.FromObject(oValue)
            Case "GIA2"
                Me.noldGia2 = DecimalType.FromObject(oValue)
            Case "TIEN_NT2"
                Me.noldTien_nt2 = DecimalType.FromObject(oValue)
            Case "TIEN2"
                Me.noldTien2 = DecimalType.FromObject(oValue)
            Case "TL_CK"
                Me.noldTl_ck = DecimalType.FromObject(oValue)
            Case "CK_NT"
                Me.noldCk_nt = DecimalType.FromObject(oValue)
            Case "CK"
                Me.noldCk = DecimalType.FromObject(oValue)
            Case "MA_THUE"
                Me.coldMa_thue = StringType.FromObject(oValue)
            Case "GIA_BAN_NT0"
                noldGia_ban_nt0 = DecimalType.FromObject(oValue)
        End Select
    End Sub

    Private Sub grdLeave(ByVal sender As Object, ByVal e As EventArgs)
        If VoucherLibObj.isLostFocus Then
            VoucherLibObj.isLostFocus = False
        End If
    End Sub

    Private Sub grdMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(num).Item("stt_rec")), "'")
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub grdPCRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Private Sub grdRetrieveMVCurrentCellChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Integer = IntegerType.FromObject(LateBinding.LateGet(LateBinding.LateGet(sender, Nothing, "CurrentCell", New Object(0 - 1) {}, Nothing, Nothing), Nothing, "RowNumber", New Object(0 - 1) {}, Nothing, Nothing))
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num).Item("stt_rec")), "'")
        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
    End Sub

    Public Function InitFlowHandling(ByVal cboHandling As ComboBox) As DataTable
        Dim ds As New DataSet
        Dim num2 As Integer = 0
        cboHandling.DropDownStyle = ComboBoxStyle.DropDownList
        Dim str As String = StringType.FromObject(Reg.GetRegistryKey("Language"))
        Dim strSQL As String = String.Concat(New String() {"fs_GetFlowHandling '", modVoucher.VoucherCode, "', '", Me.txtStatus.Text, "'"})
        Sys.Ds2XML(modVoucher.appConn, strSQL, "dmxlct", (ds), ("Structure\Voucher\Handle\Flow\" & modVoucher.VoucherCode & "\" & Strings.Trim(Me.txtStatus.Text)))
        cboHandling.Items.Clear()
        Dim table As DataTable = ds.Tables.Item("dmxlct")
        Me.tblHandling.Clear()
        Me.tblHandling = ds.Tables.Item("dmxlct")
        Dim num3 As Integer = (table.Rows.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num3)
            If (ObjectType.ObjTst(table.Rows.Item(i).Item("status"), Me.txtStatus.Text, False) = 0) Then
                num2 = i
            End If
            Dim item As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(table.Rows.Item(i).Item("action_id"), ". "), Strings.Trim(StringType.FromObject(LateBinding.LateGet(table.Rows.Item(i), Nothing, "Item", New Object() {ObjectType.AddObj("action_name", Interaction.IIf((StringType.StrCmp(str, "V", False) = 0), "", "2"))}, Nothing, Nothing)))))
            cboHandling.Items.Add(item)
            i += 1
        Loop
        ds = Nothing
        cboHandling.SelectedIndex = num2
        Return table
    End Function
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtS7 As txtDate
    Friend WithEvents txtMa_nvbh As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents lblTen_nvbh As Label
    Friend WithEvents txtT_tien_nt2 As txtNumeric
    Friend WithEvents txtT_tien2 As txtNumeric
    Friend WithEvents lblT_thue As Label
    Friend WithEvents txtT_thue_nt As txtNumeric
    Friend WithEvents txtT_thue As txtNumeric
    Friend WithEvents lblT_tt As Label
    Friend WithEvents txtT_tt_nt As txtNumeric
    Friend WithEvents txtT_tt As txtNumeric
    Friend WithEvents lblTien_ck As Label
    Friend WithEvents txtT_ck_nt As txtNumeric
    Friend WithEvents txtT_ck As txtNumeric
    Friend WithEvents txtS1 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtS6 As txtNumeric
    Friend WithEvents txtS4 As txtNumeric
    Friend WithEvents txtS8 As txtDate

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmdSave = New System.Windows.Forms.Button()
        Me.cmdNew = New System.Windows.Forms.Button()
        Me.cmdPrint = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdView = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdOption = New System.Windows.Forms.Button()
        Me.cmdTop = New System.Windows.Forms.Button()
        Me.cmdPrev = New System.Windows.Forms.Button()
        Me.cmdNext = New System.Windows.Forms.Button()
        Me.cmdBottom = New System.Windows.Forms.Button()
        Me.lblMa_dvcs = New System.Windows.Forms.Label()
        Me.txtMa_dvcs = New System.Windows.Forms.TextBox()
        Me.lblTen_dvcs = New System.Windows.Forms.Label()
        Me.lblSo_ct = New System.Windows.Forms.Label()
        Me.txtSo_ct = New System.Windows.Forms.TextBox()
        Me.txtNgay_lct = New libscontrol.txtDate()
        Me.txtTy_gia = New libscontrol.txtNumeric()
        Me.lblNgay_lct = New System.Windows.Forms.Label()
        Me.lblNgay_ct = New System.Windows.Forms.Label()
        Me.lblTy_gia = New System.Windows.Forms.Label()
        Me.txtNgay_ct = New libscontrol.txtDate()
        Me.cmdMa_nt = New System.Windows.Forms.Button()
        Me.tbDetail = New System.Windows.Forms.TabControl()
        Me.tpgDetail = New System.Windows.Forms.TabPage()
        Me.grdDetail = New libscontrol.clsgrid()
        Me.tbgOther = New System.Windows.Forms.TabPage()
        Me.txtS8 = New libscontrol.txtDate()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtS7 = New libscontrol.txtDate()
        Me.lblNgay_lx0 = New System.Windows.Forms.Label()
        Me.txtSo_lx0 = New System.Windows.Forms.TextBox()
        Me.lblSo_lx0 = New System.Windows.Forms.Label()
        Me.txtNgay_lx0 = New libscontrol.txtDate()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblStatusMess = New System.Windows.Forms.Label()
        Me.txtKeyPress = New System.Windows.Forms.TextBox()
        Me.cboStatus = New System.Windows.Forms.ComboBox()
        Me.cboAction = New System.Windows.Forms.ComboBox()
        Me.lblAction = New System.Windows.Forms.Label()
        Me.lblMa_kh = New System.Windows.Forms.Label()
        Me.txtMa_kh = New System.Windows.Forms.TextBox()
        Me.lblTen_kh = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.lblTen = New System.Windows.Forms.Label()
        Me.txtDien_giai = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtT_so_luong = New libscontrol.txtNumeric()
        Me.txtLoai_ct = New System.Windows.Forms.TextBox()
        Me.txtMa_gd = New System.Windows.Forms.TextBox()
        Me.lblMa_gd = New System.Windows.Forms.Label()
        Me.lblTen_gd = New System.Windows.Forms.Label()
        Me.txtOng_ba = New System.Windows.Forms.TextBox()
        Me.lblOng_ba = New System.Windows.Forms.Label()
        Me.txtStt_rec_lx0 = New System.Windows.Forms.TextBox()
        Me.txtMa_nvbh = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblTen_nvbh = New System.Windows.Forms.Label()
        Me.txtT_tien_nt2 = New libscontrol.txtNumeric()
        Me.txtT_tien2 = New libscontrol.txtNumeric()
        Me.lblT_thue = New System.Windows.Forms.Label()
        Me.txtT_thue_nt = New libscontrol.txtNumeric()
        Me.txtT_thue = New libscontrol.txtNumeric()
        Me.lblT_tt = New System.Windows.Forms.Label()
        Me.txtT_tt_nt = New libscontrol.txtNumeric()
        Me.txtT_tt = New libscontrol.txtNumeric()
        Me.lblTien_ck = New System.Windows.Forms.Label()
        Me.txtT_ck_nt = New libscontrol.txtNumeric()
        Me.txtT_ck = New libscontrol.txtNumeric()
        Me.txtS1 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtS6 = New libscontrol.txtNumeric()
        Me.txtS4 = New libscontrol.txtNumeric()
        Me.tbDetail.SuspendLayout()
        Me.tpgDetail.SuspendLayout()
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbgOther.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdSave
        '
        Me.cmdSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSave.Location = New System.Drawing.Point(2, 572)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(72, 26)
        Me.cmdSave.TabIndex = 12
        Me.cmdSave.Tag = "CB01"
        Me.cmdSave.Text = "Luu"
        Me.cmdSave.UseVisualStyleBackColor = False
        '
        'cmdNew
        '
        Me.cmdNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNew.Location = New System.Drawing.Point(74, 572)
        Me.cmdNew.Name = "cmdNew"
        Me.cmdNew.Size = New System.Drawing.Size(72, 26)
        Me.cmdNew.TabIndex = 13
        Me.cmdNew.Tag = "CB02"
        Me.cmdNew.Text = "Moi"
        Me.cmdNew.UseVisualStyleBackColor = False
        '
        'cmdPrint
        '
        Me.cmdPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPrint.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrint.Location = New System.Drawing.Point(146, 572)
        Me.cmdPrint.Name = "cmdPrint"
        Me.cmdPrint.Size = New System.Drawing.Size(72, 26)
        Me.cmdPrint.TabIndex = 14
        Me.cmdPrint.Tag = "CB03"
        Me.cmdPrint.Text = "In ctu"
        Me.cmdPrint.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Location = New System.Drawing.Point(218, 572)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.Size = New System.Drawing.Size(72, 26)
        Me.cmdEdit.TabIndex = 15
        Me.cmdEdit.Tag = "CB04"
        Me.cmdEdit.Text = "Sua"
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Location = New System.Drawing.Point(290, 572)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(72, 26)
        Me.cmdDelete.TabIndex = 16
        Me.cmdDelete.Tag = "CB05"
        Me.cmdDelete.Text = "Xoa"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdView
        '
        Me.cmdView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdView.BackColor = System.Drawing.SystemColors.Control
        Me.cmdView.Location = New System.Drawing.Point(362, 572)
        Me.cmdView.Name = "cmdView"
        Me.cmdView.Size = New System.Drawing.Size(72, 26)
        Me.cmdView.TabIndex = 17
        Me.cmdView.Tag = "CB06"
        Me.cmdView.Text = "Xem"
        Me.cmdView.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Location = New System.Drawing.Point(434, 572)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(72, 26)
        Me.cmdSearch.TabIndex = 18
        Me.cmdSearch.Tag = "CB07"
        Me.cmdSearch.Text = "Tim"
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Location = New System.Drawing.Point(506, 572)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(72, 26)
        Me.cmdClose.TabIndex = 19
        Me.cmdClose.Tag = "CB08"
        Me.cmdClose.Text = "Quay ra"
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdOption
        '
        Me.cmdOption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOption.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOption.Location = New System.Drawing.Point(1038, 572)
        Me.cmdOption.Name = "cmdOption"
        Me.cmdOption.Size = New System.Drawing.Size(24, 26)
        Me.cmdOption.TabIndex = 20
        Me.cmdOption.TabStop = False
        Me.cmdOption.Tag = "CB09"
        Me.cmdOption.UseVisualStyleBackColor = False
        '
        'cmdTop
        '
        Me.cmdTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTop.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTop.Location = New System.Drawing.Point(1061, 572)
        Me.cmdTop.Name = "cmdTop"
        Me.cmdTop.Size = New System.Drawing.Size(24, 26)
        Me.cmdTop.TabIndex = 21
        Me.cmdTop.TabStop = False
        Me.cmdTop.Tag = "CB10"
        Me.cmdTop.UseVisualStyleBackColor = False
        '
        'cmdPrev
        '
        Me.cmdPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdPrev.BackColor = System.Drawing.SystemColors.Control
        Me.cmdPrev.Location = New System.Drawing.Point(1084, 572)
        Me.cmdPrev.Name = "cmdPrev"
        Me.cmdPrev.Size = New System.Drawing.Size(24, 26)
        Me.cmdPrev.TabIndex = 22
        Me.cmdPrev.TabStop = False
        Me.cmdPrev.Tag = "CB11"
        Me.cmdPrev.UseVisualStyleBackColor = False
        '
        'cmdNext
        '
        Me.cmdNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNext.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNext.Location = New System.Drawing.Point(1107, 572)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(24, 26)
        Me.cmdNext.TabIndex = 23
        Me.cmdNext.TabStop = False
        Me.cmdNext.Tag = "CB12"
        Me.cmdNext.UseVisualStyleBackColor = False
        '
        'cmdBottom
        '
        Me.cmdBottom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdBottom.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBottom.Location = New System.Drawing.Point(1129, 572)
        Me.cmdBottom.Name = "cmdBottom"
        Me.cmdBottom.Size = New System.Drawing.Size(24, 26)
        Me.cmdBottom.TabIndex = 24
        Me.cmdBottom.TabStop = False
        Me.cmdBottom.Tag = "CB13"
        Me.cmdBottom.UseVisualStyleBackColor = False
        '
        'lblMa_dvcs
        '
        Me.lblMa_dvcs.AutoSize = True
        Me.lblMa_dvcs.Location = New System.Drawing.Point(326, 526)
        Me.lblMa_dvcs.Name = "lblMa_dvcs"
        Me.lblMa_dvcs.Size = New System.Drawing.Size(60, 17)
        Me.lblMa_dvcs.TabIndex = 13
        Me.lblMa_dvcs.Tag = "L001"
        Me.lblMa_dvcs.Text = "Ma dvcs"
        Me.lblMa_dvcs.Visible = False
        '
        'txtMa_dvcs
        '
        Me.txtMa_dvcs.BackColor = System.Drawing.Color.White
        Me.txtMa_dvcs.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_dvcs.Location = New System.Drawing.Point(384, 526)
        Me.txtMa_dvcs.Name = "txtMa_dvcs"
        Me.txtMa_dvcs.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_dvcs.TabIndex = 0
        Me.txtMa_dvcs.Tag = "FCNBCF"
        Me.txtMa_dvcs.Text = "TXTMA_DVCS"
        Me.txtMa_dvcs.Visible = False
        '
        'lblTen_dvcs
        '
        Me.lblTen_dvcs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_dvcs.AutoSize = True
        Me.lblTen_dvcs.Location = New System.Drawing.Point(508, 526)
        Me.lblTen_dvcs.Name = "lblTen_dvcs"
        Me.lblTen_dvcs.Size = New System.Drawing.Size(113, 17)
        Me.lblTen_dvcs.TabIndex = 15
        Me.lblTen_dvcs.Tag = "FCRF"
        Me.lblTen_dvcs.Text = "Ten don vi co so"
        Me.lblTen_dvcs.Visible = False
        '
        'lblSo_ct
        '
        Me.lblSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSo_ct.AutoSize = True
        Me.lblSo_ct.Location = New System.Drawing.Point(912, 8)
        Me.lblSo_ct.Name = "lblSo_ct"
        Me.lblSo_ct.Size = New System.Drawing.Size(40, 17)
        Me.lblSo_ct.TabIndex = 16
        Me.lblSo_ct.Tag = "L009"
        Me.lblSo_ct.Text = "So ct"
        '
        'txtSo_ct
        '
        Me.txtSo_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSo_ct.BackColor = System.Drawing.Color.White
        Me.txtSo_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_ct.Location = New System.Drawing.Point(1032, 6)
        Me.txtSo_ct.Name = "txtSo_ct"
        Me.txtSo_ct.Size = New System.Drawing.Size(120, 22)
        Me.txtSo_ct.TabIndex = 5
        Me.txtSo_ct.Tag = "FCNBCF"
        Me.txtSo_ct.Text = "TXTSO_CT"
        Me.txtSo_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNgay_lct
        '
        Me.txtNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_lct.BackColor = System.Drawing.Color.White
        Me.txtNgay_lct.Location = New System.Drawing.Point(1032, 30)
        Me.txtNgay_lct.MaxLength = 10
        Me.txtNgay_lct.Name = "txtNgay_lct"
        Me.txtNgay_lct.Size = New System.Drawing.Size(120, 22)
        Me.txtNgay_lct.TabIndex = 6
        Me.txtNgay_lct.Tag = "FDNBCFDF"
        Me.txtNgay_lct.Text = "  /  /    "
        Me.txtNgay_lct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_lct.Value = New Date(CType(0, Long))
        '
        'txtTy_gia
        '
        Me.txtTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTy_gia.BackColor = System.Drawing.Color.White
        Me.txtTy_gia.Format = "m_ip_tg"
        Me.txtTy_gia.Location = New System.Drawing.Point(1032, 54)
        Me.txtTy_gia.MaxLength = 8
        Me.txtTy_gia.Name = "txtTy_gia"
        Me.txtTy_gia.Size = New System.Drawing.Size(120, 22)
        Me.txtTy_gia.TabIndex = 8
        Me.txtTy_gia.Tag = "FNCF"
        Me.txtTy_gia.Text = "m_ip_tg"
        Me.txtTy_gia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTy_gia.Value = 0R
        '
        'lblNgay_lct
        '
        Me.lblNgay_lct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_lct.AutoSize = True
        Me.lblNgay_lct.Location = New System.Drawing.Point(912, 32)
        Me.lblNgay_lct.Name = "lblNgay_lct"
        Me.lblNgay_lct.Size = New System.Drawing.Size(64, 17)
        Me.lblNgay_lct.TabIndex = 20
        Me.lblNgay_lct.Tag = "L010"
        Me.lblNgay_lct.Text = "Ngay lap"
        '
        'lblNgay_ct
        '
        Me.lblNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNgay_ct.AutoSize = True
        Me.lblNgay_ct.Location = New System.Drawing.Point(425, 526)
        Me.lblNgay_ct.Name = "lblNgay_ct"
        Me.lblNgay_ct.Size = New System.Drawing.Size(108, 17)
        Me.lblNgay_ct.TabIndex = 21
        Me.lblNgay_ct.Tag = "L011"
        Me.lblNgay_ct.Text = "Ngay hach toan"
        Me.lblNgay_ct.Visible = False
        '
        'lblTy_gia
        '
        Me.lblTy_gia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTy_gia.AutoSize = True
        Me.lblTy_gia.Location = New System.Drawing.Point(911, 59)
        Me.lblTy_gia.Name = "lblTy_gia"
        Me.lblTy_gia.Size = New System.Drawing.Size(47, 17)
        Me.lblTy_gia.TabIndex = 22
        Me.lblTy_gia.Tag = "L012"
        Me.lblTy_gia.Text = "Ty gia"
        '
        'txtNgay_ct
        '
        Me.txtNgay_ct.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNgay_ct.BackColor = System.Drawing.Color.White
        Me.txtNgay_ct.Location = New System.Drawing.Point(771, 524)
        Me.txtNgay_ct.MaxLength = 10
        Me.txtNgay_ct.Name = "txtNgay_ct"
        Me.txtNgay_ct.Size = New System.Drawing.Size(120, 22)
        Me.txtNgay_ct.TabIndex = 11
        Me.txtNgay_ct.Tag = "FDNBCFDF"
        Me.txtNgay_ct.Text = "  /  /    "
        Me.txtNgay_ct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_ct.Value = New Date(CType(0, Long))
        Me.txtNgay_ct.Visible = False
        '
        'cmdMa_nt
        '
        Me.cmdMa_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMa_nt.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMa_nt.Enabled = False
        Me.cmdMa_nt.Location = New System.Drawing.Point(973, 54)
        Me.cmdMa_nt.Name = "cmdMa_nt"
        Me.cmdMa_nt.Size = New System.Drawing.Size(44, 23)
        Me.cmdMa_nt.TabIndex = 7
        Me.cmdMa_nt.TabStop = False
        Me.cmdMa_nt.Tag = "FCCFCMDDF"
        Me.cmdMa_nt.Text = "VND"
        Me.cmdMa_nt.UseVisualStyleBackColor = False
        '
        'tbDetail
        '
        Me.tbDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDetail.Controls.Add(Me.tpgDetail)
        Me.tbDetail.Controls.Add(Me.tbgOther)
        Me.tbDetail.Location = New System.Drawing.Point(2, 133)
        Me.tbDetail.Name = "tbDetail"
        Me.tbDetail.SelectedIndex = 0
        Me.tbDetail.Size = New System.Drawing.Size(1153, 350)
        Me.tbDetail.TabIndex = 12
        '
        'tpgDetail
        '
        Me.tpgDetail.BackColor = System.Drawing.SystemColors.Control
        Me.tpgDetail.Controls.Add(Me.grdDetail)
        Me.tpgDetail.Location = New System.Drawing.Point(4, 25)
        Me.tpgDetail.Name = "tpgDetail"
        Me.tpgDetail.Size = New System.Drawing.Size(1145, 321)
        Me.tpgDetail.TabIndex = 0
        Me.tpgDetail.Tag = "L016"
        Me.tpgDetail.Text = "Chung tu"
        '
        'grdDetail
        '
        Me.grdDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdDetail.BackgroundColor = System.Drawing.Color.White
        Me.grdDetail.CaptionBackColor = System.Drawing.SystemColors.Control
        Me.grdDetail.CaptionFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grdDetail.CaptionForeColor = System.Drawing.Color.Black
        Me.grdDetail.CaptionText = "F4 - Them, F8 - Xoa"
        Me.grdDetail.Cell_EnableRaisingEvents = False
        Me.grdDetail.DataMember = ""
        Me.grdDetail.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.grdDetail.Location = New System.Drawing.Point(-2, -1)
        Me.grdDetail.Name = "grdDetail"
        Me.grdDetail.Size = New System.Drawing.Size(1147, 316)
        Me.grdDetail.TabIndex = 0
        Me.grdDetail.Tag = "L020CF"
        '
        'tbgOther
        '
        Me.tbgOther.Controls.Add(Me.txtS8)
        Me.tbgOther.Controls.Add(Me.Label3)
        Me.tbgOther.Controls.Add(Me.txtS7)
        Me.tbgOther.Controls.Add(Me.lblNgay_lx0)
        Me.tbgOther.Controls.Add(Me.txtSo_lx0)
        Me.tbgOther.Controls.Add(Me.lblSo_lx0)
        Me.tbgOther.Controls.Add(Me.txtNgay_lx0)
        Me.tbgOther.Location = New System.Drawing.Point(4, 25)
        Me.tbgOther.Name = "tbgOther"
        Me.tbgOther.Size = New System.Drawing.Size(856, 190)
        Me.tbgOther.TabIndex = 3
        Me.tbgOther.Tag = "L015"
        Me.tbgOther.Text = "Thong tin khac"
        '
        'txtS8
        '
        Me.txtS8.BackColor = System.Drawing.Color.White
        Me.txtS8.Location = New System.Drawing.Point(256, 38)
        Me.txtS8.MaxLength = 10
        Me.txtS8.Name = "txtS8"
        Me.txtS8.Size = New System.Drawing.Size(120, 22)
        Me.txtS8.TabIndex = 3
        Me.txtS8.Tag = "FDCF"
        Me.txtS8.Text = "  /  /    "
        Me.txtS8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtS8.Value = New Date(CType(0, Long))
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(-2, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(116, 17)
        Me.Label3.TabIndex = 137
        Me.Label3.Tag = "LZ02"
        Me.Label3.Text = "Ngay giao tu/den"
        '
        'txtS7
        '
        Me.txtS7.BackColor = System.Drawing.Color.White
        Me.txtS7.Location = New System.Drawing.Point(112, 38)
        Me.txtS7.MaxLength = 10
        Me.txtS7.Name = "txtS7"
        Me.txtS7.Size = New System.Drawing.Size(120, 22)
        Me.txtS7.TabIndex = 2
        Me.txtS7.Tag = "FDCF"
        Me.txtS7.Text = "  /  /    "
        Me.txtS7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtS7.Value = New Date(CType(0, Long))
        '
        'lblNgay_lx0
        '
        Me.lblNgay_lx0.AutoSize = True
        Me.lblNgay_lx0.Location = New System.Drawing.Point(254, 6)
        Me.lblNgay_lx0.Name = "lblNgay_lx0"
        Me.lblNgay_lx0.Size = New System.Drawing.Size(41, 17)
        Me.lblNgay_lx0.TabIndex = 135
        Me.lblNgay_lx0.Tag = "L006"
        Me.lblNgay_lx0.Text = "Ngay"
        '
        'txtSo_lx0
        '
        Me.txtSo_lx0.BackColor = System.Drawing.Color.White
        Me.txtSo_lx0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSo_lx0.Enabled = False
        Me.txtSo_lx0.Location = New System.Drawing.Point(114, 6)
        Me.txtSo_lx0.Name = "txtSo_lx0"
        Me.txtSo_lx0.Size = New System.Drawing.Size(120, 22)
        Me.txtSo_lx0.TabIndex = 0
        Me.txtSo_lx0.Tag = "FCCF"
        Me.txtSo_lx0.Text = "TXTSO_LX0"
        Me.txtSo_lx0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblSo_lx0
        '
        Me.lblSo_lx0.AutoSize = True
        Me.lblSo_lx0.Location = New System.Drawing.Point(2, 8)
        Me.lblSo_lx0.Name = "lblSo_lx0"
        Me.lblSo_lx0.Size = New System.Drawing.Size(86, 17)
        Me.lblSo_lx0.TabIndex = 114
        Me.lblSo_lx0.Tag = "L004"
        Me.lblSo_lx0.Text = "So lenh xuat"
        '
        'txtNgay_lx0
        '
        Me.txtNgay_lx0.BackColor = System.Drawing.Color.White
        Me.txtNgay_lx0.Enabled = False
        Me.txtNgay_lx0.Location = New System.Drawing.Point(428, 6)
        Me.txtNgay_lx0.MaxLength = 10
        Me.txtNgay_lx0.Name = "txtNgay_lx0"
        Me.txtNgay_lx0.Size = New System.Drawing.Size(120, 22)
        Me.txtNgay_lx0.TabIndex = 1
        Me.txtNgay_lx0.Tag = "FDCF"
        Me.txtNgay_lx0.Text = "  /  /    "
        Me.txtNgay_lx0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtNgay_lx0.Value = New Date(CType(0, Long))
        '
        'txtStatus
        '
        Me.txtStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtStatus.BackColor = System.Drawing.Color.White
        Me.txtStatus.Location = New System.Drawing.Point(10, 602)
        Me.txtStatus.MaxLength = 1
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(30, 22)
        Me.txtStatus.TabIndex = 41
        Me.txtStatus.TabStop = False
        Me.txtStatus.Tag = "FCCF"
        Me.txtStatus.Text = "txtStatus"
        Me.txtStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtStatus.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(912, 83)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(73, 17)
        Me.lblStatus.TabIndex = 29
        Me.lblStatus.Tag = ""
        Me.lblStatus.Text = "Trang thai"
        '
        'lblStatusMess
        '
        Me.lblStatusMess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatusMess.AutoSize = True
        Me.lblStatusMess.Location = New System.Drawing.Point(58, 604)
        Me.lblStatusMess.Name = "lblStatusMess"
        Me.lblStatusMess.Size = New System.Drawing.Size(253, 17)
        Me.lblStatusMess.TabIndex = 42
        Me.lblStatusMess.Tag = ""
        Me.lblStatusMess.Text = "1 - Ghi vao SC, 0 - Chua ghi vao so cai"
        Me.lblStatusMess.Visible = False
        '
        'txtKeyPress
        '
        Me.txtKeyPress.Location = New System.Drawing.Point(692, 73)
        Me.txtKeyPress.Name = "txtKeyPress"
        Me.txtKeyPress.Size = New System.Drawing.Size(12, 22)
        Me.txtKeyPress.TabIndex = 11
        '
        'cboStatus
        '
        Me.cboStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboStatus.BackColor = System.Drawing.Color.White
        Me.cboStatus.Enabled = False
        Me.cboStatus.Location = New System.Drawing.Point(984, 78)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(168, 24)
        Me.cboStatus.TabIndex = 9
        Me.cboStatus.TabStop = False
        Me.cboStatus.Tag = ""
        Me.cboStatus.Text = "cboStatus"
        '
        'cboAction
        '
        Me.cboAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAction.BackColor = System.Drawing.Color.White
        Me.cboAction.Location = New System.Drawing.Point(984, 103)
        Me.cboAction.Name = "cboAction"
        Me.cboAction.Size = New System.Drawing.Size(168, 24)
        Me.cboAction.TabIndex = 10
        Me.cboAction.TabStop = False
        Me.cboAction.Tag = "CF"
        Me.cboAction.Text = "cboAction"
        '
        'lblAction
        '
        Me.lblAction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(912, 107)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(39, 17)
        Me.lblAction.TabIndex = 9
        Me.lblAction.Tag = ""
        Me.lblAction.Text = "Xu ly"
        '
        'lblMa_kh
        '
        Me.lblMa_kh.AutoSize = True
        Me.lblMa_kh.Location = New System.Drawing.Point(2, 32)
        Me.lblMa_kh.Name = "lblMa_kh"
        Me.lblMa_kh.Size = New System.Drawing.Size(69, 17)
        Me.lblMa_kh.TabIndex = 34
        Me.lblMa_kh.Tag = "L002"
        Me.lblMa_kh.Text = "Ma khach"
        '
        'txtMa_kh
        '
        Me.txtMa_kh.BackColor = System.Drawing.Color.White
        Me.txtMa_kh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_kh.Location = New System.Drawing.Point(106, 30)
        Me.txtMa_kh.Name = "txtMa_kh"
        Me.txtMa_kh.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_kh.TabIndex = 1
        Me.txtMa_kh.Tag = "FCNBCF"
        Me.txtMa_kh.Text = "TXTMA_KH"
        '
        'lblTen_kh
        '
        Me.lblTen_kh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_kh.Location = New System.Drawing.Point(230, 32)
        Me.lblTen_kh.Name = "lblTen_kh"
        Me.lblTen_kh.Size = New System.Drawing.Size(667, 18)
        Me.lblTen_kh.TabIndex = 36
        Me.lblTen_kh.Tag = "FCRF"
        Me.lblTen_kh.Text = "Ten Khach"
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Location = New System.Drawing.Point(713, 489)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(76, 17)
        Me.lblTotal.TabIndex = 60
        Me.lblTotal.Tag = "L013"
        Me.lblTotal.Text = "Tong cong"
        '
        'lblTen
        '
        Me.lblTen.AutoSize = True
        Me.lblTen.Location = New System.Drawing.Point(688, 526)
        Me.lblTen.Name = "lblTen"
        Me.lblTen.Size = New System.Drawing.Size(76, 17)
        Me.lblTen.TabIndex = 68
        Me.lblTen.Tag = "RF"
        Me.lblTen.Text = "Ten chung"
        Me.lblTen.Visible = False
        '
        'txtDien_giai
        '
        Me.txtDien_giai.BackColor = System.Drawing.Color.White
        Me.txtDien_giai.Location = New System.Drawing.Point(106, 78)
        Me.txtDien_giai.Name = "txtDien_giai"
        Me.txtDien_giai.Size = New System.Drawing.Size(536, 22)
        Me.txtDien_giai.TabIndex = 3
        Me.txtDien_giai.Tag = "FCCF"
        Me.txtDien_giai.Text = "txtDien_giai"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 81)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 17)
        Me.Label1.TabIndex = 75
        Me.Label1.Tag = "L029"
        Me.Label1.Text = "Dien giai"
        '
        'txtT_so_luong
        '
        Me.txtT_so_luong.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_so_luong.BackColor = System.Drawing.Color.White
        Me.txtT_so_luong.Enabled = False
        Me.txtT_so_luong.ForeColor = System.Drawing.Color.Black
        Me.txtT_so_luong.Format = "m_ip_sl"
        Me.txtT_so_luong.Location = New System.Drawing.Point(791, 485)
        Me.txtT_so_luong.MaxLength = 8
        Me.txtT_so_luong.Name = "txtT_so_luong"
        Me.txtT_so_luong.Size = New System.Drawing.Size(120, 22)
        Me.txtT_so_luong.TabIndex = 10
        Me.txtT_so_luong.Tag = "FN"
        Me.txtT_so_luong.Text = "m_ip_sl"
        Me.txtT_so_luong.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_so_luong.Value = 0R
        '
        'txtLoai_ct
        '
        Me.txtLoai_ct.BackColor = System.Drawing.Color.White
        Me.txtLoai_ct.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLoai_ct.Location = New System.Drawing.Point(624, 526)
        Me.txtLoai_ct.Name = "txtLoai_ct"
        Me.txtLoai_ct.Size = New System.Drawing.Size(36, 22)
        Me.txtLoai_ct.TabIndex = 85
        Me.txtLoai_ct.Tag = "FC"
        Me.txtLoai_ct.Text = "TXTLOAI_CT"
        Me.txtLoai_ct.Visible = False
        '
        'txtMa_gd
        '
        Me.txtMa_gd.BackColor = System.Drawing.Color.White
        Me.txtMa_gd.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_gd.Location = New System.Drawing.Point(106, 6)
        Me.txtMa_gd.Name = "txtMa_gd"
        Me.txtMa_gd.Size = New System.Drawing.Size(36, 22)
        Me.txtMa_gd.TabIndex = 0
        Me.txtMa_gd.Tag = "FCNBCF"
        Me.txtMa_gd.Text = "TXTMA_GD"
        '
        'lblMa_gd
        '
        Me.lblMa_gd.AutoSize = True
        Me.lblMa_gd.Location = New System.Drawing.Point(2, 8)
        Me.lblMa_gd.Name = "lblMa_gd"
        Me.lblMa_gd.Size = New System.Drawing.Size(88, 17)
        Me.lblMa_gd.TabIndex = 87
        Me.lblMa_gd.Tag = "L003"
        Me.lblMa_gd.Text = "Ma giao dich"
        '
        'lblTen_gd
        '
        Me.lblTen_gd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_gd.Location = New System.Drawing.Point(146, 8)
        Me.lblTen_gd.Name = "lblTen_gd"
        Me.lblTen_gd.Size = New System.Drawing.Size(751, 17)
        Me.lblTen_gd.TabIndex = 88
        Me.lblTen_gd.Tag = "FCRF"
        Me.lblTen_gd.Text = "Ten giao dich"
        '
        'txtOng_ba
        '
        Me.txtOng_ba.BackColor = System.Drawing.Color.White
        Me.txtOng_ba.Location = New System.Drawing.Point(106, 54)
        Me.txtOng_ba.Name = "txtOng_ba"
        Me.txtOng_ba.Size = New System.Drawing.Size(198, 22)
        Me.txtOng_ba.TabIndex = 2
        Me.txtOng_ba.Tag = "FCCF"
        Me.txtOng_ba.Text = "txtOng_ba"
        '
        'lblOng_ba
        '
        Me.lblOng_ba.AutoSize = True
        Me.lblOng_ba.Location = New System.Drawing.Point(2, 57)
        Me.lblOng_ba.Name = "lblOng_ba"
        Me.lblOng_ba.Size = New System.Drawing.Size(76, 17)
        Me.lblOng_ba.TabIndex = 119
        Me.lblOng_ba.Tag = "L005"
        Me.lblOng_ba.Text = "Nguoi mua"
        '
        'txtStt_rec_lx0
        '
        Me.txtStt_rec_lx0.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtStt_rec_lx0.BackColor = System.Drawing.Color.White
        Me.txtStt_rec_lx0.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtStt_rec_lx0.Location = New System.Drawing.Point(384, 604)
        Me.txtStt_rec_lx0.Name = "txtStt_rec_lx0"
        Me.txtStt_rec_lx0.Size = New System.Drawing.Size(72, 22)
        Me.txtStt_rec_lx0.TabIndex = 130
        Me.txtStt_rec_lx0.Tag = "FCCF"
        Me.txtStt_rec_lx0.Text = "TXTSTT_REC_LX0"
        Me.txtStt_rec_lx0.Visible = False
        '
        'txtMa_nvbh
        '
        Me.txtMa_nvbh.BackColor = System.Drawing.Color.White
        Me.txtMa_nvbh.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMa_nvbh.Location = New System.Drawing.Point(106, 103)
        Me.txtMa_nvbh.Name = "txtMa_nvbh"
        Me.txtMa_nvbh.Size = New System.Drawing.Size(120, 22)
        Me.txtMa_nvbh.TabIndex = 4
        Me.txtMa_nvbh.Tag = "FCNBCF"
        Me.txtMa_nvbh.Text = "TXTMA_NVBH"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 105)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 17)
        Me.Label2.TabIndex = 132
        Me.Label2.Tag = "L065"
        Me.Label2.Text = "Nhan vien ban"
        '
        'lblTen_nvbh
        '
        Me.lblTen_nvbh.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTen_nvbh.Location = New System.Drawing.Point(230, 105)
        Me.lblTen_nvbh.Name = "lblTen_nvbh"
        Me.lblTen_nvbh.Size = New System.Drawing.Size(667, 17)
        Me.lblTen_nvbh.TabIndex = 133
        Me.lblTen_nvbh.Tag = "FCRF"
        Me.lblTen_nvbh.Text = "Ten nvbh"
        '
        'txtT_tien_nt2
        '
        Me.txtT_tien_nt2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien_nt2.BackColor = System.Drawing.Color.White
        Me.txtT_tien_nt2.Enabled = False
        Me.txtT_tien_nt2.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien_nt2.Format = "m_ip_tien_nt"
        Me.txtT_tien_nt2.Location = New System.Drawing.Point(915, 485)
        Me.txtT_tien_nt2.MaxLength = 13
        Me.txtT_tien_nt2.Name = "txtT_tien_nt2"
        Me.txtT_tien_nt2.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tien_nt2.TabIndex = 135
        Me.txtT_tien_nt2.Tag = "FN"
        Me.txtT_tien_nt2.Text = "m_ip_tien_nt"
        Me.txtT_tien_nt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien_nt2.Value = 0R
        '
        'txtT_tien2
        '
        Me.txtT_tien2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tien2.BackColor = System.Drawing.Color.White
        Me.txtT_tien2.Enabled = False
        Me.txtT_tien2.ForeColor = System.Drawing.Color.Black
        Me.txtT_tien2.Format = "m_ip_tien"
        Me.txtT_tien2.Location = New System.Drawing.Point(1036, 485)
        Me.txtT_tien2.MaxLength = 10
        Me.txtT_tien2.Name = "txtT_tien2"
        Me.txtT_tien2.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tien2.TabIndex = 136
        Me.txtT_tien2.Tag = "FN"
        Me.txtT_tien2.Text = "m_ip_tien"
        Me.txtT_tien2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tien2.Value = 0R
        '
        'lblT_thue
        '
        Me.lblT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_thue.AutoSize = True
        Me.lblT_thue.Location = New System.Drawing.Point(793, 513)
        Me.lblT_thue.Name = "lblT_thue"
        Me.lblT_thue.Size = New System.Drawing.Size(68, 17)
        Me.lblT_thue.TabIndex = 142
        Me.lblT_thue.Tag = "L055"
        Me.lblT_thue.Text = "Tien thue"
        '
        'txtT_thue_nt
        '
        Me.txtT_thue_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue_nt.BackColor = System.Drawing.Color.White
        Me.txtT_thue_nt.Enabled = False
        Me.txtT_thue_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue_nt.Format = "m_ip_tien_nt"
        Me.txtT_thue_nt.Location = New System.Drawing.Point(915, 511)
        Me.txtT_thue_nt.MaxLength = 13
        Me.txtT_thue_nt.Name = "txtT_thue_nt"
        Me.txtT_thue_nt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_thue_nt.TabIndex = 137
        Me.txtT_thue_nt.Tag = "FN"
        Me.txtT_thue_nt.Text = "m_ip_tien_nt"
        Me.txtT_thue_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue_nt.Value = 0R
        '
        'txtT_thue
        '
        Me.txtT_thue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_thue.BackColor = System.Drawing.Color.White
        Me.txtT_thue.Enabled = False
        Me.txtT_thue.ForeColor = System.Drawing.Color.Black
        Me.txtT_thue.Format = "m_ip_tien"
        Me.txtT_thue.Location = New System.Drawing.Point(1036, 511)
        Me.txtT_thue.MaxLength = 10
        Me.txtT_thue.Name = "txtT_thue"
        Me.txtT_thue.Size = New System.Drawing.Size(120, 22)
        Me.txtT_thue.TabIndex = 138
        Me.txtT_thue.Tag = "FN"
        Me.txtT_thue.Text = "m_ip_tien"
        Me.txtT_thue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_thue.Value = 0R
        '
        'lblT_tt
        '
        Me.lblT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblT_tt.AutoSize = True
        Me.lblT_tt.Location = New System.Drawing.Point(793, 537)
        Me.lblT_tt.Name = "lblT_tt"
        Me.lblT_tt.Size = New System.Drawing.Size(113, 17)
        Me.lblT_tt.TabIndex = 141
        Me.lblT_tt.Tag = "L056"
        Me.lblT_tt.Text = "Tong thanh toan"
        '
        'txtT_tt_nt
        '
        Me.txtT_tt_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt_nt.BackColor = System.Drawing.Color.White
        Me.txtT_tt_nt.Enabled = False
        Me.txtT_tt_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt_nt.Format = "m_ip_tien_nt"
        Me.txtT_tt_nt.Location = New System.Drawing.Point(915, 534)
        Me.txtT_tt_nt.MaxLength = 13
        Me.txtT_tt_nt.Name = "txtT_tt_nt"
        Me.txtT_tt_nt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tt_nt.TabIndex = 139
        Me.txtT_tt_nt.Tag = "FN"
        Me.txtT_tt_nt.Text = "m_ip_tien_nt"
        Me.txtT_tt_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt_nt.Value = 0R
        '
        'txtT_tt
        '
        Me.txtT_tt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtT_tt.BackColor = System.Drawing.Color.White
        Me.txtT_tt.Enabled = False
        Me.txtT_tt.ForeColor = System.Drawing.Color.Black
        Me.txtT_tt.Format = "m_ip_tien"
        Me.txtT_tt.Location = New System.Drawing.Point(1036, 534)
        Me.txtT_tt.MaxLength = 10
        Me.txtT_tt.Name = "txtT_tt"
        Me.txtT_tt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_tt.TabIndex = 140
        Me.txtT_tt.Tag = "FN"
        Me.txtT_tt.Text = "m_ip_tien"
        Me.txtT_tt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_tt.Value = 0R
        '
        'lblTien_ck
        '
        Me.lblTien_ck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTien_ck.AutoSize = True
        Me.lblTien_ck.Location = New System.Drawing.Point(24, 489)
        Me.lblTien_ck.Name = "lblTien_ck"
        Me.lblTien_ck.Size = New System.Drawing.Size(75, 17)
        Me.lblTien_ck.TabIndex = 145
        Me.lblTien_ck.Tag = "L069"
        Me.lblTien_ck.Text = "Chiet khau"
        '
        'txtT_ck_nt
        '
        Me.txtT_ck_nt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_ck_nt.BackColor = System.Drawing.Color.White
        Me.txtT_ck_nt.Enabled = False
        Me.txtT_ck_nt.ForeColor = System.Drawing.Color.Black
        Me.txtT_ck_nt.Format = "m_ip_tien_nt"
        Me.txtT_ck_nt.Location = New System.Drawing.Point(128, 485)
        Me.txtT_ck_nt.MaxLength = 13
        Me.txtT_ck_nt.Name = "txtT_ck_nt"
        Me.txtT_ck_nt.Size = New System.Drawing.Size(120, 22)
        Me.txtT_ck_nt.TabIndex = 143
        Me.txtT_ck_nt.Tag = "FN"
        Me.txtT_ck_nt.Text = "m_ip_tien_nt"
        Me.txtT_ck_nt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ck_nt.Value = 0R
        '
        'txtT_ck
        '
        Me.txtT_ck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtT_ck.BackColor = System.Drawing.Color.White
        Me.txtT_ck.Enabled = False
        Me.txtT_ck.ForeColor = System.Drawing.Color.Black
        Me.txtT_ck.Format = "m_ip_tien"
        Me.txtT_ck.Location = New System.Drawing.Point(248, 485)
        Me.txtT_ck.MaxLength = 10
        Me.txtT_ck.Name = "txtT_ck"
        Me.txtT_ck.Size = New System.Drawing.Size(120, 22)
        Me.txtT_ck.TabIndex = 144
        Me.txtT_ck.Tag = "FN"
        Me.txtT_ck.Text = "m_ip_tien"
        Me.txtT_ck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtT_ck.Value = 0R
        '
        'txtS1
        '
        Me.txtS1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtS1.BackColor = System.Drawing.Color.White
        Me.txtS1.Location = New System.Drawing.Point(106, 534)
        Me.txtS1.Name = "txtS1"
        Me.txtS1.Size = New System.Drawing.Size(36, 22)
        Me.txtS1.TabIndex = 146
        Me.txtS1.Tag = "FCNBCF"
        Me.txtS1.Text = "txtS1"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(2, 539)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 17)
        Me.Label4.TabIndex = 147
        Me.Label4.Tag = ""
        Me.Label4.Text = "Kiểu giá"
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(150, 538)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(169, 17)
        Me.Label5.TabIndex = 148
        Me.Label5.Tag = ""
        Me.Label5.Text = "0 - Giá duyệt, 1 - Giá bán"
        '
        'txtS6
        '
        Me.txtS6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtS6.BackColor = System.Drawing.Color.White
        Me.txtS6.Enabled = False
        Me.txtS6.ForeColor = System.Drawing.Color.Black
        Me.txtS6.Format = "m_ip_tien"
        Me.txtS6.Location = New System.Drawing.Point(576, 509)
        Me.txtS6.MaxLength = 10
        Me.txtS6.Name = "txtS6"
        Me.txtS6.Size = New System.Drawing.Size(57, 22)
        Me.txtS6.TabIndex = 149
        Me.txtS6.Tag = "FN"
        Me.txtS6.Text = "m_ip_tien"
        Me.txtS6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtS6.Value = 0R
        Me.txtS6.Visible = False
        '
        'txtS4
        '
        Me.txtS4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtS4.BackColor = System.Drawing.Color.White
        Me.txtS4.Enabled = False
        Me.txtS4.ForeColor = System.Drawing.Color.Black
        Me.txtS4.Format = "m_ip_tien"
        Me.txtS4.Location = New System.Drawing.Point(576, 534)
        Me.txtS4.MaxLength = 10
        Me.txtS4.Name = "txtS4"
        Me.txtS4.Size = New System.Drawing.Size(57, 22)
        Me.txtS4.TabIndex = 150
        Me.txtS4.Tag = "FN"
        Me.txtS4.Text = "m_ip_tien"
        Me.txtS4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtS4.Value = 0R
        Me.txtS4.Visible = False
        '
        'frmVoucher
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(1157, 624)
        Me.Controls.Add(Me.txtS4)
        Me.Controls.Add(Me.txtS6)
        Me.Controls.Add(Me.txtS1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblTien_ck)
        Me.Controls.Add(Me.txtT_ck_nt)
        Me.Controls.Add(Me.txtT_ck)
        Me.Controls.Add(Me.lblT_thue)
        Me.Controls.Add(Me.txtT_thue_nt)
        Me.Controls.Add(Me.txtT_thue)
        Me.Controls.Add(Me.lblT_tt)
        Me.Controls.Add(Me.txtT_tt_nt)
        Me.Controls.Add(Me.txtT_tt)
        Me.Controls.Add(Me.txtT_tien_nt2)
        Me.Controls.Add(Me.txtT_tien2)
        Me.Controls.Add(Me.txtMa_nvbh)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblTen_nvbh)
        Me.Controls.Add(Me.txtMa_dvcs)
        Me.Controls.Add(Me.lblMa_dvcs)
        Me.Controls.Add(Me.lblStatusMess)
        Me.Controls.Add(Me.txtOng_ba)
        Me.Controls.Add(Me.lblOng_ba)
        Me.Controls.Add(Me.txtMa_gd)
        Me.Controls.Add(Me.lblMa_gd)
        Me.Controls.Add(Me.txtLoai_ct)
        Me.Controls.Add(Me.txtT_so_luong)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTen)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.txtMa_kh)
        Me.Controls.Add(Me.lblMa_kh)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.txtKeyPress)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblNgay_lct)
        Me.Controls.Add(Me.lblSo_ct)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.txtNgay_lct)
        Me.Controls.Add(Me.txtSo_ct)
        Me.Controls.Add(Me.lblTen_dvcs)
        Me.Controls.Add(Me.txtDien_giai)
        Me.Controls.Add(Me.lblNgay_ct)
        Me.Controls.Add(Me.lblTy_gia)
        Me.Controls.Add(Me.txtNgay_ct)
        Me.Controls.Add(Me.txtTy_gia)
        Me.Controls.Add(Me.txtStt_rec_lx0)
        Me.Controls.Add(Me.lblTen_gd)
        Me.Controls.Add(Me.lblTen_kh)
        Me.Controls.Add(Me.cboAction)
        Me.Controls.Add(Me.cboStatus)
        Me.Controls.Add(Me.tbDetail)
        Me.Controls.Add(Me.cmdMa_nt)
        Me.Controls.Add(Me.cmdBottom)
        Me.Controls.Add(Me.cmdNext)
        Me.Controls.Add(Me.cmdPrev)
        Me.Controls.Add(Me.cmdTop)
        Me.Controls.Add(Me.cmdOption)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.cmdView)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdPrint)
        Me.Controls.Add(Me.cmdNew)
        Me.Controls.Add(Me.cmdSave)
        Me.Name = "frmVoucher"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmVoucher"
        Me.tbDetail.ResumeLayout(False)
        Me.tpgDetail.ResumeLayout(False)
        CType(Me.grdDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbgOther.ResumeLayout(False)
        Me.tbgOther.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub InitInventory()
        Me.xInventory.ColItem = Me.colMa_vt
        Me.xInventory.ColLot = Me.colMa_lo
        Me.xInventory.ColSite = Me.colMa_kho
        Me.xInventory.ColLocation = Me.colMa_vi_tri
        Me.xInventory.ColUOM = Me.colDvt
        Me.xInventory.colQty = Me.colSo_luong
        Me.xInventory.txtUnit = Me.txtMa_dvcs
        Me.xInventory.InvVoucher = Me.oVoucher
        Me.xInventory.oInvItem = Me.oInvItemDetail
        Me.xInventory.oInvSite = Me.oSite
        Me.xInventory.oInvLocation = Me.oLocation
        Me.xInventory.oInvLot = Me.oLot
        Me.xInventory.oInvUOM = Me.oUOM
        Me.xInventory.Init()
    End Sub

    Public Sub InitRecords()
        Dim str As String
        If oVoucher.isRead Then
            str = String.Concat(New String() {"EXEC fs_LoadSITran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', -1"})
        Else
            str = String.Concat(New String() {"EXEC fs_LoadSITran '", modVoucher.cLan, "', '", modVoucher.cIDVoucher, "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_sl_ct0"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), "', '", Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), "', '", modVoucher.VoucherCode, "', ", Strings.Trim(StringType.FromObject(Reg.GetRegistryKey("CurrUserID")))})
        End If
        str = (str & GetLoadParameters())
        Dim ds As New DataSet
        Sql.SQLDecompressRetrieve((modVoucher.appConn), str, "trantmp", (ds))
        AppendFrom(modVoucher.tblMaster, ds.Tables.Item(0))
        AppendFrom(modVoucher.tblDetail, ds.Tables.Item(1))
        If (modVoucher.tblMaster.Count > 0) Then
            Me.iMasterRow = 0
            Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
            modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
            oVoucher.cAction = "View"
            If (modVoucher.tblMaster.Count = 1) Then
                Me.RefrehForm()
            Else
                Me.View()
            End If
            oVoucher.RefreshButton(oVoucher.ctrlButtons, oVoucher.cAction)
            If (modVoucher.tblMaster.Count = 1) Then
                Me.cmdEdit.Focus()
            End If
        Else
            Me.cmdNew.Focus()
        End If
        ds = Nothing
    End Sub

    Private Function isEdit() As Boolean
        If (StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "0", False) = 0) Then
            Return True
        End If
        Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim i As Integer = 0
        Do While (i <= num2)
            Dim view As DataRowView = modVoucher.tblDetail.Item(i)
            If BooleanType.FromObject(ObjectType.BitOrObj(ObjectType.BitOrObj((ObjectType.ObjTst(view.Item("sl_xuat"), 0, False) <> 0), (ObjectType.ObjTst(view.Item("sl_giao"), 0, False) <> 0)), (ObjectType.ObjTst(view.Item("sl_hd"), 0, False) <> 0))) Then
                Return False
            End If
            view = Nothing
            i += 1
        Loop
        Return True
    End Function
    Private Function isEdit_Edit() As Boolean
        If CType(Sql.GetValue(appConn, "exec sp_CheckAllow '" + VoucherCode + "','Edit','" + modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") + "',null"), Integer) = 0 Then
            Return False
        End If
        Return True
        'If (StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "0", False) = 0) Then
        '    Return True
        'End If
        'Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
        'Dim i As Integer = 0
        'Do While (i <= num2)
        '    Dim view As DataRowView = modVoucher.tblDetail.Item(i)
        '    If BooleanType.FromObject(ObjectType.BitAndObj(ObjectType.BitAndObj((ObjectType.ObjTst(view.Item("sl_xuat"), 0, False) = 0), (ObjectType.ObjTst(view.Item("sl_giao"), 0, False) = 0)), (ObjectType.ObjTst(view.Item("sl_hd"), 0, False) = 0))) Then
        '        Return True
        '    End If
        '    view = Nothing
        '    i += 1
        'Loop
        Return False
    End Function

    Private Sub NewItem(ByVal sender As Object, ByVal e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If (currentRowIndex < 0) Then
                modVoucher.tblDetail.AddNew()
                Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
            ElseIf ((Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec"))) AndAlso Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt")))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(currentRowIndex).Item("ma_vt"))), "", False) <> 0)) Then
                Dim count As Integer = modVoucher.tblDetail.Count
                Me.grdDetail.BeforeAddNewItem()
                Me.grdDetail.CurrentCell = New DataGridCell(count, 0)
                Me.grdDetail.AfterAddNewItem()
            End If
        End If
    End Sub

    Public Sub Options(ByVal nIndex As Integer)
        If (StringType.StrCmp(oVoucher.cAction, "View", False) = 0) Then
            Select Case nIndex
                Case 0
                    Dim view As DataRowView = modVoucher.tblMaster.Item(Me.iMasterRow)
                    oVoucher.ShowUserInfor(IntegerType.FromObject(view.Item("user_id0")), IntegerType.FromObject(view.Item("user_id2")), DateType.FromObject(view.Item("datetime0")), DateType.FromObject(view.Item("datetime2")))
                    view = Nothing
                    Exit Select
                Case 2
                    oVoucher.ViewDeletedRecord("fs_SearchDeletedSITran", "SIMaster", "SIDetail", "t_tt", "t_tt_nt")
                    Exit Select
            End Select
        End If
    End Sub

    Private Function Post() As String
        Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "groupby", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
        Dim str3 As String = "EXEC fs_PostSI "
        Return (StringType.FromObject(ObjectType.AddObj(((((((str3 & "'" & modVoucher.VoucherCode & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_master"))) & "'") & ", '" & Strings.Trim(StringType.FromObject(modVoucher.oOption.Item("m_gl_detail"))) & "'") & ", '" & Strings.Trim(str2) & "'"), ObjectType.AddObj(ObjectType.AddObj(", '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))) & ", 1")
    End Function

    Public Sub Print()
        Dim print As New frmPrint
        print.txtTitle.Text = StringType.FromObject(Interaction.IIf((StringType.StrCmp(modVoucher.cLan, "V", False) = 0), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct"))), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("tieu_de_ct2")))))
        print.txtSo_lien.Value = DoubleType.FromObject(modVoucher.oVoucherRow.Item("so_lien"))
        Dim table As DataTable = clsprint.InitComboReport(modVoucher.sysConn, print.cboReports, "SITran")
        Dim result As DialogResult = print.ShowDialog
        If ((result <> DialogResult.Cancel) AndAlso (print.txtSo_lien.Value > 0)) Then
            Dim selectedIndex As Integer = print.cboReports.SelectedIndex
            Dim strFile As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(Reg.GetRegistryKey("ReportDir"), Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_file")))), ".rpt"))
            Dim _rep_id As String = Strings.Trim(StringType.FromObject(table.Rows.Item(selectedIndex).Item("rep_id")))
            Dim view As New DataView
            Dim ds As New DataSet
            Dim tcSQL As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj(ObjectType.AddObj((("EXEC fs_PrintSITran '" & modVoucher.cLan) & "', " & "[a.stt_rec = '"), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'], '"), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf")))), "'"))
            'Cus bo sung them rep_id vao truy van in
            tcSQL += ",'" + _rep_id + "'"
            Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "cttmp", (ds))
            Dim num4 As Integer = IntegerType.FromObject(modVoucher.oVoucherRow.Item("max_row"))
            view.Table = ds.Tables.Item("cttmp")
            Dim t_tt As Decimal = 0, count As Integer, t_tt_hd As Decimal = 0
            For count = 0 To view.Count - 1
                t_tt += view.Item(count).Item("tien2") - view.Item(count).Item("ck")
                t_tt_hd += view.Item(count).Item("tien_ban0")
            Next
            'Dim num6 As Integer = num4
            'Dim i As Integer = view.Count
            'Do While (i <= num6)
            '    view.AddNew()
            '    i += 1
            'Loop
            Dim clsprint As New clsprint(Me, strFile, Nothing)
            clsprint.oRpt.SetDataSource(view.Table)
            clsprint.oVar = modVoucher.oVar
            clsprint.dr = modVoucher.tblMaster.Item(Me.iMasterRow).Row
            clsprint.SetReportVar(modVoucher.sysConn, modVoucher.appConn, "SITran", modVoucher.oOption, clsprint.oRpt)
            clsprint.oRpt.SetParameterValue("Title", Strings.Trim(print.txtTitle.Text))
            Dim str As String = Strings.Replace(Strings.Replace(Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("401")), "%s1", Me.txtNgay_ct.Value.Day.ToString, 1, -1, 0), "%s2", Me.txtNgay_ct.Value.Month.ToString, 1, -1, 0), "%s3", Me.txtNgay_ct.Value.Year.ToString, 1, -1, 0)
            Dim str3 As String = Strings.Replace(StringType.FromObject(modVoucher.oLan.Item("402")), "%s", Strings.Trim(Me.txtSo_ct.Text), 1, -1, 0)
            clsprint.oRpt.SetParameterValue("t_date", str)
            clsprint.oRpt.SetParameterValue("t_number", str3)
            clsprint.oRpt.SetParameterValue("s_byword_hd", clsprint.Num2Words(t_tt_hd, Me.cmdMa_nt.Text))
            clsprint.oRpt.SetParameterValue("s_byword", clsprint.Num2Words(t_tt, Me.cmdMa_nt.Text))
            'clsprint.oRpt.SetParameterValue("f_kh", (Strings.Trim(Me.txtMa_kh.Text) & " - " & Strings.Trim(Me.lblTen_kh.Text)))
            'Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkh", "dia_chi", ("ma_kh = '" & Strings.Trim(Me.txtMa_kh.Text) & "'"))))
            'clsprint.oRpt.SetParameterValue("f_dia_chi", str2)
            'str2 = Strings.Trim(Me.txtDien_giai.Text)
            'clsprint.oRpt.SetParameterValue("f_dien_giai", str2)
            If (result = DialogResult.OK) Then
                clsprint.PrintReport(CInt(Math.Round(print.txtSo_lien.Value)))
                clsprint.oRpt.SetDataSource(view.Table)
            Else
                clsprint.ShowReports()
            End If
            clsprint.oRpt.Close()
            ds = Nothing
            table = Nothing
            print.Dispose()
        End If
    End Sub

    Public Sub RefrehForm()
        Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
        Me.grdHeader.Scatter()
        ScatterMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'")
        modVoucher.tblDetail.RowFilter = StringType.FromObject(obj2)
        Me.EDTranType()
        Me.UpdateList()
        Me.vCaptionRefresh()
        xtabControl.ScatterTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
        Me.cmdNew.Focus()
    End Sub

    Private Sub RefreshControlField()
    End Sub

    Private Sub RetrieveItems(ByVal sender As Object, ByVal e As EventArgs)
        Dim cancel As Boolean = Me.oInvItemDetail.Cancel
        Me.oInvItemDetail.Cancel = True
        Select Case IntegerType.FromObject(LateBinding.LateGet(sender, Nothing, "Index", New Object(0 - 1) {}, Nothing, Nothing))
            Case 0
                Me.RetrieveItemsFromIC()
                Exit Select
            Case 2
                Me.RetrieveItemsFromSO()
                Exit Select
            Case 3
                Me.RetrieveItemsFromSV()
                Exit Select
        End Select
        Me.oInvItemDetail.Cancel = cancel
    End Sub

    Private Sub RetrieveItemsFromIC()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("064")), 2)
            Else
                Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchIOTran4SI '", modVoucher.cLan, "', '", Strings.Trim(Me.txtMa_kh.Text), "'"})
                Dim ds As New DataSet
                Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                Me.tblRetrieveMaster = New DataView
                Me.tblRetrieveDetail = New DataView
                If (ds.Tables.Item(0).Rows.Count <= 0) Then
                    Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                Else
                    Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                    Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                    Dim frmAdd As New Form
                    Dim gridformtran2 As New gridformtran
                    Dim gridformtran As New gridformtran
                    Dim tbs As New DataGridTableStyle
                    Dim style As New DataGridTableStyle
                    Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
                    Dim index As Integer = 0
                    Do
                        cols(index) = New DataGridTextBoxColumn
                        If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                            cols(index).NullText = StringType.FromInteger(0)
                        Else
                            cols(index).NullText = ""
                        End If
                        index += 1
                    Loop While (index <= MaxColumns - 1)
                    frmAdd.Top = 0
                    frmAdd.Left = 0
                    frmAdd.Width = Me.Width
                    frmAdd.Height = Me.Height
                    frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("014"))
                    frmAdd.StartPosition = FormStartPosition.CenterParent
                    Dim panel As StatusBarPanel = AddStb(frmAdd)
                    gridformtran2.CaptionVisible = False
                    gridformtran2.ReadOnly = True
                    gridformtran2.Top = 0
                    gridformtran2.Left = 0
                    gridformtran2.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                    gridformtran2.Width = (Me.Width - 5)
                    gridformtran2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                    gridformtran2.BackgroundColor = Color.White
                    gridformtran.CaptionVisible = False
                    gridformtran.ReadOnly = False
                    gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                    gridformtran.Left = 0
                    gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
                    gridformtran.Width = (Me.Width - 5)
                    gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
                    gridformtran.BackgroundColor = Color.White
                    Dim button As New Button
                    button.Visible = True
                    button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                    button.Left = (-100 - button.Width)
                    frmAdd.Controls.Add(button)
                    frmAdd.CancelButton = button
                    frmAdd.Controls.Add(gridformtran2)
                    frmAdd.Controls.Add(gridformtran)
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "IOMaster4SI")
                    index = 0
                    Do
                        If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                            cols(index).NullText = StringType.FromInteger(0)
                        Else
                            cols(index).NullText = ""
                        End If
                        index += 1
                    Loop While (index <= MaxColumns - 1)
                    Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "IODetail4SI")
                    index = 0
                    Do
                        If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                            cols(index).NullText = StringType.FromInteger(0)
                        Else
                            cols(index).NullText = ""
                        End If
                        index += 1
                    Loop While (index <= MaxColumns - 1)
                    Me.tblRetrieveDetail.AllowDelete = False
                    Me.tblRetrieveDetail.AllowNew = False
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
                    gridformtran.TableStyles.Item(0).GridColumnStyles.Item(2).ReadOnly = True
                    index = 3
                    Do While (1 <> 0)
                        Try
                            index += 1
                            gridformtran.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                        Catch exception1 As Exception
                            ProjectData.SetProjectError(exception1)
                            Dim exception As Exception = exception1
                            ProjectData.ClearProjectError()
                            Exit Do
                        End Try
                    Loop
                    Dim str2 As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                    Dim zero As Decimal = Decimal.Zero
                    Dim num4 As Decimal = Decimal.Zero
                    Dim count As Integer = Me.tblRetrieveMaster.Count
                    Dim num8 As Integer = (count - 1)
                    index = 0
                    Do While (index <= num8)
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien2"))) Then
                            zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien2")))
                        End If
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2"))) Then
                            num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2")))
                        End If
                        index += 1
                    Loop
                    str2 = Strings.Replace(Strings.Replace(Strings.Replace(str2, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, 0), "%n2", "X", 1, -1, 0), "%n3", "X", 1, -1, 0)
                    panel.Text = str2
                    AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdPCRetrieveMVCurrentCellChanged)
                    gridformtran2.CurrentRowIndex = 0
                    Dim rowNumber As Integer = 0
                    Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(rowNumber).Item("stt_rec")), "'")
                    Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
                    Obj.Init(frmAdd)
                    Dim button4 As New RadioButton
                    Dim button2 As New RadioButton
                    Dim button3 As New RadioButton
                    button4.Top = CInt(Math.Round(CDbl((((CDbl((Me.Height - 20)) / 2) + gridformtran.Height) + 5))))
                    button4.Left = 0
                    button4.Visible = True
                    button4.Checked = True
                    button4.Text = StringType.FromObject(modVoucher.oLan.Item("060"))
                    button4.Width = 100
                    button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    button2.Top = button4.Top
                    button2.Left = (button4.Left + 110)
                    button2.Visible = True
                    button2.Text = StringType.FromObject(modVoucher.oLan.Item("061"))
                    button2.Width = 120
                    button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    button2.Enabled = False
                    button3.Top = button4.Top
                    button3.Left = (button2.Left + 130)
                    button3.Visible = True
                    button3.Text = StringType.FromObject(modVoucher.oLan.Item("062"))
                    button3.Width = 200
                    button3.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                    frmAdd.Controls.Add(button4)
                    frmAdd.Controls.Add(button2)
                    frmAdd.Controls.Add(button3)
                    frmAdd.ShowDialog()
                    If button4.Checked Then
                        ds = Nothing
                        Me.tblRetrieveMaster = Nothing
                        Me.tblRetrieveDetail = Nothing
                        Return
                    End If
                    Dim tblRetrieveDetail As DataView = Me.tblRetrieveDetail
                    tblRetrieveDetail.RowFilter = (tblRetrieveDetail.RowFilter & " AND sl_xuat0 <> 0")
                    Dim num7 As Integer = (Me.tblRetrieveDetail.Count - 1)
                    index = 0
                    Do While (index <= num7)
                        Me.tblRetrieveDetail.Item(index).Item("so_luong") = RuntimeHelpers.GetObjectValue(Me.tblRetrieveDetail.Item(index).Item("sl_xuat0"))
                        Me.tblRetrieveDetail.Item(index).Row.AcceptChanges()
                        index += 1
                    Loop
                    Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                    count = (modVoucher.tblDetail.Count - 1)
                    If ((button3.Checked And flag) And (count >= 0)) Then
                        index = count
                        Do While (index >= 0)
                            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                modVoucher.tblDetail.Item(index).Delete()
                            ElseIf (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
                                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                End If
                                If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                End If
                            ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                modVoucher.tblDetail.Item(index).Delete()
                            ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                modVoucher.tblDetail.Item(index).Delete()
                            End If
                            index = (index + -1)
                        Loop
                    End If
                    Dim tbl As New DataTable
                    tbl = Copy2Table(Me.tblRetrieveDetail)
                    Dim num6 As Integer = (tbl.Rows.Count - 1)
                    index = 0
                    Do While (index <= num6)
                        With tbl.Rows.Item(index)
                            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                .Item("stt_rec") = ""
                            Else
                                .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                            End If
                            .Item("sl_xuat") = 0
                            .Item("sl_giao") = 0
                            .Item("sl_hd") = 0
                            tbl.Rows.Item(index).AcceptChanges()
                        End With
                        index += 1
                    Loop
                    AppendFrom(modVoucher.tblDetail, tbl)
                    count = modVoucher.tblDetail.Count
                    If flag Then
                        index = (count - 1)
                        Do While (index >= 0)
                            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                modVoucher.tblDetail.Item(index).Delete()
                            ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_lx")), "C") Then
                                modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                            End If
                            index = (index + -1)
                        Loop
                        Try
                            rowNumber = gridformtran2.CurrentCell.RowNumber
                            Dim view As DataRowView = Me.tblRetrieveMaster.Item(rowNumber)
                            Me.txtStt_rec_lx0.Text = StringType.FromObject(view.Item("stt_rec"))
                            Me.txtSo_lx0.Text = StringType.FromObject(view.Item("so_ct"))
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(view.Item("ngay_ct"))) Then
                                Me.txtNgay_lx0.Value = DateType.FromObject(view.Item("ngay_ct"))
                            Else
                                Me.txtNgay_lx0.Text = StringType.FromObject(Fox.GetEmptyDate)
                            End If
                            view = Nothing
                        Catch exception3 As Exception
                            ProjectData.SetProjectError(exception3)
                            Dim exception2 As Exception = exception3
                            ProjectData.ClearProjectError()
                        End Try
                        Me.UpdateList()
                    End If
                    frmAdd.Dispose()
                End If
                ds = Nothing
                Me.tblRetrieveMaster = Nothing
                Me.tblRetrieveDetail = Nothing
            End If
        End If
    End Sub

    Private Sub RetrieveItemsFromSO()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("064")), 2)
            Else
                Dim _date As New frmDate
                AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                If (_date.ShowDialog = DialogResult.OK) Then
                    Dim strSQLLong As String = " 1 = 1"
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        strSQLLong = StringType.FromObject(ObjectType.AddObj(strSQLLong, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")), ")")))
                    End If
                    If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        strSQLLong = StringType.FromObject(ObjectType.AddObj(strSQLLong, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                    End If
                    Dim str As String = strSQLLong
                    strSQLLong = (strSQLLong & " AND a.ma_kh LIKE '" & Strings.Trim(Me.txtMa_kh.Text) & "%'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchSOTran4SI '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(str, 10), ", 'ph64', 'ct64'"})
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                    Me.tblRetrieveMaster = New DataView
                    Me.tblRetrieveDetail = New DataView
                    If (ds.Tables.Item(0).Rows.Count <= 0) Then
                        Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                    Else
                        Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                        Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                        Dim frmAdd As New Form
                        Dim gridformtran2 As New gridformtran
                        Dim gridformtran As New gridformtran
                        Dim tbs As New DataGridTableStyle
                        Dim style As New DataGridTableStyle
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= MaxColumns - 1)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("059"))
                        frmAdd.StartPosition = FormStartPosition.CenterParent
                        Dim panel As StatusBarPanel = AddStb(frmAdd)
                        gridformtran2.CaptionVisible = False
                        gridformtran2.ReadOnly = True
                        gridformtran2.Top = 0
                        gridformtran2.Left = 0
                        gridformtran2.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        gridformtran2.Width = (Me.Width - 5)
                        gridformtran2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                        gridformtran2.BackgroundColor = Color.White
                        gridformtran.CaptionVisible = False
                        gridformtran.ReadOnly = False
                        gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        gridformtran.Left = 0
                        gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
                        gridformtran.Width = (Me.Width - 5)
                        gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
                        gridformtran.BackgroundColor = Color.White
                        Dim button As New Button
                        button.Visible = True
                        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                        button.Left = (-100 - button.Width)
                        frmAdd.Controls.Add(button)
                        frmAdd.CancelButton = button
                        frmAdd.Controls.Add(gridformtran2)
                        frmAdd.Controls.Add(gridformtran)
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "SOMaster")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= MaxColumns - 1)
                        cols(2).Alignment = HorizontalAlignment.Right
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "SODetail4SI")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= MaxColumns - 1)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(2).ReadOnly = True
                        index = 3
                        Do While (1 <> 0)
                            Try
                                index += 1
                                gridformtran.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                            Catch exception1 As Exception
                                ProjectData.SetProjectError(exception1)
                                Dim exception As Exception = exception1
                                ProjectData.ClearProjectError()
                                Exit Do
                            End Try
                        Loop
                        Dim str5 As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                        Dim zero As Decimal = Decimal.Zero
                        Dim num4 As Decimal = Decimal.Zero
                        Dim count As Integer = Me.tblRetrieveMaster.Count
                        Dim num8 As Integer = (count - 1)
                        index = 0
                        Do While (index <= num8)
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien2"))) Then
                                zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tien2")))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2"))) Then
                                num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tien_nt2")))
                            End If
                            index += 1
                        Loop
                        str5 = Strings.Replace(Strings.Replace(Strings.Replace(str5, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, 0), "%n2", Strings.Trim(Strings.Format(num4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, 0), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, 0)
                        panel.Text = str5
                        AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdRetrieveMVCurrentCellChanged)
                        gridformtran2.CurrentRowIndex = 0
                        Dim num2 As Integer = 0
                        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num2).Item("stt_rec")), "'")
                        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
                        Obj.Init(frmAdd)
                        Dim button4 As New RadioButton
                        Dim button2 As New RadioButton
                        Dim button3 As New RadioButton
                        button4.Top = CInt(Math.Round(CDbl((((CDbl((Me.Height - 20)) / 2) + gridformtran.Height) + 5))))
                        button4.Left = 0
                        button4.Visible = True
                        button4.Checked = True
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("060"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("061"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("062"))
                        button3.Width = 200
                        button3.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        frmAdd.Controls.Add(button4)
                        frmAdd.Controls.Add(button2)
                        frmAdd.Controls.Add(button3)
                        frmAdd.ShowDialog()
                        If button4.Checked Then
                            ds = Nothing
                            Me.tblRetrieveMaster = Nothing
                            Me.tblRetrieveDetail = Nothing
                            Return
                        End If
                        Me.tblRetrieveDetail.RowFilter = ""
                        Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, stt_rec0"
                        Dim num7 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num7)
                            Me.tblRetrieveDetail.Item(index).Item("so_luong") = RuntimeHelpers.GetObjectValue(Me.tblRetrieveDetail.Item(index).Item("sl_dh0"))
                            Me.tblRetrieveDetail.Item(index).Row.AcceptChanges()
                            index += 1
                        Loop
                        Me.tblRetrieveDetail.RowFilter = "sl_dh0 <> 0"
                        Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                        count = (modVoucher.tblDetail.Count - 1)
                        If ((button3.Checked And flag) And (count >= 0)) Then
                            index = count
                            Do While (index >= 0)
                                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
                                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                        modVoucher.tblDetail.Item(index).Delete()
                                    End If
                                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                                        modVoucher.tblDetail.Item(index).Delete()
                                    End If
                                ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                End If
                                index = (index + -1)
                            Loop
                        End If
                        Dim tbl As New DataTable
                        tbl = Copy2Table(Me.tblRetrieveDetail)
                        Dim num6 As Integer = (tbl.Rows.Count - 1)
                        index = 0
                        Do While (index <= num6)
                            With tbl.Rows.Item(index)
                                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                    .Item("stt_rec") = ""
                                Else
                                    .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                                End If
                                .Item("sl_dh") = 0
                                .Item("sl_xuat") = 0
                                tbl.Rows.Item(index).AcceptChanges()
                            End With
                            index += 1
                        Loop
                        AppendFrom(modVoucher.tblDetail, tbl)
                        count = modVoucher.tblDetail.Count
                        If flag Then
                            index = (count - 1)
                            Do While (index >= 0)
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_dh")), "C") Then
                                    modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Me.UpdateList()
                        End If
                        frmAdd.Dispose()
                    End If
                    ds = Nothing
                    Me.tblRetrieveMaster = Nothing
                    Me.tblRetrieveDetail = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub RetrieveItemsFromSV()
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_kh.Text), "", False) = 0) Then
                Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("064")), 2)
            Else
                Dim _date As New frmDate
                AddHandler _date.Load, New EventHandler(AddressOf Me.frmRetrieveLoad)
                If (_date.ShowDialog = DialogResult.OK) Then
                    Dim strSQLLong As String = " 1 = 1"
                    If (ObjectType.ObjTst(_date.txtNgay_ct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        strSQLLong = StringType.FromObject(ObjectType.AddObj(strSQLLong, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct >= ", Sql.ConvertVS2SQLType(_date.txtNgay_ct.Value, "")), ")")))
                    End If
                    If (ObjectType.ObjTst(Me.txtNgay_lct.Text, Fox.GetEmptyDate, False) <> 0) Then
                        strSQLLong = StringType.FromObject(ObjectType.AddObj(strSQLLong, ObjectType.AddObj(ObjectType.AddObj(" AND (a.ngay_ct <= ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, "")), ")")))
                    End If
                    Dim str As String = strSQLLong
                    strSQLLong = (strSQLLong & " AND a.ma_kh LIKE '" & Strings.Trim(Me.txtMa_kh.Text) & "%'")
                    Dim tcSQL As String = String.Concat(New String() {"EXEC fs_SearchSVTran4SI '", modVoucher.cLan, "', ", vouchersearchlibobj.ConvertLong2ShortStrings(strSQLLong, 10), ", ", vouchersearchlibobj.ConvertLong2ShortStrings(str, 10), ", 'ph81', 'ct81'"})
                    Dim ds As New DataSet
                    Sql.SQLDecompressRetrieve((modVoucher.appConn), tcSQL, "tran", (ds))
                    Me.tblRetrieveMaster = New DataView
                    Me.tblRetrieveDetail = New DataView
                    If (ds.Tables.Item(0).Rows.Count <= 0) Then
                        Msg.Alert(StringType.FromObject(oVoucher.oClassMsg.Item("017")), 2)
                    Else
                        Me.tblRetrieveMaster.Table = ds.Tables.Item(0)
                        Me.tblRetrieveDetail.Table = ds.Tables.Item(1)
                        Dim frmAdd As New Form
                        Dim gridformtran2 As New gridformtran
                        Dim gridformtran As New gridformtran
                        Dim tbs As New DataGridTableStyle
                        Dim style As New DataGridTableStyle
                        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
                        Dim index As Integer = 0
                        Do
                            cols(index) = New DataGridTextBoxColumn
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= MaxColumns - 1)
                        frmAdd.Top = 0
                        frmAdd.Left = 0
                        frmAdd.Width = Me.Width
                        frmAdd.Height = Me.Height
                        frmAdd.Text = StringType.FromObject(modVoucher.oLan.Item("063"))
                        frmAdd.StartPosition = FormStartPosition.CenterParent
                        Dim panel As StatusBarPanel = AddStb(frmAdd)
                        gridformtran2.CaptionVisible = False
                        gridformtran2.ReadOnly = True
                        gridformtran2.Top = 0
                        gridformtran2.Left = 0
                        gridformtran2.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        gridformtran2.Width = (Me.Width - 5)
                        gridformtran2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
                        gridformtran2.BackgroundColor = Color.White
                        gridformtran.CaptionVisible = False
                        gridformtran.ReadOnly = False
                        gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
                        gridformtran.Left = 0
                        gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 60))))
                        gridformtran.Width = (Me.Width - 5)
                        gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
                        gridformtran.BackgroundColor = Color.White
                        Dim button As New Button
                        button.Visible = True
                        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
                        button.Left = (-100 - button.Width)
                        frmAdd.Controls.Add(button)
                        frmAdd.CancelButton = button
                        frmAdd.Controls.Add(gridformtran2)
                        frmAdd.Controls.Add(gridformtran)
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveMaster), gridformtran2, (tbs), (cols), "SVMaster")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= MaxColumns - 1)
                        cols(2).Alignment = HorizontalAlignment.Right
                        Fill2Grid.Fill(modVoucher.sysConn, (Me.tblRetrieveDetail), gridformtran, (style), (cols), "SVDetail4SI")
                        index = 0
                        Do
                            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                                cols(index).NullText = StringType.FromInteger(0)
                            Else
                                cols(index).NullText = ""
                            End If
                            index += 1
                        Loop While (index <= MaxColumns - 1)
                        Me.tblRetrieveDetail.AllowDelete = False
                        Me.tblRetrieveDetail.AllowNew = False
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(0).ReadOnly = True
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(1).ReadOnly = True
                        gridformtran.TableStyles.Item(0).GridColumnStyles.Item(2).ReadOnly = True
                        index = 3
                        Do While (1 <> 0)
                            Try
                                index += 1
                                gridformtran.TableStyles.Item(0).GridColumnStyles.Item(index).ReadOnly = True
                            Catch exception1 As Exception
                                ProjectData.SetProjectError(exception1)
                                Dim exception As Exception = exception1
                                ProjectData.ClearProjectError()
                                Exit Do
                            End Try
                        Loop
                        Dim str5 As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
                        Dim zero As Decimal = Decimal.Zero
                        Dim num4 As Decimal = Decimal.Zero
                        Dim count As Integer = Me.tblRetrieveMaster.Count
                        Dim num8 As Integer = (count - 1)
                        index = 0
                        Do While (index <= num8)
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tt"))) Then
                                zero = DecimalType.FromObject(ObjectType.AddObj(zero, Me.tblRetrieveMaster.Item(index).Item("t_tt")))
                            End If
                            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(Me.tblRetrieveMaster.Item(index).Item("t_tt_nt"))) Then
                                num4 = DecimalType.FromObject(ObjectType.AddObj(num4, Me.tblRetrieveMaster.Item(index).Item("t_tt_nt")))
                            End If
                            index += 1
                        Loop
                        str5 = Strings.Replace(Strings.Replace(Strings.Replace(str5, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, 0), "%n2", Strings.Trim(Strings.Format(num4, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, 0), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, 0)
                        panel.Text = str5
                        AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdRetrieveMVCurrentCellChanged)
                        gridformtran2.CurrentRowIndex = 0
                        Dim num2 As Integer = 0
                        Dim obj2 As Object = ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", Me.tblRetrieveMaster.Item(num2).Item("stt_rec")), "'")
                        Me.tblRetrieveDetail.RowFilter = StringType.FromObject(obj2)
                        Obj.Init(frmAdd)
                        Dim button4 As New RadioButton
                        Dim button2 As New RadioButton
                        Dim button3 As New RadioButton
                        button4.Top = CInt(Math.Round(CDbl((((CDbl((Me.Height - 20)) / 2) + gridformtran.Height) + 5))))
                        button4.Left = 0
                        button4.Visible = True
                        button4.Checked = True
                        button4.Text = StringType.FromObject(modVoucher.oLan.Item("060"))
                        button4.Width = 100
                        button4.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Top = button4.Top
                        button2.Left = (button4.Left + 110)
                        button2.Visible = True
                        button2.Text = StringType.FromObject(modVoucher.oLan.Item("061"))
                        button2.Width = 120
                        button2.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        button2.Enabled = False
                        button3.Top = button4.Top
                        button3.Left = (button2.Left + 130)
                        button3.Visible = True
                        button3.Text = StringType.FromObject(modVoucher.oLan.Item("062"))
                        button3.Width = 200
                        button3.Anchor = (AnchorStyles.Left Or AnchorStyles.Bottom)
                        frmAdd.Controls.Add(button4)
                        frmAdd.Controls.Add(button2)
                        frmAdd.Controls.Add(button3)
                        frmAdd.ShowDialog()
                        If button4.Checked Then
                            ds = Nothing
                            Me.tblRetrieveMaster = Nothing
                            Me.tblRetrieveDetail = Nothing
                            Return
                        End If
                        Me.tblRetrieveDetail.RowFilter = ""
                        Me.tblRetrieveDetail.Sort = "ngay_ct, so_ct, stt_rec, stt_rec0"
                        Dim num7 As Integer = (Me.tblRetrieveDetail.Count - 1)
                        index = 0
                        Do While (index <= num7)
                            With Me.tblRetrieveDetail.Item(index)
                                .Item("so_luong") = RuntimeHelpers.GetObjectValue(.Item("sl_hd0"))
                                .Row.AcceptChanges()
                            End With
                            index += 1
                        Loop
                        Me.tblRetrieveDetail.RowFilter = "sl_hd0 <> 0"
                        Dim flag As Boolean = (Me.tblRetrieveDetail.Count > 0)
                        count = (modVoucher.tblDetail.Count - 1)
                        If ((button3.Checked And flag) And (count >= 0)) Then
                            index = count
                            Do While (index >= 0)
                                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
                                    If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                        modVoucher.tblDetail.Item(index).Delete()
                                    End If
                                    If (ObjectType.ObjTst(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"), False) = 0) Then
                                        modVoucher.tblDetail.Item(index).Delete()
                                    End If
                                ElseIf Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec"))) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(index).Item("stt_rec"))), "", False) = 0) Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                End If
                                index = (index + -1)
                            Loop
                        End If
                        Dim tbl As New DataTable
                        tbl = Copy2Table(Me.tblRetrieveDetail)
                        Dim num6 As Integer = (tbl.Rows.Count - 1)
                        index = 0
                        Do While (index <= num6)
                            With tbl.Rows.Item(index)
                                If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                                    .Item("stt_rec") = ""
                                Else
                                    .Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                                End If
                                .Item("sl_xuat") = 0
                                tbl.Rows.Item(index).AcceptChanges()
                            End With
                            index += 1
                        Loop
                        AppendFrom(modVoucher.tblDetail, tbl)
                        count = modVoucher.tblDetail.Count
                        If flag Then
                            index = (count - 1)
                            Do While (index >= 0)
                                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("ma_vt")), "C") Then
                                    modVoucher.tblDetail.Item(index).Delete()
                                ElseIf Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(index).Item("stt_rec_hd")), "C") Then
                                    modVoucher.tblDetail.Item(index).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
                                End If
                                index = (index + -1)
                            Loop
                            Me.UpdateList()
                        End If
                        frmAdd.Dispose()
                    End If
                    ds = Nothing
                    Me.tblRetrieveMaster = Nothing
                    Me.tblRetrieveDetail = Nothing
                End If
            End If
        End If
    End Sub

    Public Sub Save()
        Me.txtStatus.Text = Strings.Trim(StringType.FromObject(Me.tblHandling.Rows.Item(Me.cboAction.SelectedIndex).Item("action_id")))
        Me.txtLoai_ct.Text = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmmagd", "loai_ct", String.Concat(New String() {"ma_ct = '", modVoucher.VoucherCode, "' AND ma_gd = '", Strings.Trim(Me.txtMa_gd.Text), "'"})))
        'If (((StringType.StrCmp(Strings.Trim(Me.txtMa_gd.Text), "1", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "1", False) = 0)) Or ((StringType.StrCmp(Strings.Trim(Me.txtMa_gd.Text), "2", False) = 0) And (StringType.StrCmp(Strings.Trim(Me.txtStatus.Text), "2", False) = 0))) Then
        '    Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("007")), 2)
        '    oVoucher.isContinue = False
        '    Return
        'End If
        Me.txtNgay_ct.Value = Me.txtNgay_lct.Value
        Try
            Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            ProjectData.ClearProjectError()
        End Try
        If Not Me.oSecurity.GetActionRight Then
            oVoucher.isContinue = False
            Return
        End If
        If Not Me.grdHeader.CheckEmpty(RuntimeHelpers.GetObjectValue(oVoucher.oClassMsg.Item("035"))) Then
            oVoucher.isContinue = False
            Return
        End If
        Dim num As Integer
        Dim num3 As Integer = 0
        Dim num11 As Integer = (modVoucher.tblDetail.Count - 1)
        num = 0
        Do While (num <= num11)
            If (Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt"))) AndAlso (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt"))), "", False) <> 0)) Then
                num3 = 1
                Exit Do
            End If
            num += 1
        Loop
        If (num3 = 0) Then
            Msg.Alert(StringType.FromObject(modVoucher.oLan.Item("022")), 2)
            oVoucher.isContinue = False
            Return
        End If
        If (Me.txtStatus.Text = "2") And Sql.GetValue(appConn, "exec sp18_hanno '" + oVoucher.VoucherCode + "','" + Me.txtMa_kh.Text.Replace(",", "''") + "',''," + Conversion.Str(Me.txtT_tt.Value) + "," + Sql.ConvertVS2SQLType(Me.txtNgay_ct.Value, "")) = 0 Then
            If Msg.Question("Vượt hạn mức nợ hoặc hạn thanh toán, Bạn có muốn trình duyệt không", 1) = 1 Then
                Me.txtStatus.Text = "1"
            Else
                oVoucher.isContinue = False
                Return
            End If
        End If
        Dim str As String
        Dim num2 As Integer
        num3 = (modVoucher.tblDetail.Count - 1)
        num = num3
        Do While (num >= 0)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt"))) Then
                If (StringType.StrCmp(Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(num).Item("ma_vt"))), "", False) = 0) Then
                    modVoucher.tblDetail.Item(num).Delete()
                End If
            Else
                modVoucher.tblDetail.Item(num).Delete()
            End If
            num = (num + -1)
        Loop
        Dim cString As String = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldchar", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim num10 As Integer = (modVoucher.tblDetail.Count - 1)
        num = 0
        Do While (num <= num10)
            Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            num2 = 1
            Do While (num2 <= num9)
                str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                    modVoucher.tblDetail.Item(num).Item(str) = ""
                End If
                num2 += 1
            Loop
            num += 1
        Loop
        cString = StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldnumeric", ("ma_ct = '" & modVoucher.VoucherCode & "'")))
        Dim num8 As Integer = (modVoucher.tblDetail.Count - 1)
        num = 0
        Do While (num <= num8)
            Dim num7 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
            num2 = 1
            Do While (num2 <= num7)
                str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item(str))) Then
                    modVoucher.tblDetail.Item(num).Item(str) = 0
                End If
                num2 += 1
            Loop
            num += 1
        Loop
        If (StringType.StrCmp(Me.txtStatus.Text, "0", False) <> 0) Then
            Dim strFieldList As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.sysConn), "voucherinfo", "fieldcheck", ("ma_ct = '" & modVoucher.VoucherCode & "'"))))
            If (StringType.StrCmp(Strings.Trim(strFieldList), "", False) <> 0) Then
                num3 = (modVoucher.tblDetail.Count - 1)
                Dim cMap As String = clsfields.CheckEmptyFieldList("stt_rec", strFieldList, modVoucher.tblDetail)
                Try
                    If (StringType.StrCmp(cMap, "", False) <> 0) Then
                        Msg.Alert(Strings.Replace(StringType.FromObject(oVoucher.oClassMsg.Item("044")), "%s", GetColumn(Me.grdDetail, cMap).HeaderText, 1, -1, 0), 2)
                        oVoucher.isContinue = False
                        Return
                    End If
                Catch exception2 As Exception
                    ProjectData.SetProjectError(exception2)
                    Dim exception As Exception = exception2
                    ProjectData.ClearProjectError()
                End Try
            End If
            If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
                Me.cIDNumber = ""
            Else
                Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
            End If
            If Not oVoucher.CheckDuplVoucherNumber(Fox.PadL(Strings.Trim(Me.txtSo_ct.Text), Me.txtSo_ct.MaxLength), StringType.FromObject(Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "New", Me.cIDNumber))) Then
                Me.txtSo_ct.Focus()
                oVoucher.isContinue = False
                Return
            End If
        End If
        If Not Me.xInventory.isValid Then
            oVoucher.isContinue = False
            Return
        End If
        Dim str6 As String
        Me.pnContent.Text = StringType.FromObject(modVoucher.oVar.Item("m_process"))
        Me.UpdateSI()
        Me.UpdateList()
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Me.cIDNumber = oVoucher.GetIdentityNumber
            modVoucher.tblMaster.AddNew()
            Me.iMasterRow = (modVoucher.tblMaster.Count - 1)
            modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec") = Me.cIDNumber
            modVoucher.tblMaster.Item(Me.iMasterRow).Item("ma_ct") = modVoucher.VoucherCode
        Else
            Me.cIDNumber = StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
            Me.BeforUpdateSI(Me.cIDNumber, "Edit")
        End If
        xtabControl.GatherMemvarTabControl(modVoucher.tblMaster.Item(Me.iMasterRow), Me.tbDetail)
        DirLib.SetDatetime(modVoucher.appConn, modVoucher.tblMaster.Item(Me.iMasterRow), oVoucher.cAction)
        Me.grdHeader.DataRow = modVoucher.tblMaster.Item(Me.iMasterRow).Row
        Me.grdHeader.Gather()
        GatherMemvar(modVoucher.tblMaster.Item(Me.iMasterRow), Me)
        modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct") = Fox.PadL(Strings.Trim(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("so_ct"))), Me.txtSo_ct.MaxLength)
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            str6 = GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row)
        Else
            Dim cKey As String = StringType.FromObject(ObjectType.AddObj(ObjectType.AddObj("stt_rec = '", modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "'"))
            str6 = ((GenSQLUpdate((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_phdbf"))), modVoucher.tblMaster.Item(Me.iMasterRow).Row, cKey) & ChrW(13) & GenSQLDelete(Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), cKey)) & ChrW(13) & GenSQLDelete("ctcp20", cKey))
        End If
        cString = "ma_ct, ngay_ct, so_ct, stt_rec"
        Dim str5 As String = ("stt_rec = '" & Me.cIDNumber & "' or stt_rec = '' or stt_rec is null")
        modVoucher.tblDetail.RowFilter = str5
        num3 = (modVoucher.tblDetail.Count - 1)
        Dim num4 As Integer = 0
        Dim num6 As Integer = num3
        num = 0
        Do While (num <= num6)
            If (ObjectType.ObjTst(modVoucher.tblDetail.Item(num).Item("stt_rec"), Interaction.IIf((StringType.StrCmp(oVoucher.cAction, "New", False) = 0), "", RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))), False) = 0) Then
                Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(cString, ","c))
                num2 = 1
                Do While (num2 <= num5)
                    str = Strings.Trim(Fox.GetWordNum(cString, num2, ","c))
                    modVoucher.tblDetail.Item(num).Item(str) = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item(str))
                    num2 += 1
                Loop
                num4 += 1
                modVoucher.tblDetail.Item(num).Item("line_nbr") = num4
                Me.grdDetail.Update()
                str6 = (str6 & ChrW(13) & GenSQLInsert((modVoucher.appConn), Strings.Trim(StringType.FromObject(modVoucher.oVoucherRow.Item("m_ctdbf"))), modVoucher.tblDetail.Item(num).Row))
            End If
            num += 1
        Loop
        oVoucher.IncreaseVoucherNo(Strings.Trim(Me.txtSo_ct.Text))
        Me.EDTBColumns(False)
        Sql.SQLCompressExecute((modVoucher.appConn), str6)
        str6 = Me.Post
        Sql.SQLExecute((modVoucher.appConn), str6)
        Me.grdHeader.UpdateFreeField(modVoucher.appConn, StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")))
        Me.AfterUpdateSI(StringType.FromObject(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec")), "Save")
        Me.pnContent.Text = ""
        SaveLocalDataView(modVoucher.tblDetail)
        oVoucher.RefreshStatus(Me.cboStatus)
        xtabControl.ReadOnlyTabControls(True, Me.tbDetail)
    End Sub

    Public Sub Search()
        Dim frm As New frmSearch
        frm.ShowDialog()
    End Sub

    Private Sub SetEmptyColKey(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.oInvItemDetail.Cancel Then
            Me.iOldRow = Me.grdDetail.CurrentRowIndex
            Me.cOldItem = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
            If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec")))) Then
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec") = ""
                Me.WhenAddNewItem()
                oVoucher.CarryOn(modVoucher.tblDetail, currentRowIndex)
            End If
            If ((StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) And Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(currentRowIndex).Item("stt_rec")))) Then
                modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec") = RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(Me.iMasterRow).Item("stt_rec"))
                Me.WhenAddNewItem()
                oVoucher.CarryOn(modVoucher.tblDetail, currentRowIndex)
            End If
        End If
    End Sub

    Private Sub ShowTabDetail()
        Me.tbDetail.SelectedIndex = 0
    End Sub

    Private Sub tbDetail_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles tbDetail.Enter
        Me.grdDetail.Focus()
    End Sub

    Private Sub TransTypeLostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.Leave
        Me.EDTranType()
    End Sub

    Private Sub txt_Enter(ByVal sender As Object, ByVal e As EventArgs)
        'If Not IsDBNull(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("sl_hd")) Then
        '    If modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("sl_hd") <> 0 Then
        '        sender.ReadOnly = True
        '        Return
        '    End If
        'End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt"))) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {True}, Nothing)
        Else
            Dim str As String = Strings.Trim(StringType.FromObject(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_vt")))
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(StringType.StrCmp(str, "", False) = 0)}, Nothing)
        End If
    End Sub
    Private Sub txt_Enter_Ma_vt(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsDBNull(modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("sl_hd")) Then
            If modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("sl_hd") <> 0 Then
                sender.ReadOnly = True
                Return
            End If
        End If
    End Sub

    Private Sub txtDien_giai_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtDien_giai.Leave
    End Sub

    Private Sub txtKeyPress_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtKeyPress.Enter
        Me.grdDetail.Focus()
        Me.grdDetail.CurrentCell = New DataGridCell(0, 0)
    End Sub

    Private Sub txtMa_gd_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtMa_gd.Enter
        If (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0) Then
            Me.txtMa_gd.ReadOnly = True
        End If
        If (StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Then
            Dim flag As Boolean = False
            Dim num2 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ma_vt")), "C") Then
                    flag = True
                    Exit Do
                End If
                i += 1
            Loop
            Me.txtMa_gd.ReadOnly = flag
        End If
    End Sub

    Private Sub txtMa_gd_Valid(ByVal sender As Object, ByVal e As EventArgs)
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
            Me.EDTrans()
        End If
    End Sub

    Private Sub txtMa_kh_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmkh", StringType.FromObject(ObjectType.AddObj("ma_kh = ", Sql.ConvertVS2SQLType(RuntimeHelpers.GetObjectValue(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), "")))), DataRow)
        If ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) And (Not row Is Nothing)) Then
            If (StringType.StrCmp(Strings.Trim(Me.txtMa_nvbh.Text), "", False) = 0) Then
                Me.txtMa_nvbh.Text = Strings.Trim(StringType.FromObject(row.Item("ma_nvbh")))
            End If
        End If
    End Sub

    Private Sub txtNumber_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtSo_ct.Enter
        LateBinding.LateSet(sender, Nothing, "Text", New Object() {Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))}, Nothing)
    End Sub
    Private Sub txtSo_luong_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num4 As Decimal = Me.noldSo_luong
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) = 0) Then
            Return
        End If
        Dim num2 As Byte
        Dim num3 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = num3
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If

        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            .Item("so_luong") = num
            If IsDBNull(.Item("gia_nt2")) Then
                .Item("gia_nt2") = 0
            End If
            If IsDBNull(.Item("gia2")) Then
                .Item("gia2") = 0
            End If
            .Item("tien_nt2") = Math.Round(.Item("so_luong") * .Item("gia_nt2"), num2)
            .Item("tien2") = Math.Round(.Item("tien_nt2") * Me.txtTy_gia.Value, num3)
            If IsDBNull(.Item("tl_ck")) Then
                .Item("tl_ck") = 0
            End If
            .Item("ck_nt") = Math.Round(.Item("tien_nt2") * .Item("tl_ck") / 100, num2)
            .Item("ck") = Math.Round(.Item("ck_nt") * Me.txtTy_gia.Value, num3)
            .Item("tien_ban_nt0") = Round(.Item("gia_ban_nt0") * .Item("so_luong"), num2)
            .Item("tien_ban0") = Math.Round(.Item("tien_ban_nt0") * Me.txtTy_gia.Value, num3)
        End With
        Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
        Me.grdDetail.Refresh()
        Me.UpdateList()
    End Sub
    Private Sub txtGia_nt2_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num6 As Decimal = Me.noldGia_nt2
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) = 0) Then
            Return
        End If
        Dim m_round_gia_nt As Byte
        Dim m_round_tien_nt As Byte
        Dim m_round_tien As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim m_round_gia As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            m_round_gia_nt = m_round_gia
            m_round_tien_nt = m_round_tien
        Else
            m_round_tien_nt = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            m_round_gia_nt = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If

        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            .Item("gia_nt2") = num
            .Item("gia2") = Math.Round(.Item("gia_nt2") * Me.txtTy_gia.Value, m_round_gia)
            .Item("tien_nt2") = Math.Round(.Item("so_luong") * .Item("gia_nt2"), m_round_tien_nt)
            .Item("tien2") = Math.Round(.Item("tien_nt2") * Me.txtTy_gia.Value, m_round_tien)
            If IsDBNull(.Item("tl_ck")) Then
                .Item("tl_ck") = 0
            End If
            .Item("ck_nt") = Math.Round(.Item("tien_nt2") * .Item("tl_ck") / 100, m_round_tien_nt)
            .Item("ck") = Math.Round(.Item("ck_nt") * Me.txtTy_gia.Value, m_round_tien)
            If .Item("so_luong") = 0 Then
                .Item("gia_ck_nt") = Round(.Item("gia_nt2") * (1 - .Item("tl_ck") / 100), m_round_gia_nt)
            Else
                .Item("gia_ck_nt") = Math.Round((.Item("tien_nt2") - .Item("ck_nt")) / .Item("so_luong"), m_round_gia_nt)
            End If
            .Item("gia_ck") = Math.Round(.Item("gia_ck_nt") * Me.txtTy_gia.Value, m_round_gia)
            If IsDBNull(.Item("gia_ban_nt0")) Or .Item("gia_ban_nt0") = 0 Or doi_gia_vat = False Then
                .Item("gia_ban_nt0") = .Item("gia_ck_nt")
                .Item("gia_ban0") = .Item("gia_ck")
                .Item("tien_ban_nt0") = .Item("tien_nt2") - .Item("ck_nt")
                .Item("tien_ban0") = .Item("tien2") - .Item("ck")
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            End If
        End With
        Me.UpdateList()
    End Sub
    Private Sub txtGia_ban_nt0_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num6 As Decimal = Me.noldGia_ban_nt0
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num6) = 0) Then
            Return
        End If
        Dim m_round_gia_nt As Byte
        Dim m_round_tien_nt As Byte
        Dim m_round_tien As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim m_round_gia As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))

        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            m_round_gia_nt = m_round_gia
            m_round_tien_nt = m_round_tien
        Else
            m_round_tien_nt = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            m_round_gia_nt = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If
        With tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            .Item("gia_ban0") = Math.Round(.Item("gia_ban_nt0") * Me.txtTy_gia.Value, m_round_gia)
            .Item("tien_ban_nt0") = Round(.Item("gia_ban_nt0") * .Item("so_luong"), m_round_tien_nt)
            .Item("tien_ban0") = Round(.Item("tien_ban_nt0") * Me.txtTy_gia.Value, m_round_tien)
            If .Item("gia_ban_nt0") <> 0 Then
                doi_gia_vat = True
            End If
        End With

        Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
        UpdateList()
    End Sub
    Private Sub txtTl_ck_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Byte, m_round_gia As Byte, m_round_gia_nt As Byte
        Dim digits As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        m_round_gia = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num2 = digits
            m_round_gia_nt = m_round_gia
        Else
            num2 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
            m_round_gia_nt = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If
        Dim num4 As Decimal = Me.noldTl_ck
        Dim num As New Decimal(Conversion.Val(Strings.Replace(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)), " ", "", 1, -1, CompareMethod.Binary)))
        If (Decimal.Compare(num, num4) <> 0) Then
            With tblDetail.Item(grdDetail.CurrentRowIndex)
                .Item("tl_ck") = num
                If IsDBNull(.Item("tien_nt2")) Then
                    .Item("tien_nt2") = 0
                End If
                .Item("ck_nt") = Math.Round(.Item("tien_nt2") * num / 100, num2)
                .Item("ck") = Math.Round(.Item("ck_nt") * Me.txtTy_gia.Value, digits)
                .Item("gia_ck_nt") = IIf(.Item("so_luong") = 0, Round(.Item("gia_nt2") * (1 - .Item("tl_ck") / 100), m_round_gia_nt), Math.Round((.Item("tien_nt2") - .Item("ck_nt")) / .Item("so_luong"), m_round_gia_nt))
                .Item("gia_ck") = Math.Round(.Item("gia_ck_nt") * Me.txtTy_gia.Value, m_round_gia)
                If IsDBNull(.Item("gia_ban_nt0")) Or .Item("gia_ban_nt0") = 0 Or doi_gia_vat = False Then
                    .Item("gia_ban_nt0") = .Item("gia_ck_nt")
                    .Item("gia_ban0") = .Item("gia_ck")
                    .Item("tien_ban_nt0") = .Item("tien_nt2") - .Item("ck_nt")
                    .Item("tien_ban0") = .Item("tien2") - .Item("ck")
                End If
            End With
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            Me.UpdateList()
        End If
    End Sub
    Private Sub txtMa_thue_valid(ByVal sender As Object, ByVal e As EventArgs)
        Dim num As Byte
        Dim num2 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = num2
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str As String = Me.coldMa_thue
        Dim str3 As String = StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))
        If (StringType.StrCmp(Strings.Trim(str3), Strings.Trim(str), False) <> 0) Then
            Dim zero As Decimal
            If (StringType.StrCmp(Strings.Trim(str3), "", False) = 0) Then
                zero = Decimal.Zero
            Else
                Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", ("ma_thue = '" & Strings.Trim(str3) & "'")), DataRow)
                zero = DecimalType.FromObject(row.Item("thue_suat"))
                row = Nothing
            End If
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue_suat") = zero
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("ma_thue") = str3
            Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            Me.UpdateList()
            Me.colThue_nt.TextBox.Text = Strings.Trim(StringType.FromObject(tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("thue_nt")))
        End If
    End Sub

    Public Sub UpdateList()
        Dim zero As Decimal = Decimal.Zero
        Dim num10 As Decimal = Decimal.Zero
        'Dim num4 As Decimal = Decimal.Zero
        'Dim num5 As Decimal = Decimal.Zero
        Dim num2 As Decimal = Decimal.Zero
        Dim num3 As Decimal = Decimal.Zero
        Dim num7 As Decimal = Decimal.Zero
        Dim num8 As Decimal = Decimal.Zero
        Dim num6 As Decimal = Decimal.Zero
        Dim t_tien2_cn As Decimal = 0, tien2_cn As Decimal = 0, t_thue_cn As Decimal = 0
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit", "View"}) Then
            Dim num11 As Integer = (modVoucher.tblDetail.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num11)
                With modVoucher.tblDetail.Item(i)
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien2"))) Then
                        zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblDetail.Item(i).Item("tien2")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("tien_nt2"))) Then
                        num10 = DecimalType.FromObject(ObjectType.AddObj(num10, modVoucher.tblDetail.Item(i).Item("tien_nt2")))
                    End If
                    If modVoucher.tblDetail.Item(i).Item("s2") = "1" Then
                        If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("s6"))) Then
                            t_tien2_cn += modVoucher.tblDetail.Item(i).Item("s6")
                        End If
                        If Not IsDBNull(.Item("thue")) Then
                            t_thue_cn += .Item("thue")
                        End If
                    Else
                        tien2_cn = Math.Round(.Item("so_luong") * Math.Round(.Item("gia_ban") / (1 + .Item("thue_suat") / 100), 2), 0)
                        t_tien2_cn += tien2_cn
                        t_thue_cn += Math.Round(tien2_cn * .Item("thue_suat") / 100, 0)
                    End If
                    'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("cp"))) Then
                    '    num4 = DecimalType.FromObject(ObjectType.AddObj(num4, modVoucher.tblDetail.Item(i).Item("cp")))
                    'End If
                    'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("cp_nt"))) Then
                    '    num5 = DecimalType.FromObject(ObjectType.AddObj(num5, modVoucher.tblDetail.Item(i).Item("cp_nt")))
                    'End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ck"))) Then
                        num2 = DecimalType.FromObject(ObjectType.AddObj(num2, modVoucher.tblDetail.Item(i).Item("ck")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("ck_nt"))) Then
                        num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblDetail.Item(i).Item("ck_nt")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("thue"))) Then
                        num7 = DecimalType.FromObject(ObjectType.AddObj(num7, modVoucher.tblDetail.Item(i).Item("thue")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("thue_nt"))) Then
                        num8 = DecimalType.FromObject(ObjectType.AddObj(num8, modVoucher.tblDetail.Item(i).Item("thue_nt")))
                    End If
                    If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(i).Item("so_luong"))) Then
                        num6 = DecimalType.FromObject(ObjectType.AddObj(num6, modVoucher.tblDetail.Item(i).Item("so_luong")))
                    End If
                End With
                i += 1
            Loop
        End If
        Me.txtT_so_luong.Value = Convert.ToDouble(num6)
        'Me.txtT_cp.Value = Convert.ToDouble(num4)
        'Me.txtT_cp_nt.Value = Convert.ToDouble(num5)
        Me.txtT_ck.Value = Convert.ToDouble(num2)
        Me.txtT_ck_nt.Value = Convert.ToDouble(num3)
        Me.txtT_thue.Value = Convert.ToDouble(num7)
        Me.txtT_thue_nt.Value = Convert.ToDouble(num8)
        Me.txtS6.Value = Convert.ToDouble(t_tien2_cn)
        Me.txtS4.Value = Convert.ToDouble(t_thue_cn)
        If CInt(oOption.Item("m_gia_sale")) = 0 Then
            Me.txtT_tien2.Value = Convert.ToDouble(zero)
            Me.txtT_tien_nt2.Value = Convert.ToDouble(num10)
            Me.txtT_tt.Value = ((Me.txtT_tien2.Value + Me.txtT_thue.Value) - Me.txtT_ck.Value)
            Me.txtT_tt_nt.Value = ((Me.txtT_tien_nt2.Value + Me.txtT_thue_nt.Value) - Me.txtT_ck_nt.Value)
        Else
            Me.txtT_tt_nt.Value = num10 - Me.txtT_ck_nt.Value
            Me.txtT_tt.Value = zero - Me.txtT_ck.Value
            Me.txtT_tien_nt2.Value = Me.txtT_tt_nt.Value - Me.txtT_thue_nt.Value
            Me.txtT_tien2.Value = Me.txtT_tt.Value - Me.txtT_thue.Value
        End If
    End Sub

    Private Sub UpdateSI()
    End Sub

    Public Sub vCaptionRefresh()
        Me.EDFC()
        Dim cAction As String = oVoucher.cAction
        If ((StringType.StrCmp(cAction, "Edit", False) = 0) OrElse (StringType.StrCmp(cAction, "View", False) = 0)) Then
            Me.pnContent.Text = ""
        Else
            Me.pnContent.Text = ""
        End If
    End Sub

    Public Sub vFCRate()
        If (Me.txtTy_gia.Value <> Convert.ToDouble(oVoucher.noldFCrate)) Then
            Dim num As Integer
            Dim num3 As Integer = (modVoucher.tblDetail.Count - 1)
            num = 0
            Do While (num <= num3)
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("gia_nt2"))) Then
                    tblDetail.Item(num).Item("gia2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("gia_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("gia_ban_nt"))) Then
                    tblDetail.Item(num).Item("gia_ban") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("gia_ban_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("tien_nt2"))) Then
                    tblDetail.Item(num).Item("tien2") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("tien_nt2"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("thue_nt"))) Then
                    tblDetail.Item(num).Item("thue") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("thue_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                End If
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("ck_nt"))) Then
                    tblDetail.Item(num).Item("ck") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("ck_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
                'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_vc_nt"))) Then
                '    tblDetail.Item(num).Item("cp_vc") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_vc_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                'End If
                'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_bh_nt"))) Then
                '    tblDetail.Item(num).Item("cp_bh") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_bh_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                'End If
                'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_khac_nt"))) Then
                '    tblDetail.Item(num).Item("cp_khac") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_khac_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                'End If
                'If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(num).Item("cp_nt"))) Then
                '    tblDetail.Item(num).Item("cp") = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(num).Item("cp_nt"), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))}, Nothing, Nothing))
                'End If
                num += 1
            Loop
        End If
        Me.txtT_tien2.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_tien_nt2.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        'Me.txtT_cp.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_cp_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_ck.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_ck_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_thue.Value = DoubleType.FromObject(Fox.Round(CDbl((Me.txtT_thue_nt.Value * Me.txtTy_gia.Value)), IntegerType.FromObject(modVoucher.oVar.Item("m_round_tien"))))
        Me.txtT_tt.Value = ((Me.txtT_tien2.Value + Me.txtT_thue.Value) - Me.txtT_ck.Value)
    End Sub

    Public Sub View()
        Dim num3 As Decimal
        Dim frmAdd As New Form
        Dim gridformtran2 As New gridformtran
        Dim gridformtran As New gridformtran
        Dim tbs As New DataGridTableStyle
        Dim style As New DataGridTableStyle
        Dim cols As DataGridTextBoxColumn() = New DataGridTextBoxColumn(MaxColumns) {}
        Dim index As Integer = 0
        Do
            cols(index) = New DataGridTextBoxColumn
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= MaxColumns - 1)
        frmAdd.Top = 0
        frmAdd.Left = 0
        frmAdd.Width = Me.Width
        frmAdd.Height = Me.Height
        frmAdd.Text = Me.Text
        frmAdd.StartPosition = FormStartPosition.CenterParent
        Dim panel As StatusBarPanel = AddStb(frmAdd)
        gridformtran2.CaptionVisible = False
        gridformtran2.ReadOnly = True
        gridformtran2.Top = 0
        gridformtran2.Left = 0
        gridformtran2.Height = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
        gridformtran2.Width = (Me.Width - 5)
        gridformtran2.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or (AnchorStyles.Bottom Or AnchorStyles.Top)))
        gridformtran2.BackgroundColor = Color.White
        gridformtran.CaptionVisible = False
        gridformtran.ReadOnly = True
        gridformtran.Top = CInt(Math.Round(CDbl((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2))))
        gridformtran.Left = 0
        gridformtran.Height = CInt(Math.Round(CDbl(((CDbl((Me.Height - SystemInformation.CaptionHeight)) / 2) - 30))))
        gridformtran.Width = (Me.Width - 5)
        gridformtran.Anchor = (AnchorStyles.Right Or (AnchorStyles.Left Or AnchorStyles.Bottom))
        gridformtran.BackgroundColor = Color.White
        Dim button As New Button
        button.Visible = True
        button.Anchor = (AnchorStyles.Left Or AnchorStyles.Top)
        button.Left = (-100 - button.Width)
        frmAdd.Controls.Add(button)
        frmAdd.CancelButton = button
        frmAdd.Controls.Add(gridformtran2)
        frmAdd.Controls.Add(gridformtran)
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblMaster), gridformtran2, (tbs), (cols), "SIMaster")
        index = 0
        Do
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= MaxColumns - 1)
        cols(2).Alignment = HorizontalAlignment.Right
        Fill2Grid.Fill(modVoucher.sysConn, (modVoucher.tblDetail), gridformtran, (style), (cols), "SIDetail")
        index = 0
        Do
            If (Strings.InStr(modVoucher.tbcDetail(index).Format, "0", 0) > 0) Then
                cols(index).NullText = StringType.FromInteger(0)
            Else
                cols(index).NullText = ""
            End If
            index += 1
        Loop While (index <= MaxColumns - 1)
        oVoucher.HideFields(gridformtran)
        Dim str As String = StringType.FromObject(oVoucher.oClassMsg.Item("016"))
        Dim count As Integer = modVoucher.tblMaster.Count
        Dim zero As Decimal = Decimal.Zero
        Dim num5 As Integer = (count - 1)
        index = 0
        Do While (index <= num5)
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tt"))) Then
                zero = DecimalType.FromObject(ObjectType.AddObj(zero, modVoucher.tblMaster.Item(index).Item("t_tt")))
            End If
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(modVoucher.tblMaster.Item(index).Item("t_tt_nt"))) Then
                num3 = DecimalType.FromObject(ObjectType.AddObj(num3, modVoucher.tblMaster.Item(index).Item("t_tt_nt")))
            End If
            index += 1
        Loop
        str = Strings.Replace(str, "%n1", Strings.Trim(StringType.FromInteger(count)), 1, -1, 0)
        If (0 <> 0) Then
            str = Strings.Replace(Strings.Replace(str, "%n2", Strings.Trim(Strings.Format(num3, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien_nt")))), 1, -1, 0), "%n3", Strings.Trim(Strings.Format(zero, StringType.FromObject(modVoucher.oVar.Item("m_ip_tien")))), 1, -1, 0)
        Else
            str = Strings.Replace(Strings.Replace(str, "%n2", "X", 1, -1, 0), "%n3", "X", 1, -1, 0)
        End If
        panel.Text = str
        AddHandler gridformtran2.CurrentCellChanged, New EventHandler(AddressOf Me.grdMVCurrentCellChanged)
        gridformtran2.CurrentRowIndex = Me.iMasterRow
        Obj.Init(frmAdd)
        Dim collection2 As New Collection
        collection2.Add(Me, "Form", Nothing, Nothing)
        collection2.Add(gridformtran2, "grdHeader", Nothing, Nothing)
        collection2.Add(gridformtran, "grdDetail", Nothing, Nothing)
        Me.oSecurity.aVGrid = collection2
        Me.oSecurity.InnitView()
        Me.oSecurity.InvisibleView()
        frmAdd.ShowDialog()
        frmAdd.Dispose()
        Me.iMasterRow = gridformtran2.CurrentRowIndex
        Me.RefrehForm()
    End Sub

    Public Sub vTextRefresh()
    End Sub

    Private Sub WhenAddNewItem()
        modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("stt_rec0") = Me.GetIDItem(modVoucher.tblDetail, "0")
    End Sub

    Private Sub WhenItemLeave(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (Me.iOldRow <> currentRowIndex) Then
            Return
        End If
        If Me.oInvItemDetail.Cancel Then
            Return
        End If
        Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        If (StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldItem), False) = 0) Then
            Return
        End If
        With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vt")), "C") Then
                Return
            End If
            Dim str3 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
            Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmvt", ("ma_vt = '" & str3 & "'")), DataRow)
            .Item("dvt") = RuntimeHelpers.GetObjectValue(row.Item("dvt"))
            Me.colDvt.TextBox.Text = StringType.FromObject(.Item("dvt"))
            .Item("he_so") = 1
            If BooleanType.FromObject(row.Item("nhieu_dvt")) Then
                Me.oUOM.Empty = False
                Me.colDvt.ReadOnly = False
                Me.oUOM.Cancel = False
                Me.oUOM.Check = True
            Else
                Me.oUOM.Empty = True
                Me.colDvt.ReadOnly = True
                Me.oUOM.Cancel = True
                Me.oUOM.Check = False
            End If
            If IsDBNull(row.Item("lo_yn")) Then
                Sql.SQLExecute(appConn, "update dmvt set lo_yn=0 where lo_yn is null")
                row.Item("lo_yn") = 0
            End If
            If BooleanType.FromObject(ObjectType.NotObj(row.Item("lo_yn"))) Then
                .Item("ma_lo") = ""
            Else
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_lo")), "C") Then
                    Try
                        Dim rowLot As DataRow = Sql.GetRow(modVoucher.appConn, ("spGetLotNumber '" & Strings.Trim(str3) & "','" + Me.txtMa_dvcs.Text.ToString.Trim + "'"))
                        .Item("ma_lo") = rowLot.Item("ma_lo").ToString()
                        .Item("ma_kho") = rowLot.Item("ma_kho").ToString()
                    Catch ex As Exception
                    End Try
                End If
            End If
            Try
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_kho")), "C") Then
                    .Item("ma_kho") = RuntimeHelpers.GetObjectValue(row.Item("ma_kho"))
                End If
                If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vi_tri")), "C") Then
                    .Item("ma_vi_tri") = RuntimeHelpers.GetObjectValue(row.Item("ma_vi_tri"))
                End If
                If row.Item("ma_thue") <> "" Then
                    .Item("ma_thue") = row.Item("ma_thue")
                    Dim rowthue As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "dmthue", ("ma_thue = '" & .Item("ma_thue") & "'")), DataRow)
                    .Item("thue_suat") = rowthue.Item("thue_suat")
                End If
                If IsDBNull(.Item("s1") Or .Item("s1") = "") Then
                    .Item("s1") = Me.txtMa_kh.Text
                End If
            Catch ex As Exception
            End Try
        End With
    End Sub

    Private Sub WhenLocationEnter(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_kho")), "C") Then
            Dim cKey As String = ("ma_kho = '" & Strings.Trim(StringType.FromObject(view.Item("ma_kho"))) & "'")
            Me.oLocation.Key = cKey
            Me.oLocation.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvitri", "ma_vi_tri", cKey))), "", False) = 0)
        End If
        view = Nothing
    End Sub

    Private Sub WhenLotEnter(ByVal sender As Object, ByVal e As EventArgs)
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            Dim cKey As String = ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'")
            cKey += (" AND ma_kho = '" & Strings.Trim(StringType.FromObject(view.Item("ma_kho"))) & "'")
            Me.oLot.Key = cKey
            Me.oLot.Empty = (StringType.StrCmp(Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "vcdlo13", "ma_lo", cKey))), "", False) = 0)
        End If
        view = Nothing
    End Sub

    Private Sub WhenSiteEnter(ByVal sender As Object, ByVal e As EventArgs)
        Me.cOldSite = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
    End Sub

    Private Sub WhenSiteLeave(ByVal sender As Object, ByVal e As EventArgs)
        If (Me.grdDetail.CurrentRowIndex >= 0) Then
            Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
            With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
                If Not ((StringType.StrCmp(Strings.Trim(str), Strings.Trim(Me.cOldSite), False) = 0) And Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ten_kho")), "C")) Then
                    If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmkho", "dai_ly_yn", ("ma_kho = '" & str & "'"))) Then
                        Dim str3 As String = Strings.Trim(StringType.FromObject(.Item("ma_vt")))
                        Dim str2 As String = Strings.Trim(StringType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "tk_dl", ("ma_vt = '" & str3 & "'"))))
                        If (StringType.StrCmp(str2, "", False) <> 0) Then
                            .Item("tk_vt") = str2
                        End If
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub WhenUOMEnter(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        Dim view As DataRowView = modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
        If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(view.Item("ma_vt")), "C") Then
            Return
        End If
        If BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "'"))) Then
            Dim str As String = ("(ma_vt = '" & Strings.Trim(StringType.FromObject(view.Item("ma_vt"))) & "' OR ma_vt = '*')")
            Me.oUOM.Key = str
            Me.oUOM.Empty = False
            Me.colDvt.ReadOnly = False
            Me.oUOM.Cancel = False
            Me.oUOM.Check = True
        Else
            Me.oUOM.Key = "1=1"
            Me.oUOM.Empty = True
            Me.colDvt.ReadOnly = True
            Me.oUOM.Cancel = True
            Me.oUOM.Check = False
        End If
    End Sub

    Private Sub WhenUOMLeave(ByVal sender As Object, ByVal e As EventArgs)
        On Error Resume Next
        With modVoucher.tblDetail.Item(Me.grdDetail.CurrentRowIndex)
            If clsfields.isEmpty(RuntimeHelpers.GetObjectValue(.Item("ma_vt")), "C") Then
                Return
            End If
            If Not BooleanType.FromObject(Sql.GetValue((modVoucher.appConn), "dmvt", "nhieu_dvt", ("ma_vt = '" & Strings.Trim(StringType.FromObject(.Item("ma_vt"))) & "'"))) Then
                Return
            End If
            Dim cKey As String = String.Concat(New String() {"(ma_vt = '", Strings.Trim(StringType.FromObject(.Item("ma_vt"))), "' OR ma_vt = '*') AND dvt = N'", Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing))), "'"})
            Dim num As Decimal = DecimalType.FromObject(Sql.GetValue((modVoucher.appConn), "dmqddvt", "he_so", cKey))
            .Item("He_so") = num
        End With
    End Sub
    Private Sub ValidObjects(ByVal sender As Object, ByVal e As EventArgs)
        If Not ((StringType.StrCmp(oVoucher.cAction, "New", False) = 0) Or (StringType.StrCmp(oVoucher.cAction, "Edit", False) = 0)) Then
            Return
        End If
        Dim currentRowIndex As Integer = Me.grdDetail.CurrentRowIndex
        If (Me.iOldRow <> currentRowIndex) Then
            Return
        End If
        Dim ds As New DataSet
        Dim num4 As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        Dim num3 As Byte
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num3 = num4
        Else
            num3 = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim str5 As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Text", New Object(0 - 1) {}, Nothing, Nothing)))
        Dim objArray3 As Object() = New Object(1 - 1) {}
        Dim o As Object = sender
        Dim args As Object() = New Object(0 - 1) {}
        Dim paramnames As String() = Nothing
        objArray3(0) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(LateBinding.LateGet(o, Nothing, "Name", args, paramnames, Nothing), Nothing, "Trim", New Object(0 - 1) {}, Nothing, Nothing))
        Dim objArray2 As Object() = objArray3
        Dim copyBack As Boolean() = New Boolean() {True}
        If copyBack(0) Then
            LateBinding.LateSetComplex(LateBinding.LateGet(o, Nothing, "Name", args, paramnames, Nothing), Nothing, "Trim", New Object() {RuntimeHelpers.GetObjectValue(objArray2(0))}, Nothing, True, True)
        End If
        Dim obj2 As Object = LateBinding.LateGet(Nothing, GetType(Strings), "UCase", objArray2, Nothing, copyBack)
        Dim sOldString As String
        If (ObjectType.ObjTst(obj2, "MA_VT", False) = 0) Then
            sOldString = Me.coldMa_vt
        ElseIf (ObjectType.ObjTst(obj2, "MA_KHO", False) = 0) Then
            sOldString = Me.coldMa_kho
        ElseIf (ObjectType.ObjTst(obj2, "DVT", False) = 0) Then
            sOldString = Me.coldDvt
        ElseIf (ObjectType.ObjTst(obj2, "SO_LUONG", False) = 0) Then
            sOldString = Strings.Replace(Me.coldSo_luong, " ", "", 1, -1, CompareMethod.Binary)
        Else
            Return
        End If

        If (StringType.StrCmp(Strings.Trim(str5), Strings.Trim(sOldString), False) = 0) Then
            Return
        End If
        Dim str As String = Strings.Trim(StringType.FromObject(LateBinding.LateGet(sender, Nothing, "Name", New Object(0 - 1) {}, Nothing, Nothing)))
        Dim row As DataRow = DirectCast(Sql.GetRow((modVoucher.appConn), "sysspdetailinfo", String.Concat(New String() {"xid = '", modVoucher.VoucherCode, "' AND xvalid = '", str, "'"})), DataRow)
        Dim str4 As String = StringType.FromObject(row.Item("xfields"))
        Dim str3 As String = StringType.FromObject(row.Item("xfcfields"))
        Dim cString As String = StringType.FromObject(row.Item("xreffields"))
        If (StringType.StrCmp(Strings.Trim(str4), "", False) = 0) Then
            Return
        End If
        Dim str8 As String = "EXEC fs_GetSOPrice "
        str8 = (str8 & "'" & Strings.Trim(str) & "'")
        str8 = (str8 & ", '" & Strings.Trim(modVoucher.VoucherCode) & "'")
        str8 = StringType.FromObject(ObjectType.AddObj(str8, ObjectType.AddObj(", ", Sql.ConvertVS2SQLType(Me.txtNgay_lct.Value, ""))))
        str8 = (str8 & ", ''")
        str8 = (str8 & ", '" & Strings.Trim(Me.cmdMa_nt.Text) & "'")
        str8 = (str8 & ", '" & Strings.Trim(Me.txtMa_kh.Text) & "'")
        Dim view2 As DataRowView = modVoucher.tblDetail.Item(currentRowIndex)
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_vt"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", '" & Strings.Trim(StringType.FromObject(view2.Item("ma_vt"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("ma_kho"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", '" & Strings.Trim(StringType.FromObject(view2.Item("ma_kho"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("dvt"))) Then
            str8 = (str8 & ", ''")
        Else
            str8 = (str8 & ", N'" & Strings.Trim(StringType.FromObject(view2.Item("dvt"))) & "'")
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("so_luong"))) Then
            str8 = (str8 & ", 0")
        Else
            str8 = (str8 & ", " & Strings.Trim(StringType.FromObject(view2.Item("so_luong"))))
        End If
        If Information.IsDBNull(RuntimeHelpers.GetObjectValue(view2.Item("he_so"))) Then
            str8 = (str8 & ", 1")
        Else
            str8 = (str8 & ", " & Strings.Trim(StringType.FromObject(view2.Item("he_so"))) & "")
        End If
        Sql.SQLRetrieve((modVoucher.appConn), str8, "xprice", (ds))
        If ds.Tables(0).Rows.Count = 0 Then
            Return
        End If
        If ds.Tables.Item(0).Rows.Item(0).Item("tl_ck") = 0 And tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("gia_nt2") <> 0 And obj2 <> "MA_VT" Then
            Return
        End If
        Dim num9 As Integer = IntegerType.FromObject(Fox.GetWordCount(str4, ","c))
        Dim nWordPosition As Integer = 1
        For nWordPosition = 1 To num9
            str = Strings.Trim(Fox.GetWordNum(str4, nWordPosition, ","c))
            If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))) Then
                modVoucher.tblDetail.Item(currentRowIndex).Item(str) = RuntimeHelpers.GetObjectValue(ds.Tables.Item(0).Rows.Item(0).Item(str))
            End If
        Next
        If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
            Dim num8 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
            For nWordPosition = 1 To num8
                str = Strings.Trim(Fox.GetWordNum(str3, nWordPosition, ","c))
                Dim str2 As String = Strings.Trim(Fox.GetWordNum(cString, nWordPosition, ","c))
                If Not Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item(str))) Then
                    tblDetail.Item(currentRowIndex).Item(str2) = RuntimeHelpers.GetObjectValue(LateBinding.LateGet(Nothing, GetType(Fox), "Round", New Object() {ObjectType.MulObj(tblDetail.Item(currentRowIndex).Item(str), Me.txtTy_gia.Value), IntegerType.FromObject(modVoucher.oVar.Item("m_round_gia"))}, Nothing, Nothing))
                End If
            Next
        End If
        With tblDetail.Item(currentRowIndex)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("gia_nt2"))) Then
                tblDetail.Item(currentRowIndex).Item("gia_nt2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("gia2"))) Then
                tblDetail.Item(currentRowIndex).Item("gia2") = 0
            End If
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("so_luong"))) Then
                tblDetail.Item(currentRowIndex).Item("so_luong") = 0
            End If
            tblDetail.Item(currentRowIndex).Item("tien_nt2") = Math.Round(tblDetail.Item(currentRowIndex).Item("gia_nt2") * tblDetail.Item(currentRowIndex).Item("so_luong"), num3)
            tblDetail.Item(Me.grdDetail.CurrentRowIndex).Item("tien2") = Math.Round(tblDetail.Item(currentRowIndex).Item("gia2") * tblDetail.Item(currentRowIndex).Item("so_luong"), num4)
            .Item("ck_nt") = Math.Round(.Item("tien_nt2") * .Item("tl_ck") / 100, num3)
            .Item("ck") = Math.Round(.Item("ck_nt") * Me.txtTy_gia.Value, num4)
            If .Item("so_luong") = 0 Then
                .Item("gia_ck_nt") = Round(.Item("gia_nt2") * (1 - .Item("tl_ck") / 100), 2)
            Else
                .Item("gia_ck_nt") = Math.Round((.Item("tien_nt2") - .Item("ck_nt")) / .Item("so_luong"), 2)
            End If
            .Item("gia_ck") = Math.Round(.Item("gia_ck_nt") * Me.txtTy_gia.Value, 2)
            If Information.IsDBNull(RuntimeHelpers.GetObjectValue(tblDetail.Item(currentRowIndex).Item("s2"))) Then
                tblDetail.Item(grdDetail.CurrentRowIndex).Item("s2") = Me.txtS1.Text
            End If
            If IsDBNull(.Item("gia_ban_nt0")) Or .Item("gia_ban_nt0") = 0 Or doi_gia_vat = False Then
                .Item("gia_ban_nt0") = .Item("gia_ck_nt")
                .Item("gia_ban0") = .Item("gia_ck")
                .Item("tien_ban_nt0") = .Item("tien_nt2") - .Item("ck_nt")
                .Item("tien_ban0") = .Item("tien2") - .Item("ck")
                Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
            End If
        End With
        Me.RecalcTax(Me.grdDetail.CurrentRowIndex, 2)
        ds = Nothing
        Me.UpdateList()
    End Sub
    Private Sub RecalcTax(ByVal iRow As Integer, ByVal nType As Integer)
        Dim num As Byte
        Dim decimals As Byte = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            num = decimals
        Else
            num = ByteType.FromObject(modVoucher.oVar.Item("m_round_tien_nt"))
        End If
        Dim m_round_gia_nt, m_round_gia As Byte
        m_round_gia = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia"))
        If (ObjectType.ObjTst(Strings.Trim(Me.cmdMa_nt.Text), modVoucher.oOption.Item("m_ma_nt0"), False) = 0) Then
            m_round_gia_nt = m_round_gia
        Else
            m_round_gia_nt = ByteType.FromObject(modVoucher.oVar.Item("m_round_gia_nt"))
        End If
        With modVoucher.tblDetail.Item(iRow)
            If IsDBNull(.Item("thue_suat")) Then
                .Item("thue_suat") = 0
            End If
            If IsDBNull(.Item("Tien_nt2")) Then
                .Item("Tien_nt2") = 0
            End If
            If IsDBNull(.Item("Tien2")) Then
                .Item("Tien2") = 0
            End If
            .Item("s4") = Round(.Item("gia_ck_nt") / (1 + .Item("thue_suat") / 100), m_round_gia_nt)
            .Item("s5") = Math.Round(.Item("s4") * Me.txtTy_gia.Value, m_round_gia)
            .Item("s6") = Round(.Item("so_luong") * .Item("s4"), decimals)
            .Item("thue_nt") = Round(.Item("s4") * .Item("so_luong") * .Item("thue_suat") / 100, num)
            .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtTy_gia.Value, decimals)
            'If CInt(oOption.Item("m_gia_sale")) = 0 Then
            '    .Item("thue_nt") = Math.Round((.Item("tien_nt2") - .Item("ck_nt")) * .Item("thue_suat") / 100, num)
            '    .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtTy_gia.Value, decimals)
            '    '.Item("tien_ban_nt0") = .Item("tien_nt2") - .Item("ck_nt")
            '    '.Item("tien_ban0") = .Item("tien2") - .Item("ck")
            'Else
            '    .Item("thue_nt") = Math.Round((.Item("tien_nt2") - .Item("ck_nt")) / (1 + .Item("thue_suat") / 100) * .Item("thue_suat") / 100, num)
            '    .Item("thue") = Math.Round(.Item("thue_nt") * Me.txtTy_gia.Value, decimals)
            '    '.Item("tien_ban_nt0") = .Item("tien_nt2") - .Item("ck_nt") - .Item("thue_nt")
            '    '.Item("tien_ban0") = .Item("tien2") - .Item("ck") - .Item("thue")
            'End If
            '.Item("gia_ban_nt0") = IIf(.Item("so_luong") = 0, .Item("tien_ban_nt0"), Math.Round(.Item("tien_ban_nt0") / .Item("so_luong"), m_round_gia_nt))
            '.Item("gia_ban0") = IIf(.Item("so_luong") = 0, .Item("tien_ban0"), Math.Round(.Item("tien_ban0") / .Item("so_luong"), m_round_gia))
        End With
    End Sub
    Private Sub InitSOPrice()
        Dim str As String
        Dim num As Integer
        Dim str3 As String = StringType.FromObject(Sql.GetValue((modVoucher.appConn), "sysspmasterinfo", "xread", ("xid = '" & modVoucher.VoucherCode & "'")))
        If (StringType.StrCmp(Strings.Trim(str3), "", False) <> 0) Then
            Dim num5 As Integer = IntegerType.FromObject(Fox.GetWordCount(str3, ","c))
            Dim num4 As Integer
            Dim i As Integer
            Dim str2 As String
            Dim flag As Boolean
            num = 1
            Do While (num <= num5)
                str = Strings.Trim(Fox.GetWordNum(str3, num, ","c))
                num4 = (Me.Controls.Count - 1)
                i = 0
                Do While (i <= num4)
                    str2 = Strings.Trim(Me.Controls.Item(i).Name)
                    flag = False
                    Try
                        Dim obj2 As Object = DirectCast(Me.Controls.Item(i), Label)
                    Catch exception1 As Exception
                        ProjectData.SetProjectError(exception1)
                        Dim exception As Exception = exception1
                        flag = True
                        ProjectData.ClearProjectError()
                    End Try
                    If ((StringType.StrCmp(Strings.Trim(str2), "", False) <> 0) AndAlso ((StringType.StrCmp(Strings.Right(str2, (Strings.Len(str2) - 3)).ToUpper, str.ToUpper, False) = 0) And flag)) Then
                        Dim box As TextBox = DirectCast(Me.Controls.Item(i), TextBox)
                        AddHandler box.Enter, New EventHandler(AddressOf Me.ReadOnlyObjects)
                    End If
                    i += 1
                Loop
                num += 1
            Loop
        End If
        Dim ds As New DataSet
        Dim tcSQL As String = ("SELECT * FROM sysspdetailinfo WHERE xid = '" & modVoucher.VoucherCode & "' ORDER BY xorder")
        Sql.SQLRetrieve((modVoucher.appConn), tcSQL, "sysspdetailinfo", (ds))
        Dim num3 As Integer = (ds.Tables.Item(0).Rows.Count - 1)
        num = 0
        Do While (num <= num3)
            str = Strings.Trim(StringType.FromObject(ds.Tables.Item(0).Rows.Item(num).Item("xvalid")))
            GetColumn(Me.grdDetail, str).TextBox.Name = GetColumn(Me.grdDetail, str).MappingName
            AddHandler GetColumn(Me.grdDetail, str).TextBox.Validated, New EventHandler(AddressOf Me.ValidObjects)
            'AddHandler GetColumn(Me.grdDetail, str).TextBox.Enter, New EventHandler(AddressOf Me.EnterObjects)
            num += 1
        Loop
        ds = Nothing
    End Sub

    Private Sub ReadOnlyObjects(ByVal sender As Object, ByVal e As EventArgs)
        Dim num2 As Integer = 0
        Dim num6 As Integer = (modVoucher.tblDetail.Count - 1)
        Dim num As Integer = 0
        For num = 0 To num6
            If Not clsfields.isEmpty(RuntimeHelpers.GetObjectValue(modVoucher.tblDetail.Item(num).Item("ma_vt")), "C") Then
                num2 = 1
            End If
        Next
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            LateBinding.LateSet(sender, Nothing, "ReadOnly", New Object() {(num2 > 0)}, Nothing)
        End If
    End Sub
    Private Sub txtTy_gia_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Enter
        oVoucher.noldFCrate = New Decimal(Me.txtTy_gia.Value)
    End Sub

    Private Sub txtTy_gia_Validated(ByVal sender As Object, ByVal e As EventArgs) Handles txtTy_gia.Validated
        Me.vFCRate()
    End Sub
    ' Properties
    Friend WithEvents cboAction As ComboBox
    Friend WithEvents cboStatus As ComboBox
    Friend WithEvents cmdBottom As Button
    Friend WithEvents cmdClose As Button
    Friend WithEvents cmdDelete As Button
    Friend WithEvents cmdEdit As Button
    Friend WithEvents cmdMa_nt As Button
    Friend WithEvents cmdNew As Button
    Friend WithEvents cmdNext As Button
    Friend WithEvents cmdOption As Button
    Friend WithEvents cmdPrev As Button
    Friend WithEvents cmdPrint As Button
    Friend WithEvents cmdSave As Button
    Friend WithEvents cmdSearch As Button
    Friend WithEvents cmdTop As Button
    Friend WithEvents cmdView As Button
    Friend WithEvents grdDetail As clsgrid
    Friend WithEvents Label1 As Label
    Friend WithEvents lblAction As Label
    Friend WithEvents lblMa_dvcs As Label
    Friend WithEvents lblMa_gd As Label
    Friend WithEvents lblMa_kh As Label
    Friend WithEvents lblNgay_ct As Label
    Friend WithEvents lblNgay_lct As Label
    Friend WithEvents lblNgay_lx0 As Label
    Friend WithEvents lblOng_ba As Label
    Friend WithEvents lblSo_ct As Label
    Friend WithEvents lblSo_lx0 As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblStatusMess As Label
    Friend WithEvents lblTen As Label
    Friend WithEvents lblTen_dvcs As Label
    Friend WithEvents lblTen_gd As Label
    Friend WithEvents lblTen_kh As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents lblTy_gia As Label
    Friend WithEvents tbDetail As TabControl
    Friend WithEvents tbgOther As TabPage
    Friend WithEvents tpgDetail As TabPage
    Friend WithEvents txtDien_giai As TextBox
    Friend WithEvents txtKeyPress As TextBox
    Friend WithEvents txtLoai_ct As TextBox
    Friend WithEvents txtMa_dvcs As TextBox
    Friend WithEvents txtMa_gd As TextBox
    Friend WithEvents txtMa_kh As TextBox
    Friend WithEvents txtNgay_ct As txtDate
    Friend WithEvents txtNgay_lct As txtDate
    Friend WithEvents txtNgay_lx0 As txtDate
    Friend WithEvents txtOng_ba As TextBox
    Friend WithEvents txtSo_ct As TextBox
    Friend WithEvents txtSo_lx0 As TextBox
    Friend WithEvents txtStatus As TextBox
    Friend WithEvents txtStt_rec_lx0 As TextBox
    Friend WithEvents txtT_so_luong As txtNumeric
    Friend WithEvents txtTy_gia As txtNumeric

    Public arrControlButtons(12) As Button
    Public cIDNumber As String
    Public cOldIDNumber As String
    Private cOldItem As String
    Private cOldSite As String
    Private colDvt As DataGridTextBoxColumn
    Private colMa_kho As DataGridTextBoxColumn
    Private colMa_lo As DataGridTextBoxColumn
    Private colMa_vi_tri As DataGridTextBoxColumn
    Private colMa_vt As DataGridTextBoxColumn
    Private colSl_giao As DataGridTextBoxColumn
    Private colSl_hd As DataGridTextBoxColumn
    Private colSl_xuat As DataGridTextBoxColumn
    Private colSo_dh As DataGridTextBoxColumn
    Private colSo_hd As DataGridTextBoxColumn
    Private colSo_line As DataGridTextBoxColumn
    Private colSo_luong As DataGridTextBoxColumn
    Private colSv_line As DataGridTextBoxColumn
    Private colTen_vt As DataGridTextBoxColumn
    Private colGia_nt2, colTien_nt2, colCk_nt, colGia_ck_nt, colGia_ck, colGia_ban_nt, colGia2, colTien2, colTl_ck, colCk, colGia_ban As DataGridTextBoxColumn
    Private colMa_thue, colThue_suat, colThue, colThue_nt, colGia_ban0, colGia_ban_nt0, colTien_ban0, colTien_ban_nt0 As DataGridTextBoxColumn
    Private components As IContainer
    Private grdHeader As grdHeader
    Public iDetailRow As Integer
    Public iMasterRow As Integer
    Public iOldMasterRow As Integer
    Private iOldRow As Integer
    Private isActive As Boolean
    Private lAllowCurrentCellChanged As Boolean
    Private nColumnControl As Integer
    Private noldSo_luong, noldGia_nt2, noldGia2, noldTien_nt2, noldTien2, noldTl_ck, noldCk_nt, noldCk, noldGia_ban_nt0 As Decimal

    Private Sub txtS1_TextChanged(sender As Object, e As EventArgs)
        If Fox.InList(oVoucher.cAction, New Object() {"New", "Edit"}) Then
            Dim i As Integer
            For i = 0 To tblDetail.Count - 1
                Try
                    tblDetail.Item(i).Item("s2") = Me.txtS1.Text.Trim
                Catch ex As Exception
                End Try
            Next
        End If
    End Sub

    Private oInvItemDetail As VoucherLibObj
    Private oldtblDetail As DataTable
    Private oLocation As VoucherKeyLibObj
    Private oLot As VoucherKeyLibObj
    Private oSecurity As clssecurity
    Private oSite As VoucherKeyLibObj
    'Private oTitleButton As TitleButton
    Private oUOM As VoucherKeyCheckLibObj
    Public oVoucher As clsvoucher.clsVoucher
    Public pnContent As StatusBarPanel
    Private tblHandling As DataTable
    Private tblRetrieveDetail As DataView
    Private tblRetrieveMaster As DataView
    Private tblStatus As DataTable
    Private xInventory As clsInventory
    Private coldDvt As String
    Private coldMa_kho As String
    Private coldMa_vt, coldMa_thue As String
    Private coldSo_luong As String
    Private colS1 As DataGridTextBoxColumn
    Private doi_gia_vat As Boolean = False
End Class

