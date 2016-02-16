Public Class ScreenWorld
    Inherits Screen

    Private world As World

    Public Sub New(world As World)
        Me.world = world
    End Sub

    Public Overrides Sub init()
    End Sub

    Public Overrides Sub render(gameTime As GameTime)
        world.draw(gameTime)
        Globals.player.render(gameTime)
        Globals.commandLine.draw(gameTime)
    End Sub

    Public Overrides Sub update(gameTime As GameTime)
        Globals.commandLine.update(gameTime)
        If Not Globals.gamePaused Then
            world.update(gameTime)
            Globals.player.update(gameTime, world)
        End If
    End Sub

    Public Function getWorld() As World
        Return world
    End Function

End Class
