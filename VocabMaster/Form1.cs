using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Parser_Development;
using System.Diagnostics;

namespace VocabMaster
{
	public partial class main : Form
	{
		public string builtHtml;

		bool useThesaurus = false;

		public List<string> words = new List<string>();

		WebBrowser browser = new WebBrowser();
		string dwnld;
		string displayString = "";

		public main()
		{
			InitializeComponent();
			browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(pageLoaded);
			browser.ScriptErrorsSuppressed = true;
		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			start();
		}



		private void parseHTML()
		{
			GoogleParser g = new GoogleParser(dwnld, words[0]);

			displayString = words.ElementAt(0) + "\n\n" + g.OUTPUT;
			builtHtml += g.html;
		}

		private void sendHTMLrequest()
		{
			if (words.Count > 0)
			{
				words[0] = words[0].Replace(' ', '+');
				setStatus("Getting " + words[0]);

				browser.Navigate("http://www.google.com/#q=define+" + words[0]);

			}

		}

		private void pageLoaded(object sender, WebBrowserDocumentCompletedEventArgs e)
		{

			if (words.Count > 0)
			{
				dwnld = browser.DocumentText;
				setStatus("Downloaded " + words[0]);
				parseHTML();

				if (useThesaurus)
				{
					ThesaurusParser tp = new ThesaurusParser(words[0]);
					while (!tp.ISREADY)
						Application.DoEvents();

					displayString += "\n" + tp.OUTPUT;
					builtHtml += tp.builtHTML;

					rtList.AppendText(displayString + "\n");
					words.RemoveAt(0);
					sendHTMLrequest();

				}
				else
				{
					rtList.AppendText(displayString + "\n");
					words.RemoveAt(0);
					sendHTMLrequest();

				}

			}
			else
			{
				setStatus("Operation Finished");
			}
		}


		private void loadWordsFromFile()
		{
			words.Clear();
			int counter = 0;
			string line;
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Choose a text file with a list of words";

			ofd.DefaultExt = "*.txt";
			ofd.Filter = "TXT Files|*.txt";

			// Read the file and display it line by line. 
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{

					StreamReader file = new StreamReader(ofd.OpenFile());
					while ((line = file.ReadLine()) != null)
					{
						words.Add(line);
						counter++;
					}

					file.Close();
					setStatus(counter + " words read from file");
				}
				catch (Exception)
				{

					throw;
				}
			}
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			loadWordsFromFile();
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{

			addWords v = new addWords(this);

			v.ShowDialog();
		}

		void start()
		{
			if (words.Count > 0)
			{
				//reinit

				dwnld = "";
				displayString = "";
				setStatus("");
				builtHtml = "";
				rtList.Clear();

				sendHTMLrequest();
			}
			else {
				MessageBox.Show("Please add some words before starting!");
			}
		}

		public void setStatus(string s)
		{
			statusLabel.Text = s;
			statusBar.Refresh();
		}

		private void rtList_TextChanged(object sender, EventArgs e)
		{
			rtList.SelectionStart = rtList.TextLength;
			rtList.ScrollToCaret();
		}

		#region Menu Items
		private void startToolStripMenuItem_Click(object sender, EventArgs e)
		{
			start();
		}
		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Help v = new Help();
			v.ShowDialog();

		}

		private void addItemsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			addWords v = new addWords(this);

			v.ShowDialog();
		}

		private void openListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			loadWordsFromFile();

		}

		private void clearDisplayToolStripMenuItem_Click(object sender, EventArgs e)
		{
			rtList.Clear();
			setStatus("");
		}

		private void webpageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFile1 = new SaveFileDialog();

			saveFile1.DefaultExt = "*.html";
			saveFile1.Filter = "HTML Files|*.html";

			if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
			{
				File.WriteAllText(saveFile1.FileName, "<html>" + builtHtml + "</html>");
				setStatus("File Saved: " + saveFile1.FileName);
				Process.Start(saveFile1.FileName);

			}
		}

		private void textDocumentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFile1 = new SaveFileDialog();

			saveFile1.DefaultExt = "*.txt";
			saveFile1.Filter = "TXT Files|*.txt";

			if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
			{
				rtList.SaveFile(saveFile1.FileName);
				setStatus("File Saved: " + saveFile1.FileName);
				Process.Start(saveFile1.FileName);
			}
		}

		private void restartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			rtList.Clear();
			words.Clear();
			setStatus("");
		}


		#endregion

		private void chkSyn_CheckedChanged(object sender, EventArgs e)
		{
			if (chkSyn.Checked)
			{
				useThesaurus = true;
				synonymsToolStripMenuItem.Checked = true;
			}
			else {
				useThesaurus = false;
				synonymsToolStripMenuItem.Checked = false;
			}
		}

		private void synonymsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (synonymsToolStripMenuItem.Checked)
			{
				useThesaurus = true;
				chkSyn.Checked = true;
			}
			else {
				useThesaurus = false;
				chkSyn.Checked = false;
			}
		}
	}
}
