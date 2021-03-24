using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static int health = 10;
    private static int maxHealth = 10;
    private static float moveSpeed = 5f;
    [SerializeField]
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;

    private bool speedCollected = false;
    private bool attackSpeedCollected = false;
    private bool bulletSizeCollected = false;

    public List<string> collectedNames = new List<string>();

    public static int Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }

    public TextMeshProUGUI healthText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void HealPlayer(int healAmount)
    {
        Health = Mathf.Min(maxHealth, health + healAmount);
    }

    public static void MoveSpeedChange(float moveSpeedAmount)
    {
        moveSpeed += moveSpeedAmount;
    }

    public static void FireRateChange(float fireRateAmount)
    {
        if(fireRate > 0.1f) 
            fireRate -= fireRateAmount;
    }

    public static void BulletSizeChange(float bulletSizeAmount)
    {
        bulletSize += bulletSizeAmount;
    }

    public void UpdateCollectedItems(CollectionController item)
    {
        collectedNames.Add(item.item.name);

        foreach(string i in collectedNames)
        {
            switch(i)
            {
                case "Speed":
                    speedCollected = true;
                    break;
                case "Attack":
                    attackSpeedCollected = true;
                    break;
                case "Bullet":
                    bulletSizeCollected = true;
                    break;
            }
        }

        if(speedCollected && attackSpeedCollected && bulletSizeCollected)
        {
            FireRateChange(0.1f);
        }
    }

    public static void KillPlayer()
    {

    }
}
