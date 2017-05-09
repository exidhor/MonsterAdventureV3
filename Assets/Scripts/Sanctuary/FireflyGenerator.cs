using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class FireflyGenerator : MonoBehaviour
    {
        public float Range;
        public Firefly Prefab;
        public float SpawnTime;
        public float CurrentTime;

        public Vector2 SpeedRange;
        public Vector2 ShiningSpeedRange;
        public Vector2 ScaleRange;

        void Update()
        {
            CurrentTime += Time.deltaTime;

            if (CurrentTime > SpawnTime)
            {
                CurrentTime = 0;
                InstantiateFirefly();
            }
        }

        private Firefly InstantiateFirefly()
        {
            Firefly firefly = Instantiate(Prefab).GetComponent<Firefly>();
            firefly.transform.parent = transform.transform;

            firefly.transform.rotation = transform.rotation;
            firefly.transform.position = transform.position;

            float speed = RandomGenerator.Instance.NextFloat(SpeedRange.x, SpeedRange.y);
            float shiningSpeed = RandomGenerator.Instance.NextFloat(ShiningSpeedRange.x, ShiningSpeedRange.y);
            float scale = RandomGenerator.Instance.NextFloat(ScaleRange.x, ScaleRange.y);

            float dephase = RandomGenerator.Instance.NextFloat(10);

            firefly.SetUp(speed, shiningSpeed, scale, dephase);

            float offsetX = RandomGenerator.Instance.NextFloat(-Range, Range);

            firefly.transform.localPosition = new Vector3(offsetX, 0, 0);

            return firefly;
        }
    }
}
