using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    public int correctIndex;
    public int currentIndex;

    public void SetIndex(int index)
    {
        currentIndex = index;
        transform.SetSiblingIndex(index); // se estiver usando UI com GridLayoutGroup
    }

    public bool IsInCorrectPosition()
    {
        return currentIndex == correctIndex;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleManager manager = FindFirstObjectByType<PuzzleManager>();
        manager.HandlePieceClick(this);
    }
}
