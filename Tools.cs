using System;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace garage
{

    public class IniFile
    {
        public static int capacity = 512;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key,
        string defaultValue, StringBuilder value, int size, string filePath);
        
//------------------------------------------------------------------------------

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue,
        [In, Out]char[] value, int size, string filePath);
        
//------------------------------------------------------------------------------

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileSection(string section, IntPtr keyValue,
        int size, string filePath);

//------------------------------------------------------------------------------

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string section, string key,
        string value, string filePath);

//------------------------------------------------------------------------------

        public static string ReadValue(string section, string key, string filePath, string defaultValue = "")
        {
            var value = new StringBuilder(capacity);
            GetPrivateProfileString(section, key, defaultValue, value, value.Capacity, filePath);
            return value.ToString();
        }

//------------------------------------------------------------------------------

        public static bool WriteValue(string section, string key, string value, string filePath)
        {
            bool result = WritePrivateProfileString(section, key, value, filePath);
            return result;
        }
        
//------------------------------------------------------------------------------
    }

    public static class StringCipher
    {
            // This constant is used to determine the keysize of the encryption algorithm in bits.
            // We divide this by 8 within the code below to get the equivalent number of bytes.
            private const int Keysize = 256;

            // This constant determines the number of iterations for the password bytes generation function.
            private const int DerivationIterations = 1000;

//------------------------------------------------------------------------------

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

//------------------------------------------------------------------------------

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

//------------------------------------------------------------------------------

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

//------------------------------------------------------------------------------

        /* use example :
                      Console.WriteLine("Please enter a password to use:");
                     string password = Console.ReadLine();
                     Console.WriteLine("Please enter a string to encrypt:");
                     string plaintext = Console.ReadLine();
                     Console.WriteLine("");

                     Console.WriteLine("Your encrypted string is:");
                     string encryptedstring = StringCipher.Encrypt(plaintext, password);
                     Console.WriteLine(encryptedstring);
                     Console.WriteLine("");

                     Console.WriteLine("Your decrypted string is:");
                     string decryptedstring = StringCipher.Decrypt(encryptedstring, password);
                     Console.WriteLine(decryptedstring);
                     Console.WriteLine("");

                     Console.WriteLine("Press any key to exit...");
                     Console.ReadLine();
         https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
      */
        //------------------------------------------------------------------------------
    }

    //--------------------------------------------------------------------------------------------------------------------------------------
    public class MyMessageBox
    {
        private static IWin32Window _owner;
        private static HookProc _hookProc;
        private static IntPtr _hHook;

        public static DialogResult Show(string text)
        {
            Initialize();
            return MessageBox.Show(text);
        }

        public static DialogResult Show(string text, string caption)
        {
            Initialize();
            return MessageBox.Show(text, caption);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton, options);
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon,
                                   defButton, options);
        }

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);

        public const int WH_CALLWNDPROCRET = 12;

        public enum CbtHookAction : int
        {
            HCBT_MOVESIZE = 0,
            HCBT_MINMAX = 1,
            HCBT_QS = 2,
            HCBT_CREATEWND = 3,
            HCBT_DESTROYWND = 4,
            HCBT_ACTIVATE = 5,
            HCBT_CLICKSKIPPED = 6,
            HCBT_KEYSKIPPED = 7,
            HCBT_SYSCOMMAND = 8,
            HCBT_SETFOCUS = 9
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("User32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

        [DllImport("user32.dll")]
        public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };

        static MyMessageBox()
        {
            _hookProc = new HookProc(MessageBoxHookProc);
            _hHook = IntPtr.Zero;
        }
        //------------------------------------------------------------------------------
        private static void Initialize()
        {
            if (_hHook != IntPtr.Zero)
            {
                throw new NotSupportedException("multiple calls are not supported");
            }

            if (_owner != null)
            {
                uint processID = 0;
                IntPtr ptr = _owner.Handle;
                _hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, IntPtr.Zero, (int)GetWindowThreadProcessId(ptr, out processID));
            }
        }
        //------------------------------------------------------------------------------
        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(_hHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = _hHook;

            if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
            {
                try
                {
                    CenterWindow(msg.hwnd);
                }
                finally
                {
                    UnhookWindowsHookEx(_hHook);
                    _hHook = IntPtr.Zero;
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }
        //------------------------------------------------------------------------------
        private static void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle recChild = new Rectangle(0, 0, 0, 0);
            bool success = GetWindowRect(hChildWnd, ref recChild);

            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;

            Rectangle recParent = new Rectangle(0, 0, 0, 0);
            success = GetWindowRect(_owner.Handle, ref recParent);

            Point ptCenter = new Point(0, 0);
            ptCenter.X = recParent.X + ((recParent.Width - recParent.X) / 2);
            ptCenter.Y = recParent.Y + ((recParent.Height - recParent.Y) / 2);


            Point ptStart = new Point(0, 0);
            ptStart.X = (ptCenter.X - (width / 2));
            ptStart.Y = (ptCenter.Y - (height / 2));

            ptStart.X = (ptStart.X < 0) ? 0 : ptStart.X;
            ptStart.Y = (ptStart.Y < 0) ? 0 : ptStart.Y;

            int result = MoveWindow(hChildWnd, ptStart.X, ptStart.Y, width,
                                    height, false);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    public class Tools
    {
        //public static int capacity = 512;

        public static bool ExistsInListBox(ListBox lb, string cmpStr)
        {
            bool exist = false;
            for (int i = 0; i <= lb.Items.Count-1; i++)
            {
                if (lb.Items[i].ToString().ToUpper()== cmpStr.ToUpper()) exist = true;
            }
            return exist;
        }

        //------------------------------------------------------------------------------

        public static bool VerEGN(String egnin, IWin32Window owner)
        {
            bool egnres = false;
            int S, dd, d_rem;//div_t d;
            int[] Array_EGN = new int[] { 2, 4, 8, 5, 10, 9, 7, 3, 6 };
            int[] dig_str = new int[10];
            int Size = egnin.Length;
            if (Size == 0)
            {
                MyMessageBox.Show(owner, "Въведете ЕГН", "Проверка ЕГН", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return false;
            }
            char[] egn = new char[Size + 1];
            egnin.CopyTo(0, egn, 0, egnin.Length);
            int count, br;
            count = 0;
            for (br = 0; br < Size; br++)
            {
                if ((egn[br] >= '0') && (egn[br] <= '9')) count++;
                else
                {
                    MyMessageBox.Show(owner, "Въведете само цифри!", "Проверка ЕГН", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return false;
                }
            }
            if ((count != Size) || (Size != 10))
            {
                MyMessageBox.Show(owner, "ЕГН е 10 цифри!", "Проверка ЕГН", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return false;
            }
            for (br = 0; br < Size; br++)
                switch (egn[br])
                {
                    case '0': dig_str[br] = 0; break;
                    case '1': dig_str[br] = 1; break;
                    case '2': dig_str[br] = 2; break;
                    case '3': dig_str[br] = 3; break;
                    case '4': dig_str[br] = 4; break;
                    case '5': dig_str[br] = 5; break;
                    case '6': dig_str[br] = 6; break;
                    case '7': dig_str[br] = 7; break;
                    case '8': dig_str[br] = 8; break;
                    case '9': dig_str[br] = 9; break;
                }
            S = 0;
            for (int j = 0; j < 9; j++)
                S = S + dig_str[j] * Array_EGN[j];
            d_rem = S % 11;
            if (d_rem == 10) dd = 0;
            else dd = d_rem;
            if (dig_str[9] != dd)
                MyMessageBox.Show(owner, "  Грешен ЕГН!  ", "Проверка ЕГН", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else egnres = true;
            return egnres;
        }

        //------------------------------------------------------------------------------

        public static char DecimalSymbol()
        {
            return Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }

        //------------------------------------------------------------------------------

        private static void getComboData(AutoCompleteStringCollection dataCollection, string query,string cs) //"SELECT DISTINCT..."
        {
            SqlConnection connection = new SqlConnection(cs);
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                adapter.Dispose();
                command.Dispose();
                connection.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dataCollection.Add(row[0].ToString());
                }
                dataCollection.Add("празно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " проблем зареждане на hint");
            }
        }
        
        //------------------------------------------------------------------------------

        public static void loadComboBoxesAC(string sql, ComboBox cb, string CS)
        {
            cb.Items.Clear();
            SqlConnection conComboBox = new SqlConnection(CS);
            conComboBox.Open();
            SqlCommand command = new SqlCommand(sql, conComboBox);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                cb.Items.Add(reader.GetString(0));
            cb.Items.Add("празно");
            conComboBox.Close();
            cb.AutoCompleteMode = AutoCompleteMode.Suggest;// SuggestAppend;
            cb.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection combData = new AutoCompleteStringCollection();
            getComboData(combData, sql,CS);
            cb.AutoCompleteCustomSource = combData;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static void loadListBoxes_string(string sql, string conStr, ListBox lb)
        {
            lb.Items.Clear();
            SqlConnection conListBox = new SqlConnection(conStr);
            conListBox.Open();
            SqlCommand command = new SqlCommand(sql, conListBox);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                lb.Items.Add(reader.GetString(0));

            conListBox.Close();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static string getData(string sql,string cstring)
        {
            string returnValue;
            SqlConnection retrieveData = new SqlConnection(cstring);
            SqlCommand cmd = new SqlCommand(sql, retrieveData);
            retrieveData.Open();
            cmd.CommandText = sql;
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            retrieveData.Close();
            return returnValue;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static bool IsDate(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static DateTime getSQLDate()
        {
            return Convert.ToDateTime(getData("DECLARE @mydate DATETIME SET @mydate = DATEADD(DAY,DATEDIFF(day,0,getdate()),0) select @mydate;",Form1.conStr));
        }

        public static void populateDataGridView(string query, DataGridView dgw, int[] list, bool init, string conString)
        {
            try
            {
                if (init) dgw.DataSource = null;
                SqlConnection PPP = new SqlConnection(conString);
                PPP.Open();
                SqlDataAdapter dataadapter = new SqlDataAdapter(query, PPP);
                DataTable dt = new DataTable();
                dataadapter.Fill(dt);
                PPP.Close();
                dgw.DataSource = dt;
                if (dgw.Name == "dgvTeam")
                {
                    dgw.Columns["спедиция"].Width = 100;
                    dgw.Columns["влекач"].Width = 70;
                    dgw.Columns["ремарке"].Width = 70;
                    dgw.Columns["шофьор 1"].Width = 200;
                    dgw.Columns["шофьор 2"].Width = 200;

                    //dgw.Columns["amount"].Width = 60;

                    //dgw.Columns["kind"].Width = 200;
                    //dgw.Columns["description"].Width = 200;
                    //dgw.Columns["amountBGN"].Width = 60;
                    //dgw.Columns["paymentDate"].Width = 70;
                    //dgw.Columns["amount"].DefaultCellStyle.Format = "0.00";
                    //dgw.Columns["amountBGN"].DefaultCellStyle.Format = "0.00";
                    //dgw.Columns["inputDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    //dgw.Columns["paymentDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    //dgw.FirstDisplayedScrollingRowIndex = dgw.Rows[dgw.RowCount - 1].Index;
                    //dgw.AutoGenerateColumns = true;

                }
                else if (dgw.Name == "dgvRestDrivers")
                {
                    //dgw.Columns["duesID"].Width = 40;
                    //dgw.Columns["inputDate"].Width = 80;
                    //dgw.Columns["names"].Width = 230;
                    //dgw.Columns["total"].Width = 40;
                    //dgw.Columns["amount"].Width = 60;
                    //dgw.Columns["amountBGN"].Width = 60;
                    //dgw.Columns["inputDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    //dgw.Columns["paymentDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    //dgw.AutoGenerateColumns = true;
                }
                
                for (int i = 0; i < list.ToList().Count; i++)
                {
                    dgw.Columns[i].Visible = list[i] == 1 ? true : false;
                    if (dgw.Columns[i].Visible)
                        dgw.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                

               // for (int i = 0; i < dgw.Rows.Count - 1; ++i)
                
                    //for (int j = 0; j < dgw.Columns.Count - 1; ++j)
                //{
                        //Color colour = ColorTranslator.FromHtml(textBox1.Text);
                        // dgw.Rows[i].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(dgw.Rows[i].Cells[7].Value.ToString());
                        //Color.FromArgb(unchecked((int.Parse(dgw.Rows[j].Cells[8].Value.ToString()))));
                        //.ToString(); BackColor = Color.Red;
                        //dgw.Rows[i].Cells[j].Style.BackColor= (Color).ConvertColor. ConvertFromString("#FFF8696B")

               // }
            }
            catch (Exception ee)
            {
                //@@@logAction("error: " + ee.Message + "( " + query + " )");
                MyMessageBox.Show(ee.Message);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    }
    public static class Common
    {
        public static double ConvertToDouble(string Value)
        {
            if (Value == null)
            {
                return 0;
            }
            else
            {
                double OutVal;
                double.TryParse(Value, out OutVal);

                if (double.IsNaN(OutVal) || double.IsInfinity(OutVal))
                {
                    return 0;
                }
                return OutVal;
            }
        }
        public static char DecimalSymbol()
        {
            return Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static string normalizeString(string s)
        {
            if (s.Contains(",") || s.Contains("."))
            {
                if (!s.Contains(DecimalSymbol()))
                {
                    s = s.Replace('.', DecimalSymbol());
                    s = s.Replace(',', DecimalSymbol());
                }
            }
            return s;
        }
    }

    public static class Utilities
    {
        public static void CopyHtmlToClipBoard(string html)
        {
            Encoding enc = Encoding.UTF8;

            string begin = "Version:0.9\r\nStartHTML:{0:000000}\r\nEndHTML:{1:000000}"
              + "\r\nStartFragment:{2:000000}\r\nEndFragment:{3:000000}\r\n";

            string html_begin = "<html>\r\n<head>\r\n"
              + "<meta http-equiv=\"Content-Type\""
              + " content=\"text/html; charset=" + enc.WebName + "\">\r\n"
              + "<title></title>\r\n"
              + "<style>table, th, td {border: 1px solid black;}</style>"
              + "</head>\r\n<body>\r\n"
              + "<!--StartFragment-->";

            string html_end = "<!--EndFragment-->\r\n</body>\r\n</html>\r\n";

            string begin_sample = String.Format(begin, 0, 0, 0, 0);

            int count_begin = enc.GetByteCount(begin_sample);
            int count_html_begin = enc.GetByteCount(html_begin);
            int count_html = enc.GetByteCount(html);
            int count_html_end = enc.GetByteCount(html_end);

            string html_total = String.Format(
              begin
              , count_begin
              , count_begin + count_html_begin + count_html + count_html_end
              , count_begin + count_html_begin
              , count_begin + count_html_begin + count_html
              ) + html_begin + html + html_end;

            DataObject obj = new DataObject();
            obj.SetData(DataFormats.Html, new MemoryStream(
              enc.GetBytes(html_total)));

            Clipboard.SetDataObject(obj, true);
        }
    }



    //------------------------------------------------------------------------------
}
