//keep perks after death except this one
//Increase mag size
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent;
using Terraria.DataStructures;
using BlackOps3.Content.Players;
using BlackOps3.Content.Systems;

namespace BlackOps3.Content.Items.Tiles.Perks
{
    public class QuickReviveTile : PerkMachine
    {
        public override Perk perk => new QuickRevive();
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16,16];
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(150, 50, 255)); 
        }


    }
    public class QuickReviveEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<QuickReviveTile>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 4);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }
            return Place(i, j);
        }
    }
    public class QuickReviveItem : ModItem 
    {
        public override string Texture => "BlackOps3/Content/Players/PerksLogo/QuickReviveLogo";
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<QuickReviveTile>());
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