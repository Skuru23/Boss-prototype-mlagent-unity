using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EnemyStats : MonoBehaviour
    {
        public int health = 10;
        public int maxHealth;
        public int currentHealth;

        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            maxHealth = SetMaxHelthFromHealthLevel();
            currentHealth = maxHealth;
        }

        public int SetMaxHelthFromHealthLevel()
        {
            maxHealth = health * 10;
            return maxHealth;
        }

        public void TakeDmg(int Dmg)
        {
            currentHealth -= Dmg;

            animator.Play("Dmg_01");

            if(currentHealth <=0)
            {
                currentHealth = 0;
                animator.Play("Death");
                
            }
        }
    }
}