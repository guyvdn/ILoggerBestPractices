rem npm install -g reveal-md

@ECHO OFF

cls

ECHO.
ECHO Choose your theme
ECHO.
ECHO 1. Black
ECHO 2. White
ECHO 3. Solarized
ECHO 4. Beige
ECHO.

SET THEME=
SET INPUT=
SET /P INPUT=Please select a number:

IF %INPUT%==1 SET THEME=black
IF %INPUT%==2 SET THEME=white
IF %INPUT%==3 SET THEME=solarized
IF %INPUT%==4 SET THEME=beige

reveal-md README.md --theme %THEME% --highlight-theme vs --css theme.css