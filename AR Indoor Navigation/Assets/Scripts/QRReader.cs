using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;

public class QRReader : MonoBehaviour
{
    [SerializeField]
    private TMP_Text popup;
    [SerializeField]
    private ARSession session;
    [SerializeField]
    private XROrigin sessionOrigin;
    [SerializeField]
    private ARCameraManager cameraManager;
    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();
    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader();
    public SetNav setNav;
    public SceneLoader sceneLoader;
    public GameObject models;
    public GameObject maps;
    public GameObject targets;
    public GameObject searchButton;
    public GameObject footerExpanded;
    public GameObject qrMaskOnStart;

    public float qrScanStart = 0f;
    public float qrCooldown = 5f;


    private void Start()
    {
        qrScanStart = Time.time - qrCooldown;
    }
    private void Update()
    {

    }

    private void OnEnable()
    {
        cameraManager.frameReceived += OnCameraFrameReceived;
        //viaC04v14.SetActive(false);
        //c04map.SetActive(false);
        searchButton.SetActive(false);
        footerExpanded.SetActive(false);
        qrMaskOnStart.SetActive(true);
    }
    private void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            return;
        }

        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get entire image
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Downsample by 2
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

            // Choose RGBA format
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image)
            transformation = XRCpuImage.Transformation.MirrorY
        };

        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        image.Convert(conversionParams, buffer);


        image.Dispose();

        cameraImageTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat, false);

        cameraImageTexture.LoadRawTextureData(buffer);
        cameraImageTexture.Apply();

        buffer.Dispose();
        var result = reader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);
        if (result != null)
        {
            if (Time.time > qrScanStart + qrCooldown)
            {
                qrScanStart = Time.time;
                SetQrCodeRecenterTarget(result.Text);
            }
        }

    }
    private void SetQrCodeRecenterTarget(string targetText)
    {
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(targetText.ToLower()));
        if (currentTarget != null)
        {
            session.Reset();
            popup.text = " your current location is set to " + targetText;
            StartCoroutine(ShowAndHideObject());
            sessionOrigin.transform.position = currentTarget.PositionObject.transform.position;
            sessionOrigin.transform.rotation = currentTarget.PositionObject.transform.rotation;

            maps.transform.Find(targetText.Substring(0, Mathf.Min(3, targetText.Length))).gameObject.SetActive(true);
            models.transform.Find(targetText.Substring(0, Mathf.Min(3, targetText.Length))).gameObject.SetActive(true);
            targets.transform.Find(targetText.Substring(0, Mathf.Min(3, targetText.Length))).gameObject.SetActive(true);

            searchButton.SetActive(true);
            footerExpanded.SetActive(true);
            qrMaskOnStart.SetActive(false);
            setNav.CalculateAllDistances(currentTarget.PositionObject.transform);
            if (GameObject.Find("NavigationManager") == null)
            {
                sceneLoader.ToSelection();
            }
        }
    }

    IEnumerator ShowAndHideObject()
    {
        Debug.Log("set true");
        // Show the object
        popup.gameObject.SetActive(true);

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide the object after 5 seconds
        popup.gameObject.SetActive(false);
        Debug.Log("set false");
    }
}