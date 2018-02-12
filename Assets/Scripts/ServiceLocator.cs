// Author(s): Paul Calande
// Service locator class.
// Do not retrieve references to services in Awake methods, as they may not be assigned yet.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the player object.")]
    GameObject player;
    [SerializeField]
    [Tooltip("Reference to the Translator component.")]
    Translator translator;
    [SerializeField]
    [Tooltip("Reference to the InputManager component.")]
    InputManager inputManager;

    static GameObject instancePlayer = null;
    static Translator instanceTranslator = null;
    static InputManager instanceInputManager = null;

    private void Awake()
    {
        instancePlayer = player;
        instanceTranslator = translator;
        instanceInputManager = inputManager;
    }

    public static GameObject GetPlayer()
    {
        return instancePlayer;
    }
    public static Translator GetTranslator()
    {
        return instanceTranslator;
    }
    public static InputManager GetInputManager()
    {
        return instanceInputManager;
    }
}