using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PigDead : MonoBehaviour
{
    public float resistance;
    public GameObject explosionPrefab;
    public AudioSource PigFx;
    public AudioClip PigDeath;
    


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.relativeVelocity.magnitude > resistance)
        {
            
            if (explosionPrefab != null)
            {

                PigFx.PlayOneShot(PigDeath, 2);
                var go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(go, 3);
                


            }
            Destroy(gameObject, 0.3f);
        }
        else
        {
            resistance -= col.relativeVelocity.magnitude;
            
        }


    }


}
