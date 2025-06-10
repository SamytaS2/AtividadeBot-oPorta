using UnityEngine;

public class SwapCommand : ICommand
{
    private PuzzlePiece a, b;

    public SwapCommand(PuzzlePiece a, PuzzlePiece b)
    {
        this.a = a;
        this.b = b;
    }

    public void Execute()
    {
        Swap(a, b);
    }

    public void Undo()
    {
        Swap(a, b); // Desfazer é trocar novamente
    }

    private void Swap(PuzzlePiece x, PuzzlePiece y)
    {
        Vector3 tempPos = x.transform.position;
        x.transform.position = y.transform.position;
        y.transform.position = tempPos;

        int tempIndex = x.currentIndex;
        x.SetIndex(y.currentIndex);
        y.SetIndex(tempIndex);
    }

    
}
