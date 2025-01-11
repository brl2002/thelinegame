using System;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public bool IsInvincible { get; private set; }

        private void OnTriggerEnter2D(Collider2D col)
        {
            
        }
    }
}