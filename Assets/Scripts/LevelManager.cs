using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    void Awake() {
        Instance = this;
    }

    public void CheckWinState() {
        if (Board.allSlots.All(slot => !slot.IsSelectable) && Board.pegsRemaining > 1) {
            TextDisplay.Instance.DisplayScore(Board.pegsRemaining);
            Board.allSlots.ForEach(slot => slot.StopRotating());
            StartCoroutine(ClickToRestart());
        }
        else if (Board.pegsRemaining == 1) {
            TextDisplay.Instance.DisplayScore(Board.pegsRemaining);
            StartCoroutine(ClickToRestart());
        }

    }

    private IEnumerator ClickToRestart() {
        yield return new WaitForSeconds(0.1f);
        while (!Input.GetButtonDown("Select")) {
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}
