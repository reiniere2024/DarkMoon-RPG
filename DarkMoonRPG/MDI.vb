Public Class MDI
    Public MyForm As New Mainwindow2(Me)


    Private Sub MDI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        MyForm.MdiParent = Me
        MyForm.Show()


    End Sub
End Class