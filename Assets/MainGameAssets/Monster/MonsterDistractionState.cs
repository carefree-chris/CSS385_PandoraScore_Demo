using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDistractionState : IMonsterState {

    private MonsterAI monster;

    public MonsterDistractionState(MonsterAI monsterAI)
    {
        monster = monsterAI;
    }

    public void UpdateState()
    {
        monster.distractionTime += Time.smoothDeltaTime;

        //Once the duration of the distraction is over, we go back to patrolling
        if (monster.distractionTime >= monster.maxDistractionTime)
        {
            //ToSearching();
            //searchPosition = transform.position;

            ToMonsterPatrolState();
        }
    }

    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        
        //If we encounter the player, the game should reset. Nothing for now.
        if (other.tag == "Player")
        {
            //TODO reset game.
            Debug.Log("Player caught");
        }
    }
    */

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
        if (monster.targetLocation != null)
        {
            monster.searchPosition = new Vector3(monster.targetLocation.position.x, monster.targetLocation.position.y, 0f);

            monster.monsterSearchState.currentSubState = MonsterSearchState.SearchingSubState.CheckingLastPosition; //TODO verify this works
            monster.currentState = monster.monsterSearchState;
        }
        else
        {
            Debug.Log("Player disabled, transitioning to patrol state.");
        }
    }

    public void ToMonsterDistractionState(Collider2D other)
    {
        Debug.Log("Transition Error: Already in Distraction State.");
    }
    #endregion
}
