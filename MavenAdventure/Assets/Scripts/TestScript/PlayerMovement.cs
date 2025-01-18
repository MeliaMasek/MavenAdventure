using UnityEngine;

//code borrowed from https://gist.github.com/robertcedwards/dbaa2955a6e6e13f06df

public class PlayerMovement : MonoBehaviour
{
        public float speed = 10.0f;
        public GameObject character;

        void Update () {
		
            if (Input.GetKey(KeyCode.RightArrow)){
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow)){
                transform.position += Vector3.left* speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.UpArrow)){
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow)){
                transform.position += Vector3.back* speed * Time.deltaTime;
            }
        }
    }