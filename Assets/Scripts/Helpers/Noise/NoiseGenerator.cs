using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Generate a <see cref="Noise" /> and manage access to samples.
    /// It means that a <see cref="NoiseGenerator" /> store only have one
    /// <see cref="Noise" /> at the same time
    /// </summary>
    public class NoiseGenerator : MonoBehaviour
    {
        public float frequency = 1f;

        [Range(1, 8)]
        public int octaves = 1;

        [Range(1f, 4f)]
        public float lacunarity = 2f;

        [Range(0f, 1f)]
        public float persistence = 0.5f;

        [Range(1, 3)]
        public int dimensions = 3;

        public NoiseMethodType type;

        private Noise _noise;

        private float _stepSize;
        private float _halfSize;

        private Vector3 _point00;
        private Vector3 _point10;
        private Vector3 _point01;
        private Vector3 _point11;

        /// <summary>
        /// Init the <see cref="Noise" />
        /// </summary>
        public void Construct()
        {
            _noise = new Noise();
        }

        /// <summary>
        /// Generate the <see cref="Noise" />. 
        /// </summary>
        /// <param name="resolution">The number of cellula we want</param>
        /// <param name="transform">The transform on which we will position the Noise</param>
        /// <param name="random">The RandomGenerator to control the procuderal Generation</param>
        public void Generate(int resolution, Transform transform, RandomGenerator random)
        {
            _noise.InitWithSeed(random);
            _halfSize = 0.5f;
            _stepSize = (float)_halfSize / resolution;

            _point00 = transform.TransformPoint(new Vector3(-_halfSize, -_halfSize));
            _point10 = transform.TransformPoint(new Vector3(_halfSize, -_halfSize));
            _point01 = transform.TransformPoint(new Vector3(-_halfSize, _halfSize));
            _point11 = transform.TransformPoint(new Vector3(_halfSize, _halfSize));
        }

        /// <summary>
        /// Return the sample value of the <see cref="Noise" /> at the coords given.
        /// </summary>
        /// <param name="x">The abs coord</param>
        /// <param name="y">The ord coord</param>
        /// <returns>A value between 0 and 1</returns>
        public float Get(int x, int y)
        {
            Vector3 point0 = Vector3.Lerp(_point00, _point01, (y + _halfSize) * _stepSize);
            Vector3 point1 = Vector3.Lerp(_point10, _point11, (y + _halfSize) * _stepSize);
           
            Vector3 point = Vector3.Lerp(point0, point1, (x + _halfSize) * _stepSize);
            float sample = Noise.Sum(_noise.methods[(int)type][dimensions - 1], 
                                         point, 
                                         frequency, 
                                         octaves, 
                                         lacunarity, 
                                         persistence).value;

            // scale and offset the noise value to fit in 0 - 1f range
            sample = sample * 0.5f + 0.5f;

            return sample;
        }
    }
}
