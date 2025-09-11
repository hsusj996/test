using System;

namespace super_rookie.Models.Status
{
    public class AnalogInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public AnalogInput()
        {
            Id = 0;
            Name = string.Empty;
            Status = 0;
        }

        public AnalogInput(int id, string name, int status = 0)
        {
            Id = id;
            Name = name ?? string.Empty;
            Status = status;
        }
    }
}
