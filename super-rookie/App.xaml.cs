using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Threading;
using super_rookie.Models;

namespace super_rookie
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        private CancellationTokenSource _cts;

        private Tank _tank;

        // Collections for multiple devices
        private List<Valve> _valves = new List<Valve>();
        private List<LevelSensor> _sensors = new List<LevelSensor>();

        private double _dtSeconds = 0.5; // simulation step

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AllocConsole();
            Console.WriteLine("=== Chemical Feeder Simulator (Console) ===");
            Console.WriteLine("Enter Tank Capacity (e.g., 100):");
            double capacity = ReadDoubleOrDefault(100);
            Console.WriteLine("Enter Initial Amount (e.g., 0):");
            double initial = ReadDoubleOrDefault(0);

            _tank = new Tank("T1", capacity, initial);

            Console.WriteLine("Enter number of INLET valves (e.g., 1):");
            int numInlet = (int)ReadDoubleOrDefault(1);
            Console.WriteLine("Enter number of OUTLET valves (e.g., 1):");
            int numOutlet = (int)ReadDoubleOrDefault(1);
            Console.WriteLine("Enter number of level sensors (e.g., 1):");
            int numSensors = (int)ReadDoubleOrDefault(1);

            for (int i = 0; i < numInlet; i++)
            {
                Console.WriteLine($"Inlet[{i}] FlowRate (L/s) [1.0]:");
                double flow = ReadDoubleOrDefault(1.0);
                var doSig = new DigitalOutput($"DO_IN_{i}");
                var valve = new Valve($"V_IN_{i}", ValveType.Inlet, flow) { CommandDo = doSig };
                _valves.Add(valve);
                _tank.AttachValve(valve);
            }

            for (int i = 0; i < numOutlet; i++)
            {
                Console.WriteLine($"Outlet[{i}] FlowRate (L/s) [0.5]:");
                double flow = ReadDoubleOrDefault(0.5);
                var doSig = new DigitalOutput($"DO_OUT_{i}");
                var valve = new Valve($"V_OUT_{i}", ValveType.Outlet, flow) { CommandDo = doSig };
                _valves.Add(valve);
                _tank.AttachValve(valve);
            }

            for (int i = 0; i < numSensors; i++)
            {
                Console.WriteLine($"LevelSensor[{i}] Trigger Amount [80]:");
                double trigger = ReadDoubleOrDefault(80);
                var diSig = new DigitalInput($"DI_LS_{i}");
                var sensor = new LevelSensor($"LS_{i}", trigger) { StatusDi = diSig };
                _sensors.Add(sensor);
                _tank.AttachSensor(sensor);
            }

            _cts = new CancellationTokenSource();
            StartSimulationLoop(_cts.Token);

            PrintHelp();
            CommandLoop();

            Shutdown();
        }

        private void StartSimulationLoop(CancellationToken token)
        {
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    // 1) Valves read DO
                    foreach (var v in _valves) v.Update();

                    // 2) Tank integrates flow
                    _tank.Update(_dtSeconds);

                    // 3) Sensors write DI
                    foreach (var s in _sensors) s.Update(_tank.Amount);

                    await Task.Delay(TimeSpan.FromSeconds(_dtSeconds), token).ConfigureAwait(false);
                }
            }, token);
        }

        private void CommandLoop()
        {
            Console.WriteLine("Type commands. 'help' to list, 'quit' to exit.");
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == null) continue;
                string cmd = line.Trim().ToLowerInvariant();
                if (cmd == "quit" || cmd == "exit")
                {
                    _cts.Cancel();
                    break;
                }
                else if (cmd == "help")
                {
                    PrintHelp();
                }
                else if (cmd == "status")
                {
                    PrintStatus();
                }
                else if (cmd.StartsWith("open "))
                {
                    if (TryParseIndex(cmd, out int idx))
                    {
                        SetValveDo(idx, true);
                    }
                    else
                    {
                        Console.WriteLine("Usage: open <index>");
                    }
                }
                else if (cmd.StartsWith("close "))
                {
                    if (TryParseIndex(cmd, out int idx))
                    {
                        SetValveDo(idx, false);
                    }
                    else
                    {
                        Console.WriteLine("Usage: close <index>");
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command. Type 'help'.");
                }
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("  status          - show tank amount, valves and sensors states");
            Console.WriteLine("  open <index>    - open valve by index (see status)");
            Console.WriteLine("  close <index>   - close valve by index");
            Console.WriteLine("  help          - show this help");
            Console.WriteLine("  quit          - exit application");
        }

        private void PrintStatus()
        {
            Console.WriteLine($"Amount: {_tank.Amount:F2} / Capacity: {_tank.Capacity:F2}");
            Console.WriteLine("Valves:");
            for (int i = 0; i < _valves.Count; i++)
            {
                var v = _valves[i];
                string doState = v.CommandDo != null ? v.CommandDo.State.ToString() : "(no DO)";
                Console.WriteLine($"  [{i}] {v.Name} {v.Direction} Flow={v.FlowRate:F2} IsOpen={v.IsOpen} DO={doState}");
            }
            Console.WriteLine("Sensors:");
            for (int i = 0; i < _sensors.Count; i++)
            {
                var s = _sensors[i];
                string diState = s.StatusDi != null ? s.StatusDi.State.ToString() : "(no DI)";
                Console.WriteLine($"  [{i}] {s.Name} Trigger={s.TriggerAmount:F2} IsTriggered={s.IsTriggered} DI={diState}");
            }
        }

        private static double ReadDoubleOrDefault(double defaultValue)
        {
            Console.Write($"[{defaultValue}] > ");
            string s = Console.ReadLine();
            if (double.TryParse(s, out double value)) return value;
            return defaultValue;
        }

        private bool TryParseIndex(string cmd, out int index)
        {
            index = -1;
            var tokens = cmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 2) return false;
            return int.TryParse(tokens[1], out index) && index >= 0 && index < _valves.Count;
        }

        private void SetValveDo(int index, bool state)
        {
            var v = _valves[index];
            if (v.CommandDo == null)
            {
                v.CommandDo = new DigitalOutput($"DO_{v.Name}");
            }
            v.CommandDo.State = state;
            Console.WriteLine($"Valve[{index}] {v.Name} DO={(state ? "ON" : "OFF")}" );
        }
    }
}
