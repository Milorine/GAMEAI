using UnityEngine;
using Panda;
using System.Linq;

public class Healertest : MonoBehaviour
{
    public float speed = 5f;  //healer speed
    public Transform Pokebot; //pokebot prefab
    private Vector3 direction; //direction for healer bot
    private int orbitCount = 0; //orbit count for healer
    bool walkPoint = false; //walkpoint for healer
    bool reachPokebot = false; //if healer rach pokebot
    [SerializeField] LayerMask groundLayer; //layermask for ground
    GameObject[] ingredients; // ingredient array
    bool itemSelect = false; //selecting ingredient to go to
    GameObject Ingredient; // ingredient
    public float orbitSpeed = 1f; //orbit speed
    public float orbitRadius = 2f; // orbit radius
    private float angle = 0f; // angle of orbit
    private int ingredientCount = 0; // ingredient count
    public float followSpeed = 20f; //potion follow speed
    public float spawnHeight = 2f; // potion follow height
    bool potionDelivered = false;
    public GameObject potion;

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

        if (Vector3.Distance(transform.position, Pokebot.position) < 1.5f)
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
            Ingredient = ingredients[Random.Range(0, ingredients.Length )];
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
    bool AreAllIngredientsFound()
    {
        if (ingredientCount == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [Task]
    void CreatePotion()
    {
       
        Vector3 cubePosition = transform.position + Vector3.up * spawnHeight;
        potion = Instantiate(potion, cubePosition, Quaternion.identity);


        if (potion != null)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    bool IsPotionCreated()
    {
        return true;
    }

    [Task]
    void DeliverPotion()
    {
        if (potion != null)
        {
            Vector3 healerHead = transform.position + Vector3.up * 2f;
            potion.transform.position = healerHead;
        }
        Debug.Log("Delivering potion to Pokebot");

        transform.position = Vector3.MoveTowards(transform.position, Pokebot.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Pokebot.position) < 1f)
        {
            potionDelivered = true;
        }
    }

    [Task]

    bool PotionDelivered()
    {
        return potionDelivered;
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
            ingredientCount++;
        }
    }

    void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.CompareTag("Bot"))
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
