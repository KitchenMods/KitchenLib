using KitchenData;
using KitchenLib.src.ContentPack.Models.Containers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class UnlockEffectConverter : CustomConverter<UnlockEffect>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            if (jObject.TryGetValue("Type", out JToken type))
            {
                UnlockEffectContainer unlockEffectContainer = new UnlockEffectContainer();

                UnlockEffectContext EffectContext = type.ToObject<UnlockEffectContext>();
                unlockEffectContainer.Type = EffectContext;

                UnlockEffect effect = EffectContext switch
                {
                    UnlockEffectContext.FranchiseEffect => new FranchiseEffect(),
                    UnlockEffectContext.GlobalEffect => new GlobalEffect(),
                    UnlockEffectContext.ParameterEffect => new ParameterEffect(),
                    UnlockEffectContext.ShopEffect => new ShopEffect(),
                    UnlockEffectContext.StartBonusEffect => new StartBonusEffect(),
                    UnlockEffectContext.StatusEffect => new StatusEffect(),
                    UnlockEffectContext.ThemeAddEffect => new ThemeAddEffect(),
                    _ => null,
                };
                serializer.Populate(jObject["Effect"].CreateReader(), effect);
                unlockEffectContainer.Effect = effect;

                return unlockEffectContainer;
            }
            return null;
        }
    }
}
