using DanqnasQuests.Models;
using DanqnasQuests.Quests;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;
using TaleWorlds.CampaignSystem;

namespace DanqnasQuests
{
    public class DanqnasQuestsSaveableTypeDefiner : SaveableTypeDefiner
    {
        public DanqnasQuestsSaveableTypeDefiner() : base(5634132)
        {
        }

        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(DanqnaQuest), 1);
            AddClassDefinition(typeof(Story), 2);            
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(List<List<string>>));
        }
    }
}