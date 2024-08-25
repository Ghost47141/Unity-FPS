using UnityEngine;
public class JumpState : PlayerBaseState
{
    public override void Init(PlayerController ctx)
    {
        ctx.velocityY = Mathf.Sqrt(ctx._jumpHeight * -2f * ctx._gravity);
    }
    public override void Iterate(PlayerController ctx)
    {
        Vector3 force = ctx.ProcessMovement(ctx._moveSpeed);
        ctx.charCtrl.Move(ctx.ProcessGravity() + force);
        if(ctx.charCtrl.isGrounded && !ctx.isMoving)
            ctx.SwitchState(ctx.idleState);
        else if(ctx.isCrouching)
            ctx.SwitchState(ctx.crouchState);
        else
            ctx.SwitchState(ctx.moveState);
    }
}