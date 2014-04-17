Public Class Globals

    Public Shared content As ContentManager
    Public Shared WithEvents graphicsDeviceManager As GraphicsDeviceManager
    Public Shared WithEvents spriteBatch As SpriteBatch

    Public Shared currentWorld As World
    Public Shared player As Player

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

End Class
