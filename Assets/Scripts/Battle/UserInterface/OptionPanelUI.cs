using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class OptionPanelUI : MonoBehaviour, IPanelUI, IInstanceReceiver
    {
        private OptionPanelUI(){}
        private static OptionPanelUI _instance;

        public static OptionPanelUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<OptionPanelUI>();
                    if (instances.Length == 0)
                    {
                        var newInstance = UINavigation.GetView("Option")?.gameObject.AddComponent<OptionPanelUI>();
                        _instance = newInstance;
                    }
                    else if (instances.Length >= 1)
                    {
                        for (int i = 1; i > instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }

                        _instance = instances[0];
                    }
                }

                return _instance;
            }
        }
        
        public void ShowPanelData()
        {
            Time.timeScale = 0;
        }

        public void SetInstance(object obj)
        {
            
        }

        public void CloseOption()
        {
            UINavigation.PopTo("BattleOption");
            Time.timeScale = 1;
        }

        public void OnClickRestart()
        {
            BattleSceneManager.Instance.ResetData();
            SceneManager.LoadScene("Battle");
            Time.timeScale = 1;
        }

        public void ExitScene()
        {
            BattleSceneManager.Instance.ResetData();
            // 테스트 코드(GameOver에 들어가야 함.)
            BattleSceneManager.Instance.InputInformation();
            // 코드 끝
            SceneManager.LoadScene("Main");
            Time.timeScale = 1;
        }
    }
}