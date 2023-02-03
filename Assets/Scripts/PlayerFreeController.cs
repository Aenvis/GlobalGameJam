using UnityEngine;

public class PlayerFreeController : BaseController
{
     protected override Vector2 GetMoveDir()
     {
          var x = Input.GetAxisRaw("Horizontal");
          var y = Input.GetAxisRaw("Vertical");
          
          return new (x, y);
     }
}
