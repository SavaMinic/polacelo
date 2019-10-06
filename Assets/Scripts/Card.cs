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
            StartFalling();
            other.gameObject.GetComponent<Card>().StartFalling();
        
            GameManager.I.CardsClashed();
        }
    }

    #endregion

    #region Public

    public void StartFalling()
    {
        isFalling = true;
        myRigidbody.useGravity = true;
    }

    public void AddForce(Vector3 direction, float power)
    {
        myRigidbody.AddForce(direction * power);
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
