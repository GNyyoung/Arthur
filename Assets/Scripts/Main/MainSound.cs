using UnityEngine;

namespace Main
{
    public class MainSound : MonoBehaviour
    {
        private MainSound(){}
        private static MainSound _instance;

        public static MainSound Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<MainSound>();
                    if (instances.Length == 0)
                    {
                        var newInstance = Camera.main.gameObject.AddComponent<MainSound>();
                        _instance = newInstance;
                        Debug.LogWarning("MainSound가 부착되어있지 않습니다.");
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
        
        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private AudioClip UIOpenSound = null;
        [SerializeField] private AudioClip UICloseSound = null;
        [SerializeField] private AudioClip equipWeaponSound = null;
        [SerializeField] private AudioClip unequipWeaponSound = null;
        [SerializeField] private AudioClip itemClickSound = null;
        [SerializeField] private AudioClip levelUpSound = null;
        [SerializeField] private AudioClip mapOpenSound = null;
        [SerializeField] private AudioClip rewardJingle = null;
        [SerializeField] private AudioClip stageFailJingle = null;

        public void OutputPanelOpenSound()
        {
            audioSource.clip = UIOpenSound;
            audioSource.Play();
        }

        public void OutputPanelCloseSound()
        {
            audioSource.clip = UICloseSound;
            audioSource.Play();
        }

        public void OutputEquipWeaponSound()
        {
            audioSource.clip = equipWeaponSound;
            audioSource.Play();
        }

        public void OutputItemClickSound()
        {
            audioSource.clip = itemClickSound;
            audioSource.Play();
        }

        public void OutPutSwordLevelUp()
        {
            audioSource.clip = levelUpSound;
            audioSource.Play();
        }

        public void OutputMapOpen()
        {
            audioSource.clip = mapOpenSound;
            audioSource.Play();
        }

        public void OutputRewardJingle()
        {
            audioSource.clip = rewardJingle;
            audioSource.Play();
        }

        public void OutPutStageFailJingle()
        {
            audioSource.clip = stageFailJingle;
            audioSource.Play();
        }

        public void OutputUnequipSwordSound()
        {
            audioSource.clip = unequipWeaponSound;
            audioSource.Play();
        }
    }
}