using UnityEngine;

namespace SlimeProject
{
    public abstract class CustomCollision : MonoBehaviour
    {
        public virtual void OverlapBoxNonAlloc(Vector3 point, Vector3 size, CustomCollision[] Collision, LayerMask layer)
        {

        }
    }
}