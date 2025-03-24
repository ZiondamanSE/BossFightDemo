using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHelth : MonoBehaviour
{
    public Slider helthSlider;

    public float maxHelth = 100;
    private float Healing; 
    private float currentHelth;

    void Start()
    {
        currentHelth = maxHelth;

        helthSlider.maxValue = maxHelth;
        helthSlider.value = currentHelth;
        helthSlider.minValue = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DamageOBJ") // Needs fether work (Add a tag to the object that will deal damage to the player)
            HelthSystem(10, 0);

        if (collision.gameObject.tag == "HealingObject")
        {
            HelthSystem(0, 10);
            collision.gameObject.SetActive(false);
        }

    }

    public void HelthSystem(float damage, float healing) // This function is called from the Enemy script
    {
        if (healing > 0) // if the player gets healed
        {
            currentHelth += healing;
            helthSlider.value = currentHelth;
            Debug.Log("Player got healed and now ther Curent Helth is : " + currentHelth);
            return; // gets called ones
        }
        if (damage > 0) // if the player takes damage
        {
            currentHelth -= damage;

            Debug.Log("Player took damige and now ther Curent Helth is : " + currentHelth);

            if (currentHelth <= 0)
                Death();
            else
            {
                helthSlider.value = currentHelth;
                return; // gets called ones
            }
        }
    }

    void Death() // Player is dead you fuked up
    {
        Debug.Log("Player is dead");
        Time.timeScale = 0;
    }

}
