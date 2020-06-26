## Setup & Settings Screen

In order to begin using SHInspect you will need to add a window to view. This accepts either an automation id or the name (window title). These may be generated for you, although it is recommended to add an automation Id to your application especially if the window title can change.

In order to access the settings screen click the cog in the top left of the SHInspect application.

![image](/SHInspect/Documentation/images/Settings.PNG)

### Adding a window

On this screen you can see your currently 'Active Windows' and your 'Windows To Display'. To manually add a window, first click on an active window. This will prefill the name and automation id of that window. This will preferentially use the window automation id and populate a text box below. This text box acts as an identifier so that SHInspect can find your window. Next click the add button to add your window.

### Removing a window
You can edit a windows identifier by selecting the window in 'Windows To Display' and changing the text in the text box.

### Deleting a window
You can remove a window by clicking a window in 'Windows To Display' and then pressing the delete button.

### Changing the Inspection Colour

The Inspection colour is the colour that shows when you are inspecting a control with your cursor whilst holding CTRL + ALT. You may wish to change this to stand out more with your application, for accessbility or for aesthetic purposes.

### Test Crash
The test crash button is also available from within the settings screen. This button is only enabled when running SHInspect in debug mode and will generate a test crash. This is useful for tweaking the test crash window.