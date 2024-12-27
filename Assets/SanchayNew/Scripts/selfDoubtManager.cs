/*using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class selfDoubtManager : MonoBehaviour
{
    public Volume vol; // Reference to the Volume component

    [Header("Effect Settings")]
    public float maxDistortIntensity = -0.5f; // Maximum distortion intensity
    public float maxAbbIntensity = 1f; // Maximum chromatic aberration intensity
    public float effectDuration = 2f; // Time for the effect to complete (to and fro)
    public float holdTime = 1f; // Time to hold at maximum intensity

    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    private void Start()
    {
        // Try to get Lens Distortion and Chromatic Aberration overrides from the Volume
        if (vol != null)
        {
            vol.profile.TryGet(out lensDistortion);
            vol.profile.TryGet(out chromaticAberration);
        }
        else
        {
            Debug.LogError("Volume reference not set on selfDoubtManager.");
        }
    }

    public void TriggerEffects()
    {
        if (lensDistortion != null)
            StartCoroutine(AnimateLensDistortion());

        if (chromaticAberration != null)
            StartCoroutine(AnimateChromaticAberration());
    }

    private IEnumerator AnimateLensDistortion()
    {
        // Animate intensity from 0 to max and back to 0
        float timer = 0f;

        // Gradually increase intensity
        while (timer < effectDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (effectDuration / 2);
            lensDistortion.intensity.value = Mathf.Lerp(0, maxDistortIntensity, t);
            yield return null;
        }

        // Hold at maximum intensity
        lensDistortion.intensity.value = maxDistortIntensity;
        yield return new WaitForSeconds(holdTime);

        // Gradually decrease intensity
        timer = 0f;
        while (timer < effectDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (effectDuration / 2);
            lensDistortion.intensity.value = Mathf.Lerp(maxDistortIntensity, 0, t);
            yield return null;
        }

        lensDistortion.intensity.value = 0; // Reset to default
    }

    private IEnumerator AnimateChromaticAberration()
    {
        // Animate intensity from 0 to max and back to 0
        float timer = 0f;

        // Gradually increase intensity
        while (timer < effectDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (effectDuration / 2);
            chromaticAberration.intensity.value = Mathf.Lerp(0, maxAbbIntensity, t);
            yield return null;
        }

        // Hold at maximum intensity
        chromaticAberration.intensity.value = maxAbbIntensity;
        yield return new WaitForSeconds(holdTime);

        // Gradually decrease intensity
        timer = 0f;
        while (timer < effectDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (effectDuration / 2);
            chromaticAberration.intensity.value = Mathf.Lerp(maxAbbIntensity, 0, t);
            yield return null;
        }

        chromaticAberration.intensity.value = 0; // Reset to default
    }
}
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class selfDoubtManager : MonoBehaviour
{
    public Volume vol;

    public float maxDistortIntensity = -0.5f;
    public float maxAbbIntensity = 1f;
    public float effectInterval = 5f;
    public float effectDuration = 2f;
    public float holdTime = 2f;

    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;
    private Coroutine effectCoroutine;


    private void Start()
    {
        // Fetch the volume effects
        if (vol != null)
        {
            vol.profile.TryGet(out lensDistortion);
            vol.profile.TryGet(out chromaticAberration);
        }
    }

    public void TriggerEffects()
    {
        if (lensDistortion != null)
            StartCoroutine(AnimateLensDistortion());

        if (chromaticAberration != null)
            StartCoroutine(AnimateChromaticAberration());
    }

    private IEnumerator TriggerEffectsPeriodically()
    {
        while (true)
        {
            TriggerEffects(); // Trigger the effects
            yield return new WaitForSeconds(effectInterval); // Wait for the interval
        }
    }

    public void StartRepeatingEffects()
    {
        if (effectCoroutine == null)
        {
            effectCoroutine = StartCoroutine(TriggerEffectsPeriodically());

        }
    }

    public void StopRepeatingEffects()
    {
        if (effectCoroutine != null)
        {
            StopCoroutine(effectCoroutine);
            effectCoroutine = null;
        }
        chromaticAberration.intensity.value = 0;
        lensDistortion.intensity.value = 0;
    }

    private IEnumerator AnimateLensDistortion()
    {
        Debug.Log("distorting lens");
        // Animation logic for lens distortion
        // Animate intensity from 0 to max and back to 0
        float timer = 0f;

        // Gradually increase intensity
        while (timer < effectDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (effectDuration / 2);
            lensDistortion.intensity.value = Mathf.Lerp(0, maxDistortIntensity, t);
            yield return null;
        }

        // Hold at maximum intensity
        lensDistortion.intensity.value = maxDistortIntensity;
        yield return new WaitForSeconds(holdTime);

        // Gradually decrease intensity
        timer = 0f;
        while (timer < effectDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (effectDuration / 2);
            lensDistortion.intensity.value = Mathf.Lerp(maxDistortIntensity, 0, t);
            yield return null;
        }

        lensDistortion.intensity.value = 0; // Reset to default
        yield return null;
    }

    private IEnumerator AnimateChromaticAberration()
    {
        Debug.Log("distorting chrome");

        // Animation logic for chromatic aberration
        // Animate intensity from 0 to max and back to 0
        float timer = 0f;

        // Gradually increase intensity
        while (timer < effectDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (effectDuration / 2);
            chromaticAberration.intensity.value = Mathf.Lerp(0, maxAbbIntensity, t);
            yield return null;
        }

        // Hold at maximum intensity
        chromaticAberration.intensity.value = maxAbbIntensity;
        yield return new WaitForSeconds(holdTime);

        // Gradually decrease intensity
        timer = 0f;
        while (timer < effectDuration / 2)
        {
            timer += Time.deltaTime;
            float t = timer / (effectDuration / 2);
            chromaticAberration.intensity.value = Mathf.Lerp(maxAbbIntensity, 0, t);
            yield return null;
        }

        chromaticAberration.intensity.value = 0; // Reset to default
        yield return null;
    }
}

