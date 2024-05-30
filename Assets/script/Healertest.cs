using UnityEngine;
using Panda;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Healertest : MonoBehaviour
{   
    public Text displayText;
    public pokebotMovement pokebotMovement;
    public float speed = 5f;  //healer speed
    private Vector3 direction; //direction for healer bot
    [SerializeField] LayerMask groundLayer; //layermask for ground
    GameObject[] ingredients; // ingredient array
    bool itemSelect = false; //selecting ingredient to go to
    GameObject Ingredient; // ingredient
    private int ingredientCount = 0; // ingredient count
    public float followSpeed = 20f; //potion follow speed
    public float spawnHeight = 2f; // potion follow height
    bool potionDelivered = false;
    bool pokebotDetected = false;
    bool idling = false;
    bool reachPlayer = false;
    bool pokebotSeated = false;
    bool potionCreated = false;
    bool pokebotGivenBack = false;
    public GameObject potionprefab;
    private GameObject potion;
    private bool isInteract;
    public bool healerActive = true;
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
        Transform idleSpot = GameObject.FindGameObjectWithTag("IdleSpot").transform;
        transform.position = Vector3.MoveTowards(transform.position, idleSpot.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, idleSpot.position) < 1f)
        {   
            idling = true;
            Task.current.Succeed();
        }
    }
    [Task]
    bool IsIdle()
    {
        return idling;
    }
    [Task]
    bool IsPokebotDetected()
    {
        return pokebotDetected;
    }
    [Task]
    bool IsReachedPlayer()
    {
        return reachPlayer;
    }

    [Task]
    void Approach()
    {   
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Transform counterPlayer = GameObject.FindGameObjectWithTag("CounterPlayer").transform;
        Transform counterHealer = GameObject.FindGameObjectWithTag("CounterHealer").transform;
        Debug.Log(Vector3.Distance(transform.position, counterHealer.transform.position));
        if (Vector3.Distance(player.position, counterPlayer.transform.position) < 1.5f)
        {  
            transform.position = Vector3.MoveTowards(transform.position, counterHealer.transform.position, speed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, counterHealer.transform.position) < 1f)
        {
            reachPlayer = true;
            Task.current.Succeed();
        }
    }   


    [Task]
    void IdentifyWound()
    {   
        isInteract = false;
        GameObject pokebot = GameObject.Find("Pokebot");
        Transform Seat = GameObject.Find("Seat").transform;
        pokebotMovement.follow = false;
        pokebot.transform.position = transform.position + Vector3.up * 1.5f;
        pokebot.transform.position = Vector3.MoveTowards(pokebot.transform.position, transform.position, speed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, Seat.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Seat.transform.position) < 1.5f)
        {
            pokebot.transform.position = Seat.transform.position + Vector3.up * 0.3f;
        }
        if (Vector3.Distance(pokebot.transform.position, Seat.transform.position) < 1f)
        {
            pokebotSeated = true;
            Task.current.Succeed();
        }
    }

    [Task]
    bool IsPokebotSeated()
    {
        return pokebotSeated;
    }

    [Task]
    void FindIngredients()
    {   
        pokebotSeated = false;
        Debug.Log(itemSelect);
        if (!itemSelect)
        {
            Debug.Log("Finding ingredients for potion");
            itemSelect = true;
            Ingredient = ingredients[Random.Range(0, ingredients.Length )];
            Debug.Log(Ingredient);
            Debug.Log(ingredientCount);
        }
        transform.position = Vector3.MoveTowards(transform.position, Ingredient.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Ingredient.transform.position) < 1f)
        {   
            ingredientCount++;
            itemSelect = false;
        }
        if (ingredientCount == 3)
        {   
            Task.current.Succeed();
        }

    }


    [Task]
    bool AllIngredientsFound()
    {
        if (ingredientCount == 3)
        {   
            ingredientCount = 0;
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
        Transform potionMaker = GameObject.FindGameObjectWithTag("PotionMaker").transform;
        transform.position = Vector3.MoveTowards(transform.position, potionMaker.position, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, potionMaker.position) < 1)
        {
            Vector3 cubePosition = transform.position + Vector3.up * spawnHeight;
            potion = Instantiate(potionprefab, cubePosition, Quaternion.identity);

        }

        if (potion != null)
        {   
            potionCreated = true;
            Task.current.Succeed();
        }
    }

    [Task]
    bool IsPotionCreated()
    {
        return potionCreated;
    }

    [Task]
    void DeliverPotion()
    {   
        potionCreated = false;
        GameObject pokebot = GameObject.Find("Pokebot");
        if (potion != null)
        {
            Vector3 healerHead = transform.position + Vector3.up * 2f;
            potion.transform.position = healerHead;
        }
        Debug.Log("Delivering potion to Pokebot");

        transform.position = Vector3.MoveTowards(transform.position, pokebot.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pokebot.transform.position) < 2f)
        {
            potionDelivered = true;
            Destroy(potion);
            Task.current.Succeed();
        }
    }

    [Task]

    bool IsPotionDelivered()
    {
        return potionDelivered;
    }


    [Task]
    void ReturnPokebot()
    {   
        potionDelivered = false;
        Transform counterHealer = GameObject.FindGameObjectWithTag("CounterHealer").transform;
        Transform pokebot = GameObject.FindGameObjectWithTag("Pokebot").transform;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        pokebot.position = transform.position + Vector3.up * 1.5f;
        pokebot.position = Vector3.MoveTowards(pokebot.position, transform.position, speed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, counterHealer.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, counterHealer.transform.position) < 1f)
        {   
            pokebotMovement.follow = true;
            Task.current.Succeed();
        }
    }

    [Task]
    bool IsPokebotGivenBack()
    {
        return pokebotGivenBack;
    }

    [Task]
    bool Display(string text)
    {
        if (displayText != null)
        {
            displayText.text = text;
            displayText.enabled = text != "";
        }
        return true;
    }

    [Task]
    bool QueryPlayer(string text)
    {       

        displayText.text = text;
        return true;
    }
    
    [Task]

    bool IsInteract()
    {
        return isInteract;
    }

    [Task]
    bool StartInteract()
    {
        isInteract = true;
        return true;
    }

    [Task]
    bool EndInteract()
    {
        isInteract = false;
        return true;
    }
    void UpdateIngredients()
    {
        ingredients = FindObjectsOfType<GameObject>().Where(obj => obj.tag.StartsWith("Item")).ToArray();
    }
}
