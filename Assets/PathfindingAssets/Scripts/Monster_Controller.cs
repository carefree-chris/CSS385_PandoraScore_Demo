using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Controller : MonoBehaviour {

    //How long should we search furniture in seconds?
    [SerializeField] private float maxSearchTime = 2f;
    private float searchTime = 0f;
    [SerializeField] private float maxDistractionTime = 3f;
    private float distractionTime = 0f;

    

    //For searching furniture
    //public LayerMask furnitureLayers = new LayerMask();
    private Collider2D[] potentialFurniture = null;
    private List<Collider2D> realFurniture;
    private Collider2D[] finalFurniture;
    private int furnitureIndex = 0;

    //This links us to our navmesh proxy
    [SerializeField] GameObject proxy;
    private Transform proxyLocation;
    private NavMeshAgent agent;

    //List of patrol points, to be added via the inspector. Alternatively, we
    //could have a list of vectors and set that in Start() or Awake().
    [SerializeField] private Transform[] patrolNodes;
    private int currentNode = 0;

    [SerializeField] private GameObject targetActual;
    private Transform targetLocation;
    private Vector3 searchPosition;

    //Is the player clearly in our line of sight?
    public bool targetIsVisible = false;

    //For debugging - TODO remove
    private int numberOfFurniture = 0;
    private bool playerInvisible = false;
    private TextMesh debugText;
    private string CurrentStateText = "";
    private string CurrentSubstateText = "SubState: None";
    private string PlayerVisibleText = "";

    //HFSM Stuff
    private MonsterState currentState;
    private SearchingSubState currentSearchingState;
    
    //Monster states
    protected enum MonsterState
    {
        Patrolling,
        Chasing,
        Searching,
        Distracted

    }

    //Substates of Searching
    protected enum SearchingSubState
    {
        CheckingLastPosition,
        CheckingFurniture
    }


    void Awake()
    {
        //This is just so we don't have to write GetComponent a lot later.
        proxyLocation = proxy.GetComponent<Transform>();
        agent = proxy.GetComponent<NavMeshAgent>();
        targetLocation = targetActual.GetComponent<Transform>();
        realFurniture = new List<Collider2D>();

      //Put ourselves in the correct initial state
      //currentState = MonsterState.Patrolling;
      //currentSearchingState = SearchingSubState.CheckingLastPosition;


     //TODO remove w/ text object
         debugText = GetComponentInChildren<TextMesh>();
        debugText.text = "";
        
    }

    void Start()
    {
        //agent.destination = new Vector3(patrolNodes[currentNode].position.x, proxyLocation.position.y, patrolNodes[currentNode].position.y);
        ToPatrolling();
    }

	void Update () {

        //For debugging
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    playerInvisible = !playerInvisible;
        //}

        //TODO remove debug text when no longer needed.
        debugText.text = CurrentStateText + "\n" + CurrentSubstateText + "\n" + PlayerVisibleText;
        
        //Our movement is controlled by the navmesh agent. This keeps our positions synced up.
        GetComponent<Transform>().position = new Vector3(proxyLocation.position.x, proxyLocation.position.z, 0f);
        
        //Just make sure we're facing the right direction (or the left one)
        if (agent.desiredVelocity.x > 0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        } else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        //Shows the ray that we're casting to check for player visibility
        Debug.DrawRay(transform.position, targetLocation.position - transform.position, Color.blue);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetLocation.position - transform.position);
        if (hit.collider.tag == "Player" && !playerInvisible) {
            PlayerVisibleText = "PlayerIsVisible: True";
            targetIsVisible = true;
        } else
        {
            targetIsVisible = false;
            PlayerVisibleText = "PlayerIsVisible: False";
        }
        
        

        //Handle first level of FSM, directing each state to a function of the same name
        switch (currentState)
        {
            case MonsterState.Patrolling:
                CurrentStateText = "Current State: Patrolling";
                CurrentSubstateText = "SubState: None";
                Patrolling();
                break;
            case MonsterState.Chasing:
                CurrentStateText = "Current State: Chasing";
                CurrentSubstateText = "SubState: None";
                Chasing();
                break;
            case MonsterState.Searching:
                CurrentStateText = "Current State: Searching";
                //CurrentSubstateText = "SubState: None";
                Searching();
                break;
            case MonsterState.Distracted:
                CurrentStateText = "Current State: Distracted";
                CurrentSubstateText = "SubState: None";
                Distracted();
                break;

        }
        
	}


    /* BEGINNING OF CODE FOR MONSTER STATES & SUBSTATES */
    #region Monster States (and Substates)


    //Code for first state
    private void Patrolling()
    {

        if (targetIsVisible)
        {
            ToChasing();
            return;
        }

        if (patrolNodes.Length <= 0)
        {
            Debug.Log("Error: Nowhere to patrol.");
            return;
        }
        
        //Once we're close enough to a patrol point, we go to the next
        if ((transform.position - patrolNodes[currentNode].position).magnitude < agent.stoppingDistance)
        {
            currentNode++;
            if (currentNode >= patrolNodes.Length)
                currentNode = 0;
            
            agent.destination = new Vector3(patrolNodes[currentNode].position.x, proxyLocation.position.y, patrolNodes[currentNode].position.y);
        }

    }

    private void Chasing()
    {
        if (targetIsVisible == true)
        {
            agent.destination = new Vector3(targetLocation.position.x, proxyLocation.position.y, targetLocation.position.y);
        }
        else
        {
            ToSearching();
            return;
        }


    }

    private void Distracted()
    {
        distractionTime += Time.smoothDeltaTime;

        //Once the duration of the distraction is over, we go back to patrolling
        if (distractionTime >= maxDistractionTime)
        {
            //ToSearching();
            //searchPosition = transform.position;

            ToPatrolling();
        }
    }

    private void Searching()
    {
        //Debug.Log("In searching still");
        //if (currentState != MonsterState.Searching)
            //return; //TODO fix eternal "searching state" bug


        switch (currentSearchingState)
        {
            case SearchingSubState.CheckingLastPosition:
                CheckingLastPosition();
                CurrentSubstateText = "SubState: Checking last position...";
                break;
            case SearchingSubState.CheckingFurniture:
                CheckingFurniture();
                CurrentSubstateText = "SubState: Checking furniture...";
                break;
        }
        
    }

   

    //A substate of Searching
    private void CheckingLastPosition()
    {
        //Debug.Log("Still checking position");
        agent.destination = searchPosition;

        if (targetIsVisible)
        {
            ToChasing();
            return;
        }
        
        //If we've reached the spot where we last heard/saw the player, check furniture
        if ((transform.position - searchPosition).magnitude < agent.stoppingDistance)
        {

            Collider2D hidingSpot = Physics2D.OverlapCircle(transform.position, 1.5f);
            if (hidingSpot != null && hidingSpot.transform.tag == "Furniture")
            {
                if (!hidingSpot.gameObject.GetComponent<Furniture_Controller>().GetIsEmpty())
                    Debug.Log("Player Found!");
            }

            furnitureIndex = 0;
            searchTime = 0f;
            potentialFurniture = null;
            currentSearchingState = SearchingSubState.CheckingFurniture;
            return;
        }

    }

    #region Checking Furniture Stuff
    //A substate of Searching
    private void CheckingFurniture()
    {
        //TODO - check all furniture within room

        if (targetIsVisible)
        {
            ToChasing();
            return;
        }

        
        if (potentialFurniture == null)
        {
            potentialFurniture = Physics2D.OverlapCircleAll(transform.position, 20f);

            for (int i = 0; i < potentialFurniture.Length; i++)
            {
                //TODO raycast to make sure all furniture is within line of sight
                if (potentialFurniture[i].transform.tag == "Furniture" && (Physics2D.Raycast(transform.position, potentialFurniture[i].transform.position - transform.position).transform.tag == "Furniture"))
                {
                    //Debug.Log("FOUND FURNITURE");
                    realFurniture.Add(potentialFurniture[i]);
                }
            }

            finalFurniture = realFurniture.ToArray();
        }

        if (finalFurniture == null)
            return;

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
        } else
        {
            ToPatrolling();
        }

    }

    //Sends the monster to the given piece of furniture, to check for the player
    private void InvestigateFurniture(Collider2D furniture)
    {
        
        //TODO - Potential bug: Monster goes to center of furniture, and can't see outside of it.
        Vector3 furniturePos = new Vector3(furniture.gameObject.GetComponent<Transform>().position.x, proxyLocation.position.y, furniture.gameObject.GetComponent<Transform>().position.y);
        agent.destination = furniturePos; // - new Vector3(2f,0f,0f);

        if ((furniture.transform.position - transform.position).magnitude < agent.stoppingDistance)
        {

            //TODO check for player
            if (!furniture.gameObject.GetComponent<Furniture_Controller>().GetIsEmpty())
                Debug.Log("Player found!");
            

            searchTime += Time.smoothDeltaTime;

            if (searchTime >= maxSearchTime)
            {
                searchTime = 0f;
                furnitureIndex++;
                return;
                
            }
                
        } else
        {
            //Debug.Log("should be travelling");
        }

        

    }
    #endregion

    #endregion
    /* END OF CODE FOR MONSTER STATES & SUBSTATES */

    /* STATE TRANSITION FUNCTIONS - Exist in case we have to change variables
     *  other than just the MonsterState upon transition */

    private void ToPatrolling()
    {

        agent.destination = new Vector3(patrolNodes[currentNode].position.x, proxyLocation.position.y, patrolNodes[currentNode].position.y);

        currentState = MonsterState.Patrolling;
        //CurrentStateText = "Current State: Patrolling";
        //CurrentSubstateText = "SubState: None";
    }

    private void ToChasing()
    {
        
        currentState = MonsterState.Chasing;
        //CurrentStateText = "Current State: Chasing";
        //CurrentSubstateText = "SubState: None";

    }

    private void ToSearching()
    {
        //Reset all relevant variables, including the time we've been searching
        //(now zero), and where we last saw/heard the player.
        
        searchPosition = new Vector3(targetLocation.position.x, targetLocation.position.y, 0f);

        currentState = MonsterState.Searching;
        currentSearchingState = SearchingSubState.CheckingLastPosition;

        //CurrentStateText = "Current State: Searching";
        //CurrentSubstateText = "SubState: None";
    }

    private void ToDistracted(Collider2D other)
    {
        distractionTime = 0f;
        currentState = MonsterState.Distracted;
        agent.destination = new Vector3(other.GetComponent<Transform>().position.x, proxyLocation.position.y, other.GetComponent<Transform>().position.y);
        //CurrentStateText = "Current State: Distracted";
        //CurrentSubstateText = "SubState: None";
        Destroy(other.gameObject, maxDistractionTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //If we encounter a distraction, we exit our previous state,
        //no matter what that state was, and go to the Distraction state
        if (other.tag == "Distraction")
        {
            ToDistracted(other);
            return;
        }

        //If we encounter the player, the game should reset. Nothing for now.
        if (other.tag == "Player")
        {
            //Debug.Log("if (debugInfo) if (debugInfo) Debug.Log("Player caught");");
        }


        if (other.tag == "Furniture")
        {
            //Debug.Log("Furniture!");
        }
    }
}
