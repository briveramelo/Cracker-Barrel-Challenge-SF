using UnityEngine;
using System.Collections;

public class RemovalEffect : MonoBehaviour {

    void Awake() {
        Destroy(gameObject, 1f);
    }
}
