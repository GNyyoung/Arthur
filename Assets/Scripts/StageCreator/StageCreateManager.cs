using System.Collections.Generic;

namespace StageCreator
{
    public class StageCreateManager
    {
        private StageCreateManager(){}
        private static StageCreateManager _instance;

        public static StageCreateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    
                    _instance = new StageCreateManager();
                }

                return _instance;
            }
        }
        
        public List<MonsterCreateInfo> monsterList = new List<MonsterCreateInfo>();
    }
}