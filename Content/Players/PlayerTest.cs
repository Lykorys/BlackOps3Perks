using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BoneTest.Content.Players
{
    public class PlayerTestMod : ModPlayer
    {
        public bool hasJug = false;
        public bool hasSpeed = false;
        public override void PostUpdateEquips()
        {
            if (hasJug)
            {
                Player.statDefense+=4;
            }
            if (hasSpeed)
            {
                Player.moveSpeed+=5;
            }
        }
    }
}