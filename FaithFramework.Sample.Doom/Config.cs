using FaithFramework.Sample.Doom.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FaithFramework.Sample.Doom.Configuration
{
    public class Config : Configurable<Config>
    {
        [Display(Order = 0)]
        [DisplayName("WAD Path")]
        [Description("WAD file to load.")]
        [DefaultValue("")]
        [FilePickerParams(title: "Choose a WAD file to load:")]
        public string WadPath { get; set; } = "";
    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}
