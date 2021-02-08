using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigidbody2D;

    public TextMeshProUGUI collectedText;
    public static int collectedAmount;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rigidbody2D.velocity = new Vector3(horizontal * speed, vertical * speed, 0);

        collectedText.text = "Items Collected: " + collectedAmount;
    }
}
