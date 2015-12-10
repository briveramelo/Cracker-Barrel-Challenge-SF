using UnityEngine;
using System.Collections;

public class Peg : MonoBehaviour {

    [HideInInspector] public Slot mySlot;

    public void GetGrabbed() {
        transform.parent = Hand.holdingTransform;
        mySlot.UpdateIsOpen(true);
        mySlot = null;
    }

    public void GetDropped(Slot newSlot) {
        transform.parent = newSlot.transform;
        mySlot = newSlot;
        mySlot.UpdateIsOpen(false);
        transform.localPosition = Vector2.zero;
    }
}
