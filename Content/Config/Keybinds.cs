using Terraria.ModLoader;
namespace BlackOps3.Content.Config
{
	public class KeybindSystem : ModSystem
	{
		public static ModKeybind Reload { get; private set; }

		public override void Load() {
			// Registers a new keybind
			// We localize keybinds by adding a Mods.{ModName}.Keybind.{KeybindName} entry to our localization files. The actual text displayed to English users is in en-US.hjson
			Reload = KeybindLoader.RegisterKeybind(Mod, "Reload", "R");
		}

		// Please see ExampleMod.cs' Unload() method for a detailed explanation of the unloading process.
		public override void Unload() {
			// Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
			Reload = null;
		}
	}
}