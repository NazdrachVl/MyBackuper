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
	public partial class Form3 : Form
	{
		Repository repo;

		public Form3()
		{
			InitializeComponent();
		}

		public Form3(Repository repo) : this()
		{
			this.repo = repo;
			dataGridUpdate();
		}

		private void dataGridUpdate()
		{
			dataGridView1.Rows.Clear();
			foreach (var item in repo)
			{
				dataGridView1.Rows.Add(item.Key);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			repo.MakeBackup();
			dataGridUpdate();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			repo.RemoveBackup(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString());
			dataGridUpdate();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			repo.RestoreBackup(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString());
		}

		private void Form3_Load(object sender, EventArgs e)
		{

		}
	}
}