Public Class Game
    Inherits Microsoft.Xna.Framework.Game

    Private world As World
    Private inputTimer As Double = 0
    Private inputUpdated As Boolean = False
    Private renderOffsetX As Integer = 0
    Private renderOffsetY As Integer = 0

    '1024 x 1024 32 x 32
    Private texture As Texture2D

    Public Sub New()
        Globals.graphicsDeviceManager = New GraphicsDeviceManager(Me)
        Globals.content = Me.Content
        Globals.content.RootDirectory = "Content"
    End Sub

    Protected Overrides Sub Initialize()
        Randomize()

        MyBase.Initialize()
        MyBase.Window.AllowUserResizing = True
        MyBase.IsMouseVisible = True

        world = world.loadFromFile("maps/testCSV2")
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

        If inputUpdated Then
            inputTimer += gameTime.ElapsedGameTime.Milliseconds / 1000.0
            If inputTimer > 0.05 Then
                inputUpdated = False
                inputTimer = 0
            End If
        Else
            If Input.isKeyDown(Keys.W) Then
                inputUpdated = True
                renderOffsetY += 1
            ElseIf Input.isKeyDown(Keys.S) Then
                inputUpdated = True
                renderOffsetY -= 1
            End If
            
            If Input.isKeyDown(Keys.A) Then
                inputUpdated = True
                renderOffsetX += 1
            ElseIf Input.isKeyDown(Keys.D) Then
                inputUpdated = True
                renderOffsetX -= 1
            End If
        End If
        MyBase.Update(gameTime)
    End Sub

    Protected Overrides Sub Draw(ByVal gameTime As GameTime)
        GraphicsDevice.Clear(Color.CornflowerBlue)

        Globals.spriteBatch.Begin()

        For i = 0 To world.getWidth() - 1
            For j = 0 To world.getHeight() - 1
                For k = 0 To world.getDepth() - 1
                    world.getTile(i, j, k).render(Globals.spriteBatch, Resources.terrain, i + renderOffsetX, j + renderOffsetY)
                Next
            Next
        Next

        Globals.spriteBatch.End()

        MyBase.Draw(gameTime)
    End Sub

End Class
