@echo off
REM ============================================================
REM  TAMAM ULAN ASKER - Tek tikla APK build (Windows)
REM  Gerekenler: Unity Hub + Unity 2019.4.15f1 +
REM  Android Build Support / Android SDK & NDK / OpenJDK modulleri
REM  Unity baska klasordeyse UNITY_EXE satirini duzenle.
REM ============================================================
set "UNITY_EXE=C:\Program Files\Unity\Hub\Editor\2019.4.15f1\Editor\Unity.exe"
set "PROJECT=%~dp0"
if "%PROJECT:~-1%"=="\" set "PROJECT=%PROJECT:~0,-1%"

if not exist "%UNITY_EXE%" (
  echo [HATA] Unity 2019.4.15f1 bulunamadi:
  echo   %UNITY_EXE%
  echo Unity Hub'dan bu surumu kur veya bu dosyadaki UNITY_EXE yolunu duzelt.
  pause
  exit /b 1
)

echo Proje: %PROJECT%
echo APK build basliyor, ilk seferde uzun surebilir...
"%UNITY_EXE%" -quit -batchmode -projectPath "%PROJECT%" -executeMethod BuildAndroid.BuildAPKCommandLine -logFile "%PROJECT%\Builds\build_log.txt"
if errorlevel 1 (
  echo [HATA] Build basarisiz. Log: Builds\build_log.txt
  pause
  exit /b 1
)
echo.
echo APK hazir: Builds\TamamUlanAsker.apk
pause
