using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Hand : MonoBehaviour {

    public static Transform holdingTransform;

    private Peg heldPeg;

    private bool isHolding;
    private bool replace = false;
    private bool jump = true;
    private bool firstPegRemoved;
    private bool correctivePause;

    void Awake() {
        holdingTransform = transform;
    }

    void Update() {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

	void OnTriggerStay2D(Collider2D col) {
        if (!correctivePause) {
            if (Input.GetButtonDown("Select")) {
                if (!isHolding) {
                    if (col.gameObject.layer == Board.pegMask) {
                        Peg chosenPeg = col.GetComponent<Peg>();
                        if (!firstPegRemoved) {
                            RemoveFirstPeg(chosenPeg);
                        }
                        else if (chosenPeg.mySlot.IsSelectable) {
                            GrabPeg(chosenPeg);
                        }
                    }
                }
                else{
                    if (col.gameObject.layer == Board.slotMask) {
                        Slot targetSlot = col.GetComponent<Slot>();
                        if (targetSlot == heldPeg.mySlot) {
                            PlacePeg(targetSlot, replace);
                        }
                        else if (targetSlot.IsReceivable) {
                            PlacePeg(targetSlot, jump);
                        }
                    }
                }
            }
        }
    }

    void RemoveFirstPeg(Peg chosenPeg) {
        firstPegRemoved = true;
        chosenPeg.GetRemoved();
        Highlighter.HighlightOnRemoveFirstPeg();
        TextDisplay.Instance.RemoveFirstPegText();
    }

    void GrabPeg(Peg chosenPeg) {
        Highlighter.HighlightOnGrabPeg(chosenPeg.mySlot);
        chosenPeg.GetGrabbed();
        StartCoroutine(PauseDoubleClick());
        isHolding = true;
        heldPeg = chosenPeg;
    }

    IEnumerator PauseDoubleClick() {
        correctivePause = true;
        yield return new WaitForEndOfFrame();
        correctivePause = false;
    }

    void PlacePeg(Slot targetSlot, bool jumping) {
        isHolding = false;
        if (jumping) {
            Board.DecreasePegCount();
            RemoveJumpedPeg(targetSlot);
        }
        heldPeg.GetDropped(targetSlot);
        heldPeg = null;
        Highlighter.HighlightOnPlacePeg();
        LevelManager.Instance.CheckWinState();
    }

    void RemoveJumpedPeg(Slot targetSlot) {
        Vector2 checkLine = (targetSlot.transform.position - heldPeg.mySlot.transform.position)/2f;
        Vector2 checkPoint = (Vector2)heldPeg.mySlot.transform.position + checkLine;
        Physics2D.OverlapPointAll(checkPoint).Where(col => col.gameObject.layer == Board.slotMask).ToArray()[0].GetComponent<Slot>().myPeg.GetRemoved();
    }
}
