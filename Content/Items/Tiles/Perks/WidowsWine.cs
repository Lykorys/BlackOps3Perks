/*
Effect 1: Your grenades turn into sticky grenades that will slow zombies damaged by them.
Effect 2: If you are hit by a zombie, you will "explode", slowing/stunning all zombies in your vicinity, and consume one of your grenades (If you have no grenades this will not work).
Effect 3: Grenades will replenish by 2 at the start of every round, and killing zombies has a chance to drop a spider power up, which will give you a grenade.
Effect 4: Your knife will deal more damage and has a chance to slow zombies.

On hit idem CD 10-30 sec
*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent;
using BlackOps3.Content.Players;
using BlackOps3.Content.Systems;

namespace BlackOps3.Content.Items.Tiles.Perks
{
    public class WidowsWineTile : PerkMachine
    {
        public override Perk perk => new WidowsWine();
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16,16};
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(150, 50, 255)); 
        }
    }
    public class WidowsWineEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<WidowsWineTile>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 5);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }
            return Place(i, j);
        }
    }
    public class WidowsWineItem : ModItem 
    {
        public override string Texture => "BlackOps3/Content/Players/PerksLogo/WidowsWineLogo";
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<WidowsWineTile>());
            Item.width = 32;
            Item.height = 32;
        }
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddIngredient(ItemID.TissueSample, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}