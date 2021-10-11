using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviour
    {
        public Rigidbody m_Rig;
        public GameObject impactParticle;
        //Vector3 force_now;
        Vector3 moveDir;
        float speed = 10;
        public void AddForce(Vector3 force)
        {
            moveDir = force;
            //force_now = force;
            //m_Rig.AddForce(force, ForceMode.Force);
        }

        private void Update()
        {
            if (IsCollider)
                return;
            transform.position += moveDir * speed * Time.deltaTime;
        }

        private bool IsCollider = false;
        private void OnCollisionEnter(Collision collision)
        {
            if (IsCollider == true)
                return;
            IsCollider = true;

            Debug.Log(collision.gameObject.name);
            if(collision.gameObject.name == "center")
            {
                var rig = collision.gameObject.GetComponent<Rigidbody>();
                rig.AddForce(transform.forward * 500f);
            }    

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

