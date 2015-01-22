Imports System.Data.SQLite
Imports System.Text
Imports System.Math
Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.FileIO
Imports Tesseract
Imports ImageMagick
Imports System.Drawing.Text


Public Class MainForm
    Dim dbe As SQLiteDBEngine
    Dim dt_sys As New Data.DataTable
    Dim dt_stn As New Data.DataTable
    Dim dt_commPrices As New Data.DataTable
    Public Shared hdr_flag As Boolean
    Dim test_setting As Integer

    Dim filePath_from As String
    Dim filePath_to As String
    Dim fileName As String
    Dim drSurface As New DrawingSurface
    Dim ac As New AutoCompleteStringCollection
    Dim pfc As New PrivateFontCollection()
    Dim euro10 As Font
    Dim euro9 As Font
    Dim euro9Bold As Font
    Dim euro8 As Font

    '*** LOAD APPLICATION ***

    Private Sub MainForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        ' *** CHECK FOR NEW VERSION AND IMPORT EXISTING USER SETTINGS ***
        Upgrade()

        ' *** SET RUNTIME VARIABLES ***
        If My.Settings.OP_GEN_DBFile = "" Then My.Settings.OP_GEN_DBFile = Application.StartupPath + "\EDData"
        My.Settings.SQLiteConn = "Data Source=" + My.Settings.OP_GEN_DBFile + ";Version=3;New=False;Compress=True"
        If My.Settings.OP_OCR_tessdata_folder = "" Then My.Settings.OP_OCR_tessdata_folder = Application.StartupPath + "\tessdata"
        My.Settings.OP_OCR_temp_folder = SpecialDirectories.CurrentUserApplicationData
        My.Settings.OP_OCR_ScreenshotDirectory = SpecialDirectories.MyPictures + "\Frontier Developments\Elite Dangerous"



        ' *** LOAD EDDATA SQLite Database ***
        LoadDB()

        ''*** LOAD CUSTOM FONT ***
        pfc.AddFontFile(Application.StartupPath + "\Eurostile.ttf")
        euro10 = New Font(pfc.Families(0), 10, FontStyle.Regular)
        euro9 = New Font(pfc.Families(0), 9, FontStyle.Regular)
        euro9Bold = New Font(pfc.Families(0), 9, FontStyle.Bold)
        euro8 = New Font(pfc.Families(0), 9, FontStyle.Regular)

        '*** APPLY CUSTOM FONT TO CONTROLS ***
        Me.Font = euro9
        Me.ListBox_OCR_FileList.Font = euro9

        '**************************************
        '*** INITIAL DATA LOAD FOR ALL TABS ***
        '**************************************
        '*** OCR TAB SETUP ***
        Me.Timer_OCR_FileList.Start()

        'Set Price Variance Legend label to match stored value
        Me.Label_OCR_LegendPV.Text = ">" + My.Settings.OP_OCR_PVariance + "% Price Variance"
        'Set AutoComplete list for OCR Detected Station
        Dim sql As String = "select station_name from stations order by station_name"
        Dim dt As DataTable = dbe.SQLStringtoDatatable(sql)
        Me.TextBox_OCR_DetectedStationName.AutoCompleteCustomSource.Clear()
        If dt.Rows.Count > 0 Then
            For Each itm As DataRow In dt.Rows
                Me.TextBox_OCR_DetectedStationName.AutoCompleteCustomSource.Add(itm.Item(0).ToString)
            Next
        End If
        'Set AutoComplete list for item names in OCR
        sql = "select item_name from Items where item_disabled =0 order by item_name "
        dt = dbe.SQLStringtoDatatable(sql)
        For Each itm As DataRow In dt.Rows
            ac.Add(itm.Item(0).ToString)
        Next


        '*** SYSTEM/STATION TAB SETUP ***
        Refresh_SYSADD_Stations()

        '*** BROWSE STATION TAB SETUP ***
        BS_refresh_systems("")
        BS_refresh_stations("")
        AddHandler TextBox_BS_StarsystemFilter.TextChanged, AddressOf TextBox_BS_StarsystemFilter_TextChanged
        AddHandler TextBox_BS_StationFilter.TextChanged, AddressOf TextBox_BS_StationFilter_TextChanged


        '*** BROWSE COMMODITY TAB SETUP ***
        refresh_BC_Item_List()

        '*** FROM/TO TAB SETUP ***
        FT_Load_SystemStationCombobox(vbTrue)
        'Custom Drawing Surface
        drSurface.Dock = DockStyle.Fill
        drSurface.BackColor = Color.Transparent
        Me.TabPage_Trade_From_To.Controls.Add(drSurface)
        drSurface.SendToBack()

        '*** MANAGE COMMODITIES TAB SETUP ***
        Refresh_MC_ItemList()


    End Sub

    Sub LoadDB()
        Try

            dbe = New SQLiteDBEngine(My.Settings.SQLiteConn)

            Me.TabControl_Function.Enabled = True


        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Sub Upgrade()
        If My.Settings.GEN_UPGRADE_FLAG = 0 Then
            My.Settings.OP_GEN_DBFile = My.Settings.GetPreviousVersion("OP_GEN_DBFile")
            My.Settings.OP_GEN_EDProcess = My.Settings.GetPreviousVersion("OP_GEN_EDProcess")
            My.Settings.OP_OCR_Black_Threshold = My.Settings.GetPreviousVersion("OP_OCR_Black_Threshold")
            My.Settings.OP_OCR_Confidence = My.Settings.GetPreviousVersion("OP_OCR_Confidence")
            My.Settings.OP_OCR_Contrast = My.Settings.GetPreviousVersion("OP_OCR_Contrast")
            My.Settings.OP_OCR_DeleteScreenshot = My.Settings.GetPreviousVersion("OP_OCR_DeleteScreenshot")
            My.Settings.OP_OCR_Gamma = My.Settings.GetPreviousVersion("OP_OCR_Gamma")
            My.Settings.OP_OCR_Grayscale_Type = My.Settings.GetPreviousVersion("OP_OCR_Grayscale_Type")
            My.Settings.OP_OCR_Language = My.Settings.GetPreviousVersion("OP_OCR_Language")
            My.Settings.OP_OCR_PVariance = My.Settings.GetPreviousVersion("OP_OCR_PVariance")
            My.Settings.OP_OCR_ScalingFactor = My.Settings.GetPreviousVersion("OP_OCR_ScalingFactor")
            My.Settings.OP_OCR_ScreenshotDirectory = My.Settings.GetPreviousVersion("OP_OCR_ScreenshotDirectory")
            My.Settings.OP_OCR_temp_folder = My.Settings.GetPreviousVersion("OP_OCR_temp_folder")
            My.Settings.OP_OCR_tessdata_folder = My.Settings.GetPreviousVersion("OP_OCR_tessdata_folder")

            My.Settings.GEN_UPGRADE_FLAG = 1
        End If
    End Sub

    Private Sub Button_Exit_Click(sender As System.Object, e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub

    '*** CONTROL WINDOW SIZE ***
    Private Sub Tab_OCR_Enter(sender As Object, e As System.EventArgs) Handles Tab_OCR.Enter
        Me.MinimumSize = My.Settings.WIN_OCR_DEF_SIZE
        Me.Size = My.Settings.WIN_OCR_USR_SIZE
    End Sub
    Private Sub Tab_System_Station_Enter(sender As Object, e As System.EventArgs) Handles Tab_System_Station.Enter
        Me.MinimumSize = My.Settings.WIN_SYSADD_DEF_SIZE
        Me.Size = My.Settings.WIN_SYSADD_USR_SIZE
    End Sub
    Private Sub Tab_Commodities_Enter(sender As Object, e As System.EventArgs) Handles Tab_Commodities.Enter
        If Me.TabControl_Commodities.SelectedTab Is Me.TabPage_BrowseStation Then Me.Size = My.Settings.WIN_BS_USR_SIZE
        If Me.TabControl_Commodities.SelectedTab Is Me.TabPage_BrowseCommodity Then Me.Size = My.Settings.WIN_BC_USR_SIZE
        If Me.TabControl_Commodities.SelectedTab Is Me.TabPage_Trade_From_To Then Me.Size = My.Settings.WIN_FT_USR_SIZE
        If Me.TabControl_Commodities.SelectedTab Is Me.TabPage_ManageCommodities Then Me.Size = My.Settings.WIN_MC_USR_SIZE
    End Sub
    Private Sub TabPage_BrowseStation_Enter(sender As Object, e As System.EventArgs) Handles TabPage_BrowseStation.Enter
        Me.MinimumSize = My.Settings.WIN_BS_DEF_SIZE
        Me.Size = My.Settings.WIN_BS_USR_SIZE
    End Sub
    Private Sub TabPage_BrowseCommodity_Enter(sender As Object, e As System.EventArgs) Handles TabPage_BrowseCommodity.Enter
        Me.MinimumSize = My.Settings.WIN_BC_DEF_SIZE
        Me.Size = My.Settings.WIN_BC_USR_SIZE
    End Sub
    Private Sub TabPage_Trade_From_To_Enter(sender As Object, e As System.EventArgs) Handles TabPage_Trade_From_To.Enter
        drSurface.Refresh()
        Me.MinimumSize = My.Settings.WIN_FT_DEF_SIZE
        Me.Size = My.Settings.WIN_FT_USR_SIZE
    End Sub
    Private Sub TabPage_ManageCommodities_Enter(sender As Object, e As System.EventArgs) Handles TabPage_ManageCommodities.Enter
        Me.MinimumSize = My.Settings.WIN_MC_DEF_SIZE
        Me.Size = My.Settings.WIN_MC_USR_SIZE
    End Sub

    Private Sub Tab_OCR_Leave(sender As Object, e As System.EventArgs) Handles Tab_OCR.Leave
        My.Settings.WIN_OCR_USR_SIZE = Me.Size
    End Sub
    Private Sub Tab_System_Station_Leave(sender As Object, e As System.EventArgs) Handles Tab_System_Station.Leave
        My.Settings.WIN_SYSADD_USR_SIZE = Me.Size
    End Sub
    Private Sub TabPage_BrowseStation_Leave(sender As Object, e As System.EventArgs) Handles TabPage_BrowseStation.Leave
        My.Settings.WIN_BS_USR_SIZE = Me.Size
    End Sub
    Private Sub TabPage_BrowseCommodity_Leave(sender As Object, e As System.EventArgs) Handles TabPage_BrowseCommodity.Leave
        My.Settings.WIN_BC_USR_SIZE = Me.Size
    End Sub
    Private Sub TabPage_Trade_From_To_Leave(sender As Object, e As System.EventArgs) Handles TabPage_Trade_From_To.Leave
        My.Settings.WIN_FT_USR_SIZE = Me.Size
    End Sub
    Private Sub TabPage_ManageCommodities_Leave(sender As Object, e As System.EventArgs) Handles TabPage_ManageCommodities.Leave
        My.Settings.WIN_MC_USR_SIZE = Me.Size
    End Sub

    Private Sub TabPage_BrowseCommodity_Resize(sender As Object, e As System.EventArgs) Handles TabPage_BrowseCommodity.Resize

        Dim r2 As Integer = Me.TabPage_BrowseCommodity.Right - 6
        Dim l1 As Integer = Me.DataGridView_BC_Demand.Left
        Dim w As Integer = (r2 - l1 - 6) / 2
        Dim l2 As Integer = l1 + w + 6

        Me.DataGridView_BC_Demand.Width = w
        Me.DataGridView_BC_Supply.Left = l2
        Me.DataGridView_BC_Supply.Width = w
        Me.Label_BC_Supply.Left = l2
    End Sub


    '*** TOOLBAR CONTROLS ***

    Private Sub OptionsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        Options.Show()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        About.ShowDialog()
    End Sub


    '**********************
    '****** OCR TAB *******
    '**********************

    'Grab Screen Data
    Private Sub Button_Capture_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_Capture.Click
        Me.Cursor = Cursors.WaitCursor

        OCR_capture_screen()

        Me.Cursor = Cursors.Default
        My.Computer.Audio.PlaySystemSound(System.Media.SystemSounds.Asterisk)
    End Sub

    Dim OCR_fn_selected As String = ""
    'Get avaiable list of screenshots
    Private Sub Button_OCR_StartTimer_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_StartTimer.Click
        Me.Button_OCR_StartTimer.Visible = False
        Me.Button_OCR_ProcessSelectedImage.Visible = True
        Me.Timer_OCR_FileList.Start()
    End Sub
    Private Sub Timer_OCR_FileList_Tick(sender As Object, e As System.EventArgs) Handles Timer_OCR_FileList.Tick
        Refresh_OCR_file_list()
    End Sub
    Sub Refresh_OCR_file_list()

        If Directory.Exists(My.Settings.OP_OCR_ScreenshotDirectory) Then
            Dim fl As New List(Of String)(Directory.EnumerateFiles(My.Settings.OP_OCR_ScreenshotDirectory, "*.bmp", IO.SearchOption.TopDirectoryOnly))

            If Me.ListBox_OCR_FileList.Items.Count > 0 Then
                If Me.ListBox_OCR_FileList.SelectedItems.Count > 0 Then
                    OCR_fn_selected = Me.ListBox_OCR_FileList.SelectedItem
                Else
                    OCR_fn_selected = ""
                End If

                Me.ListBox_OCR_FileList.Items.Clear()
            End If

            For Each itm In fl
                Me.ListBox_OCR_FileList.Items.Add(itm.Substring(itm.LastIndexOf("\") + 1))
            Next


            If OCR_fn_selected <> "" And File.Exists(My.Settings.OP_OCR_ScreenshotDirectory + "\" + OCR_fn_selected) Then
                Me.ListBox_OCR_FileList.SelectedItem = OCR_fn_selected
            Else
                Me.ListBox_OCR_FileList.SelectedIndex = -1
            End If


        Else
            Me.Timer_OCR_FileList.Stop()
            Me.Button_OCR_ProcessSelectedImage.Visible = False
            Me.Button_OCR_StartTimer.Visible = True

            If MsgBox("Screenshot folder not found!  Would you like to specify a location now?", MsgBoxStyle.YesNo, "Temp Directory Error") = MsgBoxResult.Yes Then
                If Options.Visible = False Then
                    Options.Show()
                    Options.TabControl_Options.SelectTab(Options.TabPage_OCR)
                    Options.Button_OCR_TempFolder.PerformClick()
                Else
                    Options.Dispose()
                    Options.Show()
                    Options.TabControl_Options.SelectTab(Options.TabPage_OCR)
                    Options.Button_OCR_TempFolder.PerformClick()
                End If
            Else
                MsgBox("You must set a screenshots folder to enable data input.  To do this go to :" + Chr(13) + Chr(13) + """Tools > Options... > OCR"" and choose an ""Images Folder"" that contains your market screenshots!")
            End If



        End If



    End Sub


    ' Process selected file
    Private Sub Button_OCR_ProcessSelectedImage_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_ProcessSelectedImage.Click
        Try
            RemoveHandler DataGridView_OCR_CommPrice.CellValueChanged, AddressOf DataGridView_OCR_CommPrice_CellValueChanged

            Me.Cursor = Cursors.WaitCursor
            Me.Timer_OCR_FileList.Stop()

            'Set AutoComplete list for station dropdown
            Dim sql As String = "select station_name from stations order by station_name"
            Dim dt As DataTable = dbe.SQLStringtoDatatable(sql)
            Me.TextBox_OCR_DetectedStationName.AutoCompleteCustomSource.Clear()
            If dt.Rows.Count > 0 Then
                For Each itm As DataRow In dt.Rows
                    Me.TextBox_OCR_DetectedStationName.AutoCompleteCustomSource.Add(itm.Item(0).ToString)
                Next
            End If


            Dim fn As String = Trim(My.Settings.OP_OCR_ScreenshotDirectory + "\" + Me.ListBox_OCR_FileList.SelectedItem)
            If File.Exists(fn) And fn.Substring(Len(fn) - 3) = "bmp" Then
                'process file
                test_setting = My.Settings.offline_testing
                My.Settings.offline_testing = 2
                OCR_capture_screen()
                My.Settings.offline_testing = test_setting

                'post-processing
                Dim varPercentHigh As Double = (100 + CInt(My.Settings.OP_OCR_PVariance)) / 100
                Dim varPercentLow As Double = (100 - CInt(My.Settings.OP_OCR_PVariance)) / 100
                For Each itm As DataGridViewRow In Me.DataGridView_OCR_CommPrice.Rows
                    If Not IsNothing(itm.Cells(0).Value) Then
                        'OCR post processing - process each item name and match to Items table.  If not exists highlight red
                        sql = "select item_id from Items where item_name = '" + Trim(itm.Cells(0).Value.ToString) + "'"
                        Dim dt2 As DataTable = dbe.SQLStringtoDatatable(sql)
                        Dim itm_id As String = ""
                        If dt2.Rows.Count = 0 Then
                            itm.Cells(0).Style.BackColor = Color.Red
                        Else
                            itm_id = dt2.Rows(0).Item(0).ToString
                        End If

                        'OCR post processing - process each row for a supply/demand rating and ensure a supply/demand figure is present
                        If ((itm.Cells(5).Value <> "" Or Not IsNothing(itm.Cells(5).Value))) And (itm.Cells(4).Value = "" Or IsNothing(itm.Cells(4).Value)) Then itm.Cells(4).Style.BackColor = Color.Red
                        If ((itm.Cells(7).Value <> "" Or Not IsNothing(itm.Cells(7).Value))) And (itm.Cells(6).Value = "" Or IsNothing(itm.Cells(6).Value)) Then itm.Cells(6).Style.BackColor = Color.Red

                        'OCR post processing - process each row for a supply and make sure buy prices are present
                        If (itm.Cells(6).Value <> "" Or itm.Cells(7).Value <> "") And (IsNothing(itm.Cells(2).Value) Or itm.Cells(2).Value = "") Then itm.Cells(2).Style.BackColor = Color.Red

                        'OCR post processing - ensure every valid item has a sell price
                        If IsNothing(itm.Cells(1).Value) Or itm.Cells(1).Value = "" Then itm.Cells(1).Style.BackColor = Color.Red

                        'OCR post processing - ensure every price is composed of numbers
                        If Not IsNumeric(itm.Cells(1).Value) Then itm.Cells(1).Style.BackColor = Color.Red
                        If Not IsNothing(itm.Cells(2).Value) And Not IsNumeric(itm.Cells(2).Value) Then itm.Cells(2).Style.BackColor = Color.Red
                        If Not IsNothing(itm.Cells(4).Value) And Not IsNumeric(itm.Cells(4).Value) Then itm.Cells(4).Style.BackColor = Color.Red
                        If Not IsNothing(itm.Cells(6).Value) And Not IsNumeric(itm.Cells(6).Value) Then itm.Cells(6).Style.BackColor = Color.Red

                        'OCR post processing - enable amber background for prices that vary by more than specified percentage (set in options) - prevents number errors in OCR
                        With Me.ComboBox_OCR_SelectSystem
                            If .SelectedIndex <> -1 And Me.DataGridView_OCR_CommPrice.Rows.Count > 0 Then
                                sql = "select system_id from Starsystems where system_name='" + .SelectedItem.ToString + "'"
                                dt2 = dbe.SQLStringtoDatatable(sql)
                                Dim sys_id As String = dt2.Rows(0).Item(0).ToString
                                sql = "select station_id from Stations where station_name = '" + Me.TextBox_OCR_DetectedStationName.Text + "'"
                                dt2 = dbe.SQLStringtoDatatable(sql)
                                Dim sta_id As String = dt2.Rows(0).Item(0).ToString


                                If itm.Cells(0).Style.BackColor <> Color.Red Then
                                    sql = "select sell_price,buy_price from CommPrices where system_id = " + sys_id + " and station_id = " + sta_id + " and item_id = " + itm_id + ""
                                    dt2 = dbe.SQLStringtoDatatable(sql)
                                    If dt2.Rows.Count > 0 Then
                                        If itm.Cells(1).Style.BackColor <> Color.Red Then
                                            Dim max As Integer = dt2.Rows(0).Item(0) * varPercentHigh
                                            Dim min As Integer = dt2.Rows(0).Item(0) * varPercentLow
                                            If itm.Cells(1).Value < min Or itm.Cells(1).Value > max Then
                                                itm.Cells(1).Style.BackColor = Color.LightGoldenrodYellow
                                                itm.Cells(1).ToolTipText = "Last registered price = " + dt2.Rows(0).Item(0).ToString
                                            End If
                                        End If
                                        If itm.Cells(2).Style.BackColor <> Color.Red Then
                                            Dim max As Integer = dt2.Rows(0).Item(1) * varPercentHigh
                                            Dim min As Integer = dt2.Rows(0).Item(1) * varPercentLow
                                            If itm.Cells(2).Value < min Or itm.Cells(2).Value > max Then
                                                itm.Cells(2).Style.BackColor = Color.LightGoldenrodYellow
                                                itm.Cells(2).ToolTipText = "Last registered price = " + dt2.Rows(0).Item(1).ToString
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End With
                    End If
                Next


            Else
                MsgBox("No valid file found.  Please check directory\file exists and try again!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Timer_OCR_FileList.Start()
            Me.Cursor = Cursors.Default
            AddHandler DataGridView_OCR_CommPrice.CellValueChanged, AddressOf DataGridView_OCR_CommPrice_CellValueChanged
        End Try
    End Sub

    Private Declare Function GetForegroundWindow Lib "user32.dll" () As IntPtr

    Private Sub OCR_capture_screen()

        Try
            Dim p() As Process
            p = Process.GetProcessesByName(Trim(My.Settings.OP_GEN_EDProcess))
            If p.Count > 0 Or My.Settings.offline_testing = 1 Or My.Settings.offline_testing = 2 Then ' if ED process is running OR test mode is enabled

                If My.Settings.offline_testing = 0 Then 'Switch to ED if screencapture is needed
                    For Each ObjProcess As Process In p
                        AppActivate(ObjProcess.Id)
                        SendKeys.SendWait("~")
                    Next
                End If

                Dim x As Integer
                Dim y As Integer
                Dim w As Integer
                Dim h As Integer
                Dim grayScaleType As Integer = My.Settings.OP_OCR_Grayscale_Type
                Dim scale As Integer = My.Settings.OP_OCR_ScalingFactor
                Dim filePathtmp As String = My.Settings.OP_OCR_temp_folder + "\capture-copy.bmp"

                '*** SET SOURCE OF IMAGE ***
                filePath_from = My.Settings.OP_OCR_temp_folder + "\capture.bmp"
                filePath_to = filePath_from
                If My.Settings.offline_testing = 0 Then 'if test mode is not enabled
                    'Screen capture on active application
                    Dim SC As New ScreenShot.ScreenCapture
                    Dim hWnd As IntPtr = GetForegroundWindow()
                    Using bmp = New Bitmap(SC.CaptureWindow(hWnd))
                        bmp.Save(filePath_from, Imaging.ImageFormat.Jpeg)
                    End Using
                ElseIf My.Settings.offline_testing = 1 Then 'decides whether to use new captured image or stored test image as source
                    filePath_from = filePathtmp
                ElseIf My.Settings.offline_testing = 2 Then
                    'Process from selected file
                    filePath_from = Trim(My.Settings.OP_OCR_ScreenshotDirectory + "\" + Me.ListBox_OCR_FileList.SelectedItem)
                End If


                '*** CROP OUTER EDGE OF SCREENSHOT***
                Using image As New MagickImage(filePath_from)
                    'Set crop position / size
                    x = Round((image.Width * 0.037), 0)
                    y = Round((image.Height * 0.06), 0)
                    w = Round((image.Width * 0.64), 0)
                    h = Round((image.Height * 0.9044), 0)
                    image.Quality = 100
                    image.Crop(New ImageMagick.MagickGeometry(x, y, w, h))
                    'SAVE TO "capture.bmp"
                    image.Write(filePath_to)
                End Using
                filePath_from = filePath_to
                filePath_to = My.Settings.OP_OCR_temp_folder + "\capture2.bmp"

                '*** CROP HEADER ***
                hdr_flag = True
                Using image As New MagickImage(filePath_from)
                    'CROP TO HEADER ONLY
                    w = (image.Width * 1).ToString
                    h = (image.Height * 0.038).ToString
                    image.Quality = 100
                    image.Crop(New ImageMagick.MagickGeometry(0, 0, w, h))
                    'SAVE TO "capture2.bmp"
                    image.Write(filePath_to)
                End Using
                filePath_from = filePath_to
                filePath_to = My.Settings.OP_OCR_temp_folder + "\capture2.jpg"


                '*** PROCESS HEADER IMAGE ***
                fileName = IO.Path.GetFileName(filePath_to)
                Using image As New MagickImage(filePath_from)
                    'w = (image.Width * scale).ToString
                    'h = (image.Height * scale).ToString

                    'PROCESS IMAGE
                    image.Quality = 100
                    image.Negate(Channels.All)
                    image.Grayscale(grayScaleType)
                    'SAVE TO "capture2.jpg"
                    image.Write(filePath_to)
                End Using


                ' *** OCR ON HEADER ***
                'Run OCR on image
                OCR()
                'Set Detected Station Name
                If Not OCR_remember_station Then Me.TextBox_OCR_DetectedStationName.Text = Trim(Me.DataGridView_OCR_CommPrice.Rows(0).Cells(0).Value)


                ' *** CROP MARKET CAPTURE - REMOVE HEADER ***
                'Set filePath to Commodities Image
                filePath_from = My.Settings.OP_OCR_temp_folder + "\capture.bmp"
                filePath_to = My.Settings.OP_OCR_temp_folder + "\capture.jpg"
                fileName = IO.Path.GetFileName(filePath_to)

                hdr_flag = False
                Using image As New MagickImage(filePath_from)
                    'CROP TO COMMODITIES SECTION ONLY
                    'Set crop position / size
                    x = "0"
                    y = Round((image.Height * 0.19), 0).ToString
                    w = image.Width
                    h = Round(image.Height * 0.743, 0).ToString
                    image.Crop(New ImageMagick.MagickGeometry(x, y, w, h))

                    ' *** PROCESS MARKET IMAGE ***
                    'Set new image size / dpi
                    w = (image.Width * scale)
                    h = (image.Height * scale)

                    'PROCESS IMAGE
                    image.Quality = 100
                    image.Resize(w, h)
                    image.Grayscale(grayScaleType)
                    image.Negate(Channels.All)
                    image.BrightnessContrast(0, My.Settings.OP_OCR_Contrast)
                    image.Sharpen()
                    image.Gamma(My.Settings.OP_OCR_Gamma)
                    image.AutoLevel()
                    'SAVE TO "capture.jpg"
                    image.Write(filePath_to)
                End Using



                ' *** RUN OCR ON MARKET IMAGE ***
                'Run OCR on image
                OCR(True)

                ' *** Cleanup ***
                'bring application back into focus
                Me.BringToFront()

            Else
                ' Process is not running
                MsgBox("Elite Dangerous does not appear to be running. Please launch and try again.")
            End If

        Catch e As Exception
            MsgBox(e.Message + Chr(13) + Chr(13) + e.ToString)
        End Try
    End Sub

    Sub OCR(Optional ByVal numOnly As Boolean = False)
        Try
            Dim img As New Program_Tess302(My.Settings.OP_OCR_tessdata_folder, My.Settings.OP_OCR_temp_folder, filePath_to, fileName, My.Settings.OP_OCR_Language, EngineMode.TesseractOnly, numOnly)
            img.Main()
        Catch e As Exception
            MsgBox(e.Message + Chr(13) + Chr(13) + e.ToString)
        End Try

    End Sub

    Private Sub TextBox_OCR_DetectedStationName_TextChanged(sender As Object, e As System.EventArgs) Handles TextBox_OCR_DetectedStationName.TextChanged
        Me.ComboBox_OCR_SelectSystem.Enabled = True
        Me.ComboBox_OCR_SelectSystem.Items.Clear()
        Dim sql As String = "select t1.system_name,t1.system_id from starsystems as t1 join stations as t2 on t1.system_id = t2.system_id where t2.station_name = '" + Trim(Me.TextBox_OCR_DetectedStationName.Text).ToUpper + "' order by system_name"
        Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)
        If dt.Rows.Count > 0 Then
            For Each itm As DataRow In dt.Rows
                Me.ComboBox_OCR_SelectSystem.Items.Add(itm.Item(0).ToString)
            Next
            If dt.Rows.Count = 1 Then
                Me.ComboBox_OCR_SelectSystem.SelectedIndex = 0
            End If
        End If
        Me.ComboBox_OCR_SelectSystem.Items.Add("< ADD NEW SYSTEM >")

        For Each itm As DataGridViewRow In Me.DataGridView_OCR_CommPrice.Rows
            DGV_OCR_RowValuesCheck(itm.Index)
        Next

        If Me.ComboBox_OCR_SelectSystem.SelectedIndex = -1 Then
            Me.TextBox_OCR_DetectedStationName.BackColor = Color.Red
        Else
            Me.TextBox_OCR_DetectedStationName.BackColor = Color.White
        End If
    End Sub

    Private Sub ListBox_OCR_FileList_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox_OCR_FileList.SelectedIndexChanged
        If Me.ListBox_OCR_FileList.SelectedIndex = -1 Then Me.Button_OCR_DeleteImage.Enabled = False Else Me.Button_OCR_DeleteImage.Enabled = True
    End Sub

    Private Sub DataGridView_OCR_CommPrice_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView_OCR_CommPrice.CellValueChanged
        DGV_OCR_RowValuesCheck(e.RowIndex)
    End Sub
    Private Sub DGV_OCR_RowValuesCheck(ByVal e As Integer)
        Dim sql As String
        Dim dt2 As DataTable
        Dim varPercentHigh As Double = (100 + CInt(My.Settings.OP_OCR_PVariance)) / 100
        Dim varPercentLow As Double = (100 - CInt(My.Settings.OP_OCR_PVariance)) / 100

        'check for valid system/station
        If Me.ComboBox_OCR_SelectSystem.SelectedIndex <> -1 Then
            sql = "select system_id from Starsystems where system_name='" + Me.ComboBox_OCR_SelectSystem.SelectedItem.ToString + "'"
            dt2 = dbe.SQLStringtoDatatable(sql)
            Dim sys_id As String = dt2.Rows(0).Item(0).ToString
            sql = "select station_id from Stations where station_name = '" + Me.TextBox_OCR_DetectedStationName.Text + "'"
            dt2 = dbe.SQLStringtoDatatable(sql)
            Dim sta_id As String = dt2.Rows(0).Item(0).ToString
            Dim itm_id As String
            With Me.DataGridView_OCR_CommPrice
                'check for valid item name
                sql = "select item_id from Items where item_name = '" + .Rows(e).Cells(0).Value + "'"
                Dim dt As DataTable = dbe.SQLStringtoDatatable(sql)
                If dt.Rows.Count > 0 Then
                    .Rows(e).Cells(0).Style.BackColor = Color.White
                    itm_id = dt.Rows(0).Item(0).ToString
                Else
                    itm_id = Nothing
                    .Rows(e).Cells(0).Style.BackColor = Color.Red
                End If

                'OCR post processing - process each row for a supply/demand rating and ensure a supply/demand figure is present
                If ((.Rows(e).Cells(5).Value <> "" Or Not IsNothing(.Rows(e).Cells(5).Value))) And (.Rows(e).Cells(4).Value = "" Or IsNothing(.Rows(e).Cells(4).Value)) Then .Rows(e).Cells(4).Style.BackColor = Color.Red Else .Rows(e).Cells(4).Style.BackColor = Color.White
                If ((.Rows(e).Cells(7).Value <> "" Or Not IsNothing(.Rows(e).Cells(7).Value))) And (.Rows(e).Cells(6).Value = "" Or IsNothing(.Rows(e).Cells(6).Value)) Then .Rows(e).Cells(6).Style.BackColor = Color.Red Else .Rows(e).Cells(6).Style.BackColor = Color.White

                'OCR post processing - process each row for a supply and make sure buy prices are present
                If (.Rows(e).Cells(6).Value <> "" Or .Rows(e).Cells(7).Value <> "") And (IsNothing(.Rows(e).Cells(2).Value) Or .Rows(e).Cells(2).Value = "") Then .Rows(e).Cells(2).Style.BackColor = Color.Red Else .Rows(e).Cells(2).Style.BackColor = Color.White

                'OCR post processing - ensure every valid item has a sell price
                If IsNothing(.Rows(e).Cells(1).Value) Or .Rows(e).Cells(1).Value = "" Then .Rows(e).Cells(1).Style.BackColor = Color.Red Else .Rows(e).Cells(1).Style.BackColor = Color.White

                'OCR post processing - ensure every price is composed of numbers
                If Not IsNumeric(.Rows(e).Cells(1).Value) Then .Rows(e).Cells(1).Style.BackColor = Color.Red
                If Not IsNothing(.Rows(e).Cells(2).Value) And Not IsNumeric(.Rows(e).Cells(2).Value) Then .Rows(e).Cells(2).Style.BackColor = Color.Red Else .Rows(e).Cells(2).Style.BackColor = Color.White
                If Not IsNothing(.Rows(e).Cells(4).Value) And Not IsNumeric(.Rows(e).Cells(4).Value) Then .Rows(e).Cells(4).Style.BackColor = Color.Red Else .Rows(e).Cells(4).Style.BackColor = Color.White
                If Not IsNothing(.Rows(e).Cells(6).Value) And Not IsNumeric(.Rows(e).Cells(6).Value) Then .Rows(e).Cells(6).Style.BackColor = Color.Red Else .Rows(e).Cells(6).Style.BackColor = Color.White

                'if there is a valid item
                If Not IsNothing(itm_id) Then
                    sql = "select sell_price,buy_price from CommPrices where system_id = " + sys_id + " and station_id = " + sta_id + " and item_id = " + itm_id + ""
                    dt2 = dbe.SQLStringtoDatatable(sql)
                    If dt2.Rows.Count > 0 Then
                        If .Rows(e).Cells(1).Style.BackColor <> Color.Red Then
                            Dim max As Integer = dt2.Rows(0).Item(0) * varPercentHigh
                            Dim min As Integer = dt2.Rows(0).Item(0) * varPercentLow
                            If .Rows(e).Cells(1).Value < min Or .Rows(e).Cells(1).Value > max Then
                                .Rows(e).Cells(1).Style.BackColor = Color.LightGoldenrodYellow
                                .Rows(e).Cells(1).ToolTipText = "Last registered price = " + dt2.Rows(0).Item(0).ToString
                            Else
                                .Rows(e).Cells(1).Style.BackColor = Color.White
                                .Rows(e).Cells(1).ToolTipText = Nothing
                            End If
                        End If
                        If .Rows(e).Cells(2).Style.BackColor <> Color.Red Then
                            Dim max As Integer = dt2.Rows(0).Item(1) * varPercentHigh
                            Dim min As Integer = dt2.Rows(0).Item(1) * varPercentLow
                            If .Rows(e).Cells(2).Value < min Or .Rows(e).Cells(2).Value > max Then
                                .Rows(e).Cells(2).Style.BackColor = Color.LightGoldenrodYellow
                                .Rows(e).Cells(2).ToolTipText = "Last registered price = " + dt2.Rows(0).Item(1).ToString
                            Else
                                .Rows(e).Cells(2).Style.BackColor = Color.White
                                .Rows(e).Cells(2).ToolTipText = Nothing
                            End If
                        End If
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub Button_OCR_DeleteImage_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_DeleteImage.Click
        If Me.ListBox_OCR_FileList.SelectedIndex <> -1 Then
            If MsgBox("Are you sure you wish to delete this image?", MsgBoxStyle.OkCancel, "Delete Image") = MsgBoxResult.Ok Then
                Me.Timer_OCR_FileList.Stop()
                Dim fn As String = Trim(My.Settings.OP_OCR_ScreenshotDirectory + "\" + Me.ListBox_OCR_FileList.SelectedItem)
                File.Delete(fn)
                Me.ListBox_OCR_FileList.SelectedIndex = -1
                Me.Timer_OCR_FileList.Start()
            End If
        End If
    End Sub
    Private Sub ListBox_OCR_FileList_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles ListBox_OCR_FileList.MouseDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Left And e.Clicks = 2 Then

            Dim fn As String = Trim(My.Settings.OP_OCR_ScreenshotDirectory + "\" + Me.ListBox_OCR_FileList.SelectedItem)
            If File.Exists(fn) Then
                File.Open(fn, FileMode.Open)
            Else
                MsgBox("File not found!", MsgBoxStyle.Critical)
            End If
        End If

    End Sub

    Dim OCR_remember_station As Boolean = False
    Private Sub Button_OCR_RememberStation_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_RememberStation.Click
        If OCR_remember_station Then
            Me.Button_OCR_RememberStation.BackColor = Color.Transparent
            OCR_remember_station = False
        Else
            Me.Button_OCR_RememberStation.BackColor = Color.GreenYellow
            OCR_remember_station = True
        End If
    End Sub

    'Commit Changes
    Private Sub Button_OCR_DBUpdate_Click(sender As System.Object, e As System.EventArgs) Handles Button_OCR_DBUpdate.Click
        update_db("OCR")

    End Sub

    ' This sub handles updates from Browse Station tab also
    Sub update_db(ByVal src As String)
        Try

            Me.Cursor = Cursors.WaitCursor

            Dim dgv As DataGridView
            Dim itmCol As Integer
            Dim sellCol As Integer
            Dim buyCol As Integer
            Dim demCol As Integer
            Dim availCol As Integer
            Dim staName As String
            Dim sysName As String
            Dim lastupdate As Date = Now
            If src = "OCR" Then
                dgv = Me.DataGridView_OCR_CommPrice
                itmCol = 0
                sellCol = 1
                buyCol = 2
                demCol = 4
                availCol = 6
                staName = Trim(Me.TextBox_OCR_DetectedStationName.Text.ToString).ToUpper
                sysName = Me.ComboBox_OCR_SelectSystem.SelectedItem
            ElseIf src = "Commodities" Then
                dgv = Me.DataGridView_BS_Commodities
                itmCol = 0
                sellCol = 1
                buyCol = 2
                demCol = 3
                availCol = 4
                sysName = Me.Label_BS_System.Text
                staName = Me.Label_BS_Station.Text
            Else
                Throw New Exception("Invalid Source for src in update_db")
            End If

            Dim txt As String = ""
            If dgv.Rows.Count > 0 Then
                Dim sysID As String
                Dim staID As String
                Dim sql As String = ""


                'find system/station ID
                sql = "select system_id,station_id from Stations where station_name = '" + staName + "' and system_id = (select system_id from Starsystems where system_name = '" + sysName + "')"
                Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)
                If dt.Rows.Count > 0 Then
                    sysID = dt.Rows(0).Item("system_id").ToString
                    staID = dt.Rows(0).Item("station_id").ToString
                Else
                    Throw New Exception("No valid Starsystem/Station combination found")
                End If

                For Each itm As DataGridViewRow In dgv.Rows
                    Dim itmID As String
                    If Not IsNothing(itm.Cells(itmCol).Value) And Not IsNothing(itm.Cells(sellCol).Value) Then
                        'get item ID
                        sql = "select item_id from items where item_name = '" + Trim(itm.Cells(itmCol).Value.ToString) + "'"
                        Dim dt3 As Data.DataTable = dbe.SQLStringtoDatatable(sql)
                        If dt3.Rows.Count > 0 Then
                            'set item ID
                            itmID = dt3.Rows(0).Item("item_id").ToString


                            'get new price data from grid
                            Dim sell As String = ""
                            Dim buy As String = ""
                            Dim demand As String = ""
                            Dim supply As String = ""

                            'sell
                            If IsNothing(itm.Cells(sellCol).Value) = False Then
                                If IsNumeric(Replace(itm.Cells(sellCol).Value, " ", "")) Then
                                    sell = Replace(itm.Cells(sellCol).Value, " ", "")
                                Else
                                    sell = 0
                                    txt = txt + "Sell Price of " + Trim(itm.Cells(itmCol).Value.ToString) + " is not a number! Check for erroneous characters." + Chr(13)
                                End If
                            Else
                                sell = 0
                            End If

                            'buy
                            If IsNothing(itm.Cells(buyCol).Value) = False Then
                                If IsNumeric(Replace(itm.Cells(buyCol).Value, " ", "")) Then
                                    buy = Replace(itm.Cells(buyCol).Value, " ", "")
                                Else
                                    buy = 0
                                    txt = txt + "Buy Price of " + Trim(itm.Cells(itmCol).Value.ToString) + " is not a number! Check for erroneous characters." + Chr(13)
                                End If
                            Else
                                buy = 0
                            End If


                            'demand
                            If IsNothing(itm.Cells(demCol).Value) = False Then
                                If IsNumeric(Replace(itm.Cells(demCol).Value, " ", "")) Then
                                    demand = Replace(itm.Cells(demCol).Value, " ", "")
                                Else
                                    demand = 0
                                    txt = txt + "Demand Quantity of " + Trim(itm.Cells(itmCol).Value.ToString) + " is not a number! Check for erroneous characters." + Chr(13)
                                End If
                            Else
                                demand = 0
                            End If


                            'supply
                            If IsNothing(itm.Cells(availCol).Value) = False Then
                                If IsNumeric(Replace(itm.Cells(availCol).Value, " ", "")) Then
                                    supply = Replace(itm.Cells(availCol).Value, " ", "")
                                Else
                                    supply = 0
                                    txt = txt + "Supply Quantity of " + Trim(itm.Cells(itmCol).Value.ToString) + " is not a number! Check for erroneous characters." + Chr(13)
                                End If
                            Else
                                supply = 0
                            End If


                            'get existing CommPrices data
                            sql = "select * from CommPrices where item_id = " + itmID + " and station_id = " + staID + " and system_id = " + sysID
                            Dim dt2 As Data.DataTable = dbe.SQLStringtoDatatable(sql)
                            If dt2.Rows.Count > 0 Then
                                'if exists - update 
                                sql = "update CommPrices set sell_price = " + sell + " , buy_price = " + buy + ", qty_demand= " + demand + ", qty_avail = " + supply + ",last_updated='" + lastupdate.ToString + "' where item_id = " + itmID + " and station_id = " + staID + " and system_id = " + sysID
                                dbe.SQL_String_Execute(sql)
                            Else
                                'else - insert
                                sql = "insert into CommPrices (item_id,station_id,system_id,sell_price,buy_price,qty_demand,qty_avail,last_updated) values (" + itmID + "," + staID + "," + sysID + "," + sell + "," + buy + "," + demand + "," + supply + ",'" + lastupdate.ToString + "')"
                                dbe.SQL_String_Execute(sql)
                            End If

                        Else
                            'ITEM NOT FOUND
                            txt = txt + Trim(itm.Cells(itmCol).Value.ToString) + " is not a valid commodity! Check spellingy. " + Chr(13)
                        End If
                    Else

                    End If
                Next
            Else
                MsgBox("No Data found.  Please select another file.")
            End If

            If txt = "" Then
                'DELETE SCREENSHOT HERE IF FLAG IS CHECKED
                If My.Settings.OP_OCR_DeleteScreenshot And src = "OCR" Then
                    File.Delete(My.Settings.OP_OCR_ScreenshotDirectory + "\" + Me.ListBox_OCR_FileList.SelectedItem)
                End If
                MsgBox("Data updated!")
            Else
                MsgBox("Errors were encountered while updating : " + Chr(13) + Chr(13) + txt + Chr(13) + " All other commodities updated successfully! " + Chr(13))
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ComboBox_OCR_SelectSystem_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ComboBox_OCR_SelectSystem.SelectedIndexChanged
        If Me.ComboBox_OCR_SelectSystem.SelectedValue = "< ADD NEW SYSTEM >" Then
            Me.TabControl_Function.SelectTab(Tab_System_Station)
            Me.TextBox_SYSADD_Station.Text = Trim(Me.TextBox_OCR_DetectedStationName.Text)
            Me.TextBox_OCR_DetectedStationName.BackColor = Color.White
        ElseIf Me.ComboBox_OCR_SelectSystem.SelectedItem = "" Then
            Me.TextBox_OCR_DetectedStationName.BackColor = Color.Red
        Else
            Me.TextBox_OCR_DetectedStationName.BackColor = Color.White
        End If
    End Sub

    Private Sub DataGridView_OCR_CommPrice_EditingControlShowing(sender As Object, e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles DataGridView_OCR_CommPrice.EditingControlShowing
        Dim col As TextBox = e.Control
        If Me.DataGridView_OCR_CommPrice.CurrentCell.ColumnIndex = 0 Then
            col.AutoCompleteMode = AutoCompleteMode.Suggest
            col.AutoCompleteSource = AutoCompleteSource.CustomSource
            col.AutoCompleteCustomSource = ac
            col.CharacterCasing = CharacterCasing.Upper
        Else
            col.AutoCompleteSource = AutoCompleteSource.None
        End If
    End Sub



    '******************************
    '****** SYS/STATION TAB *******
    '******************************
    Sub Refresh_SYSADD_Stations()
        RemoveHandler ListView_SYSADD_StationList.SelectedIndexChanged, AddressOf ListView_SYSADD_StationList_SelectedIndexChanged

        Dim sql As String = "select t1.station_name,t2.system_name from Stations as 't1' join Starsystems as 't2' on t1.system_id = t2.system_id order by t2.system_name,t1.station_name"
        Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)

        With Me.ListView_SYSADD_StationList
            .Items.Clear()
            For Each itm As DataRow In dt.Rows
                Dim grpExist As Boolean = False
                For Each grp As ListViewGroup In .Groups
                    If itm.Item(1).ToString = grp.Header Then grpExist = True
                Next
                If Not grpExist Then .Groups.Add(New ListViewGroup(key:=itm.Item(1).ToString, headerText:=itm.Item(1).ToString))

                Dim lvi As ListViewItem = New ListViewItem(Text:=itm.Item(0).ToString, group:=.Groups(itm.Item(1).ToString))
                .Items.Add(lvi)
            Next
        End With

        Me.TextBox_SYSADD_System.Text = ""
        Me.TextBox_SYSADD_Station.Text = ""
        Me.TextBox_SYSADD_StatDist.Text = ""
        Me.CheckBox_SYSADD_Market.Checked = False
        Me.CheckBox_SYSADD_BlackMarket.Checked = False
        Me.CheckBox_SYSADD_Outfitting.Checked = False
        Me.CheckBox_SYSADD_Shipyard.Checked = False
        Me.Button_SYSADD_STAOutpost.BackColor = Color.Transparent
        Me.Button_SYSADD_STARing.BackColor = Color.Transparent
        Me.Button_SYSADD_STACoriolis.BackColor = Color.Transparent

        Me.Button_SYSADD_CreateNew.Enabled = False
        Me.Button_SYSADD_StationAdd.Visible = True
        Me.Button_SYSADD_Update.Visible = False

        AddHandler ListView_SYSADD_StationList.SelectedIndexChanged, AddressOf ListView_SYSADD_StationList_SelectedIndexChanged
    End Sub

    Dim SYSADD_System_ID As String = ""
    Dim SYSADD_Station_ID As String = ""
    Private Sub ListView_SYSADD_StationList_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView_SYSADD_StationList.SelectedIndexChanged

        If Me.ListView_SYSADD_StationList.SelectedItems.Count > 0 Then

            Dim sql As String = "select t1.system_id,t1.system_name,t2.station_id,t2.station_name,t2.star_dist,t2.market,t2.blackmarket,t2.outfitting,t2.shipyard,t2.station_type from Starsystems as 't1' join Stations as 't2' on t1.system_id = t2.system_id where t1.system_name = '" + Me.ListView_SYSADD_StationList.SelectedItems(0).Group.Name.ToString + "' and station_name = '" + Me.ListView_SYSADD_StationList.SelectedItems(0).Text + "'"
            Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)

            SYSADD_System_ID = dt.Rows(0).Item(0).ToString
            SYSADD_Station_ID = dt.Rows(0).Item(2).ToString
            Me.TextBox_SYSADD_System.Text = dt.Rows(0).Item(1).ToString
            Me.TextBox_SYSADD_Station.Text = dt.Rows(0).Item(3).ToString
            Me.TextBox_SYSADD_StatDist.Text = dt.Rows(0).Item(4).ToString
            If dt.Rows(0).Item(5).ToString = "1" Then Me.CheckBox_SYSADD_Market.Checked = True Else Me.CheckBox_SYSADD_Market.Checked = False
            If dt.Rows(0).Item(6).ToString = "1" Then Me.CheckBox_SYSADD_BlackMarket.Checked = True Else Me.CheckBox_SYSADD_BlackMarket.Checked = False
            If dt.Rows(0).Item(7).ToString = "1" Then Me.CheckBox_SYSADD_Outfitting.Checked = True Else Me.CheckBox_SYSADD_Outfitting.Checked = False
            If dt.Rows(0).Item(8).ToString = "1" Then Me.CheckBox_SYSADD_Shipyard.Checked = True Else Me.CheckBox_SYSADD_Shipyard.Checked = False
            If dt.Rows(0).Item(9).ToString = "1" Then Me.Button_SYSADD_STAOutpost.PerformClick()
            If dt.Rows(0).Item(9).ToString = "2" Then Me.Button_SYSADD_STARing.PerformClick()
            If dt.Rows(0).Item(9).ToString = "3" Then Me.Button_SYSADD_STACoriolis.PerformClick()

            Me.Button_SYSADD_CreateNew.Enabled = True
            Me.Button_SYSADD_StationAdd.Visible = False
            Me.Button_SYSADD_Update.Visible = True
        Else
            SYSADD_System_ID = ""
            SYSADD_Station_ID = ""
            Me.TextBox_SYSADD_System.Text = ""
            Me.TextBox_SYSADD_Station.Text = ""
            Me.TextBox_SYSADD_StatDist.Text = ""
            Me.CheckBox_SYSADD_Market.Checked = False
            Me.CheckBox_SYSADD_BlackMarket.Checked = False
            Me.CheckBox_SYSADD_Outfitting.Checked = False
            Me.CheckBox_SYSADD_Shipyard.Checked = False
            Me.Button_SYSADD_STAOutpost.BackColor = Color.Transparent
            Me.Button_SYSADD_STARing.BackColor = Color.Transparent
            Me.Button_SYSADD_STACoriolis.BackColor = Color.Transparent

            Me.Button_SYSADD_CreateNew.Enabled = False
            Me.Button_SYSADD_StationAdd.Visible = True
            Me.Button_SYSADD_Update.Visible = False
        End If

    End Sub

    Private Sub Button_SYSADD_CreateNew_Click(sender As System.Object, e As System.EventArgs) Handles Button_SYSADD_CreateNew.Click
        Refresh_SYSADD_Stations()
    End Sub

    Private Sub Button_SYSADD_Update_Click(sender As System.Object, e As System.EventArgs) Handles Button_SYSADD_Update.Click
        Dim txt As String = ""
        If Me.TextBox_SYSADD_System.Text = "" Then txt = txt + "No Valid ""System Name"" found! " + Chr(13)
        If Me.TextBox_SYSADD_Station.Text = "" Then txt = txt + "No Valid ""Station Name"" found! " + Chr(13)
        If Me.TextBox_SYSADD_StatDist.Text = "" Then txt = txt + "No Valid ""Distance to Primary Star"" found! " + Chr(13)
        If Me.Button_SYSADD_STACoriolis.BackColor = Color.Transparent And Me.Button_SYSADD_STAOutpost.BackColor = Color.Transparent And Me.Button_SYSADD_STARing.BackColor = Color.Transparent Then txt = txt + "Please select a ""Station Type""!" + Chr(13)

        If txt = "" Then
            Dim market As Integer = 0
            Dim blackmarket As Integer = 0
            Dim outf As Integer = 0
            Dim shpyrd As Integer = 0
            Dim staDist As Integer = 0
            Dim staType As Integer = 0
            If Me.CheckBox_SYSADD_Market.Checked Then market = 1
            If Me.CheckBox_SYSADD_BlackMarket.Checked Then blackmarket = 1
            If Me.CheckBox_SYSADD_Outfitting.Checked Then outf = 1
            If Me.CheckBox_SYSADD_Shipyard.Checked Then shpyrd = 1
            If Me.Button_SYSADD_STAOutpost.BackColor = Color.GreenYellow Then staType = 1
            If Me.Button_SYSADD_STARing.BackColor = Color.GreenYellow Then staType = 2
            If Me.Button_SYSADD_STACoriolis.BackColor = Color.GreenYellow Then staType = 3

            Dim sql As String = "update Starsystems set system_name = '" + Me.TextBox_SYSADD_System.Text.Trim + "' where system_id = " + SYSADD_System_ID
            dbe.SQL_String_Execute(sql)

            sql = "update Stations set station_name = '" + Me.TextBox_SYSADD_Station.Text.Trim + "', star_dist= " + Me.TextBox_SYSADD_StatDist.Text.Trim + ", market=" + market.ToString + ", blackmarket=" + blackmarket.ToString + ", outfitting=" + outf.ToString + ", shipyard=" + shpyrd.ToString + ", station_type=" + staType.ToString + " where station_id = " + SYSADD_Station_ID
            dbe.SQL_String_Execute(sql)

            Refresh_SYSADD_Stations()
        Else
            MsgBox(txt + Chr(13) + " Please correct missing items and update again.")
        End If

    End Sub
    Private Sub Button_SYSADD_StationAdd_Click(sender As System.Object, e As System.EventArgs) Handles Button_SYSADD_StationAdd.Click
        Dim system As String = Trim(Me.TextBox_SYSADD_System.Text).ToUpper
        Dim station As String = Trim(Me.TextBox_SYSADD_Station.Text).ToUpper

        Dim sql As String = ""
        If Me.TextBox_SYSADD_Station.Text = "" Then
            If Me.TextBox_SYSADD_System.Text = "" Then
                MsgBox("Please enter a System and/or Station to add")
            Else
                'add system only
                sql = "select * from starsystems where system_name = '" + system + "'"
                Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)
                If dt.Rows.Count > 0 Then
                    MsgBox("Starsystem already exists!")
                Else
                    sql = "insert into Starsystems (system_name) values ('" + system + "')"
                    dbe.SQL_String_Execute(sql)
                    MsgBox("System added.")
                End If
            End If
        Else
            If Me.TextBox_SYSADD_System.Text = "" Then
                MsgBox("Please enter the Starsystem for this Station")
            Else
                'check for system and add
                Dim sysID As String = ""
                Dim staID As String = ""
                Dim sysAdd As Boolean = False
                sql = "select system_id from Starsystems where system_name = '" + system + "'"
                Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)
                If dt.Rows.Count > 0 Then
                    sysID = dt.Rows(0).Item(0).ToString
                Else
                    sql = "insert into Starsystems (system_name) values ('" + system + "')"
                    dbe.SQL_String_Execute(sql)

                    sql = "select system_id from Starsystems where system_name = '" + system + "'"
                    dt = dbe.SQLStringtoDatatable(sql)
                    sysID = dt.Rows(0).Item(0).ToString
                End If

                'check for station and add
                sql = "select * from stations where system_id = " + sysID + " and station_name = '" + station + "'"
                dt = dbe.SQLStringtoDatatable(sql)
                If dt.Rows.Count > 0 Then
                    MsgBox("Station already exists!")
                Else
                    Dim market As Integer = 0
                    Dim blackmarket As Integer = 0
                    Dim outf As Integer = 0
                    Dim shpyrd As Integer = 0
                    Dim staDist As Integer = 0
                    If Me.CheckBox_SYSADD_Market.Checked Then market = 1
                    If Me.CheckBox_SYSADD_BlackMarket.Checked Then blackmarket = 1
                    If Me.CheckBox_SYSADD_Outfitting.Checked Then outf = 1
                    If Me.CheckBox_SYSADD_Shipyard.Checked Then shpyrd = 1

                    sql = "insert into Stations (station_name,system_id,market,blackmarket,outfitting,shipyard) values ('" + station + "'," + sysID + "," + market.ToString + "," + blackmarket.ToString + "," + outf.ToString + "," + shpyrd.ToString + ")"
                    dbe.SQL_String_Execute(sql)

                    If sysAdd Then
                        MsgBox("System/Station added.")
                    Else
                        MsgBox("Station added.")
                    End If
                End If

            End If
        End If


    End Sub

    Dim SYSADD_selected_STAType As Integer = 0
    Private Sub Button_SYSADD_STAOutpost_Click(sender As System.Object, e As System.EventArgs) Handles Button_SYSADD_STAOutpost.Click
        SYSADD_selected_STAType = 1
        Me.Button_SYSADD_STAOutpost.BackColor = Color.GreenYellow
        Me.Button_SYSADD_STARing.BackColor = Color.Transparent
        Me.Button_SYSADD_STACoriolis.BackColor = Color.Transparent
    End Sub
    Private Sub Button_SYSADD_STARing_Click(sender As System.Object, e As System.EventArgs) Handles Button_SYSADD_STARing.Click
        SYSADD_selected_STAType = 2
        Me.Button_SYSADD_STAOutpost.BackColor = Color.Transparent
        Me.Button_SYSADD_STARing.BackColor = Color.GreenYellow
        Me.Button_SYSADD_STACoriolis.BackColor = Color.Transparent
    End Sub
    Private Sub Button_SYSADD_STACoriolis_Click(sender As System.Object, e As System.EventArgs) Handles Button_SYSADD_STACoriolis.Click
        SYSADD_selected_STAType = 3
        Me.Button_SYSADD_STAOutpost.BackColor = Color.Transparent
        Me.Button_SYSADD_STARing.BackColor = Color.Transparent
        Me.Button_SYSADD_STACoriolis.BackColor = Color.GreenYellow
    End Sub

    '******************************
    '****** COMMODITIES TAB *******
    '******************************

    '   *** BROWSE STATION ***
    '       Filter Controls
    Private Sub TextBox_BS_StarsystemFilter_TextChanged(sender As System.Object, e As System.EventArgs)
        If IsNothing(Me.TextBox_BS_StarsystemFilter.Text) Then
            BS_refresh_systems("")
        Else
            BS_refresh_systems(Me.TextBox_BS_StarsystemFilter.Text)
        End If
    End Sub

    Private Sub TextBox_BS_StationFilter_TextChanged(sender As System.Object, e As System.EventArgs)
        If IsNothing(Me.TextBox_BS_StationFilter.Text) Then
            BS_refresh_stations("")
        Else
            BS_refresh_stations(Me.TextBox_BS_StationFilter.Text)
        End If
    End Sub

    Private Sub Button_BS_ClearFilters_Click(sender As System.Object, e As System.EventArgs) Handles Button_BS_ClearFilters.Click


        RemoveHandler TextBox_BS_StarsystemFilter.TextChanged, AddressOf TextBox_BS_StarsystemFilter_TextChanged
        RemoveHandler TextBox_BS_StationFilter.TextChanged, AddressOf TextBox_BS_StationFilter_TextChanged


        BS_refresh_systems("")
        BS_refresh_stations("")


        Me.ListBox_BS_Station.Text = ""
        Me.ListBox_BS_Starsystem.Text = ""



        AddHandler TextBox_BS_StarsystemFilter.TextChanged, AddressOf TextBox_BS_StarsystemFilter_TextChanged
        AddHandler TextBox_BS_StationFilter.TextChanged, AddressOf TextBox_BS_StationFilter_TextChanged


    End Sub

    Private Sub ListBox_BS_Starsystem_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        If Me.ListBox_BS_Starsystem.SelectedIndex <> -1 Then

            RemoveHandler TextBox_BS_StationFilter.TextChanged, AddressOf TextBox_BS_StationFilter_TextChanged
            Me.TextBox_BS_StationFilter.Text = ""
            AddHandler TextBox_BS_StationFilter.TextChanged, AddressOf TextBox_BS_StationFilter_TextChanged

            BS_refresh_stations(Me.ListBox_BS_Starsystem.SelectedValue.ToString, "")
        Else
            BS_refresh_stations("")
        End If
    End Sub

    Private Sub ListBox_BS_Station_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)

        If Me.ListBox_BS_Station.SelectedIndex <> -1 Then

            Dim sql As String = ""

            If Me.ListBox_BS_Starsystem.SelectedIndex <> -1 And Me.ListBox_BS_Starsystem.Items.Count > 1 Then

                Me.Label_BS_System.Text = dt_sys.Rows(Me.ListBox_BS_Starsystem.SelectedIndex).Item(1).ToString
                Me.Label_BS_Station.Text = dt_stn.Rows(Me.ListBox_BS_Station.SelectedIndex).Item(1).ToString

                'FILL DATAGRIDVIEW
                refresh_DGV_Commodities()

            Else

                sql = "select system_id,system_name from starsystems where system_id in (select system_id from stations where station_id = " + Me.ListBox_BS_Station.SelectedValue.ToString + ")"
                Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)

                Me.TextBox_BS_StarsystemFilter.Text = dt.Rows(0).Item(1).ToString

                RemoveHandler ListBox_BS_Starsystem.SelectedIndexChanged, AddressOf ListBox_BS_Starsystem_SelectedIndexChanged

                Me.ListBox_BS_Starsystem.SelectedIndex = 0

                RemoveHandler TextBox_BS_StarsystemFilter.TextChanged, AddressOf TextBox_BS_StarsystemFilter_TextChanged

                Me.TextBox_BS_StarsystemFilter.Text = ""

                AddHandler TextBox_BS_StarsystemFilter.TextChanged, AddressOf TextBox_BS_StarsystemFilter_TextChanged
                AddHandler ListBox_BS_Starsystem.SelectedIndexChanged, AddressOf ListBox_BS_Starsystem_SelectedIndexChanged

                Me.Label_BS_System.Text = dt_sys.Rows(Me.ListBox_BS_Starsystem.SelectedIndex).Item(1).ToString
                Me.Label_BS_Station.Text = dt_stn.Rows(Me.ListBox_BS_Station.SelectedIndex).Item(1).ToString

                'FILL DATAGRIDVIEW
                refresh_DGV_Commodities()

            End If

        End If


    End Sub

    Public Sub BS_refresh_systems(ByVal sys As String)

        RemoveHandler ListBox_BS_Starsystem.SelectedIndexChanged, AddressOf ListBox_BS_Starsystem_SelectedIndexChanged

        Dim sql As String
        If Me.RadioButton_BS_Market.Checked Then
            If Me.CheckBox_BS_ExcNonMarket.Checked Then
                sql = "select * from Starsystems where system_name like '%" + sys + "%'  and system_visible=1 and system_id in (select system_id from stations where market=1) order by system_name"
            Else
                sql = "select * from Starsystems where system_name like '%" + sys + "%'  and system_visible=1 order by system_name"
            End If
        Else
            sql = "select * from Starsystems where system_name like '%" + sys + "%'  and system_visible=1 and system_id in (select system_id from stations where blackmarket = 1)  order by system_name"
        End If


        dt_sys = dbe.SQLStringtoDatatable(sql)

        Me.ListBox_BS_Starsystem.DataSource = Nothing
        Me.ListBox_BS_Starsystem.Items.Clear()

        If dt_sys.Rows.Count > 0 Then

            Me.ListBox_BS_Starsystem.Enabled = True

            With Me.ListBox_BS_Starsystem
                .DataSource = dt_sys
                .ValueMember = "system_id"
                .DisplayMember = "system_name"
                .SelectedIndex = -1
            End With
        Else

            Me.ListBox_BS_Starsystem.Items.Add("No match found")
            Me.ListBox_BS_Starsystem.Enabled = False

        End If

        AddHandler ListBox_BS_Starsystem.SelectedIndexChanged, AddressOf ListBox_BS_Starsystem_SelectedIndexChanged

    End Sub

    Public Sub BS_refresh_stations(ByVal stn As String)

        RemoveHandler ListBox_BS_Station.SelectedIndexChanged, AddressOf ListBox_BS_Station_SelectedIndexChanged


        Dim sql As String
        If Me.RadioButton_BS_Market.Checked Then
            If Me.CheckBox_BS_ExcNonMarket.Checked Then
                sql = "select * from stations where station_name like '%" + stn + "%' and station_visible=1 and system_id in (select system_id from starsystems where system_visible=1) and market=1 order by station_name"
            Else
                sql = "select * from stations where station_name like '%" + stn + "%' and station_visible=1 and system_id in (select system_id from starsystems where system_visible=1) order by station_name"
            End If
        Else
            sql = "select * from stations where station_name like '%" + stn + "%' and station_visible=1 and system_id in (select system_id from starsystems where system_visible=1) and blackmarket=1 order by station_name"
        End If


        dt_stn = dbe.SQLStringtoDatatable(sql)

        Me.ListBox_BS_Station.DataSource = Nothing
        Me.ListBox_BS_Station.Items.Clear()

        If dt_stn.Rows.Count > 0 Then

            Me.ListBox_BS_Station.Enabled = True

            With Me.ListBox_BS_Station
                .DataSource = dt_stn
                .ValueMember = "station_id"
                .DisplayMember = "station_name"
                .SelectedIndex = -1
            End With

        Else

            Me.ListBox_BS_Station.Items.Add("No match found")
            Me.ListBox_BS_Station.Enabled = False

        End If

        AddHandler ListBox_BS_Station.SelectedIndexChanged, AddressOf ListBox_BS_Station_SelectedIndexChanged


    End Sub

    Public Sub BS_refresh_stations(ByVal sys As String, ByVal stn As String)

        RemoveHandler ListBox_BS_Station.SelectedIndexChanged, AddressOf ListBox_BS_Station_SelectedIndexChanged


        Dim sql As String = "select * from stations where system_id = " + sys + " and station_name like '%" + stn + "%' and station_visible=1 order by station_name"
        dt_stn = dbe.SQLStringtoDatatable(sql)

        Me.ListBox_BS_Station.DataSource = Nothing
        Me.ListBox_BS_Station.Items.Clear()

        If dt_stn.Rows.Count > 0 Then

            Me.ListBox_BS_Station.Enabled = True

            With Me.ListBox_BS_Station
                .DataSource = dt_stn
                .ValueMember = "station_id"
                .DisplayMember = "station_name"
                .SelectedIndex = -1
            End With

        Else

            Me.ListBox_BS_Station.Items.Add("No match found")
            Me.ListBox_BS_Station.Enabled = False

        End If

        AddHandler ListBox_BS_Station.SelectedIndexChanged, AddressOf ListBox_BS_Station_SelectedIndexChanged


    End Sub

    Private Sub RadioButton_BS_Market_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton_BS_Market.CheckedChanged
        If Me.RadioButton_BS_Market.Checked Then Me.RadioButton_BS_BMarket.Checked = False
        Me.CheckBox_BS_ExcNonMarket.Enabled = True
        Me.Button_BS_ClearFilters.PerformClick()
    End Sub
    Private Sub RadioButton_BS_BMarket_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton_BS_BMarket.CheckedChanged
        If Me.RadioButton_BS_BMarket.Checked Then Me.RadioButton_BS_Market.Checked = False
        Me.CheckBox_BS_ExcNonMarket.Enabled = False
        Me.Button_BS_ClearFilters.PerformClick()
    End Sub

    Private Sub CheckBox_BS_ExcNonMarket_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox_BS_ExcNonMarket.CheckedChanged
        Me.Button_BS_ClearFilters.PerformClick()
    End Sub

    '       DGV controls
    Public Sub refresh_DGV_Commodities()


        Dim sql As String = "select upper(item_class) as 'item_class',item_name,sell_price,buy_price,qty_demand,qty_avail,last_updated from CommPrices_vw where system_name='" + Me.Label_BS_System.Text.ToString + "' and station_name='" + Me.Label_BS_Station.Text.ToString + "' order by item_class,item_name"
        dt_commPrices = dbe.SQLStringtoDatatable(sql)
        If dt_commPrices.Rows.Count > 0 Then
            Dim dgv As DataGridView = Me.DataGridView_BS_Commodities
            dgv.Rows.Clear()
            With dgv
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(1).DefaultCellStyle.Format = "###,###,###"
                .Columns(2).DefaultCellStyle.Format = "###,###,###"
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(3).DefaultCellStyle.Format = "###,###,###"
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(4).DefaultCellStyle.Format = "###,###,###"
                .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            Dim itmClass As String = ""
            For Each itm As DataRow In dt_commPrices.Rows
                Dim lu As Object
                If itm.Item(6).ToString = "" Then lu = Nothing Else lu = CDate(itm.Item(6))
                If itmClass = itm.Item(0).ToString Then
                    dgv.Rows.Add(itm.Item(1).ToString, itm.Item(2), itm.Item(3), itm.Item(4), itm.Item(5), lu)
                Else
                    Dim dr As New DataGridViewRow
                    Dim dc As DataGridViewCell = New DataGridViewTextBoxCell
                    dr.DefaultCellStyle.BackColor = SystemColors.ActiveCaption
                    dr.DefaultCellStyle.ForeColor = Color.Black
                    dr.DefaultCellStyle.SelectionBackColor = SystemColors.ActiveCaption
                    dr.DefaultCellStyle.SelectionForeColor = Color.Black
                    dr.DefaultCellStyle.Font = euro9Bold
                    dr.Cells.Add(dc)
                    dr.Cells(0).Value = itm.Item(0)
                    dr.ReadOnly = True
                    dgv.Rows.Add(dr)
                    itmClass = itm.Item(0).ToString
                    dgv.Rows.Add(itm.Item(1).ToString, itm.Item(2), itm.Item(3), itm.Item(4), itm.Item(5), lu)
                End If
            Next

        Else

            With Me.DataGridView_BS_Commodities
                .DataSource = Nothing
                .Rows.Clear()
                .Rows.Add("No records found.")
            End With
        End If
    End Sub

    Private Sub DataGridView_BS_Commodities_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles DataGridView_BS_Commodities.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.DataGridView_BS_Commodities.Rows(Me.DataGridView_BS_Commodities.SelectedRows(0).Index + 1).Cells(Me.DataGridView_BS_Commodities.SelectedColumns(0).Index).Selected = True
        ElseIf e.KeyCode = Keys.Tab Then
            Me.DataGridView_BS_Commodities.Rows(Me.DataGridView_BS_Commodities.SelectedRows(0).Index).Cells(Me.DataGridView_BS_Commodities.SelectedColumns(0).Index + 1).Selected = True
        ElseIf e.KeyCode = Keys.Delete Then
            DeleteItemToolStripMenuItem.PerformClick()
        End If
    End Sub

    Private Sub DataGridView_BS_Commodities_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles DataGridView_BS_Commodities.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then Me.ContextMenuStrip_BS_DGV.Show(Windows.Forms.Cursor.Position)
    End Sub

    Private Sub DataGridView_BS_Commodities_MouseEnter(sender As Object, e As System.EventArgs) Handles DataGridView_BS_Commodities.MouseEnter
        Me.DataGridView_BS_Commodities.Focus()
    End Sub

    Private Sub DataGridView_BS_Commodities_SelectionChanged(sender As Object, e As System.EventArgs) Handles DataGridView_BS_Commodities.SelectionChanged
        Me.DataGridView_BS_Commodities.EndEdit()
    End Sub

    Private Sub BS_DeleteItemToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DeleteItemToolStripMenuItem.Click
        Dim msg As String = ""
        If Me.DataGridView_BS_Commodities.SelectedCells.Count = 1 Then
            msg = "Are you sure you wish to remove this item from this station?"
        ElseIf Me.DataGridView_BS_Commodities.SelectedCells.Count > 1 Then
            msg = "Are you sure you wish to remove these " + Me.DataGridView_BS_Commodities.SelectedCells.Count.ToString + " items from this station?"
        Else
            Return
        End If

        If MsgBox(msg, MsgBoxStyle.OkCancel, "Item Deletion") = MsgBoxResult.Ok Then
            Dim sql As String = "select system_id from Starsystems where system_name = '" + Me.Label_BS_System.Text.ToString + "'"
            Dim dt As DataTable = dbe.SQLStringtoDatatable(sql)
            Dim sys_id As String = dt.Rows(0).Item(0).ToString

            sql = "select station_id from Stations where station_name = '" + Me.Label_BS_Station.Text.ToString + "'"
            dt = dbe.SQLStringtoDatatable(sql)
            Dim stn_id As String = dt.Rows(0).Item(0).ToString

            For Each itm As DataGridViewCell In Me.DataGridView_BS_Commodities.SelectedCells
                Dim item_name As String = Me.DataGridView_BS_Commodities.Rows(itm.RowIndex).Cells(0).Value
                sql = "select item_id from Items where item_name = '" + item_name + "'"
                dt = dbe.SQLStringtoDatatable(sql)
                If dt.Rows.Count > 0 Then
                    Dim item_id As String = dt.Rows(0).Item(0).ToString
                    sql = "delete from CommPrices where item_id = " + item_id + " and station_id = " + stn_id + " and system_id = " + sys_id
                    dbe.SQL_String_Execute(sql)
                End If
            Next
        End If
        refresh_DGV_Commodities()

    End Sub


    '       Commit Changes
    Private Sub Button_BS_Save_Click(sender As System.Object, e As System.EventArgs) Handles Button_BS_Save.Click
        update_db("Commodities")
    End Sub



    '   *** BROWSE COMMODITY ***

    Public Sub refresh_BC_Item_List()
        RemoveHandler ListView_BC_Commodities.SelectedIndexChanged, AddressOf ListView_BC_Commodities_SelectedIndexChanged

        Dim sql As String = "select item_id,item_name,upper(item_class) as 'item_class' from Items where item_visible=1 order by item_class,item_name"
        Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)


        With Me.ListView_BC_Commodities
            .Items.Clear()
            For Each itm As DataRow In dt.Rows
                Dim grpExist As Boolean = False
                For Each grp As ListViewGroup In .Groups
                    If itm.Item(2).ToString = grp.Header Then grpExist = True
                Next
                If Not grpExist Then .Groups.Add(New ListViewGroup(key:=itm.Item(2).ToString, headerText:=itm.Item(2).ToString))

                Dim lvi As ListViewItem = New ListViewItem(Text:=itm.Item(1).ToString, group:=.Groups(itm.Item(2).ToString))
                .Items.Add(lvi)
            Next

        End With

        AddHandler ListView_BC_Commodities.SelectedIndexChanged, AddressOf ListView_BC_Commodities_SelectedIndexChanged
    End Sub

    Private Sub ListView_BC_Commodities_MouseEnter(sender As Object, e As System.EventArgs) Handles ListView_BC_Commodities.MouseEnter
        Me.ListView_BC_Commodities.Focus()
    End Sub

    Private Sub ListView_BC_Commodities_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ListView_BC_Commodities.SelectedIndexChanged
        If Me.ListView_BC_Commodities.SelectedIndices.Count > 0 Then

            Dim sql As String = "select system_name,station_name,item_name,qty_demand,sell_price,last_updated from demand_vw where item_name = '" + Me.ListView_BC_Commodities.SelectedItems(0).Text.ToString + "'"
            Dim dt2 As Data.DataTable = dbe.SQLStringtoDatatable(sql)


            With Me.DataGridView_BC_Demand
                .AutoGenerateColumns = False
                .DataSource = dt2
                .Columns(0).DataPropertyName = dt2.Columns(0).ColumnName
                .Columns(1).DataPropertyName = dt2.Columns(1).ColumnName
                .Columns(2).DataPropertyName = dt2.Columns(2).ColumnName
                .Columns(3).DataPropertyName = dt2.Columns(3).ColumnName
                .Columns(4).DataPropertyName = dt2.Columns(4).ColumnName
                .Columns(5).DataPropertyName = dt2.Columns(5).ColumnName
            End With

            sql = "select system_name,station_name,item_name,qty_avail,buy_price,last_updated from supply_vw where item_name = '" + Me.ListView_BC_Commodities.SelectedItems(0).Text.ToString + "'"
            Dim dt3 As Data.DataTable = dbe.SQLStringtoDatatable(sql)

            With Me.DataGridView_BC_Supply
                .AutoGenerateColumns = False
                .DataSource = dt3
                .Columns(0).DataPropertyName = dt3.Columns(0).ColumnName
                .Columns(1).DataPropertyName = dt3.Columns(1).ColumnName
                .Columns(2).DataPropertyName = dt3.Columns(2).ColumnName
                .Columns(3).DataPropertyName = dt3.Columns(3).ColumnName
                .Columns(4).DataPropertyName = dt3.Columns(4).ColumnName
                .Columns(5).DataPropertyName = dt3.Columns(5).ColumnName
            End With

        End If

    End Sub

    Private Sub Button_BC_ItemListRefresh_Click(sender As System.Object, e As System.EventArgs) Handles Button_BC_ItemListRefresh.Click
        refresh_BC_Item_List()
    End Sub



    '   *** TRADE FROM TO ***

    Public Sub FT_Load_SystemStationCombobox(Optional ByVal loadEvent As Boolean = False)
        If Not loadEvent Then
            RemoveHandler ComboBox_FT_From_Stat.SelectedValueChanged, AddressOf ComboBox_FT_From_Stat_SelectedIndexChanged
            RemoveHandler ComboBox_FT_From_Sys.SelectedValueChanged, AddressOf ComboBox_FT_From_Sys_SelectedIndexChanged
            RemoveHandler ComboBox_FT_To_Stat.SelectedValueChanged, AddressOf ComboBox_FT_To_Stat_SelectedIndexChanged
            RemoveHandler ComboBox_FT_To_Sys.SelectedValueChanged, AddressOf ComboBox_FT_To_Sys_SelectedIndexChanged
        End If

        Me.ComboBox_FT_From_Stat.DataSource = Nothing
        Me.ComboBox_FT_From_Sys.DataSource = Nothing
        Me.ComboBox_FT_To_Stat.DataSource = Nothing
        Me.ComboBox_FT_To_Sys.DataSource = Nothing

        Dim sql As String = "select system_id,system_name from Starsystems where system_visible=1 order by system_name"
        Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)
        Dim dt2 As Data.DataTable = dbe.SQLStringtoDatatable(sql)

        sql = "select system_id,station_id,station_name from Stations where market=1 and station_visible=1 order by station_name"
        Dim dt3 As Data.DataTable = dbe.SQLStringtoDatatable(sql)
        Dim dt4 As Data.DataTable = dbe.SQLStringtoDatatable(sql)

        With Me.ComboBox_FT_From_Stat
            .DataSource = dt3
            .ValueMember = "station_id"
            .DisplayMember = "station_name"
            .SelectedIndex = -1
        End With

        With Me.ComboBox_FT_To_Stat
            .DataSource = dt4
            .ValueMember = "station_id"
            .DisplayMember = "station_name"
            .SelectedIndex = -1
        End With

        With Me.ComboBox_FT_From_Sys
            .DataSource = dt
            .ValueMember = "system_name"
            .DisplayMember = "system_name"
            .SelectedIndex = -1
        End With

        With Me.ComboBox_FT_To_Sys
            .DataSource = dt2
            .ValueMember = "system_name"
            .DisplayMember = "system_name"
            .SelectedIndex = -1
        End With

        AddHandler ComboBox_FT_From_Stat.SelectedValueChanged, AddressOf ComboBox_FT_From_Stat_SelectedIndexChanged
        AddHandler ComboBox_FT_From_Sys.SelectedValueChanged, AddressOf ComboBox_FT_From_Sys_SelectedIndexChanged
        AddHandler ComboBox_FT_To_Stat.SelectedValueChanged, AddressOf ComboBox_FT_To_Stat_SelectedIndexChanged
        AddHandler ComboBox_FT_To_Sys.SelectedValueChanged, AddressOf ComboBox_FT_To_Sys_SelectedIndexChanged

    End Sub

    Private Sub Button_FT_Reset_Filters_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_Reset_Filters.Click
        FT_Load_SystemStationCombobox()
    End Sub

    Private Sub Button_FT_Calculate_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_Calculate.Click
        Dim sta_from As String
        Dim sys_to As String
        Dim sta_to As String
        Dim cargo As String
        Dim wallet As String



        If Me.ComboBox_FT_From_Sys.SelectedValue = "" Then
            MsgBox("'From System' must be selected.")
        Else
            If Me.ComboBox_FT_From_Stat.SelectedValue = "" And Me.ComboBox_FT_To_Sys.SelectedValue = "" Then
                MsgBox("At least 'From System' and either 'From Station' or 'To System' must be specified.")
            Else
                If Me.ComboBox_FT_From_Stat.SelectedValue = "" Then
                    sta_from = ""
                Else
                    sta_from = " and from_station = '" + Me.ComboBox_FT_From_Stat.SelectedValue + "' "
                End If
                If Me.ComboBox_FT_To_Sys.SelectedValue = "" Then
                    sys_to = ""
                Else
                    sys_to = " and to_system = '" + Me.ComboBox_FT_To_Sys.SelectedValue + "' "
                End If
                If Me.ComboBox_FT_To_Stat.SelectedValue = "" Then
                    sta_to = ""
                Else
                    sta_to = " and to_station = '" + Me.ComboBox_FT_To_Stat.SelectedValue + "' "
                End If
                If Me.CheckBox_FT_Cargo.Checked Then
                    cargo = ",(buy_price * " + Me.NUD_FT_Cargo.Value.ToString + ") as 'Total Buy Price',((sell_price-buy_price)*" + Me.NUD_FT_Cargo.Value.ToString + ") as 'Total Profit'"
                Else
                    cargo = ""
                End If
                If Me.CheckBox_FT_Wallet.Checked Then
                    wallet = " and (buy_price * " + Me.NUD_FT_Cargo.Value.ToString + ") < " + Me.NUD_FT_Wallet.Value.ToString + " "
                Else
                    wallet = ""
                End If

                Dim sql As String = "select from_station,to_system,to_station,item_name,sell_price,buy_price,qty_demand,qty_avail,sell_last_upd,buy_last_upd,ppu " + cargo + " from trade_from_to_vw where from_system='" + Me.ComboBox_FT_From_Sys.SelectedValue + "' " + sys_to + sta_from + sta_to + wallet + " order by ppu desc,from_station,to_system,to_station"
                Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)

                Me.DataGridView_FT_Routes.DataSource = Nothing
                Me.DataGridView_FT_Routes.Rows.Clear()
                Me.DataGridView_FT_Routes.Columns.Clear()

                If dt.Rows.Count > 0 Then
                    With Me.DataGridView_FT_Routes
                        .DataSource = dt
                        .Columns(0).HeaderText = "FROM STATION"
                        .Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        .Columns(0).Visible = False
                        If sta_from = "" Then .Columns(0).Visible = True
                        .Columns(1).HeaderText = "TO SYSTEM"
                        .Columns(1).Visible = False
                        .Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        If sys_to = "" Then .Columns(1).Visible = True
                        .Columns(2).HeaderText = "TO STATION"
                        .Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        .Columns(2).Visible = False
                        If sta_to = "" Then .Columns(2).Visible = True
                        .Columns(3).HeaderText = "COMMODITY"
                        .Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        .Columns(4).HeaderText = "SELL"
                        .Columns(4).Width = 50
                        .Columns(4).DefaultCellStyle.Format = "###,###,###"
                        .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(5).HeaderText = "BUY"
                        .Columns(5).Width = 50
                        .Columns(5).DefaultCellStyle.Format = "###,###,###"
                        .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(6).HeaderText = "DEMAND"
                        .Columns(6).Width = 60
                        .Columns(6).DefaultCellStyle.Format = "###,###,###"
                        .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(7).HeaderText = "SUPPLY"
                        .Columns(7).Width = 60
                        .Columns(7).DefaultCellStyle.Format = "###,###,###"
                        .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(8).HeaderText = "SELL PRICE DATE"
                        .Columns(8).Width = 130
                        .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(9).HeaderText = "BUY PRICE DATE"
                        .Columns(9).Width = 130
                        .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(10).HeaderText = "PROFIT PER UNIT"
                        .Columns(10).Width = 60
                        .Columns(10).DefaultCellStyle.Format = "###,###,###"
                        .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        If Me.CheckBox_FT_Cargo.Checked = True Then
                            .Columns(11).HeaderText = "TOTAL BUY PRICE"
                            .Columns(11).Width = 70
                            .Columns(11).DefaultCellStyle.Format = "###,###,###"
                            .Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                            .Columns(12).HeaderText = "TOTAL PROFIT"
                            .Columns(12).Width = 60
                            .Columns(12).DefaultCellStyle.Format = "###,###,###"
                            .Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        End If

                    End With
                Else
                    With Me.DataGridView_FT_Routes
                        .Columns.Add("Info", "Information")
                        .Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        .Rows.Add("No valid routes found!")
                    End With
                End If
            End If
        End If

    End Sub

    Private Sub Button_FT_SwapStaSys_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_SwapStaSys.Click
        Dim fr_sys As Integer
        Dim fr_sta As Integer
        Dim to_sys As Integer
        Dim to_sta As Integer

        fr_sys = Me.ComboBox_FT_From_Sys.SelectedIndex
        fr_sta = Me.ComboBox_FT_From_Stat.SelectedIndex
        to_sys = Me.ComboBox_FT_To_Sys.SelectedIndex
        to_sta = Me.ComboBox_FT_To_Stat.SelectedIndex

        Me.ComboBox_FT_From_Sys.SelectedIndex = to_sys
        Me.ComboBox_FT_From_Stat.SelectedIndex = to_sta
        Me.ComboBox_FT_To_Sys.SelectedIndex = fr_sys
        Me.ComboBox_FT_To_Stat.SelectedIndex = fr_sta

    End Sub

    Private Sub CheckBox_FT_Cargo_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox_FT_Cargo.CheckedChanged
        If Me.CheckBox_FT_Cargo.Checked Then
            Me.NUD_FT_Cargo.Enabled = True
        Else
            Me.NUD_FT_Cargo.Enabled = False
        End If
    End Sub

    Private Sub CheckBox_FT_Wallet_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox_FT_Wallet.CheckedChanged
        If Me.CheckBox_FT_Wallet.Checked Then
            Me.NUD_FT_Wallet.Enabled = True
        Else
            Me.NUD_FT_Wallet.Enabled = False
        End If
    End Sub

    Private Sub Button_FT_Cargo10_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_Cargo10.Click
        Me.NUD_FT_Cargo.Value = Me.NUD_FT_Cargo.Value + 10
    End Sub
    Private Sub Button_FT_Cargo50_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_Cargo50.Click
        Me.NUD_FT_Cargo.Value = Me.NUD_FT_Cargo.Value + 50
    End Sub
    Private Sub Button_FT_Cargo100_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_Cargo100.Click
        Me.NUD_FT_Cargo.Value = Me.NUD_FT_Cargo.Value + 100
    End Sub
    Private Sub Button_FT_Wallet1k_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_Wallet1k.Click
        Me.NUD_FT_Wallet.Value = Me.NUD_FT_Wallet.Value + 1000
    End Sub
    Private Sub Button_FT_Wallet10k_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_Wallet10k.Click
        Me.NUD_FT_Wallet.Value = Me.NUD_FT_Wallet.Value + 10000
    End Sub
    Private Sub Button_FT_Wallet100k_Click(sender As System.Object, e As System.EventArgs) Handles Button_FT_Wallet100k.Click
        Me.NUD_FT_Wallet.Value = Me.NUD_FT_Wallet.Value + 100000
    End Sub

    Sub ComboBox_FT_From_Sys_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        If Me.ComboBox_FT_From_Sys.SelectedIndex > -1 Then
            RemoveHandler ComboBox_FT_From_Stat.SelectedValueChanged, AddressOf ComboBox_FT_From_Stat_SelectedIndexChanged

            Dim sql As String = "select station_name from Stations where system_id in (select system_id from Starsystems where system_name = '" + Me.ComboBox_FT_From_Sys.SelectedValue.ToString + "')"
            Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)

            With Me.ComboBox_FT_From_Stat
                .DataSource = dt
                .ValueMember = "station_name"
                .DisplayMember = "station_name"
                .SelectedIndex = -1
            End With

            AddHandler ComboBox_FT_From_Stat.SelectedValueChanged, AddressOf ComboBox_FT_From_Stat_SelectedIndexChanged
        End If
    End Sub
    Sub ComboBox_FT_From_Stat_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        If Me.ComboBox_FT_From_Stat.SelectedIndex > -1 And Me.ComboBox_FT_From_Sys.SelectedIndex = -1 Then
            RemoveHandler ComboBox_FT_From_Sys.SelectedValueChanged, AddressOf ComboBox_FT_From_Sys_SelectedIndexChanged

            Dim sql As String = "select system_name from Starsystems where system_id in (select system_id from Stations where station_name = '" + Me.ComboBox_FT_From_Stat.SelectedValue.ToString + "')"
            Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)

            With Me.ComboBox_FT_From_Sys
                .DataSource = dt
                .ValueMember = "system_name"
                .DisplayMember = "system_name"
                .SelectedIndex = -1
            End With

            AddHandler ComboBox_FT_From_Sys.SelectedValueChanged, AddressOf ComboBox_FT_From_Sys_SelectedIndexChanged
        End If
    End Sub

    Sub ComboBox_FT_To_Sys_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        If Me.ComboBox_FT_To_Sys.SelectedIndex > -1 Then
            RemoveHandler ComboBox_FT_To_Stat.SelectedValueChanged, AddressOf ComboBox_FT_To_Stat_SelectedIndexChanged

            Dim sql As String = "select station_name from Stations where system_id in (select system_id from Starsystems where system_name = '" + Me.ComboBox_FT_To_Sys.SelectedValue.ToString + "')"
            Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)

            With Me.ComboBox_FT_To_Stat
                .DataSource = dt
                .ValueMember = "station_name"
                .DisplayMember = "station_name"
                .SelectedIndex = -1
            End With

            AddHandler ComboBox_FT_To_Stat.SelectedValueChanged, AddressOf ComboBox_FT_To_Stat_SelectedIndexChanged
        End If
    End Sub
    Sub ComboBox_FT_To_Stat_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        If Me.ComboBox_FT_To_Stat.SelectedIndex > -1 And Me.ComboBox_FT_To_Sys.SelectedIndex = -1 Then
            RemoveHandler ComboBox_FT_To_Sys.SelectedValueChanged, AddressOf ComboBox_FT_To_Sys_SelectedIndexChanged

            Dim sql As String = "select system_name from Starsystems where system_id in (select system_id from Stations where station_name = '" + Me.ComboBox_FT_To_Stat.SelectedValue.ToString + "')"
            Dim dt As Data.DataTable = dbe.SQLStringtoDatatable(sql)

            With Me.ComboBox_FT_To_Sys
                .DataSource = dt
                .ValueMember = "system_name"
                .DisplayMember = "system_name"
                .SelectedIndex = -1
            End With


            AddHandler ComboBox_FT_To_Sys.SelectedValueChanged, AddressOf ComboBox_FT_To_Sys_SelectedIndexChanged
        End If
    End Sub




    '   *** MANAGE COMMODITIES ***
    Dim MC_changed As Boolean = False
    Dim dt_MC_ItemNameList As DataTable
    Dim dt_MC_ItemDetails As DataTable

    Public Sub Refresh_MC_ItemList()
        RemoveHandler ListView_MC_Commodities.SelectedIndexChanged, AddressOf ListView_MC_Commodities_SelectedIndexChanged

        Dim sql As String = "select item_id,item_name,upper(item_class) as 'item_class' from Items order by item_class, item_name"
        dt_MC_ItemNameList = dbe.SQLStringtoDatatable(sql)


        With Me.ListView_MC_Commodities
            .Items.Clear()
            For Each itm As DataRow In dt_MC_ItemNameList.Rows
                Dim grpExist As Boolean = False
                For Each grp As ListViewGroup In .Groups
                    If itm.Item(2).ToString = grp.Header Then grpExist = True
                Next
                If Not grpExist Then .Groups.Add(New ListViewGroup(key:=itm.Item(2).ToString, headerText:=itm.Item(2).ToString))

                Dim lvi As ListViewItem = New ListViewItem(Text:=itm.Item(1).ToString, group:=.Groups(itm.Item(2).ToString))
                .Items.Add(lvi)
            Next

        End With


        AddHandler ListView_MC_Commodities.SelectedIndexChanged, AddressOf ListView_MC_Commodities_SelectedIndexChanged
    End Sub

    Private Sub ListView_MC_Commodities_MouseEnter(sender As Object, e As System.EventArgs) Handles ListView_MC_Commodities.MouseEnter
        Me.ListView_MC_Commodities.Focus()
    End Sub

    Private Sub ListView_MC_Commodities_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListView_MC_Commodities.SelectedIndexChanged
        If Me.ListView_MC_Commodities.SelectedIndices.Count > 0 Then

            Dim sql As String = "select item_id, item_name, upper(item_class) as 'item_class', item_visible from Items where item_name = '" + Me.ListView_MC_Commodities.SelectedItems(0).Text.ToString + "'"
            dt_MC_ItemDetails = dbe.SQLStringtoDatatable(sql)
            Me.Label_MC_ItemInvisible.Visible = False

            If dt_MC_ItemDetails.Rows.Count > 0 Then
                Me.Label_MC_ItemID.Text = dt_MC_ItemDetails.Rows(0).Item(0).ToString
                Me.TextBox_MC_ItemName.Text = Trim(dt_MC_ItemDetails.Rows(0).Item(1).ToString)
                Me.TextBox_MC_ItemCategory.Text = Trim(dt_MC_ItemDetails.Rows(0).Item(2).ToString)
                If dt_MC_ItemDetails.Rows(0).Item(3) = 0 Then Me.Label_MC_ItemInvisible.Visible = True
            Else
                MsgBox("No matching commodity data found!")
            End If

        End If

    End Sub

    Private Sub Button_MC_RefreshList_Click(sender As System.Object, e As System.EventArgs) Handles Button_MC_RefreshList.Click
        Refresh_MC_ItemList()
    End Sub

    Private Sub Button_MC_Save_Click(sender As System.Object, e As System.EventArgs) Handles Button_MC_Save.Click
        If MC_changed Then
            Dim sql As String = "select item_id from Items where item_id = " + Me.Label_MC_ItemID.Text
            Dim dt As DataTable = dbe.SQLStringtoDatatable(sql)
            If dt.Rows.Count > 0 Then
                If MsgBox("Are you sure you wish to change this item?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    sql = "update Items set item_name = '" + Trim(Me.TextBox_MC_ItemName.Text).ToUpper + "', item_class='" + Trim(Me.TextBox_MC_ItemCategory.Text).ToUpper + "' where item_id = " + Me.Label_MC_ItemID.Text
                    dbe.SQL_String_Execute(sql)
                End If
            Else
                If MsgBox("Are you sure you wish to create this item?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    sql = "Insert into Items (item_name,item_class) values ('" + Trim(Me.TextBox_MC_ItemName.Text).ToUpper + "','" + Trim(Me.TextBox_MC_ItemCategory.Text).ToUpper + "')"
                    dbe.SQL_String_Execute(sql)
                End If
            End If

            Refresh_MC_ItemList()

        Else
            MsgBox("No changes to save.")
        End If

    End Sub

    Private Sub Button_MC_DeleteItem_Click(sender As System.Object, e As System.EventArgs) Handles Button_MC_DeleteItem.Click
        If MsgBox("WARNING! Deleting this item will remove all price data associated with it. Are you sure you wish to delete this item?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            'delete prices
            Dim sql As String = "delete from Comm_Prices where item_id = " + Me.Label_MC_ItemID.Text
            dbe.SQL_String_Execute(sql)

            'delete item
            sql = "delete from Items where item_id = " + Me.Label_MC_ItemID.Text
            dbe.SQL_String_Execute(sql)

            Refresh_MC_ItemList()
        End If
    End Sub

    Private Sub Button_MC_NewItem_Click(sender As System.Object, e As System.EventArgs) Handles Button_MC_NewItem.Click
        dt_MC_ItemDetails = Nothing

        Dim sql As String = "select max(item_id) as 'item_id' from Items"
        Dim dt As DataTable = dbe.SQLStringtoDatatable(sql)

        Dim newNum As Integer = 1
        If dt.Rows.Count > 0 Then newNum = CInt(dt.Rows(0).Item(0).ToString) + 1

        Refresh_MC_ItemList()

        Me.Label_MC_ItemID.Text = newNum.ToString
        Me.TextBox_MC_ItemName.Focus()
        MC_changed = True
    End Sub

    Private Sub TextBox_MC_ItemName_TextChanged(sender As Object, e As System.EventArgs) Handles TextBox_MC_ItemName.TextChanged
        If Me.TextBox_MC_ItemName.Text.ToUpper = Trim(dt_MC_ItemDetails.Rows(0).Item(1).ToString) Then
            MC_changed = False
        Else
            MC_changed = True
        End If
    End Sub
    Private Sub TextBox_MC_ItemCategory_TextChanged(sender As Object, e As System.EventArgs) Handles TextBox_MC_ItemCategory.TextChanged
        If Me.TextBox_MC_ItemCategory.Text.ToUpper = Trim(dt_MC_ItemDetails.Rows(0).Item(2).ToString) Then
            MC_changed = False
        Else
            MC_changed = True
        End If
    End Sub




 

End Class
