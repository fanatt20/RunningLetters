using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RunningLetters.Core;

namespace RunningLetters.UI
{
    public partial class MainForm : Form
    {
        private readonly LetterManager _manager = new LetterManager();
        private char _currentChar = 'a';

        public MainForm()
        {
            InitializeComponent();
            _manager.OnOneIteraion += _manager_OnOneIteraion;
        }

        void _manager_OnOneIteraion(string obj)
        {
            this.Invoke(new Action(() => textBox1.Text = obj));

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            _manager.Run();
        }

        private void btnAddLetter_Click(object sender, EventArgs e)
        {
            try
            {
                _manager.AddLetter(_currentChar);
                _currentChar = (char)((int)_currentChar + 1);
                UpdateCollection();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Too many letters");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _manager.OnOneIteraion -= _manager_OnOneIteraion;
            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _manager.Stop();
        }

        private void UpdateCollection()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(_manager.GetCurrentLetterCollection());
        }
        private void btnRemoveLetter_Click(object sender, EventArgs e)
        {
            _manager.TryRemoveLetter();
            UpdateCollection();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = ((LetterDto) listBox1.SelectedItem).Speed.ToString();
            vScrollBar1.Value = ((LetterDto) listBox1.SelectedItem).Speed;

        }
    }
}
