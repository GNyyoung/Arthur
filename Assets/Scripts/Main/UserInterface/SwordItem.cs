using DefaultNamespace.Main;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class SwordItem : MonoBehaviour
    {
        public SwordInfo SwordInfo { get; set; }
        public int ItemIndex { get; set; }
        public Image itemImage;

        public void SetData(SwordInfo swordInfo, int itemIndex)
        {
            this.SwordInfo = swordInfo;
            this.ItemIndex = itemIndex;
        }
    }
}