using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject wintTextObject;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public LayerMask groundlayer;
    public float raycastDistance = 0.6f;
    public float jump;
    private bool isGrounded;
    private int jumpCount;
    //public int maxJumps;

    private float ySpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent <Rigidbody>();
        count = 0;
        SetCountText();
        wintTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            wintTextObject.SetActive(true);
        }
    }

    void Update()
    {
        //Ground check
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundlayer))
            isGrounded = true;
        else
            isGrounded = false;
    }

    void FixedUpdate()
    {

        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        float magnitude = movement.magnitude;
        magnitude = Mathf.Clamp01(magnitude);
        Vector3 vel = movement * magnitude;

        //Jump
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount == 0 || jumpCount == 1)
            {
                rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
                jumpCount++;
            }
        }
     
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }
}
