using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnHit(float damage, Vector3 hitPoint, Vector3 hitNormal, float knockBackForce);
}
