using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using super_rookie.ViewModels;

namespace super_rookie.Services
{
    /// <summary>
    /// MixingUnit 시뮬레이션 서비스
    /// </summary>
    public class SimulationService
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _simulationTask;
        private readonly object _lockObject = new object();
        private bool _isRunning = false;

        /// <summary>
        /// 시뮬레이션이 실행 중인지 여부
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (_lockObject)
                {
                    return _isRunning;
                }
            }
        }

        /// <summary>
        /// 시뮬레이션 시작
        /// </summary>
        /// <param name="mixingUnit">시뮬레이션할 MixingUnit</param>
        /// <param name="updateIntervalMs">업데이트 간격 (밀리초)</param>
        public void StartSimulation(MixingUnitVM mixingUnit, int updateIntervalMs = 100)
        {
            lock (_lockObject)
            {
                if (_isRunning)
                {
                    throw new InvalidOperationException("시뮬레이션이 이미 실행 중입니다.");
                }

                _cancellationTokenSource = new CancellationTokenSource();
                _simulationTask = Task.Run(() => RunSimulation(mixingUnit, updateIntervalMs, _cancellationTokenSource.Token));
                _isRunning = true;
            }
        }

        /// <summary>
        /// 시뮬레이션 정지
        /// </summary>
        public void StopSimulation()
        {
            lock (_lockObject)
            {
                if (!_isRunning)
                {
                    return;
                }

                _cancellationTokenSource?.Cancel();
                _simulationTask?.Wait(5000); // 5초 대기
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _simulationTask = null;
                _isRunning = false;
            }
        }

        /// <summary>
        /// 시뮬레이션 메인 루프
        /// </summary>
        private async Task RunSimulation(MixingUnitVM mixingUnit, int updateIntervalMs, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // 1. MixingUnit Output 갱신 (주석 처리)
                    // TODO: Output 갱신 로직 구현
                    // UpdateMixingUnitOutput(mixingUnit);

                    // 2. 내부 모듈들 순차적 Update
                    UpdateModules(mixingUnit);

                    // 지정된 간격만큼 대기
                    await Task.Delay(updateIntervalMs, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // 정상적인 취소
            }
            catch (Exception ex)
            {
                // 예외 처리
                System.Diagnostics.Debug.WriteLine($"시뮬레이션 오류: {ex.Message}");
            }
        }

        /// <summary>
        /// 모든 모듈들을 순차적으로 업데이트
        /// </summary>
        private void UpdateModules(MixingUnitVM mixingUnit)
        {
            try
            {
                // 탱크들 업데이트
                foreach (var tank in mixingUnit.Tanks)
                {
                    tank.Update();
                }

                // 밸브들 업데이트
                foreach (var valve in mixingUnit.Valves)
                {
                    valve.Update();
                }

                // 히터들 업데이트
                foreach (var heater in mixingUnit.Heaters)
                {
                    heater.Update();
                }

                // 믹서들 업데이트
                foreach (var mixer in mixingUnit.Mixers)
                {
                    mixer.Update();
                }

                // 펌프들 업데이트
                foreach (var pump in mixingUnit.Pumps)
                {
                    pump.Update();
                }

                // 레벨 센서들 업데이트
                foreach (var levelSensor in mixingUnit.LevelSensors)
                {
                    levelSensor.Update();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"모듈 업데이트 오류: {ex.Message}");
            }
        }

        /// <summary>
        /// MixingUnit Output 갱신 (향후 구현)
        /// </summary>
        private void UpdateMixingUnitOutput(MixingUnitVM mixingUnit)
        {
            // TODO: MixingUnit의 Output 갱신 로직 구현
            // - 전체 시스템 상태 계산
            // - 출력 값들 업데이트
            // - 외부 시스템과의 통신
        }

        /// <summary>
        /// 리소스 정리
        /// </summary>
        public void Dispose()
        {
            StopSimulation();
        }
    }
}
