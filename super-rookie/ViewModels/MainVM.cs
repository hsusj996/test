using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        public MainVM()
        {
            MixingUnits = new ObservableCollection<MixingUnitVM>();
            SelectMixingUnitCommand = new SelectMixingUnitCommand(this);
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
}