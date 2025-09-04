using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using super_rookie.Models;

namespace super_rookie.Services
{
    public enum SimulationPhase
    {
        Configuration,
        Running,
        Paused,
        Completed,
        Error
    }

    public class SimulationStateService : INotifyPropertyChanged
    {
        private SimulationPhase _currentPhase = SimulationPhase.Configuration;
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private DateTime _startTime;
        private bool _isRunning = false;

        public SimulationPhase CurrentPhase
        {
            get => _currentPhase;
            set
            {
                _currentPhase = value;
                OnPropertyChanged(nameof(CurrentPhase));
                OnPropertyChanged(nameof(IsConfigurationPhase));
                OnPropertyChanged(nameof(IsRunningPhase));
                OnPropertyChanged(nameof(IsCompletedPhase));
            }
        }

        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }

        public bool IsConfigurationPhase => CurrentPhase == SimulationPhase.Configuration;
        public bool IsRunningPhase => CurrentPhase == SimulationPhase.Running || CurrentPhase == SimulationPhase.Paused;
        public bool IsCompletedPhase => CurrentPhase == SimulationPhase.Completed;

        public ObservableCollection<Unit> Units { get; } = new ObservableCollection<Unit>();
        public ObservableCollection<DigitalOutput> AvailableDos { get; } = new ObservableCollection<DigitalOutput>();
        public ObservableCollection<DigitalInput> AvailableDis { get; } = new ObservableCollection<DigitalInput>();

        public void StartSimulation()
        {
            CurrentPhase = SimulationPhase.Running;
            IsRunning = true;
            _startTime = DateTime.Now;
        }

        public void PauseSimulation()
        {
            CurrentPhase = SimulationPhase.Paused;
            IsRunning = false;
        }

        public void StopSimulation()
        {
            CurrentPhase = SimulationPhase.Completed;
            IsRunning = false;
            ElapsedTime = DateTime.Now - _startTime;
        }

        public void ResetSimulation()
        {
            CurrentPhase = SimulationPhase.Configuration;
            IsRunning = false;
            ElapsedTime = TimeSpan.Zero;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
