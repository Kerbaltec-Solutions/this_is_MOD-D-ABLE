<!--

author:   Björn Schnabel
email:    bjoern.schnabel09@gmail.com
version:  0.0.2
language: de
narrator: Deutsch Female
title: GoT README

import: https://github.com/liascript/CodeRunner
        https://raw.githubusercontent.com/liascript-templates/plantUML/master/README.md
        https://raw.githubusercontent.com/liaTemplates/ExplainGit/master/README.md

-->

[![LiaScript](https://raw.githubusercontent.com/LiaScript/LiaScript/master/badges/course.svg)](https://liascript.github.io/course/?https://github.com/Kerbaltec-Solutions/this_is_MOD-D-ABLE/blob/main/README.md)

![logo](https://github.com/Kerbaltec-Solutions/this_is_MOD-D-ABLE/assets/61379284/6656a1be-4630-4b50-94ab-39ce8dfa382c)

# This is MOD(D)ABLE

In "This is MOD(D)ABLE" the player controls their own tribe, helps it defend itself against wild animals, collect food and resources and grow to new sizes. Game entities can be flexibly added and edited by the player to adapt the game to their own playstyle. "This is MOD(D)ABLE" is played in the console by representing the map using console colors and ASCII characters, and control is done via text commands such as "fighter1:target(X,Y)". The game should be compiled locally to ensure maximum editability.

!?[Teaser](https://youtu.be/KSM9npYXOmA?si=7c-HhC6Mh-OfnPDE)

## Downloads:

- For the last stable release, please check out the "stable" branch or the last "stable" release.
- For the last release, check out the "nightly" branch or the laskst "nightly" release. The nightly versions are more up-to-date but maybe not running properly.

## Useage: 

- Download the version, you would like to use.
- normal installation mode (only available for Linux):
  - ensure, you have .NET 6.X installed
  - open the "this_is_MOD-D-ABLE" folder in your file explorer
  - for first installation:
    - right click on "setup.sh"
    - navigate to "properties/Permissions"
    - check "Allow executing file as program"
    - close the Properties menu and right click on setup.sh again
    - click "Run as a Program"
  -starting the game after installation:
    - right click on "start.sh"
    - click "Run as a Program"
- manual installation and running
  - open the console
  - Navigate inside the folder "this_is_MOD-D-ABLE"
  - ensure, you have .NET 6.0 installed
  - on first run, first use "dotnet add Package Accord" to install neccesarry Libraries
  - start the program by typing "dotnet run"
  - press ENTER to switch in and out of the command input mode.
  - in the input mode, type sys:help() to view a list of available commands.

## Vorläufiges Dateizusammenhangsdiagramm
<!--
style="width: 90%; max-width: 860px; display: block; margin-left: auto; margin-right: auto;"
-->
````ascii

+------------------------------------------------+    +---------------------+
| Hilfsdatei                                     |    | HAUPTDATEI          |
| - Funktionen zum konvertieren von Werten, etc. |    |                     |
|                                                |<---+ - Ablaufkontrolle   |
+------------------------------------------------+    | - Entity Management |
                                                      | - Input Handling    |
+------------+                                        | - Map Management    |
|            |<---------------------------------------+                     |
| Hilfsdatei |                                        |                     |
|            |                                        |                     |
+------------+                                        +---------------------+
  ^                                                         ^
  |                                                         |
  |                            +---------------+------------+
  |                            |               |
  |                            v               v
+-+----------------------------------+ +---------------+
| Entitätsdatei                      | | Entitätsdatei |
|                                    | +---------------+
| - Entitätsfunktionen               |
| - Entitätsparameter                |
| - Modifiziert/Erstellt von Dritten |
+------------------------------------+

````
