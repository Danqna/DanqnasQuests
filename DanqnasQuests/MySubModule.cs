using DanqnasQuests.Behaviours;
using DanqnasQuests.Models;
using DanqnasQuests.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace DanqnasQuests
{
	class MySubModule : MBSubModuleBase
	{
		// MCM Menu settings
		public static readonly string ModuleFolderName = "DanqnasQuests";
		public static readonly string ModName = "DanqnasQuests";


		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
		}

		// MCM Settings
		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			MenuConfig.Instance.Settings();
		}


		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			try
			{
				base.OnGameStart(game, gameStarterObject);
				if (!(game.GameType is Campaign))
				{
					return;
				}
				AddBehaviors(gameStarterObject as CampaignGameStarter);
			}
			catch { }
		}

		private void AddBehaviors(CampaignGameStarter gameStarterObject)
		{
			if (gameStarterObject != null)
			{
				gameStarterObject.AddBehavior(new DanqnasStoryteller());
			}
		}
	}
}
