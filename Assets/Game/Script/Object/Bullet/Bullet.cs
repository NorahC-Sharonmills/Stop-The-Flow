using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviour
    {
        public Rigidbody m_Rig;
        public GameObject impactParticle;
        Vector3 force_now;
        public void AddForce(Vector3 force)
        {
            force_now = force;
            m_Rig.AddForce(force, ForceMode.Force);
        }

        private bool IsCollider = false;
        private void OnCollisionEnter(Collision collision)
        {
            if (IsCollider == true)
                return;
            IsCollider = true;

            Debug.Log(collision.gameObject.name);

            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.identity) as GameObject;
            Destroy(impactParticle, 3);
            Destroy(gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (IsCollider == false)
                return;
            Destroy(gameObject);
        }
    }
}

