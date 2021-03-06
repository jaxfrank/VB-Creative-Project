﻿Public Class Player

    Public Shared renderLocation As Integer = 10

    'Position is stored as tile coords
    Public posX As Integer
    Public posY As Integer
    Public orientation As Direction

    Private inputTimer As Double = 0
    Private inputUpdated As Boolean = False

    Public Sub New()
        posX = 0
        posY = 0
    End Sub

    Private Sub updateInput(ByVal gameTime As GameTime, ByRef world As World)
        If inputUpdated Then
            inputTimer += gameTime.ElapsedGameTime.Milliseconds / 1000.0
            If inputTimer > 0.05 Then
                inputUpdated = False
                inputTimer = 0
            End If
        Else
            If Input.isKeyDown(Input.UP) Then
                inputUpdated = True
                orientation = Direction.UP
                If Not world.getCollision(posX, posY - 1) Then posY -= 1
            ElseIf Input.isKeyDown(Input.DOWN) Then
                inputUpdated = True
                If Not world.getCollision(posX, posY + 1) Then posY += 1
                orientation = Direction.DOWN
            ElseIf Input.isKeyDown(Input.LEFT) Then
                inputUpdated = True
                If Not world.getCollision(posX - 1, posY) Then posX -= 1
                orientation = Direction.LEFT
            ElseIf Input.isKeyDown(Input.RIGHT) Then
                inputUpdated = True
                If Not world.getCollision(posX + 1, posY) Then posX += 1
                orientation = Direction.RIGHT
            End If
        End If
    End Sub

    Public Sub update(ByVal gameTime As GameTime, ByRef world As World)
        updateInput(gameTime, World)
    End Sub

    Public Sub render(ByVal gameTime As GameTime)
        Dim x As Integer
        Select Case orientation
            Case Direction.DOWN
                x = 0
            Case Direction.LEFT
                x = 1
            Case Direction.RIGHT
                x = 3
            Case Direction.UP
                x = 2
        End Select
        Globals.spriteBatch.Draw(Resources.playerTexture, New Rectangle(CInt(renderLocation * 32.0 * Globals.ZOOM_FACTOR), CInt(renderLocation * 32.0 * Globals.ZOOM_FACTOR), 32 * Globals.ZOOM_FACTOR, 32 * Globals.ZOOM_FACTOR), New Rectangle(x * 48, 0, 48, 48), Color.White)
    End Sub

    Public Sub setPosition(ByVal x As Integer, ByVal y As Integer)
        Me.posX = x
        Me.posY = y
    End Sub

    Public Enum Direction
        DOWN = 0
        LEFT = 1
        UP = 2
        RIGHT = 3
    End Enum

End Class
