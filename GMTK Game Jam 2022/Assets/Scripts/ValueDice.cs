using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueDice : MonoBehaviour {
    static Rigidbody rigidBody;
    public static Vector3 diceVelocity;
    private readonly IRandom rng = new Random();

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Roll() {
        float dirX = rng.NextInclusive(130, 150);
        float dirY = rng.NextInclusive(130, 150);
        float dirZ = rng.NextInclusive(130, 150);

        float rotX = rng.NextInclusive(0, 360);
        float rotY = rng.NextInclusive(0, 360);
        float rotZ = rng.NextInclusive(0, 360);

        transform.position = new Vector3(transform.position.x, transform.position.y + 6f, transform.position.z);
        transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);

        rigidBody.AddForce(transform.up * 500);
        rigidBody.AddTorque(dirX, dirY, dirZ);
        ValueDiceCheck.rolling = true;
    }

    void Update() {
        diceVelocity = rigidBody.velocity;
    }
}
