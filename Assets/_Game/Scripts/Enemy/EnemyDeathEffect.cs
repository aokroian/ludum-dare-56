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
        
        // Properties to reset on destroy
        private MeshRenderer _meshRenderer;
        private Material _material;
        private Vector3 _position;

        public void PlayDeathEffect(Prop prop, Action onFinished)
        {
            prop.IsDeadEnemy = true;
            var meshObject = prop.CurrentVariation;
            _deformable = meshObject.gameObject.AddComponent<Deformable>();
            
            _deformable.DeformerElements.Add(new DeformerElement(_meltDeformer));
            _deformable.DeformerElements.Add(new DeformerElement(_noiseDeformer));

            _meshRenderer = meshObject.GetComponent<MeshRenderer>();
            var bounds = _meshRenderer.bounds;
            var bottomPoint = bounds.center - new Vector3(0, bounds.extents.y - 0.1f, 0);
            _meltDeformer.transform.position = bottomPoint - new Vector3(0, _meltDeformer.Top, 0);
            _noiseDeformer.MagnitudeScalar = 0.2f;

            _position = _meshRenderer.transform.position;
            _material = _meshRenderer.material;
            var newMat = new Material(_material);
            _meshRenderer.material = newMat;
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
            if (_meshRenderer != null && _material != null)
            {
                _meshRenderer.transform.position = _position;
                _meshRenderer.material = _material;
            }
            Destroy(_deformable);
        }
    }
}