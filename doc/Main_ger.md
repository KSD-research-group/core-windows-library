Projektname	{#mainpage}
=========================

# Überschrift 1 {#LabelUeberschrift1}

## Überschrift 2 {#LabelUeberschrift2}
 
Absatz 1

Absatz 2

\image html test.png "Bildbeschreibung"
\image latex test.png "Bildbeschreibung" width=\textwidth

- Aufzählung
- Aufzählung
- Aufzählung


Beispiel einer Tabelle:

Fehlercode | Bedeutung
---------- | ----------------------------------------------
-100       | Beschreibung
-101       | Beschreibung
-102       | Beschreibung

Nicht vergessen:


## Schnelleinführung unter Windows {#Buildprocess_Windows}

Sie benötigen folgende installierte Werkzeuge:

- z.B. Visual Studio 2010 oder 2012
- z.B. boost library 1.51 oder höher (getestet 1.51 und 1.53), http://www.boost.org/
- z.B. doxygen 1.8.3.1 oder höher (getestet 1.8.3.1), http://www.stack.nl/~dimitri/doxygen/download.html
- z.B. MiKTeX 2.9 (optional für die PDF-Erstellung der Dokumentation), http://www.miktex.org/download

Es muss eine Umgebungsvariable BOOST_DIR angelegt werden, welche als Wert den Pfad des Stammverzeichnisses der boost library besitzt, z.B. D:\boost\boost_1_53_0.

Anschließend wird die Projektmappe MakeAll.sln im Stammverzeichnis des Projektes geladen und über Batch erstellen das gesammte System erstellt.

Nach einem erfolgreichen Durchlauf befindet sich u.a. im Stammverzeichnis die Dokumentationsdatei RefMan.pdf. Im Verzeichnis application befinden sich unter den Unterverzeichnissen debug und release die Bibliotheks- und Anwendungsversionen als Debug und Release kompiliert.


## Schnelleinführung unter Linux {#Buildprocess_Linux}

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
