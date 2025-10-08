Imports System.Data.SqlClient

Public Class Login
    Dim conn As New SqlConnection("Data Source=ADMIN-PC\SQLEXPRESS;Initial Catalog=QLSinhVien;Integrated Security=True")
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub
    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If String.IsNullOrWhiteSpace(txtUser.Text) OrElse String.IsNullOrWhiteSpace(txtPass.Text) Then
            MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            conn.Open()

            Dim query As String = "SELECT Password FROM USERS WHERE Username=@user"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@user", txtUser.Text)

            Dim result As Object = cmd.ExecuteScalar()

            If result IsNot Nothing Then
                Dim dbPass As String = result.ToString()

                If String.Equals(dbPass, txtPass.Text, StringComparison.Ordinal) Then
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Me.Hide()
                    Dim f As New frmQLSV()
                    f.ShowDialog()
                    Me.Show()
                Else
                    MessageBox.Show("Sai mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("Tài khoản không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub
End Class