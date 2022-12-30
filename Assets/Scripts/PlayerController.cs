using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    //character movement related fields
    public static float RunningSpeed {get; private set;} = 5f;
    private float _leftRightSpeed = 3f;
    private float _limitX = 4.4f;


    //animation related fields
    private Animator _animator;


    //shoot related fields
    private LineRenderer _lineRenderer;
    private float _force = 30f;
    private float _directionDivider = 10f;
    private int _maxIterations = 100;
    private Vector2 _firstTouch;
    [SerializeField] private GameObject _arrowPrefab;

    #region observer
        // public static event System.Action OnMoveLeft;
        // public static event System.Action OnMoveRight;
        public static bool MoveLeft = false;
        public static bool MoveRight = false;
    #endregion

    private void Awake() {
        if(Instance != null && Instance != this){
            Destroy(this);
        }
        else if(Instance == null){
            Instance = this;
        }
    }

    private void Start() {
        _animator = GetComponentInChildren<Animator>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {

        //Mathf.Clamp(-limitX, +limitX)
        if(MoveLeft){//case left and forward
            float newX = Mathf.Clamp(transform.position.x - (_leftRightSpeed * Time.deltaTime), -_limitX, _limitX);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z + (RunningSpeed * Time.deltaTime));
        }
        else if(MoveRight){//case right and forward
            float newX = Mathf.Clamp(transform.position.x + (_leftRightSpeed * Time.deltaTime), -_limitX, _limitX);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z + (RunningSpeed * Time.deltaTime));
        }
        else{//case only forward
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (RunningSpeed * Time.deltaTime));
            //fingerdrag for shooting is called only if the character is not moving to the left or to the right
            FingerDrag();
        }

        
        
    }


    private void FingerDrag(){
        //if clicked on the screen
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            //update animation state from moving to aiming by setting isAiming to true
            _animator.SetBool("isAiming", true);
            _firstTouch = Input.GetTouch(0).position;
        }
        else if(Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)){
            //rotate the character depending on the x axis
            RotateCharacter(Input.GetTouch(0).position);
        }
        else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
            //update the animation back to aiming state
            _animator.SetBool("isAiming", false);
            ThrowAnArrow(Input.GetTouch(0).position);
            ResetCharacterRotate();
            //throw an arrow
            // ThrowAnArrow(Input.GetTouch(0).position);
        }

    }


    // private void RenderTrajectory(Vector2 lastPosition){
    //     //the arrow follows a straight line (will not fall ground), thus the arrow sent should be garbaged later and there should be no gravity on the arrow
    //     //there is no gravity force on the arrow so the distance moved for the arrow is V0y*t

    //     Vector2 direction = (_firstTouch - lastPosition).normalized;
    //     Vector3 throwForce = new Vector3(direction.x, direction.y, 1.0f)*_force;

    //     _aimX = _firstTouch.x - lastPosition.x;
    //     _aimY = _firstTouch.y - lastPosition.y;

    //     //set the line renderers position count to iterations
    //     _lineRenderer.positionCount = _maxIterations;
    //     for(int i=0;i<_maxIterations;i++){
    //         float newX = transform.position.x + (_aimX/_directionDivider*i*Time.fixedDeltaTime);

    //         //the reason for additional 5f in Y axis is to align the arrow and the bow
    //         float newY = transform.position.y + 1.5f + (_aimY/_directionDivider*i*Time.fixedDeltaTime) - ((9.8f)*Mathf.Pow(i*Time.fixedDeltaTime,2))/2;

    //         float newZ = transform.position.z + 0.5f + (_force*i*Time.fixedDeltaTime);

    //         // float newX = transform.position.x + (throwForce.x*_force*i*Time.fixedDeltaTime);

    //         // float newY = transform.position.y + 1.5f + (throwForce.y*_force*i*Time.fixedDeltaTime - (0.5f*(9.8f)*(Mathf.Pow(i*Time.fixedDeltaTime,2))));

    //         // float newZ = transform.position.z + 1.5f + (throwForce.z * _force*i*Time.fixedDeltaTime);

    //         Vector3 nextPosition = new Vector3(newX, newY, newZ);

    //         _lineRenderer.SetPosition(i, nextPosition);
    //     }

    //     //calculate the distance in X from the first touch to latest position of the finger
    // }


    private void RotateCharacter(Vector2 lastFingerPosition){
        float distanceX = _firstTouch.x - lastFingerPosition.x;
        float rotateInDeg = Mathf.Atan((distanceX*3)/Screen.width) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f,rotateInDeg,0f);
    }

    private void ResetCharacterRotate(){
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void ThrowAnArrow(Vector2 lastPosition){
        Vector2 direction = (_firstTouch - lastPosition).normalized;
        Vector3 throwForce = new Vector3(direction.x, 0f, direction.y)*_force;

        //rotate the arrow on y-axis depending on the distance between initial point and last position of the finger in the x axis
        //so the rotate of the arrow will be same as the character thus instantiating with transform.rotation is enough when 
        float rotateOnY = 90-(Screen.width/2)/(_firstTouch.x-lastPosition.x);
        GameObject arrow = Instantiate(_arrowPrefab, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z+ 1.5f), transform.rotation);
        arrow.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.VelocityChange);
    }


    private void OnCollisionEnter(Collision other) {

        if(other.gameObject.tag.Equals("Arrow")){
            Debug.Log("arrow hit to player body");
        }
    }

}
