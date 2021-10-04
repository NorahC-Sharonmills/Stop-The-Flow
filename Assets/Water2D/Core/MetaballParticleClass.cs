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

    private void OnCollisionEnter(Collision col)
    {
		if(col.collider.name.Contains("point"))
        {
			m_Rig.velocity = Vector3.zero;

		}			
    }

    private void OnCollisionExit(Collision col)
    {
		if (col.collider.name.Contains("point"))
		{
			m_Rig.velocity = Vector3.zero;

		}
	}
}
