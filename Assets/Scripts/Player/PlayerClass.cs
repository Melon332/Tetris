using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerScore))]
    public class PlayerClass : MonoBehaviour
    {
        public PlayerInput PlayerInputClass { get; private set; }
        public PlayerScore PlayerScoreClass { get; private set; }

        private void Awake()
        {
            PlayerInputClass = GetComponent<PlayerInput>();
            PlayerScoreClass = GetComponent<PlayerScore>();
        }
    }
}
