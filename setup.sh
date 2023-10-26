#!/bin/bash

echo "for setup, please enter your sudo passwort:"
sudo -s
echo "making startfile executable"
sudo chmod u+r start.sh
echo "adding Accord library"
sudo dotnet add package Accord
echo "install xterm for terminal resizing"
sudo apt-get install xterm
echo "cleaning up"
sudo apt autoremove
./start.sh