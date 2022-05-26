using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{

    public static float PHealth = 100;
    public static float maxhealth = 100;
    public float health;
    public TextMeshProUGUI TextHealth;

    public void TakeDamage(float bullet_damage)
    {
        PHealth = PHealth - bullet_damage;
        health = PHealth;
        update_healthGUI(TextHealth);
        if (PHealth <= 0)
        {
            //DEATH
            gameObject.SetActive(false);
        }
    }

    public static void update_healthGUI(TextMeshProUGUI TextHealth)
    {
        TextHealth.text = "<3 " + PHealth + "/" + maxhealth;
    }
}
