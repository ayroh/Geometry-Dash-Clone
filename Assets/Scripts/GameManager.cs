using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Animations;

public class GameManager : MonoBehaviour
{

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

    public enum GameState { Jump, Fly, Restarting, MainMenu };
    public GameState gameState;


    private void Awake() => instance = this;

    void Start() => gameState = GameState.MainMenu;

    public void Restart() => ChangeState(GameState.Restarting);

    public void StartGame() => ChangeState(GameState.Jump);

    private void ChangeState(GameState state) {
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

        switch (state) {
            case GameState.Jump:
                mainCam.ChangeColor();

                mainCam.enabled = true;

                squareJump.enabled = true;
                squareJump.ParticleState(true);

                Physics2D.gravity = new Vector2(0f, gravity);
                squareCollider.enabled = true;
                break;
            case GameState.Fly:
                mainCam.ChangeColor();

                squareFly.enabled = true;
                squareFly.ParticleState(true);

                Physics2D.gravity = Vector2.zero;
                squareCollider.enabled = true;
                break;
            case GameState.Restarting:
                squareCollider.enabled = false;

                squareRb.velocity = Vector2.zero;

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

    public void MusicOnOff() {
        if (music.isPlaying)
            music.Pause();
        else
            music.Play();
    }

}