using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public Texture2D[] RoomImages;
    public Texture2D[] CornerRoomImages;
    public Texture2D startingRoom;

    public float sizeModifier;
    public int collumns = 5;
    public int rows = 5;
    public int actRow = 0;
    public int actCol = 2;

    public int CookieCount;
    public int PotionCount;

    public GameObject Room;

    private GameObject[][] Rooms;

    public GameObject Player;

    // Use this for initialization
    void Start()
    {
        Rooms = new GameObject[rows][];

        for (int i = 0; i < rows; i++)
        {
            Rooms[i] = new GameObject[collumns];
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < collumns; j++)
            {
                Rooms[i][j] = Instantiate(Room);
                string s = i + " " + j;
                Rooms[i][j].name = s;

                if (i == actRow && j == actCol) //starting room
                {
                    Rooms[i][j].GetComponent<RoomScript>().init(1, startingRoom);
                }
                else if (i == 0 && j == 0) //top left corner
                {
                    int num = Random.Range(0, CornerRoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(5, CornerRoomImages[num]);
                }
                else if (i == 0 && j == collumns - 1) //top right corner
                {
                    int num = Random.Range(0, CornerRoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(6, CornerRoomImages[num]);
                }
                else if (i == rows - 1 && j == 0) //bottom left corner
                {
                    int num = Random.Range(0, CornerRoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(7, CornerRoomImages[num]);
                }
                else if (i == rows - 1 && j == collumns - 1) //bottom right corner
                {
                    int num = Random.Range(0, CornerRoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(8, CornerRoomImages[num]);
                }
                else if (i == 0) //top row
                {
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(1, RoomImages[num]);
                }
                else if (i == rows - 1) //bottom row
                {
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(3, RoomImages[num]);
                }
                else if (j == 0) //left collumn
                {
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(4, RoomImages[num]);
                }
                else if (j == collumns - 1) //right collumn
                {
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(2, RoomImages[num]);
                }
                else //middle room
                {
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(0, RoomImages[num]);
                }

                //TODO Commented out section is original, uncomment and delete lines below to return to original condition
                //Rooms[i][j].transform.position = new Vector3((j - 2) * 1500, i * -1000, 0);
                Rooms[i][j].transform.position = new Vector3(((j - 2) * 1500) * sizeModifier, (i * -1000) * sizeModifier, 0);

                //Add all rooms to our monster's patrol route. TODO - Remove safe room.
                /*
                Vector3 patrolPoint = new Vector3(Rooms[i][j].transform.position.x, Rooms[i][j].transform.position.y, 0f);
                if (patrolPoint != null && (Rooms[i][j] != Rooms[0][4])) //Make sure the room we're adding isn't the safe room (starting)
                {
                   GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().AddPatrolPoint(patrolPoint);
                }*/
                GameObject patrolPoint = Rooms[i][j];
                if (patrolPoint != null) //Make sure the room we're adding isn't the safe room (starting)
                {
                    if (i == actRow && j == actCol)
                    {
                        GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().safeRoomX = j;
                        GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().safeRoomY = i;
                        GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().safeRoom = Rooms[i][j];
                        GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().AddPatrolPoint(patrolPoint);
                        GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().BlockRoom(Rooms[i][j]);
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().AddPatrolPoint(patrolPoint);
                    }

                }
            }
        }

        placeKeys();
        placeCookies();
        placePotions();
        Rooms[actRow][actCol].GetComponent<RoomScript>().placeEndDoor();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < collumns; j++)
            {

                //TODO Commented out section is original, uncomment and delete lines below to return to original condition
                //Rooms[i][j].transform.position = new Vector3((j - 2) * 1500, i * -1000, 0);
                Rooms[i][j].transform.position = new Vector3(((j - 2) * 1500) * sizeModifier, (-500 + i * -1000) * sizeModifier, 0);

                if (i == actRow && j == actCol)
                {
                    //RoomScript.DisableRoom(i, j); todo?
                    Rooms[i][j].SetActive(true);
                    Player.transform.position = new Vector3(Rooms[i][j].transform.position.x, Rooms[i][j].transform.position.y, -1);
                }
                else
                {
                    Rooms[i][j].GetComponent<RoomScript>().DisableRoom();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableRoom(int i, int j)
    {
        Rooms[i][j].GetComponent<RoomScript>().DisableRoom();
    }

    public void EnableRoom(int i, int j)
    {
        Rooms[i][j].GetComponent<RoomScript>().EnableRoom();
        actCol = j;
        actRow = i;
    }

    public int getRows() { return rows; }
    public int getCollumns() { return collumns; }
    public int getActiveRow() { return actRow; }
    public int getActiveCol() { return actCol; }
    public Transform getRoomTransform(int i, int j)
    {
        return Rooms[i][j].transform;
    }

    void placeKeys()
    {
        placeRandom(0, rows / 2, 0, collumns / 2, "Key"); //top left
        placeRandom(rows/2, rows, 0, collumns / 2, "Key"); //bottom left
        placeRandom(0, rows / 2, collumns / 2, collumns, "Key"); //top right
        placeRandom(rows/2, rows, collumns/2, collumns, "Key"); //bottom right
    }

    void placeCookies()
    {
        for(int i = 0; i < CookieCount; i++)
        {
            placeRandom(0, rows, 0, collumns, "Cookie");
        }
    }

    void placePotions()
    {
        for (int i = 0; i < PotionCount; i++)
        {
            placeRandom(0, rows, 0, collumns, "Potion");
        }
    }

    private void placeRandom(int startRow, int endRow, int startCol, int endCol, string name)
    {
        bool canPlace = false;
        int row = 0;
        int col = 0;

        while (canPlace == false)
        {
            row = Random.Range(startRow, endRow);
            col = Random.Range(startCol, endCol);
            if (Rooms[row][col].GetComponent<RoomScript>().canPlaceItem())
            {
                canPlace = true;
            }
        }

        if (name == "Cookie")
        {
            Rooms[row][col].GetComponent<RoomScript>().placeCookie();
        }
        if (name == "Key")
        {
            Rooms[row][col].GetComponent<RoomScript>().placeKey();
        }
        if (name == "Potion")
        {
            Rooms[row][col].GetComponent<RoomScript>().placePotion();
        }
    }

}
