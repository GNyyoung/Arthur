using UnityEngine;

namespace DefaultNamespace.Main
{
    public class SwordInfo
    {
        public string Name { get; private set; }
        public int Level { get; private set; }
        
        // name과 level을 통해 json에서 데이터 가져옴.
        public int Damage { get; private set; }
        public float Cooldown { get; private set; }
        public int Durability { get; private set; }
        public int Length { get; private set; }
        public string ActiveSkill { get; private set; }
        public string DrawSkill { get; private set; }

        public string ImageName { get; private set; }
        // 가져오는 변수 끝.

        public void Initialize(string name, int level)
        {
            this.Name = name;
            this.Level = level;
            var swordData = Data.Instance.GetSword(name);
            Debug.Log(swordData.Name);
            Damage = swordData.Damage;
            Cooldown = swordData.Cooldown;
            Durability = swordData.Durability;
            Length = swordData.Length;
            ActiveSkill = swordData.ActiveSkill;
            DrawSkill = swordData.DrawSkill;
        }
    }
}