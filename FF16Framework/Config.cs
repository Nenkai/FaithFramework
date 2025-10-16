using FF16Framework.Template.Configuration;

using Reloaded.Mod.Interfaces.Structs;

using System.ComponentModel;

namespace FF16Framework
{
    public class Config : Configurable<Config>
    {
        [Category("FFXVI")]
        [DisplayName("Serialize Saves as XML")]
        [Description("""
            Advanced Users/Save Editors only.

            Whether to let the game save files as XML within .png files rather than binary.
            The game can load both versions internally.
            Use FF16Tools to extract (and repack) the png saves.

            NOTE: You should only use this while tampering with one save only! Having all saves stored as XML can be pretty slow to load.
            Reminder: Saves are saved in "Documents/My Games/FINAL FANTASY XVI/Steam/<id>/<name>.png".
            """)]
        [DefaultValue(false)]
        public bool SerializeSavesAsXml { get; set; } = false;

        /*
        [DisplayName("Load Saves from XML")]
        [Description("""
            Advanced users/Save Editors only.
            Whether to load saves inside .png files from XML.

            NOTE: They must have been serialized to XML beforehand using above option!
            NOTE 2: Not compatible with system saves.
            """)]
        public bool LoadSavesFromXml { get; set; }
        */
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
