using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GameManager : MonoBehaviour
{
    public enum GameState { Jump, Fly, Restarting, MainMenu };

    [Header("Square")]
    [SerializeField] private GameObject square;
    [SerializeField] private Rigidbody2D squareRb;
    [SerializeField] private Collider2D squareCollider;
    [SerializeField] private Jump squareJump;
    [SerializeField] private Fly squareFly;
    [SerializeField] private SpriteRenderer squareSprite;

    [Header("References")]
    [SerializeField] private CameraMovement mainCam;
    [SerializeField] private Transform startPosition;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private AudioSource music;

    [Header("Variables")]
    [SerializeField] private float gravity = 50f;

    // Singleton
    public static GameManager instance;

    public GameState gameState;


    private void Awake() => instance = this;


    void Start() => gameState = GameState.MainMenu;
    

    private void ChangeState(GameState state) {

        // Organise current game state 
        switch (gameState) {
            case GameState.Jump:
                squareJump.StopAllCoroutines();
                squareJump.enabled = false;
                squareJump.ParticleState(false);
                break;
            case GameState.Fly:
                squareFly.enabled = false;
                squareFly.ParticleState(false);
                break;
            case GameState.Restarting:
                break;
            case GameState.MainMenu:
                mainMenuPanel.SetActive(false);
                break;
        }

        gameState = state;

        // Apply new game state
        switch (state) {
            case GameState.Jump:
                // Camera
                mainCam.ChangeColor();
                mainCam.enabled = true;

                // Open Jump script
                squareJump.enabled = true;
                squareJump.ParticleState(true);

                // Apply Gravity
                Physics2D.gravity = new Vector2(0f, gravity);

                squareCollider.enabled = true;
                break;
            case GameState.Fly:
                // Camera
                mainCam.ChangeColor();

                // Open Fly script
                squareFly.enabled = true;
                squareFly.ParticleState(true);

                // Remove Gravity
                Physics2D.gravity = Vector2.zero;
                squareCollider.enabled = true;
                break;
            case GameState.Restarting:
                // Disable Collider
                squareCollider.enabled = false;

                // End last state velocity
                squareRb.velocity = Vector2.zero;

                // Dotween animations on death
                square.transform.DOMove(startPosition.position, 1f);
                mainCam.enabled = false;
                mainCam.transform.DOMove(new Vector3(startPosition.position.x + mainCam.offset.x, 0f, startPosition.position.z + mainCam.offset.z), 1f);

                squareSprite.color = new Color(squareSprite.color.r, squareSprite.color.g, squareSprite.color.b, 0f);
                squareSprite.DOFade(1f, 1f);
                squareSprite.transform.DORotate(Vector3.zero, 1f).OnComplete(() => StartGame());
                break;
            case GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                break;
        }
    }

    public void Portal() => ChangeState(GameState.Fly);

    public void StartGame() => ChangeState(GameState.Jump);

    public void Restart() => ChangeState(GameState.Restarting);


    public void MusicOnOff() {
        if (music.isPlaying)
            music.Pause();
        else
            music.Play();
    }

}