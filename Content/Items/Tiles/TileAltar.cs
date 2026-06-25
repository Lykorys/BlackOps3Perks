using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent; // Added for TextureAssets
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics; // Required for SpriteBatch and Texture2D

namespace BlackOps3.Content.Items.Tiles
{
    public class AltarTile : ModTile
    {
        public override string Texture => "Terraria/Images/Tiles_26"; // Points directly to vanilla Altars
        public override void SetStaticDefaults() {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            // Point to the vanilla texture sheet containing both altars
            if (!Main.dedServ) {
                TextureAssets.Tile[Type] = TextureAssets.Tile[TileID.DemonAltar];
            }

            // Setup the 3x2 grid
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            
            // This is the key: it tells the game there are multiple 3x2 versions side-by-side
            TileObjectData.newTile.StyleHorizontal = true; 
            
            TileObjectData.addTile(Type);

            // Map colors (Optional: you can make this change based on style, 
            // but a neutral purple/red works too)
            AddMapEntry(new Color(150, 50, 255)); 
            
            AdjTiles = new int[] { TileID.DemonAltar };
        }

    }
    public class CrimsonAltarItem : ModItem {
        public override string Texture => "Terraria/Images/Tiles_26"; // Points the item to the tile sheet
        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<AltarTile>(), 1);
            Item.width = 28;
            Item.height = 14;
        }
        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddIngredient(ItemID.TissueSample,10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
        // The Crimson Altar starts 54 pixels to the right on the same sheet
            Rectangle sourceRect = new Rectangle(54, 0, 54, 34); 
            Texture2D texture = TextureAssets.Item[Type].Value;

            spriteBatch.Draw(texture, position, sourceRect, drawColor, 0f, origin, scale * 0.5f, SpriteEffects.None, 0f);
            return false;
        }
    }
    public class CorruptAltarItem : ModItem {
        public override string Texture => "Terraria/Images/Tiles_26"; // Points the item to the tile sheet
        public override void SetDefaults() {
            // The '0' at the end means Style 0 (Demon)
            Item.DefaultToPlaceableTile(ModContent.TileType<AltarTile>(), 0);
            Item.width = 28;
            Item.height = 14;
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 10)
                .AddIngredient(ItemID.ShadowScale, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            // The Demon Altar is at the top left of the sheet. 
            // We define a 3x2 tile area (54x34 pixels)
            Rectangle sourceRect = new Rectangle(0, 0, 54, 34); 
            Texture2D texture = TextureAssets.Item[Type].Value;

            spriteBatch.Draw(texture, position, sourceRect, drawColor, 0f, origin, scale * 0.5f, SpriteEffects.None, 0f);
            return false; // Tells Terraria not to draw the default (blank) icon
        }
    }
}