using UnityEngine;

namespace a11y_Wizards.Scripts
{
    public class HighResScreenShots : MonoBehaviour
    {
        [SerializeField] int resWidth;
        [SerializeField] int resHeight;

        [SerializeField] private Camera[] cameras; 
 
        // private bool takeHiResShot;
 
        void Start()
        {
            TakeScreenshot();
        }
 
        public static string ScreenShotName(int width, int height)
        {
            var path = "C:/Users/tejas/Documents/Personal Projects/MIT Reality Hack 2022/TEAM-23/Assets";
            return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",path,width, height,System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }
        
        void TakeScreenshot()
        {
            foreach (Camera cam in cameras)
            {
                resWidth = (int)cam.pixelWidth;
                resHeight = (int)cam.pixelHeight;
 
                RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(cam.pixelRect), 0, 0);
                cam.Render();
                RenderTexture.active = null;
                Destroy(rt);
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = ScreenShotName(resWidth, resHeight);
                System.IO.File.WriteAllBytes(filename, bytes);
            }
            
            /*takeHiResShot |= Input.GetKeyDown("k");
            if (takeHiResShot)
            {
                foreach (Camera cam in cameras)
                {
                    resWidth = (int)cam.pixelWidth;
                    resHeight = (int)cam.pixelHeight;
 
                    RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
                    Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
                    RenderTexture.active = rt;
                    screenShot.ReadPixels(new Rect(cam.pixelRect), 0, 0);
                    cam.Render();
                    RenderTexture.active = null;
                    Destroy(rt);
                    byte[] bytes = screenShot.EncodeToPNG();
                    string filename = ScreenShotName(resWidth, resHeight);
                    System.IO.File.WriteAllBytes(filename, bytes);
                    takeHiResShot = false;
                }
            }*/
        }
    }
}
