using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchState : IMonsterState {

    //For searching furniture
    private Collider2D[] potentialFurniture = null;
    private List<Collider2D> realFurniture;
    private Collider2D[] finalFurniture;
    private int furnitureIndex = 0;

    private MonsterAI monster;

    public enum SearchingSubState
    {
        CheckingLastPosition,
        CheckingFurniture
    }

    public SearchingSubState currentSubState;
   

    public MonsterSearchState(MonsterAI monsterAI)
    {
        monster = monsterAI;
    }


    public void UpdateState()
    {

        switch (currentSubState) {
            case SearchingSubState.CheckingLastPosition:
                CheckingLastPosition();
                break;
            case SearchingSubState.CheckingFurniture:
                CheckingFurniture();
                break;
        }


    }

    void Awake()
    {
        currentSubState = SearchingSubState.CheckingLastPosition;
        realFurniture = new List<Collider2D>();
    }

    private void CheckingLastPosition()
    {
        monster.agent.destination = monster.searchPosition;

        if (monster.TargetIsVisible())
        {
            ToMonsterChaseState();
            return;
        }

        //Debug.Log("Test : " + (monster.transform.position - monster.searchPosition).magnitude + "\nStopping Distance: " + monster.agent.stoppingDistance);

        //If we've reached the spot where we last heard/saw the player, check furniture
        if ((monster.transform.position - monster.searchPosition).magnitude < (monster.agent.stoppingDistance + 1f))
        {
            Collider2D hidingSpot = Physics2D.OverlapCircle(monster.transform.position, 1.5f);
            if (hidingSpot != null && (hidingSpot.transform.tag == "Furniture" || hidingSpot.transform.tag == "HideObject"))
            {
                if (!hidingSpot.gameObject.GetComponent<Furniture_Controller>().GetIsEmpty())
                    Debug.Log("Player Found!"); // TODO change this to fit with Andrue's interactive furniture, and reset level
            }
            

            furnitureIndex = 0;
            monster.furnitureSearchTime = 0f;
            potentialFurniture = null;
            currentSubState = SearchingSubState.CheckingFurniture;
            return;
        }
    }

    private void CheckingFurniture()
    {
        
        if (monster.TargetIsVisible())
        {
            ToMonsterChaseState();
            return;
        }


        if (potentialFurniture == null)
        {
            potentialFurniture = Physics2D.OverlapCircleAll(monster.transform.position, monster.furnitureSearchRadius);

            for (int i = 0; i < potentialFurniture.Length; i++)
            {
                //TODO raycast to make sure all furniture is within line of sight
                if (potentialFurniture[i].transform.tag == "Furniture" && (Physics2D.Raycast(monster.transform.position, potentialFurniture[i].transform.position - monster.transform.position).transform.tag == "Furniture"))
                {
                    //Debug.Log("FOUND FURNITURE");
                    realFurniture.Add(potentialFurniture[i]);
                }
            }


            if (realFurniture.Count <= 0)
                Debug.Log("No furniture in room.");

            finalFurniture = realFurniture.ToArray();

            
        }

        if (finalFurniture == null)
        {
            Debug.Log("No furniture found.");
            ToMonsterPatrolState();
            return;
        }


        //TODO enable for a random chance that it'll skip over furniture
        /*
            int min = 0;
            int max = 4;
            int retVal = Random.Range(min, max);
            if (retVal == 4)
            {
                furnitureIndex++;
                return;
            } else
            {
                InvestigateFurniture(potentialFurniture[furnitureIndex]);
        }*/
        
        if (furnitureIndex < finalFurniture.Length)
        {
            
            InvestigateFurniture(finalFurniture[furnitureIndex]);
        }
        else
        {
            ToMonsterPatrolState();
        }
    }

    private void InvestigateFurniture(Collider2D furniture)
    {

        //TODO - Potential bug: Monster goes to center of furniture, and can't see outside of it.
        Vector3 furniturePos = new Vector3(furniture.gameObject.GetComponent<Transform>().position.x, monster.proxyLocation.position.y, furniture.gameObject.GetComponent<Transform>().position.y);
        monster.agent.destination = furniturePos; // - new Vector3(2f,0f,0f);

        if ((furniture.transform.position - monster.transform.position).magnitude < monster.agent.stoppingDistance)
        {

            //TODO check for player
            if (!furniture.gameObject.GetComponent<Furniture_Controller>().GetIsEmpty())
                Debug.Log("Player found!");


            monster.furnitureSearchTime += Time.smoothDeltaTime;

            if (monster.furnitureSearchTime >= monster.maxFurnitureSearchTime)
            {
                monster.furnitureSearchTime = 0f;
                furnitureIndex++;
                return;

            }

        }
        else
        {
            //Debug.Log("should be travelling");
        }



    }

    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        //If we encounter a distraction, we exit our previous state,
        //no matter what that state was, and go to the Distraction state
        if (other.tag == "Distraction")
        {
            ToMonsterDistractionState(other);
            return;
        }

        //If we encounter the player, the game should reset. Nothing for now.
        if (other.tag == "Player")
        {
            //TODO reset game.
            Debug.Log("Player caught");
        }
    }*/

    #region State Transitions
    public void ToMonsterPatrolState()
    {
        monster.agent.destination = new Vector3(monster.patrolNodes[monster.currentNode].x, monster.proxyLocation.position.y, monster.patrolNodes[monster.currentNode].y);
        monster.currentState = monster.monsterPatrolState;
    }

    public void ToMonsterChaseState()
    {
        monster.currentState = monster.monsterChaseState;
    }

    public void ToMonsterSearchState()
    {
        Debug.Log("Transition Error: Already in Search State.");
        
    }

    public void ToMonsterDistractionState(Collider2D other)
    {
        monster.distractionTime = 0f;
        monster.currentState = monster.monsterDistractionState;
        monster.agent.destination = new Vector3(other.GetComponent<Transform>().position.x, monster.proxyLocation.position.y, other.GetComponent<Transform>().position.y);
        
        //Once the distraction is finished, destroy it.
        GameObject.Destroy(other.gameObject, monster.maxDistractionTime);
    }
    #endregion

    

}
