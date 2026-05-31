using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent;
using BoneTest.Content.Players;

namespace BoneTest.Content.Items.Tiles.Perks
{
    public class JuggernogTile : ModTile
    {
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            // 1. Dimensions for a 2x4 object
            TileObjectData.newTile.Width = 2;   
            TileObjectData.newTile.Height = 4;  

            // 2. Set pixel size to standard 16x16 blocks
            TileObjectData.newTile.CoordinateWidth = 16; 
            
            // 3. EVERY standard Terraria sprite needs 2 pixels of padding between frames
            TileObjectData.newTile.CoordinatePadding = 2; 

            // 4. Set the height for all 4 vertical frames. The bottom-most frame usually
            // includes the 2px padding, so make it 18 to match the sprite layout.
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 18 };

            // 5. Bottom-left block of the 2x4 is the origin (0, 3)
            TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(0, 3); 

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(150, 50, 255)); 
            AdjTiles = new int[] { TileID.Hellforge };
        }

        public override bool RightClick(int i, int j) {
            Tile tile = Main.tile[i, j];
            int left = i - (tile.TileFrameX / 18);
            int top = j - (tile.TileFrameY / 18);
            Player player = Main.LocalPlayer;
            PlayerPerks modPlayer = player.GetModPlayer<PlayerPerks>();
            if (!modPlayer.hasJug) {
                modPlayer.hasJug = true;
                Main.NewText("JugActive");
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item3, player.position);
            } else {
                Main.NewText("already");
            }
            return true;
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
                // Change the size parameter to 4 so it covers the whole 2x4 structure
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 4);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type);
                return -1;
            }
            return Place(i, j);
        }
    }
    public class JuggernogItem : ModItem 
    {
        public override string Texture => "Terraria/Images/Tiles_26";
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<JuggernogTile>(), 1);
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