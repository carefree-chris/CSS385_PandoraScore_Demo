using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomPrefabs
{
    public Color32 color;
    public GameObject[] onebyone;
    public GameObject[] onebytwo;
    public GameObject[] twobyone;
    public GameObject[] twobytwo;
    public GameObject[] threebythree;
}

[System.Serializable]
public struct Tiles
{
    public GameObject Tile;
    public bool IsOccupied;
}

public class RoomScript : MonoBehaviour
{

    public RoomPrefabs[] RoomPrefabs;

    public GameObject Door;

    private GameObject[] Colliders;
    private GameObject[] Doors;
    private Tiles[][] Tiles;
    

    List<GameObject> Interactables = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitTiles(int i, GameObject g)
    {
        Tiles[i][0].Tile = g.transform.FindChild("1").gameObject;
        Tiles[i][1].Tile = g.transform.FindChild("2").gameObject;
        Tiles[i][2].Tile = g.transform.FindChild("3").gameObject;
        Tiles[i][3].Tile = g.transform.FindChild("4").gameObject;
        Tiles[i][4].Tile = g.transform.FindChild("5").gameObject;
        Tiles[i][5].Tile = g.transform.FindChild("6").gameObject;
        Tiles[i][6].Tile = g.transform.FindChild("7").gameObject;
        Tiles[i][7].Tile = g.transform.FindChild("8").gameObject;
        Tiles[i][8].Tile = g.transform.FindChild("9").gameObject;
        Tiles[i][9].Tile = g.transform.FindChild("10").gameObject;
        Tiles[i][10].Tile = g.transform.FindChild("11").gameObject;
        Tiles[i][11].Tile = g.transform.FindChild("12").gameObject;
    }

