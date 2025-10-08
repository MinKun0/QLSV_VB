Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Public Class frmQLSV
    Dim conn As New SqlConnection("Data Source=ADMIN-PC\SQLEXPRESS;Initial Catalog=QLSinhVien;Integrated Security=True")

    Private Sub frmQLSV_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadComboBox()
        LoadDataGridView()
    End Sub
    Private Sub LoadComboBox()
        Try
            conn.Open()
            Dim cmd As New SqlCommand("SELECT MaLop, TenLop FROM LOP", conn)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)
            cmbClass.DataSource = dt
            cmbClass.DisplayMember = "TenLop"
            cmbClass.ValueMember = "MaLop"
        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub LoadDataGridView()
        Try
            conn.Open()
            Dim query As String = "SELECT MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop FROM SINHVIEN"
            Dim cmd As New SqlCommand(query, conn)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            DataGridView1.DataSource = dt

            DataGridView1.Columns("NgaySinh").DefaultCellStyle.Format = "dd/MM/yyyy"
            DataGridView1.Columns("MaSV").HeaderText = "Mã SV"
            DataGridView1.Columns("HoTen").HeaderText = "Họ và Tên"
            DataGridView1.Columns("NgaySinh").HeaderText = "Ngày sinh"
            DataGridView1.Columns("GioiTinh").HeaderText = "Giới tính"
            DataGridView1.Columns("DiaChi").HeaderText = "Địa chỉ"
            DataGridView1.Columns("MaLop").HeaderText = "Mã lớp"

        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub cmbCla_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClass.SelectedIndexChanged
        Try
            If cmbClass.SelectedValue Is Nothing Then Exit Sub
            Dim query As String = "SELECT SV.MaSV, SV.HoTen, SV.NgaySinh, SV.GioiTinh, SV.DiaChi, SV.MaLop 
                               FROM SINHVIEN SV
                               INNER JOIN LOP L ON SV.MaLop = L.MaLop
                               WHERE L.TenLop = @TenLop"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@TenLop", cmbClass.Text)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            DataGridView1.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Lỗi: " & ex.Message)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim query As String = "SELECT MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop
                               FROM SINHVIEN
                               WHERE MaSV LIKE @Search OR HoTen LIKE @Search"
        Dim da As New SqlDataAdapter(query, conn)
        da.SelectCommand.Parameters.AddWithValue("@Search", "%" & txtSearch.Text & "%")

        Dim dt As New DataTable()
        da.Fill(dt)
        DataGridView1.DataSource = dt
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim frm As New frmSinhVien
        frm.ShowDialog()
        LoadDataGridView()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim frm As New frmSinhVien()

        With DataGridView1()
            frm.txtMaSV.Text = .CurrentRow.Cells("MaSV").Value.ToString()
            frm.txtHoTen.Text = .CurrentRow.Cells("HoTen").Value.ToString()
            frm.txtNgaySinh.Text = Convert.ToDateTime(.CurrentRow.Cells("NgaySinh").Value).ToString("dd/MM/yyyy")
            frm.txtGioiTinh.Text = .CurrentRow.Cells("GioiTinh").Value.ToString()
            frm.txtDiaChi.Text = .CurrentRow.Cells("DiaChi").Value.ToString()
            frm.txtMaLop.Text = .CurrentRow.Cells("MaLop").Value.ToString()
        End With
        frm.ShowDialog()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If DataGridView1.CurrentRow Is Nothing Then
            MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim maSV As String = DataGridView1.CurrentRow.Cells("MaSV").Value.ToString()

        If MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên có mã: " & maSV & " ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Try
            conn.Open()

            Dim query As String = "DELETE FROM SINHVIEN WHERE MaSV = @MaSV"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@MaSV", maSV)

            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

            If rowsAffected > 0 Then
                MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadDataGridView() ' Load lại danh sách sau khi xóa
            Else
                MessageBox.Show("Không tìm thấy sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Lỗi khi xóa: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class











