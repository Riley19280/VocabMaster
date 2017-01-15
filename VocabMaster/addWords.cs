using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VocabMaster
{
	public partial class addWords : Form
	{
		main main;
		public addWords(main m)
		{
			InitializeComponent();

			main = m;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			string[] lines = richTextBox1.Lines;
			foreach (string s in lines)
			{
				if (s != "" && s != Environment.NewLine && s != "\n\r" && s != "\n" && s != "\r" && Regex.IsMatch(s,@"\S" ))
					main.words.Add(s);
			}
			main.setStatus(main.words.Count + " words added!");

			this.Close();

		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

	}
}
