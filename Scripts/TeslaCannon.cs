using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace CharacterWorkshop
{
    public class TeslaCannon : MonoBehaviour
    {
        private class TeslaArcTarget
        {
            public GameObject m_EnemyGO;
            public bool m_Assigned, m_InRange;

            public TeslaArcTarget(GameObject enemyGO)
            {
                m_EnemyGO = enemyGO;
                m_InRange = true;
                m_Assigned = false;
            }
        }

        [SerializeField] private GameObject m_Source, m_Midpoint;

        public List<TeslaArc> m_TeslaArcs;
        private List<TeslaArcTarget> m_Enemies;
        private bool m_IsFiring, m_WasFiring;

        void Start()
        {
            m_Enemies = new List<TeslaArcTarget>();
            m_IsFiring = m_WasFiring = false;
        }

        void Update()
        {
            if (m_IsFiring)
            {
                // loop through enemies that are in range but assigned to a tesla arc and assign them
                foreach (var e in m_Enemies.Where(e => e.m_InRange == true && e.m_Assigned == false))
                {
                    var unassignedArc = m_TeslaArcs.Where(t => t.GetTarget() == null).FirstOrDefault();
                    unassignedArc.SetTarget(m_Source.transform, m_Midpoint.transform, e.m_EnemyGO);
                    unassignedArc.ToggleActive(true);
                    e.m_Assigned = true;
                }
            }
            else
            {
                // loop through enemies and unassign any tesla arcs
                foreach (var e in m_Enemies.Where(e => e.m_Assigned = true))
                {
                    foreach (var assignedArc in m_TeslaArcs.Where(t => t.GetTarget() == e.m_EnemyGO))
                    {
                        assignedArc.RemoveTarget(e.m_EnemyGO);
                        assignedArc.ToggleActive(false);
                    }

                    e.m_Assigned = false;
                }
            }

            m_WasFiring = m_IsFiring;
        }

        public void Fire(bool value)
        {
            m_IsFiring = value;
        }

        public void AddEnemy(GameObject enemy)
        {
            var existingEnemy = m_Enemies.Where(e => e.m_EnemyGO == enemy).FirstOrDefault();
            if (existingEnemy == null)
            {
                m_Enemies.Add(new TeslaArcTarget(enemy));
            }
            else
            {
                existingEnemy.m_InRange = true;
            }
        }

        public void RemoveEnemy(GameObject enemy)
        {
            var existingEnemy = m_Enemies.Where(e => e.m_EnemyGO == enemy).FirstOrDefault();
            if (existingEnemy != null)
            {
                existingEnemy.m_InRange = false;
            }

            // loop through tesla arcs and turn off any that are targetting removed enemy
            var teslaArcsForTarget = m_TeslaArcs.Where(t => t.GetTarget() == enemy);
            if (teslaArcsForTarget != null)
            {
                foreach (var t in teslaArcsForTarget)
                {
                    t.RemoveTarget(enemy);
                    t.ToggleActive(false);
                    existingEnemy.m_Assigned = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                AddEnemy(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            RemoveEnemy(other.gameObject);
        }
    }
}