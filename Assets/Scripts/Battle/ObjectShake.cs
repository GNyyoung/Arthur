using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    private Coroutine shakeCoroutine;
    private Vector3 originalPosition;
    
    public float shakeDegree;
    // 위,아래 공격 시 카메라가 흔들리는 각도
    public int shakeAngle;
    public float ShakeTime;
    // 카메라 흔들린 후 다른 방향으로 흔들릴 때 얼마나 움직이는지.
    public float shakeElastic;
    /// <summary>
    /// 왼쪽으로 흔들릴 경우 -1, 오른쪽일 경우 1
    /// </summary>
    public int shakeDirection = -1;

    private void Start()
    {
        
    }

    public void ShakeOnDamage(AttackDirection attackDirection)
    {
        Vector3 maxShakeVector = Vector3.zero;
        switch (attackDirection)
        {
            case AttackDirection.Slash:
                maxShakeVector = new Vector3(
                    shakeDirection * shakeDegree * Mathf.Cos(Mathf.Deg2Rad * shakeAngle), 
                    shakeDirection * shakeDegree * Mathf.Cos(Mathf.Deg2Rad * (90 - shakeAngle)));
                break;
            case AttackDirection.UpperSlash:
                maxShakeVector = new Vector3(
                    shakeDirection * shakeDegree * Mathf.Cos(Mathf.Deg2Rad * shakeAngle), 
                    -shakeDirection * shakeDegree * Mathf.Cos(Mathf.Deg2Rad * (90 - shakeAngle)));
                break;
            case AttackDirection.Stab:
                maxShakeVector = new Vector3(-1 * shakeDegree, 0, 0);
                break;
        }
        
        shakeCoroutine = StartCoroutine(StartShakeOnDamage(maxShakeVector));
    }
    
    // 이미 동작 중이라면 계속 돌아가게 할까, 새로운 방향으로 흔들리게 할까?
    private IEnumerator StartShakeOnDamage(Vector3 shakeVector)
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        int shakeCount = 0;
        float shakeTimeOnce;

        float num = 1;
        while (num > 0.2f)
        {
            shakeCount += 1;
            num *= shakeElastic;
        }

        Debug.Log($"ShakeCount : {shakeCount}");
        shakeTimeOnce = ShakeTime / shakeCount;
        Debug.Log($"ShakeTimeOnce : {shakeTimeOnce}");

        originalPosition = transform.position;
        float interpolateIncrease = Time.fixedDeltaTime / shakeTimeOnce;
        var previousShakeVector = Vector3.zero;
        while (shakeCount >= 0)
        {
            float interpolate = interpolateIncrease;
            Vector3 dist;
            if (shakeCount == 0)
            {
                dist = -previousShakeVector * interpolateIncrease;
            }
            else
            {
                dist = (shakeVector - previousShakeVector) * interpolateIncrease;    
            }
            
            while (interpolate < 1)
            {
                transform.position += dist; 
                interpolate += interpolateIncrease;
                yield return waitForFixedUpdate;
            }
            
            Debug.Log(previousShakeVector + shakeVector);
            previousShakeVector = shakeVector;
            shakeVector *= -shakeElastic;
            shakeCount -= 1;
        }
    }
}
