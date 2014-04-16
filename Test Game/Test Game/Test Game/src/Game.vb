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
        If Input.isKeyDown(Keys.Enter) Then
            Me.Exit()
        End If

        Globals.player.update(gameTime)

        MyBase.Update(gameTime)
    End Sub

    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        GraphicsDevice.Clear(Color.CornflowerBlue)

        Globals.spriteBatch.Begin()

        For i = Globals.player.posX - 10 To Globals.player.posX + 11
            For j = Globals.player.posY - 10 To Globals.player.posY + 11
                For k = 0 To Globals.currentWorld.getDepth() - 1
                    Dim renderTile As Tile = Globals.currentWorld.getTile(i, j, k)
                    'I hate VB so much
                    'why have so many different ways of saying != and || and &&
                    'also this is a double negative and since VB already has so many key words why don't
                    'they just go ahead and make something(or better yet aThing) a keyword so you can say
                    '
                    'if VB is Something or VB is aThing then 
                    '   commitSuicide()
                    'end if
                    If renderTile IsNot Nothing Then
                        renderTile.render(Globals.spriteBatch, Resources.terrain, i - Globals.player.posX + Player.renderLocation, j - Globals.player.posY + Player.renderLocation)
                    End If
                Next
            Next
        Next

        Globals.player.render(gameTime)
        Globals.spriteBatch.DrawString(Resources.georgia_16, "X: " & Globals.player.posX & " Y: " & Globals.player.posY, New Vector2(0, 0), Color.Black)
        Globals.spriteBatch.End()

        MyBase.Draw(gameTime)
    End Sub

End Class
