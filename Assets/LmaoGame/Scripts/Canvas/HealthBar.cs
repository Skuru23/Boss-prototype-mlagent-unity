using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        void Awake(){
            slider = GetComponent<Slider>();
        }
        
        public void SetMaxHealth(int maxHealth){
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetCurrentValue(int health){
            slider.value = health;
        }

        public void Disable(){
            slider.enabled = false;
        }
    }
}
