using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private Rigidbody _rigidBody;
    private bool _hit = false;

    private void Start() {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if(!this._hit){
            SpinArrowInTheAir();
            DestroyArrowTimeOut();
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("called");
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(GetComponent<BoxCollider>());
        _hit = true;
    }

    private void SpinArrowInTheAir(){
        // float combinedVelocity = Mathf.Sqrt(_rigidBody.velocity.y * _rigidBody.velocity.z);
        float fallAngle = Mathf.Atan2(_rigidBody.velocity.y, _rigidBody.velocity.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(-fallAngle, transform.rotation.y, transform.rotation.z);
    }

    private void DestroyArrowTimeOut(){
        Destroy(this.gameObject, 5.0f);
    }
}
