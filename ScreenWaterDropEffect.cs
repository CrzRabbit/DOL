using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScreenWaterDropEffect : MonoBehaviour {

    #region Variables
    public Shader CurShader;//着色器实例
    private Material CurMaterial;//当前材质

    private float TimeX = 1.0f;//时间变量
    private Texture2D ScreenWaterDropTex;//屏幕水滴素材图

    //编辑器中调节的参数
    [Range(5, 64), Tooltip("溶解度")]
    public float Distortion = 8.0f;
    [Range(0, 7), Tooltip("水滴在X坐标上的尺寸")]
    public float SizeX = 1f;
    [Range(0, 7), Tooltip("水滴在Y坐标上的尺寸")]
    public float SizeY = 0.5f;
    [Range(0, 10), Tooltip("水滴的流动速度")]
    public float DropSpeed = 3.6f;

    //参数调节中间变量
    public static float ChangeDistortion;
    public static float ChangeSizeX;
    public static float ChangeSizeY;
    public static float ChangeDropSpeed;
    #endregion

    #region MaterialGetAndSet
    Material material
    {
        get
        {
            if (CurMaterial == null)
            {
                CurMaterial = new Material(CurShader);
                CurMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return CurMaterial;
        }
    }
    #endregion

    void Start () {
        ChangeDistortion = Distortion;
        ChangeSizeX = SizeX;
        ChangeSizeY = SizeY;
        ChangeDropSpeed = DropSpeed;
        
        //载入素材图
        ScreenWaterDropTex = Resources.Load("ScreenWaterDrop") as Texture2D;

        //
        CurShader = Shader.Find("Unity/vertex_fragment_shaders/ScreenWaterDropEffect");

        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (CurShader != null)
        {
            TimeX += Time.deltaTime;

            if (TimeX > 100)
                TimeX = 0;
            //设置Shader的其它外部变量
            material.SetFloat("_CurTime", TimeX);
            material.SetFloat("_Distortion", Distortion);
            material.SetFloat("_SizeX", SizeX);
            material.SetFloat("_SizeY", SizeY);
            material.SetFloat("_DropSpeed", DropSpeed);
            material.SetTexture("_ScreenWaterDropTex", ScreenWaterDropTex);

            //拷贝资源纹理到目标纹理，加上材质效果
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void OnValidate()
    {
        ChangeDistortion = Distortion;
        ChangeSizeX = SizeX;
        ChangeSizeY = SizeY;
        ChangeDropSpeed = DropSpeed;
    }

    void Update () {
        if (Application.isPlaying)
        {
            Distortion = ChangeDistortion;
            SizeX = ChangeSizeX;
            SizeY = ChangeSizeY;
            DropSpeed = ChangeDropSpeed;
        }

#if UNITY_EDITOR
        if (Application.isPlaying != true)
        {
            CurShader = Shader.Find("Unity/vertex_fragment_shaders/ScreenWaterDropEffect");
            ScreenWaterDropTex = Resources.Load("ScreenWaterDrop") as Texture2D;
        }
#endif
    }

    private void OnDisable()
    {
        if(CurMaterial)
        {
            DestroyImmediate(CurMaterial);
        }
    }
}
