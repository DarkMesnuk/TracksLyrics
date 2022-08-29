using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TracksLyrics
{
    public class Parser
    {
        public Parser()
        {
            Lyrics = new List<string>();
            Original = false;
        }

        public List<string> Lyrics { get; set; }

        public bool Original { get; set; }

        public Task Parsing(List<SiteUrl> site)
        {
            List<string> lyrics = new List<string>();
            bool isFind = false;
            try
            {
                using (HttpClientHandler hd1 = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.None })
                {
                    using (var clnt = new HttpClient(hd1))
                    {
                        foreach (var urls in site)
                        {
                            switch (urls.typeSite)
                            {
                                case Site.EnLyrsense:
                                    foreach (var url in urls.urls)
                                    {
                                        try
                                        {
                                            HttpResponseMessage resp = clnt.GetAsync(url).Result;
                                            if (resp.IsSuccessStatusCode)
                                            {
                                                var html = resp.Content.ReadAsStringAsync().Result;

                                                if (!string.IsNullOrEmpty(html))
                                                {
                                                    HtmlDocument doc = new HtmlDocument();
                                                    doc.LoadHtml(html);

                                                    lyrics.Add(doc.DocumentNode.SelectNodes(".//h2[@id='fr_title']")[0].InnerText);
                                                    lyrics.Add(doc.DocumentNode.SelectNodes(".//h2[@id='ru_title']")[0].InnerText);

                                                    lyrics.Add(ToStringLyrics(ClearSpecialSymbol(doc.DocumentNode.SelectNodes(".//p[@id='fr_text']")[0].InnerHtml.Split("</span>"), Site.EnLyrsense, false), Site.EnLyrsense));
                                                    lyrics.Add(ToStringLyrics(ClearSpecialSymbol(doc.DocumentNode.SelectNodes(".//p[@id='ru_text']")[0].InnerHtml.Split("</span>"), Site.EnLyrsense, true), Site.EnLyrsense));

                                                    isFind = true;
                                                }
                                                break;
                                            }
                                        }
                                        catch { }
                                    }
                                    break;

                                case Site.RuMusinfo:
                                    foreach (var url in urls.urls)
                                    {
                                        try
                                        {
                                            HttpResponseMessage resp = clnt.GetAsync(url).Result;
                                            if (resp.IsSuccessStatusCode)
                                            {
                                                var html = resp.Content.ReadAsStringAsync().Result;

                                                if (!string.IsNullOrEmpty(html))
                                                {
                                                    HtmlDocument doc = new HtmlDocument();
                                                    doc.LoadHtml(html);

                                                    lyrics.Add(doc.DocumentNode.SelectNodes(".//td[@id='lyric-src']//div[@class='h3 text-center']")[0].InnerText);
                                                    lyrics.Add(doc.DocumentNode.SelectNodes(".//td[@id='lyric-dst']//div[@class='h3 text-center']")[0].InnerText);

                                                    lyrics.Add(ToStringLyrics(ClearSpecialSymbol(doc.DocumentNode.SelectNodes(".//td[@id='lyric-src']")[0].InnerHtml.Split("<div class=\"line\">"), Site.RuMusinfo), Site.RuMusinfo));
                                                    lyrics.Add(ToStringLyrics(ClearSpecialSymbol(doc.DocumentNode.SelectNodes(".//td[@id='lyric-dst']")[0].InnerHtml.Split("<div class=\"line\">"), Site.RuMusinfo), Site.RuMusinfo));

                                                    isFind = true;
                                                }
                                                break;
                                            }
                                        }
                                        catch { }
                                    }
                                    break;

                                case Site.Lyricshub:
                                    foreach (var url in urls.urls)
                                    {
                                        try
                                        {
                                            HttpResponseMessage resp = clnt.GetAsync(url).Result;
                                            if (resp.IsSuccessStatusCode)
                                            {
                                                var html = resp.Content.ReadAsStringAsync().Result;
                                                if (html == "We couldn't find that page.")
                                                    break;

                                                if (!string.IsNullOrEmpty(html))
                                                {
                                                    HtmlDocument doc = new HtmlDocument();
                                                    doc.LoadHtml(html);

                                                    var title = doc.DocumentNode.SelectNodes(".//h2[@class='lyrtitle']")[0].InnerText.Split('-')[1].ToCharArray().ToList();
                                                    title.RemoveAt(0);

                                                    lyrics.Add(new string(title.ToArray()));
                                                    lyrics.Add(new string(title.ToArray()));

                                                    var lyric = ClearSpecialSymbol(doc.DocumentNode.SelectNodes(".//div[@class='col-sm-12 col-md-12 col-lg-9 cont']")[0].InnerHtml.Split("<div class=\"orlangstr\">"), Site.Lyricshub);

                                                    var enlyric = new List<string>();
                                                    var ruLyric = new List<string>();

                                                    for (int i = 0; i < lyric.Count; i += 2)
                                                    {
                                                        enlyric.Add(lyric[i]);
                                                        ruLyric.Add(lyric[i + 1]);
                                                    }

                                                    lyrics.Add(ToStringLyrics(enlyric, Site.Lyricshub));
                                                    lyrics.Add(ToStringLyrics(ruLyric, Site.Lyricshub));

                                                    isFind = true;
                                                }
                                                break;
                                            }

                                        }
                                        catch { }
                                    }
                                    break;

                                case Site.Genius:
                                    foreach (var url in urls.urls)
                                    {
                                        try
                                        {
                                            HttpResponseMessage resp = clnt.GetAsync(url).Result;
                                            if (resp.IsSuccessStatusCode)
                                            {
                                                var html = resp.Content.ReadAsStringAsync().Result;

                                                if (!string.IsNullOrEmpty(html))
                                                {
                                                    HtmlDocument doc = new HtmlDocument();
                                                    doc.LoadHtml(html);

                                                    lyrics.Add(doc.DocumentNode.SelectNodes(".//div[@id='lyrics-root']//h2[@class='TextLabel-sc-8kw9oj-0 Lyrics__Title-sc-1ynbvzw-0 hHEDka']")[0].InnerText.Split(" Lyrics")[0]);

                                                    var lyric = doc.DocumentNode.SelectNodes(".//div[@id='lyrics-root']")[0].InnerHtml.Split("<div data-lyrics-container=\"true\" class=\"Lyrics__Container-sc-1ynbvzw-6 jYfhrf\">").ToList();
                                                    lyric.RemoveAt(0);
                                                    lyrics.Add(ToStringLyrics(ClearSpecialSymbol(lyric.ToArray(), Site.Genius), Site.Genius));

                                                    isFind = true;
                                                    Original = true;
                                                }
                                                break;
                                            }

                                        }
                                        catch { }
                                    }
                                    break;

                                default:
                                    break;
                            }

                            if (isFind)
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) { }

            Lyrics = lyrics;

            return Task.CompletedTask;
        }

        private static List<string> ClearSpecialSymbol(string[] lyrics, Site site, bool translate = false)
        {
            var resultLyrics = new List<string>();

            switch (site)
            {
                case Site.EnLyrsense:
                    var correctLyrics = new List<string>();
                    bool isDouble = false;
                    for (int i = 0, k = 1; i < lyrics.Length - 1; i++, k++)
                    {
                        var tik = new List<string>();

                        while (tik.Count < 2)
                        {
                            if (!translate)
                                tik = lyrics[i].Split($"<span class=\"highlightLine puzEng line{k}\" line=\"{k}\">").ToList();
                            else
                                tik = lyrics[i].Split($"<span class=\"highlightLine line{k}\" line=\"{k}\">").ToList();

                            if (tik.Count < 2)
                            {
                                isDouble = true;
                                k++;
                            }
                        }

                        if (isDouble)
                        {
                            resultLyrics.Add("#" + tik[1]);
                            isDouble = false;
                        }
                        else
                            resultLyrics.Add(tik[1]);
                    }
                    break;


                case Site.RuMusinfo:
                    correctLyrics = lyrics.ToList();
                    correctLyrics.RemoveAt(0);

                    foreach (var line in correctLyrics)
                        resultLyrics.Add(line.Split("</div>")[0]);

                    break;

                case Site.Lyricshub:
                    correctLyrics = lyrics.ToList();
                    correctLyrics.RemoveAt(0);
                    var list = new List<List<string>>();
                    foreach (var line in correctLyrics)
                        list.Add((line.Split("<span class=\"trslwrd\" tabindex=\"0\">")).ToList());

                    foreach (var line in list)
                    {
                        var correctVerb = new List<string>();
                        string resultLine = "";

                        line.RemoveAt(0);

                        foreach (var verb in line)
                            correctVerb.Add(verb.Split("</span>")[0]);

                        for (int i = 0, k = -1; i < correctVerb.Count; i++)
                        {
                            if (k != 0)
                            {
                                for (char letter = 'А'; letter < 'Я'; letter++)
                                {
                                    if (k != 0 && (correctVerb[i][0] == letter || correctVerb[i][1] == letter))
                                    {
                                        k++;
                                        resultLine += "~";
                                        break;
                                    }
                                }

                                if (k != 0)
                                    for (char letter = 'а'; letter < 'я'; letter++)
                                    {
                                        if (k != 0 && (correctVerb[i][0] == letter || correctVerb[i][1] == letter))
                                        {
                                            k++;
                                            resultLine += "~";
                                            break;
                                        }
                                    }
                            }

                            resultLine += correctVerb[i];
                        }

                        resultLyrics.Add(resultLine);
                    }

                    var originalTranslate = new List<string>();

                    foreach (var line in resultLyrics)
                    {
                        var lyric = line.Split('~');
                        if (lyric.Length != 1)
                        {
                            originalTranslate.Add(lyric[0]);
                            originalTranslate.Add(lyric[1]);
                        }
                        else
                        {
                            originalTranslate.Add("");
                            originalTranslate.Add("");
                        }
                    }

                    resultLyrics = originalTranslate;

                    break;


                case Site.Genius:
                    correctLyrics = lyrics.ToList();

                    foreach (var line in correctLyrics)
                        resultLyrics.Add(line.Split("</div>")[0]);

                    break;

                default:
                    break;
            }
            var interval = resultLyrics.ToArray();
            resultLyrics.Clear();

            // &#x27;
            foreach (var line in interval)
            {
                var correctLine = line.Split("&#x27;");
                var resultLine = "";
                if (correctLine.Length == 1)
                    resultLine = correctLine[0];
                else
                    for (int i = 0; i < correctLine.Length; i++)
                    {
                        if (i == correctLine.Length - 1)
                            resultLine += correctLine[i];
                        else
                            resultLine += correctLine[i] + "`";
                    }

                resultLyrics.Add(resultLine);
            }
            interval = resultLyrics.ToArray();
            resultLyrics.Clear();

            // &quot;
            foreach (var line in interval)
            {
                var correctLine = line.Split("&quot;");
                var resultLine = "";
                if (correctLine.Length == 1)
                    resultLine = correctLine[0];
                else
                    for (int i = 0; i < correctLine.Length; i++)
                    {
                        if (i == correctLine.Length - 1)
                            resultLine += correctLine[i];
                        else
                            resultLine += correctLine[i] + "`";
                    }

                resultLyrics.Add(resultLine);
            }

            interval = resultLyrics.ToArray();
            resultLyrics.Clear();


            // &amp;
            foreach (var line in interval)
            {
                var correctLine = line.Split("&amp;");
                var resultLine = "";
                if (correctLine.Length == 1)
                    resultLine = correctLine[0];
                else
                    for (int i = 0; i < correctLine.Length; i++)
                    {
                        if (i == correctLine.Length - 1)
                            resultLine += correctLine[i];
                        else
                            resultLine += correctLine[i] + "`";
                    }

                resultLyrics.Add(resultLine);
            }

            interval = resultLyrics.ToArray();
            resultLyrics.Clear();

            // <sup>
            foreach (var line in interval)
            {
                var correctLine = line.Split("<sup>");
                var resultLine = "";
                resultLine = correctLine[0];

                resultLyrics.Add(resultLine);
            }

            return resultLyrics;
        }

        private static string ToStringLyrics(List<string> lyrics, Site site)
        {
            var resultLyrics = "";
            switch (site)
            {
                case Site.EnLyrsense:
                    foreach (var line in lyrics)
                        if (line[0] == '#')
                            resultLyrics += "\n" + line.Split('#')[1] + "\n";
                        else
                            resultLyrics += line + "\n";
                    break;


                case Site.RuMusinfo:
                    foreach (var line in lyrics)
                        if (line[0] == '[')
                            resultLyrics += "\n" + line + "\n\n";
                        else
                            resultLyrics += line + "\n";

                    break;

                case Site.Lyricshub:
                    foreach (var line in lyrics)
                        resultLyrics += line + "\n";

                    break;


                case Site.Genius:
                    foreach (var line in lyrics)
                        foreach (var verbs in line.Split("<br>"))
                            resultLyrics += verbs + "\n";

                    break;

                default:
                    break;
            }

            return resultLyrics;

        }

    }
}
