using System;
using System.Threading.Tasks;
using TracksLyrics.Domain.Models;
using TracksLyrics.WPF.Settings;

namespace TracksLyrics.WPF;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        _ = new Di();

        _ = Task.Run(Listener);
        _currentTrack = new TrackInfoModel
        {
            Name = "Loading", 
            Artist = "", 
            DurationTime = TimeSpan.Zero, 
            ProgressTime = TimeSpan.Zero
        };
    }
}