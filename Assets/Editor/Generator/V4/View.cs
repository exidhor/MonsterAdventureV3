using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public abstract class View
    {
        private bool _isInitialized = false;

        private EditorWindow _editorWindow;

        private bool showOptions = true;
        private string labelMenu;
        protected GizmosDrawer _gizmosDrawer;

        /*
         * \brief   Construct the Option.
         * \param   a_Label The title of the option.
         * \param   a_Window The window where the option will be.
         * \param   a_StartingValue if the option sart "hidden" (false)
         *          or not (true)
         */
        public View(EditorWindow editorWindow, string label, bool startingHidden = false)
        {
            _editorWindow = editorWindow;

            showOptions = !startingHidden;
            labelMenu = label;
        }


        /*
         * \brief   Draw the option and provide the foldout system.
         */
        public void Draw()
        {
            showOptions = EditorGUILayout.Foldout(showOptions,
                labelMenu);

            // change the style for the title.
            int oldFontSize = EditorStyles.foldout.fontSize;
            FontStyle oldFontStyle = EditorStyles.foldout.fontStyle;

            EditorStyles.foldout.fontSize = 14;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;

            EditorGUI.indentLevel++;
            if (showOptions)
            {
                DrawContent();
            }
            EditorGUI.indentLevel--;

            // restore the previous style
            EditorStyles.foldout.fontSize = oldFontSize;
            EditorStyles.foldout.fontStyle = oldFontStyle;
        }


        /*
         * \brief   [ABSTRACT PURE] Where the content
         *          has to be implement.
         *          [CARREFUL] This method doesn't need to be called
         *          because it is in the "void Draw()" method.
         */
        protected abstract void DrawContent();

        protected abstract bool TryToInit();

        private void Initialize()
        {
            if (TryToInit())
            {
                _gizmosDrawer = GameObject.FindGameObjectWithTag("GizmosDrawer").GetComponent<GizmosDrawer>();
                _isInitialized = true;

                _editorWindow.Repaint();
            }
        }

        public void Update()
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            if (_isInitialized)
            {
                _gizmosDrawer.Draw(DrawGizmosContent);
                UpdateContent();
            }
        }

        protected abstract void DrawGizmosContent();

        protected abstract void UpdateContent();

        public void Reset()
        {
            _isInitialized = false;

            ResetContent();
        }

        protected abstract void ResetContent();

        public bool IsInitialized()
        {
            return _isInitialized;
        }
    }
}
