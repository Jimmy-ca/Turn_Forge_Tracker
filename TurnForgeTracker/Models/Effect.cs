using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TurnForgeTracker.Models
{
    public class Effect : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private int _duration;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>Rounds remaining. -1 means permanent (never expires).</summary>
        public int Duration
        {
            get => _duration;
            set { _duration = value; OnPropertyChanged(); OnPropertyChanged(nameof(DurationDisplay)); }
        }

        public string DurationDisplay => Duration == -1 ? "∞" : Duration.ToString();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
