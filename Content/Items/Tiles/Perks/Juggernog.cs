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
    public class JuggernogTile : PerkMachine
    {
        public override Perk perk => new Juggernog();
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16,16,16];
            TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(150, 50, 255)); 
        }


    }
    public class JuggernogEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<JuggernogTile>();
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
    public class JuggernogItem : ModItem 
    {
        public override string Texture => "BlackOps3/Content/Players/PerksLogo/JuggernogLogo";
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<JuggernogTile>());
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