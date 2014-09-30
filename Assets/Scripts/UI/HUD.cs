using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : Singleton<HUD> {

	[SerializeField]
	private GameObject panel;

	[SerializeField]
	private GameObject targetingPrefab;
	[SerializeField]
	private float screenPadding = 0.485f;

	[SerializeField]
	private Texture allyArrow;
	[SerializeField]
	private Texture allyBox;

	[SerializeField]
	private Transform flagship;
	private Indicator flagshipIndicator;
	private bool onFlagship = false;

	[SerializeField]
	private Camera playerCamera;

	protected override void Awake(){
		base.Awake ();

		flagshipIndicator = ((GameObject)Instantiate(targetingPrefab)).GetComponent<Indicator>();
		flagshipIndicator.transform.parent = panel.transform;
		flagshipIndicator.transform.localScale = Vector3.one;
	}

	//That's {Target Ship, UI Element}
	private Dictionary<GameObject, GameObject> targetDisplayList = new Dictionary<GameObject, GameObject>();

	#region Flagship Indicator Flag Processing
	public void DockedIntoFlagship(){
		onFlagship = true;
		flagshipIndicator.gameObject.SetActive (false);
	}

	public void LaunchedFromFlagship(){
		onFlagship = false;
		flagshipIndicator.gameObject.SetActive (true);
	}
	#endregion

	//This is probably only ucamerar enemy units. Ally units will always be displayed anyway.
	public void AddTarget( GameObject newTarget ){
		//TODO Instantiate something here and add it
		targetDisplayList [newTarget] = null;
	}
	
	void Update () {

		//Process keys?

		//Paint all indicators
		Paint ();
	}

	//Paints all indicators
	void Paint(){
		if (!onFlagship) {
			//Draw Flagship
			DrawOnScreen( flagship.position, flagshipIndicator );
		}
	}

	//This is the main process of determining if the target is off the screen, and draws the appropriate indicator at the proper location
	void DrawOnScreen( Vector3 targetPosition, Indicator indicator ){

		//Find the viewport position of the target
		Vector3 viewportPoint = playerCamera.WorldToViewportPoint (targetPosition);

		//Center the origin point
		viewportPoint.x -= 0.5f;
		viewportPoint.y -= 0.5f;


		if (viewportPoint.z > 0f && viewportPoint.y > -screenPadding && viewportPoint.y < screenPadding && viewportPoint.x > -screenPadding && viewportPoint.x < screenPadding) {

			//The target is within the screen

			//Set the indicator to Box
			indicator.SetBox ();

			//Zero out the Z
			viewportPoint.z = 0f;

			//Set the position and rotation of the indicator
			indicator.transform.rotation = Quaternion.identity;

		} else {
			//The target is off the screen

			//Set the indicator to arrow
			indicator.SetArrow ();

			//Flip the viewport coordinates if the enemy is behind us
			if( viewportPoint.z < 0f ) viewportPoint *= -1;

			//Find the wide angle
			float rotationAngle = Quaternion.LookRotation( new Vector3( viewportPoint.x, 0f, viewportPoint.y ) ).eulerAngles.y;

			//Set our rotation
			indicator.transform.rotation = Quaternion.Euler( 0f, 0f, -rotationAngle );

//			Vector3 ray = new Vector3( Mathf.Sin( look * Mathf.Deg2Rad ), Mathf.Cos( look * Mathf.Deg2Rad ) );

			float sin = Mathf.Sin( rotationAngle * Mathf.Deg2Rad );
			float cos = Mathf.Cos( rotationAngle * Mathf.Deg2Rad );
			float tan = Mathf.Tan( rotationAngle * Mathf.Deg2Rad );

			if( Mathf.Abs( cos ) > Mathf.Abs( sin ) ){

				//We're touching a vertical bound

				if( cos > 0f ){
					//If we're on the top half of screen
					viewportPoint.y = screenPadding;
					viewportPoint.x = tan * screenPadding;
				} else {
					//If we're on the bottom half of the screen
					viewportPoint.y = -screenPadding;
					viewportPoint.x = -( tan * screenPadding );
				}

			} else {
				//We're touching a horizontal bound

				if( sin > 0f ){
					//If we're on the right side
					viewportPoint.x = screenPadding;
					viewportPoint.y = screenPadding / tan;
				} else {
					//If we're on the left side
					viewportPoint.x = -screenPadding;
					viewportPoint.y = - ( screenPadding / tan );
				}
			}
		}

		viewportPoint.x *= Screen.width;
		viewportPoint.y *= Screen.height;

		//Set the position
		indicator.transform.localPosition = viewportPoint;

	}

}