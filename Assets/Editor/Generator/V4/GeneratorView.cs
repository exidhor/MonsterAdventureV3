using System.Collections.Generic;
using MonsterAdventure.Generation;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GeneratorView : EditorWindow
    {
        private Generator _generator;

        private SectorView _sectorView;
        private List<GenerationMethodView> _generationMethodViews;

        private Vector2 _positionForScrollView;

        private bool _gameIsRunning = false;

        [MenuItem("Window/GeneratorView")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            GeneratorView instance = (GeneratorView) EditorWindow.GetWindow(typeof(GeneratorView));
            instance.titleContent = new GUIContent("Generator View");

            instance.Show();
        }

        private void OnEnable()
        {
            // nothing ? 
        }

        private void OnGUI()
        {
            EditorGUILayout.Separator();

            _positionForScrollView = EditorGUILayout.BeginScrollView(_positionForScrollView);
            {
                if (_gameIsRunning)
                {
                    _sectorView.Draw();

                    DrawGenerationMethodViews();
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void Update()
        {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused && !_gameIsRunning)
            {
                FindGenerator();

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
                _sectorView.Update();

                UpdateGenerationMethodViews();
            }
        }

        private void FindGenerator()
        {
            _generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
        }

        private void CreateViews()
        {
            _sectorView = new SectorView(this);
            _generationMethodViews = new List<GenerationMethodView>();

            List<GenerationMethod> generationMethods = _generator.GetGenerationMethods();

            for (int i = 0; i < generationMethods.Count; i++)
            {
                _generationMethodViews.Add(ConstructView(generationMethods[i]));
            }
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
                    return new NoiseSpreadView(this, (NoiseSpread)generationMethod, _sectorView);

                case GenerationType.Grouping:
                    return new GroupingView(this, (Grouping)generationMethod, _sectorView);

                default:
                    return new GenerationMethodView(this, generationMethod, _sectorView);
            }
        }
    }
}