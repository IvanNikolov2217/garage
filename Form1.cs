using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace garage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string PC = "";
        public static string User = "";
        public static string Rights = "";
        public static bool ReadOnlyUser = false;
        public static string conStr;
        public static int teamID = 0;
        public static int driverID = 0;
        public static int trailerID = 0;

        private void btnTrailer_Click(object sender, EventArgs e)
        {
            panelTrailer.Location = new Point(476, 50);
            panelTrailer.BringToFront();
        }

        private void btnRestDrivers_Click(object sender, EventArgs e)
        {
            panelRestDrivers.Height = 318;
            panelRestDrivers.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panelTrailer.Location = new Point(476, 50);
            panelRestDrivers.BringToFront();

            panelDriverSave.Location = new Point(7, 50);
            panelTeamSave.BringToFront();

            panelTrailerSave.Location = new Point(7, 50);
            
            

            conStr = "Data Source=" + IniFile.ReadValue("database", "server", ".\\Settings.ini", "localhost") + "; initial catalog=" + IniFile.ReadValue("database", "base", ".\\Settings.ini", "DataBase") + "; user id=" + IniFile.ReadValue("database", "user", ".\\Settings.ini", "user") + "; password=";
            conStr = conStr + StringCipher.Decrypt(IniFile.ReadValue("last values", "transaction", ".\\Settings.ini", "fhgf67ftyf9guyguyg"), IniFile.ReadValue("database", "user", ".\\Settings.ini", "guyguyg")) + ";";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panelRestDrivers.Height = 318;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            populateTables();
            
        }
        private void populateTables()
        {

            filtersClear();
            dgvRestDrivers.AllowUserToAddRows = true;
            dgvTeam.AllowUserToAddRows = true;
            dgvTrailer.AllowUserToAddRows =  true;

            //Tools.populateDataGridView("select column1,column2, column3,column4,column5,column6,column7,column8, номер from dpBuffer where UPPER(имена)='" + ime + "' order by номер", dataGridView1, new int[] { 1, 1, 1, 1, 1, 1, 1, 0, 1 }, true, conStr);
            //foreach (DataGridViewRow row in dataGridView1.Rows)  // оцветява Buffer
            //{
            //    if (row.Cells[7].Value != null && row.Cells[7].Value.ToString() != "")
            //    {
            //        row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml(row.Cells[7].Value.ToString());
            //    }
            //}
            //dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows[dataGridView1.RowCount-1].Index;


            //   [id] [int] IDENTITY(1, 1) NOT NULL,
            //[спедиция] [nvarchar](200) NULL,
            //[влекач] [nvarchar](100) NOT NULL,
            //[ремарке] [nvarchar](100) NULL,
            //[шофьор 1] [nvarchar](200) NULL,
            //[шофьор 2] [nvarchar](200) NULL,
            //[предишен шофьор] [nvarchar](200) NULL,
            //[предишен влекач] [nvarchar](100) NULL,
            //[дестинация] [nvarchar](200) NULL,
            //[срок] [date] NULL,
            //[диспонент] [nvarchar](200) NULL,
            //[шофьор TR] [nvarchar](200) NULL,
            //[шофьор EU] [nvarchar](200) NULL,
            //[описание] [nvarchar](800) NULL,
            //[fUser] [nvarchar](32) NULL,
            //[fPC] [nvarchar](32) NULL,
            //[sysDate] [datetime] NULL,



            Tools.populateDataGridView("select id, спедиция, влекач, ремарке, [шофьор 1], [шофьор 2], диспонент, описание, локация ,sysDate from garageTeam order by спедиция, влекач", dgvTeam, new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1 ,1}, true, conStr);
            //foreach (DataGridViewRow row in dgvRestDrivers.Rows)  // оцветява drivers
            //{
            //    if (row.Cells["rowColour"].Value != null && row.Cells["rowColour"].Value.ToString() != "")
            //    {
            //        row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml(row.Cells["rowColour"].Value.ToString());
            //    }

            //}

            //dgvRestDrivers.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgvRestDrivers.Columns[11].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgvRestDrivers.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgvRestDrivers.Columns[12].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgvRestDrivers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //dgvRestDrivers.FirstDisplayedScrollingRowIndex = dgvRestDrivers.Rows[dgvRestDrivers.RowCount - 1].Index;




            // 

            Tools.populateDataGridView("select  име ,егн ,статус, [качил се], [пристигнал], курсове, забележка,id from garageDrivers order by име", dgvRestDrivers, new int[] { 1, 1, 1, 1, 1, 1, 1, 0 }, true, conStr);
            Tools.populateDataGridView("select ремарке, дестинация,локация,id,описание,sysDate from garageTrailers order by ремарке ", dgvTrailer, new int[] { 1, 1, 1, 1, 1, 1 }, true, conStr);
            //foreach (DataGridViewRow row in dgvTeam.Rows)  // оцветява текущи
            //{
            //    if (row.Cells["rowColour"].Value != null && row.Cells["rowColour"].Value.ToString() != "")
            //    {
            //        row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml(row.Cells["rowColour"].Value.ToString());
            //    }
            //}

            //dgvTeam.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgvRestDrivers.Columns[10].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgvRestDrivers.Columns[11].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //dgvRestDrivers.Columns[11].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //dgvRestDrivers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            //  Tools.populateDataGridView("select id, егн , име, статус, [качил се], [пристигнал], курсове, забележка, sysDate from garageTrailers order by име", dgvTrailer, new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 1 }, true, conStr);

            dgvRestDrivers.AllowUserToAddRows = false;
            dgvTeam.AllowUserToAddRows = false;
            dgvTrailer.AllowUserToAddRows = false;
            
        }
        private void filtersClear()
        {
            cmbRestDrivers.ResetText();
            cmbTeam.ResetText();
            cmbTrailer.ResetText();
        }

        private void btnTeamSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlUpdate = new SqlConnection(conStr))
            {
                sqlUpdate.Open();
                SqlCommand sqlCmd = new SqlCommand("update garageTeam set спедиция ='" + comboBoxTeamSpediciq.Text + "', влекач= '" + txtTeamTruck.Text + "', ремарке='"+ txtTeamTrailer.Text +"',[шофьор 1]='"+txtTeamDriver1.Text+"',[шофьор 2]='"+txtTeamDriver2.Text+"',описание='"+txtTeamOpisanie.Text+ "' , диспонент='" + txtTeamDispacher.Text+ "' ,локация='" + txtTeamLocation.Text+ "' where id=" + teamID, sqlUpdate);
                // SqlCommand sqlCmd = new SqlCommand("update garageDrivers set статус='" + cmbStatus.Text + "', [качил се]=" + dat1 + ",[пристигнал]=" + dat2 + ",[курсове]='" + cmbKursove.Text + "',забележка='" + txtNote.Text + "' where id=" + driverID, sqlUpdate);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
                sqlUpdate.Close();
            }
            populateTables();
        }

        private void dgvRestDrivers_DoubleClick(object sender, EventArgs e)
        {
            driverID = Convert.ToInt32(dgvRestDrivers.CurrentRow.Cells["id"].Value.ToString());

            //textBox1.Text = Tools.getData("SELECT [качил се] FROM garageDrivers WHERE [име]='" + driver + "'",conStr).Substring(0,10);
            //panelDriverSave.Visible = true;
            // panelDriverSave.BringToFront();

            //if (textBox2.Text == "")
            //{
            //    lblWorkDays.Text = WorkTime(Convert.ToDateTime(textBox1.Text), DateTime.Now);
            //}
            //else
            //{
            //    lblWorkDays.Text = WorkTime(Convert.ToDateTime(textBox1.Text), Convert.ToDateTime(textBox2.Text));
            //}

            //if (textBox2.Text == "")
            //    lblRestDays.Text = "";
            //else
            //    lblRestDays.Text = WorkTime(Convert.ToDateTime(textBox2.Text), DateTime.Now);
            txtTeamDriver1.Text = dgvRestDrivers.CurrentRow.Cells["име"].Value.ToString();
        }

        private static string WorkTime(DateTime dateOfEmployment, DateTime EndDate)
        {
            //var NowTime = EndDate.AddDays(1);

            var Months = (EndDate.Year - dateOfEmployment.Year) * 12 + EndDate.Month - dateOfEmployment.Month;
            Months += EndDate.Day < dateOfEmployment.Day ? -1 : 0;

            var yearsOfEmployment = Months / 12;
            var monthsOfEmployment = Months % 12;
            var daysOfEmployment = EndDate.Subtract(dateOfEmployment.AddMonths(Months)).Days;
            var days = EndDate.Subtract(dateOfEmployment).Days;

            //return string.Format("( {0} г., {1} м., {2} д.)", yearsOfEmployment, monthsOfEmployment, daysOfEmployment);
            return days.ToString();
        }

        private void btnTeam_Click(object sender, EventArgs e)
        {
            panelTeamSave.BringToFront();
        }
        private void dgvTrailer_DoubleClick(object sender,EventArgs e)
        {
            txtTeamTrailer.Text = dgvTrailer.CurrentRow.Cells["ремарке"].Value.ToString();
            txtTeamLocation.Text = dgvTrailer.CurrentRow.Cells["локация"].Value.ToString();
        }
        private void dgvTeam_DoubleClick(object sender, EventArgs e)
        {
            //string truck = dgvTeam.CurrentCell.Value.ToString();
           /// teamID= Convert.ToInt32(dgvTeam.CurrentRow.Cells["id"].Value);
            
           /// SqlConnection con = new SqlConnection(conStr);
            ///con.Open();
            ///SqlCommand command = new SqlCommand("select id , спедиция, влекач , ремарке , [шофьор 1] , [шофьор 2], диспонент,описание,локация, sysDate from garageTeam WHERE id='" + teamID + "'",con);
            ///SqlDataReader reader = command.ExecuteReader();
            //List<string> list = new List<string>();

            ///reader.Read();
            teamID = Convert.ToInt32(dgvTeam.CurrentRow.Cells["id"].Value);
            comboBoxTeamSpediciq.Text = dgvTeam.CurrentRow.Cells["спедиция"].Value.ToString();
            txtTeamTruck.Text = dgvTeam.CurrentRow.Cells["влекач"].Value.ToString();
            txtTeamTrailer.Text = dgvTeam.CurrentRow.Cells["ремарке"].Value.ToString();
            txtTeamDriver1.Text = dgvTeam.CurrentRow.Cells["шофьор 1"].Value.ToString();
            txtTeamDriver2.Text = dgvTeam.CurrentRow.Cells["шофьор 2"].Value.ToString();
            txtTeamDispacher.Text = dgvTeam.CurrentRow.Cells["диспонент"].Value.ToString();
            txtTeamOpisanie.Text = dgvTeam.CurrentRow.Cells["описание"].Value.ToString();
            txtTeamLocation.Text = dgvTeam.CurrentRow.Cells["локация"].Value.ToString();

            //textBox8.Text = reader[4] as string;   // валидна от
            //textBox9.Text = reader[5] as string;   // валидна до
            //DateTime? dat1 = reader.IsDBNull(4) ?
            //    (DateTime?)null :
            //    (DateTime?)reader.GetDateTime(4);
            ////DateTime? dat1 = reader.GetDateTime(reader.GetOrdinal("активна от"));
            //DateTime? dat2 = reader.IsDBNull(5) ?
            //    (DateTime?)null :
            //    (DateTime?)reader.GetDateTime(5);
            //textBox8.Text = dat1.ToString();
            //if (textBox8.Text.Length >= 10) textBox8.Text = textBox8.Text.Substring(0, 10);
            //textBox9.Text = dat2.ToString();
            //if (textBox9.Text.Length >= 10) textBox9.Text = textBox9.Text.Substring(0, 10);
            //bool isActive = reader.GetBoolean(reader.GetOrdinal("active"));
            //// int active = reader[3] as int? ?? default(int);


            ///con.Close();



            //txtTeamTruck.Text = Tools.getData("SELECT [влекач] FROM garageTeam WHERE [влекач]='" +truck + "'", conStr);
            panelTeamSave.Visible = true;
            panelTeamSave.BringToFront();

        }

        private void btnDriverEdit_Click(object sender, EventArgs e)
        {
            if (dgvRestDrivers.CurrentRow != null)
            {

                driverID = Convert.ToInt32(dgvRestDrivers.CurrentRow.Cells["id"].Value);

                lblName.Text = dgvRestDrivers.CurrentRow.Cells["име"].Value.ToString();
                cmbStatus.Text = dgvRestDrivers.CurrentRow.Cells["статус"].Value.ToString();
                if (Tools.IsDate(dgvRestDrivers.CurrentRow.Cells["качил се"].Value.ToString()))
                {
                    txtStart.Text = ((DateTime)dgvRestDrivers.CurrentRow.Cells["качил се"].Value).ToString("dd.MM.yyyy");
                    lblWorkDays.Text = WorkTime((DateTime)dgvRestDrivers.CurrentRow.Cells["качил се"].Value,(DateTime)dgvRestDrivers.CurrentRow.Cells["пристигнал"].Value) + " дни";
                }
                if (Tools.IsDate(dgvRestDrivers.CurrentRow.Cells["пристигнал"].Value.ToString()))
                {
                    txtEnd.Text = ((DateTime)dgvRestDrivers.CurrentRow.Cells["пристигнал"].Value).ToString("dd.MM.yyyy");
                    lblRestDays.Text = WorkTime((DateTime)dgvRestDrivers.CurrentRow.Cells["пристигнал"].Value, DateTime.Now) + " дни";
                }
                cmbKursove.Text = dgvRestDrivers.CurrentRow.Cells["курсове"].Value.ToString();
                txtNote.Text = dgvRestDrivers.CurrentRow.Cells["забележка"].Value.ToString();

                panelDriverSave.BringToFront();
            }
           
        }
      
        private void btnTrailerEdit_Click(object sender ,EventArgs e)
        {

                 trailerID = Convert.ToInt32(dgvTrailer.CurrentRow.Cells["id"].Value);
                 labelTrailerNumber.Text = dgvTrailer.CurrentRow.Cells["ремарке"].Value.ToString();
                 comboTrailerDestination.Text = dgvTrailer.CurrentRow.Cells["дестинация"].Value.ToString();
                 textBoxTrailerLocation.Text = dgvTrailer.CurrentRow.Cells["локация"].Value.ToString();
                 textBoxTrailerDescription.Text = dgvTrailer.CurrentRow.Cells["описание"].Value.ToString(); 
                 panelDriverSave.SendToBack();
                 panelTeamSave.SendToBack();
                 panelTrailerSave.BringToFront();

            

        }
        

        private void btnDriverSave_Click(object sender, EventArgs e)
        {
            string dat1 = "";
            string dat2 = "";
            if (Tools.IsDate(txtStart.Text))   // дата качил се
                dat1 = "'" + DateTime.Parse(txtStart.Text).ToString("yyyy-MM-dd") + "'";
            else
                dat1 = "null";
            if (Tools.IsDate(txtEnd.Text))   // дата пристигнал
                dat2 = "'" + DateTime.Parse(txtEnd.Text).ToString("yyyy-MM-dd") + "'";
            else
                dat2 = "null";
            using (SqlConnection sqlUpdate = new SqlConnection(conStr))
            {
                sqlUpdate.Open();
                SqlCommand sqlCmd = new SqlCommand("update garageDrivers set статус='" + cmbStatus.Text + "', [качил се]=" + dat1 + ",[пристигнал]=" + dat2 + ",[курсове]='" + cmbKursove.Text + "',забележка='" + txtNote.Text + "' where id=" + driverID, sqlUpdate);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
                sqlUpdate.Close();
            }
            populateTables();
        }
        private void btnTrailerSave_Click(object sender,EventArgs e)
        {

            using (SqlConnection sqlUpdate = new SqlConnection(conStr))
            {
                sqlUpdate.Open();
                SqlCommand sqlCmd = new SqlCommand("update garageTrailers set дестинация='" + comboTrailerDestination.Text + "', локация=" + textBoxTrailerLocation.Text + ",описание=" + textBoxTrailerDescription.Text + " where id=" + trailerID, sqlUpdate);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.ExecuteNonQuery();
                sqlUpdate.Close();
            }
            populateTables();

        }

        private void panelDriverSave_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvRestDrivers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panelRestDrivers_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
