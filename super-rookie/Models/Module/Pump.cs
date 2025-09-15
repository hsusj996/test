using super_rookie.Models.Status;

namespace super_rookie.Models.Module
{
    public class Pump
    {
        public string Name { get; set; }
        public DigitalOutput ControlOutput { get; set; } // 장치 제어기에서 모듈로 나가는 제어 신호
        public DigitalInput StatusInput { get; set; }    // 모듈에서 장치 제어기로 들어오는 상태 신호
    }
}
