[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.InteractiveCanvas/master/Icon.png "Zebble.InteractiveCanvas"


## Zebble.InteractiveCanvas

![logo]

A Zebble plugin that allow you to make other Zebble views interactive.


[![NuGet](https://img.shields.io/nuget/v/Zebble.InteractiveCanvas.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.InteractiveCanvas/)

> In some cases, we need to make some objects interactive in an application, this plugin makes you able to make other Zebble objects (Views) interactive without writing lots of code.

<br>


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.InteractiveCanvas/](https://www.nuget.org/packages/Zebble.InteractiveCanvas/)
* Install in your platform client projects.
* Available for iOS, Android and UWP.
<br>


### Api Usage

To make views interactive you just need to add your views as a child into this plugin like below:
```xml
<Zebble.Plugin.InteractiveCanvas Id="Interactor" Style="width: 200px; height:200px; background: #777777">
    <ImageView Path="Images/SamplePicture.png" />
</Zebble.Plugin.InteractiveCanvas>
```
By default, all of the ability of IneractiveCanvas enabled.

### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| CanRotateX            | bool           | x       | x   | x       |
| CanRotateY            | bool           | x       | x   | x       |
| CanRotateZ            | bool           | x       | x   | x       |
| CanDragX            | bool           | x       | x   | x       |
| CanDragY            | bool           | x       | x   | x       |
| CanScaleX            | bool           | x       | x   | x       |
| CanScaleY            | bool           | x       | x   | x       |
| RotateXTouches            | int           | x       | x   | x       |
| RotateYTouches            | int           | x       | x   | x       |
| RotateZTouches            | int           | x       | x   | x       |
| MaxRotateX            | float           | x       | x   | x       |
| MaxRotateY            | float           | x       | x   | x       |
| MaxRotateZ            | float           | x       | x   | x       |
| MaxDragX            | float           | x       | x   | x       |
| MaxDragY            | float           | x       | x   | x       |
| MaxScaleX            | float           | x       | x   | x       |
| MaxScaleY            | float           | x       | x   | x       |
| RotateXWithDevice            | bool           | x       | x   | x       |
| RotateYWithDevice            | bool           | x       | x   | x       |
| RotateZWithDevice            | bool           | x       | x   | x       |
