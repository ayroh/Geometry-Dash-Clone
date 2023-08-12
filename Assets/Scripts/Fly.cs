using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Mechanic
{
    [Header("Variables")]
    [SerializeField] private float flyForce = 2f;
    [SerializeField] private float maxFlyVelocity = 7;
    [SerializeField] private float flyMaxRotation = 30;

    private void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            if(rb2d.velocity.y >= maxFlyVelocity)
                rb2d.velocity = new Vector3(movementSpeed, maxFlyVelocity, 0f);
            else
                rb2d.velocity = new Vector3(movementSpeed, rb2d.velocity.y + flyForce, 0f);
        }
        else if (isGrounded) {
            rb2d.velocity = new Vector3(movementSpeed, 0f, 0f);
        }
        else {
            if (rb2d.velocity.y <= -maxFlyVelocity)
                rb2d.velocity = new Vector3(movementSpeed, -maxFlyVelocity, 0f);
            else
                rb2d.velocity = new Vector3(movementSpeed, rb2d.velocity.y - flyForce, 0f);
        }
        spriteTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rb2d.velocity.y * flyMaxRotation / maxFlyVelocity));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!enabled)
            return;
        if (collision.transform.CompareTag("Ground")) {
            isGrounded = true;
            ParticleState(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (!enabled)
            return;
        if (collision.transform.CompareTag("Ground")) {
            isGrounded = false;
            ParticleState(true);
        }
    }

}
