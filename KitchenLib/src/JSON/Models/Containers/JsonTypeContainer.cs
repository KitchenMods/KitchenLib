using static KitchenLib.src.JSON.ContentPackUtils;

namespace KitchenLib.src.JSON.Models.Containers
{
    public class JsonTypeContainer
    {
        public ChangeType Type { get; set; }
        public SerializationContext Context { get; set; }
    }

    public enum ChangeType
    {
        CustomGDO,
        ModifyGDO
    }
}
