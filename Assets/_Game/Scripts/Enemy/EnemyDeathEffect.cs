using System;
using Deform;
using DG.Tweening;
using Level;
using UnityEngine;

namespace Enemy
{
    public class EnemyDeathEffect : MonoBehaviour
    {
        [SerializeField] private MeltDeformer _meltDeformer;
        [SerializeField] private NoiseDeformer _noiseDeformer;
        
        private Deformable _deformable;

        public void PlayDeathEffect(Prop prop, Action onFinished)
        {
            prop.isDeadEnemy = true;
            var meshObject = prop.CurrentVariation;
            _deformable = meshObject.gameObject.AddComponent<Deformable>();
            
            _deformable.DeformerElements.Add(new DeformerElement(_meltDeformer));
            _deformable.DeformerElements.Add(new DeformerElement(_noiseDeformer));

            var meshRenderer = meshObject.GetComponent<MeshRenderer>();
            var bounds = meshRenderer.bounds;
            var bottomPoint = bounds.center - new Vector3(0, bounds.extents.y - 0.1f, 0);
            _meltDeformer.transform.position = bottomPoint - new Vector3(0, _meltDeformer.Top, 0);
            _noiseDeformer.MagnitudeScalar = 0.2f;
            
            var newMat = new Material(meshRenderer.material);
            meshRenderer.material = newMat;
            var seq = DOTween.Sequence();
            seq.Append(_meltDeformer.transform.DOMove(bottomPoint, 1f).SetEase(Ease.OutCubic));
            seq.Join(meshObject.transform
                .DOMove(meshObject.transform.position - new Vector3(0, bounds.size.y * 0.6f, 0), 1f)
                .SetEase(Ease.OutCubic));
            seq.Join(DOTween.To(() => _noiseDeformer.MagnitudeScalar, x => _noiseDeformer.MagnitudeScalar = x, 0, 1f).SetEase(Ease.OutBounce));
            seq.Join(newMat.DOColor(Color.black, 0.2f));
            seq.OnComplete(() =>
            {
                onFinished?.Invoke();
            });
        }

        private void OnDestroy()
        {
            Destroy(_deformable);
        }
    }
}