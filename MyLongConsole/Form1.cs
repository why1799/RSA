using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLongConsole
{
    public partial class RSA : Form
    {
        private const int _base = 4;

        public RSA()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int bits = int.Parse(comboBox1.Text.Substring(0, comboBox1.Text.IndexOf(' ')));
            LongArithmetic bitsla = 1;

            for(int i = 0; i < bits;  i++)
            {
                bitsla *= 2;
            }

            global::RSA.Times = bitsla.ToString().Count() / _base;

            LongArithmetic p = GetRandom("p");
            Console.WriteLine();
            LongArithmetic q = GetRandom("q");
            Console.WriteLine();

            textBox1.Text = p.ToString();
            textBox2.Text = q.ToString();
            Console.ForegroundColor = ConsoleColor.Red;
            textBox3.Text = (p * q).ToString();
            Console.WriteLine("n=" + textBox3.Text);

            LongArithmetic fn = (p - 1) * (q - 1);
            Console.WriteLine("ф(n)=" + fn.ToString());
            textBox4.Text = fn.ToString();

            LongArithmetic _e = 1, d = 1;
            bool got = false;

            LongArithmetic x = new LongArithmetic();
            LongArithmetic y = new LongArithmetic();

            global::RSA.Times = global::RSA.Times * 5 / 4;

            Console.WriteLine("Вычисление e и k:");
            while (!got)
            {
                _e = global::RSA.Random();

                LongArithmetic z = global::RSA.egcd(fn, _e, ref x, ref y);

                if(z == 1 && y > 0 && (_e * y) % fn == 1)
                {
                    d = y;
                    //Console.WriteLine(d * _e % fn);
                    got = true;
                }
            }

            if(!got)
            {
                Console.WriteLine("Вычислить e и k не удалось:");
            }
            else
            {
                Console.WriteLine("e={0}", _e);
                Console.WriteLine("k={0}", d);

                textBox5.Text = _e.ToString();
                textBox6.Text = d.ToString();
            }
        }

        private LongArithmetic GetRandom(string name)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Вычисление {0}:", name);
            Console.ForegroundColor = ConsoleColor.White;

            bool exit = false;
            LongArithmetic j = new LongArithmetic(); ;
            LongArithmetic pr = new LongArithmetic();
            for (int i = 0; !exit; i++)
            {
                j = global::RSA.Random();

                if (j == pr)
                    continue;

                pr = j;

                exit = global::RSA.ferma(j);

                if (exit)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine(String.Format("{2}{0}{1}простое число", j, exit ? " " : " не ", exit ? name + "=" : ""));
                Console.ForegroundColor = ConsoleColor.White;
            }
            return j;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Введите n");
                return;
            }

            if (textBox5.Text == "")
            {
                MessageBox.Show("Введите e");
                return;
            }

            LongArithmetic _n = new LongArithmetic();
            LongArithmetic _e = new LongArithmetic();

            try
            {
                _n = LongArithmetic.Parse(textBox3.Text);
                _e = LongArithmetic.Parse(textBox5.Text);
            }
            catch
            {
                MessageBox.Show("Числа невозможно считать!");
                return;
            }

            Console.WriteLine("Начало шифрования!");

            StringBuilder sb = new StringBuilder();

            List<ushort> list = new List<ushort>();
            LongArithmetic text = new LongArithmetic();
            LongArithmetic criptedtext = new LongArithmetic();
            richTextBox2.Clear();
            foreach (var c in richTextBox1.Text)
            {
                if ((int)c > 9999 || (int)c < 0)
                {
                    MessageBox.Show("Этот текст содержить невозможные для чтения символы!");
                    return;
                }
                list.Add((ushort)c);
                if(list.Count == 6)
                {
                    text = new LongArithmetic(list);
                    criptedtext = global::RSA.mypows(text, _e, _n);
                    richTextBox2.AppendText(criptedtext.ToString() + "\n");
                    list.Clear();
                }
            }

            if (list.Count > 0)
            {
                text = new LongArithmetic(list);
                criptedtext = global::RSA.mypows(text, _e, _n);
                richTextBox2.AppendText(criptedtext.ToString() + "\n");
            }
            Console.WriteLine("Шифровка закончена!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Введите n");
                return;
            }

            if (textBox6.Text == "")
            {
                MessageBox.Show("Введите d");
                return;
            }

            LongArithmetic _n = new LongArithmetic();
            LongArithmetic _d = new LongArithmetic();

            try
            {
                _n = LongArithmetic.Parse(textBox3.Text);
                _d = LongArithmetic.Parse(textBox6.Text);
            }
            catch
            {
                MessageBox.Show("Числа невозможно считать!");
                return;
            }

            Console.WriteLine("Начало дешифровки!");

            StringBuilder sbtext = new StringBuilder();

            foreach (string stext in richTextBox2.Lines)
            {
                if(stext == null || stext == "")
                {
                    continue;
                }

                LongArithmetic criptedtext = LongArithmetic.Parse(stext);
                LongArithmetic text = global::RSA.mypows(criptedtext, _d, _n);

                StringBuilder sb = new StringBuilder();

                foreach (ushort x in text)
                {
                    sb.Append(((char)x).ToString());
                }

                sbtext.Append(sb.ToString());
            }

            richTextBox1.Text = sbtext.ToString();

            Console.WriteLine("Дешифровка закончена!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BinaryFormatter bf = new BinaryFormatter();
            saveFileDialog1.FileName = "Keys";
            saveFileDialog1.Filter = "Keys(*.kkk)|*.kkk|Все файлы(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Save s = new Save();
                s.n = textBox3.Text;
                s.e = textBox5.Text;
                s.d = textBox6.Text;
                FileStream fs = new FileStream(saveFileDialog1.FileName.ToString(), FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, s);
                fs.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "Keys(*.kkk)|*.kkk|Все файлы(*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName.ToString(), FileMode.Open, FileAccess.Read);
                    Save s = (Save)bf.Deserialize(fs);
                    fs.Close();
                    textBox3.Text = s.n;
                    textBox5.Text = s.e;
                    textBox6.Text = s.d;

                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox4.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("К сожалению, программа не может открыть этот файл!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Текстовый документ(*.txt)|*.txt|Все файлы(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                System.IO.File.WriteAllText(filename, richTextBox1.Text);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Текстовый документ(*.txt)|*.txt|Все файлы(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                string fileText = System.IO.File.ReadAllText(filename);
                richTextBox1.Text = fileText;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Зашифрованный документ(*.txtс)|*.txtc|Все файлы(*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                System.IO.File.WriteAllText(filename, richTextBox2.Text);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Зашифрованный документ(*.txtс)|*.txtc|Все файлы(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                string fileText = System.IO.File.ReadAllText(filename);
                richTextBox2.Text = fileText;
            }
        }
    }
}
