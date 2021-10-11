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
			transform.parent = col.transform;
			Destroy(m_Rig);
			gameObject.layer = 8;
        }
    }

    private void OnCollisionExit(Collision col)
    {
		if (col.collider.name.Contains("point"))
		{
			Debug.Log(m_Rig.velocity.z);
		}
	}
}
