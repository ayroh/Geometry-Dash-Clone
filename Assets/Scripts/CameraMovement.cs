using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform flyPosition;
    [SerializeField] private Camera cam;

    [Header("Variables")]
    [SerializeField] public Vector3 offset = new Vector3(8f, 0, -10f);
    [SerializeField] private float maxSquareYPosition;
    [SerializeField] private Color jumpColor;
    [SerializeField] private Color flyColor;

    private Vector3 position;

    void FixedUpdate()
    {
        if(GameManager.instance.gameState == GameManager.GameState.Jump) {
            position = new Vector3(playerTransform.position.x + offset.x, 0f, playerTransform.position.z + offset.z);

            if(playerTransform.position.y > maxSquareYPosition) {
                position.y = playerTransform.position.y - maxSquareYPosition;
            }

            transform.position = position;
        }
        else if(GameManager.instance.gameState == GameManager.GameState.Fly) {
            transform.position = new Vector3(playerTransform.position.x + offset.x, Mathf.Lerp(transform.position.y, flyPosition.position.y, .05f), playerTransform.position.z + offset.z);
        }
    }


    private void OnValidate() => transform.position = new Vector3(playerTransform.position.x + offset.x, 0f, playerTransform.position.z + offset.z);


    public void ChangeColor() {
        if (GameManager.instance.gameState == GameManager.GameState.Fly)
            cam.DOColor(flyColor, 0.5f);
        else
            cam.DOColor(jumpColor, 0.5f);
    }
}
