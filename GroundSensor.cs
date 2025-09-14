using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    void OnTriggerStay2D(Collider2D col) { IsGrounded = col.gameObject.layer == LayerMask.NameToLayer("Platform") || col.gameObject.layer == LayerMask.NameToLayer("OneWayPlatform"); }
    void OntriggerExit2D(Collider2D col) { IsGrounded = false; }
}
