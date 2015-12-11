using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Slot : MonoBehaviour {

    [SerializeField] private ParticleSystem hoverParticles;
    [SerializeField] private ParticleSystem chosenParticles;
    [SerializeField] private ParticleSystem receivableParticles;
    [SerializeField] private ParticleSystem selectableParticles;

	/*[HideInInspector]*/ public Slot[] neighborSlots;
    /*[HideInInspector]*/ public Peg myPeg;
    [HideInInspector] public List<Slot> jumpableNeighborSlots;
    [HideInInspector] public List<Slot> availableSecondNeighborSlots;

    public bool isOpen;       public bool IsOpen       { get { return isOpen; }       set { isOpen = value; } }
    public bool isSelectable; public bool IsSelectable { get { return isSelectable; } set { isSelectable = value; } }
    public bool isReceivable; public bool IsReceivable { get { return isReceivable; } set { isReceivable = value; } }
    public bool isChosen;     public bool IsChosen     { get { return isChosen; }     set { isChosen = value; } }
    public bool isJumpable;   public bool IsJumpable   { get { return isJumpable; }   set { isJumpable = value; } }
    private Collider2D myCollider;

    void Awake() {
        myCollider = GetComponent<Collider2D>();
        AssignNeighborSlots();
        UpdateIsOpen();
    }
    void Start() {
        UpdateIsSelectable();
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

    void UpdateIsOpen() {
        isOpen = !Physics2D.OverlapPointAll(transform.position).Any(col => col.gameObject.layer == Board.pegMask);
    }

    public void UpdateNewPeg(Peg newPeg, bool gotANewPeg) {
        isOpen = !gotANewPeg;
        myPeg = newPeg;
    }

    public void UpdateIsSelectable() {
        isSelectable = false;
        if (!isOpen) {
            for (int i = 0; i < Board.maxNeighborSlots; i++) {
                if (neighborSlots[i] != null) {
                    if (!neighborSlots[i].IsOpen) {
                        if (neighborSlots[i].neighborSlots[i] != null) {
                            if (neighborSlots[i].neighborSlots[i].IsOpen) {
                                isSelectable = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void Highlight(HighlightType Highlighting) {
        
        switch (Highlighting) {
            case HighlightType.Selectable:
                ParticleSystem.EmissionModule em1 = selectableParticles.emission;
                em1.enabled = true;
                break;
            case HighlightType.Receivable:
                ParticleSystem.EmissionModule em2 = receivableParticles.emission;
                em2.enabled = true;
                break;
            case HighlightType.Chosen:
                ParticleSystem.EmissionModule em3 = chosenParticles.emission;
                em3.enabled = true;
                break;
            case HighlightType.Hover:
                ParticleSystem.EmissionModule em4 = hoverParticles.emission;
                em4.enabled = true;
                break;
        }
    }

    public void Highlight(HighlightType Highlighting, bool enableHighlighting) {
        switch (Highlighting) {
            case HighlightType.Selectable:
                ParticleSystem.EmissionModule em1 = selectableParticles.emission;
                em1.enabled = enableHighlighting;
                break;
            case HighlightType.Receivable:
                ParticleSystem.EmissionModule em2 = receivableParticles.emission;
                em2.enabled = enableHighlighting;
                break;
            case HighlightType.Chosen:
                ParticleSystem.EmissionModule em3 = chosenParticles.emission;
                em3.enabled = enableHighlighting;
                break;
            case HighlightType.Hover:
                ParticleSystem.EmissionModule em4 = hoverParticles.emission;
                em4.enabled = enableHighlighting;
                break;
        }
    }

}
