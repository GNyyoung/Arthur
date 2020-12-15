using UnityEngine;

namespace DefaultNamespace
{
    public static class TextGetter
    {
        public enum TextType
        {
            SkillName,
            SkillDesc,
            SwordName,
            SwordDesc
        }

        public static string GetText(string search, TextType textType)
        {
            switch (textType)
            {
                case TextType.SwordName:
                    return Data.Instance.GetText($"Sword_{search}_Name");
                case TextType.SwordDesc:
                    return Data.Instance.GetText($"Sword_{search}_Desc");
                case TextType.SkillName:
                    return Data.Instance.GetText($"Skill_{search}_Name");
                case TextType.SkillDesc:
                    return Data.Instance.GetText($"Skill_{search}_Desc");
            }

            return null;
        }
    }
}