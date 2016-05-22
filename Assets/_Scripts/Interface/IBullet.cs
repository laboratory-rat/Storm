using System.Collections;
using UnityEngine;

namespace Game
{
    public interface IBullet
    {
        void Init(Vector3 Reletive, float range);

        void Destroy();
    }
}