using UnityEngine;
using System.Collections;

public class SlotRotator : MonoBehaviour {
    float direction;

	void Awake () {
        direction = Random.value > 0.5f ? 1f : -1f;
        float randomStartRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomStartRotation);
	}
	
	void Update () {
        transform.rotation *= Quaternion.Euler(0f, 0f, direction);
	}
}
