using System;
using RBot;
using RBot.Options;
using System.Collections.Generic;
using System.Windows.Forms;

public class Script {
	
    public ScriptInterface bot => ScriptInterface.Instance;


	public void ScriptMain(ScriptInterface bot){
		

		// You can do this
		string[] requiredItems = {"Dragon Claw",
			   "Enchanted Scale",
			   "Draco War Medal",
			   "Slayer Helm"};

		Unbank(false, requiredItems);

		// or this
		Unbank(true, "Dragon Claw",
			   "Enchanted Scale",
			   "Draco War Medal",
			   "Slayer Helm");


		// if you use drop list, don't forget to declare these:
		bot.Drops.RejectElse = true;
		bot.Drops.Start();

	}

	/// <summary> Banks all Misc AC items and unbanks your required items. By Bloom</summary>
	/// <param name="Items">Items you want to unbank</param>
	/// <param name="AddToDrops">a bool on whether to add the items to the drop list</param>
	public void Unbank(bool AddToDrops, params string[] Items) {
		
		// Moves Player to safezone in case in combat
		if (bot.Player.Cell != "Wait") bot.Player.Jump("Wait", "Spawn");
		while (bot.Player.State == 2) {}

		// Loads bank
		bot.Player.LoadBank();

		// Declares AC items to bank
		List<string> Whitelisted = new List<string>(){"Note", "Item", "Resource", "QuestItem", "ServerUse"};

		// Banks unneeded items that are included in the whitelisted
		foreach(var item in bot.Inventory.Items) {
			if (!Whitelisted.Contains(item.Category.ToString())) continue;
			if (item.Name != "Treasure Potion" && item.Coins && !Array.Exists(Items, x => x == item.Name)) bot.Inventory.ToBank(item.Name);
		}

		// Unbanks the required items
		foreach (var item in Items) {
			if (bot.Bank.Contains(item)) bot.Bank.ToInventory(item);
			if (AddToDrops) bot.Drops.Add(item);
		}

	}



}
