﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TracksLyrics
{
    public class Track
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string ruTitle {get; set;}

        public string enLyrics { get; set; }
        public string ruLyrics { get; set; }

        public string Original { get; set; }
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
        Lyricshub,
        Genius
    }

    public partial class MainWindow : Window
    {
        #region Static variable
        private static List<Track> tracks;
        private static string path = @"D:\Document\TrackList.txt";
        private static string title, artist, search;
        private static int sort_Position;
        private static bool isOriginal, isUpload;
        private static bool original;
        private static object SelectedTrack;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            tracks = new List<Track>();
            UploadFromFile();
            sort_Position = -1;
            isOriginal = false;
            isUpload = false;
        }
        /**/
        #region Button
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateTrack(new Track() { Artist = artist, Title = title });

            Import.Text = "";
        }

        private void Reload_All_Button_Click(object sender, RoutedEventArgs e)
        {
            Confirm_Reload_All_Button.IsEnabled = true;
            Cancel_Reload_All_Button.IsEnabled = true;

            Confirm_Reload_All_Button.Margin = new Thickness(450, 375, 0, 0);
            Confirm_Reload_All_Button.Width = 160;
            Confirm_Reload_All_Button.Height = 40;

            Cancel_Reload_All_Button.Margin = new Thickness(670, 375, 0, 0);
            Cancel_Reload_All_Button.Width = 160;
            Cancel_Reload_All_Button.Height = 40;

            Reload_All_BackGround.Margin = new Thickness(0, 0, 0, 0);
            Reload_All_BackGround.Width = 1280;
            Reload_All_BackGround.Height = 704;

        }

        private void Reload_Button_Click(object sender, RoutedEventArgs e)
        {
            var track = SelectedTrack as Track;

            List_Track.Items.Remove(SelectedTrack);

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
            var track = SelectedTrack as Track;
            Import.Text = track.Title + "/" + track.Artist;
        }

        private void Show_Lyrics_Button_Click(object sender, RoutedEventArgs e)
        {
            var listitem = sender as Button;
            var track = listitem.DataContext as Track;

            Lyrics.Items.Clear();

            if (isOriginal || track.ruTitle == "NLT")
            {
                Lyrics.Items.Add("\t" + track.Title);
                Lyrics.Items.Add("\t" + "");

                var enLyric = track.enLyrics.Split('\n');

                for (int i = 0; i < enLyric.Length; i++)
                    Lyrics.Items.Add("\t" + enLyric[i]);

                SetLyrics_TextBox.Text = (track.Title) + "\n\n" + track.enLyrics;
            }
            else
            {
                Lyrics.Items.Add("\t" + track.Title);
                Lyrics.Items.Add("\t" + track.ruTitle);
                Lyrics.Items.Add("\t" + "");

                var enLyric = track.enLyrics.Split('\n').ToList();
                var ruLyric = track.ruLyrics.Split('\n').ToList();

                if(enLyric.Count > ruLyric.Count || enLyric.Count > ruLyric.Count)
                    while (enLyric.Count != ruLyric.Count)
                    {
                        if (enLyric.Count > ruLyric.Count)
                            ruLyric.Add("\t" + "");
                        else
                            enLyric.Add("\t" + "");
                    }

                for (int i = 0; i < enLyric.Count; i++)
                {
                    Lyrics.Items.Add("\t" + enLyric[i]);
                    Lyrics.Items.Add("\t" + ruLyric[i]);
                    Lyrics.Items.Add("\t" + "");
                }

                SetLyrics_TextBox.Text = (track.ruTitle) + "\n\n" + track.ruLyrics;
            }
            SelectedTrack = listitem.DataContext;


            Set_Button.IsEnabled = true;
            Delete_Button.IsEnabled = true;
            Reload_Button.IsEnabled = true;
            Upload_T_Button.IsEnabled = true;
            Lyrics.ScrollIntoView(Lyrics.Items[0]);
        }

        private void Show_Lyrics_Button_Click(Track track)
        {
            Lyrics.Items.Clear();

            if (isOriginal || track.ruTitle == "NLT")
            {
                Lyrics.Items.Add("\t" + track.Title);
                Lyrics.Items.Add("\t" + "");

                var enLyric = track.enLyrics.Split('\n');

                for (int i = 0; i < enLyric.Length; i++)
                    Lyrics.Items.Add("\t" + enLyric[i]);

                SetLyrics_TextBox.Text = (track.Title) + "\n\n" + track.enLyrics;
            }
            else
            {
                Lyrics.Items.Add("\t" + track.Title);
                Lyrics.Items.Add("\t" + track.ruTitle);
                Lyrics.Items.Add("\t" + "");

                var enLyric = track.enLyrics.Split('\n').ToList();
                var ruLyric = track.ruLyrics.Split('\n').ToList();

                if (enLyric.Count > ruLyric.Count || enLyric.Count > ruLyric.Count)
                    while (enLyric.Count != ruLyric.Count)
                    {
                        if (enLyric.Count > ruLyric.Count)
                            ruLyric.Add("\t" + "");
                        else
                            enLyric.Add("\t" + "");
                    }

                for (int i = 0; i < enLyric.Count; i++)
                {
                    Lyrics.Items.Add("\t" + enLyric[i]);
                    Lyrics.Items.Add("\t" + ruLyric[i]);
                    Lyrics.Items.Add("\t" + "");
                }

                SetLyrics_TextBox.Text = (track.ruTitle) + "\n\n" + track.ruLyrics;
            }

            Lyrics.ScrollIntoView(Lyrics.Items[0]);
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
                    foreach (var track in tracks)
                        if (track.Title.Length > search.Length || track.Title.ToLower() == search.ToLower())
                        {
                            if(track.Title.ToLower() == search.ToLower())
                                sorted.Add(track);
                            else
                                if (track.Title.Remove(search.Length).ToLower() == search.ToLower())
                                    sorted.Add(track);
                        }
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

        private void Original_Button_Click(object sender, RoutedEventArgs e)
        {
            if(isOriginal)
            {
                Original_Button.Background = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
                Original_Button.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
            else
            {
                Original_Button.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                Original_Button.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }

            isOriginal = !isOriginal;
            Show_Lyrics_Button_Click(SelectedTrack as Track);
        }

        private void Upload_T_Button_Click(object sender, RoutedEventArgs e)
        {
            if (isUpload)
            {
                Upload_T_Button.Background = new SolidColorBrush(Color.FromArgb(255, 155, 155, 155));
                Upload_T_Button.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                SetLyrics_TextBox.Margin = new Thickness(964, 30, 0, 0);
                SetLyrics_TextBox.Width = 1;
                SetLyrics_TextBox.Height = 1;

                if (isOriginal)
                    ResetEnLyrics(SetLyrics_TextBox.Text);
                else
                    ResetRuLyrics(SetLyrics_TextBox.Text);
            }
            else
            {
                Upload_T_Button.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                Upload_T_Button.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

                SetLyrics_TextBox.Margin = new Thickness(10, 138, 10, 10);
                SetLyrics_TextBox.Height = 556;
                SetLyrics_TextBox.Width = 1260;
            }

            isUpload = !isUpload;
        }

        private void Confirm_Reload_All_Button_Click(object sender, RoutedEventArgs e)
        {
            var preTracks = tracks.ToArray();
            tracks.Clear();

            foreach (var track in preTracks)
                CreateTrack(new Track() { Artist = track.Artist, Title = track.Title });


            Confirm_Reload_All_Button.IsEnabled = false;
            Cancel_Reload_All_Button.IsEnabled = false;


            Confirm_Reload_All_Button.Margin = new Thickness(892, 84, 0, 0);
            Confirm_Reload_All_Button.Width = 1;
            Confirm_Reload_All_Button.Height = 1;

            Cancel_Reload_All_Button.Margin = new Thickness(892, 84, 0, 0);
            Cancel_Reload_All_Button.Width = 1;
            Cancel_Reload_All_Button.Height = 1;

            Reload_All_BackGround.Margin = new Thickness(892, 84, 228, 0);
            Reload_All_BackGround.Width = 1;
            Reload_All_BackGround.Height = 1;
        }

        private void Cancel_Reload_All_Button_Click(object sender, RoutedEventArgs e)
        {
            Confirm_Reload_All_Button.IsEnabled = false;
            Cancel_Reload_All_Button.IsEnabled = false;


            Confirm_Reload_All_Button.Margin = new Thickness(892, 84, 228, 0);
            Confirm_Reload_All_Button.Width = 1;
            Confirm_Reload_All_Button.Height = 1;

            Cancel_Reload_All_Button.Margin = new Thickness(892, 84, 228, 0);
            Cancel_Reload_All_Button.Width = 1;
            Cancel_Reload_All_Button.Height = 1;

            Reload_All_BackGround.Margin = new Thickness(892, 84, 228, 0);
            Reload_All_BackGround.Width = 1;
            Reload_All_BackGround.Height = 1;
        }
        #endregion
        /**/
        #region ImportData

        private async Task CreateTrack(Track track)
        {
            if (!tracks.Where(x => x.Artist == track.Artist).Select(x => x.Title).Contains(track.Title))
            {
                var parser = new Parser();
                await parser.Parsing(CreateUrls(track));

                if (parser.Lyrics != null && parser.Lyrics.Any())
                {
                    original = parser.Original;
                    if(original)
                    {
                        track.Title = parser.Lyrics[0];
                        track.ruTitle = "NLT";

                        track.enLyrics = parser.Lyrics[1];

                        track.Original = "O";
                        original = false;
                    }
                    else
                    {
                        track.Title = parser.Lyrics[0];
                        track.ruTitle = parser.Lyrics[1];

                        track.enLyrics = parser.Lyrics[2];
                        track.ruLyrics = parser.Lyrics[3];

                        track.Original = "T";
                    }

                    List_Track.Items.Add(track);
                    tracks.Add(track);
                    await SaveToFile();
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

            var siteGenius = "https://genius.com/" + ToUpperFirstChar(DeleteSpace(track.Artist, Site.Genius)) + "-" + DeleteSpace(track.Title, Site.Genius) + "-lyrics";
            urls.Add(new SiteUrl
            {
                typeSite = Site.Genius,
                urls = new List<string>
                {
                    siteGenius,
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

                case Site.Genius:
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

                default:
                    return null;
                    break;
            }
        }

        private static string ToUpperFirstChar(string line)
        {
            var correct = line.ToCharArray().ToList();
            var item = correct[0] + "";
            correct[0] = item.ToUpper()[0];
            return new string(correct.ToArray());
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

        private void ResetRuLyrics(string lyrics)
        {
            var track = SelectedTrack as Track;
            track.ruTitle = lyrics.Split("\n\n")[0];
            var lyric = lyrics.ToCharArray().ToList();
            lyric.RemoveRange(0, track.ruTitle.Length + 2);

            track.ruLyrics = new string(lyric.ToArray());

            List_Track.Items.Remove(SelectedTrack);
            tracks.Remove(track);

            List_Track.Items.Add(track);
            tracks.Add(track);

            if (track.Original == "O")
                track.Original = "T";
            SaveToFile();
            Show_Lyrics_Button_Click(track);
        }

        private void ResetEnLyrics(string lyrics)
        {
            var track = SelectedTrack as Track;
            track.Title = lyrics.Split("\n\n")[0];
            var lyric = lyrics.ToCharArray().ToList();
            lyric.RemoveRange(0, track.Title.Length + 2);

            track.enLyrics = new string(lyric.ToArray());

            List_Track.Items.Remove(SelectedTrack);
            tracks.Remove(track);

            List_Track.Items.Add(track);
            tracks.Add(track);

            SaveToFile();
            Show_Lyrics_Button_Click(track);
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
                        try
                        {
                            var track = trackLine.Split("&");
                            if (track[3] != "NLT")
                                tracks.Add(new Track { Original = track[0], Title = track[1], Artist = track[2], ruTitle = track[3], enLyrics = LyricsToUpload(track[4]), ruLyrics = LyricsToUpload(track[5]) });
                            else
                                tracks.Add(new Track { Original = track[0], Title = track[1], Artist = track[2], ruTitle = track[3], enLyrics = LyricsToUpload(track[4]) });
                        }
                        catch
                        {

                        }
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

        private Task SaveToFile()
        {
            if (!File.Exists(path))
                using (StreamWriter sw = File.CreateText(path)) { }

            string file = "";
            foreach(var track in tracks)
            {
                if (track.ruTitle != "NLT")
                    file += track.Original + "&" + track.Title + "&" + track.Artist + "&" + track.ruTitle + "&" + LyricsToSave(track.enLyrics) + "&" + LyricsToSave(track.ruLyrics) + "\n";
                else
                    file += track.Original + "&" + track.Title + "&" + track.Artist + "&" + track.ruTitle + "&" + LyricsToSave(track.enLyrics) + "\n";
            }
            File.WriteAllText(path, file);

            return Task.CompletedTask;
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

        private void Import_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = Import.Text.Split('/');
            if (text.Length == 2)
            {
                title = text[0];
                artist = text[1];
            }    

        }

        private void SetLyrics_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void List_Track_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void enLyrics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Lyrics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        #endregion
        /**/
    }
}