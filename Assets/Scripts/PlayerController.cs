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
        }

        //fingerdrag for shooting
        fingerDrag();
    }


        private void fingerDrag(){
            //if clicked on the screen
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
                //update animation state from moving to aiming by setting isAiming to true
                Debug.Log("touch phase began");
                _animator.SetBool("isAiming", true);
            }
            else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended){
                //update the animation back to aiming state
                _animator.SetBool("isAiming", false);
            }

        }
}
