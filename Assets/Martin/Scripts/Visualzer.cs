using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualzer : MonoBehaviour
{
    public GameObject _sampleCubePrefab;
    GameObject[] _sampleCube = new GameObject[512];
    public float maxScale;
    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject _instanceSampleCube = Instantiate(_sampleCubePrefab, this.transform.position , Quaternion.identity,this.transform);
            _instanceSampleCube.name = "SampleCube" + i;
            _instanceSampleCube.transform.position = new Vector3(-i, 0, 0);
            _instanceSampleCube.transform.position = Vector3.forward * 100;
            _sampleCube[i] = _instanceSampleCube;
        }

        //for (int i = 0; i < 512; i++)
        //{
        //    GameObject _instanceSampleCube = Instantiate(_sampleCubePrefab);
        //    _instanceSampleCube.transform.position = this.transform.position;
        //    _instanceSampleCube.transform.parent = this.transform;
        //    _instanceSampleCube.name = "SampleCube" + i;
        //    this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
        //    _instanceSampleCube.transform.position = Vector3.forward * 100;
        //    _sampleCube[i] = _instanceSampleCube;
        //}
    }
    

    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (_sampleCube != null)
            {
                _sampleCube[i].transform.localScale = new Vector3(10, (AudioData._samples[i]*maxScale)+2, 10);
            }
        }
    }
}
