using UnityEngine;
using System.Collections;
//using System.Linq;
public static class Highlighter {

    static bool doHighlight = true;
    static bool unHighlight = false;

    public static void HighlightOnRemoveFirstPeg() {
        UpdateSlotsChosenStatusToClear();
        UpdateSlotSelectability();
        HighlightSlots(HighlightType.Selectable);
        Board.allSlots.ForEach(slot => slot.Highlight(HighlightType.Hover, false));
    }

    public static void HighlightOnGrabPeg(Slot chosenSlot) {
        UpdateSlotsReceivabilityAndJumpability(chosenSlot);
        HighlightSlots(HighlightType.Receivable);
        UpdateSlotsChosenStatus(chosenSlot);
        HighlightSlots(HighlightType.Chosen);
    }

    public static void HighlightOnPlacePeg() {
        UpdateSlotReceivabilityAndJumpabilityToClear();
        UpdateSlotsChosenStatusToClear();
        UpdateSlotSelectability();
        HighlightSlots(HighlightType.Selectable);
    }

    static void UpdateSlotSelectability() {
        Board.allSlots.ForEach(slot => slot.UpdateIsSelectable());
    }

    static void UpdateSlotsReceivabilityAndJumpability(Slot selectedSlot) {
        Board.allSlots.ForEach(slot => slot.IsReceivable = false);
        Board.allSlots.ForEach(slot => slot.IsJumpable = false);
        for (int i = 0; i < Board.maxNeighborSlots; i++) {
            if (selectedSlot.neighborSlots[i] != null) {
                if (!selectedSlot.neighborSlots[i].IsOpen) {
                    if (!selectedSlot.neighborSlots[i].IsOpen) {
                        if (selectedSlot.neighborSlots[i].neighborSlots[i] != null) {
                            if (selectedSlot.neighborSlots[i].neighborSlots[i].IsOpen) {
                                selectedSlot.neighborSlots[i].IsJumpable = true;
                                selectedSlot.neighborSlots[i].neighborSlots[i].IsReceivable = true;
                            }
                        }
                    }
                }
            }
        }
    }

    static void UpdateSlotReceivabilityAndJumpabilityToClear() {
        Board.allSlots.ForEach(slot => slot.IsReceivable = false);
        Board.allSlots.ForEach(slot => slot.IsJumpable = false);
    }

    static void UpdateSlotsChosenStatus(Slot chosenSlot) {
        Board.allSlots.ForEach(slot => slot.IsChosen = false);
        chosenSlot.IsChosen = true;
    }

    static void UpdateSlotsChosenStatusToClear() {
        Board.allSlots.ForEach(slot => slot.IsChosen = false);
    }

    static void HighlightSlots(HighlightType Highlighting) {
        foreach (Slot slot in Board.allSlots) {
            switch (Highlighting) {
                case HighlightType.Selectable:
                    slot.Highlight(HighlightType.Selectable, slot.IsSelectable);
                    slot.Highlight(HighlightType.Receivable, unHighlight);
                    slot.Highlight(HighlightType.Chosen, unHighlight);
                    break;
                case HighlightType.Receivable:
                    slot.Highlight(HighlightType.Selectable, unHighlight);
                    slot.Highlight(HighlightType.Receivable, slot.IsReceivable);
                    break;
            }
        }
    }

    static void HighlightSlots(Slot slotToHighlightAsChosen) {
        slotToHighlightAsChosen.Highlight(HighlightType.Chosen, doHighlight);
    }
}
