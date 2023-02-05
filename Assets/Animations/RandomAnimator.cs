using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Script
{
    public class RandomAnimator : MonoBehaviour
    {
        private Animator _animaotr;

        void Start()
        {
            _animaotr = GetComponent<Animator>();
            var state = _animaotr.GetCurrentAnimatorStateInfo(0);
            _animaotr.Play(state.fullPathHash,0,Random.Range(0f,0.3f));
        }
    }

}
