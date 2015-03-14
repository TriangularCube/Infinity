using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetingHUD : PimpedMonoBehaviour {

#pragma warning disable 0649
    [SerializeField]
    private GameObject targetingHUD;
#pragma warning restore 0649

    void Update() {
        DrawIndicators();
    }


    #region Target Indicators
#pragma warning disable 0649
    [SerializeField, Group( "Target Indicators" )]
    private float screenPadding = 0.485f;

    [SerializeField, Group( "Target Indicators" )]
    private Camera playerCamera;


    [SerializeField, Group( "Target Indicators" )]
    private Flagship flagship;
    [SerializeField, Group( "Target Indicators" )]
    private AllyIndicator flagshipIndicator;


    [SerializeField, Group( "Target Indicators" )]
    private GameObject allyTargetingPrefab;
#pragma warning restore 0649

    private Dictionary<Transform, AllyIndicator> allyIndicatorList = new Dictionary<Transform, AllyIndicator>();

    private void DrawIndicators() {
        //Update Flagship Indicator
        if( flagshipIndicator.gameObject.activeSelf ) {
            DrawIndicatorOnScreen( flagship.transform.position, flagshipIndicator );
        }

        //Draw Ally indicators
        foreach( Transform ally in allyIndicatorList.Keys ) {
            DrawIndicatorOnScreen( ally.position, allyIndicatorList[ally] );
        }

        //Draw other indicators
    }

    //This is the main process of determining if the target is off the screen, and draws the appropriate indicator at the proper location
    private void DrawIndicatorOnScreen( Vector3 targetPosition, AllyIndicator indicator ) {

        //Find the viewport position of the target
        Vector3 viewportPoint = playerCamera.WorldToViewportPoint( targetPosition );

        //Center the origin point
        viewportPoint.x -= 0.5f;
        viewportPoint.y -= 0.5f;


        if( viewportPoint.z > 0f && viewportPoint.y > -screenPadding && viewportPoint.y < screenPadding && viewportPoint.x > -screenPadding && viewportPoint.x < screenPadding ) {

            //The target is within the screen

            //Set the indicator to Box
            indicator.SetBox();

            //Set the position and rotation of the indicator
            indicator.transform.rotation = Quaternion.identity;

        } else {
            //The target is off the screen

            //Set the indicator to arrow
            indicator.SetArrow();

            //Flip the viewport coordinates if the enemy is behind us
            if( viewportPoint.z < 0f ) viewportPoint *= -1;

            //Find the wide angle
            float rotationAngle = Vector3.Angle( Vector3.up, new Vector3( viewportPoint.x, viewportPoint.y, 0f ) );
            if( viewportPoint.x > 0f ) {
                rotationAngle *= -1;
            }

            //Set our rotation
            indicator.transform.rotation = Quaternion.Euler( 0f, 0f, rotationAngle );

            float sin = Mathf.Sin( rotationAngle * Mathf.Deg2Rad );
            float cos = Mathf.Cos( rotationAngle * Mathf.Deg2Rad );
            float tan = Mathf.Tan( rotationAngle * Mathf.Deg2Rad );

            if( Mathf.Abs( cos ) > Mathf.Abs( sin ) ) {

                //We're touching a vertical bound

                if( cos > 0f ) {
                    //If we're on the top half of screen
                    viewportPoint.y = screenPadding;
                    viewportPoint.x = -( tan * screenPadding );
                } else {
                    //If we're on the bottom half of the screen
                    viewportPoint.y = -screenPadding;
                    viewportPoint.x = tan * screenPadding;
                }

            } else {
                //We're touching a horizontal bound

                if( sin < 0f ) {
                    //If we're on the right side
                    viewportPoint.x = screenPadding;
                    viewportPoint.y = -( screenPadding / tan );
                } else {
                    //If we're on the left side
                    viewportPoint.x = -screenPadding;
                    viewportPoint.y = screenPadding / tan;
                }
            }

        }

        //Zero out the Z
        viewportPoint.z = 0f;

        viewportPoint.x *= Screen.width;
        viewportPoint.y *= Screen.height;

        //Set the position
        indicator.transform.localPosition = viewportPoint;

    }

    public void TurnOnAllyIndicator( Terminal term ) {

        //TODO Replace this with a pool
        GameObject indicator = Instantiate<GameObject>( allyTargetingPrefab );
        indicator.transform.parent = targetingHUD.transform;
        indicator.transform.localScale = Vector3.one;
        indicator.transform.localPosition = Vector3.zero;

        allyIndicatorList.Add( term.transform, indicator.GetComponent<AllyIndicator>() );

    }

    public void TurnOffAllyIndicator( Terminal term ) {
        allyIndicatorList.Remove( term.transform );
    }
    #endregion Target Indicators
}
