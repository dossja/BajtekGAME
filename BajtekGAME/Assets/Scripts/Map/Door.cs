using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;

    private GameObject player;

    private float widthOffset = 0.5f;

    public GameObject doorCollider;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
