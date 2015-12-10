using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Hand : MonoBehaviour {

    public static Transform holdingTransform;
    private bool isHolding;
    private Peg heldPeg;

    void Update() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

	void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.layer == Board.slotMask) {
            Slot selectableSlot = col.GetComponent<Slot>();
            if (selectableSlot.IsSelectable)
                selectableSlot.Highlight(HighlightType.Hover, true);
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
                    if (targetSlot.IsOpen)
                        PlacePeg(targetSlot);
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
        chosenPeg.GetGrabbed();
        isHolding = true;
        heldPeg = chosenPeg;
        Highlighter.UpdateSlotsReceivability(chosenPeg.mySlot);
        Highlighter.UpdateSlotsChosenStatus(chosenPeg.mySlot);
        Highlighter.HighlightSlots(HighlightType.Receivable);
        Highlighter.HighlightSlots(HighlightType.Chosen);
    }

    void PlacePeg(Slot targetSlot) {
        isHolding = false;
        heldPeg.GetDropped(targetSlot);
        heldPeg = null;
        Highlighter.UpdateSlotReceivabilityToClear();
        Highlighter.UpdateSlotSelectability();
        Highlighter.HighlightSlots(HighlightType.Selectable);
    }
}
