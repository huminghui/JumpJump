using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float Factor;
    //最远运动距离
    public float MaxDistance = 5;
    //初始平台
    public GameObject Stage;
    public Transform Camera;
    public Text ScoreText;
    private Rigidbody _rigidbody;
    private float _startTime;
    private GameObject _currentStage;
    private Collider _lastCollisionCollider;
    Vector3 _cameraRelativePosition;
    private int _score;
	// Use this for initialization
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = Vector3.zero;
        _currentStage = Stage;
        _lastCollisionCollider = _currentStage.GetComponent<Collider>();
        SpawnStage();
        _cameraRelativePosition =Camera.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            _startTime = Time.time;

        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            var elapse = Time.time - _startTime;
            OnJump(elapse);
        }
	}
    //根据时长决定跳的远近
    void OnJump(float elapse)
    {
        _rigidbody.AddForce(new Vector3(1, 1, 0) * elapse * Factor,ForceMode.Impulse);
    }
    //自动生成平台方法
    void SpawnStage()
    {
        var stage = Instantiate(Stage);
        stage.transform.position = _currentStage.transform.position + new Vector3(Random.Range(1.1f, MaxDistance),0,0);  
            }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name.Contains("Stage") && collision.collider != _lastCollisionCollider)
        {
            _lastCollisionCollider = collision.collider;
            _currentStage = collision.gameObject;
            SpawnStage();
            MoveCamera();
            _score++;
            ScoreText.text = _score.ToString();
        }
        if(collision.gameObject.name == "Ground")
        {
            //本局游戏结束，重新开始
            SceneManager.LoadScene(0);
        }
    }
    void MoveCamera()
    {

        Camera.DOMove(transform.position + _cameraRelativePosition,1);
       
    }
}
