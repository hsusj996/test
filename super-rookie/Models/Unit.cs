using System.Collections.Generic;

namespace super_rookie.Models
{
    public class Unit
    {
        public string Name { get; set; }
        public List<Tank> Tanks { get; } = new List<Tank>();

        public Unit(string name)
        {
            Name = name;
        }

        public void AddTank(Tank tank)
        {
            if (tank == null) return;
            Tanks.Add(tank);
        }

        public void RemoveTank(Tank tank)
        {
            if (tank == null) return;
            Tanks.Remove(tank);
        }
    }
}


