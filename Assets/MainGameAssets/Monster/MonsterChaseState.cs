using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : IMonsterState {

    private MonsterAI monster;

    public MonsterChaseState(MonsterAI monsterAI)
    {
        monster = monsterAI;
    }

    public void UpdateState()
    {
        if (monster.TargetIsVisible() == true)
        {
            monster.SetDestination();
        }
        else
        {
            ToMonsterSearchState();
            return;
        }

        if (monster.agent.speed != monster.chaseSpeed)
        {
            monster.agent.speed = monster.chaseSpeed;
        }
    }

   

    #region State Transitions
    public void ToMonsterPatrolState()
    {
        monster.SetDestination();
        monster.currentState = monster.monsterPatrolState;
        monster.agent.autoBraking = false;
    }

    public void ToMonsterChaseState()
    {
        Debug.Log("Transition Error: Already in Chase State.");
    }

    public void ToMonsterSearchState()
    {
        if (monster.targetLocation != null)
        {
            monster.searchPosition = new Vector3(monster.targetLocation.position.x, monster.targetLocation.position.y, 0f);

            monster.monsterSearchState.currentSubState = MonsterSearchState.SearchingSubState.CheckingLastPosition; 
            monster.currentState = monster.monsterSearchState;
        }
        else
        {
            Debug.Log("Player disabled, transitioning to patrol state.");
        }
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
