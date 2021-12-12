using UnityEngine;

namespace HitBoxDefinition
{
    [RequireComponent(typeof(Collider))]
    public class ReceiveHitBox : HitBox
    {
        public void EnableReceiveHitBox()=> hitBoxCollider.enabled =true;
        public void DisableReceiveHitBox() => hitBoxCollider.enabled = false;
    }
    
}
