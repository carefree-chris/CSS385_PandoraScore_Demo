using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTimer : MonoBehaviour
{
    public float lifespan;

    private void Awake()
    {
        GameObject.Destroy(this.gameObject, lifespan);
    }
}
