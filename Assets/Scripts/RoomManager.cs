using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public Texture2D[] RoomImages;
    public Texture2D startingRoom;

    public float sizeModifier;
    public int collumns = 5;
    public int rows = 5;
    public int actRow = 0;
    public int actCol = 2;

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
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(5, RoomImages[num]);
                }
                else if (i == 0 && j == collumns - 1) //top right corner
                {
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(6, RoomImages[num]);
                }
                else if (i == rows - 1 && j == 0) //bottom left corner
                {
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(7, RoomImages[num]);
                }
                else if (i == rows - 1 && j == collumns - 1) //bottom right corner
                {
                    int num = Random.Range(0, RoomImages.Length);
                    Rooms[i][j].GetComponent<RoomScript>().init(8, RoomImages[num]);
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
                  Vector3 patrolPoint = new Vector3(Rooms[i][j].transform.position.x, Rooms[i][j].transform.position.y, 0f);
                  if (patrolPoint != null && (Rooms[i][j] != Rooms[0][4])) //Make sure the room we're adding isn't the safe room (starting)
                  {
                     GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterAI>().AddPatrolPoint(patrolPoint);
                  }
           }
        }

        placeKeys();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < collumns; j++)
            {

                //TODO Commented out section is original, uncomment and delete lines below to return to original condition
                //Rooms[i][j].transform.position = new Vector3((j - 2) * 1500, i * -1000, 0);
                Rooms[i][j].transform.position = new Vector3(((j - 2) * 1500) * sizeModifier, (i * -1000) * sizeModifier, 0);

                if (i == actRow && j == actCol)
                {
                    //RoomScript.DisableRoom(i, j); todo?
                    Rooms[i][j].SetActive(true);
                    Player.transform.position = new Vector3(Rooms[i][j].transform.position.x, Rooms[i][j].transform.position.y, -1);
                }
                else
                {
                    Rooms[i][j].SetActive(false);
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
        Rooms[i][j].SetActive(false);
    }

    public void EnableRoom(int i, int j)
    {
        Rooms[i][j].SetActive(true);
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
        int row;
        int col;

        row = Random.Range(0, rows / 2);
        if (row == 0)
        {
            col = Random.Range(0, actCol - 2);
        }
        else
        {
            col = Random.Range(0, actCol - 1);
        }

        Rooms[row][col].GetComponent<RoomScript>().placeKey();

        row = Random.Range(0, rows / 2);
        if (row == 0)
        {
            col = Random.Range(actCol + 2, collumns - 1);
        }
        else
        {
            col = Random.Range(actCol + 1, collumns - 1);
        }

        Rooms[row][col].GetComponent<RoomScript>().placeKey();

        row = Random.Range(rows / 2, rows - 1);
        col = Random.Range(0, actCol - 1);

        Rooms[row][col].GetComponent<RoomScript>().placeKey();

        row = Random.Range(rows / 2, rows - 1);
        col = Random.Range(actCol + 1, collumns - 1);

        Rooms[row][col].GetComponent<RoomScript>().placeKey();
    }
}
