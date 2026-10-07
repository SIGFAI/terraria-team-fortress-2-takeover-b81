using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

public class MannCoCrate : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30; Item.height = 26;
		Item.useStyle = ItemUseStyleID.HoldUp; Item.useTime = 20; Item.useAnimation = 20;
		Item.consumable = true; Item.maxStack = 9; Item.rare = ItemRarityID.Pink;
		Item.UseSound = SoundID.Item4;
	}

	public override bool? UseItem(Player player)
	{
		int[] loot = { ModContent.ItemType<RocketLauncher>(), ModContent.ItemType<Flamethrower>(), ModContent.ItemType<Minigun>(), ItemID.GoldCoin };
		Mix.Drop(Main.rand.Next(loot), player.Center + new Vector2(0, -30), 1);
		Mix.Drop<Sandvich>(player.Center + new Vector2(0, -30), 3);
		Mix.Burst(player.Center, DustID.GoldCoin, 25, 24f, 5f);
		Mix.Popup(player.Center, "Crate opened!", Color.Gold);
		return true;
	}
}
