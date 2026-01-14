using System.Collections.ObjectModel;
using System.ComponentModel;
using RadarTracking2D.WPF.Models;

namespace RadarTracking2D.WPF.ViewModels;

public class RadarViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TrackVisual> Tracks { get; } = new();

    public void UpdateTracks(IEnumerable<Track> coreTracks)
    {
        foreach (var core in coreTracks)
        {
            // we are looking for track visualization
            var visual = Tracks.FirstOrDefault(t => t.Id == core.Id);

            if (visual == null)
            {     
                Tracks.Add(new TrackVisual(core)); // if it doesn't exist, we add a new one and keep its initial position
            }
            else
            {
                visual.AddPosition(); // if it exists, we add a new item to the history without deleting the previous ones
            }
        }

        OnPropertyChanged(nameof(Tracks));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}