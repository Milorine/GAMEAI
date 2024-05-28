using Panda.Examples.PlayTag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pokebot : MonoBehaviour
{

    public GameObject player;
    public float followSpeed = 5f;
    public float stoppingDistance = 2f;
    void Update()
    {
        followPlayer();
    }

    private void Start()
    {

    }

    public void followPlayer()
    {   

        Vector3 playerPosition = player.transform.position;
        playerPosition.y = 0.2f;
        if (playerPosition.magnitude > stoppingDistance)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, followSpeed * Time.deltaTime);
        }
    }

}