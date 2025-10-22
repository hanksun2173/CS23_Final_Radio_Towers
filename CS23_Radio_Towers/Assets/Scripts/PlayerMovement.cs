using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    public Rigidbody2D rb;
    private bool FaceRight = true;
    public static float runSpeed = 10f;
    public float startSpeed = 10f;
    private Vector2 moveDirection;
    public GameHandler gameHandler;
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        if (GameObject.FindWithTag("GameHandler") != null) {
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        }
    }

    void FixedUpdate(){
        moveDirection.x = Input.GetAxisRaw ("Horizontal");
        rb.MovePosition(rb.position + moveDirection * runSpeed * Time.fixedDeltaTime);
        if ((moveDirection.x < 0 && !FaceRight) || (moveDirection.x > 0 && FaceRight)) {
            playerTurn();
        }
    }

    private void playerTurn(){
        // NOTE: Switch player facing label
        FaceRight = !FaceRight;

        // NOTE: Multiply player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // void HandleMovement()
    // {
        // moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        // if (isAlive == true)
        // {

            // anim.ResetTrigger("Still");
            // anim.ResetTrigger("Front");
            // anim.ResetTrigger("Back");
            // anim.ResetTrigger("Side_L");
            // anim.ResetTrigger("Side_R");

            // if ((moveDirection.y > 0))
            // {
            //     anim.SetTrigger("Front");

            // }
            // else if ((moveDirection.y < 0))
            // {
            //     anim.SetTrigger("Back");
            // }
            // else if ((moveDirection.x < 0))
            // {
            //     anim.SetTrigger("Side_L");
            // }
            // else if ((moveDirection.x > 0))
            // {
            //     anim.SetTrigger("Side_R");
            // }
            // else
            // {
            //     anim.SetTrigger("Still");
            // }

            // Turning. Reverse if input is moving the Player right and Player faces left.
            // 
        // }
    // }

    // private IEnumerator DelayInteractionText()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     InteractionText.SetActive(false);
    // }
}