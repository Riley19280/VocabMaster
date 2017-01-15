﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Parser_Development
{
	class ThesaurusParser
	{
		public string builtHTML;

		public bool ISREADY = false;

		public List<string> syns { get; private set; } = new List<string>();
		public List<string> ants { get; private set; } = new List<string>();
		string HTML;

		public string OUTPUT;


		WebBrowser browser = new WebBrowser();

		public ThesaurusParser(string WORD)
		{

			browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(pageLoaded);
			browser.ScriptErrorsSuppressed = true;

			browser.Navigate("http://www.thesaurus.com/browse/+" + WORD);


		}

		private void pageLoaded(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			HTML = browser.DocumentText;

			#region parse
			//relevancy-block((.|\n)*?)<\/ul>
			string regexSyn = @"relevancy-block((.|\n)*?)<\/ul>((.|\n)*?)<\/div>";
			string regexAnt = @"container-info antonyms((.|\n)*?)<\/ul>((.|\n)*?)<\/div>";


			Match msyn = Regex.Match(HTML, regexSyn);
			Match mant = Regex.Match(HTML, regexAnt);

			string regex2 = @"<span class=""text"">((.|\n)*?)<\/span>";

			foreach (Match m in Regex.Matches(msyn.ToString(), regex2))
			{
				Match ma = Regex.Match(m.ToString(), regex2);

				string h = ma.ToString().Remove(0, 19);
				h = h.Remove(h.IndexOf("<"));
				syns.Add(h);

			}

			foreach (Match m in Regex.Matches(mant.ToString(), regex2))
			{
				Match ma = Regex.Match(m.ToString(), regex2);

				string h = ma.ToString().Remove(0, 19);
				h = h.Remove(h.IndexOf("<"));
				ants.Add(h);

			}
			#endregion
			OUTPUT += "Synonyms: ";
			builtHTML += "Synonyms: ";
			try
			{
				for (int i = 0; i < 3; i++)
				{
					OUTPUT += syns[i] + ", ";
					builtHTML += syns[i] + ", ";
				}
			}
			catch { }
			OUTPUT += "\nAntonyms: ";
			builtHTML += "<br>Antonyms: ";
			try
			{
				for (int i = 0; i < 3; i++)
				{
					OUTPUT += ants[i] + ", ";
					builtHTML += ants[i] + ", ";
				}
			}
			catch { }

			builtHTML += "<br><br>";

			ISREADY = true;
		}
	}
}
