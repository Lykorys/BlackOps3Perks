using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
public class OverwrittenRecipes : ModSystem
{
    public override void AddRecipes()
    {
        //WormHole Potion
        Recipe wormhole = Recipe.Create(ItemID.WormholePotion, 3);
        wormhole.AddIngredient(ItemID.RecallPotion);
        wormhole.AddIngredient(ItemID.BottledWater , 2);
        wormhole.Register();

        //Hermes Boots
        Recipe hermes = Recipe.Create(ItemID.HermesBoots);
        hermes.AddIngredient(ItemID.Cloud ,10);
        hermes.AddIngredient(ItemID.Feather , 3);
        hermes.AddIngredient(ItemID.Leather , 2);
        hermes.AddTile(TileID.TinkerersWorkbench);
        hermes.Register();
    }
}