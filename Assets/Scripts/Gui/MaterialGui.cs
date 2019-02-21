﻿#region

using General;
using Settings;
using UnityEngine;

namespace Gui
{

    #endregion

    public class MaterialGui : MonoBehaviour
    {
        private const int UpdateDivisor = 4;
        private int _divisorCount = UpdateDivisor;
        private static readonly int MetallicId = Shader.PropertyToID("_Metallic");
        private static readonly int AoRemapMinId = Shader.PropertyToID("_AORemapMin");
        private static readonly int AoRemapMaxId = Shader.PropertyToID("_AORemapMax");
        private static readonly int SmoothnessRemapMinId = Shader.PropertyToID("_SmoothnessRemapMin");
        private static readonly int SmoothnessRemapMaxId = Shader.PropertyToID("_SmoothnessRemapMax");
        private static readonly int HeightAmplitudeId = Shader.PropertyToID("_HeightAmplitude");
        private static readonly int TessAmplitudeId = Shader.PropertyToID("_HeightTessAmplitude");
        private static readonly int HeightCenterId = Shader.PropertyToID("_HeightCenter");


        private bool _cubeShown;
        private bool _cylinderShown;
        private Texture2D _diffuseMap;

        private Texture2D _heightMap;
        private Light _light;

        private MaterialSettings _materialSettings;

        private Texture2D _myColorTexture;
        private Texture2D _normalMap;

        private bool _planeShown = true;

        private bool _settingsInitialized;
        private bool _sphereShown;

        private Material _thisMaterial;

        private Rect _windowRect;

        public GameObject LightObject;
        public GameObject TestObject;
        public GameObject TestObjectCube;
        public GameObject TestObjectCylinder;

        public GameObject TestObjectParent;
        public GameObject TestObjectSphere;
        public ObjRotator TestRotator;
        private Texture2D _maskMap;


        private void OnDisable()
        {
            if (!MainGui.Instance.IsGuiHidden || TestObjectParent == null) return;
            if (!TestObjectParent.activeSelf) TestRotator.Reset();

            TestObjectParent.SetActive(true);
            TestObjectCube.SetActive(false);
            TestObjectCylinder.SetActive(false);
            TestObjectSphere.SetActive(false);
            
        }

        private void Awake()
        {
            _light = LightObject.GetComponent<Light>();
            ProgramManager.Instance.SceneObjects.Add(gameObject);
            _windowRect = new Rect(10.0f, 265.0f, 300f, 540f);
        }

        private void Start()
        {
            InitializeSettings();
        }

        public void GetValues(ProjectObject projectObject)
        {
            InitializeSettings();
            projectObject.MaterialSettings = _materialSettings;
        }

        public void SetValues(ProjectObject projectObject)
        {
            if (projectObject.MaterialSettings != null)
            {
                _materialSettings = projectObject.MaterialSettings;
            }
            else
            {
                _settingsInitialized = false;
                InitializeSettings();
            }
        }

        private void InitializeSettings()
        {
            _thisMaterial = TextureManager.Instance.FullMaterialInstance;
            if (_settingsInitialized) return;
            Debug.Log("Initializing MaterialSettings");
            _materialSettings = new MaterialSettings();
            _myColorTexture = TextureManager.Instance.GetStandardTexture(1, 1);
            _materialSettings.DisplacementAmplitude = _thisMaterial.GetFloat(HeightAmplitudeId);
            _materialSettings.Metallic.Value = _thisMaterial.GetFloat(MetallicId);
            _materialSettings.SmoothnessRemapMin = _thisMaterial.GetFloat(SmoothnessRemapMinId);
            _materialSettings.SmoothnessRemapMax = _thisMaterial.GetFloat(SmoothnessRemapMaxId);
            _materialSettings.AoRemapMin = _thisMaterial.GetFloat(AoRemapMinId);
            _materialSettings.AoRemapMax = _thisMaterial.GetFloat(AoRemapMaxId);

            _settingsInitialized = true;
        }


