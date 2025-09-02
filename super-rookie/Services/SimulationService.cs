using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using super_rookie.Models;

namespace super_rookie.Services
{
    public interface ISimulationService
    {
        bool IsRunning { get; }
        double StepSeconds { get; set; }
        void Configure(Tank tank, IList<Valve> valves, IList<LevelSensor> sensors);
        void Start();
        void Stop();
    }

    public class SimulationService : ISimulationService
    {
        private CancellationTokenSource _cts;
        private Tank _tank;
        private IList<Valve> _valves;
        private IList<LevelSensor> _sensors;

        public bool IsRunning { get; private set; }

        public double StepSeconds { get; set; } = 0.5;

        public void Configure(Tank tank, IList<Valve> valves, IList<LevelSensor> sensors)
        {
            _tank = tank;
            _valves = valves;
            _sensors = sensors;
        }

        public void Start()
        {
            if (IsRunning || _tank == null || _valves == null || _sensors == null) return;
            _cts = new CancellationTokenSource();
            IsRunning = true;
            var token = _cts.Token;
            Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        foreach (var v in _valves) v.Update();
                        _tank.Update(StepSeconds);
                        foreach (var s in _sensors) s.Update(_tank.Amount);
                        await Task.Delay(TimeSpan.FromSeconds(StepSeconds), token).ConfigureAwait(false);
                    }
                }
                catch (TaskCanceledException) { }
                finally
                {
                    IsRunning = false;
                }
            }, token);
        }

        public void Stop()
        {
            if (!IsRunning) return;
            _cts.Cancel();
        }
    }
}


