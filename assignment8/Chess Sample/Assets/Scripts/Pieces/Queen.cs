using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Queen : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        int d = Mathf.Max(Utils.FieldWidth, Utils.FieldHeight);
        return new MoveInfo[]
        {
            new MoveInfo(-1, -1, d),
            new MoveInfo(-1, 0, d),
            new MoveInfo(-1, 1, d),
            new MoveInfo(0, -1, d),
            new MoveInfo(0, 1, d),
            new MoveInfo(1, -1, d),
            new MoveInfo(1, 0, d),
            new MoveInfo(1, 1, d)
        };
        // ------
    }
}