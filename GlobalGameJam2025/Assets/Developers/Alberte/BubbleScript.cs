using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    public Renderer _renderer;
    [SerializeField] AnimationCurve _DisplacementCurve;
    [SerializeField] AnimationCurve _DisplacementCurveFloor;
    [SerializeField] float _DisplacementMagnitude;
   [SerializeField] float _DisplacementMagnitudeFloor;
    [SerializeField] float _LerpSpeed;
    [SerializeField] float _DisolveSpeed;
    bool _shieldOn;
    Coroutine _disolveCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        //_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                HitShield(hit.point);
            }
        }

        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //GetComponent<Rigidbody>().AddRelativeForce(movement * 2);

    }

    public void HitShield(Vector3 hitPos)
    {
        _renderer.material.SetVector("_HitPos", hitPos);
        StopAllCoroutines();
        StartCoroutine(Coroutine_HitDisplacement());
    }

    //public void OpenCloseShield()
    //{
    //    float target = 1;
    //    if (_shieldOn)
    //    {
    //        target = 0;
    //    }
    //    _shieldOn = !_shieldOn;
    //    if (_disolveCoroutine != null)
    //    {
    //        StopCoroutine(_disolveCoroutine);
    //    }
    //    _disolveCoroutine = StartCoroutine(Coroutine_DisolveShield(target));
    //}

    IEnumerator Coroutine_HitDisplacement()
    {
        float lerp = 0;
        while (lerp < 1)
        {
            _renderer.material.SetFloat("_DisplacementStrength", _DisplacementCurve.Evaluate(lerp) * _DisplacementMagnitude);
            lerp += Time.deltaTime * _LerpSpeed;
            
            yield return null;
        }
    }
    IEnumerator Coroutine_HitDisplacementFloor()
    {
        float lerp = 0;
        while (lerp < 1)
        {
            _renderer.material.SetFloat("_DisplacementStrength", _DisplacementCurveFloor.Evaluate(lerp) * _DisplacementMagnitudeFloor);
            lerp += Time.deltaTime * _LerpSpeed;
            yield return null;
        }
    }
    public void Collision(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            _renderer.material.SetVector("_HitPos", collision.contacts[0].point);
            StopAllCoroutines();
            StartCoroutine(Coroutine_HitDisplacement());
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("COLLISIOOOOOON");
            _renderer.material.SetVector("_HitPos", collision.contacts[0].point);
            StopAllCoroutines();
            StartCoroutine(Coroutine_HitDisplacement());
        }

    }
    //public void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Floor")
    //    {
    //        _renderer.material.SetVector("_HitPos", collision.contacts[0].point);
    //        StopAllCoroutines();
    //        StartCoroutine(Coroutine_HitDisplacementFloor());
    //    }
    //}

    //IEnumerator Coroutine_DisolveShield(float target)
    //{
    //    float start = _renderer.material.GetFloat("_Disolve");
    //    float lerp = 0;
    //    while (lerp < 1)
    //    {
    //        _renderer.material.SetFloat("_Disolve", Mathf.Lerp(start, target, lerp));
    //        lerp += Time.deltaTime * _DisolveSpeed;
    //        yield return null;
    //    }
    //}
}
