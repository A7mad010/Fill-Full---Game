using System;
using Game;
using UnityEngine;
using SmoothShakeFree;

namespace Effects
{
    /// <summary>
    /// Use this class to shake the camera
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private SmoothShake cameraShake;
        
        [Header("Link with")]
        [SerializeField] private MatchManager matchManager;

        private void Start()
        {
            matchManager.OnUnMatch += StartShake;
        }
        
        private void StartShake()
        {
            cameraShake.StartShake();
        }
    }
}
