using UnityEngine;
public abstract class PlayerBaseState {
    public abstract void Init(PlayerController ctx);
    public abstract void Iterate(PlayerController ctx);
}