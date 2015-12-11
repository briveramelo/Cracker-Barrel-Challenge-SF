using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Hand : MonoBehaviour {

    [SerializeField] Transform holdTransform;
    public static Transform holdingTransform;
    public bool isHolding;
    public Peg heldPeg;
    [SerializeField] private Animator handAnimator;
    private bool replace = false;
    private bool jump = true;
    private enum Handimation {
        Ready = 0,
        Holding = 1
    }

    void Awake() {
        holdingTransform = holdTransform;
    }

    void Update() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

	void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.layer == Board.slotMask) {
            Slot selectableSlot = col.GetComponent<Slot>();
            if (selectableSlot.IsSelectable) {
                if (!isHolding || heldPeg.mySlot == selectableSlot)
                    selectableSlot.Highlight(HighlightType.Hover, true);
            }
        }

        if (Input.GetButtonDown("Select")) {
            if (!isHolding) {
                if (col.gameObject.layer == Board.pegMask) {
                    Peg chosenPeg = col.GetComponent<Peg>();
                    if (chosenPeg.mySlot.IsSelectable)
                        GrabPeg(chosenPeg);
                }
            }
            else{
                if (col.gameObject.layer == Board.slotMask) {
                    Slot targetSlot = col.GetComponent<Slot>();
                    if (targetSlot == heldPeg.mySlot)
                        PlacePeg(targetSlot, replace);
                    else if (targetSlot.IsReceivable)
                        PlacePeg(targetSlot, jump);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.layer == Board.slotMask) {
            Slot deselectableSlot = col.GetComponent<Slot>();
            if (deselectableSlot.IsSelectable)
                deselectableSlot.Highlight(HighlightType.Hover, false);
        }
    }

    void GrabPeg(Peg chosenPeg) {
        Highlighter.UpdateSlotsReceivabilityAndJumpability(chosenPeg.mySlot);
        Highlighter.HighlightSlots(HighlightType.Receivable);
        Highlighter.UpdateSlotsChosenStatus(chosenPeg.mySlot);
        Highlighter.HighlightSlots(HighlightType.Chosen);
        chosenPeg.GetGrabbed();
        isHolding = true;
        handAnimator.SetInteger("AnimState", (int)Handimation.Holding);
        heldPeg = chosenPeg;
    }

    void PlacePeg(Slot targetSlot, bool jumping) {
        isHolding = false;
        handAnimator.SetInteger("AnimState", (int)Handimation.Ready);
        if (jumping)
            RemoveJumpedPeg(targetSlot);
        heldPeg.GetDropped(targetSlot);
        heldPeg = null;
        Highlighter.UpdateSlotReceivabilityAndJumpabilityToClear();
        Highlighter.UpdateSlotsChosenStatusToClear();
        Highlighter.UpdateSlotSelectability();
        Highlighter.HighlightSlots(HighlightType.Selectable);
    }

    void RemoveJumpedPeg(Slot targetSlot) {
        Vector2 checkLine = (targetSlot.transform.position - heldPeg.mySlot.transform.position)/2f;
        Vector2 checkPoint = (Vector2)heldPeg.mySlot.transform.position + checkLine;
        Physics2D.OverlapPointAll(checkPoint).Where(col => col.gameObject.layer == Board.slotMask).ToArray()[0].GetComponent<Slot>().myPeg.GetRemoved();
    }
}
