using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayButtonHandler : MonoBehaviour
{
    public void OnReplayButtonClicked()
    {
        PuzzleManager.PrepareReplay();
        SceneManager.LoadScene("Puzzle");
    }
}
