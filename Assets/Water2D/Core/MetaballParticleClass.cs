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

	private bool isCollision = false;
	private RigidbodyConstraints _cache;
    private void OnCollisionEnter(Collision col)
    {
		if(col.collider.name.Contains("point"))
        {
			if (isCollision)
				return;
			isCollision = true;

			_cache = m_Rig.constraints;

			m_Rig.constraints = m_Rig.constraints | RigidbodyConstraints.FreezePositionZ;
			CoroutineUtils.PlayCoroutine(() =>
			{
				m_Rig.constraints = _cache;
			}, 1f);
		}
    }

    private void OnCollisionExit(Collision col)
    {
  //      if (col.collider.name.Contains("point"))
  //      {
		//	m_Rig.constraints = _cache;
		//}
    }
}
