# CSharp_3Sem

## Lab4
### Реализация службы DataManager.

1. Программа состоит из двух частей: самого DataManager и ConfigurationProvider.
2. ConfigurationProvider обращается к файлам xml и json и берет их них информаци о строке подключения к базам данных ApplicationInsights и AdventureWorks2019 и путь к SourceDirectory и TargetDirectory.
3. Далее создаётся объект класса DBApplicationInsights, который отвечает за логирование.
Таблица выглядит следующим образом:

![Image alt](https://github.com/wanuchar/CSharp_3Sem/blob/main/Lab4/Screenshots/1.png)


Хранимые процедуры AddAction и ClearAction выглядят следующим образом:

Screenshot

Screenshot

4. Далее все действия предаставляются DataManager. Он подключается к базе AdventureWorks2019 с помощью класса BDAdventure и формирует xml-документ. Метод GetPerson возвращает записи из БД.

Хранимые процедуры:

Screenshot

Screenshot

5. Файлы хранятся в специальной папке Person. Далее они пересылаются в папку SourceDir, где всю работу на себя берет Service1.
