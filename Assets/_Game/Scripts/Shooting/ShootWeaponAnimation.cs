using DG.Tweening;
using UnityEngine;

namespace Shooting
{
    public class ShootWeaponAnimation : MonoBehaviour
    {
        [SerializeField] private Transform weaponT;

        private Sequence _sequence;

        private Vector3 _weaponInitialPosition;
        private Vector3 _weaponInitialRotation;

        private void Awake()
        {
            _weaponInitialPosition = weaponT.localPosition;
            _weaponInitialRotation = weaponT.localEulerAngles;
        }

        public void PlayShootAnim()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Join(weaponT.DOLocalMoveZ(-0.08f, 0.1f).SetEase(Ease.OutCirc));
            _sequence.Join(weaponT.DOLocalRotate(new Vector3(-20, 0, 0), 0.1f));
            _sequence.Join(weaponT.DOLocalRotate(_weaponInitialRotation, 0.1f));
            _sequence.Join(weaponT.DOLocalMoveZ(_weaponInitialPosition.z, 0.1f));
        }
    }
}