    public void init(int num, Texture2D room)
    {
        Tiles = new Tiles[7][];

        for (int i = 0; i < Tiles.Length; i++)
        {
            Tiles[i] = new Tiles[12];

            for(int j = 0; j < Tiles[i].Length; j++)
            {
                Tiles[i][j].IsOccupied = false;
            }

        }

        GameObject t = transform.FindChild("Tiles").gameObject;

        InitTiles(6, t.transform.FindChild("Row 1").gameObject);
        InitTiles(5, t.transform.FindChild("Row 2").gameObject);
        InitTiles(4, t.transform.FindChild("Row 3").gameObject);
        InitTiles(3, t.transform.FindChild("Row 4").gameObject);
        InitTiles(2, t.transform.FindChild("Row 5").gameObject);
        InitTiles(1, t.transform.FindChild("Row 6").gameObject);
        InitTiles(0, t.transform.FindChild("Row 7").gameObject);

        Doors = new GameObject[4];

        GameObject d = transform.FindChild("Doors").gameObject;

        Doors[0] = d.transform.FindChild("DoorLeft").gameObject;
        Doors[1] = d.transform.FindChild("DoorTop").gameObject;
        Doors[2] = d.transform.FindChild("DoorRight").gameObject;
        Doors[3] = d.transform.FindChild("DoorBottom").gameObject;

        Colliders = new GameObject[8];

        GameObject c = transform.FindChild("WallCollision").gameObject;

        Colliders[0] = c.transform.FindChild("TopLeft").gameObject;
        Colliders[1] = c.transform.FindChild("TopRight").gameObject;
        Colliders[2] = c.transform.FindChild("RightTop").gameObject;
        Colliders[3] = c.transform.FindChild("RightBottom").gameObject;
        Colliders[4] = c.transform.FindChild("BottomRight").gameObject;
        Colliders[5] = c.transform.FindChild("BottomLeft").gameObject;
        Colliders[6] = c.transform.FindChild("LeftBottom").gameObject;
        Colliders[7] = c.transform.FindChild("LeftTop").gameObject;

        if (num != 5 && num != 6 && num != 1)
        {
            GameObject TopDoor = Instantiate(Door);
            TopDoor.transform.parent = Doors[1].transform;
            TopDoor.transform.localPosition = new Vector3(0, 0, 0);
            TopDoor.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            Colliders[0].transform.localScale = new Vector3(8, 1.5f, 1);
            Colliders[0].transform.localPosition = new Vector3(-2.5f, 4.25f, 0);
        }
        if(num != 5 && num != 7 && num != 4)
        {
            GameObject LeftDoor = Instantiate(Door);
            LeftDoor.transform.parent = Doors[0].transform;
            LeftDoor.transform.localPosition = new Vector3(0, 0, 0);
            LeftDoor.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            Colliders[6].transform.localScale = new Vector3(1.5f, 5f, 1);
            Colliders[6].transform.localPosition = new Vector3(-6.75f, -1f, 0);
        }
        if(num != 6 && num != 8 && num != 2)
        {
            GameObject RightDoor = Instantiate(Door);
            RightDoor.transform.parent = Doors[2].transform;
            RightDoor.transform.localPosition = new Vector3(0, 0, 0);
            RightDoor.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            Colliders[2].transform.localScale = new Vector3(1.5f, 5f, 1);
            Colliders[2].transform.localPosition = new Vector3(6.75f, 1f, 0);
        }
        if (num != 7 && num != 8 && num != 3)
        {
            GameObject BottomDoor = Instantiate(Door);
            BottomDoor.transform.parent = Doors[3].transform;
            BottomDoor.transform.localPosition = new Vector3(0, 0, 0);
            BottomDoor.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            Colliders[5].transform.localScale = new Vector3(8, 1.5f, 1);
            Colliders[5].transform.localPosition = new Vector3(-2.5f, -4.25f, 0);
        }

        Color32[] pixels = room.GetPixels32();

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if(Tiles[i][j].IsOccupied == false)
                    spawnTiles(pixels, i, j);
            }
        }
    }

    private void spawnTiles(Color32[] c, int i, int j)
    {
        foreach (RoomPrefabs r in RoomPrefabs)
        {
            if (c[(i * 12) + j].Equals(r.color))
            {
                int num = 0;

                if (j < 11) {
                    if (Tiles[i][j + 1].IsOccupied == false && c[(i * 12) + (j + 1)].Equals(r.color))
                    {
                        num = 1;
                        if (i < 6)
                        {
                            if(Tiles[i + 1][j].IsOccupied == false && c[((i + 1) * 12) + (j)].Equals(r.color))
                            {
                                num = 3;
                                if(Tiles[i + 1][j + 1].IsOccupied == false && c[((i + 1) * 12) + (j + 1)].Equals(r.color))
                                {
                                    num = 4;
                                }
                            }
                        }
                    }
                }
                else if (i < 6)
                {
                    if (Tiles[i + 1][j].IsOccupied == false && c[((i + 1) * 12) + (j)].Equals(r.color))
                    {
                        num = 2;
                    }
                }
                placeObject(r, i, j, num);
            }
        }
    }

    public void DisableRoom(int i, int j)
    {
        //todo, disable all visuals for room, keep collisions enabled for pathing?
    }

    void placeObject(RoomPrefabs room, int i, int j, int num)
    {
        if (num == 4 && room.twobytwo.Length == 0)
        {
            num = 3;
        }
        if(num == 3 && room.onebytwo.Length == 0)
        {
            num = 1;
        }
        if(num == 2 && room.onebytwo.Length == 0)
        {
            num = 1;
        }
        if(num == 1 && room.twobyone.Length == 0)
        {
            num = 0;
        }

        if(num == 4)
        {
            GameObject g = Instantiate(room.twobytwo[Random.Range(0, room.twobytwo.Length)]);
            g.transform.parent = Tiles[i][j].Tile.transform;
            Tiles[i][j].IsOccupied = true;
            Tiles[i + 1][j].IsOccupied = true;
            Tiles[i][j + 1].IsOccupied = true;
            Tiles[i + 1][j + 1].IsOccupied = true;

            g.transform.localPosition = new Vector3(.5f, .5f, 0);
            g.transform.rotation = new Quaternion(0, 0, 0, 0);

            if (room == RoomPrefabs[0] || room == RoomPrefabs[1] || room == RoomPrefabs[2])
            {
                Interactables.Add(g);
            }
        }
        else
        {
            int n = 0;
            if (num == 3)
                n = Random.Range(0, 3);
            else if (num == 2)
            {
                n = Random.Range(0, 2);
                n *= 2;
            }
            else
                n = Random.Range(0, num + 1);

            if (n == 0)
            {
                GameObject g = Instantiate(room.onebyone[Random.Range(0, room.onebyone.Length)]);
                g.transform.parent = Tiles[i][j].Tile.transform;
                Tiles[i][j].IsOccupied = true;

                g.transform.localPosition = new Vector3(0, 0, 0);
                g.transform.rotation = new Quaternion(0, 0, 0, 0);

                if (room == RoomPrefabs[0] || room == RoomPrefabs[1] || room == RoomPrefabs[2])
                {
                    Interactables.Add(g);
                }
            }
            else if (n == 1)
            {
                GameObject g = Instantiate(room.twobyone[Random.Range(0, room.twobyone.Length)]);
                g.transform.parent = Tiles[i][j].Tile.transform;
                Tiles[i][j].IsOccupied = true;

                Tiles[i][j+1].IsOccupied = true;

                g.transform.localPosition = new Vector3(.5f, 0, 0);
                g.transform.rotation = new Quaternion(0, 0, 0, 0);

                if (room == RoomPrefabs[0] || room == RoomPrefabs[1] || room == RoomPrefabs[2])
                {
                    Interactables.Add(g);
                }
            }
            else if (n == 2)
            {
                GameObject g = Instantiate(room.onebytwo[Random.Range(0, room.onebytwo.Length)]);
                g.transform.parent = Tiles[i][j].Tile.transform;
                Tiles[i][j].IsOccupied = true;
                Tiles[i+1][j].IsOccupied = true;

                g.transform.localPosition = new Vector3(0, .5f, 0);
                g.transform.rotation = new Quaternion(0, 0, 0, 0);

                if (room == RoomPrefabs[0] || room == RoomPrefabs[1] || room == RoomPrefabs[2])
                {
                    Interactables.Add(g);
                }
            }

        }

        
    }

    void placeHazards()
    {

    }

    public void placeKey()
    {
        int num = Random.Range(0, Interactables.Count - 1);
        Interactables[num].tag = "HasKey";
      Interactables[num].GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
    }
}
