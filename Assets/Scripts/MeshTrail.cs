using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activateTime = 2f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;
    public string shaderVarRef = "_Alpha";
    public float shaderVarRate = 0.02f;
    public float shaderVarRefreshRate = 0.05f;

    private bool isTrailActive = false;
    private MeshFilter meshFilter;
    

    // Update is called once per frame
    void Update()
    {
        // if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && !isTrailActive)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isTrailActive) 
            {
                isTrailActive = true;
                StartCoroutine(ActivateTrail(activateTime));
            }
            else
            {
                isTrailActive = false;
            }
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (isTrailActive)
        // while (timeActive > 0)
        // while (!Input.GetKeyUp(KeyCode.UpArrow) && !Input.GetKeyUp(KeyCode.RightArrow))
        {
            timeActive -= meshRefreshRate;

            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }
            
            GameObject gObj = new GameObject();
            gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

            MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
            MeshFilter mf = gObj.AddComponent<MeshFilter>();

            // Mesh mesh = new Mesh();
            // mesh.material = meshRenderer.material;

            mf.mesh = meshFilter.mesh;
            mr.material = mat;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

            Destroy(gObj, meshDestroyDelay);

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
