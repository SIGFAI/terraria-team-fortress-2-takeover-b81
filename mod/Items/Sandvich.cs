using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

public class Sandvich : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 22; Item.height = 28;
		Item.useStyle = ItemUseStyleID.EatFood; Item.useTime = 20; Item.useAnimation = 20;
		Item.UseSound = SoundID.Item2; Item.consumable = true; Item.maxStack = 30;
		Item.healLife = 120; Item.rare = ItemRarityID.Green; Item.value = Item.buyPrice(silver: 50);
	}
}
