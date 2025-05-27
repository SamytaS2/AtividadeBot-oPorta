using UnityEngine;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour
{
    public int correctIndex; // Índice correto da peça (0 a 15, por exemplo)
    public int currentIndex; // Índice atual no tabuleiro (posição embaralhada)
    
    public void SetIndex(int index)
    {
        currentIndex = index;
        transform.SetSiblingIndex(index); // Se estiver usando UI com GridLayoutGroup
    }

    public bool IsInCorrectPosition()
    {
        return currentIndex == correctIndex;
    }
}
