using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour {


    [SerializeField] private GameObject navigationPlane;
    private float navigationObstacleOffset;

	void Awake () {
        //This is assuming all navigation obstacles will have a y scale of 1
        navigationObstacleOffset = navigationPlane.transform.position.y + 1;
	}
	
	public float GetObstacleOffset()
    {
        return navigationObstacleOffset;
    }
}
