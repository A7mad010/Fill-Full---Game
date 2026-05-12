using System;
using UnityEngine;

namespace Game.Wall
{
    public class WallMove : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed;
        [SerializeField] private Vector3 direction;

        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }
}
