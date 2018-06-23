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

namespace EngRusDict
{
    class ParseCl
    {
        static object Locker = new object();
        //public Dictionary<string, List<string>> valuePairs;
        //public string link;
        //public string htm;
        //public string html1;
        //public ParseCl(string link1,string htm1,string html2,Dictionary<string,List<string>> value)
        //{
        //    link = link1;
        //    htm = htm1;
        //    html1 = html2;
        //    valuePairs = value;

        //}
        public void Web(string link, ref string htm)
        {
            var webClient = new System.Net.WebClient();
            webClient.Encoding = Encoding.UTF8;
            htm = webClient.DownloadString(link);
        }
        public void Parsing(string html1, ref Dictionary<string, List<string>> valuePairs)
        {
            bool check = false;
            var pars = new HtmlParser();
            var source = html1;
            var document = pars.Parse(source);
            var lilist = document.QuerySelectorAll("li");
            Regex regex1 = new Regex(@"/.*/");
            Regex regex2 = new Regex(@",");
            Regex regex3 = new Regex(@"\s*");
            foreach (var item in lilist)
            {
                check = false;
                List<string> vs = new List<string>();
                string s = item.TextContent.ToString();
                string word = s.Substring(0, s.IndexOf(' '));
                word = word.Trim(' ');
                //rd = word.TrimEnd(' ');
                string translates = s.Substring(s.IndexOf('-') + 2, s.Length - (s.IndexOf('-') + 2));
                translates = regex1.Replace(translates, "");
                foreach (string tr in regex2.Split(translates))
                {
					string s2 = tr.Trim(' ');
                    vs.Add(s2);
                }
                lock (Locker)
                {
                    foreach (KeyValuePair<string, List<string>> hh in valuePairs)
                    {
                        if (hh.Key == word)
                        {
                            check = true;
                            foreach (string th in vs)
                            {
                                valuePairs[word].Add(th);
                            }
                        }
                    }
                    if (check == false)
                    {
                        valuePairs.Add(word, vs);
                    }
                }
            }
        }
    }
}
