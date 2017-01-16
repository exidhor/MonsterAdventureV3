using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GeneratorWindow : EditorWindow
    {
        private SectorCategory _sectorCategory;
        private BiomeCategory _biomeCategory;
        private ZoneCategory _zoneCategory;
        private LakeCategory _lakeCategory;
        private MovableGridCategory _movableGridCategory;

        private Vector2 _positionForScrollView;

        private bool _gameIsRunning = false;

        [MenuItem("Window/Generator")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            GeneratorWindow instance = (GeneratorWindow) EditorWindow.GetWindow(typeof(GeneratorWindow));
            instance.titleContent = new GUIContent("Generator");

            instance.Show();
        }

        /// <summary>
        /// Construct and initialize parameters.
        /// This function is called when the object is loaded.
        /// </summary>
        private void OnEnable()
        {
            _sectorCategory = new SectorCategory(this);

            _biomeCategory = new BiomeCategory(this, _sectorCategory);

            _zoneCategory = new ZoneCategory(this, _sectorCategory);

            _lakeCategory = new LakeCategory(this, _sectorCategory);

            _movableGridCategory = new MovableGridCategory(this);
        }

        /// <summary>
        /// Draw the interface into Unity.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.Separator();

            _positionForScrollView = EditorGUILayout.BeginScrollView(_positionForScrollView);
            {
                _sectorCategory.Draw();

                _biomeCategory.Draw();

                _zoneCategory.Draw();

                _lakeCategory.Draw();

                _movableGridCategory.Draw();
            }
            EditorGUILayout.EndScrollView();
        }

        private void Update()
        {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused && !_gameIsRunning)
            {
                _gameIsRunning = true;
            }
            else if (!EditorApplication.isPlaying)
            {
                _gameIsRunning = false;
                _sectorCategory.Reset();
                _biomeCategory.Reset();
                _zoneCategory.Reset();
                _lakeCategory.Reset();
                _movableGridCategory.Reset();
            }

            if (_gameIsRunning)
            {
                _sectorCategory.Update();
                _biomeCategory.Update();
                _zoneCategory.Update();
                _lakeCategory.Update();
                _movableGridCategory.Update();
            }
        }
    }
}