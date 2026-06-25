//Increase mag size
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent;
using BlackOps3.Content.Players;
using BlackOps3.Content.Utils.Functions;

namespace BlackOps3.Content.Items.Tiles.Perks
{
    public class StaminUpTile : PerkMachine
    {
        public override Perk perk => new StaminUp();
        public override int[] prices => [500, 1500, 3000, 4500];
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
    public class StaminUpEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<StaminUpTile>();
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
    public class StaminUpItem : ModItem 
    {
        public override string Texture => "Terraria/Images/Tiles_26";
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<StaminUpTile>(), 1);
            Item.width = 28;
            Item.height = 14;
        }
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddIngredient(ItemID.TissueSample, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            Rectangle sourceRect = new Rectangle(54, 0, 54, 34); 
            Texture2D texture = TextureAssets.Item[Type].Value;
            spriteBatch.Draw(texture, position, sourceRect, drawColor, 0f, origin, scale * 0.5f, SpriteEffects.None, 0f);
            return false;
        }
    }
}