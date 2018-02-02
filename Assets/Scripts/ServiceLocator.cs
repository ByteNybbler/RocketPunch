// Author(s): Paul Calande
// Service locator class.
// Do not retrieve references to services in Awake methods, as they may not be assigned yet.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    /*
    [SerializeField]
    [Tooltip("Reference to the AchievementManager component.")]
    AchievementManager achievementManager;
    */
    [SerializeField]
    [Tooltip("Reference to the Translator component.")]
    Translator translator;
    [SerializeField]
    [Tooltip("Reference to the InputManager component.")]
    InputManager inputManager;

    //static AchievementManager instanceAchievementManager = null;
    static Translator instanceTranslator = null;
    static InputManager instanceInputManager = null;

    private void Awake()
    {
        //instanceAchievementManager = achievementManager;
        instanceTranslator = translator;
        instanceInputManager = inputManager;
    }

    /*
    public static AchievementManager GetAchievementManager()
    {
        return instanceAchievementManager;
    }
    */
    public static Translator GetTranslator()
    {
        return instanceTranslator;
    }
    public static InputManager GetInputManager()
    {
        return instanceInputManager;
    }
}