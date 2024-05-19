using System.Collections;
using System.Collections.Generic;
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
    void OnTriggerEnter(Collider pokebot)
    {
        if (pokebot.gameObject.CompareTag("Bot"))
        {
            Pokebot = pokebot.transform;
            currentState = HealerState.Approach;
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
        if (collision.gameObject.tag == "Bot")
        {
            Debug.Log("enter");
            reachPokebot = true;
            currentState = HealerState.IdentifyWound;
        }
    }
    public void IdentifyWound()
    {
        Debug.Log("Identifying Wound on poke-bot");
        if (Pokebot != null)
        {
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
        }
        //Healer-bot identify the poke-bot and identify the natural ingredients needed to craft the healing potion

        //When healer-bot identify what natural ingredients to find, tranisiton to FindIngredients.
        //currentState = HealerState.FindIngredients;
    }

    public void FindIngredients()
    {
        Debug.Log("Finding ingredients for potion");
        //Healer-bot move around the map searching for the natural ingredients

        //When healer-bot finds the ingredients, transition to CheckList.
        currentState = HealerState.CheckList;
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
