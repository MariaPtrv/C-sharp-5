using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Lab5_
{
    public partial class Form1 : Form
    {
        BindingSource bs = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            bs.DataSource = FillList();
            //grid
            grid.AutoGenerateColumns = false;
            grid.DataSource = bs;
            var col1 = new DataGridViewTextBoxColumn
            {
                Width = 100,
                Name = "name",
                HeaderText = "Название",
                DataPropertyName = "name"
            };
            grid.Columns.Add(col1);
            var col2 = new DataGridViewComboBoxColumn
            {
                Width = 100,
                Name = "family",
                HeaderText = "Семья",
                DataPropertyName = "family",
                DataSource = Enum.GetValues(typeof(eFamily))

            };
            grid.Columns.Add(col2);
            var col3 = new DataGridViewTextBoxColumn
            {
                Width = 100,
                Name = "status",
                HeaderText = "Статус",
                DataPropertyName = "status"

            };
            grid.Columns.Add(col3);
            var col4 = new DataGridViewTextBoxColumn
            {
                Width = 100,
                Name = "speakers",
                HeaderText = "Численность, млн",
                DataPropertyName = "speakers"
            };
            grid.Columns.Add(col4);
            //picBox
            picBox.DataBindings.Add("ImageLocation", bs, "img", true);
            picBox.DoubleClick += PicBox_DoubleClick;
            //nav
            nav.BindingSource = bs;
            //chart
            chart.DataSource = from w in bs.DataSource as List<Language>
                               group w by w.StrFamily into g
                               select new { Family = g.Key, Avg = g.Average(w => w.Speakers) };
            chart.Series[0].XValueMember = "Family";
            chart.Series[0].YValueMembers = "Avg";
            chart.Legends.Clear();
            propertyGrid.DataBindings.Add("SelectedObject", bs, "");
            DataBindings.Add("Text", bs, "name");
            chart.Titles.Add("Средняя численность говорящих");
            bs.CurrentChanged += (o, e) => chart.DataBind();
        }

        private void PicBox_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Картинка в формате png|*.png"
            };
            if (opf.ShowDialog() == DialogResult.OK)
            {
                (bs.Current as Language).Img = opf.FileName;
                bs.ResetBindings(false);
            }
        }

        private List<Language> FillList() => new List<Language>
        {
             new Language("Русский", eFamily.Индоевропейская, "в безопасности", 258, "../../img/Рус.png"),
             new Language("Английский", eFamily.Индоевропейская, "в безопасности", 1132, "../../img/Англ.png"),
             new Language("Арабский", eFamily.Nan, "в безопасности", 274, "../../img/Араб.png"),
             new Language("Иврит", eFamily.Сино_тибетская, "в безопасности", 9, "../../img/Иврит.png"),
             new Language("Итальянский", eFamily.Индоевропейская, "в безопасности", 534, "../../img/Итальянс.png"),
             new Language("Китайский", eFamily.Сино_тибетская, "более чем в безопасности", 918, "../../img/Китайск.png"),
             new Language("Корейский", eFamily.Урало_алтайская, "в безопасности", 78, "../../img/Корейск.png"),
             new Language("Французский", eFamily.Индоевропейская, "	в безопасности", 280, "../../img/Франц.png"),
        };

        private void loadButton_Click(object sender, EventArgs e)
        {
            bool flag = true;
            List<Language> fromXML = new List<Language>();
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.InitialDirectory = System.Environment.CurrentDirectory;
            sfd.Filter = "Файл в bin|*.bin|Файл в xml|*.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                BinaryFormatter bin = new BinaryFormatter();
                XmlSerializer xml = new XmlSerializer(typeof(List<Language>));
                using (Stream sw = new FileStream(sfd.FileName, FileMode.Open))
                {
                   if (System.IO.Path.GetExtension(sfd.FileName) == ".bin")
                    bs.DataSource = (List<Language>)bin.Deserialize(sw);
                    if (System.IO.Path.GetExtension(sfd.FileName) == ".xml")
                    //bs.DataSource = (List<Language>)xml.Deserialize(sw);
                    { 
                 //  fromXML = (List<Language>)xml.Deserialize(sw);
                        try
                        {
                            fromXML = (List<Language>)xml.Deserialize(sw);

                        }
                        catch (InvalidOperationException error)
                        {
                            flag = false;
                            MessageBox.Show("Failed to serialize. Reason: " + error.Message);                      
                        }
                        if (flag) {
                            bs.DataSource = fromXML;
                            chart.DataSource = from w in bs.DataSource as List<Language>
                                               group w by w.StrFamily into g
                                               select new { Family = g.Key, Avg = g.Average(w => w.Speakers) };
                            chart.Series[0].XValueMember = "Family";
                            chart.Series[0].YValueMembers = "Avg";
                            foreach (var v in grid.Rows)
                            {
                                foreach (DataGridViewRow row in grid.Rows)
                                {
                                    if (row.Cells[0].Value == null)
                                    {
                                       row.Cells[0].Value = "UnDeRfInDeD";
                                    }

                                    if (row.Cells[2].Value == null || row.Cells[2].Value == "")
                                    {
                                        row.Cells[2].Value = "UnDeRfInDeD";
                                    }
                                }

                            }
                           
                        }
                    }
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = System.Environment.CurrentDirectory;
            sfd.Filter = "Файл в bin|*.bin|Файл в xml|*.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                switch (sfd.FilterIndex)
                {
                    case 1:
                        BinarySerialize(sfd.FileName);
                        break;
                    case 2:
                        SaveXml(sfd.FileName);
                        break;
                }
            }
        }
        private void SaveXml(string file)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Language>));
            using (Stream sw = new FileStream(file, FileMode.Create))
            {
                ser.Serialize(sw, bs.DataSource);
            }
        }

        private void BinarySerialize(string file)
        {
            BinaryFormatter bin = new BinaryFormatter();
            Stream sw = new FileStream(file, FileMode.Create);
            {
                bin.Serialize(sw, bs.DataSource);
            }
            sw.Close();
        }

        private void grid_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var v = grid["name", e.RowIndex].Value;
            if (v == null)
            {
                e.Cancel = true;
                grid.CurrentCell = grid["name", e.RowIndex];
                grid.BeginEdit(true);
                MessageBox.Show("Обязательное поле!!");
            }
        }
        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            int maxSpeakers = 0;
            int num;
            foreach (var v in grid.Rows)
            {
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.Cells[3].Value != null && (int)row.Cells[3].Value > maxSpeakers)
                    {
                        maxSpeakers = (int)row.Cells[3].Value;
                    }
                }
            }
            if (toolStripTextBox1.Text != "")
            {
                if (!Int32.TryParse(toolStripTextBox1.Text, out num))
                {
                    MessageBox.Show("Введите число!");
                }
                else
                {
                    if (Int32.Parse(toolStripTextBox1.Text) >= maxSpeakers)
                    {
                        MessageBox.Show(
                        "Не найдено результатов!");
                    }
                    else
                    {
                        foreach (var v in grid.Rows)
                        {
                            foreach (DataGridViewRow row in grid.Rows)
                            {
                                if (row.Cells[3].Value != null && (int)row.Cells[3].Value <= Int32.Parse(toolStripTextBox1.Text))
                                {
                                    grid.CurrentCell = null;
                                    row.Visible = false;
                                }

                                if (row.Cells[3].Value != null && (int)row.Cells[3].Value > Int32.Parse(toolStripTextBox1.Text))
                                {
                                    row.Visible = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
