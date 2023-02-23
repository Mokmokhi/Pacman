# DFD Specification Document

Project Title: Become Pac-Man (Fake)

Document version: 0

Printing date: 26 Jan 2023

Group ID: F8

Group Member

    1155127434 HO Chun Lung Terrance
    Department of Philosophy, The Chinese University of Hong Kong

    1155143519 WOO Pok
    Department of Physics, The Chinese University of Hong Kong

    1155157839 NG Yu Chun Thomas
    Department of Computer Science and Engineering, The Chinese University of Hong Kong

    1155157719 LEUNG Kit Lun Jay
    Department of Computer Science and Engineering, The Chinese University of Hong Kong

    1155143569 MOK Owen
    Department of Mathematics, The Chinese University of Hong Kong

## High Level Context Diagram
## Feature Diagrams
### Game Flow

<img src="Pictures\gameflow_DFD.png" alt="picture" width="400"/>

> The Sign-up function is to create a new datafile for new users of the game, while the Login function is to identify who is accessing the application and provide security check for the user datafile.

> The game procedure is to integrate the gameplay from level selection and game setting for each user.

> The setting overwrite function is to manage the in-game experience for users.

> The shop function is to manage items of a user.

> The record update function is to update the game record to the database after each gameplay.

> The show record function is to display statistics of users and the whole game.

### Player Control

<img src="Pictures\DFD\PlayerControl_DFD.png" alt="picture" width="400"/>

> For the data flow of the player control, the game will receive signals from the user's input device and perform different actions. For example, the user can move Pacman and change its vision using a keyboard and mouse, also one can pause the game by pressing the "Esc" key. On moving, the game checks for any collisions to update the player state. The player state (transform, "isPowered", remaining pellets, etc.) will be saved and output to the engine.