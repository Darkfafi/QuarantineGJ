using UnityEngine;
using UnityEngine.UI;

public class SpriteColorSwap : MonoBehaviour
{
	private Graphic _ownGraphic;
	private Renderer _ownRenderer;

	private Texture2D _ownColorSwapTex;
	private Color[] _ownSpriteColors;

	protected void Awake()
	{
		_ownRenderer = GetComponent<Renderer>();
		_ownGraphic = GetComponent<Graphic>();

		if(_ownRenderer == null && _ownGraphic == null)
		{
			throw new UnityException("This component Requires a Renderer or a Graphic (SpriteRenderer || Image) in order to work");
		}

		Initialize();
	}

	public void SwapColor(int redIndex, Color newColor, params int[] linkedShadowRedIndexes)
	{
		_ownSpriteColors[redIndex] = newColor;
		_ownColorSwapTex.SetPixel(redIndex, 0, newColor);
		for(int i = 0; i < linkedShadowRedIndexes.Length; i++)
		{
			float mv = (float)linkedShadowRedIndexes[i] / (float)redIndex;
			Color sc = newColor * mv;
			sc.a = newColor.a;
			_ownSpriteColors[linkedShadowRedIndexes[i]] = sc;
			_ownColorSwapTex.SetPixel(linkedShadowRedIndexes[i], 0, sc);
		}

		_ownColorSwapTex.Apply();
	}

	public void DoFullColorEffect(Color color)
	{
		for(int i = 0; i < _ownColorSwapTex.width; i++)
		{
			_ownColorSwapTex.SetPixel(i, 0, color);
		}
	}

	public void ResetFullColorEffect()
	{
		for(int i = 0; i < _ownColorSwapTex.width; i++)
		{
			_ownColorSwapTex.SetPixel(i, 0, _ownSpriteColors[i]);
		}
	}

	public void ResetSwapColor()
	{
		for(int i = 0; i < _ownColorSwapTex.width; i++)
		{
			_ownColorSwapTex.SetPixel(i, 0, new Color(0f, 0f, 0f, 0f));
		}

		_ownSpriteColors = new Color[_ownColorSwapTex.width];
	}

	private void Initialize()
	{
		Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false);
		colorSwapTex.filterMode = FilterMode.Point;

		for(int i = 0; i < colorSwapTex.width; ++i)
		{
			colorSwapTex.SetPixel(i, 0, new Color(0f, 0f, 0f, 0f));
		}

		colorSwapTex.Apply();

		Material mat = new Material(Shader.Find("Sprites/SwapColor"));

		if(_ownRenderer != null)
			_ownRenderer.material = mat;
		else if(_ownGraphic != null)
			_ownGraphic.material = mat;

		mat.SetTexture("_SwapTex", colorSwapTex);

		_ownSpriteColors = new Color[colorSwapTex.width];
		_ownColorSwapTex = colorSwapTex;
	}
}