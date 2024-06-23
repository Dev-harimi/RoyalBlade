using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObjectVFX : PoolingObject
{
    public void Timer(float timer)
    {
        StartCoroutine(TimerCor(timer));
    }

    IEnumerator TimerCor(float timer)
    {
        float t = timer;
        while(t > 0)
        {
            t -= Time.deltaTime;
            yield return GlobalCache.update;
        }

        gameObject.SetActive(false);
    }

    public void FollowTarget(Transform target, Vector3 preset)
    {
        StartCoroutine(FollowCor(target, preset));

        IEnumerator FollowCor(Transform target, Vector3 preset) 
        {
            while(true)
            {
                transform.position = target.transform.position + preset;
                yield return GlobalCache.update;
            }
        }
    }
}
