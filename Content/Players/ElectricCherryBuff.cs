using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BlackOps3.Content.Players
{
	public class ElectricCherryBuff : ModBuff
	{
        public override string Texture => "Terraria/Images/Buff_116";
        public float speedModifier;
		public ElectricCherryBuff(float speed)
		{
			speedModifier=speed;
		}
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.moveSpeed+=speedModifier;
		}
	}
}