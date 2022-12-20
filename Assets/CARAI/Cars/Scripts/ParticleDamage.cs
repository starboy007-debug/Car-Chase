using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public int damage = 20;
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag + "particle");
        other.GetComponent<Target>().TakeDamage(damage);
    }
}
