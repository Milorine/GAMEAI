using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pokebot : MonoBehaviour
{
    // Reference to the cube prefab
    public GameObject cubePrefab;
    [SerializeField] LayerMask groundLayer;
    public Vector3 randomPosition;
    // Range for random position

    private GameObject currentCube;

    void Update()
    {
        // Check for input or condition to spawn a cube
    }

    private void Start()
    {
        SpawnRandomCube();
    }
    void SpawnRandomCube()
    {
        if (currentCube != null)
        {
            Destroy(currentCube);
        }
        // Generate a random position within the specified range
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        float y = -0.8f;
        randomPosition = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
        // Instantiate the cube at the random position
        if (Physics.Raycast(randomPosition, Vector3.down, groundLayer))
        {
            currentCube = Instantiate(cubePrefab, randomPosition, Quaternion.identity);

        }

    }
}