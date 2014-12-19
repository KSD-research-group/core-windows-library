KSD Library
===========

# Einführung {#LabelEinfuehrung}

Die KSD Library dient dem vereinfachten und einheitlichen Zugriff auf verschiedene Dienste des [KSD Projektes](http://ksd.ai.ar.tum.de/).

# Benötigte Werkzeuge

This is a tutorial on how to build the project.

The following tools have to be installed for that purpose:

* Visual Studio 2013 or 2014
  
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

* **Readme.md**  
   This help file

* **MakeAll.sln**  
  Project solution by which the whole system and the documentation can be builed.

* **style.bat**  
  Batch file by which the source code of the measurement system can be formatted after the chair style.  
  This uses the Artistics Style to be downloaded from http://sourceforge.net/projects/astyle/files/.

* **application**  
  Will contain the compiled components.
  
    * **debug**  
      Will contain the debug variant of the build.

    * **release**  
      Will contain the release variant of the build.

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


# Schnelleinführung unter Windows {#Buildprocess_Windows}

Sie benötigen folgende installierte Werkzeuge:

- z.B. Visual Studio 2010 oder 2012
- z.B. boost library 1.51 oder höher (getestet 1.51 und 1.53), http://www.boost.org/
- z.B. doxygen 1.8.3.1 oder höher (getestet 1.8.3.1), http://www.stack.nl/~dimitri/doxygen/download.html
- z.B. MiKTeX 2.9 (optional für die PDF-Erstellung der Dokumentation), http://www.miktex.org/download

Es muss eine Umgebungsvariable BOOST_DIR angelegt werden, welche als Wert den Pfad des Stammverzeichnisses der boost library besitzt, z.B. D:\boost\boost_1_53_0.

Anschließend wird die Projektmappe MakeAll.sln im Stammverzeichnis des Projektes geladen und über Batch erstellen das gesammte System erstellt.

Nach einem erfolgreichen Durchlauf befindet sich u.a. im Stammverzeichnis die Dokumentationsdatei RefMan.pdf. Im Verzeichnis application befinden sich unter den Unterverzeichnissen debug und release die Bibliotheks- und Anwendungsversionen als Debug und Release kompiliert.


# Schnelleinführung unter Linux {#Buildprocess_Linux}

Sie benötigen ein eingerichtetes Buildsystem mit g++ und maketools, sowie installierter boost library. Wechseln Sie in das Unterverzeichnis ... und starten Sie make. Es wird die Applikation ... erzeugt. Zwischendateien können Sie mit make clean beseitigen.


# Verzeichnisübersicht {#Directories}
Zum Verständnis des Projektaufbaus wird die Verzeichnisstruktur erläutert.
- __Readme.html__:

  Diese Hilfsdatei

- __MakeAll.sln__:

  Projektmappe, mit der das Gesamtsystem und die Dokumentation in einem Batch-Durchgang erstellt werden kann.

- __style.bat__:

  Batchdatei, mit der der gesammte Quellcode des Projektes nach den Konventionen des Lehrstuhls formatiert wird. Setzt installiertes Artistic Style (http://sourceforge.net/projects/astyle/files/) voraus.

- __application__:

  Enthält nach dem Makeprozess die kompilierten Komponenten des Projektes.

  + __debug__:

    Enthält nach erfolreichen Buildprozess die Debugvariante des Systems.

  + __release__:

    Enthält nach erfolreichen Buildprozess die Releasevariante des Systems.

- __doc__:

  Enthält Komponenten wie Grafiken und Beschreibungstexte zur Erstellung der Dokumentation des Systems mit Doxygen

  + __Main_ger.md__:

    Enthält die Nutzerdokumentation des Systems im Markdown-Format mit Doxygen-Erweiterungen in Deutsch

  + __Main_en.md__:

    Enthält die Nutzerdokumentation des Systems im Markdown-Format mit Doxygen-Erweiterungen in Englisch

  + __Doxyfile__:

    Doxygen-Konfigurationsfile

  + __make.bat__:

    Batchdatei, welche die Dokumentationserstellung mit Doxygen und MiXTeX startet.

  + __pictures__:

    Enthält alle Grafiken der Dokumentation.

- __include__:

  Enthält alle Headerdateien des Projektes.

- __src__:

  Enthält alle Sourcedateien des Projektes..


# Fehlertests {#Tests}

Beschreibung von Unittests etc.
