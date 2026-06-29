using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace BlackOps3.Content.Players
{
	public class ElectricCherrySpeedBuff : ModBuff
	{
        public override string Texture => "Terraria/Images/Buff_116";
        public float speedModifier = 0.25f;
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.moveSpeed+=speedModifier;
		}
	}
	public class ElectricCherryStunDebuff : ModBuff
	{
        public override string Texture => "Terraria/Images/Buff_116";
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity = Vector2.Zero;
            if (npc.aiStyle != NPCAIStyleID.FaceClosestPlayer) 
            {
                npc.justHit = true;
            }
            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric);
                d.velocity *= 0.2f;
                d.scale = 0.8f;
            }
        }
	}
    public class ElectricAuraProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.MartianTurretBolt;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 40;
            Projectile.hide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.aiStyle = 0;
            Main.projFrames[Projectile.type] = 4;
        }

        public override void AI()
        {
            Projectile.rotation = MathHelper.PiOver2;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            if (Projectile.timeLeft < 10)
            {
                Projectile.alpha += 10;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
            Vector2 origin = frame.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                frame,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}