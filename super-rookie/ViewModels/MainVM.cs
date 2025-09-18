using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Core;

namespace super_rookie.ViewModels
{
    public partial class MainVM : ObservableObject
    {
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

        public MainVM()
        {
            MixingUnits = new ObservableCollection<MixingUnitVM>();
            SelectMixingUnitCommand = new SelectMixingUnitCommand(this);
            StartCommand = new RelayCommand(StartOperation);
            StopCommand = new RelayCommand(StopOperation);
            SettingsCommand = new RelayCommand(OpenSettings);
            HelpCommand = new RelayCommand(ShowHelp);
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
            MessageBox.Show("시작 버튼이 클릭되었습니다!", "시작", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StopOperation()
        {
            MessageBox.Show("정지 버튼이 클릭되었습니다!", "정지", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void OpenSettings()
        {
            MessageBox.Show("설정 버튼이 클릭되었습니다!", "설정", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void ShowHelp()
        {
            MessageBox.Show("도움말 버튼이 클릭되었습니다!", "도움말", MessageBoxButton.OK, MessageBoxImage.Information);
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
}