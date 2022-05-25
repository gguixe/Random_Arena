using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    public bool isPlayerbullet = false;
    //Assignables
    public Rigidbody2D rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    //Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity; //Only for 3D

    //Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    //Lifetime
    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicsMaterial2D physics_mat;

    private void Setup()
    {
        //Create Physic material
        physics_mat = new PhysicsMaterial2D();
        physics_mat.bounciness = bounciness;
        //Set gravity
        //rb.useGravity = useGravity;
    }

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //When to explode:
        if (collisions > maxCollisions) Explode();

        //Countdown lifetime
        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0) Explode();
    }

    private void Explode()
    {
        //Debug.Log("BOOM");
        //Explosion
        if (explosion != null)
        {
            GameObject newObject = Instantiate(explosion, transform.position, Quaternion.identity);
            newObject.transform.localScale = new Vector3(newObject.transform.localScale.x * explosionRange, newObject.transform.localScale.y * explosionRange, 1); // change its local scale in x y z format
        }

        //Check for enemies
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRange, whatIsEnemies);
        for(int i = 0; i < enemies.Length; i++)
        {
            //Get component of enemy and call Take Damage
            //Damage
            //Debug.Log("CACA!");

            if (isPlayerbullet == true && enemies != null)
            {
                //Debug.Log(enemies[i]);
                enemies[i].GetComponent<EnemyAI>().TakeDamage(explosionDamage);
                //Debug.Log("HOLA!");
            }
            else
            {
                enemies[i].GetComponent<PlayerHealth>().TakeDamage(explosionDamage);
            }
            //Explosion force
            //if(enemies[i].GetComponent<Rigidbody2D>())
            //enemies[i].GetComponent<Rigidbody2D>().AddExplosionForce(explosionForce, transform.position, explosionRange);        
        }

        //Add a delay & destroy bullet
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Count up collisions
        collisions++;

        //Explode if bullet hits an enemy direction and explodeOnTouch is activated
        if(isPlayerbullet)
        {
            if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Obstacle") && explodeOnTouch)
            {
                Explode();
            }
        }
        else
        {
            if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Obstacle") && explodeOnTouch)
            {
                Explode();
            }
            //Debug.Log(collision.collider.tag);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
