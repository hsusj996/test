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
        void Configure(IList<Unit> units);
        void Start();
        void Stop();
    }

    public class SimulationService : ISimulationService
    {
        private CancellationTokenSource _cts;
        private IList<Unit> _units;

        public bool IsRunning { get; private set; }

        public double StepSeconds { get; set; } = 0.5;

        public void Configure(IList<Unit> units)
        {
            _units = units;
        }

        public void Start()
        {
            if (IsRunning || _units == null) return;
            _cts = new CancellationTokenSource();
            IsRunning = true;
            var token = _cts.Token;
            Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        foreach (var unit in _units)
                        {
                            foreach (var tank in unit.Tanks)
                            {
                                foreach (var v in tank.Valves) v.Update();
                                tank.Update(StepSeconds);
                                foreach (var s in tank.LevelSensors) s.Update(tank.Amount);
                            }
                        }
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


