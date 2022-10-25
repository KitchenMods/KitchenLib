using System.Collections.Generic;
using UnityEngine;
using Kitchen;
using Kitchen.Modules;

namespace KitchenLib
{
    public class KLMenu<T> : Menu<T>
    {
        public KLMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id){}
        protected void BoolOption(BoolPreference pref)
        {
			this.Add<bool>(new Option<bool>(new List<bool>
			{
				false,
				true
			}, (bool)pref.Value, new List<string>
			{
				this.Localisation["SETTING_DISABLED"],
				this.Localisation["SETTING_ENABLED"]
			}, null)).OnChanged += delegate(object _, bool f)
			{
				pref.Value = f;
			};
        }
    }
}