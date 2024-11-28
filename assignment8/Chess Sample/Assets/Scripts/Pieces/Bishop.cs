using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        int d = Mathf.Max(Utils.FieldWidth, Utils.FieldHeight);
        return new MoveInfo[]
        {
            new MoveInfo(-1, -1, d),
            new MoveInfo(-1, 1, d),
            new MoveInfo(1, -1, d),
            new MoveInfo(1, 1, d)
        };
        // ------
    }
}