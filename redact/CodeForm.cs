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
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace redact
{
    public partial class CodeForm : Form
    {
        public List<string> links = new List<string>(); //ссылки на сборки
        public string openfile = String.Empty;
        public CodeForm()
        {
            InitializeComponent();
            links.Add("System.Core.dll");
            richTextBox1.Text = "System.Core.dll";
            autocompleteMenu1.Items = File.ReadAllLines("cs-reserv-list.dicr"); //динамическое подключение словаря
        }

        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            string text = fastColoredTextBox1.Text;
            string[] lines = text.Split('\n');
            label2.Text = "Символов: " + text.Length.ToString();
            label1.Text = "Строк: " + lines.Length.ToString(); ;
        }

        private void сохранитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSharp source code (*.cs)|*.cs";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            fastColoredTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
            openfile = openFileDialog1.FileName;
        }

        private void оToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "CSharp sourse code (*.cs)| *.cs";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            File.WriteAllText(saveFileDialog1.FileName, fastColoredTextBox1.Text);
        }
        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    File.WriteAllText(openfile, fastColoredTextBox1.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Файла не существует");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Dll file (*.dll)| *.dll";
            if (openFileDialog2.ShowDialog() == DialogResult.Cancel)
                return;
            links.Add(openFileDialog2.FileName);
            richTextBox1.Text += "\n" + openFileDialog2.FileName;
        }

        private void компилироватьКодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.ReadAllText(@"files/run-app-file.cs") != fastColoredTextBox1.Text)
            {
                File.WriteAllText(@"run-app-file.cs", fastColoredTextBox1.Text);
            }
            string sourceName = @"run-app-file.cs";
            FileInfo sourceFile = new FileInfo(sourceName);
            CodeDomProvider provider = null;
            bool compileOK = false;

            provider = CodeDomProvider.CreateProvider("CSharp");

            if (provider != null)
            {
                String exeName = String.Format(@"{0}\{1}.exe",
                    System.Environment.CurrentDirectory,
                    sourceFile.Name.Replace(".", "_"));

                CompilerParameters cp = new CompilerParameters(links.ToArray(), fastColoredTextBox1.Text, true);
                
                cp.GenerateExecutable = true;
                cp.OutputAssembly = exeName;

                cp.GenerateInMemory = false;

                cp.TreatWarningsAsErrors = false;

                CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceName);

                if (cr.Errors.Count > 0)
                {
                    foreach (CompilerError ce in cr.Errors)
                    {
                        MessageBox.Show(ce.ToString());
                    }
                }

                if (cr.Errors.Count > 0)
                {
                    MessageBox.Show("Приложение не запущено");
                }
                else
                {
                    MessageBox.Show("Приложение запущено");
                    Process.Start(@"run-app-file_cs.exe");
                }
            }
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            Close();
        }     

      
        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.TextLength > 0)
            {
                fastColoredTextBox1.Copy();
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.TextLength > 0)
            {
                fastColoredTextBox1.Paste();
            }
        }

        private void выделитьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.TextLength > 0)
            {
                fastColoredTextBox1.SelectAll();
            }
        }

        


        private void fastColoredTextBox1_Load(object sender, EventArgs e)
        {

        }

       

        private void buttonbuttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void открытьМенеджерСсылокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = false; 
        }


    }
}
