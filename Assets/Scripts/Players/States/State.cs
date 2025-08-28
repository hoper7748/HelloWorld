using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    protected Entity entity;
    public State(Entity entity)
    {
        this.entity = entity;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

}
