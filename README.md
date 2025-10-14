# IMPORTANT:

i could not get the logging to work without absolute path, so when you clone this project
just go into **NLog.config** file

and here
```xml
  <target xsi:type="File" 
                name="logfile"
                fileName="C:\Users\Kiuadmin\RiderProjects\HomeWorks\finalCOMM\Logging\logger.log"                
                layout="${longdate} | ${level:uppercase=true} | ${message} ${exception:format=tostring}"
                createDirs="true"
                keepFileOpen="true"
                concurrentWrites="true" />
```
change the attribute "fileName" with the absolute path of Logging/logger.log file
by right clicking, then copy path, then copy absolute path

also i removed logging from the console as it was cluttering the interface too much, you can add this back by un-commenting
```xml
        <!-- <logger name="*" minlevel="Info" writeTo="console" /> -->

```
this last line in the same file