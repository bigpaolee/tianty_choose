// Scene objects:
var list : UIScrollList;
var itemPrefab : GameObject;
var partEmitter : ParticleEmitter;
var partAnimator : ParticleAnimator;
var fuelMeter : UIProgressBar;
var slider : UISlider;
var flameSizeBtn : UIStateToggleBtn;
var torchSound : AudioSource;

var leftRadio : UIRadioBtn;
var midRadio : UIRadioBtn;
var rightRadio : UIRadioBtn;

	

// Use this for initialization
function Start () 
{
	// Add list items:
	for(var i : int = 0; i<10; ++i)
	{
		var li : UIListItem = list.CreateItem(itemPrefab, ((i+1) * 10) + " Particles");
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
function Update ()
{
	if(partEmitter.emit)
	{
		// Deplete our fuel:
		fuelMeter.Value -= 0.6 * Time.deltaTime;
	}
	
	// See if we're out of fuel:
	if(fuelMeter.Value == 0)
	{
		partEmitter.emit = false;
		torchSound.Stop();
	}
		
	// Replenish some fuel each frame:
	fuelMeter.Value += 0.3 * Time.deltaTime;
}



//-------------------------------------
// Invoked methods:
//-------------------------------------

// Method that emits our particles.
// Invoked by the fire button
function EmitParticles()
{
	partEmitter.emit = !partEmitter.emit;
	
	if(partEmitter.emit)
		torchSound.Play();
	else
		torchSound.Stop();
}

// Invoked by our force slider
function SetFlamethrowerForce()
{
	partEmitter.worldVelocity = new Vector3(0, 150 * slider.Value, 0);
	torchSound.volume = Mathf.Lerp(0.5, 1.0, slider.Value);
	torchSound.pitch = Mathf.Lerp(0.7, 1.0, slider.Value);
}

// Invoked by our scroll list
function SetEmissionCount()
{
	var count : int = list.SelectedItem.Data;
	partEmitter.minEmission = count;
	partEmitter.maxEmission = count + 25;
}


//-------------------------------------
// Delegates:
//-------------------------------------

// Method that is called when a radio button is selected:
function RadioButtonSelected(obj : IUIObject)
{
	var btn : UIRadioBtn = obj;
	
	// Move our particle emitter to the location of
	// the selected radio button:
	partEmitter.transform.position = btn.transform.position + Vector3.forward;
}
	
// Called when our flame size toggle button changes:
function SetFlameSize(obj : IUIObject)
{
	var btn : UIStateToggleBtn = obj;
	
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
