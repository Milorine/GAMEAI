using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
public class healer : MonoBehaviour
{
    public HealerState currentState;
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

    public float orbitSpeed = 1f; // Speed of rotation
    public float orbitRadius = 2f; // Distance from the cube

    private float angle = 0f; // Current angle in radians
    public enum HealerState
        //states of the healerbot
    {   
        Idle,
        Approach,
        IdentifyWound,
        FindIngredients,
        CheckList,
        CreatePotion,
        DeliverPotion,
        AdministerPotion,
        Celebrate
    }

    // Start is called before the first frame update
    void Start()
    {
        ingredients = FindObjectsOfType<GameObject>().Where(obj => obj.tag.StartsWith("Item")).ToArray();
        Debug.Log(ingredients.Length);
        currentState = HealerState.Idle;
        Idle();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case HealerState.Idle:
                Idle();
                break;
            case HealerState.Approach:
                Approach();
                break;
            case HealerState.IdentifyWound:
                IdentifyWound();
                break;
            case HealerState.FindIngredients:
                FindIngredients();
                break;
            case HealerState.CheckList:
                CheckList();
                break;
            case HealerState.CreatePotion:
                CreatePotion();
                break;
            case HealerState.DeliverPotion:
                DeliverPotion();
                break;
            case HealerState.AdministerPotion:
                AdministerPotion();
                break;
            case HealerState.Celebrate:
                Celebrate();
                break;
        }

    }
    public void Idle()
    {
        Debug.Log("Idling");

        //if walkpoint is false, start finding random location
        if (!walkPoint)
        { 
            RandomLocation(); 
        }
        //if walkpoint is true walk toward the new location
        if (walkPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        }
        //if the healerbot is near the new location set walkpoint to false and pick a new location
        if (Vector3.Distance(transform.position, direction) < 10f)
        {
            walkPoint = false;
        }
        //Healerbot goes into idle mode which is to roam around the forest

        //When healer-bot detects a poke-bot, transition to Appraoch
        //currentState = HealerState.Approach;
    }
    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.CompareTag("Bot"))
        {
            Debug.Log("collide");
            Pokebot = collide.transform;
            currentState = HealerState.Approach;
        }

        //collision for ingredients
        if (collide.gameObject == Ingredient)
        {
            Debug.Log("reached");
        }

    }
    void OnTriggerExit(Collider pokebot)
    {
        if (pokebot.gameObject.CompareTag("Bot"))
        {
            orbitCount = 0;
            reachPokebot = false;
            Debug.Log("exit");
        }
    }

    public void RandomLocation()
    {   
        //randomize the x and z value for the direction the healer-bot is moving to
        float x = Random.Range(-20f, 20f);
        float z = Random.Range(-20f, 20f);
        //set the new direction that the healerbot is moving to
        direction = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        //check if the new location has a ground, if there is ground set walkpoint to true
        if (Physics.Raycast(direction,Vector3.down,groundLayer))
        {
            walkPoint = true;
        }


    }
    public void Approach()
    {
        // calculate the direction from the healer to the cube
        if (Pokebot != null & !reachPokebot)
        {   
            //find the position of the pokebot, move the healer bot toward the pokebot
            transform.position = Vector3.MoveTowards(transform.position, Pokebot.position , speed * Time.deltaTime);
            Debug.Log("running to poke-bot");
        }
        //poke-bot spawns and healer-bot is moving towards the poke-bot

        //when healer-bot reach the poke-bot, transition to identifywound
        //currentstate = healerstate.identifywound;
        if (Pokebot == null)
        {   
            currentState = HealerState.Idle;
        }
    }

    void OnCollisionEnter(Collision collision)
    {   
        //collision with the pokebot
        if (collision.gameObject.tag == "Bot")
        {   
            //when the healer bot collide with the pokebot change state to identifywound and set reachpokebot to true avoid repeated approach to pokebot
            Debug.Log("enter");
            reachPokebot = true;
            currentState = HealerState.IdentifyWound;
        }

        ////collision with the ingredients
        //if (collision.gameObject == Ingredient)
        //{
        //    Debug.Log("reached");
        //}
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Bot")
        {
            //after the wound is identify and healerbot leave the pokebot set reachpokebot to false to allow healer to collide later on and set orbitcount to zero.
            reachPokebot = false;
            orbitCount = 0;
            Debug.Log("exit cube");
        }
    }

    public void IdentifyWound()
    {
        Debug.Log("Identifying Wound on poke-bot");
        if (Pokebot != null)
        {   
            //calculate the angle depending on the the speed of healerbot
            angle += orbitSpeed * Time.deltaTime;

            //calculate the position of the orbit around the cube
            float x = Mathf.Cos(angle) * orbitRadius;
            float z = Mathf.Sin(angle) * orbitRadius;
            if (orbitCount < 2)
            {
                transform.position = Pokebot.position + new Vector3(x, 0f, z);
            }

            if (orbitCount == 2)
            {
                currentState = HealerState.FindIngredients;
            }
            //find how many orbit the healerbot has done, 1 circle is 2pi
            if (angle >= 2 * Mathf.PI)
            {
                orbitCount++; 
                angle = 0f; 
            }
        if (Pokebot == null)
            {
                currentState = HealerState.Idle;
            }
        }
        //Healer-bot identify the poke-bot and identify the natural ingredients needed to craft the healing potion

        //When healer-bot identify what natural ingredients to find, tranisiton to FindIngredients.
        //currentState = HealerState.FindIngredients;
    }

    public void FindIngredients()
    {   
        //see if item selected is true
        if (!itemSelect)
        {   
            //sset itemselected to true to prevent selecting another item
            itemSelect = true;
            Ingredient = ingredients[Random.Range(0, ingredients.Length)];
            Debug.Log("Finding ingredients for potion");
        }
        transform.position = Vector3.MoveTowards(transform.position, Ingredient.transform.position, speed * Time.deltaTime);

        //Healer-bot move around the map searching for the natural ingredients

        //When healer-bot finds the ingredients, transition to CheckList.
        //currentState = HealerState.CheckList;
    }

    public void CheckList()
    {
        Debug.Log("Checking if there is missing ingredients");

        //When healer-bot finds all the ingredients needed for potion, transition to  CreatePotion
        currentState = HealerState.CreatePotion;

        //When healer-bot still have missing ingredient to find, transition to FindIngredients.
        //currentState = HealerState.FindIngredients;
    }

    public void CreatePotion()
    {   
        Debug.Log("Creating Potion");
        //Healer-bot create the potion with the ingredients gathered

        //When healer-bot creates the potion, transition to DeliverPotion
        currentState = HealerState.DeliverPotion;
    }
    public void DeliverPotion()
    {
        Debug.Log("Delivering potion to poke-bot");
        //Healer-bot returns back to the poke-bot with the potion

        //When healer-bot reach the poke-bot, transition to AdministerPotion.
        currentState = HealerState.AdministerPotion;

    }
    public void AdministerPotion()
    {
        Debug.Log("Giving poke-bot potion");
        //Healer-bot give the healing potion to the poke-bot and monitor the wound if the poke-bot is not healed idenfity the wound again.

        //When healer-bot give the potion to poke-bot and the poke-bot fully heals, transition to Celebrate.
        currentState = HealerState.Celebrate;

        //When healer-bot give the potion to poke-bot and the poke-bot is still not heals fully, transition to IdentifyWound.
        //currentState = HealerState.IdentifyWound;

    }
    public void Celebrate()
    {
        Debug.Log("Healer-bot celebrating");
        //Healer-bot celebrate by dancing after aiding a fellow poke-bot
            
        //After the healer-bot celebrate, transition to Idle
        currentState = HealerState.Idle;


        
    }
}
