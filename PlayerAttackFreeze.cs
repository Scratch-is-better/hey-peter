using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAttackFreeze : MonoBehaviour
{
    [SerializeField] float freezeDuration = 0.2f;

    PlayerController pc;   
    float freezeUntil;    

    void Awake() => pc = GetComponent<PlayerController>();

    public void OnAttackLanded()
    {
        freezeUntil = Time.time + freezeDuration;
    }

    public bool IsFrozen() => Time.time < freezeUntil;


    void Update()
    {
        if (IsFrozen()) pc.moveInput = Vector2.zero;   
    }
}