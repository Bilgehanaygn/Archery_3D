using UnityEngine;

public class Canvas : MonoBehaviour
{
    
    public void OnLeftClick(){
        PlayerController.MoveLeft = true;
    }

    public void OnLeftLifted(){
        PlayerController.MoveLeft = false;
    }

    public void OnRightClick(){
        PlayerController.MoveRight = true;
    }

    public void OnRightLifted(){
        PlayerController.MoveRight = false;
    }


    
}
