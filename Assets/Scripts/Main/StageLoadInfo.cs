using DefaultNamespace;
using UnityEngine;

namespace Main
{
    public struct StageLoadData
    {
        public string stageName;
        public GameManager.GameMode gameMode;

        public StageLoadData(string stageName, GameManager.GameMode gameMode)
        {
            this.stageName = stageName;
            this.gameMode = gameMode;
        }
    }
    public class StageLoadInfo : MonoBehaviour
    {
        public string stageName;
        public GameManager.GameMode gameMode;

        public void StartStage()
        {
            SelectStagePanelUI.Instance.StartStage(new StageLoadData(stageName, gameMode));
        }
    }
}