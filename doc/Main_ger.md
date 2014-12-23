KSD Library	{#mainpage}
=========================

[TOC]

# Einf�hrung {#LabelEinfuehrung}

Die KSD Library dient dem vereinfachten und einheitlichen Zugriff auf verschiedene Dienste des [KSD Projektes](http://ksd.ai.ar.tum.de/).
Im Moment sind nur Zugriffe auf das [MediatTUM](https://mediatum.ub.tum.de/)-System implementiert, weitere folgen in K�rze.

F�r jeden Dienst wird ein eigener Namensraum genutzt, beispielsweise #Ksd.Mediatum.

# Anbindung an MediaTUM {#LabelMediaTum}

## Einf�hrung {#LabelMediaTumEinfuehrung}

MediaTUM ist eine in sich geschlossenes Web Server Architektur f�r die Archivierung und das Abrufen von Bildern, Dokumenten und Videodateien. 
Mehr Infos siehe unter http://sourceforge.net/p/mediatum/wiki/Home/ und http://wiki.ub.tum.de/mediatum_dev/index.php5/Main_Page.
Eine Kurzpr�sentation findet sich unter http://mediatum.ub.tum.de/doc/1109402/1109402.pdf.
Eine Anleitung zur Installation findet sich unter http://mediatum.sourceforge.net/documentation/installation-instructions/.
 
F�r die Unterst�tzung von Clients bietet MediaTUM verschiedene REST-Schnittstellen an.
Wesentliche Schnittstellen werden von dieser Bibliothek unterst�tzt und nach OOP modelliert.

## Zugriffsautorisierung {#LabelMediaTumZugriffsautorisierung}

Die Zugriffsautorisierung erfolgt mittels [OAuth](http://oauth.net/core/1.0a/#signing_process), siehe dazu https://gitlab.ai.ar.tum.de/ksd-research-group/ksd-documentation/wikis/mediaTumBasicAuthentication.
Bei der [Live Instanz der TU M�nchen](https://mediatum.ub.tum.de/) kann ein Profil mittels https://mediatum.ub.tum.de/oauth generiert werden.
\image html MediaTUM\NewPresharedTag.png "Profil unter Live Instanz der TU M�nchen anlegen"
\image latex MediaTUM\NewPresharedTag.png "Profil unter Live Instanz der TU M�nchen anlegen"
\image rtf MediaTUM\NewPresharedTag.png "Profil unter Live Instanz der TU M�nchen anlegen"
Es wird ein entsprechender Preshared Tag erzeugt (siehe auch #Ksd.Mediatum.OAuth.PresharedTag).

Die Klasse #Ksd.Mediatum.OAuth unterst�tzt die Generierung signierter Zugriffe auf MediaTUM. Es wird dazu eine Instanz generiert. Es stehen dazu zwei Konstruktoren zur Verf�gung. 
#Ksd.Mediatum.OAuth.OAuth(String, String) wird unter Angabe des User Names und des Preshared Tag generiert. #Ksd.Mediatum.OAuth.OAuth() liest dagegen diese Werte aus der Registry.
Die Pl�tze in der Registry sind dabei die Keys `UserName` und `PresharedTag` unter dem Pfad `HKEY_CURRENT_USER\SOFTWARE\Mediatum`.

Das Wertepaar #Ksd.Mediatum.OAuth.UserName und #Ksd.Mediatum.OAuth.PresharedTag kann mittels der Methoden #Ksd.Mediatum.OAuth.ReadSigningConfigFromRegistry und #Ksd.Mediatum.OAuth.WriteSigningConfigToRegistry von der Registry gelesen bzw.
in diese geschrieben werden. Die Registry Werte k�nnen auch separat mittels der statischen Eigenschaften #Ksd.Mediatum.OAuth.UserNameInRegistry und #Ksd.Mediatum.OAuth.PresharedTagInRegistry ausgelesen und geschrieben werden.

Der Code
~~~~~~~~~~~~~~~{.cs}
Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth();
~~~~~~~~~~~~~~~
ist daher identisch zu
~~~~~~~~~~~~~~~{.cs}
Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth(Ksd.Mediatum.OAuth.UserNameInRegistry, Ksd.Mediatum.OAuth.PresharedTagInRegistry);
~~~~~~~~~~~~~~~

Die erzeugte Instanz wird anschlie�end zur Signierung aller Dienste mit Zugriffsautorisierung genutzt. Die Methode #Ksd.Mediatum.OAuth.GetSignedUri generiert dabei aus einer unsignierten URI eine signierte URI f�r einen Zugriff.
Die Methode #Ksd.Mediatum.OAuth.AddSignParams hingegen ist f�r die Zusammenarbeit mit .Net Methoden gedacht, welche die Parameter f�r einen Webzugriff separat als NameValueCollection erwarten. 
In diesem Fall werden der Parameterkollektion die Parameter user und sign hinzugef�gt. ACHTUNG: die Signierung von MediaTUM arbeitet zum Stand der Dokumentation nur teilweise korrekt. Daher werden als work arround zun�chst
viele REST-Schnittstellen unsigniert genutzt.

## Kommunikation mit dem MediaTUM-Server {#LabelMediaTumKommunikation}

Die Kommunikation zu MediaTUM dient die Klasse #Ksd.Mediatum.Server. Der Konstruktor verlangt ein #Ksd.Mediatum.OAuth Objekt f�r die Signierungen der Webaufrufe und die Angabe der Server URI. 
Die statische Eigenschaft #Ksd.Mediatum.Server.ServerNameInRegistry kann dabei genutzt werden, um die URI des Servers aus der Registry zu lesen oder in diese zu schreiben. 
Der Platz in der Registry ist dabei f�r die URI der Key `URI` unter dem Pfad `HKEY_CURRENT_USER\SOFTWARE\Mediatum`.

~~~~~~~~~~~~~~~{.cs}
Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth();
Ksd.Mediatum.Server server = new Ksd.Mediatum.Server(oAuth, Ksd.Mediatum.Server.ServerNameInRegistry);
~~~~~~~~~~~~~~~

Das erzeugte Serverobjekt �bernimmt nun die Low Level Kommunikation mit MediaTUM �ber dessen REST-Schnittstellen. Diese werden zur Vereinfachung weiter abstrahiert, wie in den folgenden Kapiteln erl�utert.
Die wesentlichen REST-Schnittstellen sind:

Schnittstelle                    | Methoden | Eigenschaft                             | Zugriff (derzeit)
---------------------------------| ---------|-----------------------------------------|------------------
services/upload/calcsign         | GET      | #Ksd.Mediatum.Server.SignPath           | unsigniert      
services/upload/new              | POST     | #Ksd.Mediatum.Server.UploadPath         | unsigniert
services/upload/update           | POST     | #Ksd.Mediatum.Server.UpdatePath         | unsigniert
services/export                  | GET      | #Ksd.Mediatum.Server.ExportPath         | signiert
services/metadata/scheme         | GET      | #Ksd.Mediatum.Server.MetadataPath       | signiert
services/metadata/appdefinitions | GET      | #Ksd.Mediatum.Server.AppDefinitionsPath | signiert
file                             | GET      | #Ksd.Mediatum.Server.FilePath           | unsigniert

In der [Live Instanz der TU M�nchen](https://mediatum.ub.tum.de/) von MediaTUM finden sich die Beschreibungen der REST-Schnittstellen unter:

Schnittstelle                    | Eigenschaft
---------------------------------| --------------------------------------------
services/upload                  | https://mediatum.ub.tum.de/services/upload
services/export                  | https://mediatum.ub.tum.de/services/export
services/metadata                | https://mediatum.ub.tum.de/services/metadata

## Nodes {#LabelMediaTumNodes}

### Einf�hrung {#LabelMediaTumNodesEinfuehrung}

Die folgende Abbildung zeigt einen Screenshot aus der Nutzersicht von MediaTUM:
\image html MediaTUM\UserViewNodes.png "Screenshot aus der Nutzersicht von MediaTUM"
\image latex MediaTUM\UserViewNodes.png "Screenshot aus der Nutzersicht von MediaTUM"
\image rtf MediaTUM\UserViewNodes.png "Screenshot aus der Nutzersicht von MediaTUM"
 
MediaTUM arbeitet intern mittels Nodes, welche zu Baumstrukturen zusammengesetzt sind. Wenn der Cursor des Browsers �ber einem Element des Navigationsbaums rechts oder einen Element stehen bleibt, so ist unten links im Browser
in der dort angezeigten URI auch die Node-ID lesbar.

\image html MediaTUM\Node_01.png "Erste Stufe"
\image latex MediaTUM\Node_01.png "Erste Stufe"
\image rtf MediaTUM\Node_01.png "Erste Stufe"

Node ID 604993

\image html MediaTUM\Node_02.png "Zweite Stufe"
\image latex MediaTUM\Node_02.png "Zweite Stufe"
\image rtf MediaTUM\Node_02.png "Zweite Stufe"

Node ID 1085713

\image html MediaTUM\Node_03.png "Dritte Stufe"
\image latex MediaTUM\Node_03.png "Dritte Stufe"
\image rtf MediaTUM\Node_03.png "Dritte Stufe"

Node ID 1085784

\image html MediaTUM\Node_04.png "Vierte Stufe"
\image latex MediaTUM\Node_04.png "Vierte Stufe"
\image rtf MediaTUM\Node_04.png "Vierte Stufe"

Node ID 1085785

\image html MediaTUM\Node_05.png "Element in vierter Stufe"
\image latex MediaTUM\Node_05.png "Element in vierter Stufe"
\image rtf MediaTUM\Node_05.png "Element in vierter Stufe"
 
Node ID 1187773

\image html MediaTUM\Node_06.png "Unterelement"
\image latex MediaTUM\Node_06.png "Unterelement"
\image rtf MediaTUM\Node_06.png "Unterelement"

Node ID 1187772

Die Nodes haben verschiedene Typen und Zugriffsrechte. Jeder Node kann Parents und Childs besitzen. Weiter besitzt ein Node verschiedene Attribute. 

### Basisklasse Node {#LabelMediaTumNodesClassNode}

Zur Vereinfachung der Handhabung von Nodes dient die Klasse
#Ksd.Mediatum.Node. Die grundlegenden Eigenschaften jedes Nodes sind:

Eigenschaft                        | Bedeutung
-----------------------------------| ----------------------------------------------------------------------------------------------------------
#Ksd.Mediatum.Node.Server          | Das Server-Objekt, mit dem der Node verbunden ist (also von diesem geladen und �ber diesen angelegt wurde)
#Ksd.Mediatum.Node.ID              | Die ID des Nodes
#Ksd.Mediatum.Node.Name            | Der Name des Nodes
#Ksd.Mediatum.Node.Type            | Der Typ des Nodes
#Ksd.Mediatum.Node.Parents         | Alle Parents des Nodes
#Ksd.Mediatum.Node.Children        | Alle Children des Nodes
#Ksd.Mediatum.Node.Read            | Wer hat Leserechte auf dem Node
#Ksd.Mediatum.Node.Write           | Wer hat Schreibrechte auf dem Node
#Ksd.Mediatum.Node.Creator         | Nutzer, welcher den Node erzeugt hat
#Ksd.Mediatum.Node.CreationTime    | Zeitpunkt, an welchem der Node erzeugt wurde
#Ksd.Mediatum.Node.UpdateUser      | Nutzer, welcher zuletzt den Node ver�ndert hat
#Ksd.Mediatum.Node.UpdateTime      | Zeitpunkt, an welchem der Node zuletzt ver�ndert wurde
#Ksd.Mediatum.Node.Files           | Alle Dateien, welche an den Node geh�ngt wurden

F�r den Abruf aller an einen Node geh�ngten Dateien dient die Klasse #Ksd.Mediatum.NodeFile.

Eigenschaft                        | Bedeutung
-----------------------------------| ----------------------------------------------------------------------------------------------------------
#Ksd.Mediatum.NodeFile.Type        | Der Typ der Datei
#Ksd.Mediatum.NodeFile.MimeType    | Der Mime Typ der Datei
#Ksd.Mediatum.NodeFile.Filename    | Der Dateiname der Datei auf dem Server selbst
#Ksd.Mediatum.NodeFile.Parent      | Der Node, an dem die Datei h�ngt

Um die Datei zu downloaden, dient die Methode #Ksd.Mediatum.NodeFile.Download().

Wie bereits beschrieben besitzt jeder Node verschiedene Attribute. F�r ihre Verwaltung dient die Klasse #Ksd.Mediatum.NodeAttribute. Sie besitzt die Eigenschaften #Ksd.Mediatum.NodeAttribute.Modifyed und 
#Ksd.Mediatum.NodeAttribute.Value. #Ksd.Mediatum.NodeAttribute.Value enth�lt den Wert des Attributes als String. #Ksd.Mediatum.NodeAttribute.Modifyed gibt an, ob der Wert des Attributes lokal ver�ndert und noch
nicht zum Server �bertragen wurde. Es k�nnen so mehrere Attribute erst ver�ndert und dann �ber einen Aufruf von #Ksd.Mediatum.Node.Update() zusammen zum Server �bertragen werden.

Die Eigenschaft #Ksd.Mediatum.Node.Attributes enth�lt alle Attribute eines Nodes als Name-Wert-Paar. Attribute k�nnen mittels der Methode #Ksd.Mediatum.Node.GetAttributeValue(string) abgefragt und mittels 
#Ksd.Mediatum.Node.SetAttributeValue(string, string) gesetzt werden.

Bestimmte Node Typen besitzen immer bestimmte Attribute. F�r diese werden spezielle Node-Klassen geschrieben, welche #Ksd.Mediatum.Node als Basisklasse nutzen. Im Folgenden wird die Klasse #Ksd.Mediatum.FloorPlanNode
gezeigt:

\includelineno FloorPlanNode.cs

Die meisten Eigenschaften und Methoden sind selbsterkl�rend. Bei den Eigenschaften wird auf die bereits beschriebenen Methoden #Ksd.Mediatum.Node.GetAttributeValue(string) und 
#Ksd.Mediatum.Node.SetAttributeValue(string, string) zur�ckgegriffen. Wesentlich ist noch der Konstruktor #Ksd.Mediatum.FloorPlanNode.FloorPlanNode. Nach diesem Schema wird der Konstruktor jeder neuen Node-Klasse erstellt.
Die statische Methode #Ksd.Mediatum.FloorPlanNode.CreateFloorPlanNode dient der Erzeugung eines neuen Nodes vom Typ FloorPlanNode.

Nach dem Schreiben einer neuen Node-Klasse muss diese bei dem Serverobjekt angemeldet werden:
~~~~~~~~~~~~~~~{.cs}
server.TypeTable.Add("image/project-arc", typeof(Ksd.Mediatum.FloorPlanNode));
~~~~~~~~~~~~~~~
Dadurch wei� das Serverobjekt beim Abfragen eines Nodes vom Server anhand des Typs (#Ksd.Mediatum.Node.Type), welche Nodeklasse f�r dessen Repr�sentation zu instanziieren ist.

Im Folgenden ist ein zusammenh�ngender Beispielcode f�r die Abfrage eines speziellen Nodes zu sehen:
~~~~~~~~~~~~~~~{.cs}
Ksd.Mediatum.OAuth oAuth = new Ksd.Mediatum.OAuth();
Ksd.Mediatum.Server server = new Ksd.Mediatum.Server(oAuth, Ksd.Mediatum.Server.ServerNameInRegistry);
server.TypeTable.Add("image/project-arc", typeof(Ksd.Mediatum.FloorPlanNode));

Ksd.Mediatum.FloorPlanNode floorNode = (Ksd.Mediatum.FloorPlanNode)server.GetNode(1229104);
~~~~~~~~~~~~~~~
1229104 steht dabei f�r die entsprechende Node-ID.

Das folgende Beispiel zeigt hingegen, wie ein spezielles Nodeobjekt auf Clientseite angelegt und ge�ndert werden kann:
~~~~~~~~~~~~~~~{.cs}
Ksd.Mediatum.FloorPlanNode loadedNode = (Ksd.Mediatum.FloorPlanNode)Ksd.Mediatum.FloorPlanNode.CreateFloorPlanNode(node, "Upload Test", "Christoph Langenhan", "Germany", new Uri("http://reference.ksd.ai.ar.tum.de:8080/AgraphMLDownloadService/DownloadAgraphml?ifcid=1IEeEPbCv1je0WG2vzMPyH&neo4jurl=http://localhost:7474&floorlevel=0.0"), binaryData);

loadedNode.Architect = "Langenhan Christoph";
loadedNode.Country = "Germany";
loadedNode.SetAttributeValue("link", "http://heise.de");
loadedNode.Update();
~~~~~~~~~~~~~~~
Das Feld binaryData enth�lt dabei den Dateiinhalt als Bin�rstring.
 
### Internas {#LabelMediaTumNodesInternal}

Folgende Ausf�hrungen zeigen interne Mechanismen, welche in der Regel nicht verstanden werden m�ssen. Der Inhalt von Nodes etc. wird �ber die REST-Schnittstellen per XML und JSON �bertragen. Im Fall der Nodes werden
die Werte von Attributen als CDATA �bertragen. Leider sind diese Werte oft nicht XML-konform. Daher werden invalide Teile aus den CDATA-Abschnitten iterativ entfernt, bis die XML-Felder valide sind. Anschlie�end wird
mittels DOM-Parser geparst. Zur Kontrolle dieses Prozesses kann mittels #Ksd.Mediatum.Node.Xml der urspr�ngliche XML-Code abgefragt werden, aus welchem der Node geparst wurde.



# Beispieldaten {#LabelExampleData}

\page Schema-XML Beispiel einer Schema Datei
\includelineno scheme-project-arc.xml

\page appdefinitions-HTML Beispiel einer App Definition Datei
\includelineno appdefinitions-project-arc.html

\page ar-searchbox-node Beispiel XML Node ar:searchbox (collection)
\includelineno services_export_node_1085713.xml

\page BimServer-node Beispiel XML Node BimServer (directory)
\includelineno services_export_node_1219448.xml

\page floorplan-node Beispiel XML Node eines Grundrisses
\includelineno services_export_node_1229104.xml

\page floorplan-node-children Beispiel Children eines XML Nodes
\includelineno services_export_node_1229104_children.xml

\page floorplan-node-parents Beispiel Parents eines XML Nodes
\includelineno services_export_node_1229104_parents.xml

\page upload-node Beispiel eines XML Nodes nach einem Upload
\includelineno services_export_node_1232572.xml

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

