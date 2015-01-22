Imports Tesseract
Imports System
Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Imports System.Drawing.Text

Public Class Options

    Dim dbe As New SQLiteDBEngine(My.Settings.SQLiteConn)
    Dim pfc As New PrivateFontCollection()
    Dim euro10 As Font
    Dim euro9 As Font
    Dim euro8 As Font

    Private Sub Options_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'initialise general settings
        Me.FolderBrowserDialog1.SelectedPath = Application.StartupPath

        'General Tab
        If My.Settings.OP_GEN_DBFile = "" Then Me.TextBox_GEN_Filepath.Text = Application.StartupPath + "\EDData" Else Me.TextBox_GEN_Filepath.Text = My.Settings.OP_GEN_DBFile
        Me.TextBox_GEN_ProcessName.Text = My.Settings.OP_GEN_EDProcess


        'OCR Tab
        If My.Settings.OP_OCR_tessdata_folder = "" Then Me.TextBox_OCR_Tessdata.Text = Application.StartupPath + "\tessdata" Else Me.TextBox_OCR_Tessdata.Text = My.Settings.OP_OCR_tessdata_folder
        If My.Settings.OP_OCR_temp_folder = "" Then Me.TextBox_OCR_TempFolder.Text = SpecialDirectories.CurrentUserApplicationData Else Me.TextBox_OCR_TempFolder.Text = My.Settings.OP_OCR_temp_folder
        Me.TextBox_OCR_Confidence.Text = My.Settings.OP_OCR_Confidence
        Me.TextBox_OCR_Contrast.Text = My.Settings.OP_OCR_Contrast
        Me.TextBox_OCR_Gamma.Text = My.Settings.OP_OCR_Gamma
        Me.TextBox_OCR_Lang.Text = My.Settings.OP_OCR_Language
        Me.NUD_OCR_Scaling.Value = My.Settings.OP_OCR_ScalingFactor
        Me.CheckBox_OCR_DelScrsht.Checked = My.Settings.OP_OCR_DeleteScreenshot
        Me.TextBox_OCR_Grayscale.Text = My.Settings.OP_OCR_Grayscale_Type.ToString
        Me.TextBox_OCR_BlackTH.Text = My.Settings.OP_OCR_Black_Threshold.ToString
        Me.TextBox_OCR_PV.Text = My.Settings.OP_OCR_PVariance

        'Visibility Tab
        Me.ListView_VIS_Commodities.Items.Clear()
        Me.ListView_VIS_Stations.Items.Clear()
        Me.ListView_VIS_Systems.Items.Clear()


        ''*** LOAD CUSTOM FONT ***
        pfc.AddFontFile(Application.StartupPath + "\Eurostile.ttf")
        euro10 = New Font(pfc.Families(0), 10, FontStyle.Regular)
        euro9 = New Font(pfc.Families(0), 9, FontStyle.Regular)
        euro8 = New Font(pfc.Families(0), 9, FontStyle.Regular)
        '*** APPLY CUSTOM FONT TO CONTROLS ***
        Me.Font = euro10
        Me.Button_SaveSettings.Font = euro10

        Me.TabControl_Options.Font = euro10
        '** GENERAL **
        Me.TabPage_General.Font = euro10
        Me.Button_GEN_ResetGeneralSettings.Font = euro10
        Me.Button_GEN_SelectDB.Font = euro10
        Me.Label_GEN_19.Font = euro10
        Me.Label_GEN_5.Font = euro10
        Me.Label_GEN_9.Font = euro10
        Me.TextBox_GEN_Filepath.Font = euro10
        Me.TextBox_GEN_ProcessName.Font = euro10

        '** OCR **
        Me.TabPage_OCR.Font = euro10
        Me.Button_OCR_ResetOCRSettings.Font = euro10
        Me.Button_OCR_TempFolder.Font = euro10
        Me.Button_OCR_TessdataFolder.Font = euro10
        Me.CheckBox_OCR_DelScrsht.Font = euro10
        Me.Label_OCR_1.Font = euro10
        Me.Label_OCR_10.Font = euro10
        Me.Label_OCR_11.Font = euro10
        Me.Label_OCR_12.Font = euro10
        Me.Label_OCR_13.Font = euro10
        Me.Label_OCR_14.Font = euro10
        Me.Label_OCR_15.Font = euro10
        Me.Label_OCR_18.Font = euro10
        Me.Label_OCR_2.Font = euro10
        Me.Label_OCR_20.Font = euro10
        Me.Label_OCR_21.Font = euro10
        Me.Label_OCR_23.Font = euro10
        Me.Label_OCR_3.Font = euro10
        Me.Label_OCR_4.Font = euro10
        Me.Label_OCR_6.Font = euro10
        Me.Label_OCR_7.Font = euro10
        Me.Label_OCR_8.Font = euro10
        Me.NUD_OCR_Scaling.Font = euro10
        Me.TextBox_OCR_TempFolder.Font = euro10
        Me.TextBox_OCR_BlackTH.Font = euro10
        Me.TextBox_OCR_Confidence.Font = euro10
        Me.TextBox_OCR_Contrast.Font = euro10
        Me.TextBox_OCR_Gamma.Font = euro10
        Me.TextBox_OCR_Grayscale.Font = euro10
        Me.TextBox_OCR_Lang.Font = euro10
        Me.TextBox_OCR_Tessdata.Font = euro10

        '** VISIBILITY **
        Me.TabPage_Visibility.Font = euro10
        Me.TabControl_Visibility.Font = euro10
        Me.TabPage_VIS_Commodities.Font = euro10
        Me.ListView_VIS_Commodities.Font = euro10
        Me.TabPage_VIS_Stations.Font = euro10
        Me.ListView_VIS_Stations.Font = euro10
        Me.TabPage_VIS_Systems.Font = euro10
        Me.ListView_VIS_Systems.Font = euro10



    End Sub

    Private Sub Button_SaveSettings_Click(sender As System.Object, e As System.EventArgs) Handles Button_SaveSettings.Click
        Dim txt As String = ""

        If File.Exists(Trim(Me.TextBox_GEN_Filepath.Text)) = False Then txt = txt + "Database Location : File not found! Location not saved" + Chr(13)
        If Directory.Exists(Trim(Me.TextBox_OCR_Tessdata.Text)) = False Then txt = txt + "Tessdata Folder : Folder not found! Location not saved" + Chr(13)
        If Directory.Exists(Trim(Me.TextBox_OCR_TempFolder.Text)) = False Then txt = txt + "Temp Folder : Folder not found! Location not saved" + Chr(13)


        If txt.Length = 0 Then
            My.Settings.OP_GEN_DBFile = Trim(Me.TextBox_GEN_Filepath.Text)
            My.Settings.OP_OCR_tessdata_folder = Trim(Me.TextBox_OCR_Tessdata.Text)
            My.Settings.OP_OCR_temp_folder = Trim(Me.TextBox_OCR_TempFolder.Text)
            My.Settings.OP_GEN_EDProcess = Trim(Me.TextBox_GEN_ProcessName.Text)
            My.Settings.OP_OCR_Confidence = Trim(Me.TextBox_OCR_Confidence.Text)
            My.Settings.OP_OCR_Contrast = Trim(Me.TextBox_OCR_Contrast.Text)
            My.Settings.OP_OCR_Gamma = Trim(Me.TextBox_OCR_Gamma.Text)
            My.Settings.OP_OCR_Language = Trim(Me.TextBox_OCR_Lang.Text)
            My.Settings.OP_OCR_ScalingFactor = Me.NUD_OCR_Scaling.Value
            My.Settings.OP_OCR_DeleteScreenshot = Me.CheckBox_OCR_DelScrsht.Checked
            My.Settings.OP_OCR_Grayscale_Type = CInt(Me.TextBox_OCR_Grayscale.Text)
            My.Settings.OP_OCR_Black_Threshold = CInt(Me.TextBox_OCR_BlackTH.Text)
            My.Settings.OP_OCR_PVariance = Me.TextBox_OCR_PV.Text
            MainForm.Label_OCR_LegendPV.Text = ">" + My.Settings.OP_OCR_PVariance + "% Price Variance"
        Else
            MsgBox(txt)
        End If



    End Sub

    Private Sub Button_ResetGeneralSettings_Click(sender As System.Object, e As System.EventArgs) Handles Button_GEN_ResetGeneralSettings.Click
        If MsgBox("Are you sure you wish to reset all fields on this tab to default values?", MsgBoxStyle.OkCancel, "EDCom") = MsgBoxResult.Ok Then
            Me.TextBox_GEN_Filepath.Text = Application.StartupPath + "\Data\EDData"
            Me.TextBox_GEN_ProcessName.Text = "EliteDangerous32"
        End If
    End Sub

    Private Sub Button_ResetOCRSettings_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_ResetOCRSettings.Click
        If MsgBox("Are you sure you wish to reset all fields on this tab to default values?", MsgBoxStyle.OkCancel, "EDCom") = MsgBoxResult.Ok Then
            Me.TextBox_OCR_Tessdata.Text = Application.StartupPath + "\Data\OCR\tessdata"
            Me.TextBox_OCR_TempFolder.Text = Application.StartupPath + "\Data\OCR\temp"
            Me.TextBox_OCR_Confidence.Text = "67"
            Me.TextBox_OCR_Contrast.Text = "31"
            Me.TextBox_OCR_Gamma.Text = "0.98"
            Me.TextBox_OCR_Lang.Text = "big"
            Me.NUD_OCR_Scaling.Value = 4
            Me.CheckBox_OCR_DelScrsht.Checked = False
            Me.TextBox_OCR_BlackTH.Text = "65"
            Me.TextBox_OCR_Grayscale.Text = "0"
            Me.TextBox_OCR_PV.Text = "20"
        End If
    End Sub

    Private Sub Button_SelectDB_Click(sender As System.Object, e As System.EventArgs) Handles Button_GEN_SelectDB.Click
        If OpenFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Me.TextBox_GEN_Filepath.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub Button_TessdataFolder_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_TessdataFolder.Click
        If FolderBrowserDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Me.TextBox_OCR_Tessdata.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button_TempFolder_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_TempFolder.Click
        If FolderBrowserDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            Me.TextBox_OCR_TempFolder.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub


    Private Sub ListView_Systems_ItemCheck(sender As Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles ListView_VIS_Systems.ItemCheck
        Dim checked As String = "0"
        If e.NewValue = CheckState.Checked Then checked = "1"
        Dim sql As String = "update Starsystems set system_visible = " + checked + " where system_name = '" + Me.ListView_VIS_Systems.Items(e.Index).Text.Trim + "'"
        dbe.SQL_String_Execute(sql)
    End Sub
    Private Sub ListView_Stations_ItemCheck(sender As Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles ListView_VIS_Stations.ItemCheck
        Dim checked As String = "0"
        If e.NewValue = CheckState.Checked Then checked = "1"
        Dim sql As String = "update Stations set station_visible = " + checked + " where station_name = '" + Me.ListView_VIS_Stations.Items(e.Index).Text.Trim + "'"
        dbe.SQL_String_Execute(sql)
    End Sub
    Private Sub ListView_Commodities_ItemCheck(sender As Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles ListView_VIS_Commodities.ItemCheck
        Dim checked As String = "0"
        If e.NewValue = CheckState.Checked Then checked = "1"
        Dim sql As String = "update Items set item_visible = " + checked + " where item_name = '" + Me.ListView_VIS_Commodities.Items(e.Index).Text.Trim + "'"
        dbe.SQL_String_Execute(sql)
    End Sub



    Private Sub TabPage_Systems_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles TabPage_VIS_Systems.Paint
        RemoveHandler ListView_VIS_Systems.ItemCheck, AddressOf ListView_Systems_ItemCheck

        'Sub-Tab : Systems
        Me.ListView_VIS_Systems.Items.Clear()
        Dim sql As String = "select system_id,system_name,system_visible from Starsystems order by system_name"
        Dim dt As DataTable = dbe.SQLStringtoDatatable(sql)
        For Each itm In dt.Rows
            Dim lvi As ListViewItem = New ListViewItem(itm.Item(1).ToString)
            If itm.Item(2) = 1 Then lvi.Checked = True
            Me.ListView_VIS_Systems.Items.Add(lvi)
        Next

        AddHandler ListView_VIS_Systems.ItemCheck, AddressOf ListView_Systems_ItemCheck
    End Sub
    Private Sub TabPage_Stations_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles TabPage_VIS_Stations.Paint
        RemoveHandler ListView_VIS_Stations.ItemCheck, AddressOf ListView_Stations_ItemCheck

        'Sub-Tab : Stations
        Me.ListView_VIS_Stations.Items.Clear()
        Dim sql As String = "select station_id,station_name,station_visible from Stations order by station_name"
        Dim dt2 As DataTable = dbe.SQLStringtoDatatable(sql)
        For Each itm As DataRow In dt2.Rows
            Dim lvi As ListViewItem = New ListViewItem(itm.Item(1).ToString)
            If itm.Item(2) = 1 Then lvi.Checked = True
            Me.ListView_VIS_Stations.Items.Add(lvi)
        Next

        AddHandler ListView_VIS_Stations.ItemCheck, AddressOf ListView_Stations_ItemCheck

    End Sub
    Private Sub TabPage_Commodities_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles TabPage_VIS_Commodities.Paint
        RemoveHandler ListView_VIS_Commodities.ItemCheck, AddressOf ListView_Commodities_ItemCheck

        'Sub-Tab : Commodities
        Me.ListView_VIS_Commodities.Items.Clear()
        Dim sql As String = "select item_id,item_name,item_visible from Items order by item_name"
        Dim dt3 As DataTable = dbe.SQLStringtoDatatable(sql)
        For Each itm In dt3.Rows
            Dim lvi As ListViewItem = New ListViewItem(itm.Item(1).ToString)
            If itm.Item(2) = 1 Then lvi.Checked = True
            Me.ListView_VIS_Commodities.Items.Add(lvi)
        Next

        AddHandler ListView_VIS_Commodities.ItemCheck, AddressOf ListView_Commodities_ItemCheck

    End Sub

    Private Sub General_Enter(sender As Object, e As System.EventArgs) Handles TabPage_General.Enter, TabPage_OCR.Enter
        Me.Button_SaveSettings.Visible = True
    End Sub
    Private Sub Visibility_Enter(sender As Object, e As System.EventArgs) Handles TabPage_Visibility.Enter
        Me.Button_SaveSettings.Visible = False
    End Sub
End Class