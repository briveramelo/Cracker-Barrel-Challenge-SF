using UnityEngine;
using System.Collections;
//using System.Linq;
public static class Highlighter {

    static bool doHighlight = true;
    static bool unHighlight = false;

    public static void UpdateSlotSelectability() {
        Board.allSlots.ForEach(slot => slot.UpdateIsSelectable());
    }

    public static void UpdateSlotsReceivability(Slot selectedSlot) {
        Board.allSlots.ForEach(slot => slot.IsReceivable = false);
        for (int i = 0; i < Board.maxNeighborSlots; i++) {
            if (selectedSlot.neighborSlots[i] != null) {
                if (!selectedSlot.neighborSlots[i].IsOpen) {
                    if (!selectedSlot.neighborSlots[i].IsOpen) {
                        if (selectedSlot.neighborSlots[i].neighborSlots[i] != null) {
                            if (selectedSlot.neighborSlots[i].neighborSlots[i].IsOpen) {
                                selectedSlot.neighborSlots[i].neighborSlots[i].IsReceivable = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public static void UpdateSlotReceivabilityToClear() {
        Board.allSlots.ForEach(slot => slot.IsReceivable = false);
    }

    public static void UpdateSlotsChosenStatus(Slot chosenSlot) {
        Board.allSlots.ForEach(slot => slot.IsChosen = false);
        chosenSlot.IsChosen = true;
    }

    public static void HighlightSlots(HighlightType Highlighting) {
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

    public static void HighlightSlots(Slot slotToHighlightAsChosen) {
        slotToHighlightAsChosen.Highlight(HighlightType.Chosen, doHighlight);
    }
}
