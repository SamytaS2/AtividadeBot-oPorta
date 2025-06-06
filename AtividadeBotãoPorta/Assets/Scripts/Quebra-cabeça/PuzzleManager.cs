using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    // Lista estática para salvar os swaps entre cenas para replay
    private static List<(int, int)> savedSwaps = new List<(int, int)>();
    private static bool shouldPlayReplay = false;

    private List<(int, int)> currentSwaps = new List<(int, int)>();
    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    public List<PuzzlePiece> pieces;
    private PuzzlePiece selectedPiece = null;

    private void Start()
    {
        // Busca todas as peças na cena para garantir que a lista esteja preenchida
        pieces = new List<PuzzlePiece>(FindObjectsOfType<PuzzlePiece>());

        if (shouldPlayReplay)
        {
            shouldPlayReplay = false;
            StartCoroutine(ReplayCoroutine());
        }
        else
        {
            savedSwaps.Clear(); // limpa swaps antigos
            ShufflePieces();
        }
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

        // Salva o swap para replay
        currentSwaps.Add((a.currentIndex, b.currentIndex));

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

    public static void PrepareReplay()
    {
        shouldPlayReplay = true;
    }

    // Chamado pelo botão Replay na cena MsgVitoria
    public void Replay()
    {
        savedSwaps = new List<(int, int)>(currentSwaps);
        shouldPlayReplay = true;
        SceneManager.LoadScene("Puzzle");
    }

    private IEnumerator ReplayCoroutine()
    {
        yield return new WaitForSeconds(1f);

        foreach (var swap in savedSwaps)
        {
            var pieceA = pieces.Find(p => p.currentIndex == swap.Item1);
            var pieceB = pieces.Find(p => p.currentIndex == swap.Item2);

            if (pieceA != null && pieceB != null)
            {
                ICommand command = new SwapCommand(pieceA, pieceB);
                command.Execute();

                yield return new WaitForSeconds(1f);
            }
        }

        StartCoroutine(DelayedCheckVictory());
    }

    public void ResetPuzzle()
    {
        savedSwaps.Clear();
        SceneManager.LoadScene("Puzzle");
    }

    private IEnumerator DelayedCheckVictory()
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
