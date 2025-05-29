using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    // Usamos static para persistir entre cenas
    private static List<ICommand> savedReplayCommands = new List<ICommand>();
    private static bool shouldPlayReplay = false;

    private List<ICommand> replayCommands = new List<ICommand>();
    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    public List<PuzzlePiece> pieces;
    private PuzzlePiece selectedPiece = null;

    private void Start()
    {
        if (shouldPlayReplay)
        {
            replayCommands = new List<ICommand>(savedReplayCommands);
            StartCoroutine(ReplayCoroutine());
            shouldPlayReplay = false;
            return;
        }

        ShufflePieces();
    }

    void ShufflePieces()
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < pieces.Count; i++)
        {
            indices.Add(i);
        }

        for (int i = 0; i < indices.Count; i++)
        {
            int temp = indices[i];
            int randomIndex = Random.Range(i, indices.Count);
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetIndex(indices[i]);
        }
    }

    public void HandlePieceClick(PuzzlePiece clickedPiece)
    {
        if (selectedPiece == null)
        {
            selectedPiece = clickedPiece;
        }
        else
        {
            if (clickedPiece != selectedPiece)
            {
                SwapPieces(selectedPiece, clickedPiece);
            }

            selectedPiece = null;
        }
    }

    void SwapPieces(PuzzlePiece a, PuzzlePiece b)
    {
        ICommand command = new SwapCommand(a, b);
        command.Execute();
        commandHistory.Push(command);
        replayCommands.Add(command);

        StartCoroutine(DelayedCheckVictory());
    }

    public void Undo()
    {
        if (selectedPiece == null && commandHistory.Count > 0)
        {
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
        }
    }

    public void Replay()
    {
        // Chamado pelo botão da MsgVitoria
        savedReplayCommands = new List<ICommand>(replayCommands);
        shouldPlayReplay = true;
        SceneManager.LoadScene("Puzzle");
    }

    private System.Collections.IEnumerator ReplayCoroutine()
    {
        yield return new WaitForSeconds(1f);
        foreach (ICommand command in replayCommands)
        {
            command.Execute();
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine(DelayedCheckVictory());
    }

    public void ResetPuzzle()
    {
        SceneManager.LoadScene("Puzzle");
    }

    private System.Collections.IEnumerator DelayedCheckVictory()
    {
        yield return null;
        CheckVictory();
    }

    private void CheckVictory()
    {
        foreach (var piece in pieces)
        {
            if (!piece.IsInCorrectPosition())
                return;
        }

        Debug.Log("Vitória detectada! Mudando para MsgVitoria");
        SceneManager.LoadScene("MsgVitoria");
    }
}
