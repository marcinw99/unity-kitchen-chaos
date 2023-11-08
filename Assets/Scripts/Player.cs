using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;

    private bool isWalking;

    private void Update() {
        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W)) {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = +1;
        }

        Vector3 normalizedInputVector = inputVector.normalized;

        Vector3 positionChange = new Vector3(normalizedInputVector.x, 0f, normalizedInputVector.y);

        transform.position += positionChange * Time.deltaTime * moveSpeed;

        isWalking = positionChange != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, positionChange, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
