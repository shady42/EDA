Public Class DrawingSurface

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        'swap sys/sta button lines
        Dim myPen As New System.Drawing.Pen(System.Drawing.Color.LightGray, 2)
        Dim formGraphics As System.Drawing.Graphics
        Dim x1 As Integer = 293
        Dim y1 As Integer = 85
        Dim x2 As Integer = x1 + 22
        Dim y2 As Integer = y1 + 123
        formGraphics = Me.CreateGraphics()
        formGraphics.DrawLine(myPen, x1, y1, x2, y1)
        formGraphics.DrawLine(myPen, x2, y1, x2, y2)
        formGraphics.DrawLine(myPen, x2, y2, x1, y2)
        'w=27; h=134
    End Sub
End Class
