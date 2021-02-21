using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;

    private Vector2 currentPosition;
    private Vector2 lastPosition;
    private Vector2 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathDelay());
        if(!isEnemyBullet)
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnemyBullet)
        {
            currentPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, 5f * Time.deltaTime);

            if (currentPosition == lastPosition)
                Destroy(gameObject);
            lastPosition = currentPosition;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPosition = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !isEnemyBullet)
        {
            collision.gameObject.GetComponent<EnemyController>().Die();
            Destroy(gameObject);
        }

        if (collision.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }
    }
}
