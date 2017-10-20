using UnityEngine;
using NUnit.Framework;
using System.Runtime.InteropServices;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6.0f;

	private Vector3 movement;
	private Animator anim;
	private Rigidbody rb;
	private int floorMask;
	private float camRayLength = 100.0f;

	void Awake(){
		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate(){
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Move(h, v);
		Turning();
		Animating(h, v);
	}

	void Move(float h, float v){
		movement.Set(h, 0.0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		rb.MovePosition(transform.position + movement);
	}

	void Turning(){
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit floorHit;

		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)){
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0.0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			rb.MoveRotation(newRotation);
		}
	}

	void Animating(float h, float v){
		bool walking = (h != 0 || v != 0);
		anim.SetBool("IsWalking", walking);
	}
}