        // Update is called once per frame
        private void Update()
        {
            if (_divisorCount > 0)
            {
                _divisorCount--;
                return;
            }

            _divisorCount = UpdateDivisor;

            if (!_settingsInitialized) InitializeSettings();

            if (!_thisMaterial || _materialSettings == null) return;


//            var temp = _thisMaterial.GetFloat(MetallicId);
//            if (Mathf.Abs(temp - _materialSettings.Metallic.Anonymous) > 0.001f)
//            {
//                if (_materialSettings.Metallic.Changed) _thisMaterial.SetFloat(MetallicId, _materialSettings.Metallic);
//                else _materialSettings.Metallic.Anonymous = temp;
//            }

            _thisMaterial.SetFloat(MetallicId, _materialSettings.Metallic);
            _thisMaterial.SetFloat(AoRemapMinId, _materialSettings.AoRemapMin);
            _thisMaterial.SetFloat(AoRemapMaxId, _materialSettings.AoRemapMax);
            _thisMaterial.SetFloat(SmoothnessRemapMinId, _materialSettings.SmoothnessRemapMin);
            _thisMaterial.SetFloat(SmoothnessRemapMaxId, _materialSettings.SmoothnessRemapMax);
//            _thisMaterial.SetFloat(DisplacementOffsetId, _dispOffset);
            _thisMaterial.SetFloat(HeightAmplitudeId, _materialSettings.DisplacementAmplitude);

            _light.color = new Color(_materialSettings.LightR, _materialSettings.LightG, _materialSettings.LightB);
            _light.intensity = _materialSettings.LightIntensity;

            if (TestObjectParent.activeSelf != _planeShown) TestObjectParent.SetActive(_planeShown);
            if (TestObjectCube.activeSelf != _planeShown) TestObjectCube.SetActive(_cubeShown);
            if (TestObjectCylinder.activeSelf != _planeShown) TestObjectCylinder.SetActive(_cylinderShown);
            if (TestObjectSphere.activeSelf != _planeShown) TestObjectSphere.SetActive(_sphereShown);

            _thisMaterial.SetFloat(HeightAmplitudeId, _materialSettings.DisplacementAmplitude);
            _thisMaterial.SetFloat(HeightCenterId, _materialSettings.DisplacementAmplitude / 4f);
            _thisMaterial.SetFloat(TessAmplitudeId, _materialSettings.DisplacementAmplitude * 100f);


//            
        }

        private void ChooseLightColor(int posX, int posY)
        {
            _materialSettings.LightR =
                GUI.VerticalSlider(new Rect(posX + 10, posY + 5, 30, 100), _materialSettings.LightR, 1.0f, 0.0f);
            _materialSettings.LightG =
                GUI.VerticalSlider(new Rect(posX + 40, posY + 5, 30, 100), _materialSettings.LightG, 1.0f, 0.0f);
            _materialSettings.LightB =
                GUI.VerticalSlider(new Rect(posX + 70, posY + 5, 30, 100), _materialSettings.LightB, 1.0f, 0.0f);
            _materialSettings.LightIntensity =
                GUI.VerticalSlider(new Rect(posX + 120, posY + 5, 30, 100), _materialSettings.LightIntensity, 30.0f,
                    0.0f);

            GUI.Label(new Rect(posX + 10, posY + 110, 30, 30), "R");
            GUI.Label(new Rect(posX + 40, posY + 110, 30, 30), "G");
            GUI.Label(new Rect(posX + 70, posY + 110, 30, 30), "B");
            GUI.Label(new Rect(posX + 100, posY + 110, 100, 30), "Intensity");

            SetColorTexture();

            GUI.DrawTexture(new Rect(posX + 170, posY + 5, 100, 100), _myColorTexture);
        }

        private void SetColorTexture()
        {
            var colorArray = new Color[1];
            colorArray[0] = new Color(_materialSettings.LightR, _materialSettings.LightG, _materialSettings.LightB,
                1.0f);

            _myColorTexture.SetPixels(colorArray);
            _myColorTexture.Apply(false);
        }

