using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanqnasQuests.Models
{
	class CurrentStory : IDisposable
	{
		private static CurrentStory _instance;

		public static CurrentStory Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new CurrentStory();
				}
				return _instance;
			}
		}

		public List<Story> CurrentStories { get; set; }
		
		public void Dispose()
        {
            
        }
    }
}
