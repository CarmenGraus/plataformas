using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	public Transform target;
	public float damping = 1; //La velocidad con la que la camara se mueve
	public float lookAheadFactor = 3; //La velocidad con la que cambia el movimiento de la camara de derecha a izq cuando cambia la direwccion del personaje
	public float lookAheadReturnSpeed = 0.5f;  //La velocidad 
	public float lookAheadMoveThreshold = 0.1f;  //Cuando cae el personaje la camara no cae con el
	public float yPosRestiction = -1;

	float offsetZ;
	Vector3 lastTargetPosition;
	Vector3 currentVelocity;
	Vector3 lookAheadPos;

	// Use this for initialization
	void Start () {
		lastTargetPosition = target.position;
		offsetZ = (transform.position - target.position).z;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
	
		// only update lookahead pos if accelerating or changed direction
		float xMoveDelta = (target.position - lastTargetPosition).x;
		
		bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
		
		if (updateLookAheadTarget) {
			lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
		} else {
			lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);	
		}
		
		Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);
		
		newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y,yPosRestiction,Mathf.Infinity), newPos.z);
		transform.position = newPos;
		
		lastTargetPosition = target.position;		
	}
}
