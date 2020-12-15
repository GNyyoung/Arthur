using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class FireBall : PlayerSkill
    {
        private GameObject toolObject;
        protected override void SetSkillAnimation()
        {
            skillAnim = Resources.Load<AnimationClip>("Animations/Player/FireBall");
            toolObject = player.transform.Find("PlayerSprite").GetComponent<CharacterModel>().toolObject;
        }

        public override IEnumerator Skill()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            bool isActive = false;
            var fireBallPrefab = Resources.Load<GameObject>("Prefabs/FireBall");
            
            ApplySkillAnimation();

            ActiveProgress = 0;
            while (ActiveProgress < skillAnim.length)
            {
                if (isActive == false &&
                    ActiveProgress > skillAnim.length * 0.25f)
                {
                    var fireBallObj = Instantiate(fireBallPrefab, toolObject.transform.position, Quaternion.identity);
                    fireBallObj.GetComponent<FireBallController>().Initialize(player, this);
                    
                    isActive = true;
                }
                
                ActiveProgress += Time.fixedDeltaTime;
                yield return waitForFixedUpdate;
            }
            EndSkill();
        }
    }
}