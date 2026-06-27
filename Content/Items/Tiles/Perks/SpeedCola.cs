//Increase mag size
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.Enums;
using BlackOps3.Content.Players;
using BlackOps3.Content.Utils.Functions;

namespace BlackOps3.Content.Items.Tiles.Perks
{
    public class SpeedColaTile : PerkMachine
    {
        public override Perk perk => new SpeedCola();
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(150, 50, 255)); 
            AdjTiles = new int[] { TileID.Hellforge };
        }
    }

    public class SpeedColaEntity : ModTileEntity
    {
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<SpeedColaTile>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3, 4);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }
            return Place(i, j);
        }
    }

    public class SpeedColaItem : ModItem 
    {
        public override string Texture => "Terraria/Images/Tiles_26"; // temporary texture reference

        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<SpeedColaTile>()); // Cleans up and configures modern placement mechanics!
            Item.width = 28;
            Item.height = 14;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
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