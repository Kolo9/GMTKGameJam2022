using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueDiceCheck : MonoBehaviour {
    static Vector3 diceVelocity;
    public static string result;
    private bool rolling;

    void Start() {
        diceVelocity = ValueDice.diceVelocity;
        result = null;
        rolling = false;
    }

    void OnTriggerStay(Collider other) {
        bool moving = !diceVelocity.Equals(Vector3.zero);
        if (moving) {
            rolling = true;
        } else if (rolling) {
            // Stopped moving
            rolling = false;
            result = other.gameObject.name;
        }
    }
}
