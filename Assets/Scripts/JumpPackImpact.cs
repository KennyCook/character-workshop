using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterWorkshop
{
    public class JumpPackImpact : MonoBehaviour
    {
        [SerializeField] private GameObject m_Parent;

        void Update()
        {
            GetComponent<SphereCollider>().enabled = false;
        }

        public void EnableJumpImpact()
        {
            GetComponent<SphereCollider>().enabled = true;
            GetComponent<ParticleSystem>().Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                try
                {
                    var forceVector = other.transform.position - m_Parent.transform.position;
                    other.GetComponent<Rigidbody>().AddForceAtPosition(forceVector.normalized * 5f, m_Parent.transform.position, ForceMode.Impulse);
                }
                catch (System.Exception e)
                {
                }
            }
        }
    }
}