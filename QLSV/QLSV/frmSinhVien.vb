Imports System.Data.SqlClient

Public Class frmSinhVien
    Dim conn As New SqlConnection("Data Source=ADMIN-PC\SQLEXPRESS;Initial Catalog=QLSinhVien;Integrated Security=True")
    Public isEditMode As Boolean = False
    Public oldMaSV As String = ""
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If String.IsNullOrWhiteSpace(txtMaSV.Text) Then
            MessageBox.Show("Mã sinh viên không được để trống.")
            txtMaSV.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtHoTen.Text) Then
            MessageBox.Show("Họ tên không được để trống.")
            txtHoTen.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtNgaySinh.Text) Then
            MessageBox.Show("Ngày sinh không được để trống.")
            txtNgaySinh.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtGioiTinh.Text) Then
            MessageBox.Show("Giới tính không được để trống.")
            txtGioiTinh.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtDiaChi.Text) Then
            MessageBox.Show("Địa chỉ không được để trống.")
            txtDiaChi.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(txtMaLop.Text) Then
            MessageBox.Show("Mã lớp không được để trống.")
            txtMaLop.Focus()
            Exit Sub
        End If

        Dim ngay As Date
        If Not Date.TryParseExact(txtNgaySinh.Text, "dd/MM/yyyy",
                              Globalization.CultureInfo.InvariantCulture,
                              Globalization.DateTimeStyles.None, ngay) Then
            MessageBox.Show("Ngày sinh không hợp lệ!")
            txtNgaySinh.Focus()
            Exit Sub
        End If

        Try
            conn.Open()
            Dim query As String

            If isEditMode = True Then

                query = "UPDATE SINHVIEN 
                         SET MaSV=@MaSV, HoTen=@HoTen, NgaySinh=@NgaySinh, GioiTinh=@GioiTinh, DiaChi=@DiaChi, MaLop=@MaLop
                         WHERE MaSV=@OldMaSV"
            Else

                query = "INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop) 
                         VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @MaLop)"
            End If

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text)
            cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text)
            cmd.Parameters.AddWithValue("@NgaySinh", ngay)
            cmd.Parameters.AddWithValue("@GioiTinh", txtGioiTinh.Text)
            cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text)
            cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text)

            If isEditMode = True Then
                cmd.Parameters.AddWithValue("@OldMaSV", oldMaSV)
            End If

            cmd.ExecuteNonQuery()

            MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class


