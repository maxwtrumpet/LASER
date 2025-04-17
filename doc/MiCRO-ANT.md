# The MiCRO-ANT Game Engine

## File Structure

Typically, any game configuration files are written in JSON format. JSON can be very picky on syntax, so make sure you familiarize yourself with the language before continuing (or refer to pre-existing game files).

### Game Config

This file, located as `resources/game.config`, modifies certain aspects of the game startup process. The settings should always be wrapped in {}, as this is a JSON file. The following options are available:

* `"Initial Scene"`: the initial scene as a string with no .scene extension. Looks in `resources/scenes`.
* `"Smooth Audio Transitions"`: A boolean notating whether or not the audio transitions smoothly between scenes. Defaults to true.

### Rendering Config

This file, located as `resources/rendering.config`, modifies certain aspects of the rendering process. The settings should always be wrapped in {}, as this is a JSON file. The following options are available:

* `"Window Height"`: The inital height of the window, in int pixels. Defaults to 360.
* `"Window Width"`: The inital width of the window, in int pixels. Defeaults to 640.
* `"Background Color (r)"`: The background red color, an int 0-255. Defaults to 0.
* `"Background Color (g)"`: The background red color, an int 0-255. Defaults to 0.
* `"Background Color (b)"`: The background red color, an int 0-255. Defaults to 0.
* `"Zoom Factor"`: The float zoom factor of the game. Defaults to 1, where 100 pixels = 1 in-game unit.
* `"Game Title"`: The text put at the top of the game window. Defualts to "My MiCRO-ANT Game".
* `"Maintain Aspect Ratio"`: A boolean that notates whether or not the initial aspect ratio should be preserved when the user resizes or fullscreens the game window. Defaults to true.
* `"Start Fullscreen"`:  A boolean that notates whether or not the game begins in fullscreen. Defaults to false.
* `"Camera Type"`: A string that determines what type of camera view is used. "Perspective" camera changes the size of objects as they move further away (typical for 3D games), and "Orthographic" does not (typical for 2D games). Defaults to "Perspective".

### Actors and Templates

Actors are used to hold components that run code, like perform physics calculations, render images or text, or perform some custom function. The format for an actor in JSON is shown below:

```
{
    "template": "Put Template File Name Here",
    "name": "Put Your Name Here",
    "components": {
        "Key": {
            "type": "Built In Type Or Lua Component Name",
            "attribute": "string value",
            "other_attribute": number_value
        },
        ...
    }
}
```

The main key-pair values of an actor JSON object are:

* `"template"`: The template file that this actor inherits from, without the .template extension. Looks in `resources/actor_templates`. Anything that is in the actor JSON object in that file is inherited to the current actor. Templates can be nested, but not circularly dependent. The template key is optional. If a template is included, any key-pair values after this will modify the already pre-exisiting data taken from the template.
* `"name"`: A string representing the name of the actor.
* `"components"`: A nested JSON object that contains all the components that are included on this actor. In this nested JSON object, key-pair values are used to distinguish each component. The key for a component is important: **within an actor, each component is updated during the frame in alphabetical key order.** Each component then has its own nested JSON object, where any of the component's data can be changed. The only required key-value pair is the `"type"`, which denotes the built-in component name or Lua component files. If the latter, the value must be a string with no .lua extenstion that is location in `resources/component_types`. The file itself must be a Lua table with the same name.

### Scenes

A scene is a JSON object that contains actors. The format is as follows:

```
{
    "actors": {
        // Your actor JSON object here
        ...
    }
}
```

## Provided Life Cycle Functions

There are certain key functions that when written in a Lua component, MiCRO-ANT will autimatically call them under certain conditions.

Functions:
* `OnStart(self)`: Runs on the first frame that the component exists. If a component/actor was added during runtime, this will run the following frame that the component was actually created.
* `OnUpdate(self)`: Runs every frame.
* `OnLateUpdate(self)`: Runs every frame, after all other `OnUpdate` calls.
* `OnDestroy(self)`: Runs on the frame that a component is removed. Just like `OnStart`, this occurs the frame after the actual destroy or remove function is called.

