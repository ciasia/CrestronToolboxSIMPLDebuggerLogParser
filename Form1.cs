// License info and recommendations
//-----------------------------------------------------------------------
// <copyright file="Form1.cs" company="AVPlus Integration Pty Ltd">
//     {c} AV Plus Pty Ltd 2017.
//     http://www.avplus.net.au
//     20170611 Rod Driscoll
//     e: rdriscoll@avplus.net.au
//     m: +61 428 969 608
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//     
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//     
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
//
//     For more details please refer to the LICENSE file located in the root folder 
//      of the project source code;
// </copyright>

namespace CI.CSharp.CrestronLogParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;    
    
    public partial class Form1 : Form
    {
        private string filteredFile = String.Empty;
        private int idx = 0;
        private string str;
        private string[] rows;
        private FileDialog ofd = new OpenFileDialog();
        private List<LogLine> lines = new List<LogLine>();
        private List<string> entries = new List<string>();
        private int prevWidth;
        private int prevHeight;

        public Form1()
        {
            InitializeComponent();
            prevWidth = this.Width;
            prevHeight = this.Height;
            cbTime.Checked = false;
            cbDate.Checked = false;
            cbFileName.Checked = true;
        }

        private void printFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (LogLine l in lines)
            {
                if (checkedListBox1.GetItemCheckState(checkedListBox1.Items.IndexOf(l.Category)) == CheckState.Checked
                    && l.Data.Contains(tbFilter.Text))
                {
                    if (cbDate.Checked)
                        sb.Append(l.Date + " "); // "2023:02:01"
                    if (cbTime.Checked && cbDate.Checked)
                        sb.Append(" ");
                    if (cbTime.Checked)
                        sb.Append(l.Time); // "12:00:00"
                    if (cbTime.Checked || cbDate.Checked)
                        sb.Append(": ");
                    if (true)
                        sb.Append(l.Category + ": ");
                    if (cbFileName.Checked)
                        sb.Append(l.File + " -> ");
                    sb.Append(l.Data + "\n");
                }
            }
            sb.Replace(@"\x0D", "\x0D");
            sb.Replace(@"\x0A", "\x0A");
            filteredFile = sb.ToString();
            richTextBox1.Text = filteredFile;
            //lines = filteredFile.Split(new string[] { "\x0D\x0A" }, StringSplitOptions.None);
            rows = Regex.Split(filteredFile, "\x0D\x0A");
        }

        private void parseFile(string filename)
        {
            this.Text = filename;
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match m = Regex.Match(line, @"(\w+): (.+) # (\d{4}-\d{2}-\d{2}) (\d{2}:\d{2}:\d{2})  # (.+)"); //"Info: CrestronMonitor.exe # 2023-02-13 17:11:42  # Crestron applications already installed"
                    if (m.Length > 1)
                    {
                        lines.Add(new LogLine(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value, m.Groups[4].Value, m.Groups[5].Value));
                        if (!entries.Contains(m.Groups[1].Value))
                        {
                            entries.Add(m.Groups[1].Value);
                            checkedListBox1.Items.Add(m.Groups[1].Value, true);
                        }
                    }
                }
            }
            //lines.Sort();
            printFile();
        }

        private void parseCcfFile(string filename)
        {
            this.Text = filename;
            Dictionary<string, string> IRData = new Dictionary<string, string>();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match m = Regex.Match(line, @"^\[(.*)\]\s*(.*)$"); //[NAME]0000 0000\n\r"
                    if (m.Success)
                    {
                        IRData.Add(m.Groups[1].Value,m.Groups[2].Value);
                    }
                }
            }
            printFile();
        }

        private void ResizeControl(Control sender, int incHeight)
        {
            sender.SetBounds(sender.Location.X, sender.Location.Y + incHeight, sender.Width, sender.Height);
        }
        private void ResizeControl(Control sender, int incHeight, int incWidth)
        {
            sender.SetBounds(sender.Location.X, sender.Location.Y + incHeight, sender.Width + incWidth, sender.Height);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            int incWidth = this.Width - prevWidth;
            int incHeight = this.Height - prevHeight;
            richTextBox1.SetBounds(richTextBox1.Location.X, richTextBox1.Location.Y, richTextBox1.Width + incWidth, richTextBox1.Height + incHeight);
            ResizeControl(checkedListBox1, incHeight, incWidth);
            ResizeControl(tbFilter, incHeight, incWidth);
            ResizeControl(cbFileName, incHeight);
            ResizeControl(cbTime, incHeight);
            ResizeControl(cbDate, incHeight);
            ResizeControl(btnSelectAll, incHeight);
            ResizeControl(btnClearAll, incHeight);
            ResizeControl(label1, incHeight);
            ResizeControl(label2, incHeight);
            prevWidth = this.Width;
            prevHeight = this.Height;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                parseFile(ofd.FileName);
            }
        }

        private void cbDate_CheckedChanged(object sender, EventArgs e)
        {
            printFile();
        }

        private void cbTime_CheckedChanged(object sender, EventArgs e)
        {
            printFile();
        }

        private void cbSigName_CheckedChanged(object sender, EventArgs e)
        {
            printFile();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            printFile();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
            printFile();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            printFile();
        }

        private void btnFilterShow_Click(object sender, EventArgs e)
        {
            /*
             //Regex pattern = new Regex(".*" + + ".*");
             //bool isMatch = pattern.IsMatch("battle");

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                Match m = Regex.Match(entries[i], tbFilter.Text);
                //if()
                checkedListBox1.SetItemChecked(i, false);
            }
            */
            printFile();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tbFilter_Enter(object sender, EventArgs e)
        {
            
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                printFile();
            }
        }

    }
    public class LogLine //: IEquatable<LogLine>, IComparable<LogLine>
    {
        public string Category { get; set; }
        public string File { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Data { get; set; }

        public LogLine(string Category, string File, string Date, string Time, string Data)
            //"Info: CrestronMonitor.exe # 2023-02-13 17:11:42  # Crestron applications already installed"
        {
            this.Category = Category;
            this.File = File;
            this.Date = Date;
            this.Time = Time;
            this.Data = Data;
        }
        public override string ToString()
        {
            return String.Format("{0} {1}: {2} # {3} # {4}", Date, Time, Category, File, Data); 
            //"2023-02-13 17:11:42: Info # CrestronMonitor.exe # Crestron applications already installed"
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            LogLine objAsLine = obj as LogLine;
            if (objAsLine == null) return false;
            else return Equals(objAsLine);
        }
        public int SortByNameAscending(string name1, string name2)
        {
            return name1.CompareTo(name2);
        }
        /*
        public int CompareTo(LogLine compareLine)
        {
            if (compareLine == null)
                return 1;

            else
                return this.Name.CompareTo(compareLine.Name);
        }
        //public override int GetHashCode()
        //{
        //    return 0;
        //}
        public bool Equals(LogLine other)
        {
            if (other == null) return false;
            return (this.Name.Equals(other.Name));
        }
        // Should also override == and != operators.
         * */
    }

}
