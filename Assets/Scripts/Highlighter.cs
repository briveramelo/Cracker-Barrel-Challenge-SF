using UnityEngine;
using System.Collections;
//using System.Linq;
public class Highlighter : MonoBehaviour {

    static bool doHighlight = true;
    static bool unHighlight = false;

    void Start() {
        HighlightSlots(HighlightType.Selectable);
    }

    public static void UpdateSlotSelectability() {
        Board.allSlots.ForEach(slot => slot.UpdateIsSelectable());
    }

    public static void UpdateSlotsReceivabilityAndJumpability(Slot selectedSlot) {
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

    public static void UpdateSlotReceivabilityAndJumpabilityToClear() {
        Board.allSlots.ForEach(slot => slot.IsReceivable = false);
        Board.allSlots.ForEach(slot => slot.IsJumpable = false);
    }

    public static void UpdateSlotsChosenStatus(Slot chosenSlot) {
        Board.allSlots.ForEach(slot => slot.IsChosen = false);
        chosenSlot.IsChosen = true;
    }

    public static void UpdateSlotsChosenStatusToClear() {
        Board.allSlots.ForEach(slot => slot.IsChosen = false);
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
