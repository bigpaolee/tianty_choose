using UnityEngine;
using System.Collections;


public class EZ_GUI_Simple_Demo_CS : MonoBehaviour 
{
	// Scene objects:
	public UIScrollList list;
	public GameObject itemPrefab;
	public ParticleEmitter partEmitter;
	public ParticleAnimator partAnimator;
	public UIProgressBar fuelMeter;
	public UISlider slider;
	public UIStateToggleBtn flameSizeBtn;
	public AudioSource torchSound;
	
	public UIRadioBtn leftRadio, midRadio, rightRadio;
	

	// Use this for initialization
	void Start () 
	{
		// Add list items:
		for(int i=0; i<10; ++i)
		{
			UIListItem li = (UIListItem) list.CreateItem(itemPrefab, ((i+1) * 10) + " Particles");
			li.Data = (i+1) * 10;
		}
		
		// Select the first item by default:
		list.SetSelectedItem(0);
		
		// Register a value changed delegate for
		// the radio buttons:
		leftRadio.SetValueChangedDelegate(RadioButtonSelected);
		midRadio.SetValueChangedDelegate(RadioButtonSelected);
		rightRadio.SetValueChangedDelegate(RadioButtonSelected);
		
		// Register a value changed delegate for
		// our flame size toggle:
		flameSizeBtn.SetValueChangedDelegate(SetFlameSize);
		
		// Get our initial values from our slider:
		SetFlamethrowerForce();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(partEmitter.emit)
		{
			// Deplete our fuel:
			fuelMeter.Value -= 0.6f * Time.deltaTime;
		}
		
		// See if we're out of fuel:
		if(fuelMeter.Value == 0)
		{
			partEmitter.emit = false;
			torchSound.Stop();
		}
			
		// Replenish some fuel each frame:
		fuelMeter.Value += 0.3f * Time.deltaTime;
	}
	

	
	//-------------------------------------
	// Invoked methods:
	//-------------------------------------

	// Method that emits our particles.
	// Invoked by the fire button
	void EmitParticles()
	{
		partEmitter.emit = !partEmitter.emit;
		
		if(partEmitter.emit)
			torchSound.Play();
		else
			torchSound.Stop();
	}

	// Invoked by our force slider
	void SetFlamethrowerForce()
	{
		partEmitter.worldVelocity = new Vector3(0, 150f * slider.Value, 0);
		torchSound.volume = Mathf.Lerp(0.5f, 1.0f, slider.Value);
		torchSound.pitch = Mathf.Lerp(0.7f, 1.0f, slider.Value);
	}
	
	// Invoked by our scroll list
	void SetEmissionCount()
	{
		Debug.Log("mmmmmmmmmmmmmmmmmm");

		int count = (int) list.SelectedItem.Data;
		partEmitter.minEmission = count;
		partEmitter.maxEmission = count + 25;
	}

	
	//-------------------------------------
	// Delegates:
	//-------------------------------------

	// Method that is called when a radio button is selected:
	void RadioButtonSelected(IUIObject obj)
	{
		UIRadioBtn btn = (UIRadioBtn) obj;
		
		// Move our particle emitter to the location of
		// the selected radio button:
		partEmitter.transform.position = btn.transform.position + Vector3.forward;
	}
		
	// Called when our flame size toggle button changes:
	void SetFlameSize(IUIObject obj)
	{
		UIStateToggleBtn btn = (UIStateToggleBtn) obj;
		
		// Find which of 3 states our toggle button is in:
		switch(btn.StateNum)
		{
			case 0:
				partAnimator.sizeGrow = 0;
				break;
			case 1:
				partAnimator.sizeGrow = 3;
				break;
			case 2:
				partAnimator.sizeGrow = 10;
				break;
		}
	}
}
