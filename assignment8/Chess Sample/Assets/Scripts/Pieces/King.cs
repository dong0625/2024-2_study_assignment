using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// King.cs
public class King : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        int d = 1;
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
