using System;
using UnityEngine;

namespace Player
{
    public class PlayerClass : MonoBehaviour
    {
        public PlayerInput PlayerInput { get; private set; }
        public PlayerScore PlayerScore { get; private set; }

        private void Start()
        {
            PlayerInput = gameObject.AddComponent<PlayerInput>();
            PlayerScore = gameObject.AddComponent<PlayerScore>();
        }
    }
}
