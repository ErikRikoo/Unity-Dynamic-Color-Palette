# Dynamic Color Palette - Unity

_Color palette that automatically updates dependencies_

## Motivation
While working in Unity, you often have to define colors on Meshes, UI, etc.
But when someone ask you to change one color in your design, you have to run through all the occurrences
to update.
You easily forgot some and then get weird results in your game.

This asset is there to fix that by creating links between a Color Palette and your dependencies.

I wanted to implement that in order to use on a [game](https://www.deadsigns.fr/) I work on with a team. 

## Installation
To install this package you can do one of this:
- Using Package Manager Window
    - Opening the Package Manager Window: Window > Package Manager
    - Wait for it to load
    - Click on the top left button `+` > Add package from git URL
    - Copy paste the [repository link](https://github.com/ErikRikoo/Unity-Dynamic-Color-Palette.git)
    - Press enter

- Modifying manifest.json file
Add the following to your `manifest.json` file (which is under your project location in `Packages` folder)
```json
{
  "dependancies": {
    ...
    "com.rikoo.dynamic-color-palette": "https://github.com/ErikRikoo/Unity-Dynamic-Color-Palette.git",
    ...
  }
}
```

You can now use it and don't want to kill your designer/artist anymore. 

## Updating
Sometimes Unity has some hard time updating git dependencies so when you want to update the package, 
follow this steps:
- Go into `package-lock.json` file (same place that `manifest.json` one)
- It should look like this:
```json
{
  "dependencies": {
    ...
    "com.rikoo.dynamic-color-palette": {
      "version": "https://github.com/ErikRikoo/Unity-Dynamic-Color-Palette.git",
      "depth": 0,
      "source": "git",
      "dependencies": {},
      "hash": "hash-number-there"
    },
    ...
}
```
- Remove the _"com.rikoo.dynamic-color-palette"_ and save the file
- Get back to Unity
- Let him refresh
- Package should be updated

## How does it work
### Creating and using the Color Palette
- Creating the color Palette 

It is a Scriptable Object so you can create as much as you want by 
Right Clicking in the Project Hierarchy view, and then choose ColorPalette.

- Using the Color Palette
    - Adding and Removing colors can be done by pressing `+` and `-` button on top right.
    - A color has a color and a name that can be used to distinguish between colors and have a clear label
    - You can remove the color you want by pressing the `x` button on the right of the color.
    
### Create links between the palette and an object
Currently, you have in the package some `MonoBehaviour` to create the link, they are all named like this `[Target]ColorLink`
- Renderer
- Image **[WIP]** 
- Generic

After adding the MonoBehaviour to an object:
- One of the top attribute should ask for a ColorPalette, just pick one
- Then it will show all the color available in the Palette, pick some and you will get a nice cyan border
- Everytime you update it in the the palette, you can see the change

The special case of GenericColorLinker
- You have the color linker like others
- You have a method storage attribute:
    - It allows you to pick a component on your object and then a valid method to bind to
        
### Creating new links
Current linker are using an abstract class to simplify the process and you can easily use it!
- Create a MonoBehaviour
- Extends AColorLinker
- Override the GetAction method which returns the right function
- Override the Initialize method if you need to do stuff before creating the link like caching components

**Exemple:**
```csharp
public class MyAmazingColorLinker : AColorLinker
{
    // Caching the renderer to use it when the color is updated 
    private Renderer m_Renderer;

    // Called in OnValidate this method allows you to do any prelude work before the linking
    protected override void Initialization()
    {
        if (m_Renderer == null)
        {
            m_Renderer = GetComponent<Renderer>();
        }
    }

    // We return a UnityAction to create link
    protected override UnityAction<Color> GetAction()
    {
        // You can directly return your function like this
        return OnColorUpdated;
    }


    // The function will be called everytime the color is changed in the palette
    public void OnColorUpdated(Color _newColor)
    {
        Debug.Log($"The color has changed: {_newColor}");
        // Doing great stuff with the renderer and the changed color
    }
}
```

## TODO
- [ ] Add information on the listeners in the Color Palette
- [ ] Allows to specify a default value for new colors
- [ ] Image Color Linker

## Suggestions
Feel free to suggest features by creating an issue, any idea is welcome !