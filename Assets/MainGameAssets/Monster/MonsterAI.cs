using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{

    public AudioSource patrolSound;
    public AudioSource transitionSound;

    //Keep in mind that the area of our maze is roughly 500x500

    [SerializeField] public float walkListeningDistance;
    [SerializeField] public float runListeningDistance;
    [SerializeField] public int walkSoundStrength;
    [SerializeField] public int runSoundStrength;


    [HideInInspector]
    public SoundManager soundManager;
    [SerializeField]
    public AudioClip monsterPatrolSound;
    [SerializeField]
    public AudioClip monsterRoar;

    [SerializeField]
    public float patrolSpeed;
    [SerializeField]
    public float chaseSpeed;
    [SerializeField]
    public float searchSpeed;

    [SerializeField]
    public float damageOnContact;

    //How long should we search furniture in seconds?
    [SerializeField]
    public float maxFurnitureSearchTime = 2f;
    [HideInInspector]
    public float furnitureSearchTime = 0f;
    [SerializeField]
    public float maxDistractionTime = 3f;
    [HideInInspector]
    public float distractionTime = 0f;
    //This is a radius in which we'll check for furniture. For each item
    //in this radius, we'll cast a ray to see if its in our sightline. If so,
    //then if we're in the searching state, we'll investigate it.
    [SerializeField]
    public float furnitureSearchRadius = 40f;

    //All Monster states
    [HideInInspector]
    public MonsterPatrolState monsterPatrolState;
    [HideInInspector]
    public MonsterSearchState monsterSearchState;
    [HideInInspector]
    public MonsterChaseState monsterChaseState;
    [HideInInspector]
    public MonsterDistractionState monsterDistractionState;

    [HideInInspector]
    public IMonsterState currentState;

    //List of patrol points, to be added via the inspector. Alternatively, we
    //could have a list of vectors and set that in Start() or Awake().
    [SerializeField]
    public List<GameObject> patrolNodes;
    [HideInInspector]
    public int currentNode = 0;

    //List of patrol nodes for a local patrol, where we investigate a smaller area
    [SerializeField]
    public List<GameObject> localPatrolNodes;
    [HideInInspector]
    public int currentLocalNode = 0;

    [SerializeField]
    public GameObject targetActual;
    private Player player;
    [HideInInspector]
    public Transform targetLocation;
    [HideInInspector]
    public Vector3 searchPosition;

    //This links us to our navmesh proxy, which controls navigation across rooms
    //and from point A to B (A being our position, B being our destination).
    [SerializeField]
    GameObject proxy;
    [HideInInspector]
    public Transform proxyLocation;
    [HideInInspector]
    public NavMeshAgent agent;

    [HideInInspector]
    private Animator anim;

    [HideInInspector]
    public GameObject mainCamera;

    [HideInInspector]
    public float roomDimensionsX;
    [HideInInspector]
    public float roomDimensionsY;

    //TODO make sure this room isn't patrolable
    [SerializeField]
    public GameObject roomManager;
    [HideInInspector]
    public GameObject safeRoom;
    [HideInInspector]
    public int safeRoomX;
    [HideInInspector]
    public int safeRoomY;

    [SerializeField]
    public bool debugInfo = false;

    private float stuckTime = 0f;
    private float stuckTimeLimit = 7f;
    private float stuckCheckTime = 0;
    private float stuckCheckTimeMax = 5f;
    private Vector3 stuckPosition = new Vector3();

    //This is a hacky way of hopefully making sure we don't get stuck too long.
    private void getUnstuck()
    {

        stuckCheckTime += Time.smoothDeltaTime;

        if (stuckCheckTime >= stuckCheckTimeMax)
        {
            stuckPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            stuckCheckTime = 0f;

        }

        if (transform.position == stuckPosition)
        {
            stuckTime += Time.smoothDeltaTime;

            if (debugInfo)
                Debug.Log("Stationary for " + stuckTime + " seconds.");
            
        } else
        {
            stuckTime = 0f;
        }
        

        

        if (stuckTime >= stuckTimeLimit)
        {
            SetDestination();
            stuckTime = 0f;
        }
        
    }

    void Awake()
    {
        monsterPatrolState = new MonsterPatrolState(this);
        monsterSearchState = new MonsterSearchState(this);
        monsterChaseState = new MonsterChaseState(this);
        monsterDistractionState = new MonsterDistractionState(this);

        anim = GetComponentInChildren<Animator>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();


        //This is just so we don't have to write GetComponent a lot later.
        proxyLocation = proxy.GetComponent<Transform>();
        agent = proxy.GetComponent<NavMeshAgent>();
        targetLocation = targetActual.GetComponent<Transform>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        currentState = monsterPatrolState;

        player = targetActual.GetComponent<Player>();

    }

    void Start()
    {
        GameObject testNode = patrolNodes[0];
        //AddPatrolPoint(testNode);
        //SetDestination();

        patrolSound.clip = monsterPatrolSound;
        SetDestination();

        //Get the dimensions of the standard room.

        roomDimensionsY = mainCamera.GetComponent<Camera>().orthographicSize * 2f;
        roomDimensionsX = roomDimensionsY * mainCamera.GetComponent<Camera>().aspect;
    }



    void Update()
    {

        UpdateSprite();
        getUnstuck();

        //Our movement is controlled by the navmesh agent. This keeps our positions synced up.
        GetComponent<Transform>().position = new Vector3(proxyLocation.position.x, proxyLocation.position.z, 0f);

        //Update our current state, whatever it may be.
        currentState.UpdateState();

        if (debugInfo && Input.GetKeyDown(KeyCode.Q))
        {
            DebugInfo();
        }

        if (debugInfo)
        {
            Debug.DrawRay(agent.transform.position, agent.destination, Color.red);
        }

        if (agent.pathStatus.ToString() != "PathComplete")
        {
            Debug.Log("Error: Got stuck");
            currentNode++;
            NextLocalPatrolDestination();
            if (currentNode >= patrolNodes.Count)
                currentNode = 0;
            SetDestination();

        }


        //if (localPatrolNodes.Count <= 2)
            Listen();

    }

    

    private void Listen()
    {
        if (player.getMoveState().ToString() == "walk")
        {
            SoundTrigger(targetActual.transform.position, walkListeningDistance, walkSoundStrength);
        }
        if (player.getMoveState().ToString() == "run")
        {
            SoundTrigger(targetActual.transform.position, runListeningDistance, runSoundStrength);
        }
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
            if (debugInfo) Debug.Log("Player caught");
            mainCamera.GetComponent<healthbar>().TakeDemage(damageOnContact);
        }
    }

    public bool TargetIsVisible()
    {

        //TODO player mustn't be in safe room.
        // && NearPlayer() && PlayerInSafeZone()

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetLocation.position - transform.position);
        if (hit.collider != null && hit.collider.tag == "Player" && NearPlayer() && !PlayerInSafeZone() && !hit.collider.GetComponent<Player>().IsInvisible())
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

    public bool AddPatrolPoint(GameObject point)
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

        //Debug.Log(agent.velocity.normalized);
        if (proxy.transform.rotation.eulerAngles.y < 40)
        {
            anim.SetInteger("Direction", 0);
        }
        else if (proxy.transform.rotation.eulerAngles.y < 140)
        {
            anim.SetInteger("Direction", 1);
        }
        else if (proxy.transform.rotation.eulerAngles.y < 220)
        {
            anim.SetInteger("Direction", 2);
        }
        else if (proxy.transform.rotation.eulerAngles.y < 320)
        {
            anim.SetInteger("Direction", 3);
        }
        else
        {
            anim.SetInteger("Direction", 0);
        }


        transform.GetChild(0).transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y % 50 * .02f);
    }

    #region Monster Destination Functions
    public void SetDestination()
    {


        if (currentState == monsterPatrolState)
        {

            if (localPatrolNodes.Count > 0)
            {
                Debug.Log("LOCAL PATROL");
                SetLocalPatrolDestination();
            }
            else
            {
                SetPatrolDestination();
            }


        }
        else if (currentState == monsterSearchState)
        {
            SetSearchDestination();

        }
        else if (currentState == monsterChaseState)
        {

            agent.destination = new Vector3(targetLocation.position.x, proxyLocation.position.y, targetLocation.position.y);

        }
        else
        {
            Debug.Log("Error: Cannot set destination while monster is in " + currentState.ToString());
        }


    }

    private void SetSearchDestination()
    {
        agent.destination = new Vector3(searchPosition.x, proxyLocation.position.y, searchPosition.y);
    }

    private void SetLocalPatrolDestination()
    {

        agent.destination = new Vector3(localPatrolNodes[currentLocalNode].transform.position.x, proxyLocation.position.y, localPatrolNodes[currentLocalNode].transform.position.y);

    }

    public void NextLocalPatrolDestination()
    {

        if (localPatrolNodes.Count == 0)
            return;

        if (localPatrolNodes.Contains(localPatrolNodes[currentLocalNode]))
            localPatrolNodes.Remove(localPatrolNodes[currentLocalNode]);

        if (localPatrolNodes.Count == 0)
            return;

        currentLocalNode = Random.Range(0, localPatrolNodes.Count - 1);
        if (currentLocalNode < 0)
            Debug.Log("Error: current node cannot be zero");



        agent.destination = new Vector3(localPatrolNodes[currentLocalNode].transform.position.x, proxyLocation.position.y, localPatrolNodes[currentLocalNode].transform.position.y);

    }

    private void SetPatrolDestination()
    {
        if (patrolNodes[currentNode].tag == "StartNode")
        {

            if (debugInfo)
                Debug.Log("Exception: Starting Node");


            StartCoroutine(DestinationHelper());
        }
        else
        {
            agent.destination = new Vector3(patrolNodes[currentNode].transform.position.x, proxyLocation.position.y, patrolNodes[currentNode].transform.position.y);

            if (debugInfo)
            {
                Debug.Log("New Destination Proxy: " + agent.destination + "\nNew Destination Sprite: (" + patrolNodes[currentNode].transform.position.x + ", " + patrolNodes[currentNode].transform.position.y + ")");
            }

        }

        /*
        if (currentNode == 1)
        {
            patrolNodes.RemoveAt(0);
        }*/
    }

    private IEnumerator DestinationHelper()
    {
        yield return new WaitForSeconds(1f);

        currentNode++;
        patrolNodes.RemoveAt(0);


        agent.destination = new Vector3(patrolNodes[currentNode].transform.position.x, proxyLocation.position.y, patrolNodes[currentNode].transform.position.y);

        if (debugInfo)
            Debug.Log("New Destination Proxy: " + agent.destination + "\nNew Destination Sprite: (" + patrolNodes[currentNode].transform.position.x + ", " + patrolNodes[currentNode].transform.position.y + ")");
    }
    #endregion

    public void DebugInfo()
    {

        Debug.Log("Current State" + currentState.ToString() + "\nPlayer Room Position: (" + GetPlayerRoomX() + ", " + GetPlayerRoomY() + ")");
        //Debug.Log("TEST: Player move state = " + player.getMoveState().ToString());

        SoundTrigger(targetActual.transform.position, 500f, 2); //TODO remove

        //Debug.Log("Player Room Position: (" + GetPlayerRoomX() + ", " + GetPlayerRoomY() + ")");
        //Debug.Log("Next to player: " + NearPlayer());

        if (currentState.ToString() == "MonsterPatrolState")
        {
            Debug.Log("Current Proxy Target: " + agent.destination + ", Path Status: " + agent.path.status.ToString());

        }
        if (currentState.ToString() == "MonsterSearchState")
        {
            Debug.Log("Current Proxy Target: " + agent.destination + ", searchPosition: " + searchPosition);
        }
        //mainCamera.GetComponent<UI>().ResetGame();

    }

    //We want to make sure that the monster proxy will avoid the start room
    public void BlockRoom(GameObject roomToBlock)
    {
        NavigationManager navManagement = GameObject.FindGameObjectWithTag("NavigationManager").GetComponent<NavigationManager>();

        GameObject obstacleRepObj = GameObject.FindGameObjectWithTag("SolidObject");

        //TODO: roomToBlock.transform.position.y - Fix the third parameter of representationLocation, so it works with any starting room
        Vector3 representationLocation = new Vector3(roomToBlock.transform.position.x, navManagement.GetObstacleOffset(), -25f);
        Vector3 representationScale = new Vector3(roomDimensionsX, 1f, roomDimensionsY);


        GameObject obstacleRep = Instantiate(obstacleRepObj);
        obstacleRep.GetComponent<Transform>().position = representationLocation;
        obstacleRep.GetComponent<Transform>().localScale = representationScale;

        //performance increase?
        obstacleRep.GetComponent<MeshRenderer>().enabled = false;

        obstacleRep.name = "Safe Room Obstacle";
        //Debug.Log("Intention" + representationLocation + "\nActual: " + obstacleRep.transform.position);
    }
    #region Roomspace Coordinates
    public int GetRoomX()
    {
        for (int i = 0; i < patrolNodes.Count; i++)
        {
            if (Mathf.Abs(transform.position.x - patrolNodes[i].transform.position.x) <= (roomDimensionsX / 2f))
            {
                //Debug.Log("Dist: " + (transform.position.x - patrolNodes[i].transform.position.x) + "\nDimensionX: " + roomDimensionsX / 2f);
                return i;
            }
        }



        return -1;
    }

    public int GetRoomY()
    {
        int yCount = 0;

        for (int i = 0; i < patrolNodes.Count - 1; i++)
        {


            if (Mathf.Abs(transform.position.y - patrolNodes[i].transform.position.y) <= (roomDimensionsY / 2f))
            {
                //Debug.Log("Dist: " + (transform.position.y - patrolNodes[i].transform.position.y) + "\nDimensionX: " + roomDimensionsY / 2f);
                return yCount;
            }

            if (patrolNodes[i + 1].transform.position.y < patrolNodes[i].transform.position.y)
            {
                yCount++;
            }
        }


        return -1;
    }

    //TODO fix it being 10
    public int GetPlayerRoomX()
    {
        //int xCount = 0;

        for (int i = 0; i < patrolNodes.Count - 1; i++)
        {
            if (Mathf.Abs(targetActual.transform.position.x - patrolNodes[i].transform.position.x) <= (roomDimensionsX / 2f))
            {
                //Debug.Log("Dist: " + (targetActual.transform.position.x - patrolNodes[i].transform.position.x) + "\nDimensionX: " + roomDimensionsX / 2f);
                return i;
            }


        }


        return -1;
    }

    public int GetPlayerRoomY() //TODO fix y
    {

        int yCount = 0;

        for (int i = 0; i < patrolNodes.Count - 1; i++)
        {


            if (Mathf.Abs(targetActual.transform.position.y - patrolNodes[i].transform.position.y) <= (roomDimensionsY / 2f))
            {
                //Debug.Log("Dist: " + (targetActual.transform.position.y - patrolNodes[i].transform.position.y) + "\nDimensionX: " + roomDimensionsY / 2f);
                return yCount;
            }

            if (patrolNodes[i + 1].transform.position.y < patrolNodes[i].transform.position.y)
            {
                yCount++;
            }
        }


        return -1;
    }


    //Is the monster either in the same room, or in an adjacent room to the
    //player? If so, return true. This is so we know just how far away the
    //monster is from the player. Used in the visibilty function, to make
    //sure that we don't see the player from several rooms away
    public bool NearPlayer()
    {
        if ((Mathf.Abs(GetRoomX() - GetPlayerRoomX()) <= 1) &&
            (Mathf.Abs(GetRoomY() - GetPlayerRoomY()) <= 1))
        {
            return true;
        }
        else
        {
            return false;
        }



    }

    public bool PlayerInSafeZone()
    {
        if (GetPlayerRoomX() == safeRoomX && GetPlayerRoomY() == safeRoomY)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    #endregion



    public void ResetActors()
    {
        //if (GetPlayerRoomX() != safeRoomX || GetPlayerRoomY() != safeRoomY)
        //mainCamera.GetComponent<CameraScript>().TransitionRoom(safeRoomX); //TODO fix this for modularity

        //TODO fix this to fit with room size
        //mainCamera.GetComponent<CameraScript>().TransitionRoom(0);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(2);

        mainCamera.GetComponent<CameraScript>().TransitionRoom(3);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(3);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(3);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(3);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(3);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(3);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(3);


        mainCamera.GetComponent<CameraScript>().TransitionRoom(1);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(1);
        mainCamera.GetComponent<CameraScript>().TransitionRoom(1);

        proxy.transform.position = new Vector3(-75f, -898f, -10f); //TODO set this to a proper starting location.
        targetActual.transform.position = new Vector3(safeRoom.transform.position.x, safeRoom.transform.position.y, -0.5f);
        Debug.Log("TEST: " + new Vector3(safeRoom.transform.position.x, safeRoom.transform.position.y, -0.5f));
        mainCamera.GetComponent<healthbar>().FullHealth();

    }

    //Ideally, sound strength should be about 1-3 at most
    public void SoundTrigger(Vector3 soundLocation, float soundRange, int soundStrength)
    {
        //First, check if we're out of range of the sound. If so, return.
        float distance = (soundLocation - transform.position).magnitude;
        if (distance > soundRange)
        {
            return;
        }

        //Second, check if we heard the sound, and are in the next room, in which
        //case we investigate that room.
        if (NearPlayer())
        {
            PopulateLocalNodes(0, GetPlayerRoomX(), GetPlayerRoomY());
        }

        //Third, we aren't right by the player, but we did hear the sound, so
        //we investigate rooms in an area around the player.
        //TODO set size based on sound strength
        PopulateLocalNodes(soundStrength, GetPlayerRoomX(), GetPlayerRoomY());
        if (GetPlayerRoomX() == -1 || GetPlayerRoomY() == -1)
        {
            Debug.Log("Error: Player map coordinates at (" + GetPlayerRoomX() + ", " + GetPlayerRoomY() + ")");
            return;
        }
        if (currentState.ToString() == "MonsterPatrolState")
            SetDestination();

    }



    //areaSize given in number of rooms to add to the local patrol. Input 0 for a 1x1 room,
    //1 for a 3x3 room, 2 for a 5x5 room, etc. You  might think of areaSize as a radius. It
    //will always go out from a conter room (roomX,roomY, where the sound originated).
    private void PopulateLocalNodes(int areaSize, int roomX, int roomY)
    {
        


        localPatrolNodes.Clear();

        int numCollumns = roomManager.GetComponent<RoomManager>().collumns;
        int numRows = roomManager.GetComponent<RoomManager>().rows;

        int nodeCount = 0;

        for (int i = 0; i < roomY; i++)
        {
            nodeCount += roomManager.GetComponent<RoomManager>().collumns;

        }
        nodeCount += roomX;

        //Debug.Log("Local Node Count: " + nodeCount + ", room location: " + patrolNodes[nodeCount].transform.position);
        if (roomX < 0 || roomY < 0 || roomY > numRows || roomX > numCollumns || areaSize < 0)
        {
            Debug.Log("Error: Room XY parameters out of bounds.");
            return;
        }


        localPatrolNodes.Add(patrolNodes[nodeCount]);




        for (int i = 0; i <= areaSize; i++)
        {

            //First, x-axis
            if ((roomX - i) >= 0)
            {
                PopulateLocalNodesHelper(patrolNodes[nodeCount - i]);
            }


            if (roomX + i < numCollumns)
            {
                PopulateLocalNodesHelper(patrolNodes[nodeCount + i]);
            }

            if ((roomY - i) >= 0)
            {
                PopulateLocalNodesHelper(patrolNodes[nodeCount - (numRows * i)]);

                for (int j = 0; j <= areaSize; j++)
                {

                    if ((roomX - j) >= 0)
                        PopulateLocalNodesHelper(patrolNodes[nodeCount - (numRows * i) - j]);

                    if ((roomX + j) < numRows)
                        PopulateLocalNodesHelper(patrolNodes[nodeCount - (numRows * i) + j]);
                }

            }

            if ((roomY + i) < numRows)
            {
                PopulateLocalNodesHelper(patrolNodes[nodeCount + (numRows * i)]);

                for (int j = 0; j <= areaSize; j++)
                {

                    if ((roomX - j) >= 0)
                        PopulateLocalNodesHelper(patrolNodes[nodeCount + (numRows * i) - j]);

                    if ((roomX + j) < numRows)
                        PopulateLocalNodesHelper(patrolNodes[nodeCount + (numRows * i) + j]);
                }
            }
        }


        //And, of course, make sure we don't patrol the safe room.
        if (localPatrolNodes.Contains(safeRoom))
            localPatrolNodes.Remove(safeRoom);

    }

    private void PopulateLocalNodesHelper(GameObject itemToAdd)
    {
        if (!localPatrolNodes.Contains(itemToAdd))
            localPatrolNodes.Add(itemToAdd);
    }
}