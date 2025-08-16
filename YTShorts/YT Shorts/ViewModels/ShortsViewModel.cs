using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using YTShorts.Models;
using YTShorts.Services;

namespace YTShorts.ViewModels;

public class ShortsViewModel : ObservableObject
{
    private readonly IVideoService _videoService;
    private int _currentPosition;
    private ObservableCollection<VideoItem> _videos = new();

    public ObservableCollection<VideoItem> Videos
    {
        get => _videos;
        set
        {
            _videos = value;
            OnPropertyChanged();
        }
    }

    public int CurrentPosition
    {
        get => _currentPosition;
        set
        {
            if (value == _currentPosition || value < 0 || value >= Videos.Count)
                return;

            _currentPosition = value;
            OnPropertyChanged();
        }
    }

    public ShortsViewModel()
    {

    }

    public ShortsViewModel(IVideoService videoService)
    {
        _videoService = videoService;
        LoadVideos();
    }

    private async void LoadVideos()
    {
        var videos = await _videoService.GetVideosAsync();
        CurrentPosition = 0;
        Videos = new ObservableCollection<VideoItem>(videos);
    }

    public void NextVideo() => CurrentPosition++;
    public void PreviousVideo() => CurrentPosition--;
}