# Unity Experiment Tools

Collection of functions useful for VR experiments in Unity

## Classes
### SaveTracking
- Attach script to any GameObject to start tracking.
- Adjust settings in the inspector
- Drag and drop a GameObject to "Tracked Gameobject" to track it's position and/or orientation. This could for example be the Camera object to track head position and orientation
- Use SaveTracking.Msg(string msg) in any script to write a message in the tracking file

### ExperimentPreparation
- Shuffle arrays
