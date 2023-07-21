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

-->!

[![LiaScript](https://raw.githubusercontent.com/LiaScript/LiaScript/master/badges/course.svg)](https://liascript.github.io/course/?https://github.com/Kerbaltec-Solutions/this_is_MOD-D-ABLE/blob/master/README.md)

[logo](https://github.com/Kerbaltec-Solutions/this_is_MOD-D-ABLE/assets/61379284/6656a1be-4630-4b50-94ab-39ce8dfa382c)

# This is MOD(D)ABLE

In "This is MOD(D)ABLE" the player controls their own tribe, helps it defend itself against wild animals, collect food and resources and grow to new sizes. Game entities can be flexibly added and edited by the player to adapt the game to their own playstyle. "This is MOD(D)ABLE" is played in the console by representing the map using console colors and ASCII characters, and control is done via text commands such as "fighter1:target(X,Y)". The game should be compiled locally to ensure maximum editability.

-- Hier folgt bald ein Teaser-Video --

## Downloads:

- For the last stable release, please check out the "stable" branch or the last "stable" release.
- For the last release, check out the "nightly" branch or the laskst "nightly" release. The nightly versions are more up-to-date but maybe not running properly.

## Useage: 

- Download the version, you would like to use.
- open the console
- Navigate inside the folder "this_is_MOD-D-ABLE"
- ensure, you have .NET 6.0 installed
- on first run, first use "dotnet add Package Accord" to install neccesarry Libraries
- start the program by typing "dotnet run"
- press ENTER to switch in and out of the command input mode.
- in the input mode, type sys:help() to view a list of available commands. (not yet implemented)

## Zielsetzung

- Das Ziel ist das erstellen eines Management/Survival-Spiels ähnlich AoE in C# unter berücksichtigung der Modifizierbarkeit durch Dritte in der Zukunft.
- Die Software soll als Sourcecode verteilt werden und erst bei Ausführung lokal compiliert werden.
- Mittels System.Reflexion soll das Programm selbstständig die Funktionen der Entitäten des Spiels aufrufen können.
- Die Steuerung des Spiels erfolgt hauptsächlich über pseudo-funktionsaufrufe, die der Spieler in die Konsole eingibt.
- Um die Konsole zu öffnen und, um den gezeigten Kartenausschnitt zu ändern soll ein asyncroner Input handler mittels System.Threading geschrieben werden.
- Das Spiel soll in Takten die Eigenschaften sämmtlicher Entitäten bearbeiten und zwar über eine Funktion der Entität, die angibt, was geschehen soll.
- Die Karte, mit Ressourcen, Hindernissen und Farben soll mittels Perlin noise aus der Bibliothek Accord zufällig erzeugt werden.
- Entitäten, die auf der Karte gezeigt werden, sollen über einen einzelnen Charakter repräsentiert werden.
- Die Karte soll mittels variation der Hintergrundfarbe in der Konsole gezeichnet werden.

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

## Einschränkung im Rahmen der Abschlussarbeit

Es soll nur das Grundgerüst des Spiels sowie folgende Entitäten und Resourcen erstellt werden.

Entitäten:

- Fighter: Kann andere Entitäten umbringen, Kostet Nahrung und Geld
- Worker: Kann Gestein & Ore abbauen, Kostet Nahrung
- Wildlife (friendly): Kann von Fighter getötet werden, enthält Nahrung
- Wildlife (aggresive): Kann andere Entitäten umbringen, Kann von Fighter getötet werden
- Ore: Kann von Worker abgebaut werden, Enthält Geld
- House: Kann Worker und Fighter ausbilden, Kostet Geld

Resourcen:

- Geld: produziert aus Ore von Worker
- Nahrung: produziert aus Wildlife von Fighter
