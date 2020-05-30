using SoftLiu_TestForLiu.ExcelManagers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_TestForLiu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "Out";
            textBox3.Text = "CENTER";
            textBox4.Text = "OutputAfdxMessages";
            comboBox2.Text = "55";
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            this.textBox1.Text = string.Empty;
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            if (this.textBox1.Text.CompareTo(path) != 0)
            {
                this.textBox1.Text = path; //由一个textBox显示路径
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All; //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<LineData>> allPortID = new Dictionary<string, List<LineData>>();
            Dictionary<string, List<string>> samePortID = new Dictionary<string, List<string>>();

            ReadConfig(allPortID, samePortID);

            #region Show
            int colorIndex = 0;
            Color color = Color.Green;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, List<string>> item in samePortID)
            {
                if (item.Value == null || item.Value.Count <= 1)
                {
                    continue;
                }
                if (colorIndex % 2 == 0)
                {
                    color = Color.Blue;
                }
                else
                {
                    color = Color.Green;
                }
                foreach (string same in item.Value)
                {
                    richTextBox1.SelectionColor = color;
                    richTextBox1.AppendText(same);
                }
                colorIndex++;
            }

            if (colorIndex <= 0)
            {
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.AppendText(string.Format("没有相同的PortID"));
            }
            #endregion

        }

        private void ReadConfig(Dictionary<string, List<LineData>> allPortID, Dictionary<string, List<string>> samePortID)
        {
            string path = this.textBox1.Text;
            if (string.IsNullOrEmpty(path))
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText("文件路径不能为空!");
                return;
            }
            if (allPortID == null)
                allPortID = new Dictionary<string, List<LineData>>();
            if (samePortID == null)
                samePortID = new Dictionary<string, List<string>>();

            allPortID.Clear();
            samePortID.Clear();

            using (StreamReader sr = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read)))
            {
                string line = string.Empty;
                int lineIndex = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    lineIndex++;
                    if (line.Contains("#") || !line.Contains("="))
                    {
                        if (allPortID.ContainsKey("#"))
                        {
                            allPortID["#"].Add(new LineData(false, "", line, "", "", lineIndex));
                        }
                        else
                        {
                            List<LineData> list = new List<LineData>();
                            list.Add(new LineData(false, "", line, "", "", lineIndex));
                            allPortID.Add("#", list);
                        }
                        continue;
                    }
                    string[] data1 = line.Split('=');
                    if (data1.Length == 2)
                    {
                        string data2 = data1[1];
                        string[] data3 = data2.Split('-');
                        if (data3.Length == 3)
                        {
                            string data4 = data3[1];

                            if (allPortID.ContainsKey(data4))
                            {
                                allPortID[data4].Add(new LineData(true, data4, data1[0], data3[0], data3[2], lineIndex, checkBox1.Checked));
                            }
                            else
                            {
                                List<LineData> list = new List<LineData>();
                                list.Add(new LineData(true, data4, data1[0], data3[0], data3[2], lineIndex, checkBox1.Checked));
                                allPortID.Add(data4, list);
                            }

                            if (samePortID.ContainsKey(data4))
                            {
                                samePortID[data4].Add(string.Format("行数：{0} -> {1}\r\n", lineIndex, line));
                            }
                            else
                            {
                                List<string> list = new List<string>();
                                list.Add(string.Format("行数：{0} -> {1}\r\n", lineIndex, line));
                                samePortID.Add(data4, list);
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = string.Empty;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<LineData>> allPortID = new Dictionary<string, List<LineData>>();
            Dictionary<string, List<string>> samePortID = new Dictionary<string, List<string>>();

            ReadConfig(allPortID, samePortID);

            #region Overwrite
            StringBuilder OverwriteSB = new StringBuilder();
            List<int> hasChange = new List<int>();

            Dictionary<int, LineData> lineDataDir = new Dictionary<int, LineData>();

            List<LineData> fileData = new List<LineData>();

            int firstPortID = 0;

            foreach (KeyValuePair<string, List<LineData>> item in allPortID)
            {
                if (item.Value != null)
                {
                    if (item.Value.Count == 1)
                    {
                        int id = 0;
                        if (int.TryParse(item.Value[0].portID, out id))
                        {
                            fileData.Add(item.Value[0]);
                            if (lineDataDir.ContainsKey(id))
                            {
                                while (true)
                                {
                                    if (lineDataDir.ContainsKey(id) || allPortID.ContainsKey(id.ToString()))
                                    {
                                        id++;
                                        continue;
                                    }
                                    break;
                                }
                                lineDataDir.Add(id, item.Value[0]);
                            }
                            else
                            {
                                lineDataDir.Add(id, item.Value[0]);
                            }

                        }
                        else
                        {
                            fileData.Add(item.Value[0]);
                        }
                        continue;
                    }
                    bool isFirst = true;
                    foreach (var data in item.Value)
                    {
                        hasChange.Add(data.lineIndex);

                        if (!data.isPortID)
                        {
                            fileData.Add(data);
                        }
                        else
                        {
                            if (firstPortID <= 0)
                            {
                                bool result = int.TryParse(data.portID, out firstPortID);
                                if (!result) this.richTextBox1.AppendText("TryParse Error: " + string.Format("First:  行数：{0} -> {1}\r\n", data.lineIndex, data.ToString()));
                            }
                            int portID = firstPortID;
                            if (isFirst)
                            {
                                isFirst = false;
                                while (true)
                                {
                                    if (lineDataDir.ContainsKey(portID) || allPortID.ContainsKey(portID.ToString()))
                                    {
                                        portID = ++firstPortID;
                                        continue;
                                    }
                                    break;
                                }                                

                                fileData.Add(data);
                                lineDataDir.Add(portID, data);

                                continue;
                            }
                            do
                            {
                                if (lineDataDir.ContainsKey(portID) || allPortID.ContainsKey(portID.ToString()))
                                {
                                    portID = ++firstPortID;
                                    continue;
                                }
                                data.portID = portID.ToString();
                                fileData.Add(data);

                                lineDataDir.Add(portID, data);
                                firstPortID++;
                                break;
                            }
                            while (true);
                        }
                    }
                }
            }

            fileData.Sort((x, y) => { return x.lineIndex - y.lineIndex; });

            for (int i = 0; i < fileData.Count; i++)
            {
                LineData item = fileData[i];
                OverwriteSB.Append(item.ToString());
                if (i < fileData.Count - 1)
                {
                    OverwriteSB.Append("\n");
                }
            }
            string path = this.textBox1.Text;
            FileInfo fileInfo = new FileInfo(path);
            string newPath = Path.Combine(fileInfo.Directory.FullName, "new" + Path.GetFileNameWithoutExtension(fileInfo.FullName) + fileInfo.Extension);
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            using (StreamWriter sw = new StreamWriter(File.Open(newPath, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                sw.Write(OverwriteSB.ToString());
            }
            #endregion
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            this.textBox2.Text = string.Empty;
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            if (this.textBox2.Text.CompareTo(path) != 0)
            {
                this.textBox2.Text = path; //由一个textBox显示路径
            }
        }

        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All; //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string excelPath = textBox2.Text;
                string sheet = textBox4.Text.Trim();
                string center = textBox3.Text.Trim();
                if (string.IsNullOrEmpty(excelPath) || string.IsNullOrEmpty(sheet)) return;

                DataTable dt = ExcelManager.ReadExcelToDataTable(excelPath, sheet);
                if (dt.Rows.Count <= 1 || dt.Columns.Count <= 1)
                {
                    Console.WriteLine("None Item Information.");
                    return;
                }
                int outOrin = 0;
                switch (comboBox1.Text)
                {
                    case "In":
                        outOrin = 1;
                        break;

                }
                string defaultPortName = "defaultPortName";
                string defaultPortID = "50000";
                List<string> columnNames = new List<string>() { "A653PortName", "DestUDP" };

                string[] centerArray = new string[] { "CENTER", "LIB", "LOB", "ROB", "RIB" };

                string[] portNameArray = new string[dt.Rows.Count - 1];
                string[] portIDArray = new string[dt.Rows.Count - 1];


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string columnName = dt.Rows[0][i].ToString().Trim();

                    switch (columnName)
                    {
                        case "A653PortName":
                            for (int j = 1; j < dt.Rows.Count; j++)
                            {
                                string data = dt.Rows[j][i].ToString().Trim();
                                if (string.IsNullOrEmpty(data))
                                {
                                    data = defaultPortName;
                                }
                                portNameArray[j - 1] = data;
                            }
                            break;
                        case "DestUDP":
                            for (int j = 1; j < dt.Rows.Count; j++)
                            {
                                string data = dt.Rows[j][i].ToString().Trim();
                                if (string.IsNullOrEmpty(data))
                                {
                                    data = defaultPortID;
                                }
                                portIDArray[j - 1] = data;
                            }
                            break;
                    }
                }
                richTextBox1.AppendText(string.Format("name:{0} -> id:{1}\n", portNameArray.Length, portIDArray.Length));

                Dictionary<string, LineData> portInfo = new Dictionary<string, LineData>();
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("[{0}]", sheet));
                sb.Append("\n");
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    string name = portNameArray[i];
                    string id = portIDArray[i];
                    if (portInfo.ContainsKey(name))
                    {
                        richTextBox1.AppendText(string.Format("sane port name:{0}\n", name));
                        continue;
                    }
                    string centerinfo = center;
                    if (outOrin == 0)
                    {
                        Random rd = new Random(i);
                        int index = rd.Next(0, 5);
                        centerinfo = centerArray[index];
                    }
                    LineData line = new LineData(true, id, name, centerinfo, outOrin.ToString(), i);
                    portInfo.Add(name, line);
                    sb.Append(line.ToString());
                    sb.Append("\n");
                }
                sb.Append(string.Format("[END_OF_{0}]", sheet));

                FileInfo info = new FileInfo(excelPath);
                string dic = info.Directory.FullName;
                string writepath = Path.Combine(dic, string.Format("{0}_config_file_Center.ini", sheet));
                if (File.Exists(writepath))
                {
                    File.Delete(writepath);
                }
                using (StreamWriter sw = new StreamWriter(File.Open(writepath, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    sw.Write(sb.ToString());
                }

                richTextBox1.AppendText("End.\n");
            }
            catch (Exception error)
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText(string.Format("Error:{0}\n", error.Message));
            }
        }

        private void textBox5_DragDrop(object sender, DragEventArgs e)
        {
            this.textBox5.Text = string.Empty;
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            if (this.textBox5.Text.CompareTo(path) != 0)
            {
                this.textBox5.Text = path; //由一个textBox显示路径
            }
        }

        private void textBox5_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All; //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = this.textBox5.Text;
            if (string.IsNullOrEmpty(path))
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText("文件路径不能为空!");
                return;
            }


            Dictionary<string, List<LineData>> allPortID = new Dictionary<string, List<LineData>>();

            Dictionary<string, List<string>> samePortID = new Dictionary<string, List<string>>();

            allPortID.Clear();
            samePortID.Clear();

            List<LineData> lins = new List<LineData>();

            int start = 0;
            int.TryParse(comboBox2.Text, out start);
            start = start * 1000;
            int lineIndex = 0;
            using (StreamReader sr = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read)))
            {
                string line = string.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    lineIndex++;
                    LineData dt = new LineData(false, "", "", "", "", lineIndex);
                    if (line.Contains("#") || !line.Contains("="))
                    {
                        dt = new LineData(false, "", line, "", "", lineIndex);
                    }
                    else
                    {
                        string[] data1 = line.Split('=');
                        if (data1.Length == 2)
                        {
                            string data2 = data1[1];
                            string[] data3 = data2.Split('-');
                            if (data3.Length == 3)
                            {
                                string data4 = data3[1];
                                data4 = (start++).ToString();

                                dt = new LineData(true, data4, data1[0], data3[0], data3[2], lineIndex);
                            }
                        }
                    }
                    lins.Add(dt);
                }
            }
            StringBuilder OverwriteSB = new StringBuilder();
            for (int i = 0; i < lins.Count; i++)
            {
                LineData dt = lins[i];
                OverwriteSB.Append(dt.ToString());
                if (i < lins.Count - 1)
                {
                    OverwriteSB.Append("\n");
                }
            }

            FileInfo fileInfo = new FileInfo(path);
            string newPath = Path.Combine(fileInfo.Directory.FullName, "new" + Path.GetFileNameWithoutExtension(fileInfo.FullName) + fileInfo.Extension);
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            using (StreamWriter sw = new StreamWriter(File.Open(newPath, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                sw.Write(OverwriteSB.ToString());
            }
        }
    }
}
