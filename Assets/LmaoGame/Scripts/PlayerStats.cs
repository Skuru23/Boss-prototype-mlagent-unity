using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
    public class PlayerStats : MonoBehaviour
    {
        public int health = 10;
        public int maxHealth;
        public int currentHealth;
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
        }
    }
}