using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,
    Follow,
    Die
};

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currentState = EnemyState.Wander;

    public float range;
    public float speed;

    private bool chooseDirection = false;
    private bool dead = false;
    private Vector3 randomDirection;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):
                Die();
                break;
        }

        if(IsPlayerInRange(range) && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Follow;
        }
        else if(!IsPlayerInRange(range) && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Wander;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirectionCoroutine()
    {
        chooseDirection = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDirection = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDirection);

        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDirection = false;
    }

    void Wander()
    {
        if (!chooseDirection)
            StartCoroutine(ChooseDirectionCoroutine());

        transform.position += -transform.right * speed * Time.deltaTime;

        if (IsPlayerInRange(range))
            currentState = EnemyState.Follow;
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
