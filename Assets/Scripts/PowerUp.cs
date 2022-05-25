using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject Powerups;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //FUNCION RANDOMIZER DEL TIPO ESPECIFICO
            GameManager.setupNewWave = true;
            Powerups.SetActive(false);
        }
    }
}
