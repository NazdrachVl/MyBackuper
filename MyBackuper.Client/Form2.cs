using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyBackuper.Classes;

namespace MyBackuper.Client
{
	public partial class Form2 : Form
	{
		public string RepoName { get; set; }

		public string Directory { get; set; }

		public string BackupDirectory { get; set; }

		public BackupTrigger Trigger { get; set; }

		public Form2()
		{
			InitializeComponent();
			comboBox1.DataSource = Enum.GetValues(typeof(BackupTrigger));
		}

		private void Form2_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
			{
				RepoName = textBox3.Text;
				Directory = textBox1.Text;
				BackupDirectory = textBox2.Text;

				BackupTrigger trigger;
				Enum.TryParse<BackupTrigger>(comboBox1.SelectedValue.ToString(), out trigger);
				Trigger = trigger;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				textBox2.Text = folderBrowserDialog1.SelectedPath;
			}
		}
	}
}