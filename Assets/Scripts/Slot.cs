using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Slot : MonoBehaviour {

    [SerializeField] private ParticleSystem hoverParticles;
    [SerializeField] private ParticleSystem chosenParticles;
    [SerializeField] private ParticleSystem receivableParticles;
    [SerializeField] private ParticleSystem selectableParticles;

	[HideInInspector] public Slot[] neighborSlots;
    [HideInInspector] public List<Slot> jumpableNeighborSlots;
    [HideInInspector] public List<Slot> availableSecondNeighborSlots;

    private bool isOpen;       public bool IsOpen       { get { return isOpen; } }
    private bool isSelectable; public bool IsSelectable { get { return isSelectable; } set { isSelectable = value; } }
    private bool isReceivable; public bool IsReceivable { get { return isReceivable; } set { isReceivable = value; } }
    private bool isChosen;     public bool IsChosen     { get { return isChosen; }     set { isChosen = value; } }
    private Collider2D myCollider;
    public Collider2D[] neighborSlotColliders;

    void Awake() {
        Board.allSlots.Add(this);
        myCollider = GetComponent<Collider2D>();
        AssignNeighborSlots();
        UpdateIsOpen();
    }

    #region AssignNeighborSlots
    //neighborSlots Slot[] array, visualized
    //
    //  [5]0   0[0]
    //[4]0   0   0[1]  
    //  [3]0   0[2]
    //
    //
    //Order The Neighbor Slots To Match the above visual
    void AssignNeighborSlots() {
        Collider2D[] neighborSlotColliders = Physics2D.OverlapCircleAll(transform.position, Board.slotSeparationDistance).Where(col => (col.gameObject.layer == Board.slotMask && col != myCollider)).ToArray(); 
        neighborSlots = new Slot[Board.maxNeighborSlots];
       
        Vector2 myPosition = transform.position;
        for (int i=0; i< neighborSlotColliders.Length; i++) {
            Vector2 relativeSlotPosition = ((Vector2)neighborSlotColliders[i].transform.position - myPosition).normalized;
            if (relativeSlotPosition.x > 0) {
                if (relativeSlotPosition.y > 0)
                    neighborSlots[0] = neighborSlotColliders[i].GetComponent<Slot>();
                else if (relativeSlotPosition.y == 0)
                    neighborSlots[1] = neighborSlotColliders[i].GetComponent<Slot>();
                else if (relativeSlotPosition.y < 0)
                    neighborSlots[2] = neighborSlotColliders[i].GetComponent<Slot>();
            }
            else {
                if (relativeSlotPosition.y < 0)
                    neighborSlots[3] = neighborSlotColliders[i].GetComponent<Slot>();
                else if (relativeSlotPosition.y == 0)
                    neighborSlots[4] = neighborSlotColliders[i].GetComponent<Slot>();
                else if (relativeSlotPosition.y > 0)
                    neighborSlots[5] = neighborSlotColliders[i].GetComponent<Slot>();
            }
        }
    }
    #endregion

    public void UpdateIsOpen() {
        isOpen = Physics2D.OverlapPoint(transform.position, Board.pegMask);
    }

    public void UpdateIsOpen(bool open) {
        isOpen = open;
    }

    public void UpdateIsSelectable() {
        isSelectable = false;
        if (!isOpen) {
            for (int i = 0; i < Board.maxNeighborSlots; i++) {
                if (neighborSlots[i] != null) {
                    if (neighborSlots[i].neighborSlots[i] != null) {
                        if (neighborSlots[i].neighborSlots[i].IsOpen) {
                            isSelectable = true;
                        }
                    }
                }
            }
        }
    }

    public void Highlight(HighlightType Highlighting) {
        switch (Highlighting) {
            case HighlightType.Selectable:  selectableParticles.enableEmission = true; break;
            case HighlightType.Receivable:  receivableParticles.enableEmission = true; break;
            case HighlightType.Chosen:      chosenParticles.enableEmission =     true; break;
            case HighlightType.Hover:       hoverParticles.enableEmission =      true; break;
        }
    }

    public void Highlight(HighlightType Highlighting, bool enableHighlighting) {
        switch (Highlighting) {
            case HighlightType.Selectable:  selectableParticles.enableEmission = enableHighlighting; break;
            case HighlightType.Receivable:  receivableParticles.enableEmission = enableHighlighting; break;
            case HighlightType.Chosen:      chosenParticles.enableEmission =     enableHighlighting; break;
            case HighlightType.Hover:       hoverParticles.enableEmission =      enableHighlighting; break;
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Board.slotSeparationDistance);
    }
}
