using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VocabMaster
{
    class GoogleParser
    {
		public string html;

        public string OUTPUT;

		string word;

        public GoogleParser(string HTML,string  word)
		{
			try
			{

				this.word = word;

				string box = Regex.Match(HTML, @"<div class=""g"">((.|\n)*?)<div class=""g"">").ToString();

				string speech = Regex.Match(box, @">\/(.*?)\/<").ToString().Remove(0, 2);
				speech = speech.Replace("/<", "");

				MatchCollection types = Regex.Matches(box, @"color:#666;padding:5px 0(.*?)<");
				string type = "";
				foreach (Match m in types)
				{
					string t = m.ToString().Replace("<", "").Remove(0, 26);
					type += t + "\n";
				}

				MatchCollection defs = Regex.Matches(box, @"<li(.*?)>(.*?)<");
				string definitions = "";
				foreach (Match m in defs)
				{
					string d = RipTags(m.ToString());
					d = d.Replace("<", "");
					definitions += d + "\n";
				}

				OUTPUT = speech + "\n" + type + "\n" + definitions + "\n";

				html += "<u><b>" + word + ": </b></u>" + "(" + type.Replace("\n", "/") + ")" + "<br>" + definitions.Replace("\n", "<br>") + "<br>";




			}
			catch
			{
				
			}
		}

		private string RipTags(string s)
        {
            s = Regex.Replace(s, "<(.|\n)*?>", "");
            s = Regex.Replace(s, "\n*\n", "\n");
            return s;
        }
    }
}
