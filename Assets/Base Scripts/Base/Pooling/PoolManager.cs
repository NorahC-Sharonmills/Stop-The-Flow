using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
public enum ItemType
{
    FRUIT,
    TRAP
}
public enum PoolName
{
    // prefabs
    NULL = 0,
    FRUIT_PIGGY_GOLD = 1,
    FRUIT_APPLE = 2,
    FRUIT_WATER_MELONS = 3,
    FRUIT_BANANA = 4,
    FRUIT_STRAW_BERRY = 5,
    FRUIT_ORANGES = 6,
    FRUIT_COIN_BOTTLE = 7,
    FRUIT_GRAPES = 8,
    FRUIT_LEMON = 9,
    FRUIT_KIWI = 10,
    FRUIT_COCONUT = 11,
    FRUIT_PINE_APPLE = 12,
    FRUIT_BELL_PEPPER = 13,
    FRUIT_EGG_PLANT = 14,
    FRUIT_RED_PUMPKIN = 15,
    FRUIT_CABBAGE = 16,
    FRUIT_CARROT = 17,
    FRUIT_POTATO = 18,
    TRAP_STUN_CHOPPING_BOARD = 19,
    TRAP_STUN_ANVIL = 20,
    TRAP_STUN_BONE = 21,
    TRAP_DEATH_NEEDLE_TABLE = 22,
    TRAP_DEATH_BOMB = 23,
    TRAP_DEATH_ROTTEN_FRUIT = 24,
    TRAP_DEATH_NEEDLE_SPHERES = 25,
    CHECK_BOX = 26,
    TRAP_DEATH_SHIT = 27,
}

[System.Serializable]
public struct PoolNameAndNamePrefab
{
    public PoolName poolName;
    public string nameRef;

    public PoolNameAndNamePrefab(PoolName _poolName, string _nameRef)
    {
        poolName = _poolName;
        nameRef = _nameRef;
    }
}

public class PoolManager : MonoSingleton<PoolManager>
{
    [Header("Fruit")]
    public PoolNameAndNamePrefab[] fruitPool = null;
    [Header("Others")]
    public PoolNameAndNamePrefab[] otherPool = null;

    private PoolNameAndNamePrefab[] poolNameAndNamePrefab = null;
    public IDictionary<PoolName, Queue<GameObject>> pools;
    public IDictionary<string, GameObject> PrefabOnResources;

    protected override void Awake()
    {
        base.Awake();
        pools = new Dictionary<PoolName, Queue<GameObject>>();
        var fruitPoolNumber = fruitPool.Length;
        var otherPoolNumber = otherPool.Length;
        var poolNumber = fruitPoolNumber + otherPoolNumber;

        poolNameAndNamePrefab = new PoolNameAndNamePrefab[poolNumber];
        for (int i = 0; i < poolNumber; i++)
        {
            if (i < fruitPoolNumber)
            {
                poolNameAndNamePrefab[i] = fruitPool[i];
                continue;
            }
            if (i - fruitPoolNumber < otherPoolNumber)
            {
                poolNameAndNamePrefab[i] = otherPool[i - fruitPoolNumber];
                continue;
            }
        }
    }

    public GameObject PopPool(PoolName poolName, Vector3 pos = new Vector3(), Quaternion rotate = new Quaternion())
    {
        GameObject obj = null;
        if (pools.ContainsKey(poolName) && pools[poolName].Count > 0)
        {
            obj = pools[poolName].Dequeue();
        }

        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = pos;
            obj.transform.rotation = rotate;
        }
        else
        {
            obj = Instantiate(GetPrefabByName(poolName), pos, rotate) as GameObject;
        }

