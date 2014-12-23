KSD Library
===========

# Einf�hrung {#LabelEinfuehrung}

Die KSD Library dient dem vereinfachten und einheitlichen Zugriff auf verschiedene Dienste des [KSD Projektes](http://ksd.ai.ar.tum.de/).
Im Moment sind nur Zugriffe auf das [MediatTUM](https://mediatum.ub.tum.de/)-System implementiert, weitere folgen in K�rze.

# Building {#Build}

Sie ben�tigen folgende Werkzeuge:

- Visual Studio 2013 oder h�her (getestet 2013 Ultimate)
- doxygen 1.8.8 oder h�her (getestet 1.8.8), http://www.stack.nl/~dimitri/doxygen/download.html
- MiKTeX 2.9 (optional f�r die PDF-Erstellung der Dokumentation), http://www.miktex.org/download

Anschlie�end wird die Projektmappe KsdLibrary.sln im Stammverzeichnis des Projektes geladen und �ber Batch erstellen das gesamte System erstellt.

Nach einem erfolgreichen Durchlauf befindet sich u.a. im Stammverzeichnis die Dokumentationsdatei RefMan.pdf. Im Verzeichnis Library befinden sich unter den Unterverzeichnissen Debug und Release die Bibliotheksversionen als Debug und Release kompiliert.

# Verzeichnis�bersicht {#Directories}
Zum Verst�ndnis des Projektaufbaus wird die Verzeichnisstruktur erl�utert.
- __Readme.html__:

  Diese Hilfsdatei

- __KsdLibrary.sln__:

  Projektmappe, mit der das Gesamtsystem und die Dokumentation in einem Batch-Durchgang erstellt werden kann.

- __style.bat__:

  Batchdatei, mit der der gesammte Quellcode des Projektes nach den Konventionen des Lehrstuhls formatiert wird. Setzt installiertes Artistic Style (http://sourceforge.net/projects/astyle/files/) voraus.

- __Library__:

  Enth�lt nach dem Makeprozess die kompilierte KSD Library.

  + __debug__:

    Enth�lt nach erfolgreichen Buildprozess die Debugvariante der KSD Library.

  + __release__:

    Enth�lt nach erfolgreichen Buildprozess die Releasevariante der KSD Library.

- __doc__:

  Enth�lt Komponenten wie Grafiken und Beschreibungstexte zur Erstellung der Dokumentation des Systems mit Doxygen

  + __Main_ger.md__:

    Enth�lt die Nutzerdokumentation des Systems im Markdown-Format mit Doxygen-Erweiterungen in Deutsch

  + __Main_en.md__:

    Enth�lt die Nutzerdokumentation des Systems im Markdown-Format mit Doxygen-Erweiterungen in Englisch

  + __Doxyfile__:

    Doxygen-Konfigurationsfile

  + __make.bat__:

    Batchdatei, welche die Dokumentationserstellung mit Doxygen und MiXTeX startet.

  + __pictures__:

    Enth�lt alle Grafiken der Dokumentation.

- __Ksd__:

  Enth�lt die Sourcen der KSD Library.

- __UnitTestKsd__:

  Enth�lt die Sourcen der Unit Tests.
