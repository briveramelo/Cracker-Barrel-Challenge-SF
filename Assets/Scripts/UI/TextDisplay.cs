using UnityEngine;
using System.Collections;

public class TextDisplay : MonoBehaviour {

    public static TextDisplay Instance;

    [SerializeField] private GameObject removeFirstPeg;
    [SerializeField] private TextMesh endGameText;
    [SerializeField] private GameObject clickToRestart;
    [SerializeField] private GameObject textBackdrop;

    void Awake() {
        Instance = this;
    }


    public void RemoveFirstPegText() {
        removeFirstPeg.SetActive(false);
        textBackdrop.SetActive(false);
    }

    public void DisplayScore(int remainingPegs) {
        string displayText = "";
        bool first = Random.value > 0.5f;
        if (remainingPegs >= 5) {
            displayText = first ? "New HIGH score! " +remainingPegs + " Pegs remaining" :
                                  "... you can see how many pegs you left on there";
        }
        else if (remainingPegs == 4) {
            displayText = first ? remainingPegs + " Pegs remaining? I 'pegged' you as better than that" :
                                  remainingPegs + ", score!";
        }
        else if (remainingPegs == 3) {
            displayText = first ? remainingPegs + " is good for plebs" :
                                  "You left " + remainingPegs +" pegs on the board... You're supposed to leave 1";
        }
        else if (remainingPegs == 2) {
            displayText = first ? "0_0" :
                                  "You should probably stop here.";
        }
        else if (remainingPegs == 1) {
            displayText = first ? "WOWOWOWO I thought only computers and uber nerds could beat this! ..." :
                                  "Go play outside already";
        }
        endGameText.text = displayText;
        textBackdrop.SetActive(true);
        StartCoroutine(ClickToRestartText());

    }

    IEnumerator ClickToRestartText() {
        yield return new WaitForSeconds(1f);
        clickToRestart.SetActive(true);
    }
}
