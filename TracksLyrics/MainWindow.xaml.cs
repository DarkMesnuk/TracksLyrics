﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TracksLyrics
{
    public class Track
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string ruTitle {get; set;}

        public string enLyrics { get; set; }
        public string ruLyrics { get; set; }
    }

    public class SiteUrl
    {
        public Site typeSite { get; set; }
        public List<string> urls { get; set; }
    }

    public enum Site
    {
        EnLyrsense,
        RuMusinfo,
        Lyricshub
    }

    public partial class MainWindow : Window
    {
        #region static variable
        private static List<Track> tracks;
        private static string path = @"C:\TrackList.txt";
        private static string title, artist, search;
        private static int sort_Position;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            tracks = new List<Track>();
            UploadFromFile();
            sort_Position = -1;
        }
        /**/
        #region Button
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateTrack(new Track() { Artist = artist, Title = title });

            ImportTitle.Text = "";
            ImportArtist.Text = "";
        }

        private void Reload_Button_Click(object sender, RoutedEventArgs e)
        {
            var listitem = sender as Button;
            var track = listitem.DataContext as Track;

            List_Track.SelectedItem = listitem.DataContext;
            List_Track.Items.Remove(List_Track.SelectedItem);


            tracks.Remove(track);
            CreateTrack(new Track() { Artist = track.Artist, Title = track.Title });

            SaveToFile();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            var listitem = sender as Button;
            var track = listitem.DataContext as Track;
            List_Track.SelectedItem = listitem.DataContext;
            List_Track.Items.Remove(List_Track.SelectedItem);

            tracks.Remove(track);
            SaveToFile();
        }

        private void Set_Button_Click(object sender, RoutedEventArgs e)
        {
            var listitem = sender as Button;
            var track = listitem.DataContext as Track;
            ImportTitle.Text = track.Title;
            ImportArtist.Text = track.Artist;
        }

        private void Show_Lyrics_Button_Click(object sender, RoutedEventArgs e)
        {
            var listitem = sender as Button;
            var track = listitem.DataContext as Track;

            Lyrics.Items.Clear();

            string lyrics = track.ruLyrics;

            Lyrics.Items.Add(track.Title);
            Lyrics.Items.Add(track.ruTitle);
            Lyrics.Items.Add("");

            var enLyric = track.enLyrics.Split('\n');
            var ruLyric = track.ruLyrics.Split('\n');
            
            for(int i = 0; i < enLyric.Length; i++)
            {
                Lyrics.Items.Add(enLyric[i]);
                Lyrics.Items.Add(ruLyric[i]);
                Lyrics.Items.Add("");
            }
        }

        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            var sorted = new List<Track>();

            sort_Position++;
            if (sort_Position == 4)
                sort_Position = 0;

            switch (sort_Position)
            {
                case 0:
                    sorted = tracks.OrderBy(x => x.Title).ToList();
                    break;

                case 1:
                    sorted = tracks.OrderByDescending(x => x.Title).ToList();
                    break;

                case 2:
                    sorted = tracks.OrderBy(x => x.Artist).ToList();
                    break;

                case 3:
                    sorted = tracks.OrderByDescending(x => x.Artist).ToList();
                    break;

                default:
                    sorted = tracks.OrderBy(x => x.Title).ToList();
                    break;
            }

            tracks.Clear();
            List_Track.Items.Clear();

            foreach(var track in sorted)
            {
                tracks.Add(track);
                List_Track.Items.Add(track);
            }

            SaveToFile();
        }

        private void Sort()
        {
            var sorted = new List<Track>();
            switch (sort_Position)
            {
                case 0:
                    sorted = tracks.OrderBy(x => x.Title).ToList();
                    break;

                case 1:
                    sorted = tracks.OrderByDescending(x => x.Title).ToList();
                    break;

                case 2:
                    sorted = tracks.OrderBy(x => x.Artist).ToList();
                    break;

                case 3:
                    sorted = tracks.OrderByDescending(x => x.Artist).ToList();
                    break;

                default:
                    sorted = tracks.ToList();
                    break;
            }

            tracks.Clear();
            List_Track.Items.Clear();

            foreach (var track in sorted)
            {
                tracks.Add(track);
                List_Track.Items.Add(track);
            }

            SaveToFile();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            search = Search.Text.ToLower();

            if (search != "")
            {
                var sorted = new List<Track>();
                if (search.Split("-").Length != 2)
                {
                    sorted = tracks.Where(x => x.Title.Remove(search.Length).ToLower() == search).ToList();
                }
                else
                {
                    var searchSplit = search.Split("-");
                    sorted = tracks.Where(x => x.Title.Remove(searchSplit[0].Length).ToLower() == searchSplit[0]).ToList();
                    sorted = sorted.Where(x => x.Artist.Remove(searchSplit[1].Length).ToLower() == searchSplit[1]).ToList();
                }

                List_Track.Items.Clear();


                if (sorted != null)
                {
                    foreach (var track in sorted)
                    {
                        List_Track.Items.Add(track);
                    }
                }
            }
            else
            {
                List_Track.Items.Clear();
                if (tracks != null)
                {
                    foreach (var track in tracks)
                    {
                        List_Track.Items.Add(track);
                    }
                }
            }
        }
        #endregion
        /**/
        #region ImportData

        private void CreateTrack(Track track)
        {
            if (!tracks.Where(x => x.Artist == track.Artist).Select(x => x.Title).Contains(track.Title))
            {
                var parsingInformation = Parsing(CreateUrls(track));

                if (parsingInformation != null && parsingInformation.Any())
                {
                    track.Title = parsingInformation[0];
                    track.ruTitle = parsingInformation[1];

                    track.enLyrics = parsingInformation[2];
                    track.ruLyrics = parsingInformation[3];

                    List_Track.Items.Add(track);
                    tracks.Add(track);
                    SaveToFile();
                }

                Sort();
            }
        }

        private List<SiteUrl> CreateUrls(Track track)
        {

            var urls = new List<SiteUrl>();

            var siteRuMusinfo = "https://ru.musinfo.net/lyrics/" + DeleteSpace(track.Artist, Site.RuMusinfo) + "/" + DeleteSpace(track.Title, Site.RuMusinfo);
            urls.Add(new SiteUrl
            {
                typeSite = Site.RuMusinfo,
                urls = new List<string>
                {
                    siteRuMusinfo,
                }
            });


            string urlArtist = TheArtist(DeleteSpace(track.Artist, 0));
            var artistArticle = ArtistArticle(urlArtist);
            var siteEnLyrsense = "https://en.lyrsense.com/" + urlArtist + "/" + DeleteSpace(track.Title, Site.EnLyrsense);
            urls.Add(new SiteUrl
            {
                typeSite = Site.EnLyrsense,
                urls = new List<string>
                {
                    siteEnLyrsense,
                    siteEnLyrsense + "_",
                    siteEnLyrsense + "_" + artistArticle,
                    siteEnLyrsense + "_" + urlArtist,
                    siteEnLyrsense + "_" + urlArtist[0],
                    siteEnLyrsense + "_" + urlArtist[0] + urlArtist[1],
                }
            });


            var siteLyricshub = "https://lyricshub.ru/track/" + DeleteSpace(track.Artist, Site.Lyricshub) + "/" + DeleteSpace(track.Title, Site.Lyricshub) + "/translation";
            urls.Add(new SiteUrl
            {
                typeSite = Site.Lyricshub,
                urls = new List<string>
                {
                    siteLyricshub,
                }
            });

            return urls;
        }

        private static string DeleteSpace(string line, Site site)
        {
            switch (site)
            {
                case Site.EnLyrsense:
                    char[] nameByChar = line.ToCharArray();

                    List<char> corectNameByChar = new List<char>();

                    foreach (var token in nameByChar)
                    {
                        if (token == ' ')
                            corectNameByChar.Add('_');
                        else
                            corectNameByChar.Add(token);
                    }
                    return new string(corectNameByChar.ToArray()).ToLower();
                    break;


                case Site.RuMusinfo:
                    nameByChar = line.ToCharArray();

                    corectNameByChar = new List<char>();

                    foreach (var token in nameByChar)
                    {
                        if (token == ' ')
                            corectNameByChar.Add('-');
                        else
                            corectNameByChar.Add(token);
                    }
                    return new string(corectNameByChar.ToArray()).ToLower();
                    break;

                case Site.Lyricshub:
                    nameByChar = line.ToCharArray();

                    corectNameByChar = new List<char>();

                    foreach (var token in nameByChar)
                    {
                        if (token == ' ')
                            corectNameByChar.Add('-');
                        else
                            corectNameByChar.Add(token);
                    }
                    return new string(corectNameByChar.ToArray());
                    break;

                default:
                    return null;
                    break;
            }
        }

        private static string TheArtist(string artist)
        {
            if (artist.Split('_')[0] == "the")
                artist = artist.Remove(0, 4) + "_the";

            return artist;
        }

        private string ArtistArticle(string artist)
        {
            var article = artist.Split("_").ToList();
            string result = "";

            if (article[article.Count - 1] == "the")
                article.RemoveAt(article.Count - 1);

            foreach (var verb in article)
                result += verb[0];

            return result.ToLower();
        }

        public static List<string> Parsing(List<SiteUrl> site)
        {
            List<string> lyrics = new List<string>();
            bool isFind = false;
            try
            {
                using (HttpClientHandler hd1 = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.None })
                {
                    using (var clnt = new HttpClient(hd1))
                    {
                        foreach(var urls in site)
                        {
                            switch (urls.typeSite)
                            {
                                case Site.EnLyrsense:
                                    foreach (var url in urls.urls)
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
                                    break;


                                case Site.RuMusinfo:
                                    foreach (var url in urls.urls)
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
                                    break;

                                case Site.Lyricshub:
                                    foreach (var url in urls.urls)
                                    {
                                        HttpResponseMessage resp = clnt.GetAsync(url).Result;
                                        if (resp.IsSuccessStatusCode)
                                        {
                                            var html = resp.Content.ReadAsStringAsync().Result;

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

                                                for(int i = 0; i < lyric.Count; i += 2)
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
            catch (Exception ex) { return null; }

            return lyrics;
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

                    foreach(var line in correctLyrics)
                        resultLyrics.Add(line.Split("</div>")[0]);

                    break;
                
                case Site.Lyricshub:
                    correctLyrics = lyrics.ToList();
                    correctLyrics.RemoveAt(0);
                    var list = new List<List<string>>();
                    foreach (var line in correctLyrics)
                        list.Add((line.Split("<span class=\"trslwrd\" tabindex=\"0\">")).ToList());

                    foreach(var line in list)
                    {
                        var correctVerb = new List<string>();
                        string resultLine = "";

                        line.RemoveAt(0);

                        foreach(var verb in line)
                            correctVerb.Add(verb.Split("</span>")[0]);
                        
                        for(int i = 0, k = -1; i < correctVerb.Count; i++)
                        {
                            if(k != 0)
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

                                if(k != 0)
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
                        if(lyric.Length != 1)
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

                default:
                    break;
            }

            var result = new List<string>();
            foreach(var line in resultLyrics)
            {
                var correctLine = line.Split("&#x27;");
                var resultLine = "";
                if (correctLine.Length == 1)
                    resultLine = correctLine[0];
                else
                    for(int i = 0; i < correctLine.Length; i++)
                    {
                        if (i == correctLine.Length - 1)
                            resultLine += correctLine[i];
                        else
                            resultLine += correctLine[i] + "`";
                    }

                result.Add(resultLine);
            }
            return result;
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


                default:
                    break;
            }

            return resultLyrics;

        }
        #endregion
        /**/
        #region File
        private void UploadFromFile()
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string trackLine;
                    while ((trackLine = sr.ReadLine()) != null)
                    {
                        var track = trackLine.Split("&");
                        tracks.Add(new Track { Title = track[0], Artist = track[1], ruTitle = track[2], enLyrics = LyricsToUpload(track[3]), ruLyrics = LyricsToUpload(track[4]) });
                    }
                }

                if (tracks != null)
                {
                    foreach (var track in tracks)
                    {
                        List_Track.Items.Add(track);
                    }
                }
            }
        }

        private void SaveToFile()
        {
            if (!File.Exists(path))
                using (StreamWriter sw = File.CreateText(path)) { }

            string file = "";
            foreach(var track in tracks)
            {
                file += track.Title + "&" + track.Artist + "&" + track.ruTitle + "&" + LyricsToSave(track.enLyrics) + "&" + LyricsToSave(track.ruLyrics) + "\n";
            }
            File.WriteAllText(path, file);
        }

        private string LyricsToSave(string lyrics)
        {
            var result = new List<char>();
            foreach(var token in lyrics)
            {
                if(token == '\n')
                    result.Add('#');
                else
                    result.Add(token);
            }

            return new string(result.ToArray());
        }

        private string LyricsToUpload(string lyrics)
        {
            var result = new List<char>();
            foreach(var token in lyrics)
            {
                if(token == '#')
                    result.Add('\n');
                else
                    result.Add(token);
            }

            return new string(result.ToArray());

        }
        #endregion
        /**/
        #region MainWindow
        private void ImportArtist_TextChanged(object sender, TextChangedEventArgs e)
        {
            artist = ImportArtist.Text;
        }

        private void ImportTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            title = ImportTitle.Text;
        }

        private void enLyrics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Lyrics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

    }
    #endregion
    /**/
}