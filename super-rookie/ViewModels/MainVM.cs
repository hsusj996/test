using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Core;
using super_rookie.Services;

namespace super_rookie.ViewModels
{
    public partial class MainVM : ObservableObject
    {
        private readonly SimulationService _simulationService;

        public ObservableCollection<MixingUnitVM> MixingUnits { get; set; }

        private MixingUnitVM? _selectedMixingUnit;

        public MixingUnitVM? SelectedMixingUnit
        {
            get => _selectedMixingUnit;
            set
            {
                if (_selectedMixingUnit != value)
                {
                    _selectedMixingUnit = value;
                    OnPropertyChanged(nameof(SelectedMixingUnit));
                }
            }
        }

        public ICommand SelectMixingUnitCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand HelpCommand { get; }
        public ICommand ToggleValveCommand { get; }

        public MainVM()
        {
            _simulationService = new SimulationService();
            MixingUnits = new ObservableCollection<MixingUnitVM>();
            SelectMixingUnitCommand = new SelectMixingUnitCommand(this);
            StartCommand = new RelayCommand(StartOperation, CanStartOperation);
            StopCommand = new RelayCommand(StopOperation, CanStopOperation);
            SettingsCommand = new RelayCommand(OpenSettings);
            HelpCommand = new RelayCommand(ShowHelp);
            ToggleValveCommand = new RelayCommand<Module.ValveVM>(ToggleValve);
            LoadMixingUnits();
            
            // 첫 번째 유닛을 기본 선택으로 설정
            if (MixingUnits.Count > 0)
            {
                SelectMixingUnit(MixingUnits[0]);
            }
        }

        private void LoadMixingUnits()
        {
            var store = MixingUnitStore.Instance;
            foreach (var mixingUnit in store.MixingUnits)
            {
                MixingUnits.Add(new MixingUnitVM(mixingUnit));
            }
        }

        public void SelectMixingUnit(MixingUnitVM? mixingUnit)
        {
            if (mixingUnit == null) return;

            // 이전 선택 해제
            if (SelectedMixingUnit != null)
            {
                SelectedMixingUnit.IsSelected = false;
            }

            // 새 선택 설정
            SelectedMixingUnit = mixingUnit;
            SelectedMixingUnit.IsSelected = true;
        }

        // 리본 메뉴 명령어 메서드들
        private void StartOperation()
        {
            if (SelectedMixingUnit == null)
            {
                MessageBox.Show("시뮬레이션할 MixingUnit을 선택해주세요.", "오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _simulationService.StartSimulation(SelectedMixingUnit, 100); // 100ms 간격
                MessageBox.Show($"시뮬레이션이 시작되었습니다!\n유닛: {SelectedMixingUnit.Name}", "시뮬레이션 시작", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void StopOperation()
        {
            try
            {
                _simulationService.StopSimulation();
                MessageBox.Show("시뮬레이션이 정지되었습니다.", "시뮬레이션 정지", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"시뮬레이션 정지 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 명령어 실행 가능 여부 확인
        private bool CanStartOperation()
        {
            return SelectedMixingUnit != null && !_simulationService.IsRunning;
        }

        private bool CanStopOperation()
        {
            return _simulationService.IsRunning;
        }

        private void OpenSettings()
        {
            MessageBox.Show("설정 버튼이 클릭되었습니다!", "설정", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void ShowHelp()
        {
            MessageBox.Show("도움말 버튼이 클릭되었습니다!", "도움말", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 밸브 상태 토글
        /// </summary>
        private void ToggleValve(Module.ValveVM valve)
        {
            if (valve == null) return;

            valve.IsOpen = !valve.IsOpen;
            
            string status = valve.IsOpen ? "열림" : "닫힘";
            MessageBox.Show($"밸브 '{valve.Name}'이(가) {status} 상태로 변경되었습니다.", 
                          "밸브 제어", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        /// <summary>
        /// 리소스 정리
        /// </summary>
        public void Dispose()
        {
            _simulationService?.Dispose();
        }
    }

    public class SelectMixingUnitCommand : ICommand
    {
        private readonly MainVM _mainVM;

        public SelectMixingUnitCommand(MainVM mainVM)
        {
            _mainVM = mainVM;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return parameter is MixingUnitVM;
        }

        public void Execute(object? parameter)
        {
            if (parameter is MixingUnitVM mixingUnit)
            {
                _mainVM.SelectMixingUnit(mixingUnit);
            }
        }
    }

    // 간단한 RelayCommand 클래스 (초급자용)
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }

    // 매개변수를 받는 RelayCommand 클래스
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}