using System;

namespace super_rookie.Models
{
    // Represents a digital output signal (written by controller to field device like valve)
    public class DigitalOutput
    {
        public string Name { get; set; }

        // True = ON, False = OFF
        public bool State { get; set; }

        public DigitalOutput(string name)
        {
            Name = name;
            State = false;
        }
    }
}


