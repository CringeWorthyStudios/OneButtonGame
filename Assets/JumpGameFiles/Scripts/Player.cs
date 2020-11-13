using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;

    [SerializeField] private float movementSensitivity = 50f;

    [SerializeField] private GameObject deathScreen;

    private void Awake()
    {
        rigidbody2d = transform.GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }

    IEnumerator DeathScreen() 
    {
        deathScreen.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            float jumpVelocity = 20f;

            Vector2 velocity = rigidbody2d.velocity;
            velocity.y = Vector2.up.y * jumpVelocity;
            rigidbody2d.velocity = velocity;
        }
        MoveHorizontal();

        if (gameObject.transform.position.y < -6f)
        {
            StartCoroutine(DeathScreen());
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, 0.1f, platformLayerMask);
        Debug.Log(raycastHit2d.collider);
        return raycastHit2d.collider != null;
    }

    private void MoveHorizontal()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = new Vector3(mousePosition.x - (Screen.width / 2), 0, 0);

        if (mousePosition.x > movementSensitivity || mousePosition.x < -movementSensitivity)
        {

            Vector2 velocity = rigidbody2d.velocity;
            velocity.x = Vector2.right.x * (mousePosition.x * 0.025f);
            velocity.x = Mathf.Clamp(velocity.x, -10, 10);
            rigidbody2d.velocity = velocity;

            /*if(mousePosition.x > movementSensitivity)
            {
                Debug.Log("Greater than buffer");
            }
            else
            {
                Debug.Log("Less than buffer");
            }*/
        }
        else
        {
            Debug.Log("within buffer");
        }

        //Debug.Log(mousePosition);
    }
        

}

