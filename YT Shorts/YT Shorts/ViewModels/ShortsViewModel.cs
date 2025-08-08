using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using YTShorts.Models;
using YTShorts.Services;

namespace YTShorts.ViewModels;

public class ShortsViewModel : INotifyPropertyChanged
{
    private readonly IVideoService _videoService;
    private int _currentPosition;
    private ObservableCollection<VideoItem> _videos = new();

    public event PropertyChangedEventHandler PropertyChanged;

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
        Videos = new ObservableCollection<VideoItem>(videos);
    }

    public void NextVideo() => CurrentPosition++;
    public void PreviousVideo() => CurrentPosition--;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}