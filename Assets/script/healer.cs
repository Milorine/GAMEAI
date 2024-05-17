using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healer : MonoBehaviour
{
    public HealerState currentState;
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
        //Healerbot goes into idle mode which is to roam around the forest
            
        //When healer-bot detects a poke-bot, transition to Appraoch
        currentState = HealerState.Approach;

    }
    public void Approach()
    {
        Debug.Log("Running to poke-bot");
        //Poke-bot spawns and healer-bot is moving towards the poke-bot

        //When healer-bot reach the poke-bot, transition to IdentifyWound
        currentState = HealerState.IdentifyWound;
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
