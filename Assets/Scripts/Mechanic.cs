using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mechanic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D rb2d;
    [SerializeField] protected Transform spriteTransform;
    [SerializeField] protected ParticleSystem particle;

    [Header("Speed")]
    [SerializeField] protected float movementSpeed = 2f;

    protected bool isGrounded = false;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (!enabled)
            return;
        if (collision.transform.CompareTag("Deadly")) {
            ParticleState(false);
            GameManager.instance.Restart();
        }
        else if (collision.transform.CompareTag("Portal")) {
            GameManager.instance.Portal();
        }
    }

    public void ParticleState(bool state) {
        if (state) {
            particle.Play();
        }
        else {
            particle.Pause();
            particle.Clear();
        }
    }

}
