/**
 * Adapted from TakeHiResScreenShot here: http://answers.unity3d.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraShot : MonoBehaviour {
	public int resWidth = Screen.width; 
	public int resHeight = Screen.height;
	public Image guiImage;
	private int screenReticleSize;
	new private Camera camera;
	private bool takeShot = false;
	private int x, y;
	private const int UNITY_TO_SCREEN_CONVERSION = 500;
	void Start() {
		camera = gameObject.GetComponent<Camera> ();
	}
	/**public static string ScreenShotName(int width, int height) {
		return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png", 
			Application.dataPath, 
			width, height, 
			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}*/
	public void TakeCameraShot(float unityReticleSize) {
		this.screenReticleSize = (int) unityReticleSize * UNITY_TO_SCREEN_CONVERSION;
		takeShot = true;
		x = (resWidth - this.screenReticleSize) / 2;
		y = (resHeight - this.screenReticleSize) / 2;
	}
	void LateUpdate() {
		if (guiImage != null && takeShot) {
			RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
			camera.targetTexture = rt;
			Texture2D screenShot = new Texture2D(screenReticleSize, screenReticleSize, TextureFormat.RGB24, false);
			camera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect (x, y, screenShot.width, screenShot.height), 0, 0);
			screenShot.Apply ();
			guiImage.sprite = Sprite.Create (screenShot, new Rect(0,0,screenShot.width, screenShot.height), new Vector2(0.5f,0.5f));
			camera.targetTexture = null;
			RenderTexture.active = null; // JC: added to avoid errors
			Destroy(rt);
			/**byte[] bytes = screenShot.EncodeToPNG();
			string filename = ScreenShotName(resWidth, resHeight);
			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log(string.Format("Took screenshot to: {0}", filename));*/
			takeShot = false;
		}
	}

}