* `OnCollisionEnter2D(self, contact)`: Runs the frame that two 2D colliders begin intersecting. `contact` is a `Collision` object (see `Collision` class).
* `OnCollisionExit2D(self, contact)`: Runs the frame that two 2D colliders end intersecting. `contact` is a `Collision` object (see `Collision` class).
* `OnriggerEnter2D(self, contact)`: Runs the frame that two 2D triggers begin intersecting. `contact` is a `Collision` object (see `Collision` class).
* `OnTriggerExit2D(self, contact)`: Runs the frame that two 2D triggers end intersecting. `contact` is a `Collision` object (see `Collision` class).

* `OnCollisionEnter3D(self, other)`: Runs the frame that two 3D colliders start intersecting. `other` is the `Actor` that this actor collided with (see `Actor` class).
* `OnCollisionStay3D(self, other)`: Runs every frame that two 3D colliders are intersecting. `other` is the `Actor` that this actor collided with (see `Actor` class).
* `OnCollisionExit3D(self, other)`: Runs the frame that two 3D colliders end intersecting. `other` is the `Actor` that this actor collided with (see `Actor` class).

* `OnTriggerEnter3D(self, other)`: Runs the frame that two 3D triggers start intersecting. `other` is the `Actor` that this actor collided with (see `Actor` class).
* `OnTriggerStay3D(self, other)`: Runs every frame that two 3D triggers are intersecting. `other` is the `Actor` that this actor collided with (see `Actor` class).
* `OnTriggerExit3D(self, other)`: Runs the frame that two 3D triggers end intersecting. `other` is the `Actor` that this actor collided with (see `Actor` class).

## Classes Provided

### The Actor Class

An Actor is essentially an object that holds multiple components and performs some sort of action. For example, you could have an actor that has a SpriteRenderer, Rigidbody 2D/3D, or any Lua script you wish to write.

Functions:

* `GetName(self)`: Gets the name of the actor. Returns a string.
* `GetID(self)`: Gets the unique id of the actor. Returns a number.
* `GetComponentByKey(self, string key)`: Returns the first component by reference on the actor with the specified key, if any exist.
* `GetComponent(self, string type)`: Returns the first component by reference on the actor with the specified type name, if any exist.
* `GetComponents(self, string type)`: Returns all components on the actor with the specified type name in a Lua Table.
* `AddComponent(self, string type)`: Adds a new component to the actor with the specified type name.
* `RemoveComponent(self, LuaRef component)`: Removes the component specified by the given reference from the actor.

### The Collision2D Class

This is the datatype that is returned upon a 2D collision or trigger event (see Life Cycle functions).

Variables:

* `other (Actor *)`: The other actor that this one interacted with.
* `point (Vector2)`: The point in space at which these two actors interacted.
* `relative_velocity (Vector2)`: The relative velocity at which the two actors interacted (only relevant for an entry collision).
* `normal (Vector2)`: The normal vector at the point of interaction (only relevant for an entry collision).

### The Collision3D Class

This is the datatype that will be returned upon a 3D collision or trigger event. However, this functionality is not working just yet and these events currently only return the actor that triggered the event (see Life Cycle functions).

### The HitResult2D Class

When conducting a 2D Raycast, this is the datatype that is returned.

Variables:

* `actor (Actor *)`: The other actor that the raycast hit.
* `point (Vector2)`: The point in space at which the raycast hit.
* `normal (Vector2)`: The normal vector at the point of the raycast hit.
* `is_trigger (bool)`: Whether or not the hitbox the raycast hit is a trigger.

### The HitResult3D Class

When conducting a 3D Raycast, this is the datatype that is returned.

Variables:

