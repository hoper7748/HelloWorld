using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeProject
{
    public class BaseState
    {
        public FSMController FSMController;
        public BaseState(FSMController fSMController)
        {
            FSMController = fSMController;
        }

        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {

        }
    }
}