// using UnityEngine;
//
// // https://github.com/ReCogMission/FirstTutorials/blob/master/CameraFade.cs
// public class CameraFade : MonoBehaviour
// {
//     public bool fadeIn;
//     public float speedScale = 1f;
//     public Color fadeColor = Color.black;
//     // Rather than Lerp or Slerp, we allow adaptability with a configurable curve
//     public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1),
//         new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
//     public bool startFadedOut = false;
//
//
//     private float alpha = 0f; 
//     private Texture2D texture;
//     private int direction = 1;
//     private float time = 0f;
//
//     private void Start()
//     {
//         alpha = startFadedOut ? 1f : 0f;
//         texture = new Texture2D(1, 1);
//         texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
//         texture.Apply();
//     }
//
//     public void OnGUI()
//     {
//         if (fadeIn)
//         {
//             direction = 1;
//             alpha = 1f;
//         }
//         
//         if (alpha > 0f)
//             GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
//         if (direction != 0)
//         {
//             time += direction * Time.deltaTime * speedScale;
//             alpha = Curve.Evaluate(time);
//             texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
//             texture.Apply();
//             if (alpha <= 0f || alpha >= 1f) direction = 0;
//         }
//     }
// }


using UnityEngine;
using System.Collections;

public class CameraFader : MonoBehaviour {

  public delegate void endFunction();
	public endFunction endCall;
	
	Texture2D blackTex;
	
	public bool fadeout = false;
	public float delay = 0.0f;
	float startTime = 0f;
	Color color = new Color(1f,1f,1f,0);
	
	bool  needDestroy = false; //used for delay destroy, as fadeout then fadein screen flashing

  float _duration = 1f;
	public float duration {
		set {
			_duration = value;
		}
		get {
			return _duration;	
		}
	}
	
	float elapseNormlized {
		get {
#if UNITY_EDITOR
			float norm = (Time.time - startTime ) / _duration;
#else 
			float norm = (Time.realtimeSinceStartup - startTime ) / _duration;
#endif
			return norm;
		}
	}
	
	public float target {
		get {
			return fadeout ? 1.0f : 0f;	
		}
	}

	void Awake() 
	{
#if UNITY_EDITOR
		startTime = Time.time + delay;
#else
		startTime = Time.realtimeSinceStartup + delay;
#endif
		blackTex = new Texture2D(1,1,TextureFormat.ARGB32,false);
		Color[] colors = new Color[1];
		for (int i = 0; i < colors.Length; i++) {
			colors[i]=Color.black;
		}
		blackTex.SetPixels(colors);
		blackTex.Apply();
		
	}

	void Update() {
		if(needDestroy) {
			Destroy(gameObject);
			return;
		}
		float current = 0;
		float norm = elapseNormlized;
		if(norm > 1.0f) {
			endCall?.Invoke();
			StartCoroutine(SetDestroy());
			return;
		}
		
		current = fadeout ? Mathf.Lerp(0f, 1f, norm) : Mathf.Lerp(1f, 0f, norm);
		
		color.a = current;	
	}

	IEnumerator SetDestroy()
	{
		yield return new WaitForSeconds(0.2f);
		needDestroy = true;
	}
	
	void OnGUI () 
	{
		if(Event.current.type != EventType.Repaint) return;
#if UNITY_EDITOR
		if(Time.time < startTime ) return;
#else 
		if(Time.realtimeSinceStartup < startTime) return;
#endif
		Color c = GUI.color;
		GUI.color = color;
		int depth = GUI.depth;
		GUI.depth = -10;
		GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), blackTex);
		GUI.depth = depth;
		GUI.color = c;
	}
	
	public static CameraFader NewFadeIn(float duration = 1f, float delay = 0.0f)
	{
		GameObject go = new GameObject("CameraFader");
		DontDestroyOnLoad(go);
		CameraFader fader = go.AddComponent<CameraFader>();
		fader.fadeout = false;
		fader.duration = duration;
		fader.delay = delay;
		return fader;
	}
	
	public static CameraFader NewFadeOut(float duration = 1f, float delay = 0.0f) {
		GameObject go = new GameObject("CameraFader");
		DontDestroyOnLoad(go);
		CameraFader fader = go.AddComponent<CameraFader>();
		fader.fadeout = true;
		fader.duration = duration;
		fader.delay = delay;
		return fader;
	}
}