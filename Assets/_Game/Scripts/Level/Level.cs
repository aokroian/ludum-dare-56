using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        [field: SerializeField] public List<PropSpawnSurface> PropSpawnSurfaces { get; private set; }
        [field: SerializeField] public List<Prop> Props { get; private set; }
    }
}