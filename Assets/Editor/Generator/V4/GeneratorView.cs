using System.Collections.Generic;
using MonsterAdventure.Generation;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GeneratorView : EditorWindow
    {
        private Map _map;

        private Generator _generator = null;

        //private GenerationGridView _gridView;
        private List<GenerationMethodView> _generationMethodViews;

        private Vector2 _positionForScrollView;

        private bool _gameIsRunning = false;

        private static GridDisplay _gridDisplay;

        [MenuItem("Window/GeneratorView")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            GeneratorView instance = (GeneratorView) EditorWindow.GetWindow(typeof(GeneratorView));
            instance.titleContent = new GUIContent("Generator View");

            _gridDisplay = (GridDisplay)EditorWindow.GetWindow(typeof(GridDisplay));
            _gridDisplay.titleContent = new GUIContent("Grid Display");

            _gridDisplay.Show();

            instance.Show();
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 45;
#endif
        }

        private void OnGUI()
        {
            EditorGUILayout.Separator();

            _positionForScrollView = EditorGUILayout.BeginScrollView(_positionForScrollView);
            {
                if (_gameIsRunning)
                {
                    //_gridView.Draw();

                    DrawGenerationMethodViews();
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void Update()
        {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused && !_gameIsRunning)
            {
                FindGameObjectsInScene();

                _gridDisplay = (GridDisplay)EditorWindow.GetWindow(typeof(GridDisplay));

                if (_generator != null && _generator.IsInitialized())
                {
                    _gameIsRunning = true;
                    CreateViews();
                }
            }
            else if (!EditorApplication.isPlaying)
            {
                _gameIsRunning = false;

                //_sectorView.Reset();

                //ResetGenerationMethodViews();
            }

            if (_gameIsRunning)
            {
                //_gridView.Update();

                UpdateGenerationMethodViews();
            }
        }

        private void FindGameObjectsInScene()
        {
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

            if (_map != null)
            {
                _generator = _map.generator;
            }
        }

        private void CreateViews()
        {
            //_gridView = new GenerationGridView(this);
            _generationMethodViews = new List<GenerationMethodView>();

            List<GenerationMethod> generationMethods = _generator.GetGenerationMethods();

            for (int i = 0; i < generationMethods.Count; i++)
            {
                _generationMethodViews.Add(ConstructView(generationMethods[i]));
            }

            Repaint();
        }

        private void DrawGenerationMethodViews()
        {
            for (int i = 0; i < _generationMethodViews.Count; i++)
            {
                _generationMethodViews[i].Draw();
            }
        }

        private void ResetGenerationMethodViews()
        {
            for (int i = 0; i < _generationMethodViews.Count; i++)
            {
                _generationMethodViews[i].Reset();
            }
        }

        private void UpdateGenerationMethodViews()
        {
            for (int i = 0; i < _generationMethodViews.Count; i++)
            {
                _generationMethodViews[i].Update();
            }
        }

        private GenerationMethodView ConstructView(GenerationMethod generationMethod)
        {
            switch (generationMethod.GetGenerationType())
            {
                case GenerationType.Noise:
                    return new NoiseSpreadView(this, (NoiseSpread) generationMethod, _gridDisplay);

                case GenerationType.Grouping:
                    return new GroupingView(this, (Grouping) generationMethod, _gridDisplay);

                case GenerationType.Random:
                    return new RandomSpreadView(this, (RandomSpread) generationMethod, _gridDisplay);

                default:
                    return new GenerationMethodView(this, generationMethod, _gridDisplay);
            }
        }

        public SectorManager GetSectorManager()
        {
            if (_map != null)
            {
                return _map.SectorManager;
            }

            return null;
        }

        public TileManager GetTileManager()
        {
            if (_map != null)
            {
                return _map.TileManager;
            }

            return null;
        }

        public Generator GetGenerator()
        {
            return _generator;
        }
    }
}