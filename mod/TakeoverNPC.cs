using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Every vanilla monster that spawns is swapped for a BLU mercenary.</summary>
public class TakeoverNPC : GlobalNPC
{
	public override void OnSpawn(NPC npc, IEntitySource source) => Swap(npc);

	// Catches monsters that were already in the world (or spawned some other way).
	public override bool PreAI(NPC npc)
	{
		Swap(npc);
		return npc.active;
	}

	static void Swap(NPC npc)
	{
		if (!npc.active || npc.ModNPC != null || npc.boss || npc.friendly || npc.townNPC || npc.damage <= 0 || npc.lifeMax <= 5) return;
		if (npc.type == NPCID.TargetDummy) return;
		Vector2 at = npc.Bottom;
		npc.active = false;
		npc.life = 0;
		int[] mercs = { ModContent.NPCType<Soldier>(), ModContent.NPCType<Scout>(), ModContent.NPCType<Scout>(), ModContent.NPCType<Heavy>(), ModContent.NPCType<Pyro>(), ModContent.NPCType<Soldier>() };
		Mix.Spawn(mercs[Main.rand.Next(mercs.Length)], at);
		Mix.Burst(at + new Vector2(0, -20), DustID.MagicMirror, 14, 16f, 3f);
	}

	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
	{
		spawnRate = (int)(spawnRate * 0.35f);
		maxSpawns = (int)(maxSpawns * 2f);
	}
}
