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

## UI DFD

### UI Rendering

The UI Manager handles input and output of the UI. For example, during game, a 'game end' event can be invoked when a player choose to quit the game after pausing or a ghost killed the pacman. For the first case, a button UI element calls UI Manager to end a game; for the second case, an other system calls UI Manager. After that, the UI Manager will choose to show and render the UI panel by calling the SwitchPanel() function.

UI panel list stores a list of displyable panels where a panel contains a list of UI elements. The panel list may contain panels such as title main screen panel, shop panel and game pause panel. A UI panel may contains elements such as button, text and slider.

### Title screen UI

As mentoned before, UI Manager handles input and rendering of UI. The UI Manager may call different functions according to the inputs. Change game setting function change the player game setting data such as music volume and graphic setting. It will update the Game Setting List which stores all player setting on the database. 

Our Pacman has a shop system where player can buy skin or other game items. The game item list stores all game item while user profile list stores all user profile. A user profile contains the player's owned game items and score.

A record list stores all players highest score. the world record will be displayed on the title screen for players to compare.

Everytime when the game is started, it will fetch the player game setting and the player profile for the game rendering.

### In-game UI

During game, a game event, such as game pause and game end, can be called by different systems, like UI Manager and Player Controller. For instance, a game end event can be invoked by the UI Manager when a player clicked the end button in pause panel or by the Player Controller when a player health is below 0. UI Manager handles all the game events in game and ...

### Player Control

For the data flow of the player control, the game will receive signals from the user's input device and perform different actions. For example, the user can move Pacman and change its vision using a keyboard and mouse, also one can pause the game by pressing the "Esc" key. On moving, the game checks for any collisions to update the player's state, such as collisions with ghosts will decrease lives, collisions with power pellets will enter the powered state, and collisions with walls refuse the player to move along the direction. The player state (transform, "isPowered", remaining pellets, etc.) will be saved and output to the engine.