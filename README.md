# Conway's Game of Life

## Introduction

Conway's Game of Life is a very well-known simple computer algorithm that simulates cells growing and dying in a medium. Like many good things, it originated in Cambridge around 1970. Yes, I am that old. It is described at https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life .

This repository contains a C#.Net implementation of Conway's Game of Life.

The initial implementation, which will run the simulation and make a very basic display of its progress in a console window, is hopefully not bad code, but there are many ways in which it could be improved.

## Development Setup for a New Computer

These are the high-level steps necessary to get Conway's Game of Life building and running on your new computer. These are not detailed step-by-step instructions, so it is expected that you will need assistance with some of the steps. But don't forget - you are a team and should help each other as much as possible.

* Install Microsoft .Net 6 SDK for Windows x64. Be sure you find the SDK, not just the runtime.
* Install Git for Windows. The default options should be OK.
* Install Microsoft Visual Studio Code for Windows x64.
* In Visual Studio Code, install the following extensions
  * C#
  * Git Graph
  * GitHub Pull Requests
* Create a folder "git" under C:\users\yourname
* Log in to github.com. If you do not have a GitHub account, create one - it's free.
* Navigate to https://github.com/peterchase/conway.git
* Make a fork of the above `conway.git` in your GitHub
* Set up the GitHub extension, connecting it to your GitHub
* Use the GitHub extension to check out your fork of `conway.git`
* Hopefully, you now have: -
  * A buildable, runnable Conway's Game of Life
  * A list of potential issues to work on, in the GitHub extension issues list