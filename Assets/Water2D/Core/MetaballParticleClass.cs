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
			if(m_Rig.velocity.z <= -3f)
                gameObject.SetActive(false);

			if (isCollision)
				return;
			isCollision = true;

			m_Rig.velocity = Vector3.zero;
		}
    }

    private void OnCollisionStay(Collision col)
    {
		if (col.collider.name.Contains("point"))
		{
			if (m_Rig.velocity.z <= -1.5f)
				gameObject.SetActive(false);
			else
				Debug.Log(m_Rig.velocity.z);
		}
	}

    private void OnCollisionExit(Collision col)
    {
		//if (col.collider.name.Contains("point"))
		//{
  //          if (m_Rig.velocity.z <= -0.79f)
  //              gameObject.SetActive(false);
  //          else
  //              Debug.Log(m_Rig.velocity);
  //      }
	}
}
