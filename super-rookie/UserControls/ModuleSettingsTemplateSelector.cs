using System;
using System.Windows;
using System.Windows.Controls;
using super_rookie.ViewModels.Module;

namespace super_rookie.UserControls
{
    public class ModuleSettingsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TankTemplate { get; set; }
        public DataTemplate ValveTemplate { get; set; }
        public DataTemplate HeaterTemplate { get; set; }
        public DataTemplate MixerTemplate { get; set; }
        public DataTemplate PumpTemplate { get; set; }
        public DataTemplate LevelSensorTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return DefaultTemplate;

            return item switch
            {
                TankVM => TankTemplate,
                ValveVM => ValveTemplate,
                HeaterVM => HeaterTemplate,
                MixerVM => MixerTemplate,
                PumpVM => PumpTemplate,
                LevelSensorVM => LevelSensorTemplate,
                _ => DefaultTemplate
            };
        }
    }
}
