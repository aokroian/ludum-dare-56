using DG.Tweening;
using UnityEngine;

namespace Cutscenes
{
    public class GameStartCameraAnimation
    {
        public void PlayAnim(
            Camera camera,
            Vector3 cameraTarget,
            float duration)
        {
            camera.DOKill();
            camera.transform.DOMove(cameraTarget, duration);
            // ping pong rotation by z axis
            camera.transform.DORotate(new Vector3(0, 0, 10), duration / 3).SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}