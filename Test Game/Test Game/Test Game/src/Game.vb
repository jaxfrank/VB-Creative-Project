Imports System.CodeDom

Public Class Game
    Inherits Microsoft.Xna.Framework.Game

    Private renderOffsetX As Integer = 0
    Private renderOffsetY As Integer = 0

    '1024 x 1024 32 x 32
    Private texture As Texture2D
    Private testPath As Path

    Public Sub New()
        Globals.graphicsDeviceManager = New GraphicsDeviceManager(Me)
        Globals.graphicsDeviceManager.PreferredBackBufferHeight = 11 * 64
        Globals.graphicsDeviceManager.PreferredBackBufferWidth = 11 * 64
        Globals.graphicsDeviceManager.ApplyChanges()
        Globals.content = Me.Content
        Globals.content.RootDirectory = "Content"
        Console.WriteLine("This is interesting")
    End Sub

    Protected Overrides Sub Initialize()
        Randomize()

        MyBase.Initialize()
        MyBase.Window.AllowUserResizing = False
        MyBase.IsMouseVisible = True

        Globals.commandLine = New CommandLine()
        Globals.commandLine.addCommand("tp", "ii", AddressOf tp)
        Globals.commandLine.addCommand("debug", "b", Sub(arguments As Object()) Globals.setDebugMode(CBool(arguments(0))))
        Globals.commandLine.addCommand("debugT", Sub(arguments As Object()) Globals.toggleDebugMode())
        Globals.commandLine.addCommand("noClip", "b", Sub(arguments As Object()) DirectCast(Globals.screenManager.getCurrentScreen(), ScreenWorld).getWorld().setNoClip(CBool(arguments(0))))
        Globals.commandLine.addCommand("noClipT", Sub(arguments As Object()) DirectCast(Globals.screenManager.getCurrentScreen(), ScreenWorld).getWorld().toggleNoClip())
        Globals.commandLine.addCommand("showColliders", "b", Sub(arguments As Object()) DirectCast(Globals.screenManager.getCurrentScreen(), ScreenWorld).getWorld().setShowColliders(CBool(arguments(0))))
        Globals.commandLine.addCommand("showCollidersT", Sub(arguments As Object()) DirectCast(Globals.screenManager.getCurrentScreen(), ScreenWorld).getWorld().toggleShowColliders())
        Globals.commandLine.addCommand("showEvents", "b", Sub(arguments As Object()) DirectCast(Globals.screenManager.getCurrentScreen(), ScreenWorld).getWorld().setShowEvents(CBool(arguments(0))))
        Globals.commandLine.addCommand("showEventsT", Sub(arguments As Object()) DirectCast(Globals.screenManager.getCurrentScreen(), ScreenWorld).getWorld().toggleShowEvents())

        Dim currentWorld = World.loadFromFile("maps/testWorld")
        Globals.player = New Player()
        'Globals.pathFinder = New PathFinder()
        'Me.testPath = Globals.pathFinder.findPath(New Point(0, 0), New Point(29, 29))
        'Util.log(CStr(testPath.length()))
        Globals.screenManager = New ScreenManager
        Globals.screenManager.addScreen("TestWorld", New ScreenWorld(currentWorld))
        Globals.screenManager.addScreen("MainMenu", New ScreenMainMenu())
        Globals.screenManager.setCurrentScreen("TestWorld")
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
        Globals.screenManager.update(gameTime)
        MyBase.Update(gameTime)
    End Sub

    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        GraphicsDevice.Clear(Color.Black)

        Globals.spriteBatch.Begin()

        Globals.screenManager.render(gameTime)
        'Globals.currentWorld.draw(gameTime)

        'For Each p As Point In testPath.getNodes()
        '    Dim x As Single = p.X - Globals.player.posX + Player.renderLocation
        '    Dim y As Single = p.Y - Globals.player.posY + Player.renderLocation
        '    Globals.spriteBatch.Draw(Resources.debugTextures, New Rectangle(CInt(x * 32.0 * Globals.ZOOM_FACTOR), CInt(y * 32.0 * Globals.ZOOM_FACTOR), 32 * Globals.ZOOM_FACTOR, 32 * Globals.ZOOM_FACTOR), New Rectangle(64, 0, 32, 32), Color.White)
        'Next

        Globals.spriteBatch.End()

        MyBase.Draw(gameTime)
    End Sub

    Public Sub tp(parameters As Object())
        Dim x As Integer = CInt(parameters(0))
        Dim y As Integer = CInt(parameters(1))
        If DirectCast(Globals.screenManager.getCurrentScreen(), ScreenWorld).getWorld().getCollision(x, y) Then
            Globals.commandLine.print("Invalid Teleport location")
        Else
            Globals.player.posX = x
            Globals.player.posY = y
        End If
    End Sub

End Class
