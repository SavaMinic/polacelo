using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    #region Fields

    private Rigidbody myRigidbody;
    private int cardLayerIndex;

    private bool isFalling;

    #endregion

    #region Mono
    
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        cardLayerIndex = LayerMask.NameToLayer("card");
    }

    #endregion

    #region Triggers

    private void OnCollisionEnter(Collision other)
    {
        if (isFalling)
            return;
        
        if (other.gameObject.layer == cardLayerIndex)
        {
            isFalling = true;
            myRigidbody.useGravity = true;
        
            GameManager.I.StartCallFalling();
        }
    }

    #endregion

    #region Public

    public void SetUseGravity(bool useGravity)
    {
        myRigidbody.useGravity = useGravity;
    }

    public void ResetTo(Vector3 pos, Quaternion? rot = null)
    {
        isFalling = false;
        
        transform.position = pos;
        transform.rotation = rot ?? Quaternion.identity;
        
        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
        myRigidbody.useGravity = false;
    }

    #endregion
}
