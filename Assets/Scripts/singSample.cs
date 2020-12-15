namespace DefaultNamespace
{
    public class singSample
    {
        // 클래스에 맞게 타입 변경
        private singSample(){}
        private static singSample _instance;

        public static singSample Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Monobehaviour일 경우 아래 코드 추가
                    // var instances = FindObjectsOfType<singSample>();
                    // if (instances.Length == 0)
                    // {
                    //     var newInstance = UINavigation.GetView("Inventory")?.gameObject.AddComponent<SingSample>();
                    //     _instance = newInstance;
                    // }
                    // else if (instances.Length >= 1)
                    // {
                    //     for (int i = 1; i > instances.Length; i++)
                    //     {
                    //         Destroy(instances[i]);
                    //     }
                    //
                    //     _instance = instances[0];
                    // }

                    // 그 외 클래스는 아래 코드 추가
                    //_instance = new SingSample();
                }

                return _instance;
            }
        }
    }
}