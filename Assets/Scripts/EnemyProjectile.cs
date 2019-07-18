using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterWorkshop
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float m_Lifetime;
        [SerializeField] private int m_AttackPower;

        // Start is called before the first frame update
        void Start()
        {
            m_Lifetime = 3;
            m_AttackPower = 50;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Lifetime <= 0)
            {
                Destroy(gameObject);
            }

            transform.Translate(Vector3.forward * Time.deltaTime * 10);
            m_Lifetime -= Time.deltaTime;
        }

        public int GetAttackPower()
        {
            return m_AttackPower;
        }
    }
}