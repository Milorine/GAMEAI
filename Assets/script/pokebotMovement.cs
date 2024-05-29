using Panda.Examples.PlayTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pokebotMovement : MonoBehaviour
{
    public GameObject player;
    public float speed = 5f;
    public float maxDistanceFromPlayer = 10f;
    public bool follow = true;
    private Transform target;
    void Update()
    {
        followPlayer();
    }
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {

    }

    public void followPlayer()
    {   

        Vector3 dirToPlayer = target.position - transform.position;

        if (dirToPlayer.magnitude > maxDistanceFromPlayer)
        {
            dirToPlayer = dirToPlayer.normalized * maxDistanceFromPlayer;
        }

        // Calculate the new position within the controlled radius
        Vector3 newPosition = target.position - dirToPlayer;
        if (follow){
        // Update the position
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);
        }
    }

}