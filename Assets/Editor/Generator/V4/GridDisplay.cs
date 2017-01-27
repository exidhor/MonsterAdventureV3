using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.Generation;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GridDisplay : EditorWindow
    {
        public delegate void DrawOnBox(int x, int y, out Color color, out string text);

        private Vector2 _positionForScrollView;

        private Rect _baseViewRect;
        private Rect _viewRect;
        private Rect _scrolPosition;

        private float _zoom = 3;
        private Rect _positionZoomSlider;
        private int _offset = 5;

        private uint _size;

        private DrawOnBox _drawOnBox;

        /*[MenuItem("Window/Grid Display")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            GridDisplay instance = (GridDisplay)EditorWindow.GetWindow(typeof(GridDisplay));
            instance.titleContent = new GUIContent("Grid Display");

            instance.Show();
        }*/

        private void OnEnable()
        {
            _baseViewRect = new Rect(0, 0, 500, 500);
            _viewRect = new Rect(_baseViewRect);
            UpdateScrollPosition();
            _positionZoomSlider = new Rect(_viewRect.xMax + _offset, 
                _viewRect.yMin, 
                5 + _offset, 
                _viewRect.yMax - _scrolPosition.height);
        }

        public void SetDatas(uint size, DrawOnBox drawOnBox)
        {
            _size = size;
            _drawOnBox = drawOnBox;

            Repaint();
        }

        private void OnGUI()
        {
            if (_drawOnBox == null)
                return;

            UpdateViewRect();
            UpdateScrollPosition();
            UpdateZoomSlider();

            _positionForScrollView = GUI.BeginScrollView(_scrolPosition,
                _positionForScrollView,
                _viewRect);

            uint lineSize = _size;

            float boxSize = _baseViewRect.width / lineSize * GetZoomValue();

            int start_x = (int)Mathf.Clamp((float)Math.Floor((decimal)(_positionForScrollView.x / boxSize)), 0f, (float)lineSize);
            int start_y = (int)Mathf.Clamp((float)Math.Floor((decimal)(_positionForScrollView.y / boxSize)), 0f, (float)lineSize);
            int end_x = (int)Mathf.Clamp((float)Math.Ceiling((decimal)((_positionForScrollView.x + _scrolPosition.width) / boxSize)), 0f, (float)lineSize);
            int end_y = (int)Mathf.Clamp((float)Math.Ceiling((decimal)((_positionForScrollView.y + _scrolPosition.height) / boxSize)), 0f, (float)lineSize);

            int nbDrawCall = 0;

            for (int x = start_x; x < end_x; x++)
            {
                for (int y = start_y; y < end_y; y++)
                {
                    Color colorBox;
                    string textBox;

                    _drawOnBox(x, y, out colorBox, out textBox);

                    GUI.backgroundColor = colorBox;
                    GUI.Box(new Rect(x * boxSize, y * boxSize, boxSize, boxSize), textBox);

                    nbDrawCall++;
                }
            }

            GUI.EndScrollView();

            _zoom = GUI.VerticalSlider(_positionZoomSlider, _zoom, 10f, 1f);

            Debug.Log("Nb Draw Call from Grid Display : " + nbDrawCall);
            Debug.Log("Start x : " + start_x + ", End x : " + end_x);
            Debug.Log("Start y : " + start_y + ", End y : " + end_y);

        }

        private void UpdateViewRect()
        {
            _viewRect.width = _baseViewRect.width * GetZoomValue();
            _viewRect.height = _baseViewRect.height * GetZoomValue();
        }

        private void UpdateScrollPosition()
        {
            _scrolPosition.width = position.width - _positionZoomSlider.width - 2 * _offset;
            _scrolPosition.height = position.height;
        }

        private void UpdateZoomSlider()
        {
            if (_scrolPosition.width < _viewRect.width)
            {
                _positionZoomSlider.x = _scrolPosition.width + _offset;
            }
            else// if (_scrolPosition.height < _viewRect.height)
            {
                _positionZoomSlider.x = _scrolPosition.width + _offset;
            }
            /*
            else
            {
                _positionZoomSlider.x = _viewRect.xMax + _offset;
            }*/

            _positionZoomSlider.height = position.height;
        }

        private float GetZoomValue()
        {
            return _zoom*_zoom;
        }
    }
}