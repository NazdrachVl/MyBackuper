using MyBackuper.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyBackuper.Client
{
	public partial class Form1 : Form
	{
		Backuper backuper;

		public Form1()
		{
			InitializeComponent();

			try
			{
				backuper = Backuper.FromConfig();
			}
			catch (Exception)
			{
				MessageBox.Show("Failed to open config file. Creating new...");
				backuper = new Backuper();
			}
			DataGrid_Update();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var d = new Form2();
			if (d.ShowDialog() == DialogResult.OK)
			{
				backuper.AddRepository(d.RepoName, d.Directory, d.BackupDirectory);
				DataGrid_Update();
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			backuper.RemoveRepository(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString());
			DataGrid_Update();
		}

		private void DataGrid_Update()
		{
			dataGridView1.Rows.Clear();
			foreach (var item in backuper)
			{
				dataGridView1.Rows.Add(item.Key, item.Value.DirectoryPath, item.Value.BackupDirectoryPath, item.Value.Trigger);
			}
		}

		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			new Form3(backuper[dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString()]).ShowDialog();
			backuper.SaveConfig();
		}
	}
}
