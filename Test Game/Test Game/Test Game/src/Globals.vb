﻿Public Class Globals
    Public Shared XNA_INSTALL_LOCATION As String = System.Environment.GetEnvironmentVariable("XNAGSv4")

    Public Shared content As ContentManager
    Public Shared WithEvents graphicsDeviceManager As GraphicsDeviceManager
    Public Shared WithEvents spriteBatch As SpriteBatch

    Public Shared gamePaused As Boolean
    Public Shared pathFinder As PathFinder
    Public Shared player As Player
    Public Shared commandLine As CommandLine
    Public Shared screenManager As ScreenManager

    Public Shared ZOOM_FACTOR As Integer = 1

    Private Shared debugMode As Boolean = False

    Public Shared Function inDebugMode() As Boolean
        Return debugMode
    End Function

    Public Shared Function toggleDebugMode() As Boolean
        debugMode = Not debugMode
        Return debugMode
    End Function

    Public Shared Sub setDebugMode(ByVal debugMode As Boolean)
        Globals.debugMode = debugMode
    End Sub

    Public Shared Function togglePause() As Boolean
        gamePaused = Not gamePaused
        Return gamePaused
    End Function

End Class
