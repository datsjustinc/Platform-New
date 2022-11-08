using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace de.enjoyLife.Confetti
{
    public class ParticleCollision : MonoBehaviour
    {
        [SerializeField]
        private ParticleGroundPool pool;

        List<ParticleCollisionEvent> collisionEvents;
        private ParticleSystem system;

        private void Start()
        {
            collisionEvents = new List<ParticleCollisionEvent>();
            system = GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            ParticlePhysicsExtensions.GetCollisionEvents(system, other, collisionEvents);
            for (int i = 0; i < collisionEvents.Count; i++)
            {
                pool.ParticleHit(collisionEvents[i], system.main.startColor.gradient, system.main.startSize.constant);
            }
        }
    }
}