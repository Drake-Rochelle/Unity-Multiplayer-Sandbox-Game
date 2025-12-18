using UnityEngine;


/// <summary>
/// 
///  ----- READ ME ----- 
///  
/// This event bus system should help us keep all event definitions in one folder and yet still
/// raise and sub to them from anywhere we want to.
/// 
/// CREATING A NEW EVENT:
/// 
/// 1. In the Assets/Scripts/Events folder create a newly defined event.
///     Right click... Create... and down near the bottom is "Game Event".
///     
/// 3. Name the event something specific and unique
/// 
/// 4. Alternatively use SHIFT-D to copy an existing event then rename it.
/// 
///   BE CLEAR in naming the event so that there is little doubt as to it's purpose.
/// 
/// 
/// RAISING AN EVENT:
/// 1. In the script that is RAISING the event:
///     a. Declare a variable of the type GameEventSO so that it shows up in the Inspector
///     b. In the Inspector choose your event or drag and drop it.
///     c. Invoke the event with:
///         "yourEventVariableNameHere.RaiseEvent(this, null);"
///     d. The 'this' is required, so that we know WHO is raising the event.
///     e. If your event has data replace the 'null' with it as appropriate. Almost any data
///        type is acceptable
///  
/// 
/// RESPONDING TO AN EVENT:
/// 5. In the responding GameObject (NOT SCRIPT):
///     a. Add a "Event Listener Component" component
///     b. Add a Listener to the list.
///     c. Select which event this is responding to
///     d. Click the big plus button to add a new Response.
///     e. Drag the appropriate script in
///     f. Choose the appropriate PUBLIC method to run.
///     g. Repeat d, e, f to add additional Responses if you need them.
///     
/// 
/// RESPONDING TO AN EVENT AND USING PASSED PARAMATER DATA
/// 6. Method responding to an event must have the following signature:
///     a. 'public void RespondingMethod(Component sender, object data)'
///     b. If data is used, cast it into the proper format inside the method as such:
///         - 'DesiredFormat newData = (DesiredFormat) data;'
///     c. This puts RespondingMethod at the top of the flyout menu 
///     in the EventListenerComponent when choosing which method will respond.
///     
///     
/// 7. NOTE: All Events:
///     1. During run-time have a list of subscribers
///     
/// 8. Noice.
///     
///     
/// Scripts did NOT come from: https://www.youtube.com/watch?v=e8WHdMI8hxk 
/// (first attempt at this)
/// but from: https://www.youtube.com/watch?v=7_dyDmF0Ktw&t=335s
/// 
/// </summary>
public class READMETOLEARNTHISSYSTEM 
{
}
