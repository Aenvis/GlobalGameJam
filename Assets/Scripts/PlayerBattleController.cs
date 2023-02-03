using System;
using UnityEngine;

public class PlayerBattleController : BaseController
{
    protected override Vector2 GetMoveDir()
    {
        return new (5.0f, 0.0f);
    }
}
