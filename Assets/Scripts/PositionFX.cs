using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFX : MonoBehaviour
{
    private float time = 2f;
    private float timer = 2f;

    bool IsSpawn = false;

    void Update()
    {
        if(IsSpawn)
        {
            time += Time.deltaTime;
            if (time > timer)
            {
                time = 0;
                IsSpawn = false;
            }
        }    
    }

    public void SpawnFX()
    {
        if(!IsSpawn)
        {
            IsSpawn = true;
            Vector3 pos = transform.position;
            pos.y = 1;
            var fx = PoolByID.Instance.GetPrefab(Game.ResourceManager.Instance.m_EffectWaterSmoke, pos, transform.rotation, null);
            fx.transform.eulerAngles = Vector3.right * 90;
            PoolByID.Instance.PushToPoolWithTime(fx, 2f);
        }    
    }    
}
