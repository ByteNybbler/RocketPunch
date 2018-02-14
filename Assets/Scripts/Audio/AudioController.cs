// Author(s): Paul Calande
// Script that manages audio channels and plays audio.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The Audio Mixer Group to use for music.")]
    AudioMixerGroup groupMusic;
    [SerializeField]
    [Tooltip("The Audio Mixer Group to use for sound effects.")]
    AudioMixerGroup groupSFX;
}