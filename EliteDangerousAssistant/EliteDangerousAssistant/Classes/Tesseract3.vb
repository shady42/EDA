Imports EliteDangerousAssistant
Imports System.Text
Imports System.Drawing
Imports System.Threading
Imports System.Collections.Generic
Imports System.Math
Imports System.Diagnostics
Imports System.IO
Imports Tesseract



Public Class Program_Tess302

    Public Shared tdFolder As String
    Dim fullFileName As String
    Dim fileName As String
    Dim tempfolder As String
    Dim lang As String
    Dim eMode As EngineMode
    Dim numOnly As Integer
    Public Shared c0 As Integer
    Public Shared c1 As Integer
    Public Shared c2 As Integer
    Public Shared c3 As Integer
    Public Shared c4 As Integer
    Public Shared c5 As Integer
    Public Shared c6 As Integer
    Public Shared c7 As Integer
    Public Shared left1 As Integer

    Sub New(ByVal td As String, ByVal tmp As String, ByVal fullFile As String, ByVal file As String, ByVal lng As String, ByVal engmode As EngineMode, ByVal num As Integer)
        tdFolder = td
        tempfolder = tmp
        fullFileName = fullFile
        fileName = file
        lang = lng
        eMode = engmode
        numOnly = num
    End Sub


    Public Sub Main()

        Try
            Using engine = New TesseractEngine(tdFolder, lang, eMode)
                Using img = Pix.LoadFromFile(fullFileName)
                    set_column_threshold(img.Width)
                    'engine.SetVariable("tessedit_char_whitelist", "0123456798.")
                    engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.")

                    engine.DefaultPageSegMode = PageSegMode.SingleBlock 'treat document as single block of text
                    Dim page As Tesseract.Page = engine.Process(img) 'performs OCR

                    DumpResult(page)
                End Using
            End Using


        Catch e As Exception
            MsgBox(e.Message + Chr(13) + Chr(13) + e.ToString)
        End Try

    End Sub


    Sub set_column_threshold(ByVal width As Integer)

        c0 = Round(width * 0.31, 0)
        c1 = Round(width * 0.383, 0)
        c2 = Round(width * 0.456, 0)
        c3 = Round(width * 0.5344, 0)
        c4 = Round(width * 0.625, 0)
        c5 = Round(width * 0.68, 0)
        c6 = Round(width * 0.777, 0)
        c7 = Round(width * 0.845, 0)
        left1 = Round(width * 0.026, 0)

    End Sub


    Public Sub DumpResult(ByVal page As Tesseract.Page)
        Dim line As Integer = 1
        Dim lineUsed As Boolean = False
        Dim lIndex As Integer
        Dim rIndex As Integer
        Dim colIndex As Integer = 0
        Dim i As Integer = 1

        'clear containers and prep for new data
        MainForm.DataGridView_OCR_CommPrice.Rows.Clear()

        Dim text = page.GetText()
        If text = "" Or text Is Nothing Then
        Else
            Using iter = page.GetIterator()
                iter.Begin()
                Do 'loops paragraphs in page
                    Do 'loops lines in paragraph
                        If i > line Then
                            If lineUsed Then
                                line += 1
                                lineUsed = False
                            Else
                                line += 1
                                lineUsed = False
                                MainForm.DataGridView_OCR_CommPrice.Rows.Add("", "", "", "", "", "", "", "")
                            End If
                        End If
                        Do 'loops words in line

                            'gets position of current word 'box'
                            Dim pos As Tesseract.Rect
                            iter.TryGetBoundingBox(PageIteratorLevel.Word, pos)
                            lIndex = pos.X1
                            rIndex = pos.X2
                            Dim TX As String = iter.GetText(PageIteratorLevel.Word)
                            If lIndex < left1 And MainForm.hdr_flag = False Then
                                i = i - 1
                                Exit Do
                            End If


                            'uses word if confidence is above threshold
                            If iter.GetConfidence(PageIteratorLevel.Word) >= CDec(My.Settings.OP_OCR_Confidence) Then

                                If rIndex < Program_Tess302.c0 Then 'name
                                    colIndex = 0
                                ElseIf rIndex < Program_Tess302.c1 Then 'sell
                                    colIndex = 1
                                ElseIf rIndex < Program_Tess302.c2 Then 'buy
                                    colIndex = 2
                                ElseIf rIndex < Program_Tess302.c3 Then 'cargo
                                    colIndex = 3
                                ElseIf rIndex < Program_Tess302.c4 Then 'demand
                                    colIndex = 4
                                ElseIf rIndex < Program_Tess302.c5 Then 'demand rating
                                    colIndex = 5
                                ElseIf rIndex < Program_Tess302.c6 Then 'supply
                                    colIndex = 6
                                ElseIf rIndex < Program_Tess302.c7 Then 'supply rating
                                    colIndex = 7
                                Else
                                    colIndex = 99
                                End If


                                If colIndex < 99 Then

                                    Dim rCount As Integer = MainForm.DataGridView_OCR_CommPrice.Rows.Count - 1

                                    If rCount >= i - 1 Then
                                        'existing line
                                        MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(colIndex).Value = Trim(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(colIndex).Value + " " + iter.GetText(PageIteratorLevel.Word))
                                        For j = 1 To 7
                                            If j = 5 Or j = 7 Then
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, " ", "")
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, ".", "")
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, "-", "")
                                            Else
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, " ", "")
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, ".", "")
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, "-", "")
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, "G", "6")
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, "O", "0")
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, "S", "5")
                                                MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_OCR_CommPrice.Rows(i - 1).Cells(j).Value, "B", "8")
                                                'MainForm.DataGridView_CommPrice.Rows(i - 1).Cells(j).Value = Replace(MainForm.DataGridView_CommPrice.Rows(i - 1).Cells(j).Value, "O", "0")
                                            End If
                                        Next
                                        lineUsed = True
                                    Else
                                        'new line
                                        MainForm.DataGridView_OCR_CommPrice.Rows.Add(iter.GetText(PageIteratorLevel.Word), "", "", "", "", "", "", "")
                                        lineUsed = True
                                    End If
                                End If
                            End If
                        Loop While iter.[Next](PageIteratorLevel.TextLine, PageIteratorLevel.Word)
                        i += 1
                    Loop While iter.[Next](PageIteratorLevel.Para, PageIteratorLevel.TextLine)
                Loop While iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para)
            End Using
        End If


    End Sub



End Class

