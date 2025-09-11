using System.Collections.Generic;
using super_rookie.Models;

namespace super_rookie.Core
{
    public class MixingUnitStore
    {
        private static MixingUnitStore? _instance;
        private static readonly object _lock = new object();

        public List<MixingUnit> MixingUnits { get; set; }

        private MixingUnitStore()
        {
            MixingUnits = new List<MixingUnit>();
            InitializeTestData();
        }

        public static MixingUnitStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MixingUnitStore();
                        }
                    }
                }
                return _instance;
            }
        }

        private void InitializeTestData()
        {
            MixingUnits.Add(new MixingUnit("CHEM001", "192.168.1.100", "Main Mixing Unit"));
            MixingUnits.Add(new MixingUnit("CHEM002", "192.168.1.101", "Secondary Mixing Unit"));
            MixingUnits.Add(new MixingUnit("CHEM003", "192.168.1.102", "Backup Mixing Unit"));
        }
    }
}
