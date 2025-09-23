using CommunityToolkit.Mvvm.ComponentModel;
using super_rookie.Models.Module;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class LevelSensorVM : ObservableObject
    {
        private readonly LevelSensor _model;

        public LevelSensorVM(LevelSensor model)
        {
            _model = model;
            _name = model.Name;
            _triggerAmount = model.TriggerAmount;
            _isTriggered = model.IsTriggered;
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

        private double _triggerAmount;
        private bool _isTriggered;
        private DigitalInputVM? _statusDi;
        private TankVM? _tank;

        public double TriggerAmount
        {
            get => _triggerAmount;
            set
            {
                if (SetProperty(ref _triggerAmount, value))
                {
                    _model.TriggerAmount = value;
                }
            }
        }

        public bool IsTriggered
        {
            get => _isTriggered;
            set
            {
                if (SetProperty(ref _isTriggered, value))
                {
                    _model.IsTriggered = value;
                }
            }
        }

        public DigitalInputVM? StatusDi
        {
            get => _statusDi;
            set
            {
                if (SetProperty(ref _statusDi, value))
                {
                    _model.StatusDi = value?.GetModel();
                }
            }
        }

        public TankVM? Tank
        {
            get => _tank;
            set
            {
                if (SetProperty(ref _tank, value))
                {
                    _model.Tank = value?.GetModel();
                }
            }
        }

        // Tank의 현재 레벨과 최대 레벨에 대한 읽기 전용 속성
        public double CurrentLevel => _tank?.Amount ?? 0;
        public double MaxLevel => _tank?.Capacity ?? 0;

        public LevelSensor GetModel() => _model;

        /// <summary>
        /// 레벨 센서 시뮬레이션 업데이트
        /// </summary>
        public void Update()
        {
            // TODO: 레벨 센서 시뮬레이션 로직 구현
            // - 탱크 레벨 모니터링
            // - 트리거 상태 업데이트
            // - 디지털 입력 상태 반영
        }
    }
}