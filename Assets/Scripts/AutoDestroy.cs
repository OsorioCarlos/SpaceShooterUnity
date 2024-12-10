using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Variables publicas desde el Editor de Unity
    [SerializeField] private float timeLife;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timeLife);
    }
}
