using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEditor;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class Jump : Mechanic
{
    [Header("Variables")]
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float rotationSpeed = 0.1f;

    private float rerotateSpeed = 0.02f;

    private void Start() => isGrounded = true;

    private void Update() {
        CheckJump();
        rb2d.velocity = new Vector2(movementSpeed, rb2d.velocity.y);
    }


    private void FixedUpdate() {
        if (!isGrounded) 
            spriteTransform.Rotate(new Vector3(0f, 0f, -rotationSpeed));
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (!enabled)
            return;
        if (collision.transform.CompareTag("Ground")) {
            if (spriteTransform.rotation.eulerAngles.z % 90 != 0) {
                StartCoroutine(LerpRotationInSeconds());
            }
            ParticleState(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (!enabled)
            return;
        if (collision.transform.CompareTag("Ground")) {
            isGrounded = false;
            ParticleState(false);
        }
    }


    private IEnumerator LerpRotationInSeconds() {
        isGrounded = false;
        var timePassed = 0f;
        float initialZRot = spriteTransform.rotation.eulerAngles.z;
        float finalRot = spriteTransform.rotation.eulerAngles.z % 90;
        if (finalRot > 60) {
            finalRot = spriteTransform.rotation.eulerAngles.z + (90 - finalRot);
        }
        else {
            finalRot = spriteTransform.rotation.eulerAngles.z - finalRot;
        }
        while (timePassed < rerotateSpeed) {
            var factor = timePassed / rerotateSpeed;
            spriteTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Lerp(initialZRot, finalRot, factor)));
            timePassed += Time.deltaTime;
            yield return null;
        }
        spriteTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, finalRot));
        isGrounded = true;
    }


    private bool CheckJump() {
        if (Input.GetKey(KeyCode.Mouse0) && isGrounded) {
            JumpSquare();
            isGrounded = false;
            return true;
        }
        return false;
    }

    private void JumpSquare() => rb2d.velocity = new Vector2(movementSpeed, jumpForce);
    

}
