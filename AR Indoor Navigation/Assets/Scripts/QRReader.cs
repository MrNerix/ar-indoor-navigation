using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using ZXing;

public class QRReader : MonoBehaviour
{
    [SerializeField]
    private ARCameraBackground aRCameraBackground;
    [SerializeField]
    private RenderTexture targetRenderTexture;
    [SerializeField]
    private TextMeshProUGUI qrCodeText;

    private Texture2D cameraImageTexture;
    private IBarcodeReader reader = new BarcodeReader(); //Barcode reader instance
    // Update is called once per frame
    private void Update()
    {
        Graphics.Blit(null, targetRenderTexture, aRCameraBackground.material);
        cameraImageTexture = new Texture2D(targetRenderTexture.width, targetRenderTexture.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(targetRenderTexture, cameraImageTexture);

        //detects and decodes the barcode inside the bitmap
        var result = reader.Decode(cameraImageTexture.GetPixels32(), cameraImageTexture.width, cameraImageTexture.height);

        //use the result
        if (result != null)
        {
            qrCodeText.text = result.Text;
        }
    }
}
