using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour {
    


    //How long should we search furniture in seconds?
    [SerializeField] public float maxFurnitureSearchTime = 2f;
    [HideInInspector] public  float furnitureSearchTime = 0f;
    [SerializeField] public float maxDistractionTime = 3f;
    [HideInInspector] public  float distractionTime = 0f;
    //This is a radius in which we'll check for furniture. For each item
    //in this radius, we'll cast a ray to see if its in our sightline. If so,
    //then if we're in the searching state, we'll investigate it.
    [SerializeField] public float furnitureSearchRadius = 40f;

    //All Monster states
    [HideInInspector] public MonsterPatrolState monsterPatrolState;
	[HideInInspector] public MonsterSearchState monsterSearchState;
	[HideInInspector] public MonsterChaseState monsterChaseState;
	[HideInInspector] public MonsterDistractionState monsterDistractionState;

    [HideInInspector] public IMonsterState currentState;

    //List of patrol points, to be added via the inspector. Alternatively, we
    //could have a list of vectors and set that in Start() or Awake().
    [SerializeField] public List<Vector3> patrolNodes;
    [HideInInspector] public int currentNode = 0;

    [SerializeField] public GameObject targetActual;
    [HideInInspector] public Transform targetLocation;
    [HideInInspector] public Vector3 searchPosition;

    //This links us to our navmesh proxy, which controls navigation across rooms
    //and from point A to B (A being our position, B being our destination).
    [SerializeField] GameObject proxy;
    [HideInInspector] public Transform proxyLocation;
    [HideInInspector] public NavMeshAgent agent;

    [HideInInspector] public GameObject mainCamera;

    void Awake() {
        monsterPatrolState = new MonsterPatrolState(this);
        monsterSearchState = new MonsterSearchState(this);
        monsterChaseState = new MonsterChaseState(this);
        monsterDistractionState = new MonsterDistractionState(this);

        //This is just so we don't have to write GetComponent a lot later.
        proxyLocation = proxy.GetComponent<Transform>();
        agent = proxy.GetComponent<NavMeshAgent>();
        targetLocation = targetActual.GetComponent<Transform>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        //Start in patrol mode.
        agent.destination = new Vector3(patrolNodes[currentNode].x, proxyLocation.position.y, patrolNodes[currentNode].y);
        currentState = monsterPatrolState;


        
    }

    void Update()
    {

        UpdateSprite();

        //Our movement is controlled by the navmesh agent. This keeps our positions synced up.
        GetComponent<Transform>().position = new Vector3(proxyLocation.position.x, proxyLocation.position.z, 0f);

        //Update our current state, whatever it may be.
        currentState.UpdateState();

        //Debug.Log("CurrentState = " + currentState.ToString() + "\nSubstate: " + monsterSearchState.currentSubState.ToString());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //If we encounter a distraction, we exit our previous state,
        //no matter what that state was, and go to the Distraction state
        if (other.tag == "Distraction" && currentState != monsterDistractionState)
        {
            currentState.ToMonsterDistractionState(other);
            return;
        }

        
    }

   private void OnTriggerStay2D(Collider2D collision)
   {
      //If we encounter the player, the game should reset. For now, we affect health.
      if (collision.tag == "Player")
      {
         //TODO reset game.
         Debug.Log("Player caught");
         mainCamera.GetComponent<healthbar>().TakeDemage(1);
      }
   }

   public bool TargetIsVisible()
    {
        

        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetLocation.position - transform.position);
        if (hit.collider != null && hit.collider.tag == "Player")
        {
            Debug.DrawRay(transform.position, targetLocation.position - transform.position, Color.red);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, targetLocation.position - transform.position, Color.blue);
            return false;
        }
    }

    public bool AddPatrolPoint(Vector3 point)
    {

        if (patrolNodes.Contains(point))
        {
            return false;
        }
        else
        {
            patrolNodes.Add(point);
            return true;
        }
    }

    private void UpdateSprite()
    {
        //Just make sure we're facing the right direction (or the left one)
        if (agent.desiredVelocity.x > 0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

}
