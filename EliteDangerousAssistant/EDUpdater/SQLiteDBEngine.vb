Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SQLite

Public Class SQLiteDBEngine

    Dim _sqlConn As SQLiteConnection

    Sub New(ByVal conn As String)
        _sqlConn = New SQLiteConnection(conn)
    End Sub



    Public Function SQLStringtoDatatable(ByVal sql As String) As DataTable

        Dim query2 As String = sql
        Dim sqlDA2 As New SQLiteDataAdapter(query2, _sqlConn)
        Dim dt2 As New DataTable()

        sqlDA2.Fill(dt2)

        Return dt2
    End Function

    Public Function SQL_String_Execute(ByVal sql As String) As Boolean

        Dim result As Boolean = False
        Try
            Dim Cmd As New SQLiteCommand

            Cmd.CommandType = CommandType.Text
            Cmd.Connection = _sqlConn
            Cmd.CommandText = sql

            If _sqlConn.State = ConnectionState.Closed Then
                _sqlConn.Open()
            End If

            Cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
            Return result
        End Try

        result = True
        Return result
    End Function

    Public Function existCheck(ByVal sql As String) As Boolean
        Dim exists As Boolean = False

        Dim query2 As String = sql
        Dim sqlDA2 As New SQLiteDataAdapter(query2, _sqlConn)
        Dim dt2 As New DataTable()

        sqlDA2.Fill(dt2)

        If dt2.Rows.Count > 0 Then exists = True

        Return exists
    End Function

    Public Function sqlDateConvert(ByVal idate As Date) As String
        Dim newdate As String

        Dim iyear As String
        Dim imonth As String
        Dim iday As String

        iyear = DatePart(DateInterval.Year, idate).ToString

        If Len(DatePart(DateInterval.Month, idate).ToString) = 1 Then
            imonth = "0" + DatePart(DateInterval.Month, idate).ToString
        Else
            imonth = DatePart(DateInterval.Month, idate).ToString
        End If

        If Len(DatePart(DateInterval.Day, idate).ToString) = 1 Then
            iday = "0" + DatePart(DateInterval.Day, idate).ToString
        Else
            iday = DatePart(DateInterval.Day, idate).ToString
        End If

        newdate = iyear + "-" + imonth + "-" + iday + " 00:00:00.000"

        Return newdate
    End Function






End Class

