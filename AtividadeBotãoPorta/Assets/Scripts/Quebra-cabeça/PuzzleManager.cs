using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public List<Transform> pieces; // Arraste aqui os GameObjects das peças na ordem correta

    private void Start()
    {
        ShufflePieces();
    }

    void ShufflePieces()
    {
        // Criar uma cópia das posições
        List<Vector3> positions = new List<Vector3>();
        foreach (Transform piece in pieces)
        {
            positions.Add(piece.position);
        }

        // Embaralhar posições
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 temp = positions[i];
            int randomIndex = Random.Range(i, positions.Count);
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // Aplicar posições embaralhadas às peças
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].position = positions[i];
        }
    }
}
