using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class MixerVM : ObservableObject
    {
        private readonly Mixer _model;

        public MixerVM(Mixer model)
        {
            _model = model;
            _name = model.Name;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    _model.Name = value;
                }
            }
        }

        private DigitalOutputVM? _controlOutput;
        private DigitalInputVM? _statusInput;

        public DigitalOutputVM? ControlOutput
        {
            get => _controlOutput;
            set
            {
                if (SetProperty(ref _controlOutput, value))
                {
                    _model.ControlOutput = value?.GetModel();
                }
            }
        }

        public DigitalInputVM? StatusInput
        {
            get => _statusInput;
            set
            {
                if (SetProperty(ref _statusInput, value))
                {
                    _model.StatusInput = value?.GetModel();
                }
            }
        }

        public Mixer GetModel() => _model;

        /// <summary>
        /// 믹서 시뮬레이션 업데이트
        /// </summary>
        public void Update()
        {
            // TODO: 믹서 시뮬레이션 로직 구현
            // - 회전 속도 제어 시뮬레이션
            // - 디지털 출력 상태 반영
            // - 상태 입력 모니터링
        }
    }
}