using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Índice correto da peça (posição final desejada)")]
    public int correctIndex;

    [Tooltip("Índice atual da peça na hierarquia")]
    public int currentIndex;

    /// <summary>
    /// Define o índice atual da peça e ajusta a posição na UI (GridLayout usa SiblingIndex).
    /// </summary>
    public void SetIndex(int index)
    {
        currentIndex = index;
        transform.SetSiblingIndex(index);
    }

    /// <summary>
    /// Retorna true se a peça estiver na posição correta.
    /// </summary>
    public bool IsInCorrectPosition()
    {
        return currentIndex == correctIndex;
    }

    /// <summary>
    /// Detecta o clique na peça e envia o evento para o PuzzleManager.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleManager manager = FindFirstObjectByType<PuzzleManager>();

        if (manager != null)
        {
            manager.HandlePieceClick(this);
        }
        else
        {
            Debug.LogWarning("PuzzleManager não encontrado!");
        }
    }
}
