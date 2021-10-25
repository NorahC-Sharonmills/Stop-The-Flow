﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolByID : MonoSingleton<PoolByID>
{
    private IDictionary<int, List<GameObject>> pools;

    protected override void Awake()
    {
        base.Awake();
        pools = new Dictionary<int, List<GameObject>>();
    }

    public GameObject GetPrefab(GameObject obj)
    {
        var id = obj.GetInstanceID();
        if (pools.ContainsKey(id))
        {
            for (int i = 0; i < pools[id].Count; i++)
            {
                if (!pools[id][i].activeSelf)
                {
                    pools[id][i].SetActive(true);

                    return pools[id][i];
                }
            }
        }
        else
        {
            pools.Add(id, new List<GameObject>());
        }

        var _obj = Instantiate(obj) as GameObject;
        pools[id].Add(_obj);
        return _obj;
    }

    public GameObject GetPrefab(GameObject obj, Vector3 pos, Quaternion quaternion, Transform parent)
    {
        var id = obj.GetInstanceID();
        if (pools.ContainsKey(id))
        {
            //Find obj from pool
            for (int i = 0; i < pools[id].Count; i++)
            {
                if (!pools[id][i].activeSelf)
                {
                    pools[id][i].transform.position = pos;
                    pools[id][i].transform.rotation = quaternion;
                    pools[id][i].transform.parent = parent;
                    pools[id][i].SetActive(true);

                    return pools[id][i];
                }
                else
                {
                    goto a1;
                }
            }
        }
        else
        {
            pools.Add(id, new List<GameObject>());
        }
        a1:
        var _obj = Instantiate(obj, pos, quaternion, parent) as GameObject;
        pools[id].Add(_obj);
        return _obj;
    }

    public T GetPrefab<T>(GameObject obj, Vector3 pos, Quaternion quaternion, Transform parent)
    {
        var _obj = GetPrefab(obj, pos, quaternion, parent);
        return _obj.GetComponent<T>();
    }

    public T GetPrefab<T>(GameObject obj)
    {
        var _obj = GetPrefab(obj);
        return _obj.GetComponent<T>();
    }

    public void PushToPool(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }    
    }

    public void PushToPoolWithTime(GameObject obj, float time)
    {
        if (obj != null)
        {
            CoroutineUtils.PlayCoroutine(() =>
            {
                if(obj != null)
                {
                    obj.SetActive(false);
                }             
            }, time);
        }
    }
}
