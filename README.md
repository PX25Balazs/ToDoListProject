# ToDo Konsoles Lietotne (C#)

C# konsoles lietotne, kas izstrādāta efektīvai personīgo uzdevumu pārvaldībai ar prioritāšu, termiņu un iknedēļas Kanban dēļa atbalstu.

Galvenās funkcijas
Uzdevumu pārvaldība: Jaunu uzdevumu izveide, vispārējā saraksta skatīšana un dzēšana.
Prioritātes un termiņi: Iespēja norādīt uzdevuma prioritāti (*Augsta*, *Vidēja*, *Zema*) un izpildes termiņu.
Kanban dēlis "Šīs nedēļas saraksts": Uzdevumu pārvietošana uz kārtējo nedēļu ar sadalījumu trīs slejās:
Jāsāk
Daru
Pabeidzu
Lokālā saglabāšana: Datu automātiska serializācija un saglabāšana lokālajā `tasks.json` failā.

## Kā palaist projektu
1. Atveriet projekta mapi programmā **Visual Studio**.
2. Pārliecinieties, ka projekts ir atvērts kā .NET konsoles lietotne.
3. Nospiediet taustiņu **F5** (vai zaļo pogu "Start"), lai saliktu un palaistu programmu.

## Tehnoloģijas
* Valoda: C# (.NET)
* Serializācijas bibliotēka: `System.Text.Json`
