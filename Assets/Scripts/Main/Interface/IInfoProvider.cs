using System.Collections.Generic;

namespace DefaultNamespace
{
    public interface IInfoProvider
    {
        void AddProvider();
        KeyValuePair<string, object>[] GetInfo();
    }
}