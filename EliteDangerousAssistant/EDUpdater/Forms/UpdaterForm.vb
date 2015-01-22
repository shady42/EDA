Imports System.IO
Imports System.Data.SQLite
Imports System.Diagnostics
Imports Microsoft.Office.Interop.Word


Public Class UpdaterForm

    Dim appName As String = "EliteDangerousAssistant"
    Dim dbName As String = "EDData"


    ''''''''''''''''''''''''''''''''''
    '''' exe Updates              ''''
    '''' altered for 0.3.0        ''''
    '''' altered for 0.3.1        ''''
    '''' altered for 0.3.2        ''''
    '''' altered for 0.4.0        ''''
    '''' altered for 0.4.1        ''''
    '''' altered for 0.4.2        ''''
    '''' altered for 0.4.3        ''''
    '''' altered for 0.5.0        ''''
    ''''''''''''''''''''''''''''''''''
    Dim _exeUpdate As Boolean = True

    ''''''''''''''''''''''''''''''''''
    '''' SQL Updates              ''''
    '''' none up to 0.2.1         ''''
    '''' 1. update 0.2.1 to 0.3.0 ''''
    '''' none for 0.3.1           ''''
    '''' none for 0.3.2           ''''
    '''' 2. update 0.3.0 to 0.4.0 ''''
    '''' none for 0.4.1           ''''
    '''' none for 0.4.2           ''''
    '''' none for 0.4.3           ''''
    '''' 3. update 0.4.3 to 0.5.0 ''''
    ''''''''''''''''''''''''''''''''''
    Dim _sqlUpdate As Boolean = True

    ''''''''''''''''''''''''''''''''''
    '''' Traineddata Update       ''''
    '''' none up to 0.5.0         ''''
    ''''''''''''''''''''''''''''''''''
    Dim _OCRlangUpdate As Boolean = False


    Dim exeBackup As Boolean = False
    Dim exeNewCreated As Boolean = False
    Dim dbBackup As Boolean = False
    Dim dbNewCreated As Boolean = False
    Dim fileVersion As String
    Dim sup As String
    Private Sub UpdaterForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Cursor = Cursors.WaitCursor

            sup = System.Windows.Forms.Application.StartupPath

            load_change_log()

            If File.Exists(sup + "\" + appName + ".exe") And File.Exists(sup + "\" + dbName) Then
                Me.Label_Status.Text = "Application files found"
                Me.ProgressBar_Update.PerformStep()

                Dim p As Process()
                p = Process.GetProcessesByName(appName)
                If p.Count > 0 Then
                    Throw New Exception("Update Failed!  Please close Elite Dangerous Assistant application and try again.")
                Else
                    Me.Label_Status.Text = "Beginning Update..."
                    Me.ProgressBar_Update.PerformStep()

                    'find existing file version
                    Dim fv As FileVersionInfo = FileVersionInfo.GetVersionInfo(sup + "\" + appName + ".exe")
                    fileVersion = fv.FileVersion


                    '*** EXECUTABLE UPDATE ***
                    If _exeUpdate Then
                        'back up 
                        Me.Label_Status.Text = "Backing up application files"
                        File.Move(sup + "\" + appName + ".exe", sup + "\" + appName + ".tmp")
                        exeBackup = True
                        Me.ProgressBar_Update.PerformStep()

                        'create new
                        Me.Label_Status.Text = "Writing new file - EliteDangerousAssistant.exe"
                        File.WriteAllBytes(sup + "\" + appName + ".exe", My.Resources.EliteDangerousAssistant)
                        exeNewCreated = True
                        Me.ProgressBar_Update.PerformStep()
                    End If


                    '*** DB UPDATE ***
                    If _sqlUpdate Then
                        'back up 
                        Me.Label_Status.Text = "Backing up database files"
                        File.Copy(sup + "\" + dbName, sup + "\" + dbName + "TMP")
                        dbBackup = True
                        dbNewCreated = True
                        Me.ProgressBar_Update.PerformStep()

                        'alter db to current version
                        db_updates()

                    End If


                    '*** Miscellaneous ***
                    ' For < 0.4.0
                    If CInt(fileVersion.Substring(0, 1)) > 0 Or CInt(fileVersion.Substring(2, 1)) > 3 Or (CInt(fileVersion.Substring(2, 1)) = 3 And CInt(fileVersion.Substring(4, 1)) > 2) Then
                    Else
                        Me.Label_Status.Text = "Writing new file - Eurostile.ttf"
                        File.WriteAllBytes(sup + "\Eurostile.ttf", My.Resources.Eurostile)
                        Me.ProgressBar_Update.PerformStep()
                    End If


                End If
            Else
                Throw New Exception("Update Failed!  Please ensure updater is placed in correct folder (default is C:\Program Files (x86)\Shady Enterprises Ltd\EliteDangerousAssistant)")
            End If

            'delete temp files
            Me.Label_Status.Text = "Deleting any temporary files"
            If _exeUpdate Then File.Delete(sup + "\" + appName + ".tmp")
            Me.ProgressBar_Update.PerformStep()
            If _sqlUpdate Then File.Delete(sup + "\" + dbName + "TMP")
            Me.ProgressBar_Update.PerformStep()

            Me.Label_Status.Text = "Update Complete! Close updater when ready and enjoy the update!"
            Me.ProgressBar_Update.Value = 100

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            rollback_update()
            Me.Close()
        Finally

        End Try

    End Sub

    Sub db_updates()
        'create connection to old data file
        My.Settings.DBFile = sup + "\" + dbName
        My.Settings.SQLiteConn = "Data Source=" + My.Settings.DBFile + ";Version=3;New=False;Compress=True"
        Dim dbe As New SQLiteDBEngine(My.Settings.SQLiteConn)

        Dim sql As String
        Select Case fileVersion.Substring(0, 3)
            Case "0.2"
                '*** < 1.> ***
                'backup Starsystems/Stations/Items
                sql = "create table temp_starsystems as select * from starsystems; create table temp_stations as select * from stations; create table temp_items as select * from items; create table temp_CommPrices as select * from CommPrices "
                dbe.SQL_String_Execute(sql)

                'create updated tables
                'Starsystems
                sql = "drop table Starsystems; CREATE TABLE Starsystems (system_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, system_name TEXT NOT NULL, allegiance TEXT NOT NULL DEFAULT '', security TEXT NOT NULL DEFAULT '', system_visible INTEGER DEFAULT 1, pos_x INTEGER DEFAULT 0, pos_y INTEGER DEFAULT 0, pos_z INTEGER DEFAULT 0, region TEXT DEFAULT '', constellation TEXT DEFAULT '')"
                dbe.SQL_String_Execute(sql)
                'Stations
                sql = "drop table Stations; CREATE TABLE Stations (station_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, station_name TEXT NOT NULL, system_id INTEGER NOT NULL, market INTEGER NOT NULL DEFAULT 0, blackmarket INTEGER NOT NULL DEFAULT 0, outfitting INTEGER NOT NULL DEFAULT 0, shipyard INTEGER NOT NULL DEFAULT 0, star_dist INTEGER DEFAULT 0, station_visible INTEGER DEFAULT 1)"
                dbe.SQL_String_Execute(sql)
                'Items
                sql = "drop table Items; CREATE TABLE Items ( item_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, item_name TEXT NOT NULL, item_class TEXT NOT NULL, item_visible NUMERIC DEFAULT 1, item_disabled NUMERIC DEFAULT 0)"
                dbe.SQL_String_Execute(sql)
                'CommPrices
                sql = "drop table CommPrices; CREATE TABLE CommPrices (item_id INTEGER NOT NULL, station_id INTEGER NOT NULL, system_id INTEGER NOT NULL, sell_price INTEGER NOT NULL DEFAULT 0, buy_price INTEGER NOT NULL DEFAULT 0, qty_demand INTEGER NOT NULL DEFAULT 0, qty_avail INTEGER NOT NULL DEFAULT 0, last_updated TEXT DEFAULT '', PRIMARY KEY(item_id, station_id, system_id))"
                dbe.SQL_String_Execute(sql)


                'insert stored data from temp tables
                'Starsystems
                sql = "insert into Starsystems (system_id,system_name,allegiance,security) select system_id,system_name,allegiance,security from temp_Starsystems "
                dbe.SQL_String_Execute(sql)
                'Stations
                sql = "insert into Stations (station_id,station_name,system_id,market,blackmarket,outfitting,shipyard,star_dist) select station_id,station_name,system_id,market,blackmarket,outfitting,shipyard,star_dist from temp_Stations "
                dbe.SQL_String_Execute(sql)
                'Items
                sql = "insert into Items (item_id,item_name,item_class) select item_id,item_name,item_class from temp_Items "
                dbe.SQL_String_Execute(sql)
                'CommPrices
                sql = "insert into CommPrices (item_id,station_id,system_id,sell_price,buy_price,qty_demand,qty_avail) select item_id,station_id,system_id,sell_price,buy_price,qty_demand,qty_avail from temp_CommPrices "
                dbe.SQL_String_Execute(sql)

                'delete temporary tables 
                sql = "drop table temp_Starsystems; drop table temp_Stations; drop table temp_Items; drop table temp_CommPrices "
                dbe.SQL_String_Execute(sql)

                'update views
                sql = "drop view CommPrices_vw; CREATE VIEW CommPrices_vw as select t2.item_name,t2.item_class,t4.system_name,t3.station_name,t1.sell_price,t1.buy_price,t1.qty_demand,t1.qty_avail,t1.last_updated from commPrices t1 join Items t2 on t1.item_id = t2.item_id and t2.item_visible=1 join Stations t3 on t1.station_id = t3.station_id and t3.station_visible=1 join Starsystems t4 on t1.system_id = t4.system_id and t4.system_visible=1 "
                dbe.SQL_String_Execute(sql)
                sql = "drop view ShipLocaion_vw; CREATE VIEW [ShipLocation_vw] as select t2.ship_name,t4.system_name,t3.station_name,t1.price from ShipPrices t1 join Ships t2 on t1.ship_id = t2.ship_id join Stations t3 on t1.station_id = t3.station_id join Starsystems t4 on t1.system_id = t4.system_id"
                dbe.SQL_String_Execute(sql)
                sql = "drop view demand_vw; CREATE VIEW [demand_vw] as select t3.system_name,t4.station_name,t2.item_name,t1.qty_demand,t1.sell_price,t1.last_updated from CommPrices t1 join Items t2 on t1.item_id = t2.item_id and t2.item_visible=1 join Starsystems t3 on t1.system_id = t3.system_id and t3.system_visible=1 join Stations t4 on t1.station_id = t4.station_id and t4.station_visible=1 where t1.qty_demand>0 "
                dbe.SQL_String_Execute(sql)
                sql = "drop view supply_vw; CREATE VIEW [supply_vw] as select t3.system_name,t4.station_name,t2.item_name,t1.qty_avail,t1.buy_price,t1.last_updated from commPrices t1 join Items t2 on t1.item_id = t2.item_id and t2.item_visible=1 join Starsystems t3 on t1.system_id = t3.system_id and t3.system_visible=1 join Stations t4 on t1.station_id = t4.station_id and t4.station_visible=1 where t1.qty_avail>0 "
                dbe.SQL_String_Execute(sql)
                sql = "drop view trade_from_to_vw; CREATE VIEW [Trade_From_To_vw] as select t2.system_name as 'from_system',t1.system_name as 'to_system',t2.station_name as 'from_station',t1.station_name as 'to_station',t1.item_name,t1.qty_demand,t1.sell_price,t1.last_updated as 'sell_last_upd',t2.qty_avail,t2.buy_price,t2.last_updated as 'buy_last_upd',(t1.sell_price-t2.buy_price) as 'ppu' from demand_vw t1 join supply_vw t2 on t1.item_name = t2.item_name "
                dbe.SQL_String_Execute(sql)
                '*** < 1.> END ***

                '*** < 2.> ***
                'convert all Item Categories to CAPITALS
                sql = "update Items set item_class = upper(item_class)"
                dbe.SQL_String_Execute(sql)
                '*** < 2.> END ***

                '*** < 3.> ***
                'backup Stations
                sql = "create table temp_stations as select * from stations "
                dbe.SQL_String_Execute(sql)
                'drop/create new table
                sql = "drop table Stations; CREATE TABLE Stations (station_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, station_name TEXT NOT NULL, system_id INTEGER NOT NULL,station_type INTEGER DEFAULT 0, market INTEGER NOT NULL DEFAULT 0, blackmarket INTEGER NOT NULL DEFAULT 0, outfitting INTEGER NOT NULL DEFAULT 0, shipyard INTEGER NOT NULL DEFAULT 0, star_dist INTEGER DEFAULT 0, station_visible INTEGER DEFAULT 1) "
                dbe.SQL_String_Execute(sql)
                'reinsert data
                sql = "insert into Stations (station_id,station_name,system_id,station_type,market,blackmarket,outfitting,shipyard,star_dist) select station_id,station_name,system_id,0,market,blackmarket,outfitting,shipyard,star_dist from temp_Stations "
                dbe.SQL_String_Execute(sql)
                'delete temporary table
                sql = "drop table temp_Stations "
                dbe.SQL_String_Execute(sql)
                '*** < 3.> END ***

            Case "0.3"
                '*** < 2.> ***
                'convert all Item Categories to CAPITALS
                sql = "update Items set item_class = upper(item_class)"
                dbe.SQL_String_Execute(sql)
                '*** < 2.> END ***

                '*** < 3.> ***
                'backup Stations
                sql = "create table temp_stations as select * from stations "
                dbe.SQL_String_Execute(sql)
                'drop/create new table
                sql = "drop table Stations; CREATE TABLE Stations (station_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, station_name TEXT NOT NULL, system_id INTEGER NOT NULL,station_type INTEGER DEFAULT 0, market INTEGER NOT NULL DEFAULT 0, blackmarket INTEGER NOT NULL DEFAULT 0, outfitting INTEGER NOT NULL DEFAULT 0, shipyard INTEGER NOT NULL DEFAULT 0, star_dist INTEGER DEFAULT 0, station_visible INTEGER DEFAULT 1) "
                dbe.SQL_String_Execute(sql)
                'reinsert data
                sql = "insert into Stations (station_id,station_name,system_id,station_type,market,blackmarket,outfitting,shipyard,star_dist) select station_id,station_name,system_id,0,market,blackmarket,outfitting,shipyard,star_dist from temp_Stations "
                dbe.SQL_String_Execute(sql)
                'delete temporary table
                sql = "drop table temp_Stations "
                dbe.SQL_String_Execute(sql)
                '*** < 3.> END ***

            Case "0.4"
                '*** < 2.> ***
                'convert all Item Categories to CAPITALS
                sql = "update Items set item_class = upper(item_class)"
                dbe.SQL_String_Execute(sql)
                '*** < 2.> END ***

                '*** < 3.> ***
                'backup Stations
                sql = "create table temp_stations as select * from stations "
                dbe.SQL_String_Execute(sql)
                'drop/create new table
                sql = "drop table Stations; CREATE TABLE Stations (station_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, station_name TEXT NOT NULL, system_id INTEGER NOT NULL,station_type INTEGER DEFAULT 0, market INTEGER NOT NULL DEFAULT 0, blackmarket INTEGER NOT NULL DEFAULT 0, outfitting INTEGER NOT NULL DEFAULT 0, shipyard INTEGER NOT NULL DEFAULT 0, star_dist INTEGER DEFAULT 0, station_visible INTEGER DEFAULT 1) "
                dbe.SQL_String_Execute(sql)
                'reinsert data
                sql = "insert into Stations (station_id,station_name,system_id,station_type,market,blackmarket,outfitting,shipyard,star_dist) select station_id,station_name,system_id,0,market,blackmarket,outfitting,shipyard,star_dist from temp_Stations "
                dbe.SQL_String_Execute(sql)
                'delete temporary table
                sql = "drop table temp_Stations "
                dbe.SQL_String_Execute(sql)
                '*** < 3.> END ***

        End Select

    End Sub

    Sub rollback_update()
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Label_Status.Text = "Rolling back changes..."

            If exeNewCreated Then
                File.Delete(sup + "\" + appName + ".exe")
                File.Move(sup + "\" + appName + ".tmp", sup + "\" + appName + ".exe")
            Else
                If exeBackup Then
                    File.Move(sup + "\" + appName + ".tmp", sup + "\" + appName + ".exe")
                End If
            End If


            If dbNewCreated Then
                File.Delete(sup + "\" + dbName)
                File.Move(sup + "\" + dbName + "TMP", sup + "\" + dbName)
            Else
                If dbBackup Then
                    File.Move(sup + "\" + dbName + "TMP", sup + "\" + dbName)
                End If
            End If
            Me.Label_Status.Text = "Rollback successful!"



        Catch ex As Exception
            Me.Label_Status.Text = "Rollback failed!"

            MsgBox("Rollback failed!  Please reinstall application")
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub


    '**************************
    '*** DISPLAY CHANGE LOG ***
    '**************************
    Dim cs As Cursor
    Private Sub WebBrowser1_GotFocus(sender As Object, e As System.EventArgs) Handles WebBrowser1.GotFocus
        cs = Me.Cursor
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub WebBrowser1_LostFocus(sender As Object, e As System.EventArgs) Handles WebBrowser1.LostFocus
        Me.Cursor = cs
    End Sub
    Sub load_change_log()
        File.WriteAllBytes(sup + "\EDAChangeLog.docx", My.Resources.EDAChangeLog)
        LoadDocument(sup + "\EDAChangeLog.docx")
    End Sub
    Sub LoadDocument(ByVal fileName As String)

        ' First, create a new Microsoft.Office.Interop.Word.ApplicationClass.
        Dim ac As New Application
        Dim oldFileName As Object = fileName
        Try
            ' Now we open the document.
            Dim doc As Document = ac.Documents.Open(oldFileName, vbFalse)

            ' Create a temp file to save the HTML file to. 
            Dim tempFileName As String = GetTempFile("html")

            ' Cast these items to object.  The methods we're calling 
            ' only take object types in their method parameters. 
            Dim newFileName As Object = CObj(tempFileName)

            ' We will be saving this file as HTML format. 
            Dim fileType As Object = CObj(WdSaveFormat.wdFormatHTML)

            ' Save the file. 
            doc.SaveAs(newFileName, fileType)

            Me.WebBrowser1.Url = New Uri(newFileName)

        Finally

            ' Make sure we close the application class. 
            If ac IsNot Nothing Then
                ac.Quit(False)
            End If
        End Try
    End Sub
    Function GetTempFile(ByVal extension As String) As String
        ' Uses the Combine, GetTempPath, ChangeExtension, 
        ' and GetRandomFile methods of Path to 
        ' create a temp file of the extension we're looking for. 
        Return Path.Combine(Path.GetTempPath(),
            Path.ChangeExtension(Path.GetRandomFileName(), extension))
    End Function


End Class
