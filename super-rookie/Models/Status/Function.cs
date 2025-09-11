using System;

namespace super_rookie.Models.Status
{
    public class Function
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public Function()
        {
            Id = 0;
            Name = string.Empty;
            Status = false;
        }

        public Function(int id, string name, bool status = false)
        {
            Id = id;
            Name = name ?? string.Empty;
            Status = status;
        }
    }
}
