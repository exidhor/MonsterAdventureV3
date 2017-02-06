using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Sector : MonoBehaviour
    {
        public Rect Bounds
        {
            get { return _bounds; }
        }

        public List<TracedObject> TracedObjects;

        private Rect _bounds;

        private Coords _coords;

        private bool _isVisible;

        private PoolAllocator _poolAllocator;
        //private IEnumerator _coroutine;

        public void Construct(Rect bounds, Coords coords, PoolAllocator poolAllocator)
        {
            _bounds = bounds;
            transform.position = _bounds.position;

            _poolAllocator = poolAllocator;
            _coords = coords;

            name = "Sector (" + coords.abs + ", " + coords.ord + ")";

            TracedObjects = new List<TracedObject>();
        }

        public void AddTracedObject(TracedObject tracedObject)
        {
            TracedObjects.Add(tracedObject);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }

        public void SetIsVisible(bool isVisible)
        {
            if (_isVisible != isVisible)
            {
                _isVisible = isVisible;

                if (isVisible)
                {
                    //_coroutine = EnableTracedObjects();
                    //StartCoroutine(_coroutine);
                    EnableTracedObjects();
                }
                else
                {
                    //_coroutine = DisableTracedObjects();
                    //StartCoroutine(_coroutine);
                    DisableTracedObjects();
                }
            }
        }
    
        /*
        private IEnumerator EnableTracedObjects()
        {
            int instanciationPerFrame = TracedObjects.Count / 30;
            int currentInstanciation = 0;

            for (int i = 0; i < TracedObjects.Count; i++)
            {
                TracedObjects[i].Instantiate();

                currentInstanciation++;

                if(currentInstanciation >= instanciationPerFrame)
                {
                    currentInstanciation = 0;
                    yield return null;
                }
            }
        }

        private IEnumerator DisableTracedObjects()
        {
            int releasePerFrame = TracedObjects.Count / 30;
            int currentRelease = 0;

            for (int i = 0; i < TracedObjects.Count; i++)
            {
                TracedObjects[i].Release();

                currentRelease++;

                if (currentRelease >= releasePerFrame)
                {
                    currentRelease = 0;
                    yield return null;
                }
            }
        }*/
        
        private void EnableTracedObjects()
        {
            //Debug.Log("Instanciate Sector " + _coords);
            _poolAllocator.AddPoolRequest(TracedObjects, PoolRequestAction.Allocate);
        }

        private void DisableTracedObjects()
        {
            //Debug.Log("Release Sector " + _coords);
            _poolAllocator.AddPoolRequest(TracedObjects, PoolRequestAction.Free);
        }

        public Coords GetCoords()
        {
            return _coords;
        }

        /*
        public void ConstructTile(uint tileCount, Tile tilePrefab, float tileSize)
        {
            _tiles = new Tile[tileCount, tileCount];

            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    _tiles[i, j] = InstanciateTile(new Coords(i, j), tilePrefab, tileSize);
                }
            }
        }

        private Tile InstanciateTile(Coords coordsInSector, Tile tilePrefab, float tileSize)
        {
            Tile tile = Instantiate<Tile>(tilePrefab);
            tile.transform.parent = gameObject.transform;

            Vector2 position = ComputeTilePosition(coordsInSector, tileSize);

            tile.Construct(coordsInSector, position);

            return tile;
        }

        private Vector2 ComputeTilePosition(Coords coordsInSector, float tileSize)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);

            position.x += coordsInSector.abs*tileSize;
            position.y += coordsInSector.ord*tileSize;

            return position;
        }*/
    }
}