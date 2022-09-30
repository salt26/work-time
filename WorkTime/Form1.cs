namespace WorkTime
{
    public partial class Form1 : Form
    {
        private readonly List<string> referenceData = new() { "06:00~15:00", "06:30~15:30", "07:00~16:00", "07:30~16:30", "08:00~17:00",
                "08:30~17:30", "09:00~18:00", "09:30~18:30", "10:00~19:00", "10:30~19:30", "11:00~20:00" };

        private int[] workedMinutes = new int[] { 0, 0, 0, 0, 0 };
        private int[] lateMinutes = new int[] { 0, 0, 0, 0, 0 };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    break;
                case DayOfWeek.Tuesday:
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = true;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    break;
                case DayOfWeek.Wednesday:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = true;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    break;
                case DayOfWeek.Thursday:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = true;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = true;
                    radioButton5.Enabled = false;
                    break;
                case DayOfWeek.Friday:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = true;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = true;
                    break;
                default:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    break;
            }

            string reference = (string)Properties.Settings.Default["reference"];
            int referenceIndex = referenceData.IndexOf(reference);
            if (referenceIndex == -1)
            {
                referenceIndex = 6;
                Properties.Settings.Default["reference"] = "09:00~18:00";
                Properties.Settings.Default.Save();
            }
            comboBox1.SelectedIndex = referenceIndex;
            comboBox2.SelectedIndex = referenceIndex;
            comboBox3.SelectedIndex = referenceIndex;
            comboBox4.SelectedIndex = referenceIndex;
            comboBox5.SelectedIndex = referenceIndex;
            textBox18.Text = DateTime.Now.ToString("MM-dd(ddd) HH:mm");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["reference"] = referenceData[comboBox1.SelectedIndex];
            Properties.Settings.Default.Save();
            comboBox2.SelectedIndex = comboBox1.SelectedIndex;
            comboBox3.SelectedIndex = comboBox1.SelectedIndex;
            comboBox4.SelectedIndex = comboBox1.SelectedIndex;
            comboBox5.SelectedIndex = comboBox1.SelectedIndex;
            MondayGoHomeTime();
            TuesdayGoHomeTime();
            WednesdayGoHomeTime();
            ThursdayGoHomeTime();
            FridayGoHomeTime();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["reference"] = referenceData[comboBox2.SelectedIndex];
            Properties.Settings.Default.Save();
            comboBox3.SelectedIndex = comboBox2.SelectedIndex;
            comboBox4.SelectedIndex = comboBox2.SelectedIndex;
            comboBox5.SelectedIndex = comboBox2.SelectedIndex;
            TuesdayGoHomeTime();
            WednesdayGoHomeTime();
            ThursdayGoHomeTime();
            FridayGoHomeTime();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["reference"] = referenceData[comboBox3.SelectedIndex];
            Properties.Settings.Default.Save();
            comboBox4.SelectedIndex = comboBox3.SelectedIndex;
            comboBox5.SelectedIndex = comboBox3.SelectedIndex;
            WednesdayGoHomeTime();
            ThursdayGoHomeTime();
            FridayGoHomeTime();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["reference"] = referenceData[comboBox4.SelectedIndex];
            Properties.Settings.Default.Save();
            comboBox5.SelectedIndex = comboBox4.SelectedIndex;
            ThursdayGoHomeTime();
            FridayGoHomeTime();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["reference"] = referenceData[comboBox5.SelectedIndex];
            Properties.Settings.Default.Save();
            FridayGoHomeTime();
        }

        private int[] CheckTimeFormat(string time)
        {
            if (!time.Contains(':')) return Array.Empty<int>();
            string[] tokens = time.Split(':');
            int minute = 0, hour = 0;
            if (tokens.Length == 1)
            {
                if (!int.TryParse(tokens[0].Trim(), out minute)) {
                    return Array.Empty<int>();
                }
            }
            else if (tokens.Length == 2 || tokens.Length == 3)
            {
                if (!int.TryParse(tokens[0].Trim(), out hour))
                {
                    return Array.Empty<int>();
                }
                if (!int.TryParse(tokens[1].Trim(), out minute))
                {
                    return Array.Empty<int>();
                }
            }
            else if (tokens.Length > 3) return Array.Empty<int>();

            if (minute < 0 || minute >= 60 || hour < 0)
            {
                return Array.Empty<int>();
            }

            return new int[] { hour, minute };
        }

        private int GetStartTimeInMinute(string referenceTime)
        {
            string[] time = referenceTime.Split('~')[0].Split(':');
            int minute = int.Parse(time[1]);
            int hour = int.Parse(time[0]);
            return hour * 60 + minute;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int[] time = CheckTimeFormat(textBox1.Text);
            if (time.Length == 2)
            {
                //textBox1.Text = time[0] + ":" + time[1];
                workedMinutes[0] = time[0] * 60 + time[1];
                MondayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (textBox1.Text == "")
            {
                workedMinutes[0] = 0;
                textBox11.Text = "";
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                workedMinutes[0] = -1;
                textBox11.Text = "잘못된 입력";
                textBox16.Text = "잘못된 입력";
                textBox17.Text = "잘못된 입력";
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox3.Enabled)
                    textBox3.Focus();
                else if (textBox5.Enabled)
                    textBox5.Focus();
                else if (textBox7.Enabled)
                    textBox7.Focus();
                else if (textBox9.Enabled)
                    textBox9.Focus();
                else
                    textBox2.Focus();
            }
        }

        private void MondayGoHomeTime()
        {
            if (workedMinutes[0] == 0)
            {
                textBox11.Text = "";
                return;
            }
            if (workedMinutes[0] < 4 * 60 || workedMinutes[0] > 12 * 60 || lateMinutes[0] < 0)
            {
                textBox11.Text = "잘못된 입력";
                return;
            }
            int minute = workedMinutes[0] + 60 + lateMinutes[0] + GetStartTimeInMinute(referenceData[comboBox1.SelectedIndex]);
            if (workedMinutes[0] == 12 * 60) minute += 30;
            textBox11.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int[] time = CheckTimeFormat(textBox3.Text);
            if (time.Length == 2)
            {
                //textBox3.Text = time[0] + ":" + time[1];
                workedMinutes[1] = time[0] * 60 + time[1];
                TuesdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (textBox3.Text == "")
            {
                workedMinutes[1] = 0;
                textBox12.Text = "";
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                workedMinutes[1] = -1;
                textBox12.Text = "잘못된 입력";
                textBox16.Text = "잘못된 입력";
                textBox17.Text = "잘못된 입력";
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox5.Enabled)
                    textBox5.Focus();
                else if (textBox7.Enabled)
                    textBox7.Focus();
                else if (textBox9.Enabled)
                    textBox9.Focus();
                else if (textBox2.Enabled)
                    textBox2.Focus();
                else
                    textBox4.Focus();
            }
        }

        private void TuesdayGoHomeTime()
        {
            if (workedMinutes[1] == 0)
            {
                textBox12.Text = "";
                return;
            }
            if (workedMinutes[1] < 4 * 60 || workedMinutes[1] > 12 * 60 || lateMinutes[1] < 0)
            {
                textBox12.Text = "잘못된 입력";
                return;
            }
            int minute = workedMinutes[1] + 60 + lateMinutes[1] + GetStartTimeInMinute(referenceData[comboBox2.SelectedIndex]);
            if (workedMinutes[1] == 12 * 60) minute += 30;
            textBox12.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2");
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            int[] time = CheckTimeFormat(textBox5.Text);
            if (time.Length == 2)
            {
                //textBox5.Text = time[0] + ":" + time[1];
                workedMinutes[2] = time[0] * 60 + time[1];
                WednesdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (textBox5.Text == "")
            {
                workedMinutes[2] = 0;
                textBox13.Text = "";
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                workedMinutes[2] = -1;
                textBox13.Text = "잘못된 입력";
                textBox16.Text = "잘못된 입력";
                textBox17.Text = "잘못된 입력";
            }
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox7.Enabled)
                    textBox7.Focus();
                else if (textBox9.Enabled)
                    textBox9.Focus();
                else if (textBox2.Enabled)
                    textBox2.Focus();
                else if (textBox4.Enabled)
                    textBox4.Focus();
                else
                    textBox6.Focus();
            }
        }

        private void WednesdayGoHomeTime()
        {
            if (workedMinutes[2] == 0)
            {
                textBox13.Text = "";
                return;
            }
            if (workedMinutes[2] < 4 * 60 || workedMinutes[2] > 12 * 60 || lateMinutes[2] < 0)
            {
                textBox13.Text = "잘못된 입력";
                return;
            }
            int minute = workedMinutes[2] + 60 + lateMinutes[2] + GetStartTimeInMinute(referenceData[comboBox3.SelectedIndex]);
            if (workedMinutes[2] == 12 * 60) minute += 30;
            textBox13.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2");
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            int[] time = CheckTimeFormat(textBox7.Text);
            if (time.Length == 2)
            {
                //textBox7.Text = time[0] + ":" + time[1];
                workedMinutes[3] = time[0] * 60 + time[1];
                ThursdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (textBox7.Text == "")
            {
                workedMinutes[3] = 0;
                textBox14.Text = "";
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                workedMinutes[3] = -1;
                textBox14.Text = "잘못된 입력";
                textBox16.Text = "잘못된 입력";
                textBox17.Text = "잘못된 입력";
            }
        }

        private void textBox7_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox9.Enabled)
                    textBox9.Focus();
                else if (textBox2.Enabled)
                    textBox2.Focus();
                else if (textBox4.Enabled)
                    textBox4.Focus();
                else if (textBox6.Enabled)
                    textBox6.Focus();
                else
                    textBox8.Focus();
            }
        }

        private void ThursdayGoHomeTime()
        {
            if (workedMinutes[3] == 0)
            {
                textBox14.Text = "";
                return;
            }
            if (workedMinutes[3] < 4 * 60 || workedMinutes[3] > 12 * 60 || lateMinutes[3] < 0)
            {
                textBox14.Text = "잘못된 입력";
                return;
            }
            int minute = workedMinutes[3] + 60 + lateMinutes[3] + GetStartTimeInMinute(referenceData[comboBox4.SelectedIndex]);
            if (workedMinutes[3] == 12 * 60) minute += 30;
            textBox14.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2");
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            int[] time = CheckTimeFormat(textBox9.Text);
            if (time.Length == 2)
            {
                //textBox9.Text = time[0] + ":" + time[1];
                workedMinutes[4] = time[0] * 60 + time[1];
                FridayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (textBox9.Text == "")
            {
                workedMinutes[4] = 0;
                textBox15.Text = "";
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                workedMinutes[4] = -1;
                textBox15.Text = "잘못된 입력";
                textBox16.Text = "잘못된 입력";
                textBox17.Text = "잘못된 입력";
            }
        }

        private void textBox9_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Enabled)
                    textBox2.Focus();
                else if (textBox4.Enabled)
                    textBox4.Focus();
                else if (textBox6.Enabled)
                    textBox6.Focus();
                else if (textBox8.Enabled)
                    textBox8.Focus();
                else
                    textBox10.Focus();
            }
        }

        private void FridayGoHomeTime()
        {
            if (workedMinutes[4] == 0)
            {
                textBox15.Text = "";
                return;
            }
            if (workedMinutes[4] < 4 * 60 || workedMinutes[4] > 12 * 60 || lateMinutes[4] < 0)
            {
                textBox15.Text = "잘못된 입력";
                return;
            }
            int minute = workedMinutes[4] + 60 + lateMinutes[4] + GetStartTimeInMinute(referenceData[comboBox5.SelectedIndex]);
            if (workedMinutes[4] == 12 * 60) minute += 30;
            textBox15.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2");
        }

        private void WeekTotalWorkTime()
        {
            bool bad = false;
            int totalMinute = 0;
            for (int i = 0; i < 5; i++)
            {
                if (workedMinutes[i] != 0 && (workedMinutes[i] < 4 * 60 || workedMinutes[i] > 12 * 60))
                {
                    bad = true;
                    break;
                }
                totalMinute += workedMinutes[i];
            }
            if (!bad)
            {
                textBox16.Text = (totalMinute / 60).ToString("D2") + ":" + (totalMinute % 60).ToString("D2");
            }
        }


        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox4.Enabled)
                    textBox4.Focus();
                else if (textBox6.Enabled)
                    textBox6.Focus();
                else if (textBox8.Enabled)
                    textBox8.Focus();
                else if (textBox10.Enabled)
                    textBox10.Focus();
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox6.Enabled)
                    textBox6.Focus();
                else if (textBox8.Enabled)
                    textBox8.Focus();
                else if (textBox10.Enabled)
                    textBox10.Focus();
            }
        }

        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox8.Enabled)
                    textBox8.Focus();
                else if (textBox10.Enabled)
                    textBox10.Focus();
            }
        }

        private void textBox8_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox10.Enabled)
                    textBox10.Focus();
            }
        }

        private void textBox10_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                lateMinutes[0] = 0;
                MondayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (int.TryParse(textBox2.Text, out int time) && time >= 0)
            {
                lateMinutes[0] = time;
                MondayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                lateMinutes[0] = -1;
                textBox11.Text = "잘못된 입력";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                lateMinutes[1] = 0;
                TuesdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (int.TryParse(textBox4.Text, out int time) && time >= 0)
            {
                lateMinutes[1] = time;
                TuesdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                lateMinutes[1] = -1;
                textBox12.Text = "잘못된 입력";
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                lateMinutes[2] = 0;
                WednesdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (int.TryParse(textBox6.Text, out int time) && time >= 0)
            {
                lateMinutes[2] = time;
                WednesdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                lateMinutes[2] = -1;
                textBox13.Text = "잘못된 입력";
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text == "")
            {
                lateMinutes[3] = 0;
                ThursdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (int.TryParse(textBox8.Text, out int time) && time >= 0)
            {
                lateMinutes[3] = time;
                ThursdayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                lateMinutes[3] = -1;
                textBox14.Text = "잘못된 입력";
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text == "")
            {
                lateMinutes[4] = 0;
                FridayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else if (int.TryParse(textBox10.Text, out int time) && time >= 0)
            {
                lateMinutes[4] = time;
                FridayGoHomeTime();
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                lateMinutes[4] = -1;
                textBox15.Text = "잘못된 입력";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox11.Clear();
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                int minute = 8 * 60 + 60 + lateMinutes[0] + GetStartTimeInMinute(referenceData[comboBox1.SelectedIndex]);
                textBox11.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                WeekRemainingWorkTime();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox3.Clear();
                textBox4.Clear();
                textBox12.Clear();
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                int minute = 8 * 60 + 60 + lateMinutes[1] + GetStartTimeInMinute(referenceData[comboBox2.SelectedIndex]);
                textBox12.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                WeekRemainingWorkTime();
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox5.Clear();
                textBox6.Clear();
                textBox13.Clear();
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                int minute = 8 * 60 + 60 + lateMinutes[2] + GetStartTimeInMinute(referenceData[comboBox3.SelectedIndex]);
                textBox13.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                WeekRemainingWorkTime();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                textBox7.Clear();
                textBox8.Clear();
                textBox14.Clear();
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                int minute = 8 * 60 + 60 + lateMinutes[3] + GetStartTimeInMinute(referenceData[comboBox4.SelectedIndex]);
                textBox14.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                WeekRemainingWorkTime();
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                textBox9.Clear();
                textBox10.Clear();
                textBox15.Clear();
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                WeekTotalWorkTime();
                WeekRemainingWorkTime();
            }
            else
            {
                textBox9.Enabled = true;
                textBox10.Enabled = true;
                int minute = 8 * 60 + 60 + lateMinutes[4] + GetStartTimeInMinute(referenceData[comboBox5.SelectedIndex]);
                textBox15.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                WeekRemainingWorkTime();
            }
        }

        private void WeekRemainingWorkTime()
        {
            int workday = 0;
            if (!checkBox1.Checked) workday++;
            if (!checkBox2.Checked) workday++;
            if (!checkBox3.Checked) workday++;
            if (!checkBox4.Checked) workday++;
            if (!checkBox5.Checked) workday++;

            bool bad = false;
            int totalMinute = 0;
            for (int i = 0; i < 5; i++)
            {
                if (workedMinutes[i] != 0 && (workedMinutes[i] < 4 * 60 || workedMinutes[i] > 12 * 60))
                {
                    bad = true;
                    break;
                }
                totalMinute += workedMinutes[i];
            }
            int remainingMinute = 8 * 60 * workday - totalMinute;
            if (remainingMinute < 0) remainingMinute = 0;
            if (!bad)
            {
                textBox17.Text = (remainingMinute / 60).ToString("D2") + ":" + (remainingMinute % 60).ToString("D2");
            }

            List<int> futuredays = new() { 0, 0, 0, 0, 0 };
            if (!checkBox1.Checked && workedMinutes[0] == 0 && lateMinutes[0] >= 0) futuredays[0] = 1;
            if (!checkBox2.Checked && workedMinutes[1] == 0 && lateMinutes[1] >= 0) futuredays[1] = 1;
            if (!checkBox3.Checked && workedMinutes[2] == 0 && lateMinutes[2] >= 0) futuredays[2] = 1;
            if (!checkBox4.Checked && workedMinutes[3] == 0 && lateMinutes[3] >= 0) futuredays[3] = 1;
            if (!checkBox5.Checked && workedMinutes[4] == 0 && lateMinutes[4] >= 0) futuredays[4] = 1;

            if (futuredays.Sum() == 1)
            {
                int index = futuredays.IndexOf(1);
                string msg;
                if (remainingMinute > 12 * 60)
                {
                    msg = "무단결근 불가피";
                    switch (index)
                    {
                        case 0:
                            textBox11.Text = msg;
                            break;
                        case 1:
                            textBox12.Text = msg;
                            break;
                        case 2:
                            textBox13.Text = msg;
                            break;
                        case 3:
                            textBox14.Text = msg;
                            break;
                        case 4:
                            textBox15.Text = msg;
                            break;
                    }
                }
                else
                {
                    if (remainingMinute < 4 * 60) remainingMinute = 4 * 60;
                    int minute;
                    switch (index)
                    {
                        case 0:
                            minute = remainingMinute + 60 + lateMinutes[0] + GetStartTimeInMinute(referenceData[comboBox1.SelectedIndex]);
                            if (remainingMinute == 12 * 60) minute += 30;
                            textBox11.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                            break;
                        case 1:
                            minute = remainingMinute + 60 + lateMinutes[1] + GetStartTimeInMinute(referenceData[comboBox2.SelectedIndex]);
                            if (remainingMinute == 12 * 60) minute += 30;
                            textBox12.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                            break;
                        case 2:
                            minute = remainingMinute + 60 + lateMinutes[2] + GetStartTimeInMinute(referenceData[comboBox3.SelectedIndex]);
                            if (remainingMinute == 12 * 60) minute += 30;
                            textBox13.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                            break;
                        case 3:
                            minute = remainingMinute + 60 + lateMinutes[3] + GetStartTimeInMinute(referenceData[comboBox4.SelectedIndex]);
                            if (remainingMinute == 12 * 60) minute += 30;
                            textBox14.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                            break;
                        case 4:
                            minute = remainingMinute + 60 + lateMinutes[4] + GetStartTimeInMinute(referenceData[comboBox5.SelectedIndex]);
                            if (remainingMinute == 12 * 60) minute += 30;
                            textBox15.Text = (minute / 60 % 24).ToString("D2") + ":" + (minute % 60).ToString("D2") + " 이후 퇴근";
                            break;
                    }
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    break;
                case DayOfWeek.Tuesday:
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = true;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    break;
                case DayOfWeek.Wednesday:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = true;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = true;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    break;
                case DayOfWeek.Thursday:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = true;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = true;
                    radioButton5.Enabled = false;
                    break;
                case DayOfWeek.Friday:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = true;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = true;
                    break;
                default:
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    break;
            }

            textBox18.Text = DateTime.Now.ToString("MM-dd(ddd) HH:mm");
        }

        private void textBox1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox1, "Enter를 눌러 아래 칸 입력\nTab을 눌러 오른쪽 칸 입력");
        }

        private void textBox3_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox3, "Enter를 눌러 아래 칸 입력\nTab을 눌러 오른쪽 칸 입력");
        }

        private void textBox5_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox5, "Enter를 눌러 아래 칸 입력\nTab을 눌러 오른쪽 칸 입력");
        }

        private void textBox7_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox7, "Enter를 눌러 아래 칸 입력\nTab을 눌러 오른쪽 칸 입력");
        }

        private void textBox9_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox9, "Enter를 눌러 오른쪽 가장 위 칸 입력\nTab을 눌러 오른쪽 칸 입력");
        }

        private void textBox2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox2, "Enter를 눌러 아래 칸 입력\nTab을 눌러 왼쪽 아래 칸 입력");
        }

        private void textBox4_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox4, "Enter를 눌러 아래 칸 입력\nTab을 눌러 왼쪽 아래 칸 입력");
        }

        private void textBox6_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox6, "Enter를 눌러 아래 칸 입력\nTab을 눌러 왼쪽 아래 칸 입력");
        }

        private void textBox8_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(textBox8, "Enter를 눌러 아래 칸 입력\nTab을 눌러 왼쪽 아래 칸 입력");
        }
    }
}