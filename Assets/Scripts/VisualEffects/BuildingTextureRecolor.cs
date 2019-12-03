using UnityEngine;

public class BuildingTextureRecolor : MonoBehaviour
{
    [Header("Glow options")]

    [SerializeField]
    private Material _glowMaterial = null;

    [SerializeField]
    private float _glowRepeatTime = 1f;

    [SerializeField]
    private ParticleSystem.MinMaxGradient _glowColorGradient = default;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _glowColorIntensity = default;

    [Header("Tip Glow options")]

    [SerializeField]
    private Material _tipMaterial = null;

    [SerializeField]
    private float _tipRepeatTime = 1f;

    [SerializeField]
    private ParticleSystem.MinMaxGradient _tipColorGradient = default;

    [SerializeField]
    private ParticleSystem.MinMaxCurve _tipColorIntensity = default;

    void Update()
    {
        float glowTime = (Time.time / _glowRepeatTime) % 1f;
        _glowMaterial.SetColor("_EmissionColor", _glowColorGradient.Evaluate(glowTime) * _glowColorIntensity.Evaluate(glowTime));

        float tipGlowTime = (Time.time / _tipRepeatTime) % 1f;
        _tipMaterial.SetColor("_EmissionColor", _tipColorGradient.Evaluate(tipGlowTime) * _tipColorIntensity.Evaluate(tipGlowTime));
    }
}
