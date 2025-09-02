using System;

namespace super_rookie.Models
{
    // Represents a digital input signal (read by controller from field device like sensor)
    public class DigitalInput
    {
        public string Name { get; set; }

        // True = ON, False = OFF
        public bool State { get; set; }

        public DigitalInput(string name)
        {
            Name = name;
            State = false;
        }
    }
}


