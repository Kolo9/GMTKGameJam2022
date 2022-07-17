using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheck : MonoBehaviour
{
    Vector3 diceVelocity;

    void Start()
    {
        diceVelocity = ShapeDice.diceVelocity;
    }

    void OnTriggerStay(Collider other) {
        if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f) {
            // switch (other.gameObject.name)
            // {
                
            //     default:
            // }
            Debug.Log(other.gameObject.name + " is up!");
        }
    }
}
