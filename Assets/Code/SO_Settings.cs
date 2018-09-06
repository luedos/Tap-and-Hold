using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game settings SO", menuName = "Custom SO")]
public class SO_Settings : ScriptableObject {

    [Tooltip("Scale per second")]
    public float ScaleSpeed;

    [Tooltip("Speed of scaling blocks on perfect move")]
    [Range(0.5f, 8f)]
    public float WaveSpeed = 5f;

    [Tooltip("Delay between each block pulse activation on perfect move")]
    [Range(0.05f, 2f)]
    public float WaveDelay = 0.2f;

    [Tooltip("Range you must get in to make perfect move")]
    [Range(0.01f, 1f)]
    public float PerfectDelta = 0.05f;

    [Tooltip("Make none perfect cyliders smoller by 0.8 on each Perfect Move (it's stupid!)")]
    public bool SmollerOnPerfect = true;

    
    [Range(0.2f,2f)]
    public float CameraOnEndDistance = 1.3f;
}
