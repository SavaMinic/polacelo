using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Rigidbody myRigidbody;
    
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    #region Public

    public void ResetTo(Vector3 pos, Quaternion? rot = null)
    {
        transform.position = pos;
        transform.rotation = rot ?? Quaternion.identity;
        
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
    }

    #endregion
}
