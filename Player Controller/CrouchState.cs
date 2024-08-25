using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class CrouchState : PlayerBaseState
{
    Transform camera;
    public override void Init(PlayerController ctx)
    {
        camera = Camera.main.transform;
        ctx.StartCoroutine(CrouchStand(ctx));
    }

    public override void Iterate(PlayerController ctx)
    {
        Vector3 force = ctx.ProcessMovement(ctx._crouchSpeed);
        ctx.charCtrl.Move(ctx.ProcessGravity() + force);
        if(ctx.inCrouchMode && ctx.isCrouching && !Physics.Raycast(camera.position, Vector3.up,1f)) {
            ctx.SwitchState(ctx.crouchState);
        }
    }
    IEnumerator CrouchStand(PlayerController ctx) {
        float timeElapsed = 0;
        float targetHeight = ctx.inCrouchMode ? ctx._standingHeight : ctx._crouchingHeight;
        float currentHeight =  ctx.charCtrl.height;
        Vector3 targetCenter = ctx. inCrouchMode ? ctx._standingCenter : ctx._crouchingCenter;
        Vector3 currentCenter =  ctx.charCtrl.center;
        while(timeElapsed <  ctx._crouchTime) {
            ctx.charCtrl.height = Mathf.Lerp(currentHeight,targetHeight,timeElapsed/ctx._crouchTime);
            ctx.charCtrl.center = Vector3.Lerp(currentCenter,targetCenter,timeElapsed/ctx._crouchTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        ctx.charCtrl.height = targetHeight;
        ctx.charCtrl.center = targetCenter;
        ctx.inCrouchMode = !ctx.inCrouchMode;
        if(!ctx.inCrouchMode) {
            ctx.SwitchState(ctx.idleState);
        }
    }
    // Can only exit this state after fully standing up.
}