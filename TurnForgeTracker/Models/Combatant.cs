using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TurnForgeTracker.Models
{
    public class Combatant : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private int _initiative;
        private int _currentHp;
        private int _maxHp;
        private bool _isActive;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public int Initiative
        {
            get => _initiative;
            set { _initiative = value; OnPropertyChanged(); }
        }

        public int CurrentHp
        {
            get => _currentHp;
            set { _currentHp = value; OnPropertyChanged(); OnPropertyChanged(nameof(HpDisplay)); }
        }

        public int MaxHp
        {
            get => _maxHp;
            set { _maxHp = value; OnPropertyChanged(); OnPropertyChanged(nameof(HpDisplay)); }
        }

        public string HpDisplay => $"{CurrentHp}/{MaxHp}";

        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Effect> Effects { get; } = new();

        public string EffectsSummary =>
            Effects.Count == 0 ? "—" : string.Join(", ", System.Linq.Enumerable.Select(Effects, e => $"{e.Name}({e.DurationDisplay})"));

        public void RefreshEffectsSummary() => OnPropertyChanged(nameof(EffectsSummary));

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