        return obj;
    }

    public GameObject PopPool(PoolName poolName, Transform parent, Vector3 pos = new Vector3(), Quaternion rotate = new Quaternion())
    {
        GameObject obj = null;
        if (pools.ContainsKey(poolName) && pools[poolName].Count > 0)
        {
            obj = pools[poolName].Dequeue();
        }

        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = pos;
            obj.transform.rotation = rotate;
        }
        else
        {
            obj = Instantiate(GetPrefabByName(poolName), pos, rotate, parent) as GameObject;
        }

        return obj;
    }

    public T PopPoolWithComponent<T>(PoolName poolName, Vector3 pos = new Vector3(), Quaternion rotate = new Quaternion())
    {
        GameObject obj = null;
        if (pools.ContainsKey(poolName) && pools[poolName].Count > 0)
        {
            obj = pools[poolName].Dequeue();
        }

        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = pos;
            obj.transform.rotation = rotate;
        }
        else
        {
            obj = Instantiate(GetPrefabByName(poolName), pos, rotate) as GameObject;
        }

        return obj.GetComponent<T>();
    }

    public T PopPoolWithComponent<T>(PoolName poolName, Transform parent, Vector3 pos = new Vector3(), Quaternion rotate = new Quaternion())
    {
        GameObject obj = null;
        if (pools.ContainsKey(poolName) && pools[poolName].Count > 0)
        {
            obj = pools[poolName].Dequeue();
        }

        if (obj != null)
        {
            obj.SetActive(true);
            obj.transform.position = pos;
            obj.transform.rotation = rotate;
        }
        else
        {
            obj = Instantiate(GetPrefabByName(poolName), pos, rotate, parent) as GameObject;
        }

        return obj.GetComponent<T>();
    }

    public void PushPool(GameObject obj, PoolName poolName)
    {
        if (obj == null)
            return;

        if (!pools.ContainsKey(poolName))
            pools.Add(poolName, new Queue<GameObject>());

        if (obj.activeSelf)
        {
            obj.transform.DOKill();
            obj.SetActive(false);
        }
        pools[poolName].Enqueue(obj);
    }

    private GameObject GetPrefabByName(PoolName name)
    {
        string path;
        path = string.Format("Prefabs/{0}", GetStringNameByPoolName(name));
        if (!PrefabOnResources.ContainsKey(path))
            PrefabOnResources.Add(path, Resources.Load<GameObject>(path));

        if (PrefabOnResources[path] == null)
            Debug.Log(name);

        return PrefabOnResources[path];
    }

    private void OnEnable()
    {
        PrefabOnResources = new Dictionary<string, GameObject>();
    }

    public void UnloadAllResource()
    {

    }

    private string GetStringNameByPoolName(PoolName name)
    {
        var _name = "";
        for (int i = 0; i < poolNameAndNamePrefab.Length; i++)
        {
            if (poolNameAndNamePrefab[i].poolName.Equals(name))
            {
                _name = poolNameAndNamePrefab[i].nameRef;
            }
        }
        return _name;
    }

    public void PushToPushAfter(GameObject obj, PoolName poolname, float time)
    {
        CoroutineHandler.StartStaticCoroutine(PushAfter(obj, poolname, time));
    }

    private IEnumerator PushAfter(GameObject obj, PoolName poolname, float time)
    {
        yield return WaitForSecondCache.GetWFSCache(time);

        PushPool(obj, poolname);
    }

    public GameObject SpawnObject(PoolName poolName, Vector3 pos = new Vector3(), Quaternion rotate = new Quaternion())
    {
        return PopPool(poolName, pos, rotate);
    }

    public T SpawnObjectWithComponent<T>(PoolName poolName, Vector3 pos = new Vector3(), Quaternion rotate = new Quaternion())
    {
        return PopPoolWithComponent<T>(poolName, pos, rotate);
    }

    public T SpawnObjectWithComponetOnCustomParent<T>(PoolName poolName, Transform parent, Vector3 pos = new Vector3(), Quaternion rotate = new Quaternion())
    {
        return PopPoolWithComponent<T>(poolName, parent, pos, rotate);
    }

    public void RemoveObject(GameObject obj, PoolName poolName)
    {
        obj.name = "removed";
        PushPool(obj, poolName);
    }

    public void RemoveObjectAfter(GameObject obj, PoolName poolname, float time)
    {
        PushToPushAfter(obj, poolname, time);
    }
}