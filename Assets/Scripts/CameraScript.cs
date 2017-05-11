using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour {

    int rows;
    int collumns;

    int curRow;
    int curCol;

    public GameObject player;

    public RoomManager RoomManager;

	// Use this for initialization
	void Start () {
        rows = RoomManager.getRows();
        collumns = RoomManager.getCollumns();
        curRow = RoomManager.getActiveRow();
        curCol = RoomManager.getActiveCol();

        transform.position = RoomManager.getRoomTransform(curRow, curCol).position;
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    if (curCol < collumns - 1)
        //    {
        //        RoomManager.EnableRoom(curRow, curCol + 1);
        //        RoomManager.DisableRoom(curRow, curCol);
        //        curCol++;
        //        //TransitionRoom(curRow, curCol);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    if (curCol > 0)
        //    {
        //        RoomManager.EnableRoom(curRow, curCol - 1);
        //        RoomManager.DisableRoom(curRow, curCol);
        //        curCol--;
        //        //TransitionRoom(curRow, curCol);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    if (curRow > 0)
        //    {
        //        RoomManager.EnableRoom(curRow - 1, curCol);
        //        RoomManager.DisableRoom(curRow, curCol);
        //        curRow--;
        //        //TransitionRoom(curRow, curCol);
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    if (curRow < (rows - 1))
        //    {
        //        RoomManager.EnableRoom(curRow + 1, curCol);
        //        RoomManager.DisableRoom(curRow, curCol);
        //        curRow++;
        //        //TransitionRoom(curRow, curCol);
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
        Vector3 t = Vector3.Lerp(transform.position, RoomManager.getRoomTransform(curRow, curCol).position, .4f);
        t.z = -10f;
        transform.position = t;
    }

    public void TransitionRoom(int num)
    {
        if (num == 0)
        {
            if (curRow < (rows - 1))
            {
                RoomManager.EnableRoom(curRow + 1, curCol);
                RoomManager.DisableRoom(curRow, curCol);
                curRow++;
                player.transform.position += new Vector3(0, -1.25f, 0);
                //player.transform.position += new Vector3(0, -200, 0);
                player.transform.position += new Vector3(0, -200 * RoomManager.sizeModifier, 0);
            }
        }
        else if (num == 1)
        {
            if (curCol > 0)
            {
                RoomManager.EnableRoom(curRow, curCol - 1);
                RoomManager.DisableRoom(curRow, curCol);
                curCol--;
                player.transform.position += new Vector3(-1.25f, 0, 0);
                //player.transform.position += new Vector3(-200, 0, 0);
                player.transform.position += new Vector3(-200 * RoomManager.sizeModifier, 0, 0);
            }
        }
        else if(num == 2)
        {
            if (curRow > 0)
            {
                RoomManager.EnableRoom(curRow - 1, curCol);
                RoomManager.DisableRoom(curRow, curCol);
                curRow--;
                player.transform.position += new Vector3(0, 1.25f, 0);
                //player.transform.position += new Vector3(0, 200, 0);
                player.transform.position += new Vector3(0, 200 * RoomManager.sizeModifier, 0);
            }
        }
        else if (num == 3)
        {
            if (curCol < collumns - 1)
            {
                RoomManager.EnableRoom(curRow, curCol + 1);
                RoomManager.DisableRoom(curRow, curCol);
                curCol++;
                player.transform.position += new Vector3(1.25f, 0, 0);
                //player.transform.position += new Vector3(200, 0, 0);
                player.transform.position += new Vector3(200 * RoomManager.sizeModifier, 0, 0);
            }
        }
    }
}
