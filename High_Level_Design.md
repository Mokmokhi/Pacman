# High Level Design Document

Project Title: Become Pacman (Fake)

Document version: 0

Printing date: 26 Jan 2023

Group ID:

Group Member

    1155127434 HO Chun Lung Terrance
    Department of Philosophy, The Chinese University of Hong Kong

    <SID> <Full name> woo pok
    Department of Physics, The Chinese University of Hong Kong

    1155157839 NG Yu Chun Thomas
    Department of Computer Science and Engineering, The Chinese University of Hong Kong

    1155157719 Leung Kit Lun Jay
    Department of Computer Science and Engineering, The Chinese University of Hong Kong

    1155143569 Mok Owen
    Department of Mathematics, The Chinese University of Hong Kong

Table of content
- [High Level Design Document](#high-level-design-document)
  - [Introduction](#introduction)
    - [Project Overview](#project-overview)
    - [System Feature](#system-feature)
  - [System Architecture](#system-architecture)
    - [Technologies](#technologies)
    - [Architecture Diagram](#architecture-diagram)
      - [**Game Flow**](#game-flow)
      - [**Basic Game Loop**](#basic-game-loop)
      - [**UML Diagram of Singleton Class**](#uml-diagram-of-singleton-class)
      - [**Global Event Bus System**](#global-event-bus-system)
      - [**Global Data Flow**](#global-data-flow)
      - [**Character States**](#character-states)
      - [**Ghost States**](#ghost-states)
      - [**Props Class**](#props-class)
    - [System Components](#system-components)
      - [**Game Flow**](#game-flow-1)
      - [**Basic Game Loop**](#basic-game-loop-1)
      - [**UML Diagram of Singleton Class**](#uml-diagram-of-singleton-class-1)
      - [**Global Event Bus System**](#global-event-bus-system-1)
      - [**Global Data Flow**](#global-data-flow-1)
      - [**Character States**](#character-states-1)
      - [**Ghost States**](#ghost-states-1)
      - [**Props Class**](#props-class-1)
    - [Note (Excluded from main document)](#note-excluded-from-main-document)

---

## Introduction

### Project Overview

> This project is to create a Pacman game through Unity game engine. The infamous Pacman game is one side an arcade game to escape from the chasing of ghosts, but one side has the potential to become a dungeon-like horror game, such as dark deception. We aim at producing a brand new Pacman game inheriting the idea of traditional Pacman, but enhancing the experience by introducing horror features. The project will be built under the guidance of software development, and be evaluated by aspects of software engineering taught in the course CSCI3100 Software Engineering.

### System Feature

> The system features:  
> 1. 3-D constructions: Apart from traditional Pacman games, this project will introduce a 3-D maze with 3-D objects for advanced experience of Pacman. 
> 2. Props: Pacman will interact with props inside the maze. When Pacman eats special props, there will be buffs or debuffs according to the function of that prop. Usual beans will be placed as main props.
> 3. Third-person camera: The sight of player will no longer be a god view, but focusing on the surrounding of the Pacman. Players will eventually experience the feeling of being a Pacman.
> 4. Minimap: Similar to many RPG (Role-playing Games), a minimap will be shown on the screen for player to know where he is as player's sight is no longer a bird-eye view. Some information of the maze will be shown on the minimap, for enhancing player's experience.
> 5. Timer: A timer will be placed on top of the screen. The time will be used to calculate the score of the player and saved as a record.
> 6. Teleport: With different levels of the Pacman game, the length of the gameplay will be increased by inserting teleports. The teleports may bring players to a specific point, or act as a bridge between mazes.
> 7. Fog of war: As difficulties diverges, player may see only the sight of the Pacman, but no longer its surrounding. In particular, under the situation of fog, player has no ability to see what's behind the Pacman if he isn't facing in that direction.
> 8. Artificial intelligence of the ghost: Ghast might move in random direction so that player could easily escape from their chase. However, once the difficulty is being raised, the ghast may have the ability to trace player's movement. They could also reflect from histories to attack the frequently appear spot of the player, so no exact solution could be given to win the game easily.
> 9. Store system: As if most of the arcade games, player may buy characters or any items from store to enhance their in-game experience.

---

## System Architecture

### Technologies

> This project will be produced using the following technologies:
> - Game engine: Unity (Version 2021.3.17f1)
> - Programming language: C#
> - Documentation: Markdown, Microsoft Word
> - Database: Firebase

### Architecture Diagram

#### **Game Flow**  

<img src="Pictures\Gameflow.png" alt="picture" width="400"/>

#### **Basic Game Loop**  

<img src="Pictures\GameProcess.png" alt="picture" width="400"/>

#### **UML Diagram of Singleton Class**

<img src="Pictures\UML-Class-Singleton.png" alt="picture" width="400"/>  

#### **Global Event Bus System**

<img src="Pictures\Event-Bus-System.png" alt="picture" width="400"/>  

#### **Global Data Flow**
> \<need further discussion>

#### **Character States**  
> \<to be drew by Thomas>  

#### **Ghost States**

<img src="Pictures\Ghost-State.png" alt="picture" width="400"/> 

#### **Props Class**  

<img src="Pictures\Props-Class.png" alt="picture" width="400"/>  

> \<to be drew by Thomas>  


### System Components

#### **Game Flow**  
> By executing the application, users will be directed to a **Login/Sign-up screen**. Users are free to leave the application, sign-up an account or login to the **title screen page**. In the title screen, users could perform the following action:

> - Accessing the **setting**: Users could here customize the environment of the gaming, or other attributes provided.
> - Accessing the **shop**: Users could here buy characters or items provided.
> - Retrieving **records**: All gaming histories are recorded with time and score in this page.
> - Accessing **level selection**: Users could start the game by choosing a suitable level here.
> - **Leaving the application**: Close the window and leave.

> During the level selection procedure, users could still turn back to the title screen for other possible actions. Once level is selected and confirmed by users, the **game process** will be started. From here, users are not allowed to leave or they should **pause the game** to return to the title page, or until the game process is ended.

> The game can be ended in two situations, either the user **won the game** or **lose the game**. If users won the game, the current game process will be over and users will be directed to the title screen; if users lose the game, there will be a procedure asking for **restarting the game process**. By choosing restart, users could return to the game process for another trial, otherwise users will be directed to title screen and wait for other actions.

#### **Basic Game Loop**
> When the **game process** is started, the application will begin to read user input. If *esc* button is clicked, then the game will enter the **pause** section, and it is possible to resume by clicking *esc* once again. By reading user input, there will be an *update function* scripted to **update character attributes**. The function will check whether the character collides with ghosts. If there is a **collision with ghost**, then **the lives will be decreased by 1**. Once the **number of lives goes 0**, the game ends and player is announced to be **losing the game**. If there is more than 0 lives, the play continues. After checking collision with ghost, it **checks the dots remaining** in the maze. If there is no dots left and the character still alive, then player is claimed to be winning the game; otherwise the game continues until there is a determination on player's winning state.

#### **UML Diagram of Singleton Class**
> A singleton pattern guarantee the class will **only have one instance**. It is useful to manage a system that needs to be **globally accessible**. Although a singleton pattern is not good for unit testing and could create dependency between singleton objects, our game, Pacman, is relatively small in scale and we will also adopt other design pattern to avoid strong coupling.

> Throughout the whole system, a **singleton** class is derived from **MonoBehaviour**, with a *private static instance* and *public static instance* of type T. 3 Manager classes are then inherited from the singleton class. In our projects, all classes with a suffix of "Manager" in their name inherit from Singleton class. The following are a few examples.

> A **UIManager** class will be derived with *private* objects *_canvas* for UI creation, *_mapCamera* for player vision and *_panels* a queue of type T for detecting different maps, as well as *public* function *SwitchPanel()* for changing panels; 

> A **GameManager** class will be derived with a *private* object *_timeElapse* as timer and a *public* object *score* for recording game score, as well as *public* functions *GameStart()* for calling game process, *GamePause()* for pausing the game process, *GameResume()* for resuming from pause action, *GameLose()* for prompting lose effect, *GameNext()* for calling next game and *GameDone()* for ending a game.

> An **AudioManager** class will be derived with public object *sound[]* as a list of sound effects and a *public* function *Play(String)* for calling sound effects.

#### **Global Event Bus System**

> In order to manage global event efficienly and avoid strong coupling between objects, a event bus system is used. When an event is raised by a publisher, it sends out a signal to its subscribers. In the event system, **only subscribers listening to the specific event that published by a publisher will be notified and choose how to handle it.** In our design, the global events that allows to subscribe are different game states, such as Login, Pause, Win, etc.

> For instance, when the UIManager is publish an Pause event, the GameManager will be notified if he is subscibed to Pause event. After that, the GameManager can choose what to do under his classes. Neither the publisher nor other subscriber know what will GameManager do next.

#### **Global Data Flow**

#### **Character States**  
> \<to be written>  

#### **Ghost States**
> Every ghost has basically **6 states**. When a level is loading and not started yet, it will be at *idle* state. When game starts, a ghost will decide where to go and it is at *decide turn* state. After that, it turns to *move* state and keep moving until 1. a corner is met 2. the player uses a props. In case 1, it goes back to *decide turn*. In case 2, it turns into *terrified* state. When the ghost is terrified, it will either be engulfed (*engulf* state) or go back to *move* state, which is determined by whether a player "eat" the ghost. If the ghost is ate, it turns into *die* state and will head back to "home", which is a ghost box directly. After that, it will turn into *Locked* state and wait unitl it is released and turn to *decide turn* state.

#### **Props Class**  
> A **PropsManager** class will be derived from the singleton class with *private* objects *PropsList* for carrying a list of Props type objects, and *SpawnedPropsList* for carrying a list of existing props in the scene. There will be functions for controlling props, including *Instantiate(Props)* for getting the instance of corresponding props, and *Destroy(props)* for destroying any props.\<to be append>  

> An *abstract* **PropsBase** class will be inherited from *ScriptableObject* which is able to be controlled by scripts. Each prop created under the specification of PropsBase will consist of *public* object *name*, *propsType* and *propsState*. propsType is of *PropsType* class, an *enum* class with 4 values: *SpeedUp*, *SpeedDown*, *Engulf* and *ScareGhost*; propsState is of *PropsState* class, an *enum* class with 4 values: *Unspawned*, *Spawned*, *Using* and *Used*. The PropsBase consists of a function *Use()* for changing PropsState from null to Using or Used, depends on object characteristics.\<to be append> 

---

### Note (Excluded from main document)

- similar to dark deception

1. 3D
2. multiple props
3. third-personal view, follow camera
4. a minimap is showed on the top-right corner (show ghosts for 2 seconds (skill or props))
5. Timer
6. different (physical) levels ~teleport
7. 戰爭迷霧 (limited vision) (difficulites)
8. Advanced Ghost movements (record player past movements) instead of random
9. Buy Character
