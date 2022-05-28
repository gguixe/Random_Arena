using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject Powerups;
    public GameObject player;
    public enum PowerUps { WeaponRandomizer, BodyRandomizer, BulletRandomizer, SuperRandomizer};
    public PowerUps ThisPowerUp;

    public void Start()
    {
        player = GameObject.Find("player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //FUNCION RANDOMIZER DEL TIPO ESPECIFICO
            if(ThisPowerUp == PowerUps.WeaponRandomizer)
            {
                player.GetComponent<GunHandler>().GunRandomizer();
            }
            else if((ThisPowerUp == PowerUps.BodyRandomizer))
            {
                player.GetComponent<GunHandler>().GraphicRandomizer();
            }
            else if ((ThisPowerUp == PowerUps.BulletRandomizer))
            {
                player.GetComponent<GunHandler>().BulletRandomizer();
            }
            else if ((ThisPowerUp == PowerUps.SuperRandomizer))
            {
                player.GetComponent<GunHandler>().GeneralRandomizer();
            }


            GameManager.setupNewWave = true;
            Powerups.SetActive(false);
        }
    }

    public void ClickButton()
    {
        //FUNCION RANDOMIZER DEL TIPO ESPECIFICO
        if (ThisPowerUp == PowerUps.WeaponRandomizer)
        {
            player.GetComponent<GunHandler>().GunRandomizer();
        }
        else if ((ThisPowerUp == PowerUps.BodyRandomizer))
        {
            player.GetComponent<GunHandler>().GraphicRandomizer();
        }
        else if ((ThisPowerUp == PowerUps.BulletRandomizer))
        {
            player.GetComponent<GunHandler>().BulletRandomizer();
        }
        else if ((ThisPowerUp == PowerUps.SuperRandomizer))
        {
            player.GetComponent<GunHandler>().GeneralRandomizer();
        }


        GameManager.setupNewWave = true;
        Powerups.SetActive(false);      
    }
}


