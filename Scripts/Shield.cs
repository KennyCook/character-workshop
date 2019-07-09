using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterWorkshop
{
    public class Shield : MonoBehaviour
    {
        public GameObject m_Parent;
        public Collider m_Collider;

        [SerializeField] private float m_MaxHealth;
        [SerializeField] private float m_CurrentHealth;

        // Start is called before the first frame update
        void Start()
        {
            m_MaxHealth = m_CurrentHealth = 900;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_CurrentHealth <= 0)
            {
                Destroy(m_Parent);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy Attack"))
            {
                //m_Health -= other.gameObject.GetComponent<EnemyProjectile>().GetAttackPower();

                m_CurrentHealth -= 50;
                GameController.UpdateShieldBar(m_CurrentHealth / m_MaxHealth);
                print(m_CurrentHealth / m_MaxHealth);

                Destroy(other.gameObject);
            }
        }
    }
}