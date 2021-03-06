﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;

    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentWorldName = "City";

    RoomInfo currentLoadRoomData;

    Room currentRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    bool spawnedBossRoom = false;
    bool updatedRooms = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
            return;
        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
                StartCoroutine(SpawnBossRoom());
            else if(spawnedBossRoom && !updatedRooms)
            {
                foreach(Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }

                UpdateRooms();
                updatedRooms = true;

                foreach (Room room in loadedRooms)
                {
                    string name = (room.name);
                    int x = int.Parse(name.Split(' ')[1].Split(',')[0]);
                    int y = int.Parse(name.Split(' ')[2]);

                    string xy = (x + 1) + ", " + y;

                    if (!GameObject.Find(currentWorldName + "-Basic1 " + xy) && !GameObject.Find(currentWorldName + "-Empty " + xy)
                        && !GameObject.Find(currentWorldName + "-End " + xy) && !GameObject.Find(currentWorldName + "-Start " + xy))
                        room.rightCollider.GetComponent<BoxCollider2D>().enabled = true;

                    xy = (x - 1) + ", " + y;

                    if (!GameObject.Find(currentWorldName + "-Basic1 " + xy) && !GameObject.Find(currentWorldName + "-Empty " + xy)
                        && !GameObject.Find(currentWorldName + "-End " + xy) && !GameObject.Find(currentWorldName + "-Start " + xy))
                        room.leftCollider.GetComponent<BoxCollider2D>().enabled = true;

                    xy = x + ", " + (y + 1);

                    if (!GameObject.Find(currentWorldName + "-Basic1 " + xy) && !GameObject.Find(currentWorldName + "-Empty " + xy)
                        && !GameObject.Find(currentWorldName + "-End " + xy) && !GameObject.Find(currentWorldName + "-Start " + xy))
                        room.topCollider.GetComponent<BoxCollider2D>().enabled = true;

                    xy = x + ", " + (y - 1);

                    if (!GameObject.Find(currentWorldName + "-Basic1 " + xy) && !GameObject.Find(currentWorldName + "-Empty " + xy)
                        && !GameObject.Find(currentWorldName + "-End " + xy) && !GameObject.Find(currentWorldName + "-Start " + xy))
                        room.bottomCollider.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }
    
    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);

        if(loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);

            Destroy(bossRoom.gameObject);

            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
            return;

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if(!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(currentLoadRoomData.X * room.Width, currentLoadRoomData.Y * room.Height, 0);

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }

            loadedRooms.Add(room);

            room.RemoveUnconnectedDoors();
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[]{
            "Empty",
            "Basic1"
        };

        return possibleRooms[Random.Range(0,possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;

        currentRoom = room;

        Debug.Log(currentRoom.transform.position);

        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void UpdateRooms()
    {
        foreach(Room room in loadedRooms)
        {
            if(currentRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies != null)
                {
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                    }

                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }

                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if (enemies.Length > 0)
                {
                    foreach (EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                    }

                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    foreach (Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }
            }
        }
    }
}
