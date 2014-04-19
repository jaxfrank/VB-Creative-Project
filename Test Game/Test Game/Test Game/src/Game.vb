Imports System.CodeDom

Public Class Game
    Inherits Microsoft.Xna.Framework.Game

    Private renderOffsetX As Integer = 0
    Private renderOffsetY As Integer = 0

    '1024 x 1024 32 x 32
    Private texture As Texture2D

    Public Sub New()
        Globals.graphicsDeviceManager = New GraphicsDeviceManager(Me)
        Globals.graphicsDeviceManager.PreferredBackBufferHeight = 11 * 64
        Globals.graphicsDeviceManager.PreferredBackBufferWidth = 11 * 64
        Globals.graphicsDeviceManager.ApplyChanges()
        Globals.content = Me.Content
        Globals.content.RootDirectory = "Content"
    End Sub

    Protected Overrides Sub Initialize()
        Randomize()

        MyBase.Initialize()
        MyBase.Window.AllowUserResizing = False
        MyBase.IsMouseVisible = True

        Globals.currentWorld = World.loadFromFile("maps/testWorld")
        Globals.player = New Player()
        Globals.commandLine = New CommandLine()
        Globals.commandLine.addCommand("tp", "iib", AddressOf handleTP)
        Globals.commandLine.addCommand("debug", "b", Sub(arguments As Object()) Globals.setDebugMode(CBool(arguments(0))))
        Globals.commandLine.addCommand("debugT", Sub(arguments As Object()) Globals.toggleDebugMode())
        Globals.commandLine.addCommand("noClip", "b", Sub(arguments As Object()) Globals.currentWorld.setNoClip(CBool(arguments(0))))
        Globals.commandLine.addCommand("noClipT", Sub(arguments As Object()) Globals.currentWorld.toggleNoClip())
        Globals.commandLine.addCommand("showColliders", "b", Sub(arguments As Object()) Globals.currentWorld.setShowColliders(CBool(arguments(0))))
        Globals.commandLine.addCommand("showCollidersT", Sub(arguments As Object()) Globals.currentWorld.toggleShowColliders())
    End Sub

    Protected Overrides Sub LoadContent()
        ' Create a new SpriteBatch, which can be used to draw textures.
        Globals.spriteBatch = New SpriteBatch(GraphicsDevice)

        Resources.load()
    End Sub

    Protected Overrides Sub UnloadContent()
        Resources.unload()
    End Sub

    Protected Overrides Sub Update(ByVal gameTime As GameTime)
        Input.update()
        Util.newFrame()
        Globals.commandLine.update(gameTime)
        If Not Globals.gamePaused Then
            Globals.currentWorld.update(gameTime)
            Globals.player.update(gameTime)
        End If
        MyBase.Update(gameTime)
    End Sub

    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        GraphicsDevice.Clear(Color.Black)

        Globals.spriteBatch.Begin()

        Globals.currentWorld.draw(gameTime)

        Globals.player.render(gameTime)

        Globals.commandLine.draw(gameTime)

        Globals.spriteBatch.End()

        MyBase.Draw(gameTime)
    End Sub

    Public Sub handleTP(parameters As Object())
        Dim x As Integer = CInt(parameters(0))
        Dim y As Integer = CInt(parameters(1))
        Dim ignoreColliders As Boolean = CBool(parameters(2))
        If ignoreColliders Then
            Globals.player.posX = x
            Globals.player.posY = y
        Else
            If Globals.currentWorld.getCollision(x, y) Then
                Globals.commandLine.print("Invalid Teleport location")
            Else
                Globals.player.posX = x
                Globals.player.posY = y
            End If
        End If
    End Sub

End Class
