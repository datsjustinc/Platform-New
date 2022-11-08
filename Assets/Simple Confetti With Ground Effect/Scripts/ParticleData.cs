using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace de.enjoyLife.Confetti
{
    public class ParticleData
    {
        private Vector3 _position;
        private float _size;
        private Vector3 _rotation;
        private Color _color;

        public Vector3 Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
            }
        }

        public float Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return _rotation;
            }

            set
            {
                _rotation = value;
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }
    }
}
