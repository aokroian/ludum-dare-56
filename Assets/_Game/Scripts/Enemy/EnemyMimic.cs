using System;
using Deform;
using UnityEngine;

namespace Enemy
{
    public class EnemyMimic : MonoBehaviour
    {
        [SerializeField] private NoiseDeformer deformer;
        [SerializeField] private float deformSpeedMultiplier = 1f;
        

        private void Update()
        {
            var vector = deformer.OffsetVector;
            vector.x = vector.x + Time.deltaTime * deformSpeedMultiplier;
            deformer.OffsetVector = vector;
        }
    }
}