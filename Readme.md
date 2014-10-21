# Project name

## Quick Guide for Windows

This is a tutorial on how to build the project.

The following tools have to be installed for that purpose:

* Visual Studio 2013 or 2014
  
* boost library 1.56 or higher (tested with 1.56)  
  http://www.boost.org/

* doxygen 1.8.8 or higher (tested with 1.8.8)  
  http://www.stack.nl/%7Edimitri/doxygen/download.html

* MiKTeX 2.9 (optional for building the PDF documentation)  
  http://www.miktex.org/download

An environment variable **BOOST_DIR** has to be set pointing to the root directory of the Boost library installation e.g. "D:\boost\boost_1_56_0".  
After that the project folder MakeAll.sln is loaded into the root directory of the project and the while system is built by a batch file.  
After a successful build, the root directory will contain the documentation file **RefMan.pdf**.  
The application directory will also contain the Release and Debug versions of the libraries and applications in the respective sub directory debug or release.

## Quick Guide for Linux

You'll need a complete Gnu C++ build system with Gnu make, as well as the appropriate Boost library (version 1.56 or higher)  
Change to the directory ... and start `make`.  
The application is being built.  
Temporary files are eliminated with `make clean`.

## Directory Tree

For a better understanding of the project structure the directory structure is getting explained now.

* **Readme.html**  
   This help file

* **MakeAll.sln**  
  Project solution by which the whole system and the documentation can be builed.

* **style.bat**  
  Batch file by which the source code of the measurement system can be formatted after the chair style.  
  This ueses the Artistics Style to be downloaded from http://sourceforge.net/projects/astyle/files/.

* **application**  
  Will contain the compiled components.
  
    * **debug**  
      Will contain the debug variant of the build.<br>

    * **release**  
      Will contain the release variant of the build.<br>

* **doc**  
  Contains the graphics and description text components for building the documentation with Doxygen.
  
    * **main_ger.md**  
      Contains the user manual of the system in markdown format including some Doxygen extensions.

    * **Doxyfile**  
      Doxygen configuration file

    * **make.bat**  
      Batch file starting the documentation by means of Doxygen and MiXTeX.

    * **Pictures**  
      Contains the graphics of the documentation.
    
* **include**  
  Contains all header files for the project.

* **src**  
  Contains all source files for the project.
