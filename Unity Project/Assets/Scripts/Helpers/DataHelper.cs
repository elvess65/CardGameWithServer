using System.Collections.Generic;

namespace CardGame.Helpers
{
    public static class DataHelper
    {
        public enum EResourceTypes
        {
            Body,
            Face,
            Hair,
            Kit
        }

        public enum ECardTypes
        {
            Attack, Defence, Max
        }

        private static Dictionary<EResourceTypes, string> m_resourceTypesToNames = new()
        {
            { EResourceTypes.Body, "Body" },
            { EResourceTypes.Face, "Face" },
            { EResourceTypes.Hair, "Hair" },
            { EResourceTypes.Kit, "Kit" },
        };
        
        public static bool TryGetResourceName(EResourceTypes resourceType, out string resourceName) => m_resourceTypesToNames.TryGetValue(resourceType, out resourceName);

        public static ECardTypes RawCardTypeToEnum(int cardTypeInt)
        {
            if (cardTypeInt is <= 0 or >= (int)ECardTypes.Max)
                return ECardTypes.Attack;

            return (ECardTypes)cardTypeInt;
        }
    }
}
