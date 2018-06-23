using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using AngleSharp.Extensions;
using AngleSharp;
using System.Threading;
using System.Xml.Serialization;

namespace EngRusDict
{
	class Program
	{
		static void Thread11(Queue<string> links,ref Dictionary<string, List<string>> valuePairs1)
		{
			ParseCl cll = new ParseCl();
			while (links.Count != 0)
			{
				string link1 = links.Dequeue();
				string source = "";
				cll.Web(link1, ref source);
				cll.Parsing(source, ref valuePairs1);
			}
		}

		static void Thread22(Queue<string> links,ref Dictionary<string, List<string>> valuePairs1)
		{
			ParseCl cll = new ParseCl();
			while (links.Count != 0)
			{
				string link1 = links.Dequeue();
				string source = "";
				cll.Web(link1, ref source);
				cll.Parsing(source, ref valuePairs1);
			}
		}

		static void Thread33(Queue<string> links,ref Dictionary<string, List<string>> valuePairs1)
		{
			ParseCl cll = new ParseCl();
			while (links.Count != 0)
			{
				string link1 = links.Dequeue();
				string source = "";
				cll.Web(link1, ref source);
				cll.Parsing(source, ref valuePairs1);

			}
		}

		static void Thread44(Queue<string> links,ref Dictionary<string, List<string>> valuePairs1)
		{
			ParseCl cll = new ParseCl();
			while (links.Count != 0)
			{
				string link1 = links.Dequeue();
				string source = "";
				cll.Web(link1, ref source);
				cll.Parsing(source, ref valuePairs1);
			}
		}

		static void Main(string[] args)
		{
			var webClient = new System.Net.WebClient();
			webClient.Encoding = Encoding.UTF8;
			string html1 = webClient.DownloadString("http://www.lexicons.ru/modern/a/english/eng-rus-a.htm");
			var pars = new HtmlParser();
			var source = html1;
			var document = pars.Parse(source);
			Queue<string> links = new Queue<string>();
			var urls = document.QuerySelectorAll("div.body div a");
			foreach(var item in urls)
			{
				string s = item.GetAttribute("href");
				if (s.Contains("eng-rus"))
				{
					links.Enqueue(s);
				}
			}
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
			Thread thred1 = new Thread(delegate () { Thread11(links,ref dict); });
			Thread thred2 = new Thread(delegate () { Thread22(links,ref dict); });
			Thread thred3 = new Thread(delegate () { Thread33(links,ref dict); });
			Thread thred4 = new Thread(delegate () { Thread44(links,ref dict); });
			thred1.Start();
			thred2.Start();
			thred3.Start();
			thred4.Start();
            while (thred1.IsAlive || thred2.IsAlive || thred3.IsAlive || thred4.IsAlive) { }
            using (StreamWriter hh = new StreamWriter("resultdict.txt"))
            {
                int i = 1;
                foreach (KeyValuePair<string, List<string>> res in dict)
                {
                    hh.Write(i + "."+res.Key+" - ");
                    foreach (string ss in res.Value)
                    {
                        hh.Write(ss+", ");
                    }
                    hh.WriteLine();
                    i++;
                }
            }
            Console.WriteLine("Словарь составлен.");
            Console.ReadKey();
		}
	}
}
