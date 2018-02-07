// Author(s): Paul Calande
// Script for player health in Rocket Puncher.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Health component.")]
    Health health;

    private void Start()
    {
        health.Died += Health_Died;
    }

    private void Health_Died()
    {
        UtilScene.ResetScene();
    }
}