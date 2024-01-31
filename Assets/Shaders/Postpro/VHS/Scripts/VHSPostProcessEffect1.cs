using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;
using Random = UnityEngine.Random;


[AddComponentMenu("Image Effects/GlitchEffect")]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(VideoPlayer))]
public class VHSPostProcessEffect1 : MonoBehaviour
{
	public Shader shader;
	public VideoClip VHSClip;
	
	private float _yScanline;
	private float _xScanline;
	private Material _material = null;
	private VideoPlayer _player;
	[Range(0.0f, 1.0f)]
	public float Intensity = 1;
#if UNITY_EDITOR
	bool isPlaying => UnityEditor.EditorApplication.isPlaying;
#else
    bool isPlaying => true;
#endif
	bool playOnAwake;

	private void Awake()
	{
		if (isPlaying)
		{
			_player = GetComponent<VideoPlayer>();
			// Wait until the video player is ready before starting the timeline
			_player.prepareCompleted += VideoPlayer_prepareCompleted;
		}
	}

	// void OnRenderImage(RenderTexture source, RenderTexture destination)
	// {
	// 	_material.SetTexture("_VHSTex", _player.texture);
	//
	// 	_yScanline += Time.deltaTime * 0.01f;
	// 	_xScanline -= Time.deltaTime * 0.1f;
	//
	// 	if (_yScanline >= 1)
	// 	{
	// 		_yScanline = Random.value;
	// 	}
	// 	if (_xScanline <= 0 || Random.value < 0.05)
	// 	{
	// 		_xScanline = Random.value;
	// 	}
	// 	_material.SetFloat("_yScanline", _yScanline);
	// 	_material.SetFloat("_xScanline", _xScanline);
	// 	_material.SetFloat("_Intensity", Intensity);
	// 	Graphics.Blit(source, destination, _material);
	// }

	protected void OnDisable()
	{
		if (_material)
		{
			DestroyImmediate(_material);
		}
	}
	private void VideoPlayer_prepareCompleted(VideoPlayer source)
	{
		if (playOnAwake)
			_player.Stop();
	}

	protected void OnEnable()
	{
		_material = new Material(shader);
		_player.isLooping = true;
		_player.renderMode = VideoRenderMode.APIOnly;
		_player.audioOutputMode = VideoAudioOutputMode.None;
		_player.clip = VHSClip;
		_player.Play();
	}
}
