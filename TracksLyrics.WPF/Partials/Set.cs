using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TracksLyrics.Application.Dtos;
using TracksLyrics.Application.Handlers;
using TracksLyrics.Domain.Models;
using TracksLyrics.WPF.Helpers;
using static TracksLyrics.WPF.Settings.Di;

// ReSharper disable once CheckNamespace
namespace TracksLyrics.WPF;

public partial class MainWindow
{
    private TrackInfoModel _currentTrack;

    private Task Listener()
    {
        try
        {
            TransmitterMusicService.GetCurrentPlayingTrack(SetTrack());
        }
        catch (Exception)
        {
            // ignored
        }
        
        TransmitterMusicService.Dispose();
        return Task.CompletedTask;
    }
    
    

    private Action<TrackInfoModel?> SetTrack()
    {
        return track =>
        {
            if (track is not null)
            {
                _ = Task.Run(() =>
                {
                    if (track.Equals(_currentTrack))
                        SetProgressBar(track.ProgressTime, track.DurationTime);
                    else
                        SetAll(track);
                });   
            }
        };
    }

    private void SetAll(TrackInfoModel track)
    {
        _currentTrack = track;
        
        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
        {
            Lyrics.Items.Clear();
            Lyrics.Items.AddLyric("Loading");
        }));

        Task.Run(() =>
        {
            SetTitle(track);
            SetProgressBar(track.ProgressTime, track.DurationTime);
            _ = SetLyrics(track);   
        });
    }

    private void SetTitle(TrackInfoModel track)
    {
        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
        {
            Title.Text = $"{track.Name} - {track.Artist}";
        }));
    }

    private void SetProgressBar(TimeSpan currentPosition, TimeSpan totalDuration)
    {
        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
        {
            CurrentPosition.Text = currentPosition.ToString(@"mm\:ss");

            TrackProgress.Value = currentPosition.TotalSeconds;
            TrackProgress.Maximum = totalDuration.TotalSeconds;

            TotalDuration.Text = totalDuration.ToString(@"mm\:ss");
        }));
    }

    private async Task SetLyrics(TrackInfoModel trackInfo)
    {
        var trackLyric = await GetTrackLyric(trackInfo);
        
        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
        {

            if (trackLyric == null)
            {
                Lyrics.Items.Clear();
                Lyrics.Items.AddLyric("Not detect track");
                
                return;
            }

            if (trackLyric.Lyrics!.Any())
            {
                Lyrics.Items.Clear();
                Lyrics.Items.AddLyric("Not found track lyric");
            }
            
            Lyrics.Items.Clear();
            Lyrics.Items.AddLyrics(trackLyric);   
                
            Lyrics.ScrollIntoView(" ");
            
            ActivateButtonsInListBox();
        }));
    }

    private async Task<TrackDto?> GetTrackLyric(TrackInfoModel trackInfo)
    {
        var request = new GetLyricsCommandRequest
        {
            TrackInfo = trackInfo
        };

        var handler = new GetLyricsCommandHandler(TrackLyricService, ParsersService);

        try
        {
            var response = await handler.Handle(request, new CancellationToken());
        
            return response.Dto;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    private void ActivateButtonsInListBox()
    {
        var counter = 2;

        foreach (var item in Lyrics.Items)
        {
            if (counter % 3 == 0 && Lyrics.ItemContainerGenerator.ContainerFromItem(item) is ListBoxItem listBoxItem)
            {
                var button = FindVisualChild<Button>(listBoxItem);

                if (button != null)
                    button.Visibility = Visibility.Visible;
            }

            counter++;
        }
    }

    private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is T t)
                return t;

            var childOfChild = FindVisualChild<T>(child);

            if (childOfChild != null)
                return childOfChild;
        }

        return null;
    }
}