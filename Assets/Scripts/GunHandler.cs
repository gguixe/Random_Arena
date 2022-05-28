﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Help https://www.youtube.com/watch?v=wZ2UUOC17AY

public class GunHandler : MonoBehaviour
{
    //Rotate Gun
    public GameObject pivot;
    public float offset = 0;

    //Bullet
    public GameObject bullet;
    public float shootForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    public bool allowInvoke = true;

    //Graphics
    public GameObject muzzleflash;
    public TextMeshProUGUI ammunitionDisplay;
    public Sprite[] spriteList;

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        MyInput();
        AmmoDisplay();
    }

    void AmmoDisplay()
    {
        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
    }

    void MyInput()
    {
        //Check if allowed to hold down button and take correct input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Automatic reload when out of ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if (readyToShoot)
            //Shooting
            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                //Set bullets shot to 0
                bulletsShot = 0;
                Shoot();
            }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            muzzleflash.SetActive(false);
        }

        if (reloading == false) //reload sound
        {
            FindObjectOfType<SoundManager>().Stop("reload"); //Audio/Sound
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        FindObjectOfType<SoundManager>().Play("shooting"); //Audio/Sound

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //Store instance of bullet
        //Rotate bullet to shoot direction
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivot.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        currentBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 90f);
        //currentBullet.transform.right = difference;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody2D>().AddForce(-attackPoint.transform.right * shootForce, ForceMode2D.Impulse);

        //Muzzle flash
        //if(muzzleflash != null)
        //{
        //    GameObject newObject = Instantiate(muzzleflash, attackPoint.position, Quaternion.identity);
        //    newObject.transform.localScale = new Vector3(0.25f, 0.25f, 1f); // change its local scale in x y z format
        //
        //}
        muzzleflash.SetActive(true);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //if more than one bulletPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        FindObjectOfType<SoundManager>().Play("reload"); //Audio/Sound
        muzzleflash.SetActive(false);
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    void RotateGun()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivot.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + offset - 180f);
    }

    public void GeneralRandomizer()
    {
        GraphicRandomizer();
        BulletRandomizer();
        GunRandomizer();
    }

    public void GraphicRandomizer()
    {
        //Sprite Renderer & transform
        float RanScale = Random.Range(0.5f, 5f); //Random Scale
        gameObject.transform.localScale = new Vector3(RanScale, RanScale, 1);

        //int RanSpriteRenderer = Random.Range(0, spriteList.Length); //Random Sprite
        //SpriteRenderer EnemySprite = gameObject.GetComponent<SpriteRenderer>();
        //EnemySprite.sprite = spriteList[RanSpriteRenderer];

        //Adapt Box collider to size of sprite //That's not random
        //Vector2 S = EnemySprite.sprite.bounds.size;
        //gameObject.GetComponent<BoxCollider2D>().size = S;
        //gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((S.x / 2), 0);

        //Add speed, etc...here !!!!
    }

    public void BulletRandomizer()
    {
        //Bullet Stats
        int RanExplosionDamage = Random.Range(5, 20); //Random Explosion Damage
        bullet.GetComponent<CustomBullet>().explosionDamage = RanExplosionDamage; //Current 10
        float RanExplosionRange = Random.Range(0.1f, 5f); //Random Explosion Range
        bullet.GetComponent<CustomBullet>().explosionRange = RanExplosionRange; //Current 0.1
        int RanNumCollisions = Random.Range(1, 10); //Random Explosion Range
        bullet.GetComponent<CustomBullet>().maxCollisions = RanNumCollisions; //Current 3 (not used)
        int RanMaxLifeTime = Random.Range(1, 50); //Random Explosion Range
        bullet.GetComponent<CustomBullet>().maxLifeTime = RanMaxLifeTime; //Current 10
        int RanExplodeOnTouch = Random.Range(1, 100); //Random Explosion Range
        bool RanExplodeOnTouchBool;
        if (RanExplodeOnTouch > 90) RanExplodeOnTouchBool = true; else RanExplodeOnTouchBool = false; //90% Explode on Touch
        bullet.GetComponent<CustomBullet>().explodeOnTouch = RanExplodeOnTouchBool; //Current true
        int RanshootForce = Random.Range(2, 10); //Random Shoot force
        shootForce = RanshootForce;//Current 3 
    }

    public void GunRandomizer()
    {
        //Gun stats
        float RantimeBetweenShooting = Random.Range(0.25f, 1.5f);
        timeBetweenShooting = RantimeBetweenShooting; //Current 0.5
        float RanreloadTime = Random.Range(0.5f, 3f);
        reloadTime = RanreloadTime; //Current 1.5
        float RantimeBetweenShots = Random.Range(0.5f, 5);
        timeBetweenShots = RantimeBetweenShots; //Current 2
        int RanmagazineSize = Random.Range(10, 100);
        magazineSize = RanmagazineSize; //Current 50
        int RanbulletsPerTap = Random.Range(1, 3);
        bulletsPerTap = RanbulletsPerTap; //Current 1
    }

    public void DefaultVariables()
    {
        //Body Stats
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        //Bullet Stats
        bullet.GetComponent<CustomBullet>().explosionDamage = 25; //Current 10
        bullet.GetComponent<CustomBullet>().explosionRange = 0.5f; //Current 0.1
        bullet.GetComponent<CustomBullet>().maxCollisions = 3; //Current 3 (not used)
        bullet.GetComponent<CustomBullet>().maxLifeTime = 10; //Current 10
        bullet.GetComponent<CustomBullet>().explodeOnTouch = true; //Current true
        shootForce = 5;//Current 3 
        //Gun stats
        timeBetweenShooting = 0.1f; //Current 0.5
        reloadTime = 1.5f; //Current 1.5
        timeBetweenShots = 0.3f; //Current 2
        magazineSize = 100; //Current 50
        bulletsPerTap = 1; //Current 1
    }
}
