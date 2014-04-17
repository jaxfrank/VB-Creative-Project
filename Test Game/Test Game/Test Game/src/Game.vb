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
        If Input.isKeyDown(Keys.Enter) Then
            Me.Exit()
        End If

        If Input.keyPressed(Input.DEBUG_KEY) Then
            Globals.toggleDebugMode()
        End If

        Globals.currentWorld.update(gameTime)
        Globals.player.update(gameTime)

        MyBase.Update(gameTime)
    End Sub

    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        GraphicsDevice.Clear(Color.Black)

        Globals.spriteBatch.Begin()

        Globals.currentWorld.draw(gameTime)

        Globals.player.render(gameTime)

        Globals.spriteBatch.End()

        MyBase.Draw(gameTime)
    End Sub

End Class
