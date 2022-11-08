using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace de.enjoyLife.Confetti
{
    public class ParticleGroundPool : MonoBehaviour
    {
        [SerializeField]
        private int maxGroundParticles = 100;

        private int paricleDataIndex = 0;
        private ParticleData[] particleData;
        private ParticleSystem.Particle[] particles;
        private ParticleSystem system;

        // Use this for initialization
        void Start()
        {
            particles = new ParticleSystem.Particle[maxGroundParticles];
            particleData = new ParticleData[maxGroundParticles];
            for (int i = 0; i < maxGroundParticles; i++)
            {
                particleData[i] = new ParticleData();
            }
            system = GetComponent<ParticleSystem>();
        }

        public void ParticleHit(ParticleCollisionEvent particleCollisionEvent, Gradient c, float size)
        {
            SetParticleData(particleCollisionEvent, c,size);
            DisplayParticles();
        }

        void SetParticleData(ParticleCollisionEvent particleCollisionEvent, Gradient c, float size)
        {
            if(paricleDataIndex >= maxGroundParticles)
            {
                paricleDataIndex = 0;
            }

            particleData[paricleDataIndex].Position = particleCollisionEvent.intersection;
            Vector3 paritcleRotationEuler = Quaternion.LookRotation(particleCollisionEvent.normal).eulerAngles;
            paritcleRotationEuler.z = Random.Range(0, 360);
            particleData[paricleDataIndex].Rotation = paritcleRotationEuler;
            particleData[paricleDataIndex].Size = size;
            particleData[paricleDataIndex].Color = c.Evaluate(Random.Range(0f,1f));

           paricleDataIndex++;
        }

        void DisplayParticles()
        {
            for (int i = 0; i < particleData.Length; i++)
            {
                particles[i].position = particleData[i].Position;
                particles[i].rotation3D = particleData[i].Rotation;
                particles[i].startSize = particleData[i].Size;
                particles[i].startColor = particleData[i].Color;

            }
            system.SetParticles(particles, particles.Length);
        }
    }
}