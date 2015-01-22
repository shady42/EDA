Public Class About

    Private Sub About_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Label_ABOUT_Version.Text = My.Application.Info.Version.ToString
    End Sub
End Class