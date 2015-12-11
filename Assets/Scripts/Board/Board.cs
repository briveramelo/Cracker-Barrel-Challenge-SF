using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour{

    [SerializeField] private Animator boardAnimator;
    [SerializeField] private AudioSource stereo;
    [SerializeField] private AudioClip nightroad;
    [SerializeField] private AudioClip lakeside;
    [SerializeField] private AudioClip hometree;

    public static Board Instance;
    public static int pegsRemaining;
    public static int maxNeighborSlots = 6;
    public static float slotSeparationDistance = 10f;

    public static int handMask = LayerMask.NameToLayer("Hand");
    public static int slotMask = LayerMask.NameToLayer("Slot");
    public static int pegMask = LayerMask.NameToLayer("Peg");

    public static List<Slot> allSlots;
    private int lifeCount;

    private enum BoardType {
        Escher =0,
        Floyd = 1,
        Fractal =2
    }
    private BoardType LastBoard;

    void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Initialize();
        }
        else{
            Destroy(gameObject);
        }
    }

    void OnLevelWasLoaded() {
        Initialize();
    }

    void Initialize() {
        pegsRemaining = 14;
        allSlots = new List<Slot>(FindObjectsOfType<Slot>());
        if (lifeCount < 3) {
            ChooseBoard((BoardType)lifeCount);
        }
        else {
            ChooseRandomBoard();
        }
        lifeCount++;
    }

    void ChooseRandomBoard() {
        BoardType TrialBoard;
        float randomValue = Random.value;
        if (randomValue < 0.33f)        TrialBoard = BoardType.Escher;
        else if (randomValue < 0.67f)   TrialBoard = BoardType.Floyd;
        else                            TrialBoard = BoardType.Fractal;

        if (TrialBoard == LastBoard) {
            ChooseRandomBoard();
            return;
        }

        ChooseBoard(TrialBoard);
    }

    void ChooseBoard(BoardType ChosenBoard) {
        switch (ChosenBoard) {
            case BoardType.Escher:
                boardAnimator.SetInteger("AnimState", (int)BoardType.Escher);
                stereo.clip = lakeside;
                stereo.Play();
                LastBoard = BoardType.Escher;
                break;
            case BoardType.Floyd:
                boardAnimator.SetInteger("AnimState", (int)BoardType.Floyd);
                stereo.clip = nightroad;
                stereo.Play();
                LastBoard = BoardType.Floyd;
                break;
            case BoardType.Fractal:
                boardAnimator.SetInteger("AnimState", (int)BoardType.Fractal);
                stereo.clip = hometree;
                stereo.Play();
                LastBoard = BoardType.Fractal;
                break;
        }
    }

    public static void DecreasePegCount() {
        pegsRemaining--;
    }

}
