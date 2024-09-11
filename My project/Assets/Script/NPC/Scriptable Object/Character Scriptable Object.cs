using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SlimeProject
{
    [CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/Character", order = int.MaxValue)]
    public class CharacterScriptableObject : ScriptableObject
    {
        public int Hp;
        public float Damage;
        public float AtkInterval;
        public float Speed;

    }

}