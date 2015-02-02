# XnaDarts
##About
This program communicates with the dart board through a virtual com-port. I'm using the FT232 usb -> serial converter and an arduino connected to the dart board matrix.

The software is written in XNA and is currently under development.

For more details on the setup, check out my [blog](http://www.martinpersson.org/wordpress/2010/08/electronic-darts-board-to-pc/)

##Building and Running
In order to build the source code you need [XNA which Microsoft provides for free](http://www.microsoft.com/en-us/download/details.aspx?id=23714).

If you just want to run a build of the program you need [XNA Redist](http://www.microsoft.com/en-us/download/details.aspx?id=20914).

By default the COM port that I use is 3 and Baud Rate is set to 9600, but these settings can be changed in the options menu.

Once the program recognizes the dart board you need to map each set of coordinates that are being transmitted to a segment in the options menu.

If the dart board is being recognized (the icon in the top right will animate if it is) but you're not able to map any segments, make sure that the messages that are being transmitted are in the format "H: X, Y". I use "H:" for the board and "B:" for the buttons, X and Y are the input pins on the Arduino. If you're having problems, it might help to look at the code in SerialManager.cs and the code that is running on the [Arduino](http://pastebin.com/4sh8wWDE).

Link to the [FTDI VCP (Virtual Com Port) Driver](http://www.ftdichip.com/Drivers/CDM/CDM%20v2.12.00%20WHQL%20Certified.exe).

##Arduino Details
The code running on the arduino is hosted on [pastebin](http://pastebin.com/4sh8wWDE)

This is what my circuit layout looks like:
http://dart.martinpersson.org/layout.jpg

I've soldered the connectors to a experimentation board, then added connectors from that board which goes to my breadboard pcb with the arduino (see photos below).

##Todo
  * Comment code
  * Marks per round/average number of marks/ppd/ppr in cricket
  * Add Master In/Out Options. With Master Out (and Master -Bull, Triples, Doubles) enabled, players have to end on a triple, double or bulls-eye.
  * Personal Statistics (online score tracking?)
  * Graphical improvements
  * Add Cricket Cut Throat when playing with three or more players
  * Ability to change the Bull split (50/50) or (25/50)
  * Ability to change the setting of allowing all players to end their round even if a player before them has won
  * Improve end of game screen (display all player statistics etc)
  * Migrate the source code to GitHub
  * Rename the project (any suggestions?)
  * Add Unit Tests

Game modes to add:
  * [Archery](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=927)
  * [Jump Up](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=930)
  * [3 In Line](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=931)
  * [Gold Hunting](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=932)
  * [Hyper Jump Up](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=933)
  * [Kunitori](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=934)
  * [Cr. Count Up](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=924)
  * [Bullshoot](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=925)
  * [Half-It](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=926)
  * [Over](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=928)
  * [Up Down Count Up](http://www.phoenixdart.com/ca/guide/view?guidecode_1=916&guidecode_2=929)

##Screenshots
http://www.martinpersson.org/wordpress/wp-content/uploads/2012/12/superdarts-580x399.jpg
http://www.martinpersson.org/dart/binding.png

##Photos
http://dart.martinpersson.org/IMGP5680.jpg
http://dart.martinpersson.org/IMGP5683.jpg
http://files.martinpersson.org/WP_000197.jpg
http://files.martinpersson.org/WP_000198.jpg
