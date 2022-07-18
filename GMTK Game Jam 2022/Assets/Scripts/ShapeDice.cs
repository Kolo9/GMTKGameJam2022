using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDice : MonoBehaviour {
    public static readonly IRandom rng = new Random();

    static Rigidbody rigidBody;
    public static Vector3 diceVelocity;
    private bool triggeredRoll = false;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Roll() {
        float dirX = rng.NextInclusive(400, 600) * (rng.NextInclusive(1, 2) == 1 ? 1 : -1);
        float dirY = rng.NextInclusive(400, 600) * (rng.NextInclusive(1, 2) == 1 ? 1 : -1);
        float dirZ = rng.NextInclusive(400, 600) * (rng.NextInclusive(1, 2) == 1 ? 1 : -1);

        //float rotX = rng.NextInclusive(0, 360);
        //float rotY = rng.NextInclusive(0, 360);
        //float rotZ = rng.NextInclusive(0, 360);

        transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        //transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);

        rigidBody.AddForce(transform.up * 1);
        rigidBody.AddTorque(dirX, dirY, dirZ);
        triggeredRoll = true;
    }
    private IEnumerator TriggerChecker() {
        yield return null;
        ShapeDiceCheck.rolling = true;
    }

    void Update() {
        diceVelocity = rigidBody.velocity;
        if (triggeredRoll) {
            triggeredRoll = false;
            StartCoroutine(TriggerChecker());
        }
    }
}
