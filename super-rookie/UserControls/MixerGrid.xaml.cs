using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using super_rookie.ViewModels;
using super_rookie.ViewModels.Module;
using super_rookie.Models.Module;

namespace super_rookie.UserControls
{
    /// <summary>
    /// MixerGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MixerGrid : UserControl
    {
        public MixerGrid()
        {
            InitializeComponent();
            this.DataContextChanged += MixerGrid_DataContextChanged;
        }

        private void MixerGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MixingUnitVM oldMixingUnit)
            {
                oldMixingUnit.PropertyChanged -= MixingUnitVM_PropertyChanged;
            }
            
            if (e.NewValue is MixingUnitVM newMixingUnit)
            {
                newMixingUnit.PropertyChanged += MixingUnitVM_PropertyChanged;
            }
        }

        private bool _isUpdatingSelection = false;

        private void MixingUnitVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MixingUnitVM.SelectedModule))
            {
                var mixingUnitVM = sender as MixingUnitVM;
                if (mixingUnitVM != null)
                {
                    // SelectedModule이 MixerVM이 아니면 선택 해제
                    if (!(mixingUnitVM.SelectedModule is MixerVM))
                    {
                        _isUpdatingSelection = true;
                        DataGrid.SelectedItem = null;
                        _isUpdatingSelection = false;
                    }
                }
            }
        }

        private void AddMixer_Click(object sender, RoutedEventArgs e)
        {
            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                // 새 Mixer 모델 생성
                var newMixer = new Mixer
                {
                    Name = $"Mixer_{mixingUnitVM.Mixers.Count + 1}"
                };
                
                // 새 MixerVM 생성 및 추가
                var newMixerVM = new MixerVM(newMixer);
                mixingUnitVM.Mixers.Add(newMixerVM);
            }
        }

        private void DeleteMixer_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var mixerVM = button?.Tag as MixerVM;
            var mixingUnitVM = DataContext as MixingUnitVM;

            if (mixerVM != null && mixingUnitVM != null)
            {
                // 선택된 믹서가 삭제될 믹서와 같다면 선택 해제
                if (mixingUnitVM.SelectedModule == mixerVM)
                {
                    mixingUnitVM.SelectedModule = null;
                }

                // 믹서 삭제
                mixingUnitVM.Mixers.Remove(mixerVM);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 내부 업데이트 중이면 무시
            if (_isUpdatingSelection) return;

            var mixingUnitVM = DataContext as MixingUnitVM;
            if (mixingUnitVM != null)
            {
                var selectedMixer = DataGrid.SelectedItem as MixerVM;
                mixingUnitVM.SelectedModule = selectedMixer;
            }
        }
    }
}
