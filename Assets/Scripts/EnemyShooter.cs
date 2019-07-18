using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterWorkshop
{
    public class EnemyShooter : Enemy
    {
        public GameObject m_Projectile;
        [SerializeField] private float m_FireCooldown;

        // Start is called before the first frame update
        void Start()
        {
            m_FireCooldown = 0;
        }

        // Update is called once per frame
        void Update()
        {
            m_FireCooldown -= Time.deltaTime;

            if (m_FireCooldown <= 0)
            {
                Instantiate(m_Projectile, transform.position, transform.rotation);
                m_FireCooldown = 0.5f;
            }
        }
    }
}