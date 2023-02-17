# CrestronLogParser
C# program that reads in a Crestron Log file and filters the results.

## Synopsis

This program will read a Crestron log file and display it, you will also have the option to hide the timestamp and other unwanted info.

## Motivation

This was originally created to filter a log file to only show user generated entries created by a SIMPL program using "User event logger" symbols.

## Installation

No installation required.

## Usage

Download the log files from a Crestron processor using Toolbox or the Information Tool.
Open the *.log file using this program.
An example log file you can use to parse is in the root folder "Crestron_00.log"
To only see user generated entries then type "USER SPECIFIED" (without the double quotes) in the filter section and press enter.

Click on the check boxes to filter out other items.
## API Reference

The code is the current documentation, feel free to create some and submit a pull request. 

## Tests

Tests are not implemented, feel free to make some.

## Contributors

Rod Driscoll: rodney.driscoll@thecigroup.com.au

## License

MIT License.
