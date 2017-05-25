using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidObject : MonoBehaviour {

    [SerializeField] private GameObject obstacleRepObj;
    private NavigationManager navManagement;

    private Vector3 representationLocation;
    private Vector3 representationScale;
    private Vector3 representationOffset;
	
    void Awake()
    {
        navManagement = GameObject.FindGameObjectWithTag("NavigationManager").GetComponent<NavigationManager>();

        obstacleRepObj = GameObject.FindGameObjectWithTag("SolidObject");
    }

	void Start()
    {
        representationLocation = new Vector3(GetComponent<Transform>().position.x, navManagement.GetObstacleOffset(),GetComponent<Transform>().position.y);

        //Use the first line if you want the monster to avoid it based on its collider size (rectangle), and the second if you want them to avoid it
        //based on its sprite (also as a rectangle). Ideally, they should have the same result.
        representationScale = new Vector3(GetComponent<Collider2D>().bounds.size.x, 1f, GetComponent<Collider2D>().bounds.size.y);
        //representationScale = new Vector3(GetComponent<SpriteRenderer>().bounds.size.x, 1f, GetComponent<SpriteRenderer>().bounds.size.y);

        GameObject obstacleRep = Instantiate(obstacleRepObj);
        obstacleRep.GetComponent<Transform>().position = representationLocation;
        obstacleRep.GetComponent<Transform>().localScale = representationScale;

        //performance increase?
        obstacleRep.GetComponent<MeshRenderer>().enabled = false;

    }
}
