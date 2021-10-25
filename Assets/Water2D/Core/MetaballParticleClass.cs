using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaballParticleClass : MonoBehaviour {


	public GameObject MObject;
	public float LifeTime;
	public bool Active{
		get{ return _active;}
		set{ _active = value;
			if (MObject) {
				MObject.SetActive (value);
			}
		}
	}

	private Rigidbody m_Rig;
	private bool _active;

    void Start()
    {
        MObject = gameObject;
		m_Rig = this.GetComponent<Rigidbody>();
    }

	bool isConnect = false;

    private void OnCollisionEnter(Collision col)
    {
		if(col.collider.name.Contains("point"))
        {
			if (isConnect)
				return;

			isConnect = true;
			Vector3 pos = transform.position;
			if(Mathf.Abs(pos.x - (int)pos.x) < 0.05f)
            {				
				switch(Game.LevelManager.Instance.m_WaterType)
                {
					case Enum.WaterType.Top:
						pos.y = transform.position.y * 4;
						break;
					case Enum.WaterType.Bottom:
						pos.y = 0;
						break;
				}					
				var fx = PoolByID.Instance.GetPrefab(Game.ResourceManager.Instance.m_EffectWaterSmoke, pos, transform.rotation, col.transform);
				PoolByID.Instance.PushToPoolWithTime(fx, 2f);
			}				
			transform.parent = col.transform;
			Destroy(m_Rig);
			gameObject.layer = 8;
        }

		if(!isConnect)
        {
			if (col.collider.name.Contains("Drop3d") || 
				col.collider.name.Contains("Boxx") ||
				col.collider.name.Contains("character"))
				return;
			if (col.collider.name.Contains("Pillar"))
            {
				Vector3 pos = transform.position;
				if (Mathf.Abs(pos.x - col.transform.position.x) < 0.1f)
				{
					pos.y = transform.position.y * 2;
					var fx = PoolByID.Instance.GetPrefab(Game.ResourceManager.Instance.m_EffectWaterSmoke, pos, transform.rotation, null);
					PoolByID.Instance.PushToPoolWithTime(fx, 2f);
				}
				return;
			}
			if (col.collider.name.Contains("PositionFX"))
			{
				col.transform.GetComponent<PositionFX>().SpawnFX();
				return;
			}
			
			Debug.Log(col.collider.name);

		}			
    }

    private void OnCollisionExit(Collision col)
    {
		if (col.collider.name.Contains("point"))
		{

		}
	}
}
