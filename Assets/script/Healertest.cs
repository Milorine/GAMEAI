using UnityEngine;
using Panda;
using System.Linq;

public class Healertest : MonoBehaviour
{
    public float speed = 5f;
    public Transform Pokebot;
    private Vector3 direction;
    private int orbitCount = 0;
    bool walkPoint = false;
    bool reachPokebot = false;
    [SerializeField] LayerMask groundLayer;
    GameObject[] ingredients;
    bool itemSelect = false;
    GameObject Ingredient;
    public float orbitSpeed = 1f;
    public float orbitRadius = 2f;
    private float angle = 0f;
    private int ingredientCount = 0;

    void Start()
    {
        UpdateIngredients();
    }

    void Update()
    {

    }

    [Task]
    void Idle()
    {
        Debug.Log("Idling");
        if (!walkPoint)
        {
            RandomLocation();
        }
        if (walkPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, direction) < 10f)
        {
            walkPoint = false;
        }

        Task.current.Succeed();
    }

    [Task]
    bool IsPokebotDetected()
    {
        return Pokebot != null;
    }

    [Task]
    void Approach()
    {
        if (Pokebot != null && !reachPokebot)
        {
            transform.position = Vector3.MoveTowards(transform.position, Pokebot.position, speed * Time.deltaTime);
            Debug.Log("Approaching Pokebot");
        }

        if (Vector3.Distance(transform.position, Pokebot.position) < 2f)
        {
            reachPokebot = true;
            Task.current.Succeed();
        }
    }

    [Task]
    bool IsPokebotReached()
    {
        return reachPokebot;
    }

    [Task]
    void IdentifyWound()
    {
        Debug.Log("Identifying Wound on Pokebot");
        if (Pokebot != null)
        {
            angle += orbitSpeed * Time.deltaTime;
            float x = Mathf.Cos(angle) * orbitRadius;
            float z = Mathf.Sin(angle) * orbitRadius;

            if (orbitCount < 2)
            {
                transform.position = Pokebot.position + new Vector3(x, 0.7f, z);
            }

            if (orbitCount == 2)
            {
                Task.current.Succeed();
                return;
            }

            if (angle >= 2 * Mathf.PI)
            {
                orbitCount++;
                angle = 0f;
            }
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    bool IsWoundIdentified()
    {
        return orbitCount >= 2;
    }

    [Task]
    void FindIngredients()
    {
        if (!itemSelect)
        {
            Debug.Log("Finding ingredients for potion");
            itemSelect = true;
            Ingredient = ingredients[Random.Range(0, ingredients.Length)];
            ingredientCount++;
            Debug.Log(Ingredient);
            Debug.Log(ingredientCount);
        }

        transform.position = Vector3.MoveTowards(transform.position, Ingredient.transform.position, speed * Time.deltaTime);

        if (ingredientCount == 3)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    bool IsIngredientReached()
    {
        return Vector3.Distance(transform.position, Ingredient.transform.position) < 1f;
    }

    [Task]
    void CheckList()
    {
        Debug.Log("Checking if there is missing ingredients");

        // Assuming all ingredients are found for simplicity
        Task.current.Succeed();
    }

    [Task]
    bool AreAllIngredientsFound()
    {
        return true; // Adjust this condition based on your game logic
    }

    [Task]
    void CreatePotion()
    {
        Debug.Log("Creating Potion");

        Task.current.Succeed();
    }

    [Task]
    void DeliverPotion()
    {
        Debug.Log("Delivering potion to Pokebot");

        transform.position = Vector3.MoveTowards(transform.position, Pokebot.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Pokebot.position) < 1f)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    void AdministerPotion()
    {
        Debug.Log("Administering Potion to Pokebot");

        // Assuming the potion is administered successfully for simplicity
        Task.current.Succeed();
    }

    [Task]
    bool IsPokebotHealed()
    {
        return true; // Adjust this condition based on your game logic
    }

    [Task]
    void Celebrate()
    {
        Debug.Log("Celebrating");

        Task.current.Succeed();
    }

    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.CompareTag("Bot"))
        {
            Pokebot = collide.transform;
        }

        if (collide.gameObject == Ingredient)
        {
            itemSelect = false;
        }
    }

    void OnTriggerExit(Collider pokebot)
    {
        if (pokebot.gameObject.CompareTag("Bot"))
        {
            orbitCount = 0;
            reachPokebot = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bot"))
        {
            reachPokebot = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bot"))
        {
            reachPokebot = false;
            orbitCount = 0;
        }
    }

    void RandomLocation()
    {
        float x = Random.Range(-15f, 15f);
        float z = Random.Range(-15f, 15f);
        direction = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(direction, Vector3.down, groundLayer))
        {
            walkPoint = true;
        }
    }
    void UpdateIngredients()
    {
        ingredients = FindObjectsOfType<GameObject>().Where(obj => obj.tag.StartsWith("Item")).ToArray();
    }
}
