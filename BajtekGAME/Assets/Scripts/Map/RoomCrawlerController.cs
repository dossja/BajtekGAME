using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3
};

public class RoomCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();

    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up },
        {Direction.left, Vector2Int.left },
        {Direction.down, Vector2Int.down },
        {Direction.right, Vector2Int.right }
    };

    public static List<Vector2Int> GenerateRooms(RoomGenerationData roomData)
    {
        List<RoomCrawler> roomCrawlers = new List<RoomCrawler>();

        for(int i = 0; i < roomData.numberOfCrawlers; i++)
        {
            roomCrawlers.Add(new RoomCrawler(Vector2Int.zero));
        }

        int iterations = Random.Range(roomData.iterationMin, roomData.iterationMax);

        for(int i = 0; i < iterations; i++)
        {
            foreach(RoomCrawler roomCrawler in roomCrawlers)
            {
                Vector2Int newPos = roomCrawler.Move(directionMovementMap);
                positionsVisited.Add(newPos);
            }
        }

        return positionsVisited;
    }
}
