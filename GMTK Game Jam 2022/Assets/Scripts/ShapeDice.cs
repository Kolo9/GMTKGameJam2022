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

            float dirX = rng.NextInclusive(100, 150);
            float dirY = rng.NextInclusive(100, 150);
            float dirZ = rng.NextInclusive(100, 150);
            
            float rotX = rng.NextInclusive(0, 360);
            float rotY = rng.NextInclusive(0, 360);
            float rotZ = rng.NextInclusive(0, 360);

            transform.position = new Vector3(transform.position.x, transform.position.y+6f, transform.position.z);
            transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);

            rigidBody.AddForce(transform.up * 500);
            rigidBody.AddTorque(dirX, dirY, dirZ);
        }
    }
}
