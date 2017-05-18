using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchState : IMonsterState {

    //For searching furniture
    private Collider2D[] potentialFurniture = null;
    private List<Collider2D> realFurniture;
    private Collider2D[] finalFurniture;
    private int furnitureIndex = 0;

    private float furnitureCheckDistance = 3.75f;

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

        if (monster.agent.speed != monster.searchSpeed)
        {
            monster.agent.speed = monster.searchSpeed;
        }

    }

    void Awake()
    {
        currentSubState = SearchingSubState.CheckingLastPosition;
        
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
        if ((monster.transform.position - monster.searchPosition).magnitude < (furnitureCheckDistance))
        {
            Collider2D hidingSpot = Physics2D.OverlapCircle(monster.transform.position, 1.5f);
            if (hidingSpot != null && (hidingSpot.transform.tag == "Furniture" || hidingSpot.transform.tag == "HideObject"))
            {
                if (hidingSpot.gameObject.GetComponent<HideHero>().GetIsHiding())
                {
                    if (monster.debugInfo)
                        Debug.Log("Player found in hiding place");
                }
                    
            }
            

            furnitureIndex = 0;
            monster.furnitureSearchTime = 0f;

            potentialFurniture = null;
            realFurniture = new List<Collider2D>();
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
            //potentialFurniture = Physics2D.OverlapCircleAll(monster.transform.position, monster.furnitureSearchRadius);
            potentialFurniture = Physics2D.OverlapBoxAll(
                monster.roomManager.GetComponent<RoomManager>().getRoomTransform(monster.GetRoomY(),
                monster.GetRoomX()).position, new Vector2(monster.roomDimensionsX, monster.roomDimensionsY), 0f); //TODO verify this works

            for (int i = 0; i < potentialFurniture.Length; i++)
            {
                //TODO renable raycast if needed.
                if (potentialFurniture[i].transform.tag == "HideObject")// && (Physics2D.Raycast(monster.transform.position, potentialFurniture[i].transform.position - monster.transform.position).transform.tag == "HideObject"))
                {
                    if (monster.debugInfo)
                    {
                        Debug.Log("FOUND FURNITURE");
                    }
                        
                    realFurniture.Add(potentialFurniture[i]);
                }
            }

            
            //if (finalFurniture == null)
            //  finalFurniture = realFurniture.ToArray();


            if (realFurniture.Count <= 0)
            {
                if (monster.debugInfo)
                    Debug.Log("No furniture found, resuming patrol state.");
                ToMonsterPatrolState();
                return;
            }
            else if (monster.debugInfo)
            {
                Debug.Log("Furniture found in room: " + realFurniture.Count);

            }

        }

        


        //TODO enable for a random chance that it'll skip over 1/4 furniture
        int min = 0;
        int max = 4;
        int retVal = Random.Range(min, max);
        if (retVal == 4)
        {
            furnitureIndex++;
            return;
        } else //TODO to disable skipping furniture, comment lines from here to the comment above
        if (furnitureIndex < realFurniture.Count)
        {
            
            InvestigateFurniture(realFurniture[furnitureIndex]);
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

        if ((furniture.transform.position - monster.transform.position).magnitude < furnitureCheckDistance)
        {
            

            //TODO check for player
            if (furniture.gameObject.GetComponent<HideHero>().GetIsHiding())
            {
                if (monster.debugInfo)
                    Debug.Log("Player found!");

                furniture.gameObject.GetComponent<HideHero>().LeaveObject();
                monster.mainCamera.GetComponent<healthbar>().TakeDemage(monster.damageOnContact);

            }
                

            //TODO - Rummaging through furnature noise
            monster.furnitureSearchTime += Time.smoothDeltaTime;

            if (monster.furnitureSearchTime >= monster.maxFurnitureSearchTime)
            {
                monster.furnitureSearchTime = 0f;
                furnitureIndex++;
                return;

            }

        }
        else if (monster.debugInfo)
        {
            Debug.Log("Approaching furniture: " + (furniture.transform.position - monster.transform.position).magnitude);
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
            Debug.Log("if (debugInfo) if (debugInfo) Debug.Log("Player caught");");
        }
    }*/

    #region State Transitions
    public void ToMonsterPatrolState()
    {
        monster.SetDestination();
        monster.currentState = monster.monsterPatrolState;
        monster.agent.autoBraking = false;
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
