using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;

public class Clipper : MonoBehaviour
{
    public Transform TriggerPosTrans;

    private Transform _fingerTrans;
    private Material _handMat;

    private Vector3 _initPos;

    private float _hpTime;
    private float _colorTime;

    private float _colorChangeSpeed = 240 / 255f;

    private bool _isInit;

    public void Init(MeshRenderer mr)
    {
        _handMat = mr.sharedMaterial;
        _fingerTrans = mr.transform;

        _initPos = _fingerTrans.position;

        _isInit = true;
    }

    private void Update()
    {
        if (!_isInit) return;
        
        var isNoHitHandOrNail = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, TriggerPosTrans.position - Camera.main.transform.position, out hit))
        {
            if (hit.transform.CompareTag("NailShell"))
            {
                hit.transform.gameObject.AddComponent<Rigidbody>(); //指甲赋予刚体属性，当自由落体属性进行计算
                hit.transform.SetParent(null);
                Destroy(hit.transform.GetComponent<MeshCollider>());
                Destroy(hit.transform.gameObject,4);
                HandControl.Instance.NailShellDrop();
            }
            else if (hit.transform.CompareTag("Hand") || hit.transform.CompareTag("Nail"))
            {
                isNoHitHandOrNail = false;
                
                _hpTime += Time.deltaTime;
                Vector3 hitPoint = _fingerTrans.position - hit.point;
                _fingerTrans.Translate(new Vector3(hitPoint.x,0, hitPoint.z) * Time.deltaTime*0.5f,Space.World);
                {
                    _colorTime += Time.deltaTime;
                    var tmp = 1 - _colorChangeSpeed * _colorTime;
                    _handMat.SetColor("_Color", new Color(1, tmp, tmp));
                }
                //扣血机制
                if (_hpTime >= 0.3f)
                {
                    Data.HP -= 1;
                    if (Data.Setting_Vibration==1)
                    {
                        TapticManager.Impact(ImpactFeedback.Heavy);
                        TapticManager.Impact(ImpactFeedback.Heavy);
                        TapticManager.Impact(ImpactFeedback.Heavy);
                    }
                    if (Data.HP <= 0)
                    {
                        //TODO游戏结束界面结算
                        UIControl.Instance.LevelFailed();
                        if (Data.Setting_Vibration==1)
                        {
                            TapticManager.Impact(ImpactFeedback.Heavy);
                            TapticManager.Impact(ImpactFeedback.Heavy);
                            TapticManager.Impact(ImpactFeedback.Heavy);
                            TapticManager.Impact(ImpactFeedback.Heavy);
                            TapticManager.Impact(ImpactFeedback.Heavy);
                            TapticManager.Impact(ImpactFeedback.Heavy);
                        }
                    }
                    _hpTime = 0;
                    Debug.Log(Data.HP);
                }
                
            }
        }
        
        if (isNoHitHandOrNail)
        {
            if (_colorTime > 0)
            {
                _colorTime -= Time.deltaTime;
                var tmp = 1 - _colorChangeSpeed * _colorTime;
                _handMat.SetColor("_Color", new Color(1, tmp, tmp));
            }
            
            _fingerTrans.position = Vector3.Lerp(_fingerTrans.position, _initPos, Time.deltaTime * 2);
        }
    }
}
