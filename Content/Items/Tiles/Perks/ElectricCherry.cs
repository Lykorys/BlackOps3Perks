//On reload shock
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent;
using BlackOps3.Content.Players;
using BlackOps3.Content.Systems;
using Terraria.DataStructures;
using Terraria.Enums;

namespace BlackOps3.Content.Items.Tiles.Perks
{
    public class ElectricCherryTile : PerkMachine
    {
        public override Perk perk => new ElectricCherry();
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
    public class ElectricCherryEntity : ModTileEntity
    {
         public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<ElectricCherryTile>();
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
    public class ElectricCherryItem : ModItem 
    {
        public override string Texture => "BlackOps3/Content/Players/PerksLogo/ElectricCherryLogo";
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<ElectricCherryTile>());
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