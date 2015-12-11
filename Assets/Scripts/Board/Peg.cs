using UnityEngine;
using System.Collections;
using System.Linq;

public class Peg : MonoBehaviour {

    [SerializeField] private GameObject removalEffect;
    [HideInInspector] public Slot mySlot;

    void Awake() {
        Collider2D myCollider = GetComponent<Collider2D>();
        Collider2D mySlotCollider = Physics2D.OverlapPointAll(transform.position).Where(col => col.gameObject.layer == Board.slotMask && col != myCollider).ToArray()[0];
        mySlot = mySlotCollider.GetComponent<Slot>();
        if (mySlot != null)
            mySlot.myPeg = this;
    }

    public void GetGrabbed() {
        transform.parent = Hand.holdingTransform;
        transform.localPosition = Vector2.zero;
        mySlot.IsOpen = true;
    }

    public void GetDropped(Slot newSlot) {
        transform.parent = newSlot.transform;
        mySlot.UpdateNewPeg(null, false);
        mySlot = null;
        mySlot = newSlot;
        mySlot.UpdateNewPeg(this, true);
        transform.localPosition = Vector2.zero;
    }

    public void GetRemoved() {
        mySlot.UpdateNewPeg(null, false);
        Instantiate(removalEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
