using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDice : MonoBehaviour
{
    static Rigidbody rigidBody;
    public static Vector3 diceVelocity;
    private readonly IRandom rng = new Random();

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        diceVelocity = rigidBody.velocity;

        if (Input.GetKeyDown(KeyCode.Space)) {
            // ShapeDiceText.diceShape = ?

            float dirX = rng.NextInclusive(0, 500);
            float dirY = rng.NextInclusive(0, 500);
            float dirZ = rng.NextInclusive(0, 500);
            transform.position = new Vector3(0, 2, 0);
            transform.rotation = Quaternion.identity;

            rigidBody.AddForce(transform.up * 500);
            rigidBody.AddForce(dirX, dirY, dirZ);
        }
    }
}
