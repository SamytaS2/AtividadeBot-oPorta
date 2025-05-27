using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    private List<ICommand> replayCommands = new List<ICommand>();

    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    public List<PuzzlePiece> pieces; // Agora usamos PuzzlePiece diretamente
    private PuzzlePiece selectedPiece = null;

    private void Start()
    {
        ShufflePieces();
    }

    void ShufflePieces()
    {
        // Cria uma lista de índices
        List<int> indices = new List<int>();
        for (int i = 0; i < pieces.Count; i++)
        {
            indices.Add(i);
        }

        // Embaralha os índices
        for (int i = 0; i < indices.Count; i++)
        {
            int temp = indices[i];
            int randomIndex = Random.Range(i, indices.Count);
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // Aplica a nova ordem visual (GridLayout usa SiblingIndex)
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetIndex(indices[i]); // atualiza currentIndex e posição visual
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
        CheckVictory(); 
    }

    public void Undo()
    {
        if (selectedPiece == null && commandHistory.Count > 0)
        {
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
        }
    }

    public void StartReplay()
    {
        StartCoroutine(ReplayCoroutine());
    }

    private System.Collections.IEnumerator ReplayCoroutine()
    {
        // Resetar o puzzle
        ResetPuzzle();

        // Esperar um segundo antes de começar
        yield return new WaitForSeconds(1f);

        foreach (ICommand command in replayCommands)
        {
            command.Execute();
            yield return new WaitForSeconds(1f);
        }

    }

    public void ResetPuzzle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
