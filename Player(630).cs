using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float Factor;
    //最远运动距离
    public float MaxDistance = 2;
    //初始平台
    public GameObject Stage;
    public Transform Camera;
    public Text ScoreText;

    public Transform Head;
    public Transform Body;

    public Text Singscoretext;
    private Rigidbody _rigidbody;
    private float _startTime;
    private GameObject _currentStage;
    private Collider _lastCollisionCollider;
    Vector3 _cameraRelativePosition;
    private int _score;
    private bool _isUpdateScoreAnimation=false;
    private float _scoreAnimationStartTime;
    Vector3 _direction = new Vector3(1, 0, 0); //随机生成方向的方向定义
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

            Body.transform.DOScale(0.1f, 0.2f);   //人还原
            Head.transform.DOLocalMoveY(0.29f, 0.2f);

            
            _currentStage.transform.DOScale(new Vector3(1,0.5f,1),0.2f);   //台子还原
            _currentStage.transform.DOLocalMoveY(0.25f, 0.2f);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Body.transform.localScale += new Vector3(1, -1, 1) * 0.05f * Time.deltaTime;//deltatime每一帧渲染的时间（此处建议用fixupdate），o.o5是比例值
            Head.transform.localPosition += new Vector3(0, -1, 0) * 0.1f * Time.deltaTime; //根据父物体而改变位置
            _currentStage.transform.localScale += new Vector3(0, -1, 0)*0.15f*Time.deltaTime; //台子压缩
            _currentStage.transform.localPosition += new Vector3(0, -1, 0) * 0.15f * Time.deltaTime; //台子压缩
        }

       /* if (_isUpdateScoreAnimation)
        {
            UpdateScoreAnimation();
        } */
       
    }
    //根据时长决定跳的远近
    void OnJump(float elapse)
    {
        _rigidbody.AddForce((new Vector3(0, 1, 0)+_direction) * elapse * Factor,ForceMode.Impulse);
    }
    //自动生成平台方法
    void SpawnStage()
    {
        var stage = Instantiate(Stage);
        stage.transform.position = _currentStage.transform.position + _direction * Random.Range(1.1f, 1.8f);
        var randomScale = Random.Range(0.8f, 0.9f);
        stage.transform.localScale = new Vector3(randomScale,0.5f,randomScale);  //随机生成大小格子
        stage.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));//随机生成颜色，可以改进使得添加提前设置好的材质
            }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name.Contains("Stage") && collision.collider != _lastCollisionCollider)
        {
            _lastCollisionCollider = collision.collider;
            _currentStage = collision.gameObject;
            RandomDirection(); //随机生成方向的方法
            SpawnStage();
            MoveCamera();
            ShowScoreAnimation();

            _score++;
            ScoreText.text = _score.ToString();
        }
        if(collision.gameObject.name == "Ground")
        {
            //本局游戏结束，重新开始
            SceneManager.LoadScene(0);
        }
    }

    private void ShowScoreAnimation()
    {

        /*_isUpdateScoreAnimation = true;
        _scoreAnimationStartTime = Time.time; */
        
        
         Singscoretext.gameObject.SetActive(true);
        var pos=RectTransformUtility.WorldToScreenPoint(Camera.GetComponent<Camera>(),transform.position);  //转换为屏幕坐标
        Singscoretext.transform.position = pos;
Singscoretext.transform.DOMoveY(Singscoretext.transform.position.y + 100, 1).OnComplete( ()=>    //lambda表达式与匿名函数
         {
             Singscoretext.gameObject.SetActive(false); //利用oncomple方法回调
          }) ; //分数上扬动画
        

    }

    /* void UpdateScoreAnimation()
    {
        if (Time.time - _scoreAnimationStartTime > 1)
            _isUpdateScoreAnimation = false;

        var playerScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.GetComponent<Camera>(), transform.position);
        ScoreText.transform.position = playerScreenPos + Vector2.Lerp(Vector2.zero, new Vector2(0, 200), Time.time - _scoreAnimationStartTime);

        ScoreText.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), Time.time - _scoreAnimationStartTime);
    }  */




    void RandomDirection()
    {
        var seed = Random.Range(0, 2); //随机出0或1
        if (seed == 0)
        {
            _direction = new Vector3(1, 0, 0);
        }
        else
        {
            _direction = new Vector3(0, 0, 1);
        }
    }

    void MoveCamera()
    {

        Camera.DOMove(transform.position + _cameraRelativePosition,1);
       
    }
}
