using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAI : MonoBehaviour
{
    //Rotate Gun
    public GameObject player;
    public GameObject pivot;
    private GameObject[] parray;

    public float moveSpeed = 0.25f;

    float maxX = 6.1f;
    float minX = -6.1f;
    float maxY = 4.2f;
    float minY = -4.2f;

    private float tChange = 0; // force new direction in the first Update
    private float randomX;
    private float randomY;

    private void Start()
    {
        parray = GameObject.FindGameObjectsWithTag("Player");
        int rand = Random.Range(0, 9);
        player = parray[rand];
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        RandomMovement();
    }

    void RotateGun()
    {
        Vector3 difference = player.transform.position - pivot.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z - 180f);
    }

    void RandomMovement()
    {
        // change to random direction at random intervals
        if (Time.time >= tChange)
        {
            randomX = Random.Range(-2.0f, 2.0f); // with float parameters, a random float
            randomY = Random.Range(-2.0f, 2.0f); //  between -2.0 and 2.0 is returned
                                               // set a random interval between 0.5 and 1.5
            tChange = Time.time + Random.Range(0.5f, 1.5f);
        }
        transform.Translate(new Vector3(randomX, randomY, 0) * moveSpeed * Time.deltaTime);
        // if object reached any border, revert the appropriate direction
        if (transform.position.x >= maxX || transform.position.x <= minX)
        {
            randomX = -randomX;
        }
        if (transform.position.y >= maxY || transform.position.y <= minY)
        {
            randomY = -randomY;
        }
        // make sure the position is inside the borders
        float Xpos = Mathf.Clamp(transform.position.x, minX, maxX);
        float Ypos = Mathf.Clamp(transform.position.y, minY, maxY);
        Vector3 temp = new Vector3(Xpos, Ypos, transform.position.z);
        gameObject.transform.position = temp;
    }
}
    




