using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanqnasQuests.Models
{
    public class Enums
    {
        public enum Occupation
        {
            Unspecified,
            ArenaMaster,
            Armorer,
            Artisan,
            Bandit,
            BannerBearer,
            Blacksmith,
            CaravanGuard,
            GangLeader,
            Gangster,
            GoodsTrader,
            Guard,
            Headman,
            HorseTrader,
            Lord,
            Mercenary,
            Merchant,
            Musician,
            NotAssigned,
            NumberOfOccupations,
            Preacher,
            PrisonGuard,
            RansomBroker,
            RuralNotable,
            ShopWorker,
            Soldier,
            Special,
            TavernGameHost,
            Tavernkeeper,
            TavernWench,
            Townsfolk,
            Villager,
            Wanderer,
            Weaponsmith
        }

        public enum Gender
        {
            Unspecified,
            Male,
            Female
        }

        public enum Faction
        {
            Unspecified,
            SameFaction,
            OtherFaction
        }

        public enum QuestActions
        {
            ACCEPT,
            ABORT
        }
    }
}
