using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState {

    void UpdateState();
    void ToMonsterPatrolState();
    void ToMonsterChaseState();
    void ToMonsterSearchState();
    void ToMonsterDistractionState(Collider2D other);
}
