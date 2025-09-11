using super_rookie.ViewModels.Module;

namespace super_rookie.ViewModels.Messages
{
    public record TankLevelChangedMessage(TankVM Tank, double Capacity, double Amount);
}
