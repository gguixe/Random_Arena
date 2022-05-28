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
    public Sprite[] spriteList;
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

        int RanSpriteRenderer = Random.Range(0, spriteList.Length); //Random Sprite
        SpriteRenderer EnemySprite = gameObject.GetComponent<SpriteRenderer>();
        EnemySprite.sprite = spriteList[RanSpriteRenderer];

        //Adapt Box collider to size of sprite //That's not random
        Vector2 S = EnemySprite.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((S.x / 2), 0);

        //Health
        int RanHealth = Random.Range(10, 100); //Random Sprite
        health = RanHealth;

        //Speed
        float RanSpeed = Random.Range(1, 2); //Random Sprite
        gameObject.GetComponent<NavMeshAgent>().speed = RanSpeed;

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
        int RanshootForce = Random.Range(2, 8); //Random Shoot force
        shootForce = RanshootForce;//Current 3 No Use
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
        SpriteRenderer EnemySprite = gameObject.GetComponent<SpriteRenderer>();
        EnemySprite.sprite = spriteList[0];
        health = 50;
        gameObject.GetComponent<NavMeshAgent>().speed = 1;
        Vector2 S = EnemySprite.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((S.x / 2), 0);
        //Bullet Stats
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
