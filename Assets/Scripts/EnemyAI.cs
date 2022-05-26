using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    //Player
    public GameObject player;
    //Rotate Gun
    public GameObject pivot;
    //Distance
    public float attackDistance = 2;

    //GUN/SHOOTING VARIABLES
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
    //Graphics
    public GameObject muzzleflash;
    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public bool allowInvoke = true;

    //Health
    public float health = 50;

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyRandomizer();

        string chicken_audio = "chicken" + (Random.Range(0, 7)); //chicken audio to play random
        FindObjectOfType<SoundManager>().Play(chicken_audio); //Sound

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //Automatically assign player from Tag
        GameObject[] parray = GameObject.FindGameObjectsWithTag("Player");
        player = parray[0];
        //Automatically assign pivot from children
        pivot = transform.Find("Enemy_gunPivot").gameObject;
        //attackPoint = pivot.transform;
        //target
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        NavMeshMovement();
        MyInput();
    }

    void RotateGun()
    {
        int error = Random.Range(-10, 10);
        Vector3 difference = player.transform.position - pivot.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z - 180f);
    }

    void NavMeshMovement()
    {
        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);

        if (distance > attackDistance) agent.SetDestination(target.position);
        else agent.SetDestination(transform.position);

    }

    void MyInput()
    {

        //TEMPORAL
        shooting = true;

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

        if (readyToShoot)
        {
            //muzzleflash.SetActive(false);
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        FindObjectOfType<SoundManager>().Play("shooting_chicken"); //Sound
        //muzzleflash.SetActive(true);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //Store instance of bullet
        //Rotate bullet to shoot direction
        //Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivot.transform.position;
        //difference.Normalize();
        //float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //currentBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 90f);
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
        muzzleflash.SetActive(false);
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public void TakeDamage(float bullet_damage)
    {
        health = health - bullet_damage;
        if (health <= 0)
        {
            //DEATH
            GameManager.killedEnemies = GameManager.killedEnemies + 1;
            Destroy(gameObject,0);
        }
    }

    public void EnemyRandomizer()
    {
        //Sprite Renderer & transform
        //gameObject.transform.localScale = XXXX
        SpriteRenderer EnemySprite = gameObject.GetComponent<SpriteRenderer>();
        //EnemySprite.sprite = Sprite HERE

        //Adapt Box collider to size of sprite
        Vector2 S = EnemySprite.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((S.x / 2), 0);
        //Bullet
        bullet.GetComponent<CustomBullet>().explosionDamage = 10; //Current 10
        bullet.GetComponent<CustomBullet>().explosionRange = 0.1f; //Current 0.1
        bullet.GetComponent<CustomBullet>().maxCollisions = 3; //Current 3 (not used)
        bullet.GetComponent<CustomBullet>().maxLifeTime = 2; //Current 10
        bullet.GetComponent<CustomBullet>().explodeOnTouch = true; //Current true
        shootForce = 3;//Current 3
        //Gun stats
        timeBetweenShooting = 0.5f; //Current 0.5
        reloadTime = 1.5f; //Current 1.5
        timeBetweenShots = 2f; //Current 2
        magazineSize = 50; //Current 50
        bulletsPerTap = 1; //Current 1
    }
}
