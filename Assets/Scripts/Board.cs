using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class Board {

    public static float slotSeparationDistance = 1f;
    public static int maxNeighborSlots = 6;

    public static int handMask = 10;
    public static int slotMask = 11;
    public static int pegMask = 12;

    public static List<Slot> allSlots;

}
