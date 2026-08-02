# 3DPinball

3DPinball is a 3D pinball game developed with Unity and C#.

The goal of the project is to implement the main systems of a pinball machine, including realistic ball physics, controllable flippers, a chargeable launcher, bumpers, scoring, lives, respawning, and camera movement.


## Features

* Physics-based ball movement
* Left and right controllable flippers
* Chargeable ball launcher
* Interactive bumpers
* Score system
* Lives system
* Ball respawning
* Game Over state
* Scene restart
* Smooth camera tracking
* Configurable values through the Unity Inspector

## Technologies

* C#
* Unity
* Unity Physics
* Rigidbody
* Colliders
* Hinge Joint
* Unity Input Manager

## Controls

| Key         | Action                           |
| ----------- | -------------------------------- |
| Left Arrow  | Activate the left flipper        |
| Right Arrow | Activate the right flipper       |
| Space       | Charge and launch the ball       |
| R           | Restart the game after Game Over |

## Project Structure

```text
3dpinball/
├── Scripts/
│   ├── BallController.cs
│   ├── FlipperController.cs
│   ├── PlungerController.cs
│   ├── BumperController.cs
│   ├── ScoreManager.cs
│   ├── GameManager.cs
│   └── CameraController.cs
├── Models/
├── Materials/
├── Audio/
└── Scenes/
```

## Scripts

### BallController.cs

Controls the pinball and manages:

* maximum ball speed;
* lost-ball detection;
* ball position reset;
* linear velocity reset;
* angular velocity reset.

### FlipperController.cs

Controls the left or right flipper using a `HingeJoint`.

Each flipper has configurable:

* keyboard input;
* released angle;
* pressed angle;
* spring force;
* spring damping.

### PlungerController.cs

Controls the ball launching mechanism.

The player holds the Space key to charge the launcher. The longer the key is held, the stronger the launch force becomes.

### BumperController.cs

Controls the pinball bumpers.

When the ball collides with a bumper:

* the ball receives an impulse force;
* points are added to the score;
* an optional sound effect can be played.

### ScoreManager.cs

Manages the current score.

It provides:

* score addition;
* score reset;
* on-screen score display;
* score change events.

### GameManager.cs

Manages the main game state.

It controls:

* player lives;
* lost balls;
* ball respawning;
* Game Over state;
* scene restart.

### CameraController.cs

Makes the camera smoothly follow the ball during gameplay.

## Unity Setup

### Ball

Create a Sphere and add:

* `SphereCollider`
* `Rigidbody`
* `BallController`

Recommended Rigidbody settings:

```text
Mass: 1
Linear Damping: 0.05
Angular Damping: 0.05
Interpolate: Interpolate
Collision Detection: Continuous Dynamic
Use Gravity: Enabled
```

### Flippers

Each flipper should contain:

* `BoxCollider`
* `Rigidbody`
* `HingeJoint`
* `FlipperController`

Example left flipper configuration:

```text
Control Key: Left Arrow
Released Angle: 0
Pressed Angle: 45
```

Example right flipper configuration:

```text
Control Key: Right Arrow
Released Angle: 0
Pressed Angle: -45
```

The angle values may need to be reversed depending on the orientation of the flipper models.

### Plunger

Create a trigger area near the initial ball position and add:

* `Collider`
* `PlungerController`

Enable `Is Trigger` on the Collider.

The forward Z axis of the launcher object determines the launch direction.

### Bumpers

Each bumper should contain:

* a Collider;
* `BumperController`.

The Collider should have `Is Trigger` disabled.

Example settings:

```text
Impulse Force: 10
Score Value: 100
```

### Game Managers

Create an empty GameObject named `Managers` and add:

* `GameManager`
* `ScoreManager`

Create another empty GameObject named `BallSpawnPoint` and place it at the initial ball position.

Assign the following fields in `GameManager`:

```text
Ball: the object with BallController
Ball Spawn Point: BallSpawnPoint
Starting Lives: 3
Respawn Delay: 1
```

### Camera

Add `CameraController` to the Main Camera.

Assign the ball as the camera target.

Example offset:

```text
X: 0
Y: 8
Z: -8
```

## Input Configuration

The current scripts use Unity's legacy Input Manager.

Open:

```text
Edit → Project Settings → Player → Active Input Handling
```

Select:

```text
Both
```

## Current Status

The project currently contains the basic systems required for a functional 3D pinball prototype.

The following systems are implemented:

* ball physics;
* flipper controls;
* ball launcher;
* bumpers;
* scoring;
* lives;
* respawning;
* Game Over state;
* camera tracking.

## Planned Improvements

* Custom 3D pinball table
* Main menu
* TextMeshPro user interface
* High-score system
* Multiple balls
* Combo system
* Missions and objectives
* Animated bumpers
* Sound effects
* Background music
* Particle effects
* Ball trail
* Tilt mechanic
* Save system
* Multiple levels
* Improved lighting
* Post-processing effects

## Purpose

The project was created to practise:

* object-oriented programming with C#;
* component-based game development;
* Unity physics;
* collision detection;
* input handling;
* game state management;
* scene organization;
* reusable gameplay components.

## License

This project is intended for educational purposes.