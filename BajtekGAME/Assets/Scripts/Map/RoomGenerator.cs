using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public RoomGenerationData roomGenerationData;
    private List<Vector2Int> mapRooms;

    void Start()
    {
        mapRooms = RoomCrawlerController.GenerateRooms(roomGenerationData);
        SpawnRooms(mapRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0);

        foreach(Vector2Int roomLocation in rooms)
        {
             RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);
        }
    }
}
