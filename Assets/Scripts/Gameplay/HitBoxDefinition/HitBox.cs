using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitBoxDefinition
{
    [RequireComponent(typeof(Collider))]
    public abstract class HitBox : MonoBehaviour
    {
        [Header("Generic HitBox Settings")]
        [Tooltip("The tag of this damageable gameObject")]
        [SerializeField] private HitBoxTag hitBoxTag;

        public HitBoxTag GetHitBoxTag() => hitBoxTag;

        //public abstract void TakeDamageBy(float damage);
    }
    
}
