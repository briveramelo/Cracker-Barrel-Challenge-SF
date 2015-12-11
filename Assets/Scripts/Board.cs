using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Board : MonoBehaviour{

    public static float slotSeparationDistance = 1f;
    public static int maxNeighborSlots = 6;

    public static int handMask = LayerMask.NameToLayer("Hand");
    public static int slotMask = LayerMask.NameToLayer("Slot");
    public static int pegMask = LayerMask.NameToLayer("Peg");

    public static List<Slot> allSlots;
    void Awake() {
        allSlots = new List<Slot>(FindObjectsOfType<Slot>());
    }

}
