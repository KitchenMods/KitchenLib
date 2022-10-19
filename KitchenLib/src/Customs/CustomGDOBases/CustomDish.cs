using KitchenData;
using KitchenLib.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomDish
	{
        public virtual string Name { get; internal set; }
        public virtual DishType Type { get; internal set; }
        public virtual string AchievementName { get; internal set; }
        public virtual List<Dish.MenuItem> UnlocksMenuItems { get; internal set; }
        public virtual HashSet<Dish.IngredientUnlock> UnlocksIngredients { get; internal set; }
        public virtual HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks { get; internal set; }
        public virtual List<string> StartingNameSet { get {return new List<string>(); } }
        public virtual HashSet<Item> MinimumIngredients { get; internal set; }
        public virtual HashSet<Process> RequiredProcesses { get; internal set; }
        public virtual HashSet<Item> BlockProviders { get; internal set; }
        public virtual HashSet<Dish> PrerequisiteDishes { get; internal set; }
        public virtual GameObject IconPrefab { get; internal set; }
        public virtual GameObject DisplayPrefab { get; internal set; }

        public virtual int ID { get; internal set; }
        public string ModName { get; internal set; }
        public virtual int BaseDishId { get { return -1; } }
        public Dish Dish{ get; internal set; }
        public virtual void OnRegister(Dish dish) { }
        public int GetHash() { return StringUtils.GetInt32HashCode($"{ModName}:{Name}"); }
    }
}