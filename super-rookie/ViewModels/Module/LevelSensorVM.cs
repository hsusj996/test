using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using super_rookie.Models.Module;
using super_rookie.Models.Status;
using super_rookie.ViewModels.Messages;
using super_rookie.ViewModels.Status;

namespace super_rookie.ViewModels.Module
{
    public partial class LevelSensorVM : ObservableObject
    {
        private readonly LevelSensor _model;

        public string Name => _model.Name;

        [ObservableProperty]
        private double _triggerAmount;

        [ObservableProperty]
        private bool _isTriggered;

        [ObservableProperty]
        private DigitalInputVM? _statusDi;

        [ObservableProperty]
        private TankVM? _tank;

        // Tank의 현재 레벨과 최대 레벨에 대한 읽기 전용 속성
        public double CurrentLevel => _tank?.Amount ?? 0;
        public double MaxLevel => _tank?.Capacity ?? 0;

        partial void OnTankChanged(TankVM? value)
        {
            // Tank가 변경되면 CurrentLevel과 MaxLevel 변경 알림
            OnPropertyChanged(nameof(CurrentLevel));
            OnPropertyChanged(nameof(MaxLevel));
        }

        public LevelSensorVM()
        {
            _model = new LevelSensor();
            _triggerAmount = 0;
            _isTriggered = false;
            _statusDi = null;
            _tank = null;
            
            // 메시지 구독 설정
            WeakReferenceMessenger.Default.Register<TankLevelChangedMessage>(this, OnTankLevelChanged);
        }

        public LevelSensorVM(LevelSensor model)
        {
            _model = model ?? new LevelSensor();
            _triggerAmount = _model.TriggerAmount;
            _isTriggered = _model.IsTriggered;
            _statusDi = _model.StatusDi != null ? new DigitalInputVM(_model.StatusDi) : null;
            _tank = _model.Tank != null ? new TankVM(_model.Tank) : null;
            
            // 메시지 구독 설정
            WeakReferenceMessenger.Default.Register<TankLevelChangedMessage>(this, OnTankLevelChanged);
        }

        public LevelSensorVM(string name, TankVM tank)
        {
            _model = new LevelSensor(name, tank?.GetModel());
            _triggerAmount = 0;
            _isTriggered = false;
            _statusDi = null;
            _tank = tank;
            
            // 메시지 구독 설정
            WeakReferenceMessenger.Default.Register<TankLevelChangedMessage>(this, OnTankLevelChanged);
        }

        public LevelSensorVM(string name, double triggerAmount, TankVM tank)
        {
            _model = new LevelSensor(name, triggerAmount, tank?.GetModel());
            _triggerAmount = triggerAmount;
            _isTriggered = false;
            _statusDi = null;
            _tank = tank;
            
            // 메시지 구독 설정
            WeakReferenceMessenger.Default.Register<TankLevelChangedMessage>(this, OnTankLevelChanged);
        }

        private void OnTankLevelChanged(object recipient, TankLevelChangedMessage message)
        {
            // 자신이 참조하는 Tank의 메시지인지 확인
            if (message.Tank == _tank)
            {
                OnPropertyChanged(nameof(CurrentLevel));
                OnPropertyChanged(nameof(MaxLevel));
            }
        }

        public LevelSensor GetModel()
        {
            _model.TriggerAmount = _triggerAmount;
            _model.IsTriggered = _isTriggered;
            _model.StatusDi = _statusDi?.GetModel();
            _model.Tank = _tank?.GetModel();
            return _model;
        }
    }
}
