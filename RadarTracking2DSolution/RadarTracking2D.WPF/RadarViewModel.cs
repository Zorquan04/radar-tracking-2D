using System.Collections.ObjectModel;
using System.ComponentModel;
using RadarTracking2D.Core.Tracking;
using RadarTracking2D.WPF.Models;

namespace RadarTracking2D.WPF.ViewModels;

public class RadarViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TrackVisual> Tracks { get; } = new();

    public void UpdateTracks(IEnumerable<Track> coreTracks)
    {
        foreach (var core in coreTracks)
        {
            var visual = Tracks.FirstOrDefault(t => t.Id == core.Id);
            if (visual == null)
                Tracks.Add(new TrackVisual(core));
            else
                visual.Update(core);
        }
        OnPropertyChanged(nameof(Tracks));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}