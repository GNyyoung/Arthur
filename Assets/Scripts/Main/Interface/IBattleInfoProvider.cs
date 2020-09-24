using System.Collections.Generic;

namespace DefaultNamespace
{
    public interface IBattleInfoProvider
    {
        KeyValuePair<string, object> GetBattleInfo();
    }
}