* `actor (Actor *)`: The other actor that the raycast hit.
* `point (Vector3)`: The point in space at which the raycast hit.
* `normal (Vector3)`: The normal vector at the point of the raycast hit.
* `is_trigger (bool)`: Whether or not the hitbox the raycast hit is a trigger.

### The Rigidbody2D Class

This is a C++ component that handles all 2D physics related events on an actor.

The Rigidbody2D class has many variables that are important to Box2D physics calculations or are basic variables common across all components:

* `type (string)`: The type name of the component (always "Rigidbody2D").
* `key (string)`: The unique key identifying this component.
* `body_type (string)`: The body type used when handles physics. See [the Box2D documentation](https://box2d.org/documentation/md__d_1__git_hub_box2d_docs_dynamics.html#autotoc_md52) for info on the three different types of bodies. This string must be "dynamic", "static", or "kinematic". Defaults to dynamic.
* `collider_type (string)`: Whether the collider is a "box" or a "circle". Defaults to box.
* `trigger_type (string)`: Whether the trigger is a "box" or a "circle". Defaults to box.
* `actor (Actor *)`: The actor this component belongs to.
* `x_position (float)`: The initial x position of the Rigidbody2D. Defaults to 0.
* `x_position (float)`: The initial y position of the Rigidbody2D. Defaults to 0.
* `gravity_scale (float)`: The gravity scale of the Rigidbody2D. Defaults to 1.
* `density (float)`: The density of the Rigidbody2D. Defaults to 1.
* `angular_friction (float)`: The angular friction applied to the Rigidbody2D. Defaults to 0.3.
* `rotation (float)`: The initial rotation of the Rigidbody2D, in degrees. Defaults to 0.
* `width (float)`: The collider width of the Rigidbody2D, if it is a box. Defaults to 1.
* `height (float)`: The collider height of the Rigidbody2D, if it is a box. Defaults to 1.
* `trigger_width (float)`: The trigger width of the Rigidbody2D, if it is a box. Defaults to 1.
* `trigger_height (float)`: The trigger height of the Rigidbody2D, if it is a box. Defaults to 1.
* `radius (float)`: The collider radius of the Rigidbody2D, if it is a circle. Defaults to 0.5.
* `trigger_radius (float)`: The trigger radius of the Rigidbody2D, if it is a circle. Defaults to 0.5.
* `friction (float)`: The friction applied to the Rigidbody2D. Defaults to 0.3.
* `bounciness (float)`: The bounciness of the Rigidbody2D. Defaults to 0.3.
* `enabled (bool)`: Whether or not this component is enabled.
* `precise (bool)`: See [bullets](https://box2d.org/documentation/md__d_1__git_hub_box2d_docs_dynamics.html#autotoc_md63) on the Box2D documentation. Defaults to true.
* `has_collider (bool)`: Whether or not this Rigidbody2D has a collider. Defaults to true.
* `has_trigger (bool)`: Whether or not this Rigidbody2D has a trigger. Defaults to true.
* `fixed_rotation (bool)`: Whether or not the rotation of this Rigidbody2D is fixed. Defaults to false.
* `ui (bool)`: Whether or not this is a UI Rigidbody2D. UI Rigidbodies positions are measured relative to the center of the screen, and only interact with other UI Rigidbodies. Defaults to false.

These variables are meant to only be modified before the Rigidbody2D is initialized (used either in a configuration file or when a new instance of a Rigidbody2D is created during runtime, the frame before it becomes active). If you want to retrieve or modify member data otherwise, use the following functions:

* `GetPosition()`: Gets the current position of the Rigidbody2D in a Vector2.
* `GetRotation()`: Gets the current rotation of the Rigidbody2D in degrees.
* `GetVelocity()`: Gets the current velocity of the Rigidbody2D in a Vector2.
* `GetAngularVelocity()`: Gets the current angular velocity of the Rigidbody2D in degrees.
* `GetGravityScale()`: Gets the current gravity scale of the Rigidbody2D.
* `GetUpDirection()`: Returns a unit Vector2 that is the "up" direction from the Rigidbody2D's perspective.
* `GetRightDirection()`: Returns a unit Vector2 that is the "right" direction from the Rigidbody2D's perspective.
* `GetDensity()`: Returns the current body density.
* `AddForce(Vector2 force)`: Applies the given force to the Rigidbody2D.
* `SetPosition(Vector2 position)`: Sets the current position of the Rigidbody2D to the given Vector2.
* `SetRotation(float degrees)`: Sets the current rotation of the Rigidbody2D to the given value in degrees.
* `SetVelocity(Vector2 velocity)`: Sets the current velocity of the Rigidbody2D to the given Vector2.
* `SetAngularVelocity(float degrees)`: Sets the current angular velocity of the Rigidbody2D to the given value in degrees.
* `SetGravityScale(float scale)`: Sets the current gravity scale of the Rigidbody2D to the given value.
* `SetUpDirection(Vector2 up)`: Sets the "up" direction from the Rigidbody2D's perspective to the given Vector2.
* `SetRightDirection(Vector2 right)`: Sets the "right" direction from the Rigidbody2D's perspective to the given Vector2.
* `SetDensity(float density)`: Set the body density to the given float.
* `SetColliderDimensions(Vector2 dimensions)`: Changes the dimensions of the Rigidbody2D's box collider. 
* `SetColliderRadius(Vector2 dimensions)`: Changes the dimensions of the Rigidbody2D's circle collider. 
* `SetTriggerDimensions(Vector2 dimensions)`: Changes the dimensions of the Rigidbody2D's box trigger. 
* `SetTriggerRadius(Vector2 dimensions)`: Changes the dimensions of the Rigidbody2D's circle trigger. 

### The Rigidbody3D Class

This is a C++ component that handles all 3D physics related events on an actor.

The Rigidbody3D class has many variables that are important to Bullet physics calculations or are basic variables common across all components:

* `type (string)`: The type name of the component (always "Rigidbody3D").
* `key (string)`: The unique key identifying this component.
* `body_shape (string)`: Whether this is a box or sphere collider. Defaults to "box".
* `body_type (string)`: Whether this is a collider or trigger. Defaults to "collider".
* `actor (Actor *)`: The actor this component belongs to.
* `x_position (float)`: The initial x position of the Rigidbody3D. Defaults to 0.
* `x_position (float)`: The initial y position of the Rigidbody3D. Defaults to 0.
* `z_position (float)`: The initial z position of the Rigidbody3D. Defaults to 0.
* `x_rotation (float)`: The initial x rotation of the Rigidbody3D, in degrees. Defaults to 0.
* `y_rotation (float)`: The initial y rotation of the Rigidbody3D, in degrees. Defaults to 0.
* `z_rotation (float)`: The initial z rotation of the Rigidbody3D, in degrees. Defaults to 0.
* `x_box_radius (float)`: The x box radius of the Rigidbody3D. Defaults to 0.5.
* `y_box_radius (float)`: The x box radius of the Rigidbody3D. Defaults to 0.5.
* `z_box_radius (float)`: The x box radius of the Rigidbody3D. Defaults to 0.5.
* `sphere_radius (float)`: The sphere radius of the Rigidbody3D. Defaults to 1.
* `mass (float)`: The mass of the object. If the mass is set to 0, the body becomes static and will not be affected by gravity or other forces. Defaults to 1.
* `x_gravity (float)`: The x gravity of the Rigidbody3D. Defaults to 0.
* `y_gravity (float)`: The y gravity of the Rigidbody3D. Defaults to -9.8.
* `z_gravity (float)`: The z gravity of the Rigidbody3D. Defaults to 0.
* `friction (float)`: The friction applied to the Rigidbody3D. Defaults to 0.3.
* `angular_friction (float)`: The angular friction applied to the Rigidbody3D. Defaults to 0.3.
* `bounciness (float)`: The bounciness of the Rigidbody3D. Defaults to 0.3.
* `enabled (bool)`: Whether or not this component is enabled.
* `fixed_rotation (bool)`: Whether or not the rotation of this Rigidbody3D is fixed. Defaults to false.

These variables are meant to only be modified before the Rigidbody3D is initialized (used either in a configuration file or when a new instance of a Rigidbody3D is created during runtime, the frame before it becomes active). If you want to retrieve or modify member data otherwise, use the following functions:

* `GetPosition()`: Gets the current position of the Rigidbody3D in a Vector3.
* `GetRotation()`: Gets the current rotation of the Rigidbody3D in a Vector3.
* `GetVelocity()`: Gets the current velocity of the Rigidbody3D in a Vector3.
* `GetAngularVelocity()`: Gets the current angular velocity of the Rigidbody3D in degrees.
* `GetGravity()`: Gets the current gravity of the Rigidbody3D in a Vector3.
* `GetMass()`: Returns the current body mass.
* `AddForce(Vector3 force)`: Applies the given force to the Rigidbody3D.
* `SetPosition(Vector3 position)`: Sets the current position of the Rigidbody3D to the given Vector3.
* `SetRotation(Vector3 degrees)`: Sets the current rotation of the Rigidbody3D to the given Vector3 in degrees.
* `SetVelocity(Vector3 velocity)`: Sets the current velocity of the Rigidbody3D to the given Vector3.
* `SetAngularVelocity(Vector3 degrees)`: Sets the current angular velocity of the Rigidbody3D to the given Vector3 in degrees.
* `SetGravity(Vector3 gravity)`: Sets the current gravity of the Rigidbody3D to the given Vector3.
* `SetMass(float mass)`: Set the body mass to the given float.

### The SpriteRenderer Class

This is a built in C++ component for rendering images. This component automatically searches for a Rigidbody 2D/3D component (prioritizng 2D) and will update the image position and rotation to match it. The following variables can be modified to alter the appearance:

* `type (string)`: The type name of the component (always "SpriteRenderer").
* `key (string)`: The unique key identifying this component.
* `sprite (string)`: The image being used, with no .png extension. Searches with a relative path of `resources/images`.
* `actor (Actor *)`: The actor this component belongs to.
* `x_position`: The initial x position of this image. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `y_position`: The initial y position of this image. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `z_position`: The initial z position of this image. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `x_alignment`: What part of the image the x position correlates to. 0 means the left, 1 means the right, and 0.5 means centered. Defaults to 0.5.
* `y_alignment`: What part of the image the y position correlates to. 0 means the top, 1 means the bottom, and 0.5 means centered. Defaults to 0.5.
* `x_position_offset`: The x offset from the initial position. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `y_position_offset`: The y offset from the initial position. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `z_position_offset`: The z offset from the initial position. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `x_rotation`: The initial x rotation of this image. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `y_rotation`: The initial y rotation of this image. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `z_rotation`: The initial z rotation of this image. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `x_rotation_offset`: The x offset from the initial rotation. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `y_rotation_offset`: The y offset from the initial rotation. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `z_rotation_offset`: The z offset from the initial rotation. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `x_scale`: The width scale of the image. Note that 100 pixels = 1 in-game unit. Defaults to 1.
* `y_scale`: The height scale of the image. Note that 100 pixels = 1 in-game unit. Defaults to 1.
* `r`: The red color mod (0-255) from the initial image. Defaults to 255.
* `g`: The green color mod (0-255) from the initial image. Defaults to 255.
* `b`: The blue color mod (0-255) from the initial image. Defaults to 255.
* `a`: The alpha color mod (0-255) from the initial image. Defaults to 255.
* `sorting_order`: The integer sorting order of the image. The higher the number, the more in the foreground an image is. For example, background elements would likely be negative numbers like -999, and special overlays would be positive numbers like 999. Only relevant for orthographic/UI rendering.
* `enabled (bool)`: Whether or not this component is enabled.

### The TextRenderer Class

This is a built in C++ component for rendering text. This component automatically searches for a Rigidbody 2D/3D component (prioritizng 2D) and will update the image position and rotation to match it. The following variables can be modified to alter the appearance:

* `type (string)`: The type name of the component (always "TextRenderer").
* `key (string)`: The unique key identifying this component.
* `text (string)`: The text to be rendered. Accepts special characters like \n.
* `font (string)`: The font being used, with no .ttf extension. Searches with a relative path of `resources/fonts`.
* `actor (Actor *)`: The actor this component belongs to.
* `x_position`: The initial x position of this text. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `y_position`: The initial y position of this text. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `z_position`: The initial z position of this text. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `x_alignment`: What part of the text the x position correlates to. 0 means the left, 1 means the right, and 0.5 means centered. Defaults to 0.5.
* `y_alignment`: What part of the text the y position correlates to. 0 means the top, 1 means the bottom, and 0.5 means centered. Defaults to 0.5.
* `x_position_offset`: The x offset from the initial position. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `y_position_offset`: The y offset from the initial position. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `z_position_offset`: The z offset from the initial position. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `x_rotation`: The initial x rotation of this text. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `y_rotation`: The initial y rotation of this text. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `z_rotation`: The initial z rotation of this text. Only relevant if there is no matching Rigidbody component. Defaults to 0.
* `x_rotation_offset`: The x offset from the initial rotation. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `y_rotation_offset`: The y offset from the initial rotation. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `z_rotation_offset`: The z offset from the initial rotation. Only relevant if there is a matching Rigidbody component. Defaults to 0.
* `size`: The font size. Defaults to 50.
* `r`: The text red color (0-255). Defaults to 0.
* `g`: The text green color (0-255). Defaults to 0.
* `b`: The text blue color (0-255). Defaults to 0.
* `a`: The text alpha (0-255). Defaults to 255.
* `sorting_order`: The integer sorting order of the text. The higher the number, the more in the foreground an text is. For example, background elements would likely be negative numbers like -999, and special overlays would be positive numbers like 999. Only relevant for orthographic/UI rendering.
* `enabled (bool)`: Whether or not this component is enabled.

### The Vector2 Class

This is a basic two dimensional vector with x and y member variables. It also has the following static functions:

* `Distance(Vector2 a, Vector2 b)`: Computes the distance between the two given vectors.
* `Dot(Vector2 a, Vector2 b)`: Computes the dot product between the two given vectors.
* `Length(Vector2 a)`: Get the magntiude/length of the vector.
* `Normalize(Vector2 a)`: Normalize the vector to have magnitude 1.

### The Vector3 Class

This is a basic three dimensional vector with x, y, and z member variables. It also has the following static functions:

* `Length(Vector3 a)`: Get the magntiude/length of the vector.
* `Normalize(Vector3 a)`: Normalize the vector to have magnitude 1.

## Namespaces Provided

### The Actor Namespace

This is a namespace used to perform general actor functions that don't apply to a specific insatnce of an actor.

Functions:

* `Find(string name)`: Returns a pointer to the first actor found in the current scene with the given name, if one exists.
* `FindAll(string name)`: Returns a Lua Table of all the actors found in the current scene with the given name.
* `Instantiate(string template)`: Instantiate an actor during runtime from the given template file (no extension, assuming it is in the actor_templates folder).
* `Destroy(Actor * actor)`: Destroy the actor indicated by the pointer.
* `DontDestroy(Actor* actor)`: Allow the given actor to persist across scenes without being deleted automatically. Can still be deleted manually.

### The Application Namespace

These are basic utility functions that relate to technical functions.

* `Quit()`: Immediately quit the program.
* `Sleep(int milliseconds)`: Sleep the thread for the given number of milliseconds.
* `GetFrame()`: Get the current game frame number.
* `OpenURL(string url)`: Open the provided URL.
* `FullPath(string path)`: Returns the full path of the given relative path.
* `IsCursorVisible()`: Returns a boolean representing whether or not the mouse is visible on the screen.
* `HideCursor()`: Hides the mouse from the screen.
* `ShowCursor()`: Shows the mouse on the screen.

### The Audio Namespace

This contains all audio utility functions. This engine integrates FMOD studio, so both basic sounds as well as FMOD Events are supported.

Functions:

* `PlaySound(string file, float volume, bool loops)`: Play the sound from the given file (extension included, assuming it is located in the audio folder) with the given volume and given loop status. This is not spatial audio and will sound the same everywhere. Returns an integer indicating which channel the sound is being played on.
* `Play3DSound(string file, float volume, bool loops, Vector3 position, Vector3 velocity)`: Play the sound from the given file (extension included, assuming it is located in the audio folder) with the given volume and given loop status. This is spatial audio, and the position and velocity are initialized to the given values. Returns an integer indicating which channel the sound is being played on.
* `StopSound(int channel)`: Stops the sound being played on the given channel, if there one.
* `IsChannelLive(int channel)`: Returns a bool indicating whether or not there is a sound currently playing on the given channel.
* `SetChannelVolume(int channel, float volume)`: Sets the volume of the given channel to the given value.
* `GetChannelVolume(int channel)`: Gets the volume of the given channel.
* `StopAllSounds(bool persistent)`: Stops all currently playing sounds, including ones that persist across scenes if given a `true` value.
* `LoadBank(string bank)`: Loads the FMOD bank with the given name (extension not included, assuming it is located in the banks folder).
* `PlayEvent(string event, Vector3 position, Vector3 velocity)`: Plays the given event with the specified spatial positon and velocity. If the event doesn't have a 3D panner, these values have no effect.
* `StopEvent(string event, bool fade)`: Stops the given effect and fades it based on the given boolean.
* `IsEventPlaying(string event)`: Returns a boolean indicating whether or not the event with the given name is currently playing.
* `GetEventParameter(string event, string parameter)`: Returns a float correlating the to the value of the given parameter on the given event. 
* `SetEventParameter(string event, string parameter, float value)`: Sets the given parameter on the given event to the given value.
* `SetListenerPosition(Vector2 position)`: Sets the position of the audio listener to the given Vector2.
* `SetListenerVelocity(Vector2 velocity)`: Sets the velocity of the audio listener to the given Vector2.
* `GetListenerPosition()`: Returns the position of the audio listener as a Vector2.
* `GetListenerVelocity()`: Returns the velocity of the audio listener as a Vector2.
* `SetEventPosition(string event, Vector2 position)`: Sets the position of the given event to the given Vector2.
* `SetEventVelocity(string event, Vector2 velocity)`: Sets the velocity of the given event to the given Vector2.
* `GetEventPosition(string event)`: Returns the position of the given event as a Vector2.
* `GetEventVelocity(string event)`: Returns the velocity of the given event as a Vector2.
* `SetChannelPosition(int channel, Vector2 position)`: Sets the position of the given channel to the given Vector2.
* `SetChannelVelocity(int channel, Vector2 velocity)`: Sets the velocity of the given channel to the given Vector2.
* `GetChannelPosition(int channel)`: Returns the position of the given channel as a Vector2.
* `GetChannelVelocity(int channel)`: Returns the velocity of the given channel as a Vector2.
* `StopAllEvents(bool fade, bool persistent)`: Stops all currently playing events (fading if applicable), including ones that persist across scenes if given a `true` value.

### The Camera Namespace

Basic Camera utility functions.

* `SetPosition(Vector3 position)`: Sets the position of the camera to the given Vector3.
* `GetPosition()`: Returns the position of the camera as a Vector3.
* `SetDirection(Vector3 direction)`: Sets the position that the camera is looking at to the given Vector3.
* `GetDirection(V)`: Returns the position that the camera is looking at as a Vector3.
* `SetZoom(float zoom)`: Sets the zoom factor of the camera to the given float.
* `GetZoom()`: Return the zoom factor of the camera as a float.

### The Debug Namespace

Two simple functions for printing debug statements to the command line.

* `Log(string message)`: Log a message to the command line.
* `LogError(string error)`: Log an error to the command line.

### The Event Namespace

A basic namespace for using the EventBus.

* `Publish(string type, LuaRef component)`: Publish an event of the given type to the EventBus with a reference to the component that published it.
* `Subscribe(string type, LuaRef component, LuaRef function)`: Subscribe the given component to all events of the given type, running the given function whenever an instance of that event is published.
* `Unsubscribe(string type, LuaRef component, LuaRef function)`: Unsubscribe the given function on the given component from all events of the given type.

### The Input Namespace

This is used to detect all kinds of keyboard, mouse, and controller inputs.

* `IsKeyDown(string key)`: Returns whether or not the given key was pressed this frame.
* `IsKeyJustDown(string key)`: Returns whether or not the given key was just pressed this frame.
* `IsKeyJustUp(string key)`: Returns whether or not the given key was just released this frame.
* `GetMousePosition()`: Returns a Vector2 in game units where the mouse is, meaured from the center of the screen.
* `SetMousePosition(Vector2 position)`: Sets the mouse position to the given Vector in game units, meaured from the center of the screen.
* `IsMouseDown(string key)`: Returns whether or not the given mouse button was pressed this frame.
* `IsMouseJustDown(string key)`: Returns whether or not the given mouse button was just pressed this frame.
* `IsMouseJustUp(string key)`: Returns whether or not the given mouse button was just released this frame.
* `GetMouseScrollDelta()`: Returns a float measuring how much the mouse wheel was scrolled.
* `NumControllersAvailable()`: Returns the number of game controllers connected at the time of game launch.
* `ActivateControllers(int number)`: Activates the given number of game controllers. Cannot be more than the number of game controllers connected at game launch.
* `IsJoyconDown(string key)`: Returns whether or not the given joycon button was pressed this frame.
* `IsJoyconJustDown(string key)`: Returns whether or not the given joycon button was just pressed this frame.
* `IsJoyconJustUp(string key)`: Returns whether or not the given joycon button was just released this frame.
* `GetPrimaryJoystickDelta()`: Returns a Vector2 measuring how much the primary joystick was pushed this frame.
* `GetSecondaryJoystickDelta()`: Returns a Vector2 measuring how much the secondary joystick was pushed this frame.

### The Physics Namespace

This is used for Raycasting in both 2D and 3D.
* `Raycast2D(Vector2 position, Vector2 direction, float distance)`: Send a 2D Raycast starting at the given position towards the given direction for the given amount of game units. Only returns the first HitResult2D.
* `RaycastAll2D(Vector2 position, Vector2 direction, float distance)`: Send a 2D Raycast starting at the given position towards the given direction for the given amount of game units. Returns all HitResult2Ds in a Lua table.
* `Raycast3D(Vector3 position, Vector3 direction, float distance)`: Send a 3D Raycast starting at the given position towards the given direction for the given amount of game units. Only returns the first HitResult3D.
* `RaycastAll3D(Vector3 position, Vector3 direction, float distance)`: Send a 3D Raycast starting at the given position towards the given direction for the given amount of game units. Returns all HitResult3Ds in a Lua table.

### The Scene Namespace

This is for a couple functions relevant to scenes.

* `Load(string scene)`: Queues the scene with the given name (no .scene extension) to be loaded the next frame. Looks for the relative path from `resources/scenes`.
* `GetCurrent()`: Gets the name of the current scene.