using UnityEngine;

[CreateAssetMenu(fileName = "RoomGenerationData.asset", menuName = "RoomGenerationData/Room Data")]
public class RoomGenerationData : ScriptableObject
{
    public int numberOfCrawlers;
    public int iterationMin;
    public int iterationMax;
}
