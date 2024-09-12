using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

namespace SlimeProject
{
    public class FSMController
    {
        public CharacterScriptableObject CharacterSO;
        public BaseState currentState;
        public IdleState IdleState;
        public AttackState AttackState;

        public Transform transform;
        public LayerMask targetLayer;

        public Arrow arrowPrefab;

        public FSMController(Character character)
        {
            CharacterSO = character.CharacterSO;
            arrowPrefab = (character as BossMonster)?.ArrowPrefab;

            IdleState = new IdleState(this);
            AttackState = new AttackState(this);

            transform = character.transform;
            targetLayer = character.targetLayer;

            currentState = IdleState;
            currentState.Enter();

        }

        public void ChangeState(BaseState state)
        {
            currentState.Exit();
            currentState = state;
            currentState.Enter();
        }

        public void StateUpdate()
        {
            currentState.Update();
        }
    }
}