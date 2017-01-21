# GoogleVR demo

## Instructions

Build to Google Cardboard ready Android device and see my Precise Profile.
Click and drag to move sections around the room

## Notes

This project was built using Unity v5.4.2f2-GVR13, which was the latest version of Unity Daydream Preview available at time of writing.

This project also containsGoogleVR SDK v1.10

Requires Android SDK 24 a.k.a. Android 7 (Nougat)


## Dev
Objects tagged with 'Wall' will be treated as walls and will allow objects with the component script 'MoveObjectAlongWalls' to move along them

## Limitations
Objects with the component 'MoveObjectAlongWalls' can only move along walls that are rotated in multiples of 90 degrees (i.e. a four sided square room).
Rotating a wall at other angles will result in the moveable objects being placed near the point on the wall in an inconsistent position and rotation.

On some walls the object texture/text may be upside down or backwards. This functionality is still in development.
