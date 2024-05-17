using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class healer : MonoBehaviour
{
    public HealerState currentState;
    public float speed = 5f;
    private Vector3 direction;
    [SerializeField] LayerMask groundLayer;
    bool walkPoint;
    public Transform Pokebot;
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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bot"))
        {
            Pokebot = other.transform;
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

        // Calculate the direction from the healer to the cube
        if (Pokebot != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Pokebot.position, speed * Time.deltaTime);
            Debug.Log("Running to poke-bot");
        }
        //Poke-bot spawns and healer-bot is moving towards the poke-bot

        //When healer-bot reach the poke-bot, transition to IdentifyWound
        //currentState = HealerState.IdentifyWound;
        if (Pokebot == null)
        {
            currentState = HealerState.Idle;
        }
    }

    
    public void IdentifyWound()
    {
        Debug.Log("Identifying Wound on poke-bot");
        //Healer-bot identify the poke-bot and identify the natural ingredients needed to craft the healing potion

        //When healer-bot identify what natural ingredients to find, tranisiton to FindIngredients.
        currentState = HealerState.FindIngredients;
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
