using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDiceCheck : MonoBehaviour {
    public static string result;
    public static bool rolling;

    void Start() {
        result = null;
        rolling = false;
    }

    void OnTriggerStay(Collider other) {
        if (!rolling) {
            return;
        }
        if (ShapeDice.diceVelocity.Equals(Vector3.zero)) {
            rolling = false;
            result = other.gameObject.name;
        }
    }
}
