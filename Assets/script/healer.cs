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
        FindPokeBot,
        IdentifyWound,
        FindHealingItem,
        CheckHealingItem,
        DeliverHealingItem,
        ApplyHealingItem,
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
                IdentifyWound();
                break;
            case HealerState.FindPokeBot:
                FindPokeBot();
                break;
            case HealerState.IdentifyWound:
                IdentifyWound();
                break;
            case HealerState.FindHealingItem:
                FindHealingItem();
                break;
            case HealerState.CheckHealingItem:
                IdentifyWound();
                break;
            case HealerState.DeliverHealingItem:
                IdentifyWound();
                break;
            case HealerState.ApplyHealingItem:
                IdentifyWound();
                break;
            case HealerState.Celebrate:
                IdentifyWound();
                break;
        }
    }

    public void Idle()
    {
        Debug.Log("Idling");
        //Healerbot goes into idle mode when no pokebot spawn for 5 seconds
    }
    public void FindPokeBot()
    {
        Debug.Log("Finding pokebot");
        //Pokebot spawns and Healerbot is moving towards the pokebot
    }
    public void IdentifyWound()
    {
        Debug.Log("Identifying Wound on pokebot");
        //Healerbot identify the pokebot and identify the healing item needed to heal the pokebot
    }
    public void FindHealingItem()
    {
        Debug.Log("Finding Healing Item");
        //Healerbot move around the map searching for the nearest healing item in range
    }
    public void CheckHealingItem()
    {
        Debug.Log("Checking if healing item is the correct one");
        //Healerbot check if the healing item picked up was correct. if it is transition to DeliverHealingItem, if not transition to FindingHealinItem
    }
    public void DeliverHealingItem()
    {
        Debug.Log("Delivering healing item to pokebot");
        //Healerbot returns back to the pokebot with the healing item

    }
    public void ApplyHealingItem()
    {
        Debug.Log("Finding pokebot");
        //Healerbot Apply the HealingItem to the pokebot

    }

    public void MonitorPokeBot()
    {
        Debug.Log("Monitoring Pokebot");
        //Healerbot monitor the pokebot for awhile, if pokebot still require healing transition to FindHealingItem, if not transition to Celebrate

    }
    public void Celebrate()
    {
        Debug.Log("Healer bot celebrating");
        //Healerbot celebrate by dancing after aiding a fellow pokebot

    }
}
