//Increase mag size
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
    public class MuleKickTile : PerkMachine
    {
        public override Perk perk => new MuleKick();
        public override string Texture => "Terraria/Images/Tiles_26"; 
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(150, 50, 255)); 
            AdjTiles = new int[] { TileID.Hellforge };
        }
    }
    public class MuleKickEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<MuleKickTile>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3, 2);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }
            return Place(i, j);
        }
    }
    public class MuleKickItem : ModItem 
    {
        public override string Texture => "BlackOps3/Content/Players/PerksLogo/MuleKickLogo";
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<MuleKickTile>(), 1);
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