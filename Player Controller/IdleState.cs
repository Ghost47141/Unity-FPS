using System;
using UnityEngine;
public class IdleState : PlayerBaseState {
    public override void Init(PlayerController ctx) 
    {
    }
    public override void Iterate(PlayerController ctx) 
    {
        ctx.charCtrl.Move(ctx.ProcessGravity());
        if(ctx.isMoving)
            ctx.SwitchState(ctx.moveState);
        else if(ctx.isJumping && ctx.charCtrl.isGrounded)
            ctx.SwitchState(ctx.jumpState);
        else if(ctx.isCrouching)
            ctx.SwitchState(ctx.crouchState);
    }
}