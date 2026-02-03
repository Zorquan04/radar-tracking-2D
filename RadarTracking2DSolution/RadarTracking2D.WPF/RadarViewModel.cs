using System.Collections.ObjectModel;
using System.ComponentModel;
using RadarTracking2D.WPF.Models;

namespace RadarTracking2D.WPF.ViewModels;

// ViewModel stores visual tracks for WPF binding
public class RadarViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TrackVisual> Tracks { get; } = new();

    // sync core Tracks with visuals
    public void UpdateTracks(IEnumerable<Track> coreTracks)
    {
        foreach(var core in coreTracks)
    {
            var visual = Tracks.FirstOrDefault(t => t.Id == core.Id);

            if (visual == null)
            {
                visual = new TrackVisual(core); // add new visual if it doesn't exist
                Tracks.Add(visual);
            }
            else
            {
                if (!visual.GetCoreTrack().IsManual)
                    visual.AddPosition();
            }
        }

        OnPropertyChanged(nameof(Tracks));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}