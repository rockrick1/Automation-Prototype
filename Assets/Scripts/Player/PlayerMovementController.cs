using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] float _moveSpeed = 5f;
        [SerializeField] Rigidbody2D _rigidBody;
        [SerializeField] Animator _animator;

        Vector2 _movement;

        // Update is called once per frame
        void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            _animator.SetFloat("Horizontal", _movement.x);
            _animator.SetFloat("Vertical", _movement.y);
            _animator.SetFloat("Speed", _movement.sqrMagnitude);
        }

        void FixedUpdate()
        {
            _rigidBody.MovePosition(_rigidBody.position + _movement.normalized * _moveSpeed * Time.fixedDeltaTime);
        }
    }
}