        private void DoMyWindow(int windowId)
        {
            const int offsetX = 10;
            var offsetY = 30;

            var temp = _materialSettings.Metallic.Value;
            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Metallic Multiplier", temp,
                out temp, 0.0f, 1.0f);
            offsetY += 40;
            _materialSettings.Metallic.Value = temp;

            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Ambient Occlusion Remap Min",
                _materialSettings.AoRemapMin, out _materialSettings.AoRemapMin, 0.0f, _materialSettings.AoRemapMax);
            offsetY += 40;

            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Ambient Occlusion Remap Max",
                _materialSettings.AoRemapMax, out _materialSettings.AoRemapMax, 0, 1.0f);
            if (_materialSettings.AoRemapMin > _materialSettings.AoRemapMax)
            {
                _materialSettings.AoRemapMin = _materialSettings.AoRemapMax;
            }

            offsetY += 40;

            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Smoothness Remap Min",
                _materialSettings.SmoothnessRemapMin, out _materialSettings.SmoothnessRemapMin, 0.0f, 1.0f);
            if (_materialSettings.AoRemapMax < _materialSettings.AoRemapMin)
            {
                _materialSettings.AoRemapMax = _materialSettings.AoRemapMin;
            }

            offsetY += 40;

            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Smoothness Remap Max",
                _materialSettings.SmoothnessRemapMax, out _materialSettings.SmoothnessRemapMax,
                _materialSettings.SmoothnessRemapMin, 1.0f);
            offsetY += 40;


            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Tessellation Amplitude",
                _materialSettings.DisplacementAmplitude, out _materialSettings.DisplacementAmplitude, 0.0f, 3.0f);
            offsetY += 40;

            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Texture Tiling X", _materialSettings.TexTilingX,
                out _materialSettings.TexTilingX, 0.1f, 5.0f);
            offsetY += 30;

            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Texture Tiling Y", _materialSettings.TexTilingY,
                out _materialSettings.TexTilingY, 0.1f, 5.0f);
            offsetY += 40;

            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Texture Offset X", _materialSettings.TexOffsetX,
                out _materialSettings.TexOffsetX, -1.0f, 1.0f);
            offsetY += 30;

            GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Texture Offset Y", _materialSettings.TexOffsetY,
                out _materialSettings.TexOffsetY, -1.0f, 1.0f);
            offsetY += 40;

            GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Light Color");
            ChooseLightColor(offsetX, offsetY + 20);
            offsetY += 160;

            if (GUI.Button(new Rect(offsetX, offsetY, 60, 30), "Plane"))
            {
                _planeShown = true;
                _cubeShown = false;
                _cylinderShown = false;
                _sphereShown = false;
                Shader.DisableKeyword("TOP_PROJECTION");
            }

            if (GUI.Button(new Rect(offsetX + 70, offsetY, 60, 30), "Cube"))
            {
                _planeShown = false;
                _cubeShown = true;
                _cylinderShown = false;
                _sphereShown = false;
                Shader.EnableKeyword("TOP_PROJECTION");
            }

            if (GUI.Button(new Rect(offsetX + 140, offsetY, 70, 30), "Cylinder"))
            {
                _planeShown = false;
                _cubeShown = false;
                _cylinderShown = true;
                _sphereShown = false;
                Shader.EnableKeyword("TOP_PROJECTION");
            }

            if (GUI.Button(new Rect(offsetX + 220, offsetY, 60, 30), "Sphere"))
            {
                _planeShown = false;
                _cubeShown = false;
                _cylinderShown = false;
                _sphereShown = true;
                Shader.EnableKeyword("TOP_PROJECTION");
            }

            GUI.DragWindow();
        }

        private void OnGUI()
        {
            var pivotPoint = new Vector2(_windowRect.x, _windowRect.y);
            GUIUtility.ScaleAroundPivot(ProgramManager.Instance.GuiScale, pivotPoint);

            _windowRect = GUI.Window(14, _windowRect, DoMyWindow, "Full Material");
        }

        public void Initialize()
        {
            InitializeSettings();
            TestObject.GetComponent<Renderer>().material = _thisMaterial;
            TestObjectCube.GetComponent<Renderer>().material = _thisMaterial;
            TestObjectCylinder.GetComponent<Renderer>().material = _thisMaterial;
            TestObjectSphere.GetComponent<Renderer>().material = _thisMaterial;
        }
    }
}