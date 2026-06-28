using BlackOps3.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ModLoader.IO; // Required for TagCompound
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using log4net.DateFormatter;
using BlackOps3.Content.Systems;
using System.Linq;
using Mono.Cecil.Cil;
using Steamworks;
namespace BlackOps3.Content.Items.Tiles
{
    public class PackAPunchTile : ModTile
    {
        public override string Texture => "Terraria/Images/Tiles_77";

        public override void SetStaticDefaults() {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<PackAPunchEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(100, 100, 100));
        }

        public override bool RightClick(int i, int j) {

            // Use 3 for the width, as it is a 3x2 tile
            int frameX = Main.tile[i, j].TileFrameX / 18;
            int frameY = Main.tile[i, j].TileFrameY / 18;
            int x = i - (frameX % 3); 
            int y = j - (frameY % 2);
            Point16 pos = new Point16(x, y);
            // DEBUGGING: Tell us if the entity exists at all
            if (!TileEntity.ByPosition.TryGetValue(new Point16(x, y), out TileEntity te) || !(te is PackAPunchEntity entity))
                return false;

            Player player = Main.LocalPlayer;
            bool isPackable = PackAPunchProcesses.PunchUpgrades.Any(recipe => recipe.Input == player.HeldItem.type);
            if (!entity.itemSlot.IsAir) {
                player.QuickSpawnItem(player.GetSource_TileInteraction(x, y), entity.itemSlot);
                entity.itemSlot = new Item();
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, new Vector2(x * 16, y * 16));
            } else if (!player.HeldItem.IsAir && isPackable) {
                // --- ADDED SUCKING VISUALS ---
                for (int k = 0; k < 10; k++) {
                    Dust.NewDust(player.Center, 10, 10, DustID.MagicMirror, 
                        (x * 16 - player.Center.X) / 10, (y * 16 - player.Center.Y) / 10);
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, new Vector2(x * 16, y * 16));
                
                entity.itemSlot = player.HeldItem.Clone();
                entity.itemSlot.stack = 1;
                player.HeldItem.stack--;
                if (player.HeldItem.stack <= 0) player.HeldItem.TurnToAir();
            }

            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, entity.ID, x, y);
            }
            
            return true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            // 1. Only run this once for the whole 3x2 tile structure
            // We check if it's the top-left tile
            if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0) {
                
                // 2. Find the entity at this location
                if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te) && te is PackAPunchEntity pEntity) {
                    float progress = (float)pEntity.timer / PackAPunchProcesses.Duration;
                    float  scale = 1f - progress;
                    if (pEntity.timer < PackAPunchProcesses.Duration){
                        Texture2D texture = Terraria.GameContent.TextureAssets.Item[pEntity.itemSlot.type].Value;
                        Vector2 tileCenter = new(i * 16 + 24, j * 16 + 16);
                        Vector2 drawPos = tileCenter - Main.screenPosition;
                        spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, texture.Size() / 2f, scale, SpriteEffects.None, 0f);
                    }
                    else if(pEntity.timer == PackAPunchProcesses.Duration){
                        Texture2D texture = Terraria.GameContent.TextureAssets.Item[pEntity.itemSlot.type].Value;
                        Vector2 tileCenter = new(i * 16 + 24, j * 16 + 16);
                        Vector2 drawPos = tileCenter - Main.screenPosition;
                        spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, texture.Size() / 2, 1f, SpriteEffects.None, 0f);
                    }
                    else if(pEntity.timer > PackAPunchProcesses.Duration && pEntity.timer < PackAPunchProcesses.Timeout){
                        Texture2D texture = Terraria.GameContent.TextureAssets.Item[pEntity.itemSlot.type].Value;
                        Vector2 tileCenter = new(i * 16 + 24, j * 16 + 16);
                        Vector2 drawPos = tileCenter - Main.screenPosition;
                        spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, texture.Size() / 2f, scale, SpriteEffects.None, 0f);
                    }
                    
                }
            }
}
    }

    public class PackAPunchEntity : ModTileEntity
    {
        public Item itemSlot = new Item();
        public int timer = 0;
        public PackAPunchProcesses.PunchProcess recipe;
        public override bool IsTileValidForEntity(int x, int y) => Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<PackAPunchTile>();

        public override void Update() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            if(timer==0) recipe=PackAPunchProcesses.PunchUpgrades.FirstOrDefault(x => x.Input == itemSlot.type);
            if (recipe!=null) {//contains normal
                
                timer++;
                if (timer % 10 == 0) {
                    Dust.NewDust(Position.ToWorldCoordinates(), 32, 32, DustID.PurpleTorch);
                }
                if (timer == 120) {
                    Main.NewText("Cooked");
                    itemSlot.SetDefaults(recipe.Output);
                    recipe=null;
                    // Sync and play finished sound
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item37, Position.ToWorldCoordinates());
                }
            }
            else if(itemSlot.type != ItemID.None)
            {
                timer++;
                if(timer >= 480)
                {
                    Main.NewText("too late");
                    timer=0;
                    itemSlot.TurnToAir();
                    recipe=null;
                } 
            }
            else {
                timer = 0; // Reset if item is removed
            }
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
            // The i, j passed here are the coordinates of the click. 
            // We need the top-left, so we must calculate it based on the frame.
            int x = i - (Main.tile[i, j].TileFrameX / 18 % 3);
            int y = j - (Main.tile[i, j].TileFrameY / 18 % 2);

            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendTileSquare(Main.myPlayer, x, y, 3, 2);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, x, y, Type, 0, 0, 0, 0);
                return -1;
            }
            return Place(x, y);
        }
        
        public override void NetSend(BinaryWriter writer) {
            ItemIO.Send(itemSlot, writer, true);
        }
        public override void NetReceive(BinaryReader reader) {
            itemSlot = ItemIO.Receive(reader, true);
        }
        public override void SaveData(TagCompound tag) => tag["item"] = ItemIO.Save(itemSlot);
        public override void LoadData(TagCompound tag) => itemSlot = ItemIO.Load(tag.GetCompound("item"));
    }
    public class PackAPunch : ModItem
    {
        public override string Texture => "Terraria/Images/Item_221";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<PackAPunchTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 10) 
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}