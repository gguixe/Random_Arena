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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun();
        NavMeshMovement();
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

}
