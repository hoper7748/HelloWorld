using UnityEngine;

namespace SlimeProject
{
    public class BoxCollision : CustomCollision
    {
        public override void OverlapBoxNonAlloc(Vector3 point, Vector3 size, CustomCollision[] Collision, LayerMask layer)
        {
            if(Collision != null)
            {
                
            }
            
        }
    }

}