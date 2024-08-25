using UnityEngine;
public class MoveState : PlayerBaseState
{
    public override void Init(PlayerController ctx)
    {
    }

    public override void Iterate(PlayerController ctx)
    {
        Vector3 force = ctx.ProcessMovement(ctx._moveSpeed);
        ctx.charCtrl.Move(ctx.ProcessGravity() + force);
        if(force == Vector3.zero)
            ctx.SwitchState(ctx.idleState);
        else if(ctx.isJumping && ctx.charCtrl.isGrounded)
            ctx.SwitchState(ctx.jumpState);
        else if(ctx.isCrouching)
            ctx.SwitchState(ctx.crouchState);
    }
}