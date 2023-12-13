using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public float smoothing = 5f;
	Vector3 offset;

	// Set up camera follow and apply slight smoothing

	void Start () {
		offset = transform.position - target.position;
	}
	
	void LateUpdate () {
		Vector3 targetCamPos = target.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}