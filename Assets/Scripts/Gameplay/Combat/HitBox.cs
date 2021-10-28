using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;

public abstract class HitBox : MonoBehaviour
{
    [Tooltip("The tag of this damageable gameObject")]
    [SerializeField] private HitBoxTag selfHitBoxTag;

    public HitBoxTag GetSelfHitBoxTag() => selfHitBoxTag;

    public abstract void TakeDamageBy(float damage);
}
