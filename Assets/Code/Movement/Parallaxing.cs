using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	
	public Transform[] backgrounds;			
	private float[] parallaxScales;			
	public float smoothing = 1f;			

	private Transform cam;					// reference to the main cameras transform
	private Vector3 previousCamPos;         // the position of the camera in the previous frame

    private Vector2[] StartPosition;

    public float MoveBorder = 2; 
	public float YSpeed;
	private float backgroundTargetPosY;
	private Vector2 parallax;
	void Awake () {
		cam = Camera.main.transform;
	}

	void Start () {
		previousCamPos = cam.position;
		
		// asigning coresponding parallaxScales
		parallaxScales = new float[backgrounds.Length];
        StartPosition = new Vector2[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z*-1;


            StartPosition[i] = backgrounds[i].transform.position;
           /* if (PlayerPrefs.GetFloat(backgrounds[i].name + "ParX")!=0)
            backgrounds[i].position = new Vector3(PlayerPrefs.GetFloat(backgrounds[i].name + "ParX"), backgrounds[i].position.y, backgrounds[i].position.z);
            */
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (GameObject.Find("Player").GetComponent<Player>().Getcollob().Contains(gameObject))
        {
            // for each background
            for (int i = 0; i < backgrounds.Length; i++)
            {
                if (backgrounds[i] != null)
                {
                    // the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
					parallax = new Vector2((previousCamPos.x - cam.position.x) * parallaxScales[i], (previousCamPos.y - cam.position.y) * parallaxScales[i]);
				
                    // set a target x position which is the current position plus the parallax
                    float backgroundTargetPosX = backgrounds[i].position.x + parallax.x;

					if(YSpeed!=0)  backgroundTargetPosY = backgrounds[i].position.y + parallax.x/YSpeed;
					else backgroundTargetPosY = backgrounds[i].position.y;
                    // create a target position which is the background's current position with it's target x position
					Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

                    // fade between current position and the target position using lerp
                    if (backgroundTargetPosX > StartPosition[i].x - MoveBorder && backgroundTargetPosX < StartPosition[i].x + MoveBorder)
                        backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
                }
            }

            // set the previousCamPos to the camera's position at the end of the frame
            previousCamPos = cam.position;
        }
    }

 
}
