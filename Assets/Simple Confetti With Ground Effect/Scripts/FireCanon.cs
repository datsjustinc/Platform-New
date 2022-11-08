using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace de.enjoyLife.Confetti
{
    public class FireCanon : MonoBehaviour
    {

        [SerializeField]
        ParticleSystem canonFire;

        public void StartFire()
        {
            canonFire.Play();
        }

        public void StopFire()
        {
            canonFire.Stop();
        }
    }
}
