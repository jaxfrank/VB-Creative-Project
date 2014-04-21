Imports Microsoft.Xna.Framework.Input
Imports Test_Game

Public Class EventLoader
    Implements IWorldEventLoaderVB

    Public Sub addEvents(ByRef world As World) Implements IWorldEventLoaderVB.addEvents
        world.addEvent(New WorldEvent(0, 2, AddressOf playerAtLocation, AddressOf teleport))
    End Sub

    Function playerAtLocation(x As Integer, y As Integer) As Boolean
        Return Globals.player.posX = x And Globals.player.posY = y And Test_Game.Input.keyPressed(Keys.E)
    End Function

    Sub teleport(x As Integer, y As Integer)
        'Globals.commandLine.execute("tp 10 0")
        Globals.player.posX = 10
        Globals.player.posY = 0
        Globals.commandLine.execute("say ""Teleported player from (0,2) to (10,0)"" 1")
    End Sub

End Class
