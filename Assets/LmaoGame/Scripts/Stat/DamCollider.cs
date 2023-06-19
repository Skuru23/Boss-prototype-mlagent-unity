using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamCollider : MonoBehaviour
    {
        Collider dmgCollider;

        public int currentWeapDmg = 25;

        private void Awake()
        {
            dmgCollider = GetComponent<Collider>();
            dmgCollider.gameObject.SetActive(true);
            dmgCollider.isTrigger = true;
            dmgCollider.enabled = false;
        }

        public void EnableDmgCollider()
        {
            dmgCollider.enabled = true;
        }

        public void DisableDmgCollider()
        {
            dmgCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();


                if (playerStats != null)
                {
                    playerStats.TakeDmg(currentWeapDmg);
                }
            }

            if (collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

                if (enemyStats != null)
                {
                    if (enemyStats.canAttacked)
                        enemyStats.TakeDmg(currentWeapDmg);
                }
            }
        }
    }
}