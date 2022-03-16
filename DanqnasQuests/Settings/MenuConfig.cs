using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Base.Global;
using System;

namespace DanqnasQuests.Settings
{
    class MenuConfig : IDisposable
    {
        private static MenuConfig _instance;

        private FluentGlobalSettings globalSettings;

        public bool DebuggingEnabled;

        public bool FirstRunDone;

        public bool DisableNegativeDisposition;

        public bool DisableDeliveryQuests;

        public bool DisableMurderQuests;

        public bool DisableMessengerQuests;

        public bool OverrideGoldReward;
        
        public int SetGoldReward;

        public bool OverrideRelationReward;

        public int SetRelationReward;

        public bool OverrideTime;

        public int SetTime;

        public bool DisableTestosteroneChecks;

        public int ModifiableHeroDeathChance;

        public bool DisableFactionChecks;

        public bool DisableRelationChecks;

        public static MenuConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MenuConfig();
                }
                return _instance;
            }
        }

        public void Settings()
        {
            // Can't have nulls            

            var builder = BaseSettingsBuilder.Create("DanqnasQuests", "Danqnas Quest Options")!
                .SetFormat("xml")
                .SetFolderName(MySubModule.ModuleFolderName)
                .SetSubFolder(MySubModule.ModName)
                .CreateGroup("1. GENERAL SETTINGS", groupBuilder => groupBuilder
                    .AddBool("DebuggingEnabled", "Enable Debug Messages", new ProxyRef<bool>(() => DebuggingEnabled, o => DebuggingEnabled = o), boolBuilder => boolBuilder
                        .SetHintText("Enable debug messages")
                        .SetRequireRestart(false)))
                .CreateGroup("2. QUEST CONFIGURATION", groupBuilder => groupBuilder
                    .AddBool("DisableNegativeDisposition", "Turn off negative disposition quests", new ProxyRef<bool>(() => DisableNegativeDisposition, o => DisableNegativeDisposition = o), boolBuilder => boolBuilder
                        .SetHintText("Disables quests where you are mean to the hero you're speaking with")
                        .SetRequireRestart(false))
                    .AddBool("DisableTestosteroneChecks", "Turn off testosterone checking", new ProxyRef<bool>(() => DisableTestosteroneChecks, o => DisableTestosteroneChecks = o), boolBuilder => boolBuilder
                        .SetHintText("Enables all quests for every hero regardless of testosterone, may result in weird pronouns")
                        .SetRequireRestart(false))
                    .AddBool("DisableFactionChecks", "Turn off faction checking", new ProxyRef<bool>(() => DisableFactionChecks, o => DisableFactionChecks = o), boolBuilder => boolBuilder
                        .SetHintText("Enables all quests regardless of faction")
                        .SetRequireRestart(false))
                    .AddBool("DisableRelationChecks", "Turn off relation checking", new ProxyRef<bool>(() => DisableRelationChecks, o => DisableRelationChecks = o), boolBuilder => boolBuilder
                        .SetHintText("Enables all quests regardless of relation to the player")
                        .SetRequireRestart(false))
                    .AddBool("DisableDeliveryQuests", "Turn off delivery quests", new ProxyRef<bool>(() => DisableDeliveryQuests, o => DisableDeliveryQuests = o), boolBuilder => boolBuilder
                        .SetHintText("Disables all quests marked as delivery")
                        .SetRequireRestart(false))
                    .AddBool("DisableMurderQuests", "Turn off murder quests", new ProxyRef<bool>(() => DisableMurderQuests, o => DisableMurderQuests = o), boolBuilder => boolBuilder
                        .SetHintText("Disables all quests marked as delivery")
                        .SetRequireRestart(false))
                    .AddBool("DisableMessengerQuests", "Turn off messenger quests", new ProxyRef<bool>(() => DisableMessengerQuests, o => DisableMessengerQuests = o), boolBuilder => boolBuilder
                        .SetHintText("Disables all quests marked as delivery")
                        .SetRequireRestart(false)))
                .CreateGroup("3. OVERRIDES", groupBuilder => groupBuilder
                    .AddInteger("ModifiableHeroDeathChance", "Set Hero death chance", 1, 200, new ProxyRef<int>(() => ModifiableHeroDeathChance, o => ModifiableHeroDeathChance = o), integerBuilder => integerBuilder
                        .SetHintText("For murder quests, adjusts how likely the lord is to die via age or battle"))
                    .AddBool("OverrideGoldReward", "Override Gold Reward", new ProxyRef<bool>(() => OverrideGoldReward, o => OverrideGoldReward = o), boolBuilder => boolBuilder
                        .SetHintText("Set your own gold reward and ignore quests values")
                        .SetRequireRestart(false))
                    .AddInteger("SetGoldReward", "Override Gold Reward Value", 1, 10000, new ProxyRef<int>(() => SetGoldReward, o => SetGoldReward = o), integerBuilder => integerBuilder
                        .SetHintText("Cost values"))
                    .AddBool("OverrideRelationReward", "Override Relation Reward", new ProxyRef<bool>(() => OverrideRelationReward, o => OverrideRelationReward = o), boolBuilder => boolBuilder
                        .SetHintText("Set your own Relation reward and ignore quests values")
                        .SetRequireRestart(false))
                    .AddInteger("SetRelationReward", "Override Relation Reward Value", 0, 10, new ProxyRef<int>(() => SetRelationReward, o => SetRelationReward = o), integerBuilder => integerBuilder
                        .SetHintText("Override the relation reward"))
                    .AddBool("OverrideTime", "Override Quest Time", new ProxyRef<bool>(() => OverrideTime, o => OverrideTime = o), boolBuilder => boolBuilder
                        .SetHintText("Set your own time requirement on quests and ignore quests values")
                        .SetRequireRestart(false))
                    .AddInteger("SetTime", "Override Timed Quests", 0, 10, new ProxyRef<int>(() => SetTime, o => SetTime = o), integerBuilder => integerBuilder
                        .SetHintText("Override the time values"))
                    )
                .CreateGroup("9. SYSTEM SETTINGS", groupBuilder => groupBuilder
                    .AddBool("FirstRunDone", "Untick to reset to defaults", new ProxyRef<bool>(() => FirstRunDone, o => FirstRunDone = o), boolBuilder => boolBuilder
                        .SetHintText("Reset all settings to initial state")
                        .SetRequireRestart(false)));

            globalSettings = builder.BuildAsGlobal();
            globalSettings.Register();

            if (!FirstRunDone)
            {
                Perform_First_Time_Setup();
            }
        }

        private void Perform_First_Time_Setup()
        {
            DebuggingEnabled = false;

            DisableNegativeDisposition = false;

            DisableDeliveryQuests = false;

            DisableMurderQuests = false;

            DisableMessengerQuests = false;

            OverrideGoldReward = false;

            SetGoldReward = 200;

            OverrideRelationReward = false;

            SetRelationReward = 1;

            OverrideTime = false;

            SetTime = 30;

            FirstRunDone = true;
        }

        public void Dispose()
        {
            //MenuConfig.Unregister();
        }
    }
}