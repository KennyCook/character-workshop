using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterWorkshop
{
    public class TeslaCannonHitbox : MonoBehaviour
    {
        public GameObject m_Parent;

        private void OnTriggerEnter(Collider other)
        {
            // damage enemy
            // pass reference of gameobject to main script
            m_Parent.GetComponent<TeslaCannon>().AddEnemy(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            // damage enemy
            // pass reference of gameobject to main script
            m_Parent.GetComponent<TeslaCannon>().RemoveEnemy(other.gameObject);
        }
    }
}