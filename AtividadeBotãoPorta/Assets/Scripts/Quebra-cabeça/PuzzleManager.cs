using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public List<PuzzlePiece> pieces;
    private PuzzlePiece selectedPiece = null;

    private void Start()
    {
        if (ReplayData.ShouldPlayReplay)
        {
            StartCoroutine(ReplayCoroutine());
            ReplayData.ShouldPlayReplay = false;
        }
        else
        {
            ShufflePieces();
            ReplayData.SwapHistory.Clear(); // limpa se for jogo novo
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

        // Salva os índices trocados
        ReplayData.SwapHistory.Add((a.currentIndex, b.currentIndex));

        StartCoroutine(DelayedCheckVictory());
    }

    public void Replay()
    {
        ReplayData.ShouldPlayReplay = true;
        SceneManager.LoadScene("Puzzle");
    }

    private IEnumerator ReplayCoroutine()
    {
        yield return new WaitForSeconds(1f);

        foreach (var swap in ReplayData.SwapHistory)
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
        ReplayData.SwapHistory.Clear();
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

        SceneManager.LoadScene("MsgVitoria");
    }
}
