﻿using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace UnityHelpers.GUI
{
    using UnityEngine;

    public class VerticalBlock : IDisposable
    {
        public VerticalBlock(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        public VerticalBlock(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }

    public class ScrollviewBlock : IDisposable
    {
        public ScrollviewBlock(ref Vector2 scrollPos, params GUILayoutOption[] options)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, options);
        }

        public void Dispose()
        {
            GUILayout.EndScrollView();
        }
    }

    public class HorizontalBlock : IDisposable
    {
        public HorizontalBlock(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }

        public HorizontalBlock(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }

    public class ColoredBlock : IDisposable
    {
        public ColoredBlock(Color color)
        {
            GUI.color = color;
        }

        public void Dispose()
        {
            GUI.color = Color.white;
        }
    }

    public class DisabledBlock : IDisposable
    {
        public DisabledBlock(bool state)
        {
            GUI.enabled = state;
        }

        public void Dispose()
        {
            GUI.enabled = true;
        }
    }

    [Serializable]
    public class TabsBlock
    {
        private readonly Dictionary<string, Action> _methods;
        private Action _currentGuiMethod;
        public int CurMethodIndex = -1;

        public TabsBlock(Dictionary<string, Action> _methods)
        {
            this._methods = _methods;
            SetCurrentMethod(0);
        }

        public void DrawTabs()
        {
            var keys = _methods.Keys.ToArray();
            using (new HorizontalBlock())
            {
                for (var i = 0; i < keys.Length; i++)
                {
                    var btnStyle = i == 0 ? EditorStyles.miniButtonLeft : i == (keys.Length - 1) 
                        ? EditorStyles.miniButtonRight 
                        : EditorStyles.miniButtonMid;

                    using (new ColoredBlock(_currentGuiMethod == _methods[keys[i]] ? Color.grey : Color.white))
                        if (GUILayout.Button(keys[i], btnStyle))
                            SetCurrentMethod(i);
                }
            }
        }

        public void DrawBody(GUIStyle backgroundStyle)
        {
            DrawTabs();
            using (new VerticalBlock(backgroundStyle))
            {
                var keys = _methods.Keys.ToArray();
                GUILayout.Label(keys[CurMethodIndex], EditorStyles.centeredGreyMiniLabel);
                _currentGuiMethod();
            }
        }

        public void SetCurrentMethod(int index)
        {
            CurMethodIndex = index;
            _currentGuiMethod = _methods[_methods.Keys.ToArray()[index]];
        }
    }
}
