using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using System.Data.SqlClient;
using System.Data;

namespace TriggerQLVT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
       /* public static SqlConnection connection = null;
        public static SqlCommand command = null;

        public static string GetConnectionString()
        {
            return @"Data Source=DESKTOP-635R4EM\VUVU;Initial Catalog=QLVT_DATHANG;User ID=sa;Password=123;";
        }
        */
        public static SqlConnection conn = new SqlConnection();//Biết để kết nối SQL, tồn tại xuyên suốt ct
        public static String connstr= "Data Source=DESKTOP-635R4EM\\VUVU;Initial Catalog=QLVT_DATHANG;User ID=sa;Password=123;";//Chứa chuỗi kết nối
        public static SqlDataReader myReader;
        public static int KetNoi()
        {
            if (Program.conn != null && Program.conn.State == ConnectionState.Open)
            {
                Program.conn.Close();
            }
            try
            {
                Program.conn.ConnectionString = Program.connstr;
                Program.conn.Open();
                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu. " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }

        }
        public static SqlDataReader ExecSqlDataReader(String strLenh) //sự khác biệt giữa datareader và datatable: khi nào sd reader:tải dl về chỉ để xem k sửa đc, chỉ có 1 chìu đi xuống k lên.(khuyết điểm), ưu điểm: tải về nhanh. datatable ngc lại. (muốn thêm xóa sửa: data table)
        {
            SqlDataReader myreader;
            SqlCommand sqlcmd = new SqlCommand(strLenh, Program.conn);
            sqlcmd.CommandType = CommandType.Text;
            if (Program.conn.State == ConnectionState.Closed) Program.conn.Open();
            try
            {
                myreader = sqlcmd.ExecuteReader(); return myreader;

            }
            catch (SqlException ex)
            {
                Program.conn.Close();
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public static DataTable ExecSqlDataTable(String cmd)
        {
            DataTable dt = new DataTable();
            if (Program.conn.State == ConnectionState.Closed) Program.conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public static int ExecSqlNonQuery(String strlenh) //gọi n k trả về giá trị
        {
            SqlCommand Sqlcmd = new SqlCommand(strlenh, conn);
            Sqlcmd.CommandType = CommandType.Text;
            Sqlcmd.CommandTimeout = 600;// 10 phut 
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                Sqlcmd.ExecuteNonQuery(); conn.Close();
                return 0;
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Error converting data type varchar to int"))
                    MessageBox.Show("Bạn format Cell lại cột \"Ngày Thi\" qua kiểu Number hoặc mở File Excel.");
                else MessageBox.Show(ex.Message);
                conn.Close();
                return ex.State; // trang thai lỗi gởi từ RAISERROR trong SQL Server qua
            }
        }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            Application.Run(new Form1());
        }
    }
}
