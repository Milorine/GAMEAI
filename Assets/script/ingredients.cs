using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ingredients : MonoBehaviour
{
    public List<GameObject> Ingredients = new List<GameObject>();
    [SerializeField] LayerMask groundLayer;
    public Vector3 randomPosition;
    // Start is called before the first frame update
    void Start()
    {
        SpawnIngredients();
    }

    // Update is called once per frame
    void Update()
    {   
        
    }
    void SpawnIngredients()
    {     
        for (int i = 0; i < Ingredients.Count; i++) 
        {
            // Generate a random position within the specified range
            float x = Random.Range(-20f, 20f);
            float z = Random.Range(-20f, 20f);
            float y = -0.8f;
            // Instantiate the Ingredients at the random position
            randomPosition = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
            if (Physics.Raycast(randomPosition, Vector3.down, groundLayer))
            {
                Ingredients[i] = Instantiate(Ingredients[i], randomPosition, Quaternion.identity);

            }
            
        }

    }
}
