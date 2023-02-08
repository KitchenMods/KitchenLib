using KitchenData;
using System.Collections.Generic;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ModularUnlockPackDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder modularUnlockPackSetsDump = new StringBuilder();
			StringBuilder modularUnlockPackFiltersDump = new StringBuilder();
			StringBuilder modularUnlockPackSortersDump = new StringBuilder();
			StringBuilder modularUnlockPackOptionsDump = new StringBuilder();

			modularUnlockPackSetsDump.AppendLine("ID,Type,IUnlockSet");
			modularUnlockPackFiltersDump.AppendLine("ID,Type,IUnlockFilter");
			modularUnlockPackSortersDump.AppendLine("ID,Type,IUnlockSorter");
			modularUnlockPackOptionsDump.AppendLine("ID,Type,Selector,Condition");

			foreach (ModularUnlockPack modularUnlockPack in GameData.Main.Get<ModularUnlockPack>())
			{
				foreach (IUnlockSet set in modularUnlockPack.Sets)
				{
					modularUnlockPackSetsDump.AppendLine($"{modularUnlockPack.ID},{modularUnlockPack.name},{set}");
				}
				foreach (IUnlockFilter filter in modularUnlockPack.Filter)
				{
					modularUnlockPackFiltersDump.AppendLine($"{modularUnlockPack.ID},{modularUnlockPack.name},{filter}");
				}
				foreach (IUnlockSorter sorter in modularUnlockPack.Sorters)
				{
					modularUnlockPackSortersDump.AppendLine($"{modularUnlockPack.ID},{modularUnlockPack.name},{sorter}");
				}
				foreach (ConditionalOptions option in modularUnlockPack.ConditionalOptions)
				{
					modularUnlockPackOptionsDump.AppendLine($"{modularUnlockPack.ID},{modularUnlockPack.name},{option.Selector},{option.Condition}");
				}
			}

			SaveCSV("ModularUnlockPack", "ModularUnlockPackSets", modularUnlockPackSetsDump);
			SaveCSV("ModularUnlockPack", "ModularUnlockPackFilters", modularUnlockPackFiltersDump);
			SaveCSV("ModularUnlockPack", "ModularUnlockPackSorters", modularUnlockPackSortersDump);
			SaveCSV("ModularUnlockPack", "ModularUnlockPackOptions", modularUnlockPackOptionsDump);
		}
	}
}
