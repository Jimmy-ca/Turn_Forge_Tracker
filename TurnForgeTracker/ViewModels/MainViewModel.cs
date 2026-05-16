using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TurnForgeTracker.Models;

namespace TurnForgeTracker.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object? parameter) => _execute(parameter);
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        // ── Combatant list ──────────────────────────────────────────────────────
        public ObservableCollection<Combatant> Combatants { get; } = new();

        private int _currentRound = 1;
        public int CurrentRound
        {
            get => _currentRound;
            set { _currentRound = value; OnPropertyChanged(); }
        }

        private int _activeCombatantIndex = -1;
        public int ActiveCombatantIndex
        {
            get => _activeCombatantIndex;
            set { _activeCombatantIndex = value; OnPropertyChanged(); OnPropertyChanged(nameof(ActiveCombatant)); }
        }

        public Combatant? ActiveCombatant =>
            (_activeCombatantIndex >= 0 && _activeCombatantIndex < Combatants.Count)
                ? Combatants[_activeCombatantIndex]
                : null;

        // ── New combatant input fields ──────────────────────────────────────────
        private string _newCombatantName = string.Empty;
        public string NewCombatantName
        {
            get => _newCombatantName;
            set { _newCombatantName = value; OnPropertyChanged(); }
        }

        private int _newCombatantInitiative;
        public int NewCombatantInitiative
        {
            get => _newCombatantInitiative;
            set { _newCombatantInitiative = value; OnPropertyChanged(); }
        }

        private int _newCombatantHp = 10;
        public int NewCombatantHp
        {
            get => _newCombatantHp;
            set { _newCombatantHp = value; OnPropertyChanged(); }
        }

        // ── New effect input fields ─────────────────────────────────────────────
        private string _newEffectName = string.Empty;
        public string NewEffectName
        {
            get => _newEffectName;
            set { _newEffectName = value; OnPropertyChanged(); }
        }

        private int _newEffectDuration = 1;
        public int NewEffectDuration
        {
            get => _newEffectDuration;
            set { _newEffectDuration = value; OnPropertyChanged(); }
        }

        // ── Commands ────────────────────────────────────────────────────────────
        public ICommand AddCombatantCommand { get; }
        public ICommand RemoveCombatantCommand { get; }
        public ICommand NextTurnCommand { get; }
        public ICommand ResetCombatCommand { get; }
        public ICommand AddEffectCommand { get; }
        public ICommand RemoveEffectCommand { get; }

        public MainViewModel()
        {
            AddCombatantCommand = new RelayCommand(
                _ => AddCombatant(),
                _ => !string.IsNullOrWhiteSpace(NewCombatantName));

            RemoveCombatantCommand = new RelayCommand(
                p => RemoveCombatant(p as Combatant),
                p => p is Combatant);

            NextTurnCommand = new RelayCommand(
                _ => NextTurn(),
                _ => Combatants.Count > 0);

            ResetCombatCommand = new RelayCommand(_ => ResetCombat());

            AddEffectCommand = new RelayCommand(
                _ => AddEffect(),
                _ => ActiveCombatant != null && !string.IsNullOrWhiteSpace(NewEffectName));

            RemoveEffectCommand = new RelayCommand(
                p => RemoveEffect(p as Effect),
                p => p is Effect);
        }

        private void AddCombatant()
        {
            var combatant = new Combatant
            {
                Name = NewCombatantName.Trim(),
                Initiative = NewCombatantInitiative,
                MaxHp = NewCombatantHp,
                CurrentHp = NewCombatantHp
            };

            // Insert sorted by initiative descending
            int insertIndex = 0;
            while (insertIndex < Combatants.Count && Combatants[insertIndex].Initiative >= combatant.Initiative)
                insertIndex++;

            Combatants.Insert(insertIndex, combatant);

            // If combat hasn't started yet, mark first as active
            if (ActiveCombatantIndex == -1 && Combatants.Count == 1)
                SetActive(0);
            else if (insertIndex <= ActiveCombatantIndex)
                ActiveCombatantIndex++; // keep the same combatant active after reindex

            NewCombatantName = string.Empty;
            NewCombatantInitiative = 0;
            NewCombatantHp = 10;
        }

        private void RemoveCombatant(Combatant? combatant)
        {
            if (combatant == null) return;
            int idx = Combatants.IndexOf(combatant);
            Combatants.Remove(combatant);

            if (Combatants.Count == 0)
            {
                ActiveCombatantIndex = -1;
                return;
            }

            // Adjust active index
            if (idx < ActiveCombatantIndex)
                ActiveCombatantIndex--;
            else if (idx == ActiveCombatantIndex)
                SetActive(ActiveCombatantIndex % Combatants.Count);
        }

        private void NextTurn()
        {
            if (Combatants.Count == 0) return;

            int next = (ActiveCombatantIndex + 1) % Combatants.Count;

            // Cycling back to the first combatant → new round
            if (next == 0)
            {
                CurrentRound++;
                DecrementEffects();
            }

            SetActive(next);
        }

        private void DecrementEffects()
        {
            foreach (var combatant in Combatants)
            {
                var expired = combatant.Effects
                    .Where(e => e.Duration != -1 && e.Duration <= 1)
                    .ToList();

                foreach (var effect in expired)
                    combatant.Effects.Remove(effect);

                foreach (var effect in combatant.Effects.Where(e => e.Duration != -1))
                    effect.Duration--;

                combatant.RefreshEffectsSummary();
            }
        }

        private void SetActive(int index)
        {
            if (ActiveCombatantIndex >= 0 && ActiveCombatantIndex < Combatants.Count)
                Combatants[ActiveCombatantIndex].IsActive = false;

            ActiveCombatantIndex = index;

            if (index >= 0 && index < Combatants.Count)
                Combatants[index].IsActive = true;
        }

        private void ResetCombat()
        {
            foreach (var c in Combatants)
            {
                c.IsActive = false;
                c.Effects.Clear();
                c.RefreshEffectsSummary();
            }
            Combatants.Clear();
            ActiveCombatantIndex = -1;
            CurrentRound = 1;
        }

        private void AddEffect()
        {
            if (ActiveCombatant == null) return;
            ActiveCombatant.Effects.Add(new Effect
            {
                Name = NewEffectName.Trim(),
                Duration = NewEffectDuration
            });
            ActiveCombatant.RefreshEffectsSummary();
            NewEffectName = string.Empty;
            NewEffectDuration = 1;
        }

        private void RemoveEffect(Effect? effect)
        {
            if (effect == null || ActiveCombatant == null) return;
            ActiveCombatant.Effects.Remove(effect);
            ActiveCombatant.RefreshEffectsSummary();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
