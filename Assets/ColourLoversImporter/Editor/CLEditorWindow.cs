namespace CLImporter
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.Xml.Serialization;
    using System.IO;
    using System;
    using System.Linq;
    using System.Text;

    public class CLEditorWindow : EditorWindow
    {
        private List<CLPaletteInfo> _palettes = new List<CLPaletteInfo>();
        private List<CLColorInfo> _colors = new List<CLColorInfo>();

        private string _searchKeywords = "";
        private Vector2 _scrollPos;

        private string _colorPresetLibraryTemplate;
        private string _colorPresetColorTemplate;

        private const string PALETTES_API = "http://www.colourlovers.com/api/palettes{0}";
        private const string COLORS_API = "http://www.colourlovers.com/api/colors{0}";
        private const string NEW_REQUEST = "/new";
        private const string TOP_REQUEST = "/top";
        private const string RANDOM_REQUEST = "/random";
        private const string KEYWORDS_PARAMETER = "?keywords={0}";

        [MenuItem("Window/ColourLovers Importer")]
        static void Init()
        {
            CLEditorWindow window = (CLEditorWindow)GetWindow(typeof(CLEditorWindow), false, "CL Importer");
            window.minSize = new Vector2(300f, 300f);
            window.Show();
        }

        private void Awake()
        {
            StreamReader sr = new StreamReader(string.Format("{0}/ColourLoversImporter/ColorPresetLibraryTemplate.txt", Application.dataPath));
            _colorPresetLibraryTemplate = sr.ReadToEnd();
            sr.Close();

            sr = new StreamReader(string.Format("{0}/ColourLoversImporter/ColorPresetColorTemplate.txt", Application.dataPath)); ;
            _colorPresetColorTemplate = sr.ReadToEnd();
            sr.Close();
        }

        void OnGUI()
        {
            GUILayout.Label("Palettes", EditorStyles.boldLabel);
            DrawLoadOptions(LoadPalettes);

            GUILayout.Label("Colours", EditorStyles.boldLabel);
            DrawLoadOptions(LoadColors);

            DrawSearchOptions();
            GUILayout.Space(10);

            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            if (_palettes.Count > 0)
            {
                DrawPalettes();
            }
            else if (_colors.Count > 0)
            {
                DrawColors();
            }
            GUILayout.EndScrollView();
        }

        private void DrawLoadOptions(Action<string> loadAction)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Latest", GUILayout.MaxWidth(200)))
            {
                loadAction(NEW_REQUEST);
            }
            if (GUILayout.Button("Top", GUILayout.MaxWidth(200)))
            {
                loadAction(TOP_REQUEST);
            }
            if (GUILayout.Button("Random", GUILayout.MaxWidth(200)))
            {
                loadAction(RANDOM_REQUEST);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

        private void DrawSearchOptions()
        {
            GUILayout.Label("Search", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Keywords", "Comma separated keywords to search for"), GUILayout.Width(70));
            _searchKeywords = GUILayout.TextField(_searchKeywords, GUILayout.MaxWidth(535));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Palettes", GUILayout.MaxWidth(200)) && _searchKeywords != "")
            {
                LoadPalettes(string.Format(KEYWORDS_PARAMETER, _searchKeywords.Replace(",", "+").Replace(" ", "")));
            }
            if (GUILayout.Button("Colours", GUILayout.MaxWidth(200)) && _searchKeywords != "")
            {
                LoadColors(string.Format(KEYWORDS_PARAMETER, _searchKeywords.Replace(",", "+").Replace(" ", "")));
            }
            GUILayout.EndHorizontal();
        }

        private void DrawPalettes()
        {
            foreach (var p in _palettes)
            {
                GUILayout.BeginHorizontal(GUILayout.MaxWidth(600));
                GUILayout.Label(p.Title, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("[Go to URL]", GUI.skin.label))
                {
                    Application.OpenURL(p.Url);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Save as: ", GUILayout.Width(50));
                p.SaveName = EditorGUILayout.TextField(p.SaveName, GUILayout.MaxWidth(555));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                for (int i = 0; i < p.Colors.Count; i++)
                {
#if UNITY_2018
                    p.Colors[i] = EditorGUILayout.ColorField(new GUIContent(""), p.Colors[i], false, true, false, GUILayout.Width(30));
#else
                    p.Colors[i] = EditorGUILayout.ColorField(new GUIContent(""), p.Colors[i], false, true, false, null, GUILayout.Width(30));
#endif
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Revert", GUILayout.MaxWidth(200)))
                {
                    p.Revert();
                }
                if (GUILayout.Button("Save", GUILayout.MaxWidth(200)))
                {
                    SavePalette(p);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(20);
            }
        }

        private void DrawColors()
        {
            foreach (var c in _colors)
            {
                GUILayout.BeginHorizontal(GUILayout.MaxWidth(600));
                GUILayout.Label(c.Title, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("[Go to URL]", GUI.skin.label))
                {
                    Application.OpenURL(c.Url);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

#if UNITY_2018
                EditorGUILayout.ColorField(new GUIContent(""), c.Color, false, true, false, GUILayout.MaxWidth(600));
#else
                EditorGUILayout.ColorField(new GUIContent(""), c.Color, false, true, false, null, GUILayout.MaxWidth(600));
#endif

                GUILayout.Space(20);
            }
        }

        private void Refresh()
        {
            _scrollPos = Vector2.zero;
            Repaint();
        }

        private void LoadPalettes(string apiType)
        {
            GUI.FocusControl("");
            _palettes.Clear();

            var request = string.Format(PALETTES_API, apiType);
            var www = new WWW(request);

            ContinuationManager.Add(() => www.isDone, () =>
            {
                if (!string.IsNullOrEmpty(www.error)) Debug.Log("WWW failed: " + www.error);
                //Debug.Log(www.text);

                var serializer = new XmlSerializer(typeof(CLPalettes));
                var result = serializer.Deserialize(new StringReader(www.text)) as CLPalettes;

                foreach (var p in result.Palettes)
                {
                    _palettes.Add(new CLPaletteInfo(p));
                }

                _colors.Clear();
                Refresh();
            });
        }

        private void LoadColors(string apiType)
        {
            GUI.FocusControl("");
            _colors.Clear();

            var request = string.Format(COLORS_API, apiType);
            var www = new WWW(request);

            ContinuationManager.Add(() => www.isDone, () =>
            {
                if (!string.IsNullOrEmpty(www.error)) Debug.Log("WWW failed: " + www.error);
                //Debug.Log(www.text);

                var serializer = new XmlSerializer(typeof(CLColors));
                var result = serializer.Deserialize(new StringReader(www.text)) as CLColors;

                foreach (var c in result.Colors)
                {
                    _colors.Add(new CLColorInfo(c));
                }

                _palettes.Clear();
                Refresh();
            });
        }

        private void SavePalette(CLPaletteInfo palette)
        {
            var colorAssetName = SanitiseName(palette.SaveName);
            var colorAssetsFolder = "Assets/Editor/";
            var colorAssetPath = string.Format("{0}{1}.colors", colorAssetsFolder, colorAssetName);

            if (!Directory.Exists(colorAssetsFolder))
            {
                Directory.CreateDirectory(colorAssetsFolder);
            }
            if (File.Exists(colorAssetPath))
            {
                EditorUtility.DisplayDialog("Sorry!", "A colour palette with the same name is already saved. Please enter a different name to save this palette.", "Ok");
                return;
            }

            // Unity apparently doesnt expose the ColorPresetLibrary class
            // Hackity hack: write asset data directly using a template that matches the color preset files
            var sb = new StringBuilder();
            foreach (var c in palette.Colors)
            {
                sb.Append(_colorPresetColorTemplate);
                sb.Append("{ ");
                sb.Append(string.Format("r: {0}, g: {1}, b: {2}, a: {3}", c.r, c.g, c.b, c.a));
                sb.AppendLine(" }");
            }
            StreamWriter sw = new StreamWriter(colorAssetPath);
            sw.Write(_colorPresetLibraryTemplate);
            sw.Write(sb.ToString());
            sw.Close();

            AssetDatabase.Refresh();
        }

        internal static string SanitiseName(string name)
        {
            return Path.GetInvalidFileNameChars().Aggregate(name, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }

    internal class CLPaletteInfo
    {
        public string Title;
        public string SaveName;
        public string Url;
        public List<Color> Colors = new List<Color>();

        private List<Color> _originalColors = new List<Color>();

        public CLPaletteInfo(CLPalette palette)
        {
            Title = palette.Title;
            SaveName = CLEditorWindow.SanitiseName(palette.Title);
            Url = palette.Url;

            foreach (var c in palette.HexColors)
            {
                Color color;
                ColorUtility.TryParseHtmlString(string.Format("#{0}", c), out color);
                _originalColors.Add(color);
            }

            Colors = new List<Color>(_originalColors);
        }

        public void Revert()
        {
            Colors = new List<Color>(_originalColors);
        }
    }

    internal class CLColorInfo
    {
        public string Title;
        public string Url;
        public Color Color;

        public CLColorInfo(CLColor color)
        {
            Title = color.Title;
            Url = color.Url;

            Color = new Color32((byte)color.RGB.Red, (byte)color.RGB.Green, (byte)color.RGB.Blue, 255);
        }
    }

    internal static class ContinuationManager
    {
        private class Job
        {
            public Job(Func<bool> completed, Action continueWith)
            {
                Completed = completed;
                ContinueWith = continueWith;
            }
            public Func<bool> Completed { get; private set; }
            public Action ContinueWith { get; private set; }
        }

        private static readonly List<Job> jobs = new List<Job>();

        public static void Add(Func<bool> completed, Action continueWith)
        {
            if (!jobs.Any()) EditorApplication.update += Update;
            jobs.Add(new Job(completed, continueWith));
        }

        private static void Update()
        {
            for (int i = 0; i >= 0; --i)
            {
                var jobIt = jobs[i];
                if (jobIt.Completed())
                {
                    jobIt.ContinueWith();
                    jobs.RemoveAt(i);
                }
            }
            if (!jobs.Any()) EditorApplication.update -= Update;
        }
    }
}